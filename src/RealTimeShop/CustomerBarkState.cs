namespace RealTimeShop;

internal static class BarkUiPolicy
{
    // Native animation and unknown table provenance alone do not obscure speech.
    internal static bool Blocks(ClosingBlocker blockers) => ((int)blockers & -133) != 0;
}

internal sealed class CustomerBarkState
{
    internal string Session { get; private set; } = "";
    internal long Visit { get; private set; }
    internal long Owner { get; private set; }
    private string content = "", line = "";
    internal string Speaker { get; private set; } = "";
    private bool obscured;
    internal string Line => obscured ? "" : line;
    internal string Text => obscured ? "" : content;
    internal bool Departing { get; private set; }
    private long warned;
    private double remaining, age;

    internal void Observe(string session, long visit, bool blocked, double seconds,
        bool presented = true, bool speakerVisible = true)
    {
        if (session != Session) { Reset(); Session = session; }
        if (visit != Visit) { warned = 0; Visit = visit; }
        if (!speakerVisible || (Departing ? visit != 0 : visit != Owner)) Hide();
        if (!double.IsFinite(seconds) || seconds < 0) seconds = 0;
        age += seconds;
        if (!blocked && !obscured && presented) remaining = Math.Max(0, remaining - seconds);
        obscured = blocked;
        if (remaining == 0 || age >= 15) Hide();
    }

    internal bool Warn(string session, long visit, string name, string locale, bool safe)
    {
        if (session != Session || visit == 0 || visit != Visit || warned == visit) return false;
        warned = visit;
        Show(visit, name, BarkText.Warning(locale), false);
        obscured = !safe;
        return true;
    }

    internal bool Departure(string session, long visit, long currentVisit, string name,
        string locale, bool confirmed, bool safe)
    {
        if (session != Session || visit == 0 || Visit != visit || currentVisit != 0 || !confirmed) return false;
        Show(visit, name, BarkText.Departure(locale), true);
        obscured = !safe;
        Visit = 0; warned = 0;
        return true;
    }

    private void Show(long owner, string name, string value, bool departing)
    {
        Owner = owner; Departing = departing; remaining = 6; age = 0; obscured = false;
        Speaker = string.IsNullOrWhiteSpace(name) ? "Customer" : name;
        line = value; content = Speaker + ":\n" + value;
    }
    internal void Hide()
    {
        Owner = 0; content = line = Speaker = ""; Departing = false;
        remaining = age = 0; obscured = false;
    }
    internal void Reset() { Hide(); Session = ""; Visit = 0; warned = 0; }
}
