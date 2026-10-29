using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace RealTimeShop;

// Full release: scenario -> perks -> difficulty. Consent belongs to the last
// setup screen, defaults OFF on each entry and is consumed once at creation.
internal static class NewGameChoiceUI
{
    private static readonly NewGameSetupSelection selection = new();
    private static GameObject? owned;
    private static Button? button;
    private static Image? mark;
    private static NewGameDifficultyFrame? frame;
    private static bool selected;
    internal static bool Starting => selection.Starting;

    internal static void Clear()
    {
        selection.Clear(); selected = false;
        if (owned != null) { owned.SetActive(false); UnityEngine.Object.Destroy(owned); }
        owned = null; button = null; mark = null; frame = null;
    }

    internal static void Open()
    {
        Clear();
        try
        {
            var data = NewGameData.Instance;
            if (Plugin.Current?.Enabled != true || data == null || data.isLoadFromMainMenu || !NewGameDifficultyUI.IsOpen) return;
            frame = AccessTools.Field(typeof(NewGameDifficultyUI), "frame").GetValue(null) as NewGameDifficultyFrame;
            var source = frame?.endless;
            if (frame == null || source?.root == null || source.button == null || source.label == null || source.mark == null || frame.optionsHost == null)
                throw new InvalidOperationException("Native difficulty controls unavailable");
            long ticket = selection.Begin(frame.Id(), data.Id());
            owned = UnityEngine.Object.Instantiate(source.root.gameObject, frame.optionsHost, false);
            owned.name = "RealTimeModeChoice";
            owned.SetActive(false);
            T Copy<T>(T component) where T : Component
            {
                var path = AnimationPath(component.transform, source.root);
                var target = path.Length == 0 ? owned!.transform : owned!.transform.Find(path);
                if (target == null) throw new InvalidOperationException("Native checkbox hierarchy changed");
                return target.GetComponent<T>() ?? throw new InvalidOperationException("Native checkbox layout changed");
            }
            button = Copy(source.button); mark = Copy(source.mark);
            var label = Copy(source.label);
            button.onClick = new Button.ButtonClickedEvent();
            // Remove dependants before their required localization components.
            foreach (var component in Enumerable.Reverse(owned.GetComponentsInChildren<MonoBehaviour>(true)))
            {
                if (component is Button || component is Image || component is TextMeshProUGUI) continue;
                component.enabled = false; UnityEngine.Object.Destroy(component);
            }
            var group = owned.GetComponent<CanvasGroup>();
            if (group != null) { group.alpha = 1; group.interactable = true; group.blocksRaycasts = true; }
            label!.text = "Real-time mode"; label.richText = false;
            mark.enabled = false; mark.gameObject.SetActive(false); button.interactable = true;
            var rect = (RectTransform)owned.transform;
            rect.anchorMin = rect.anchorMax = new Vector2(0.5f, 0);
            rect.pivot = new Vector2(0.5f, 0);
            rect.sizeDelta = new Vector2(280, 32);
            rect.anchoredPosition = new Vector2(0, -44);
            label.rectTransform.sizeDelta = new Vector2(236, 32);
            button.onClick.AddListener(() =>
            {
                try
                {
                    var current = NewGameData.Instance;
                    if (frame == null || !NewGameDifficultyUI.IsOpen || current == null ||
                        !selection.Select(ticket, frame.Id(), current.Id(), !selected, current.isLoadFromMainMenu))
                    { Clear(); return; }
                    selected = !selected;
                    mark!.gameObject.SetActive(selected); mark.enabled = selected;
                    Plugin.Current?.LogChoice("New-game setup Real-time mode=" + selected);
                }
                catch (Exception ex) { Clear(); Plugin.Current?.LogChoice("Setup choice failed OFF: " + ex.Message, true); }
            });
            owned.SetActive(true);
            Plugin.Current.LogChoice("Difficulty-screen real-time checkbox installed; default OFF.");
        }
        catch (Exception ex) { Clear(); Plugin.Current?.LogChoice("Setup choice unavailable: " + ex.Message, true); }
    }

    private static string AnimationPath(Transform child, Transform root)
    {
        var names = new List<string>();
        while (child != root)
        {
            if (child == null || names.Count > 16) throw new InvalidOperationException("Checkbox hierarchy changed");
            names.Add(child.name); child = child.parent;
        }
        names.Reverse(); return string.Join("/", names);
    }

    internal static void Refresh()
    {
        if (owned != null && !Starting && (!NewGameDifficultyUI.IsOpen || frame == null)) Clear();
    }
    internal static void Back() { if (!Starting) Clear(); }
    internal static void BeforeStart()
    {
        if (NewGameDifficultyUI.Selected == null || MainMenuUIController.Instance == null) return;
        var data = NewGameData.Instance;
        selection.Start(frame.Id(), data.Id(), data == null || data.isLoadFromMainMenu);
        if (owned != null) owned.SetActive(false);
    }
    internal static bool Consume()
    {
        var data = NewGameData.Instance;
        bool enabled = selection.Consume(data.Id(), data == null || data.isLoadFromMainMenu);
        Clear(); return enabled;
    }
}
