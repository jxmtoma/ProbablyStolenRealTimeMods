using System.Diagnostics;
using System.Security.Cryptography;
using HarmonyLib;
using UnityEngine;

namespace RealTimeShop;

public sealed partial class Plugin : IMod
{
    private readonly Harmony harmony = new("probablystolen.realtime_shop");
    private ModLog log = null!;
    internal static Plugin? Current;
    internal ShopClock Clock = new(new Timing());
    internal bool Enabled, Paused;
    internal bool Diagnostics { get; private set; }
    internal readonly SaveOptInSession Session = new();
    internal bool RealTimeActive => Enabled && GameHooks.InShop &&
        Session.Matches(PlayerStore.Instance.saveSlotId, PlayerStore.Instance.runID);
    private Timing configured = new();
    private readonly Stopwatch wall = Stopwatch.StartNew();
    private double lastTick;
    private string suspension = "", nativeHoldExplanation = "";
    private double lastNoticeTick;
    private readonly NativeNoticeQueue notices = new();
    private readonly CustomerDiagnostics diagnostics = new(192);
    private GameObject? watcher;
    private readonly NativePauseAccounting nativePause = new();
    private bool applicationPaused;
    private readonly Dictionary<int, (TMPro.TextMeshProUGUI Label, string Native, string Rendered)> calendarText = new();

    public void Init(ModManifest manifest) => log = new ModLog(manifest);

    public void OnEnable()
    {
        if (Enabled) return;
        Current = this;
        try
        {
            var category = ModConfig.For("realtime_shop");
            Diagnostics = category.Define("Diagnostics", false, "Diagnostics", () => "Debug logging", tooltip: () => "Optional bounded local logging.", requiresRestart: true).Value;
            configured = new Timing(
                category.Define("DayMinutes", 30f, "Timing", () => "Day length (minutes)", min: 1f, max: 1440f, tooltip: () => "Active minutes from 08:00 to 20:00.", requiresRestart: true).Value * 60,
                category.Define("PatienceSeconds", 120f, "Timing", () => "Customer patience (seconds)", min: 1f, max: 86400f, requiresRestart: true).Value,
                category.Define("ClosingGraceSeconds", 60f, "Timing", () => "Closing grace (seconds)", min: 0f, max: 86400f, requiresRestart: true).Value);
            configured.Validate();
            Clock = new ShopClock(configured);
            var gameAssembly = typeof(PlayerStore).Assembly.Location;
            using var stream = File.OpenRead(gameAssembly);
            using var sha = SHA256.Create();
            string hash = SaveEncoding.Hex(sha.ComputeHash(stream));
            if (!GameCompatibility.Supports(Application.platform == RuntimePlatform.WindowsPlayer, hash))
                throw new NotSupportedException("This mod targets the tested Windows full release. Unsupported Assembly-CSharp.dll SHA-256: " + hash);
            GameHooks.Install(harmony);
            watcher = new GameObject("RealTimeShop.Focus");
            UnityEngine.Object.DontDestroyOnLoad(watcher);
            _ = watcher.AddComponent<FocusWatcher>();
            Enabled = true;
            LogChoice("Installed capability; real time defaults OFF. Enable only at new-game creation.");
            Rebase();
            log.Log("Real-Time Shop 2.0.0 loaded with the native mod loader; real time requires new-game opt-in.");
        }
        catch (Exception ex) { Disable(ex); }
    }

    internal void Rebase() { lastTick = wall.Elapsed.TotalSeconds; lastNoticeTick = lastTick; }
    internal void ClearNotices() { GameHooks.ClearGoods(); notices.Clear(); nativeHoldExplanation = ""; ClearBarks(); }
    internal void LogChoice(string text, bool warning=false)
    {
        if (warning) log.Warning("Real-time mode unavailable: " + text);
        else Trace("opt-in " + text);
    }
    internal void ResetSession()
    {
        Session.Reset(); ClearNotices(); Clock.Deactivate(); Paused=false; awaitingMorningReport=false;
        try { RestoreCalendar(); }
        catch(Exception ex) { calendarText.Clear(); LogChoice("Native calendar restoration unavailable: "+ex.GetType().Name, true); }
        nativePause.Observe(new NativePauseInputs(false,true,false,false,false,false,false),0);
        Rebase();
    }
    internal void InitializeNewClock(string key)
    {
        ClearNotices(); awaitingMorningReport=false; Clock=new ShopClock(configured); Clock.StartDay(key); Rebase();
    }
    private bool awaitingMorningReport;
    internal void FreshDay()
    {
        if(!RealTimeActive) return;
        string key = GameHooks.DayKey();
        if (Clock.Timed && Clock.DayKey == key) return;
        ClearNotices();
        awaitingMorningReport = Clock.SleepRequested;
        int wake = Clock.SleepRequested ? Clock.PendingWakeMinute : 480;
        string prior = Clock.DayKey;
        if (Clock.SleepRequested && !IsNextDay(prior, key)) throw new InvalidOperationException("Unexpected sleep day transition");
        Clock = new ShopClock(configured);
        Clock.StartDay(key, wake);
        Trace("sleep-day committed clock " + key + "; wake=" + wake + "; previous=" + prior);
        Rebase();
    }
    private static bool IsNextDay(string before, string after)
    {
        var a=before.Split('/'); var b=after.Split('/');
        return a.Length==3 && b.Length==3 && a[0]==b[0] && a[1]==b[1] &&
            int.TryParse(a[2],out int day) && int.TryParse(b[2],out int next) && next==day+1;
    }

    internal void Notify(string text)
    {
        log.Log(text);
        notices.Publish(text);
    }

    internal bool TimersHeld => RealTimeActive &&
        (Paused || !Application.isFocused || applicationPaused || nativePause.HoldsFrame(Time.frameCount));

    internal void ApplicationPauseChanged(bool value) { applicationPaused = value; Rebase(); }

    private void SyncNativePause()
    {
        bool inShop = GameHooks.InShop;
        var inputs = inShop ? GameHooks.PauseInputs(Application.isFocused, applicationPaused) :
            new NativePauseInputs(false, Application.isFocused, applicationPaused, false, false, false, false);
        bool prior = Paused;
        Paused = nativePause.Observe(inputs, Time.frameCount);
        nativeHoldExplanation = NativeHoldText.Describe(inputs);
        if (prior != Paused) Trace(Paused ?
            "Native pause/focus state holds Real-Time Shop timers; native UI input remains active." :
            "Native pause/focus state released; Real-Time Shop timers resume after this frame.");
    }

    internal void OnUpdate()
    {
        if (!Enabled) return;
        try
        {
            if (!RealTimeActive) { ClearBarks(); Rebase(); return; }
            SyncNativePause();
            UpdateBarks();
            if (!GameHooks.InShop) { ClearNotices(); Rebase(); return; }
            PumpNotices();
        }
        catch (Exception ex) { Disable(ex); }
    }

    internal void OnLateUpdate()
    {
        if (!Enabled) return;
        NewGameChoiceUI.Refresh();
        try
        {
            // Loading is observed while consent is OFF; gameplay requires validated consent and clock.
            if(GameHooks.InShop && GameHooks.FinishPendingLoad()) return;
            if(!RealTimeActive) { Rebase(); return; }
            SyncNativePause();
            UpdateBarks();
            double now = wall.Elapsed.TotalSeconds, delta = now - lastTick;
            lastTick = now;
            if (!GameHooks.InShop) return;
            if (!Clock.Timed) return;
            var store = PlayerStore.Instance;
            // BeginDay runs inside the sleep transition, before MORNING is published.
            // Only the EndDay hook confirms closure; observing CLOSED here must not close a fresh day.
            if (Clock.Phase == Phase.Closed)
            {
                GameHooks.EveningUpdate(delta);
                return;
            }
            if (awaitingMorningReport)
            {
                if (!GameHooks.NativeReportVisible) return;
                awaitingMorningReport = false;
            }
            var frame = GameHooks.Frame(out suspension);
            if (TimersHeld || GameHooks.Busy)
                frame = frame with { Suspended = true };
            string holdReason = TimersHeld ? nativeHoldExplanation : suspension;
            bool announceHold = !TimersHeld && !frame.Suspended && Clock.AdmissionClosed &&
                (frame.Customer == null || frame.Protected ? !ClosingSafety.Safe(frame.ClosingBlockers)
                    : !ClosingSafety.SafeToDismiss(frame.ClosingBlockers));
            if (announceHold)
                holdReason = "Closing waits for a safe point: " + frame.ClosingBlockers + ".";
            notices.SetHold(holdReason, announceHold);
            GameHooks.TraceCustomer(frame, holdReason);
            var priorPhase = Clock.Phase;
            var events = Clock.Advance(delta, frame);
            if (Clock.Phase != priorPhase) Trace("phase " + priorPhase + " -> " + Clock.Phase + "; elapsed=" + Clock.Elapsed.ToString("0.###", System.Globalization.CultureInfo.InvariantCulture));
            if (events.HasFlag(ClockEvent.OneHour)) Notify("One hour until closing (20:00).");
            if (events.HasFlag(ClockEvent.FifteenMinutes)) Notify("15 minutes until closing.");
            if (CustomerBarkDispatch.ShouldWarn(events, frame.Protected)) CustomerWarning();
            if (events.HasFlag(ClockEvent.Dismiss))
            {
                Trace("timeout-dismiss-request customer=" + frame.Customer + "; closingOnly=" + frame.ClosingOnlyDismissal + "; patience=" + Clock.PatienceLeft + "; grace=" + Clock.GraceLeft + "; safe=" + frame.SafeToDismiss);
                var customer = CaptureBarkCustomer();
                bool departed = GameHooks.DismissTimedCustomer();
                if (!departed) Clock.RetryDismissal();
                if (!frame.Protected && customer.Visit != 0 && departed)
                    CustomerDeparture(customer.Visit, customer.Name);
            }
            if (events.HasFlag(ClockEvent.Close))
            {
                Trace("automatic-close-request elapsed=" + Clock.Elapsed + "; canClose=" + frame.CanClose + "; customer=" + frame.Customer);
                StoreShutterButton.Instance.EndAndClose();
                Notify("Closed for the day.");
            }
            if (Clock.Phase == Phase.Closing && !frame.Suspended && !frame.Protected && !TimersHeld)
                GameHooks.HandleClosing();
        }
        catch (Exception ex) { Disable(ex); }
    }

    internal void Disable(Exception ex)
    {
        NewGameChoiceUI.Clear();
        ClearNotices();
        RestoreCalendar();
        Enabled = false;
        Session.Reset();
        Paused = false;
        Clock.Deactivate();
        Unpatch();
        log.Error("Real-Time Shop disabled; vanilla behavior remains active. " + ex);
    }

    public void OnDisable()
    {
        NewGameChoiceUI.Clear();
        ClearNotices();
        RestoreCalendar();
        Enabled = false;
        Session.Reset();
        Paused = false;
        Unpatch();
        if (watcher != null) UnityEngine.Object.Destroy(watcher);
        Current = null;
    }

    internal void Trace(string text) { if (Diagnostics && diagnostics.Take()) log.Log("SHOP-DIAG " + text); }
    private void Unpatch()
    {
        foreach (var method in harmony.GetPatchedMethods().ToArray())
            harmony.Unpatch(method, HarmonyPatchType.All, harmony.Id);
    }
    private string lastTableAssessment = "";
    internal void TraceTableAssessment(TableAssessment assessment)
    {
        string state="tables owned="+assessment.Owned+"; customerUnsold="+assessment.CustomerUnsold+"; unknown="+assessment.Unknown+"; nested="+assessment.Nested+"; safe="+assessment.Safe;
        if(state!=lastTableAssessment){lastTableAssessment=state;Trace(state);}
    }
    internal void TraceObservation(CustomerObservation observation)
    {
        if (!Diagnostics) return;
        string? message = diagnostics.Observe(observation);
        if (message != null) Trace(message);
    }

    private void PumpNotices()
    {
        double now = wall.Elapsed.TotalSeconds, delta = Math.Max(0, now - lastNoticeTick);
        lastNoticeTick = now;
        bool active = !TimersHeld && !GameHooks.Busy && GameHooks.NativeNoticeSafe;
        notices.Pump(active ? delta : 0, active, TryDisplayNativeNotice);
    }

    private bool TryDisplayNativeNotice(string text)
    {
        try
        {
            var ui = StoreUIManager.Instance;
            var rep = ReputationUIManager.Instance;
            if (ui == null || rep == null || ui.dialogBubbleList == null ||
                rep.reputationNotifier == null || rep.reputationNotifierTMP == null || rep.reputationNotifierImage == null)
                return false;
            // This is the game's own shared toast. Do not replace a reputation/game notification.
            if (rep.reputationNotifier.activeInHierarchy && rep.notifierTimer > 0) return false;
            ui.Notify(text, "default");
            bool applied = rep.reputationNotifier.activeInHierarchy && rep.reputationNotifierTMP.text == text;
            Trace("native-notice applied=" + applied + "; nativeDuration=" + rep.notifierTimerDelay + "; text=" + text);
            return applied;
        }
        catch (Exception ex) { Trace("native-notice route failed: " + ex.GetType().Name); return false; }
    }

    internal void Calendar(StoreCalendar calendar)
    {
        if (!RealTimeActive) return;
        var label = calendar.dayTMP;
        if (label == null || !label.gameObject.activeInHierarchy) return;
        if (GameHooks.NativeExitPending) return;
        string native = label.text;
        int id = label.GetInstanceID();
        // Update's native body refreshes the day/rent text before this postfix.
        // Defensive duplicate suppression also handles repeated postfix invocation.
        if (calendarText.TryGetValue(id, out var old) && native == old.Rendered) native = old.Native;
        string clock = Clock.Timed ? Clock.DisplayTime : "Clock starts next morning";
        string rendered = CalendarText.Append(native, clock, label.richText);
        label.text = rendered;
        calendarText[id] = (label, native, rendered);
        // Existing RectTransform, canvas, visibility, font settings and raycast behavior stay native.
    }
    private void RestoreCalendar()
    {
        foreach (var item in calendarText.Values)
            if (item.Label != null && item.Label.text == item.Rendered) item.Label.text = item.Native;
        calendarText.Clear();
    }
}

public sealed class FocusWatcher : MonoBehaviour
{
    private void Update() => Plugin.Current?.OnUpdate();
    private void LateUpdate() => Plugin.Current?.OnLateUpdate();
    public void OnApplicationFocus(bool focused)
    {
        Plugin.Current?.Rebase();
    }
    public void OnApplicationPause(bool paused)
    {
        Plugin.Current?.ApplicationPauseChanged(paused);
    }
}




