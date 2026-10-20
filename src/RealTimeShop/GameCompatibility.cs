namespace RealTimeShop;

internal static class GameCompatibility
{
    // Exact Windows Steam Demo binaries; unknown updates must still fail closed.
    internal const string PreviousGameAssemblySha256 = "3BEA17EEEC77ADAB6418918C28A8AA9A06F290582A2972639A48BD7E5A01AB44"; // 25382790
    internal const string CurrentGameAssemblySha256 = "A2CAA0CECBF197CCD6B12DDEC7BB9E6FA0BC2CD2A5C52ABEF627AA919E2B9BFD"; // 26017150

    internal static bool Supports(bool windows, string gameAssemblySha256) => windows &&
        (gameAssemblySha256 == PreviousGameAssemblySha256 || gameAssemblySha256 == CurrentGameAssemblySha256);
}
