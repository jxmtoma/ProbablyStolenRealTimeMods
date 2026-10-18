namespace RealTimeShop;

internal static class SleepHintPolicy
{
    internal static bool Blocks(string hint, int stage, bool manual) => !manual ||
        (hint != "tutorialSleep" && hint != "tutorialNightlyService" &&
         (hint != "tutorialOpenShutterDoor" || stage != 13));
}
