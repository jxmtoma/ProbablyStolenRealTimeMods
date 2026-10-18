# Real-Time Shop 0.2.1-rc.23-recovered

**Local review candidate; native gameplay acceptance is pending.** This revision reconstructs the missing RC.22 functionality from the preserved MIT-licensed mod binary, then disables routine release diagnostics. It is a new reproducible build, not the original RC.22 binary. See [RECOVERY.md](RECOVERY.md) for provenance and intentional differences.

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

The public checks exercise clock, consent, closing, sleep, speech and security policies with synthetic inputs. They do not run native gameplay. This candidate passed 24,936 ordinary managed checks and 38 security assertions. With the private RC.22 fixture, 164,987 total managed assertions passed. Fresh isolated native acceptance and screenshots are still pending. Historical native results apply only to their original binaries. See [VALIDATION.txt](VALIDATION.txt) and [NEXUS-DRAFT.md](NEXUS-DRAFT.md).

## Conduct and privacy

The mod makes no network requests and transmits no player data. Optional local debug logging defaults OFF (`Diagnostics=false`); useful warnings, errors, startup identification and gameplay notices remain. Routine pause/resume notices are silent. It changes opted-in save behavior, customer timing, available trading time and therefore potential income; native overnight expenses and security outcomes remain in charge. Other mods touching these systems may conflict.

No game source, assets, generated interop assemblies, saves, or loader binaries are distributed. Native UI controls are reused only at runtime. The mod does not activate dormant or unreleased perks. The game's [modding code of conduct](https://github.com/dragonpan2/probably-stolen-modding/blob/main/CodeOfConduct.md) and [native API guide](https://github.com/dragonpan2/probably-stolen-modding/blob/main/ModdingGuide.md) are separate from this loader's installation instructions.

MIT covers the original project. See [LICENSE](LICENSE), [LICENSE-NOTICE.txt](LICENSE-NOTICE.txt), and `licenses/` for dependency notices and applicable license texts.

## Recovered installed build

[`recovered/installed/ProbablyStolen.RealTimeShop.dll`](recovered/installed/ProbablyStolen.RealTimeShop.dll) preserves historical **0.2.1-rc.22**. Its exact source and original commit remain unavailable. This branch reconstructs its functional additions for a separately versioned candidate. The preserved binary is excluded from new player/source archives. See [the recovery record](recovered/InstalledBuild.md).
