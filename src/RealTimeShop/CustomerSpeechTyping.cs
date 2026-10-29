using System.Globalization;

namespace RealTimeShop;

internal static class CustomerSpeechVisibility
{
    internal static bool BeforeShadow(float blend) => float.IsFinite(blend) && blend >= 0 && blend < 0.5f;
}

// Original mod presentation policy recovered from RC.22, not native game code.
internal sealed class CustomerSpeechTyping
{
    private string text = "";
    private int[] starts = Array.Empty<int>();
    private int visible;
    private double wait, normal, middle, longWait, speed;
    private bool avoidRepeat, newLines;
    internal bool Complete => visible == starts.Length;
    internal int VisibleCharacters { get; private set; }
    internal int VisibleElements => visible;

    internal void Start(string value, double normalWait, double middleWait, double punctuationWait,
        double multiplier, bool avoidMultiplePunctuation, bool waitForNewLines)
    {
        text = value; starts = StringInfo.ParseCombiningCharacters(value);
        visible = VisibleCharacters = 0; wait = 0;
        normal = Safe(normalWait, 0.03); middle = Safe(middleWait, 0.15);
        longWait = Safe(punctuationWait, 0.3);
        speed = double.IsFinite(multiplier) && multiplier > 0 ? multiplier : 1;
        avoidRepeat = avoidMultiplePunctuation; newLines = waitForNewLines;
    }
    private static double Safe(double value, double fallback) => double.IsFinite(value) && value >= 0 ? value : fallback;

    internal bool Advance(double seconds)
    {
        if (!double.IsFinite(seconds) || seconds < 0) seconds = 0;
        wait -= seconds;
        bool changed = false;
        while (visible < starts.Length && wait <= 0)
        {
            int from = starts[visible], to = visible + 1 < starts.Length ? starts[visible + 1] : text.Length;
            for (int index = from; index < to; index++)
            {
                VisibleCharacters++;
                if (char.IsHighSurrogate(text[index]) && index + 1 < to && char.IsLowSurrogate(text[index + 1])) index++;
            }
            char c = text[from]; visible++; changed = true;
            double delay = avoidRepeat && visible < starts.Length && char.IsPunctuation(c) &&
                char.IsPunctuation(text[starts[visible]]) ? normal :
                c is '!' or '.' or '?' or '\u3002' or '\uff01' or '\uff1f' ? longWait :
                c is ',' or ':' or ';' or '\u3001' or '\uff0c' or '\uff1a' or '\uff1b' ? middle :
                c is '\n' or '\r' && !newLines ? 0 : normal;
            wait += delay / speed;
        }
        return changed;
    }
}
