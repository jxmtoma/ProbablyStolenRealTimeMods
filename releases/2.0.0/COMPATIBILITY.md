# Real-Time Shop 2.0.0 compatibility record

Prepared **2026-10-28** for the Windows full release, Steam app **4348910**. Steam's queued launch-day update completed as public build **26284217 / VERSION 100-Rev3**, superseding the initially queued target 26281553. The earlier installed full release **26274380 / VERSION 100-Rev1** was tested before that update and remains recognized. Both use Unity **2021.3.45f2**, Mono and the built-in mod loader. Internal Application.version remains **0.46D** and is not a sufficient compatibility identifier.

## Exact artifacts

| Artifact | SHA-256 |
| --- | --- |
| Final 2.0.0 DLL | F911A599F9A88C39E662D16C2684A6CD43D4EF150AB78BBBD16AD6DAF0015A0C |
| Build 26284217 Assembly-CSharp.dll | 0FD3DE05D961C8A415758F8F78A8E0A32692B8B45FB34350DF885ED8BD5E8EB0 |
| Previous full build 26274380 Assembly-CSharp.dll | 52F11980B737F10315723FD6EA09AC49C033A7044978DC1CEFAC0137BA93E556 |
| Earlier launch-build test candidate DLL | 55C04EF3E643BEF08DC81A853F4B988AF8FEA7F92F41F98D24D11FBA17AB129A |

The final candidate accepts the two exact managed game hashes on Windows. It rejects demo IL2CPP hashes and unknown updates. New reference provenance is in [DEPENDENCY-HASHES.json](../../DEPENDENCY-HASHES.json). Earlier demo hashes/evidence remain in their versioned release directories.

## Port and checks

- Native IMod lifecycle, MonoBehaviour update/focus callbacks, built-in ModLog/ModConfig, native Harmony 2.4.2 and Newtonsoft.Json 13.0.2. Production references remain Private=false; no dependency is bundled.
- Managed object identity replaces IL2CPP pointers using monotonic IDs stored in a ConditionalWeakTable. The test suite verifies stable, distinct and concurrent identities.
- New-game consent belongs to the final Difficulty screen. The cloned checkbox removes localization dependants before dependencies and resets on back/reopen. Failed native starts clear the choice without swallowing the game exception.
- Native private fields and new tablet controls are checked explicitly. Customer provenance is recorded only after successful TryAddDirectSellingItemToTable placement. Idle panel cancellation retains the existing ownership/commit/confirmation guards.
- Atomic consent/timer writes retain existing versions and accept the prior System.Text.Json timer schema. A historical load uses its actual file path, preventing latest-slot sidecars from authorizing a different snapshot.
- SDK **10.0.401**, deterministic Release build, **zero warnings/errors**. **31,141 managed assertions and 38 security assertions** pass against the updated game's Newtonsoft.Json. Tests use a workspace-local temporary folder and an offline NuGet configuration.
- [Managed-member audit](INTEROP-VERIFICATION.json): **605/605** referenced members resolve, with no legacy loader/IL2CPP reference. Reflection targets and Harmony installation also succeed during native initialization. The audit alone does not verify behavior.
- Native scanning detects only **files** and **reflection-external**, both declared by the manifest; no refusal. Computed game-type reflection triggers the latter classification. Activation/restart succeeds through the built-in loader.

## Focused game checks and limits

See [NATIVE-TESTS.md](NATIVE-TESTS.md) and [NATIVE-SMOKE.json](NATIVE-SMOKE.json). Testing used a private copy with the exact game Assembly-CSharp.dll and exact candidate DLL. A test-only Unity managed wrapper redirects persistentDataPath into the workspace and forces focus true for hidden-window testing. A private bootstrap blocks Steam init/restart/stat/achievement writes, disables trust-list fetching and records state/invokes native UI actions. These modifications/helpers are excluded from the player and source packages.

The full-release checks exercise default-off and cancelled consent, opted-in creation, matched native saves and sidecars, native pause, active closed-shutter preparation, ordinary idle offers, owned/unsold item separation and unchanged cash, automatic 20:00 closure, evening/bedroll time, 03:00 native sleep, one power charge, the 11:00 next morning and matched overnight reload. A private helper stages an ordinary supplier queue and owned table item. The final evening minute is advanced near 03:00 to exercise the native boundary promptly.

These are controlled integration checks, not a full campaign. Native focus loss is simulated by pure policy tests; the hidden game copy forces focus. Exhaustive warning readability, trade/confirmation/drag/nested-item variants, security/rent/scripted events, outings/perks, all difficulties, historical-day UI loading, mod combinations, Steam Workshop/cloud/achievements and long campaigns are not certified. No normal campaign was loaded for testing. The initial test helper used an unset save slot; normal native loading and the corrected slot-aware helper restore successfully.

## Delivery

Only the player mod folder is installed into the full game. Native campaign saves and old demo preferences are not migrated or overwritten. Activate the mod and review its declared capabilities in the normal Mods menu; create an explicitly opted-in new game. See [installation/rollback](../../USER-INSTALL-AND-ACCEPTANCE.md).

The release assets are the player ZIP and SHA256SUMS.txt. [Tag v2.0.0](https://github.com/jxmtoma/ProbablyStolenRealTimeMods/tree/v2.0.0) preserves source and build instructions. The player ZIP's SOURCE.txt identifies its exact source commit and DLL hash. Local source snapshots, installation records and reproducibility records remain local. Game files, dependency binaries, saves, recovered historical DLLs and private helpers are excluded from the release.
