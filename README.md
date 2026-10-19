# Real-Time Shop 1.0.0

Ordinary customers cancel idle trading panels when their patience expires or closing time arrives. The clock continues with the shutters closed and on the bedroll screen. Confirmations, trade commits, protected visits and normal pause/dialogue holds remain guarded.

Version 1.0.0 is prepared for release. The standard SDK build passes 31,127 managed checks and 38 security assertions. Its 653 normalized mod method bodies match the native-tested RC.25 build, with startup version text normalized. The exact 1.0.0 binary also passes isolated native startup and matched save/consent/timer restore checks. See [release notes](RELEASE-NOTES-1.0.0.md) and [the preparation record](releases/1.0.0/PREPARATION.md) for hashes, packages and validation scope. No tag or public release has been created.

This version includes the RC.24 reconstruction of missing RC.22 functionality from the preserved MIT-licensed mod binary, with routine diagnostics disabled by default. See [RECOVERY.md](RECOVERY.md) for reconstruction provenance. The timing fixes intentionally differ from the historical RC.22 behavior; [the RC.25 test record](release-rc25-native-test.txt) retains their original native evidence.

RC.24 supersedes RC.23 and restores its missing dismissal/protected-frame warning guards. See [RECOVERY-FIDELITY-REVIEW.md](RECOVERY-FIDELITY-REVIEW.md) for the remaining method review and [USER-INSTALL-AND-ACCEPTANCE.md](USER-INSTALL-AND-ACCEPTANCE.md) for user-run testing and rollback.

The exact historical RC.24 archive/source hashes and validation status are retained in [the release provenance record](releases/0.2.1-rc.24-recovered/PROVENANCE.md). RC.25 does not replace those historical archives. Its focused native timing tests do not constitute exhaustive gameplay acceptance.

An unofficial, opt-in real-time shop clock and after-hours sleep schedule for Probably Stolen's Windows Demo. Original mod code is MIT licensed, including commercial reuse.

Compatibility is intentionally narrow: Steam Demo app **4349200**, public build **25382790**, visible **049-Rev5-L**. Unity's internal `Application.version` is **0.46D** for those same tested binaries. Other builds and the full game are not certified. The mod disables itself when its GameAssembly hash or MelonLoader version checks fail.

Read [README.txt](README.txt) before installation for save backups, timing, economy effects, conflicts, uninstall and coverage limits. Install the compiled mod through **MelonLoader 0.7.3**, using the game's `Mods` folder. This is not a native `IMod`/`manifest.xml` package.

## Build

Use Windows, PowerShell, the .NET SDK 10.0.401 (release build toolchain), and the .NET 6 reference pack. Obtain the game through a licensed installation and MelonLoader 0.7.3 from its official distribution. Let the loader generate the game's interop assemblies. Dependencies and game files are not included here; see [DEPENDENCIES.txt](DEPENDENCIES.txt).

```powershell
./Build.ps1 -GameDir 'D:/SteamLibrary/steamapps/common/Probably Stolen Demo'
./Test.ps1
```

Building reads dependency references and writes only local `bin`/`obj` outputs. It does not install the mod or launch the game. Output: `src/RealTimeShop/bin/Release/net6.0/ProbablyStolen.RealTimeShop.dll`. All runtime references use `Private=false`. To supply references separately, build the project with `-p:LoaderDir=... -p:InteropDir=...`.

Deterministic release compilation uses `ContinuousIntegrationBuild`, no PDB, and a source path map. For byte-for-byte reproduction, use the same SDK/reference packs and dependency versions. Interfaces are dynamically referenced; Il2CppInterop remains separately replaceable and mod source is provided for rebuilding/relinking. Modification and reverse engineering to debug library modifications are not restricted.

## Validation

The public checks exercise clock, consent, closing, sleep, speech and security policies with synthetic inputs. Current results, the exact-binary native smoke check and the relationship to RC.25's focused timing tests are recorded in [the preparation record](releases/1.0.0/PREPARATION.md). Historical fixture/IL reviews apply to their recorded binaries. These checks do not exhaust every transaction, protected event, save/reload failure or long campaign. See [VALIDATION.txt](VALIDATION.txt).

## Conduct and privacy

The mod makes no network requests and transmits no player data. Optional local debug logging defaults OFF (`Diagnostics=false`); useful warnings, errors, startup identification and gameplay notices remain. Routine pause/resume notices are silent. It changes opted-in save behavior, customer timing, available trading time and therefore potential income; native overnight expenses and security outcomes remain in charge. Other mods touching these systems may conflict.

No game source, assets, generated interop assemblies, saves, or loader binaries are distributed. Native UI controls are reused only at runtime. The mod does not activate dormant or unreleased perks. The game's [modding code of conduct](https://github.com/dragonpan2/probably-stolen-modding/blob/main/CodeOfConduct.md) and [native API guide](https://github.com/dragonpan2/probably-stolen-modding/blob/main/ModdingGuide.md) are separate from this loader's installation instructions.

MIT covers the original project. See [LICENSE](LICENSE), [LICENSE-NOTICE.txt](LICENSE-NOTICE.txt), and `licenses/` for dependency notices and applicable license texts.

## Recovered installed build

[`recovered/installed/ProbablyStolen.RealTimeShop.dll`](recovered/installed/ProbablyStolen.RealTimeShop.dll) preserves historical **0.2.1-rc.22**. Its exact source and original commit remain unavailable. This branch reconstructs its functional additions for a separately versioned candidate. The preserved binary is excluded from new player/source archives. See [the recovery record](recovered/InstalledBuild.md).
