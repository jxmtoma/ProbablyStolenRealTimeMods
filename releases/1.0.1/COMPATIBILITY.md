# Real-Time Shop 1.0.1 demo compatibility update

Prepared and installed **2026-10-20** for Windows Steam Demo app **4349200**, public build **26017150**, visible **050rev5-L** (the main menu capitalizes it **050REV5-L**). Unity still reports internal game version **0.46D**, engine **2021.3.45f2**, and MelonLoader is **0.7.3**. The original 1.0.0 mod rejected this update's changed GameAssembly. The exact 1.0.1 DLL now passes focused native tests and normal installed startup.

## Change

Version 1.0.1 accepts the exact new GameAssembly hash alongside the previously tested build **25382790**. Unknown hashes and non-Windows systems still fail closed; the MelonLoader 0.7.3 check remains. Assembly/file/Melon version and startup identification are **1.0.1**. Timing, gameplay hooks, defaults and consent/timer schemas are unchanged.

| Artifact | SHA-256 |
| --- | --- |
| 1.0.1 mod DLL | `E3F56BB1FF367AE2EBE9B97124BC6AB373D14CBF69EE297D154054BB04843250` |
| New build 26017150 GameAssembly.dll | `A2CAA0CECBF197CCD6B12DDEC7BB9E6FA0BC2CD2A5C52ABEF627AA919E2B9BFD` |
| New build global-metadata.dat | `8067B2F4EE069FDBA6FB2E051ACF7C465C9A275CDDC8D8CA15B313D9FC54B92C` |
| Previous build 25382790 GameAssembly.dll | `3BEA17EEEC77ADAB6418918C28A8AA9A06F290582A2972639A48BD7E5A01AB44` |

Metadata hashes record provenance; the runtime native-binary guard checks GameAssembly. Current generated reference hashes are in [DEPENDENCY-HASHES.json](../../DEPENDENCY-HASHES.json). The previous references are preserved in [the 1.0.0 record](../1.0.0/DEPENDENCY-HASHES.json).

## Completed checks

- An initial rebuild of unchanged 1.0.0 against the new interop reproduced its exact prior DLL hash (`27A568AE67C434B392FB674D5D0B5DEB9124258886E266D3D2D465324552ED70`). No source API migration was needed to compile against the regenerated interfaces.
- Standard SDK **10.0.401**, Release/.NET 6 build: **zero warnings and errors**. Existing local SDK and cached .NET 6 reference packages were used with online package sources cleared.
- **31,133 managed assertions and 38 security assertions** pass. Six new checks accept both known Windows binaries and reject missing, unknown, altered and non-Windows inputs. The first test attempt hit a sandbox denial in the host temporary directory; rerunning with a workspace-local temporary directory passed.
- [Interop verification](INTEROP-VERIFICATION.json): all **339** referenced generated game/Unity members resolve; no recognized unstripping/unsupported stub found. This read-only check does not execute native methods or verify their behavior.
- [Method comparison](METHOD-COMPARISON.json): **652 of 653 existing mod method bodies** are unchanged from 1.0.0. The expected initialization change and one added compatibility predicate are recorded explicitly. No existing method is missing. This does not prove unchanged native game behavior.

Reproduce the build/checks with `Build.ps1`, `Test.ps1` and `tools/Verify-GameInterop.ps1` as described in README.md. Set `TEMP`/`TMP` to a writable workspace directory when running inside the restricted sandbox. Both build scripts accept `-RestoreConfigFile` for an existing offline configuration.

## Focused native checks

A private game copy used equal-length Unity company/product substitutions selecting the disposable **Codex Testing Sandbox / ModTest Sandbox** profile. The candidate DLL was unmodified. The normal profile, disposable profile, installed mod and preferences were backed up before testing. Private helpers verified the native profile, blocked Steam init/restart/stat/achievement writes, recorded telemetry and invoked guarded native customer/save actions. Setup, pause, loading, dialogue and bedroll interactions used the native UI. Helpers and native files are excluded from distribution.

Earlier standalone launches exited with code **53**, including an all-mods-disabled control. A later running process used those test arguments on the normal installed executable. With Steam running and a local `steam_appid.txt` configuration in the private copy, isolated startup succeeded. Valve documents that file for local development/testing and explains that Steam relaunches normally select the installed executable; this is consistent with the earlier forwarding behavior, but the precise cause of exit code 53 was not independently established. [Steamworks API overview](https://partner.steamgames.com/doc/sdk/api).

The exact candidate passes explicit new-game opt-in and initial matched sidecars; closed-shutter time and a 14-second native pause hold; restore of the old Day 2 / 545-credit save; native save with matched sidecars; ordinary idle wholesale timeout preserving both owned table items and cash while removing eight unsold items; automatic 20:00 closing; closed/bedroll time across midnight; one 03:00 native sleep; Day 3 at 11:00 with one 5-credit power charge; a playable next morning; and matched overnight autosave reload without another charge/day advance. A scripted DIALOGUE visitor remains protected after cutoff/grace. See [NATIVE-TESTS.md](NATIVE-TESTS.md) and [NATIVE-SMOKE.json](NATIVE-SMOKE.json).

The native Exit Game action was observed completing without mod errors. The external process watcher did not provide an exit code; no code-zero assertion is made. These checks do not exhaust speech readability, transaction variants, security/rent/scripted events, long campaigns, other mods or live Steam gameplay. Historical 1.0.0/RC.25 results remain specific to their older binaries.

## Installation and local artifacts

The exact tested DLL is installed in the normal game's Mods folder. Its prior 1.0.0 DLL and preferences are backed up under `backups/install-1.0.1-20261020-140147/`; full native saves and both sidecars were backed up separately before testing. All **21** normal save/options/sidecar files and the normal mod preferences matched the continuation backup after isolated testing. Normal installed startup identifies 1.0.1, loads one mod, and reports successful initialization. No normal campaign was loaded for testing. See [INSTALLATION-STATE.json](INSTALLATION-STATE.json) and [installation/rollback instructions](../../USER-INSTALL-AND-ACCEPTANCE.md).

The installable DLL is built under `src/RealTimeShop/bin/Release/net6.0/`. Current player/source ZIPs are prepared under `artifacts/release-1.0.1/`, with hashes and a source snapshot manifest. The snapshot includes the uncommitted update and identifies its base commit rather than claiming it is a committed release. Earlier pending-native-test candidate archives remain historical under `artifacts/update-1.0.1/package/`. Game files, loader/interop binaries, saves, private logs, the recovered historical DLL and test helpers are excluded. No release tag or public upload has been created.
