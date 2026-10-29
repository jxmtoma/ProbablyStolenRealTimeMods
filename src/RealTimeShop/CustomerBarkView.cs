using TMPro;
using Febucci.UI.Examples;
using UnityEngine;
using UnityEngine.UI;

namespace RealTimeShop;

// Reconstructed from the user's RC.22 mod IL. Native resources are referenced
// at runtime; no native component is cloned, driven or distributed.
internal sealed class CustomerBarkView
{
    private GameObject? root;
    private Image? background, nameBackground;
    private TextMeshProUGUI? label, nameLabel;
    private long template, canvasId, renderedOwner;
    private string renderedLine = "", renderedSpeaker = "";
    private readonly CustomerSpeechTyping typing = new();
    private AudioSource? typingAudio;
    private TAnimSoundWriter? nativeSound;
    private double soundWait;
    private int clipIndex;
    internal string Status { get; private set; } = "hidden";

    internal bool Render(string line, string speaker, long owner=0, double seconds=0)
    {
        if (line.Length == 0) { Hide("hidden", owner != 0); return false; }
        var dialog = DialogUIManager.Instance;
        var source = dialog?.clientBubble;
        if (source == null || source.textfield == null || source.backgroundImage == null ||
            dialog!.clientNameTMP == null || dialog.clientNameBubbleImage == null ||
            source.textfield.font == null || dialog.clientNameTMP.font == null)
        { Hide("native-template-unavailable"); return false; }
        var canvas = source.GetComponentInParent<Canvas>(true)?.rootCanvas;
        if (canvas == null || !canvas.isActiveAndEnabled || canvas.renderMode != RenderMode.ScreenSpaceOverlay)
        { Hide("native-canvas-unavailable"); return false; }
        if (!Usable(source.backgroundImage.rectTransform) || !Usable(source.textfield.rectTransform) ||
            !Usable(dialog.clientNameBubbleImage.rectTransform) || !Usable(dialog.clientNameTMP.rectTransform) ||
            !UsableScale(canvas.transform.lossyScale))
        { Hide("native-layout-unavailable"); return false; }
        if (root == null || template != source.Id() || canvasId != canvas.Id())
        {
            Clear(); template = source.Id(); canvasId = canvas.Id();
            root = new GameObject("RealTimeShop.CustomerSpeech");
            _ = root.AddComponent<RectTransform>();
            var ownCanvas = root.AddComponent<Canvas>();
            ownCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            ownCanvas.sortingLayerID = canvas.sortingLayerID;
            ownCanvas.sortingOrder = canvas.sortingOrder + 1;
            ownCanvas.pixelPerfect = canvas.pixelPerfect;
            root.layer = canvas.gameObject.layer;
            background = MakeImage("SpeechBackground"); label = MakeText("SpeechText");
            nameBackground = MakeImage("SpeakerBackground"); nameLabel = MakeText("SpeakerName");
            typingAudio = root.AddComponent<AudioSource>();
            typingAudio.playOnAwake = false; typingAudio.loop = false; typingAudio.spatialBlend = 0;
        }
        if (line != renderedLine || speaker != renderedSpeaker || owner != renderedOwner)
        {
            CopyImage(source.backgroundImage, background!); CopyText(source.textfield, label!);
            CopyImage(dialog.clientNameBubbleImage, nameBackground!); CopyText(dialog.clientNameTMP, nameLabel!);
            label!.text = line; nameLabel!.text = speaker;
            renderedLine = line; renderedSpeaker = speaker; renderedOwner = owner;
            var writer = source.typewriter;
            bool useTyping = writer == null || writer.useTypeWriter;
            typing.Start(line, !useTyping ? 0 : writer?.waitForNormalChars ?? 0.03f,
                !useTyping ? 0 : writer?.waitMiddle ?? 0.15f,
                !useTyping ? 0 : writer?.waitLong ?? 0.3f,
                GameAccess.TypingSpeed(writer),
                writer?.avoidMultiplePunctuactionWait ?? true, writer?.waitForNewLines ?? false);
            nativeSound = !useTyping ? null :
                source.GetComponentInChildren<TAnimSoundWriter>(true) ?? source.GetComponentInParent<TAnimSoundWriter>(true);
            clipIndex = 0; soundWait = 0; label.maxVisibleCharacters = 0;
            root!.transform.SetAsLastSibling();
        }
        root!.SetActive(true);
        soundWait -= Math.Max(0, seconds);
        if (typing.Advance(seconds)) { label!.maxVisibleCharacters = typing.VisibleCharacters; PlayTypingSound(); }
        Status = !root.activeInHierarchy ? "inactive-hierarchy" : !typing.Complete ? "typing-native-style" : "active-native-style";
        return root.activeInHierarchy && typing.Complete;
    }

    private void PlayTypingSound()
    {
        var source = nativeSound?.source;
        var clips = nativeSound == null ? null : GameAccess.Sounds(nativeSound);
        if (typingAudio == null || source == null || clips == null || clips.Length == 0 || soundWait > 0) return;
        typingAudio.volume = source.volume; typingAudio.pitch = source.pitch;
        typingAudio.mute = source.mute; typingAudio.outputAudioMixerGroup = source.outputAudioMixerGroup;
        typingAudio.ignoreListenerPause = source.ignoreListenerPause;
        var clip = clips[clipIndex++ % clips.Length];
        if (clip == null) return;
        if (GameAccess.Interrupt(nativeSound!)) typingAudio.Stop();
        typingAudio.PlayOneShot(clip); soundWait = Math.Max(0, GameAccess.SoundDelay(nativeSound!));
    }
    private static bool UsableScale(Vector3 scale) => float.IsFinite(scale.x) && float.IsFinite(scale.y) &&
        float.IsFinite(scale.z) && Math.Abs(scale.x) > 0.0001f && Math.Abs(scale.y) > 0.0001f && Math.Abs(scale.z) > 0.0001f;
    private static bool Usable(RectTransform rect) => float.IsFinite(rect.rect.width) && float.IsFinite(rect.rect.height) &&
        rect.rect.width > 0 && rect.rect.height > 0 && UsableScale(rect.lossyScale);
    private Image MakeImage(string name)
    {
        var item = new GameObject(name); _ = item.AddComponent<RectTransform>();
        item.transform.SetParent(root!.transform, false);
        var image = item.AddComponent<Image>(); image.raycastTarget = false; return image;
    }
    private TextMeshProUGUI MakeText(string name)
    {
        var item = new GameObject(name); _ = item.AddComponent<RectTransform>();
        item.transform.SetParent(root!.transform, false);
        var text = item.AddComponent<TextMeshProUGUI>(); text.richText = false; text.raycastTarget = false; return text;
    }
    private static void CopyRect(RectTransform source, RectTransform target)
    {
        target.anchorMin = target.anchorMax = new Vector2(0.5f, 0.5f);
        target.pivot = source.pivot; target.sizeDelta = source.rect.size;
        target.position = source.position; target.rotation = source.rotation;
        var parent = target.parent.lossyScale; var scale = source.lossyScale;
        target.localScale = new Vector3(scale.x / parent.x, scale.y / parent.y, scale.z / parent.z);
        target.gameObject.layer = source.gameObject.layer;
    }
    private static void CopyImage(Image source, Image target)
    {
        CopyRect(source.rectTransform, target.rectTransform);
        target.sprite=source.sprite; target.overrideSprite=source.overrideSprite; target.type=source.type;
        target.color=source.color; target.material=source.material; target.preserveAspect=source.preserveAspect;
        target.fillCenter=source.fillCenter; target.pixelsPerUnitMultiplier=source.pixelsPerUnitMultiplier;
        target.fillMethod=source.fillMethod; target.fillAmount=source.fillAmount;
        target.fillClockwise=source.fillClockwise; target.fillOrigin=source.fillOrigin;
    }
    private static void CopyText(TextMeshProUGUI source, TextMeshProUGUI target)
    {
        CopyRect(source.rectTransform, target.rectTransform);
        target.font=source.font; target.fontSize=source.fontSize; target.fontStyle=source.fontStyle;
        target.color=source.color; target.alignment=source.alignment; target.enableWordWrapping=source.enableWordWrapping;
        target.enableAutoSizing=source.enableAutoSizing; target.fontSizeMin=source.fontSizeMin; target.fontSizeMax=source.fontSizeMax;
        target.overflowMode=source.overflowMode; target.margin=source.margin; target.characterSpacing=source.characterSpacing;
        target.wordSpacing=source.wordSpacing; target.lineSpacing=source.lineSpacing; target.paragraphSpacing=source.paragraphSpacing;
    }
    private void Hide(string status, bool preserve=false)
    {
        if (typingAudio != null) typingAudio.Stop();
        if (root != null) root.SetActive(false);
        if (!preserve) { renderedLine=renderedSpeaker=""; renderedOwner=0; }
        Status=status;
    }
    internal void Clear()
    {
        if (root != null) { root.SetActive(false); UnityEngine.Object.Destroy(root); }
        root=null; label=null; nameLabel=null; background=null; nameBackground=null; typingAudio=null; nativeSound=null;
        renderedOwner=0; renderedLine=renderedSpeaker=""; template=canvasId=0; Status="hidden";
    }
}
