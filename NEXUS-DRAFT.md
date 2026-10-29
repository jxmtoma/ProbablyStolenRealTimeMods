# Real-Time Shop 2.0.0 listing draft

Release listing text for the existing [Nexus listing](https://www.nexusmods.com/probablystolen/mods/608).

## Short description

Opt-in real-time days, ordinary customer patience, safe shop closing and late sleep for the Windows full release of Probably Stolen. Uses the game's built-in mod loader.

## Description

Choose **Real-time mode** on the new-game **Difficulty** screen. It defaults off. The calendar shows the shop clock: by default 30 active minutes from 08:00 to 20:00, with 120 seconds of ordinary customer patience. Idle trade panels can cancel at timeout; confirmations, commits, dragging and protected visits retain their guards. Native dialogue, pause/options, focus loss and transitions hold time.

Time continues behind closed shutters and on the bedroll screen. Safe native sleep at 03:00 can delay tomorrow's opening to 11:00; closing remains 20:00. Native overnight expenses, rent, security and outing rules remain in charge. Timing changes can affect trading opportunities and income.

Version 2.0.0 ports the demo mod to Mono and the native mod loader. The tested target is Windows Steam app **4348910**, build **26284217 / VERSION 100-Rev3**. Internal version 0.46D is shared with older builds; compatibility uses the exact managed game assembly hash. Unknown binaries disable the mod. The old demo requires the separately retained 1.0.1 release.

Back up the entire save folder and both realtime sidecars before installation. Extract the player ZIP into the game folder so Mods/RealTimeShop contains manifest.xml and ProbablyStolen.RealTimeShop.dll. Activate Real-Time Shop in the game's Mods menu, review its file/reflection permissions, and restart. Create a disposable save with explicit opt-in first. Existing vanilla saves are not converted.

Use native Mods settings for timing and local diagnostics; restart after changes. To uninstall, close the game and remove the mod folder, or deactivate and restart. Preserve matched native saves and sidecars. Removing the mod does not rewind progress.

Exact validation and limitations: [compatibility record](https://github.com/jxmtoma/ProbablyStolenRealTimeMods/blob/v2.0.0/releases/2.0.0/COMPATIBILITY.md). Historical demo tests apply only to their recorded binaries. No exhaustive campaign, mod-combination or live Steam acceptance is claimed. The mod makes no network requests and transmits no player data. Original mod code is MIT licensed; no game or dependency binaries are included.

## Release assets and source

Download **RealTimeShop-2.0.0.zip** and **SHA256SUMS.txt**. [Source and build instructions at tag v2.0.0](https://github.com/jxmtoma/ProbablyStolenRealTimeMods/tree/v2.0.0). Public uploads contain the player ZIP and checksums only; source snapshots and reproducibility records stay local.
