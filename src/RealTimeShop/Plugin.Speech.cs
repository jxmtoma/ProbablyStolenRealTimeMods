using Il2Cpp;
using UnityEngine.Localization.Settings;

namespace RealTimeShop;

public sealed partial class Plugin
{
    private readonly CustomerBarkState bark = new();
    private readonly CustomerBarkView barkView = new();
    private double lastBarkTick;
    private bool barkFailed, barkPresented;

    private string BarkLocale() => LocalizationSettings.SelectedLocale?.Identifier.Code ?? "en";

    private (long Visit, string Name) CaptureBarkCustomer()
    {
        try
        {
            var client = PlayerStore.Instance.currentClientInstance;
            return (client?.Pointer.ToInt64() ?? 0, client?.storeClient?.displayName ?? "");
        }
        catch (Exception ex) { BarkFailure(ex); return (0, ""); }
    }

    private void ClearBarks()
    {
        bark.Reset(); barkPresented = false; lastBarkTick = wall.Elapsed.TotalSeconds;
        try { barkView.Clear(); } catch { /* Presentation cleanup must not interrupt native gameplay. */ }
    }

    private void BarkFailure(Exception ex)
    {
        barkFailed = true; ClearBarks();
        LoggerInstance.Warning("Customer speech unavailable; gameplay continues unchanged: " + ex);
    }

    internal void CustomerVisualGone()
    {
        if (bark.Owner == 0) return;
        try { bark.Hide(); barkPresented = barkView.Render("", "", 0, 0); }
        catch (Exception ex) { BarkFailure(ex); }
    }

    private void UpdateBarks()
    {
        if (barkFailed) return;
        try
        {
            double now = wall.Elapsed.TotalSeconds;
            bark.Observe(Clock.DayKey, PlayerStore.Instance.currentClientInstance?.Pointer.ToInt64() ?? 0,
                bark.Departing ? GameHooks.DepartureBarkUiBlocked() : GameHooks.BarkUiBlocked(),
                Math.Max(0, now - lastBarkTick), barkPresented, GameHooks.BarkSpeakerVisible());
            double delta = Math.Max(0, now - lastBarkTick);
            lastBarkTick = now;
            barkPresented = barkView.Render(bark.Line, bark.Speaker, bark.Owner, delta);
        }
        catch (Exception ex) { BarkFailure(ex); }
    }

    private void CustomerWarning()
    {
        if (barkFailed) return;
        try
        {
            var client = PlayerStore.Instance.currentClientInstance;
            bark.Warn(Clock.DayKey, client == null ? 0 : client.Pointer.ToInt64(),
                client?.storeClient?.displayName ?? "", BarkLocale(), !GameHooks.BarkUiBlocked());
        }
        catch (Exception ex) { BarkFailure(ex); }
    }

    private void CustomerDeparture(long visit, string name)
    {
        if (barkFailed) return;
        try
        {
            if (GameHooks.BarkSpeakerVisible())
                bark.Departure(Clock.DayKey, visit, 0, name, BarkLocale(), true, !GameHooks.DepartureBarkUiBlocked());
        }
        catch (Exception ex) { BarkFailure(ex); }
    }
}
