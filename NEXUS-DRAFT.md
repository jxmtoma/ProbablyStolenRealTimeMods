# Nexus listing draft — review only

**Do not publish yet:** native acceptance and genuine screenshots are pending. Confirm the intended Nexus game/listing and release destination before publication. No Nexus upload or submission has been performed.

Current candidate is **0.2.1-rc.24-recovered**, superseding RC.23. Its missing dismissal/protected-frame warning guards are restored and statically verified. No native gameplay pass is claimed.

## Title

Real-Time Shop - Real-Time Days and Customer Patience

## Short description

An opt-in shop clock for the Probably Stolen Windows Demo: active-time days, customer patience, safe closing, and after-hours sleep with later wake times.

## Description

Choose **Real-time mode** when creating a new game. It defaults off. The calendar shows your shop clock, with a default 30 active minutes from 08:00 to 20:00 and 120 seconds of ordinary customer patience. Trading uses time while unfinished transactions defer unsafe dismissal. Native dialogue, pause menus, focus loss and protected transitions hold timers.

Customer patience speech uses localized text and the game's presentation references. The recovered policy allows six fully readable seconds after typing, bounded by fifteen seconds of wall time, and clears the line when the customer visual leaves. Native rendering and perceived readability still require the pending gameplay check.

Time continues after closing. Sleep through the native bedroll and sleep screen; late sleep moves the next opening as late as 11:00 while closing remains 20:00. Native overnight costs, rent, security and protected interactions retain control. No forced sleep bypasses native disabled controls. Timed departures and shorter late-start days can affect trading opportunities and income.

Version **0.2.1-rc.24-recovered** is a reconstructed source candidate based on the preserved RC.22 mod. It is not the original RC.22 binary or a gameplay-certified stable release. Fresh native installation, clock/dialogue/trade, patience, early/late sleep and save/load/menu acceptance remain pending. Diagnostics default off; useful warnings and errors remain.

## Requirements and build caveats

- Windows Probably Stolen **Steam Demo app 4349200**, public build **25382790**, visible **049-Rev5-L**; internal Unity version **0.46D**, engine 2021.3.45f2.
- **MelonLoader 0.7.3** and its .NET 6 prerequisites, obtained separately. HarmonyX 2.10.2 and Il2CppInterop.Runtime 1.5.1-ci.845 are the installed reference versions.
- The GameAssembly hash is pinned in README.txt and checked at startup. Full game, other builds/platforms/loaders and mod combinations are unverified.
- No game, runtime, loader or generated interop files are included. This is a MelonLoader `Mods` installation, not a native `IMod/manifest.xml` package.

## Installation

1. Close the game. Back up the entire save folder, both `.realtime-shop.json` and `.realtime-choice.json` sidecars, current mod DLL and preferences. Keep matched files together.
2. Obtain MelonLoader 0.7.3 and its prerequisites separately. Start the supported game once so the loader generates interop files.
3. Extract this archive into the game directory so `Mods/ProbablyStolen.RealTimeShop.dll` lands inside `Mods`. Keep only one copy of this mod. Read README.txt and retain your backup.
4. Launch normally and explicitly select Real-time mode for a new game. Existing vanilla saves are not converted. Defaults are created in `[RealTimeShop]` in `UserData/MelonPreferences.cfg`; restart after changing them. Merge the example section rather than replacing other mods' settings.

## Uninstallation and rollback

Close the game and move this DLL out of `Mods`. Keep native saves, both sidecars and backups. For rollback, restore the backed-up DLL, configuration and matched native save plus both sidecars from the same backup. A save changed while the mod is absent may no longer match its former consent/timing sidecars. Do not edit sidecar hashes or overwrite your only backup.

## Changelog

- Restore RC.22 warning suppression on dismissal/protected frames and reference interaction ordering; add focused lifetime regressions and compiled IL dispatch verification.
- Reconstruct RC.22 speech, Unicode typing, visibility cleanup, nested goods tracking and early closure sleep safeguards into reviewable source.
- Disable routine debug output by default; keep pause/resume toasts silent and warnings/errors available.
- Add recovery regression tests, deterministic source/build metadata and current dependency hashes.

## Screenshots — capture required

No usable tracked or accessible historical gameplay images were found; only package icons appeared in the accessible artifact search. No new gameplay screenshots have been captured. Use only genuine game captures from this exact candidate in a disposable test profile:

1. New-game opt-in control at its default off state.
2. Calendar clock during an ordinary active day.
3. Fully typed patience speech with the visible customer (record a timed clip as well).
4. Trade panel with clock progress and intact items/money.
5. Native sleep screen after early closure; next morning calendar.
6. Late sleep and its delayed next morning opening.

Retain originals with candidate DLL hash, exact game build, timestamp and factual captions. No generated gameplay imagery.

## Permissions and support

Original mod source is MIT licensed, including commercial reuse. Dependency notices apply separately; no game rights are granted. The mod makes no network requests. Share version/build and relevant loader errors when reporting issues, removing personal data from logs. Other clock, customer, closing, sleep, save or new-game UI mods may conflict.
