# Real-Time Shop 2.0.0

An unofficial, opt-in shop clock for the **Windows full release of Probably Stolen**. Version 2.0.0 ports the mod from the demo's IL2CPP/MelonLoader runtime to the full game's **Mono runtime and built-in mod loader**.

The tested game is Steam app **4348910**, installed build **26284217**, visible **VERSION 100-Rev3**, Unity **2021.3.45f2**. The earlier full-release build **26274380** remains recognized. Internal Application.version is still **0.46D**; that string alone does not identify a compatible build. The mod checks exact Assembly-CSharp.dll SHA-256 values and disables itself on unknown binaries. The old demo requires [version 1.0.1](https://github.com/jxmtoma/ProbablyStolenRealTimeMods/releases/tag/v1.0.1).

## Install

Close the game and back up the entire save folder, including native saves and both realtime sidecars. Extract the player ZIP into the full game's directory. The resulting files must be:

    Probably Stolen/Mods/RealTimeShop/manifest.xml
    Probably Stolen/Mods/RealTimeShop/ProbablyStolen.RealTimeShop.dll

Activate **Real-Time Shop** in the game's Mods menu, review its file and reflection permissions, and restart. The package uses the game's supplied Harmony; MelonLoader and generated interop are no longer used. Keep one mod version installed.

Create a new game through scenario, perks and difficulty. On the **Difficulty** screen, explicitly select **Real-time mode** before Start Game. It defaults OFF and resets when you go back. Installing or activating the mod does not convert vanilla saves.

See [README.txt](README.txt) for timing, backup, configuration, uninstall and economy effects, and [installation and acceptance](USER-INSTALL-AND-ACCEPTANCE.md) for validation steps.

## Behavior

Default timing is 30 active minutes from 08:00 to 20:00, 120 seconds of ordinary customer patience and 60 seconds of closing grace. Idle trade panels allow time to pass and can be cancelled on timeout. Confirmations, trade commits, dragging and protected visits retain their safety guards. Native dialogue, pause/options, focus loss and transitions hold time.

The clock continues with closed shutters and on the bedroll screen. At 03:00 it requests native sleep only when the native requirements and safety guards permit it. Late sleep can move the next opening to 11:00; closing remains 20:00. Native overnight expenses, security, rent and outing rules remain in charge.

Consent and timer sidecar schemas are retained. Restore requires both sidecars to match the exact native save, slot, run and day. Loading a historical day file without its own matched sidecars stays vanilla; it cannot borrow the latest slot's timing.

## Build and verify

Use Windows, SDK **10.0.401** from global.json and a licensed full-game installation. Compile against the game's own Mono reference assemblies; no dependency binaries are included or copied into the mod output. The mod targets netstandard2.1; the standalone checks run on .NET 10 and use the game's supplied Newtonsoft.Json.

    ./Build.ps1 -GameDir 'C:/Program Files (x86)/Steam/steamapps/common/Probably Stolen' -Rebuild
    ./Test.ps1 -GameDir 'C:/Program Files (x86)/Steam/steamapps/common/Probably Stolen'
    ./tools/Verify-GameInterop.ps1 -GameDir '<full game directory>' -DllPath './src/RealTimeShop/bin/Release/netstandard2.1/ProbablyStolen.RealTimeShop.dll' -ReportPath './interop-verification.json'

Build.ps1 does not install the mod or launch the game. For separately supplied references, set ManagedDir on the project. The verifier retains its historical filename but now resolves managed Mono members using the game's supplied Cecil library. It does not execute gameplay or validate reflection semantics.

Use [DEPENDENCIES.txt](DEPENDENCIES.txt) and [DEPENDENCY-HASHES.json](DEPENDENCY-HASHES.json) for exact reference provenance. Release compilation is deterministic, with a source path map and no PDB or Git informational-version suffix.

## Validation and release status

The port builds with zero warnings/errors. Managed/security checks and the managed-member audit are complemented by isolated game testing. Exact hashes, completed native cases and remaining limits are recorded in [the compatibility record](releases/2.0.0/COMPATIBILITY.md) and [release notes](RELEASE-NOTES-2.0.0.md). Earlier demo tests apply only to their recorded binaries.

Download the player ZIP and SHA256SUMS.txt from [GitHub v2.0.0](https://github.com/jxmtoma/ProbablyStolenRealTimeMods/releases/tag/v2.0.0) or [Nexus Mods](https://www.nexusmods.com/probablystolen/mods/608?tab=files). Source and build instructions are preserved at [tag v2.0.0](https://github.com/jxmtoma/ProbablyStolenRealTimeMods/tree/v2.0.0). Local source snapshots and reproducibility records remain local; no source ZIP is manually uploaded.

## License, privacy and conflicts

Original mod code is MIT licensed, including commercial reuse. The mod makes no network requests and transmits no player data. Optional bounded local diagnostics default OFF; warnings, startup identification and gameplay notices remain. Timed departures and shorter late-start days can affect income. Other clock, customer, closing, sleep, save or new-game UI mods may conflict.

No game code, assets, generated interfaces, saves or dependency binaries are distributed. Native UI resources are reused at runtime. See [LICENSE-NOTICE.txt](LICENSE-NOTICE.txt), [the developer's modding guide](https://github.com/dragonpan2/probably-stolen-modding/blob/main/ModdingGuide.md), and [RECOVERY.md](RECOVERY.md) for the preserved demo reconstruction lineage.
