using System.Reflection;
using HarmonyLib;
using UnityEngine;
using Febucci.UI.Core;
using Febucci.UI.Examples;

namespace RealTimeShop;

// IL2CPP exposed these native fields as public interop properties. The Mono
// release keeps them private. Resolve exact, typed fields once; missing members
// must disable the adapter instead of treating an unknown state as safe.
internal static class GameAccess
{
    private static FieldInfo Field<T, TValue>(string name)
    {
        var field = AccessTools.Field(typeof(T), name);
        if (field == null || field.FieldType != typeof(TValue)) throw new MissingFieldException(typeof(T).FullName, name);
        return field;
    }
    private static readonly FieldInfo typing = Field<StoreUIManager, bool>("isTyping");
    private static readonly FieldInfo exit = Field<EscapeUIManager, bool>("isPendingTransition");
    private static readonly FieldInfo unfinished = Field<DialogUIManager, Dialogue>("displayingUncompletedDialog");
    private static readonly FieldInfo negotiation = Field<NegociationUIManager, NegotiationTabletFrame>("frame");
    private static readonly FieldInfo sleep = Field<EndOfDayUIManager, TabletButton>("endDayButton");
    private static readonly FieldInfo tutorial = Field<TutorialUIManager, int>("tutorialStage");
    private static readonly FieldInfo shadow = Field<StoreClientMono, MaterialPropertyBlock>("mpb");
    private static readonly FieldInfo shadowId = Field<StoreClientMono, int>("SolidBlendId");
    private static readonly FieldInfo clips = Field<TAnimSoundWriter, AudioClip[]>("sounds");
    private static readonly FieldInfo interrupt = Field<TAnimSoundWriter, bool>("interruptPreviousSound");
    private static readonly FieldInfo soundDelay = Field<TAnimSoundWriter, float>("minSoundDelay");
    private static readonly PropertyInfo speed = AccessTools.Property(typeof(TypewriterCore), "typewriterPlayerSpeed")
        ?? throw new MissingMemberException(typeof(TypewriterCore).FullName, "typewriterPlayerSpeed");
    internal static void Validate() { _ = typing; _ = speed; }
    internal static bool IsTyping(StoreUIManager value) => (bool)typing.GetValue(value);
    internal static bool ExitPending(EscapeUIManager value) => (bool)exit.GetValue(value);
    internal static Dialogue? Unfinished(DialogUIManager value) => (Dialogue?)unfinished.GetValue(value);
    internal static GameObject? NegotiationConfirm(NegociationUIManager? value) => value == null ? null : ((NegotiationTabletFrame?)negotiation.GetValue(value))?.confirmRoot;
    internal static TabletButton? SleepButton(EndOfDayUIManager value) => (TabletButton?)sleep.GetValue(value);
    internal static int TutorialStage(TutorialUIManager value) => (int)tutorial.GetValue(value);
    internal static float? Shadow(StoreClientMono value) => ((MaterialPropertyBlock?)shadow.GetValue(value))?.GetFloat((int)shadowId.GetValue(null));
    internal static AudioClip[]? Sounds(TAnimSoundWriter value) => (AudioClip[]?)clips.GetValue(value);
    internal static bool Interrupt(TAnimSoundWriter value) => (bool)interrupt.GetValue(value);
    internal static float SoundDelay(TAnimSoundWriter value) => (float)soundDelay.GetValue(value);
    internal static float TypingSpeed(TypewriterCore? value) => value == null ? 1 : (float)speed.GetValue(value);
}
