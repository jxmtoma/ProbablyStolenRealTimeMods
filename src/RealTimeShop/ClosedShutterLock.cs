namespace RealTimeShop;

// Recovered from the original mod's RC.22 IL; this never changes native locks.
internal sealed class ClosedShutterLock
{
    private string day = "";
    internal void Observe(string key, int before, int after, bool closed) =>
        day = closed && before == 0 && after == 1 ? key : "";
    internal bool Blocks(string key, int value) => value != 0 &&
        (value != 1 || day.Length == 0 || day != key);
    internal void Clear() => day = "";
}
