namespace RealTimeShop;

internal static class GameCompatibility
{
    // Full release now uses Mono. Demo IL2CPP binaries require the old 1.0.1 mod.
    internal const string PreviousGameAssemblySha256 = "52F11980B737F10315723FD6EA09AC49C033A7044978DC1CEFAC0137BA93E556"; // 26274380
    internal const string CurrentGameAssemblySha256 = "0FD3DE05D961C8A415758F8F78A8E0A32692B8B45FB34350DF885ED8BD5E8EB0"; // 26284217

    internal static bool Supports(bool windows, string gameAssemblySha256) => windows &&
        (gameAssemblySha256 == CurrentGameAssemblySha256 || gameAssemblySha256 == PreviousGameAssemblySha256);
}
