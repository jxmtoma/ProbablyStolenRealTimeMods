# Focused full-release integration checks

Date: **2026-10-28**. Final mod SHA-256: **F911A599F9A88C39E662D16C2684A6CD43D4EF150AB78BBBD16AD6DAF0015A0C**. Current target: Windows Steam **26284217 / VERSION 100-Rev3**, internal Unity version 0.46D. Original full-release checks also ran on **26274380 / VERSION 100-Rev1**, with the prior candidate hash recorded in [COMPATIBILITY.md](COMPATIBILITY.md).

## Isolation and method

A private game copy retains the exact game Assembly-CSharp.dll and candidate DLL. Only the test Unity wrapper/initialization metadata and private helper are changed. The Unity persistent save path points to a workspace-local disposable profile; focus is forced true so hidden-window execution can progress. The helper blocks Steam init/restart/stats/achievement writes and trust-list fetching, records actual game/mod state and invokes native setup, save, load, trading and sleep actions. No game, helper, save or raw log is distributed.

The game's loader discovers and scans the native mod folder. Its detected files/reflection-external capabilities match the manifest, and activation/restart initializes the mod successfully. Source API inspection and runtime field validation complement the managed-member audit.

## Observed cases

| Case | Result |
| --- | --- |
| Scenario/perks/difficulty setup | Native screens open in order. Real-time mode defaults OFF. Selecting, going back and reopening resets OFF. Selecting and starting creates only that opted-in run. |
| Native consent and save | Native slot/run/day are allocated before both hash-bound sidecars commit. Native save and reload restore active timing rather than creating consent for vanilla data. |
| Pause | Visible native pause menu holds elapsed time exactly for the recorded interval. Resume progresses without catch-up. |
| Closed preparation | Safe time progresses before opening with shutters closed. |
| Idle trade | A private ordinary scavenger food supplier populates five unsold items on the hotfix (six in the earlier full-build sample) through the new successful-placement API. The native idle offer remains open while ordinary patience counts down. |
| Ownership/cash | The native timeout closes the offer and removes the five hotfix unsold items (six in the earlier full-build sample). Owned newspaper and bottled water remain, with unchanged cash. The original full-build sample preserves 545 credits after the first overnight charge; the hotfix sample starts at 550. |
| Closing/evening | Automatic 20:00 closure is observed with an empty ordinary queue. Closed/bedroll time continues through midnight. |
| Sleep/morning | Near 03:00, safe native sleep advances one day, charges native power once (550 to 545) and creates an 11:00 clock. The native nightly report holds time; closing it allows progression. |
| Overnight reload | Matched native autosave/consent/timer reload stays on the same next day, at the saved morning time and 545 credits. No second power charge/day advance occurs. |

The helper stages a controlled ordinary queue and clears that fixture's shutter lock before arrival, and adds an owned bottled-water item using the native placement API. It does not accept an offer. Native save/load, trade cancellation, closure and sleep execute game code. The final evening minute is moved to 02:59; the sleep boundary then runs normally. This fixture does not establish normal scripted-queue acceptance.

The original full-build capture stayed open long enough to observe automatic closing and evening time naturally. An extra ordinary visit on the following morning establishes removal before closing (11:01 before, 12:13 after), with two owned items, six removed unsold items and 545 unchanged credits. Exact final-hotfix measurements are summarized in [NATIVE-SMOKE.json](NATIVE-SMOKE.json).

## Limits

These checks cover focused paths. They do not exhaust campaigns, all difficulties, security/rent/scripted queues, outings/perks, confirmation/drag/commit combinations, nested goods, speech readability, focus-loss behavior in the native window, historical-day UI loading or other mods. Steam/cloud/workshop/achievements remain untested because Steam writes are blocked. Initial helper mistakes are excluded from passing evidence; the corrected native load uses the allocated save slot and actual load file.
