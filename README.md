# Real-Time Shop 1.0.1

Ordinary customers cancel idle trading panels when their patience expires or closing time arrives. The clock continues with the shutters closed and on the bedroll screen. Confirmations, trade commits, protected visits and normal pause/dialogue holds remain guarded.

Version 1.0.1 supports the updated demo, Steam build **26017150**. It builds with zero warnings/errors, passes **31,133 managed checks and 38 security assertions**, and resolves all **339** referenced game/Unity members. The exact DLL passes focused isolated native checks for new-game opt-in, save/consent/timer restore, idle-trade timeout, pause, automatic closing, evening time, 03:00 sleep and next-morning reload. Version 1.0.1 is installed locally with preferences preserved; no public release has been created. See [release notes](RELEASE-NOTES-1.0.1.md) and [the compatibility record](releases/1.0.1/COMPATIBILITY.md) for hashes, evidence and limits.

The previous 1.0.0 build's native tests and packages remain documented in [its preparation record](releases/1.0.0/PREPARATION.md). Those results apply to the older game build and do not establish native compatibility with this update.

This version includes the RC.24 reconstruction of missing RC.22 functionality from the preserved MIT-licensed mod binary, with routine diagnostics disabled by default. See [RECOVERY.md](RECOVERY.md) for reconstruction provenance. The timing fixes intentionally differ from the historical RC.22 behavior; [the RC.25 test record](release-rc25-native-test.txt) retains their original native evidence.

RC.24 supersedes RC.23 and restores its missing dismissal/protected-frame warning guards. See [RECOVERY-FIDELITY-REVIEW.md](RECOVERY-FIDELITY-REVIEW.md) for the remaining method review and [USER-INSTALL-AND-ACCEPTANCE.md](USER-INSTALL-AND-ACCEPTANCE.md) for user-run testing and rollback.

The exact historical RC.24 archive/source hashes and validation status are retained in [the release provenance record](releases/0.2.1-rc.24-recovered/PROVENANCE.md). RC.25 does not replace those historical archives. Its focused native timing tests do not constitute exhaustive gameplay acceptance.

An unofficial, opt-in real-time shop clock and after-hours sleep schedule for Probably Stolen's Windows Demo. Original mod code is MIT licensed, including commercial reuse.

Compatibility is intentionally narrow: Windows Steam Demo app **4349200**, updated public build **26017150**, visible **050rev5-L**, with the previous build **25382790** / **049-Rev5-L** retained. MelonLoader reports internal Unity `Application.version` **0.46D** for both builds. The mod accepts only their exact GameAssembly hashes and MelonLoader 0.7.3; unknown builds still disable the mod. The full game remains unsupported.

Read [README.txt](README.txt) before installation for save backups, timing, economy effects, conflicts, uninstall and coverage limits. Install the compiled mod through **MelonLoader 0.7.3**, using the game's `Mods` folder. This is not a native `IMod`/`manifest.xml` package.

## Build

Use Windows, PowerShell, the .NET SDK 10.0.401 (release build toolchain), and the .NET 6 reference pack. Obtain the game through a licensed installation and MelonLoader 0.7.3 from its official distribution. Let the loader generate the game's interop assemblies. Dependencies and game files are not included here; see [DEPENDENCIES.txt](DEPENDENCIES.txt).

```powershell
./Build.ps1 -GameDir 'D:/SteamLibrary/steamapps/common/Probably Stolen Demo'
./Test.ps1
./tools/Verify-GameInterop.ps1 -GameDir 'D:/SteamLibrary/steamapps/common/Probably Stolen Demo' -DllPath './src/RealTimeShop/bin/Release/net6.0/ProbablyStolen.RealTimeShop.dll' -ReportPath './interop-verification.json'
```

Building reads dependency references and writes only local `bin`/`obj` outputs. It does not install the mod or launch the game. Output: `src/RealTimeShop/bin/Release/net6.0/ProbablyStolen.RealTimeShop.dll`. All runtime references use `Private=false`. To supply references separately, build the project with `-p:LoaderDir=... -p:InteropDir=...`.

Deterministic release compilation uses `ContinuousIntegrationBuild`, no PDB, and a source path map. For byte-for-byte reproduction, use the same SDK/reference packs and dependency versions. Interfaces are dynamically referenced; Il2CppInterop remains separately replaceable and mod source is provided for rebuilding/relinking. Modification and reverse engineering to debug library modifications are not restricted.

## Validation

The public checks exercise compatibility guards, clock, consent, closing, sleep, speech and security policies with synthetic inputs. The interop check resolves generated members without executing game code. Current evidence and focused native coverage are recorded in [the compatibility record](releases/1.0.1/COMPATIBILITY.md). Historical native/fixture/IL reviews apply to their recorded binaries. These checks do not exhaust every transaction, protected event, save/reload failure or long campaign. See [VALIDATION.txt](VALIDATION.txt).

## Conduct and privacy

The mod makes no network requests and transmits no player data. Optional local debug logging defaults OFF (`Diagnostics=false`); useful warnings, errors, startup identification and gameplay notices remain. Routine pause/resume notices are silent. It changes opted-in save behavior, customer timing, available trading time and therefore potential income; native overnight expenses and security outcomes remain in charge. Other mods touching these systems may conflict.

No game source, assets, generated interop assemblies, saves, or loader binaries are distributed. Native UI controls are reused only at runtime. The mod does not activate dormant or unreleased perks. The game's [modding code of conduct](https://github.com/dragonpan2/probably-stolen-modding/blob/main/CodeOfConduct.md) and [native API guide](https://github.com/dragonpan2/probably-stolen-modding/blob/main/ModdingGuide.md) are separate from this loader's installation instructions.

MIT covers the original project. See [LICENSE](LICENSE), [LICENSE-NOTICE.txt](LICENSE-NOTICE.txt), and `licenses/` for dependency notices and applicable license texts.

## Recovered installed build

[`recovered/installed/ProbablyStolen.RealTimeShop.dll`](recovered/installed/ProbablyStolen.RealTimeShop.dll) preserves historical **0.2.1-rc.22**. Its exact source and original commit remain unavailable. This branch reconstructs its functional additions for a separately versioned candidate. The preserved binary is excluded from new player/source archives. See [the recovery record](recovered/InstalledBuild.md).
