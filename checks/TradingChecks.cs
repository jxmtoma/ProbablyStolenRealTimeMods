using RealTimeShop;

internal static class TradingChecks
{
    internal static void Run(Action<bool,string> check)
    {
        foreach (bool closing in new[] { false, true })
        {
            var clock = new ShopClock(new Timing(1800, 120, 60));
            clock.StartDay("trading/run/day1");
            if (closing) clock.Advance(1680, new ShopFrame());
            var trading = new ShopFrame("ordinary", Ready:true, SafeToDismiss:true,
                ClosingBlockers:ClosingBlocker.TradePanel);
            check(!clock.Advance(119, trading).HasFlag(ClockEvent.Dismiss), "idle trade remains available before expiry");
            check(clock.Advance(1, trading).HasFlag(ClockEvent.Dismiss), "customer leaves with idle trade panel open at patience or closing deadline");
            check(!clock.Advance(1, trading).HasFlag(ClockEvent.Dismiss), "trade timeout dismissal is requested once");
            clock.RetryDismissal();
            check(!clock.Advance(1, trading with { ClosingBlockers=ClosingBlocker.TradePanel | ClosingBlocker.TradeCommit }).HasFlag(ClockEvent.Dismiss),
                "cancellation that meets active native work waits safely");
            check(clock.Advance(0, trading).HasFlag(ClockEvent.Dismiss), "deferred native cancellation retries without renewed patience");
            clock.CustomerDeparted();
            check(!clock.Advance(0, new ShopFrame(CanClose:true, ClosingBlockers:ClosingBlocker.TradePanel)).HasFlag(ClockEvent.Close),
                "shop cannot close through leftover trading UI");
            clock.Advance(0, trading with { Customer="next" });
            check(clock.Customer == "next" && clock.PatienceLeft == 120, "next visitor gets full patience after a cancelled offer");
        }

        var protectedClock = new ShopClock(new Timing(60));
        protectedClock.StartDay("trading/run/day1");
        var protectedTrade = new ShopFrame("story", Ready:true, Protected:true, SafeToDismiss:true,
            ClosingBlockers:ClosingBlocker.TradePanel);
        check(!protectedClock.Advance(1000, protectedTrade).HasFlag(ClockEvent.Dismiss), "protected trading visitor remains after ordinary deadline");
        check(!protectedClock.Advance(0, protectedTrade with { ClosingOnlyDismissal=true }).HasFlag(ClockEvent.Dismiss),
            "patrol exception still requires the trading UI resolved");

        var talking = new ShopClock(new Timing());
        talking.StartDay("trading/run/day1");
        var offer = new ShopFrame("ordinary", Ready:true, SafeToDismiss:true, ClosingBlockers:ClosingBlocker.TradePanel);
        talking.Advance(119, offer);
        var before = talking.Capture();
        check(talking.Advance(1000, offer with { Suspended=true }) == ClockEvent.None && talking.Capture() == before,
            "speech or pause over an idle offer still holds all timers");
        check(talking.Advance(1, offer).HasFlag(ClockEvent.Dismiss), "ordinary idle offer expires after speech resumes");
    }
}
