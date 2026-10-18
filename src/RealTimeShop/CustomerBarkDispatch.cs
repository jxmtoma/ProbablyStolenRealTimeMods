namespace RealTimeShop;

internal static class CustomerBarkDispatch
{
    // RC.22's warning branch: never issue a warning on a dismissal or protected frame.
    internal static bool ShouldWarn(ClockEvent events, bool protectedFrame) =>
        events.HasFlag(ClockEvent.PatienceWarning) && !events.HasFlag(ClockEvent.Dismiss) && !protectedFrame;
}
