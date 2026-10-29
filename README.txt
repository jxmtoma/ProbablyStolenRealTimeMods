Real-Time Shop 2.0.0 - FULL RELEASE / NATIVE MOD LOADER

Supports the tested Windows full-game Assembly-CSharp.dll from Steam app
4348910, build 26284217, visible VERSION 100-Rev3. Internal Unity version
0.46D is shared with the demo and is not enough to identify compatibility.
Unknown managed game hashes disable the mod. Demo installations need 1.0.1.
Assembly-CSharp.dll SHA-256:
0FD3DE05D961C8A415758F8F78A8E0A32692B8B45FB34350DF885ED8BD5E8EB0

INSTALL
Close the game and back up the entire native save folder, BOTH realtime
sidecars, current mod files and configuration. Default save folder:
%USERPROFILE%\AppData\LocalLow\Questing Goose Studio\Probably Stolen

Extract the player ZIP into the full game's directory so these exist:
Mods\RealTimeShop\manifest.xml
Mods\RealTimeShop\ProbablyStolen.RealTimeShop.dll

Activate Real-Time Shop in the game's Mods menu, review the declared file
and reflection permissions, then restart. Only this mod's folder is installed;
the game supplies the runtime and Harmony. No MelonLoader installation is
needed. Keep one copy/version. Never copy the old demo DLL into this folder.

NEW-GAME CONSENT
Choose scenario, perks and difficulty. On the Difficulty screen select
Real-time mode before Start Game. It defaults OFF and resets when you go back.
Existing vanilla saves remain vanilla. Valid consent AND timer sidecars must
match the exact native save, slot, run and day to restore real time. A historical
day file without its own matched sidecars cannot reuse the latest slot's timers.
Do not edit hashes or mix native saves and sidecars from different backups.

TIMING
Default day: 30 active minutes from 08:00 to 20:00. Ordinary customer patience:
120 seconds; closing grace: 60 seconds. Idle trade panels allow time to advance
and cancel safely at ordinary customer timeout/closing. Confirmations, trade
commits, dragging, security and protected visits still defer unsafe actions.
Native dialogue, pause/options, focus loss and transitions hold time.

Closed shutters do not stop preparation time. After closing, evening time
continues, including on the native bedroll screen. Midnight alone does not
advance the native day. At 02:00 a reminder appears. At 03:00 the mod requests
native sleep when it is safe and the game's sleep button is enabled. It does
not dismiss protected visitors, return from outings, resolve rent, pay fines
or bypass a disabled native sleep control.

Sleep allows at least eight hours of rest, with earliest wake 08:00 and latest
11:00. A 01:00 bedtime wakes at 09:00; 03:00 wakes at 11:00. Closing stays 20:00,
so late sleep reduces tomorrow's trading time. Native overnight expenses and
the day/save sequence run once. The morning report holds time. On nights
marked by the game's outing flag, wake stays 08:00 to avoid stacking a delay
on the native outing penalty. This mod does not unlock or add native perks.

CONFIGURATION
Use the Settings control for Real-Time Shop in the game's Mods menu. Settings
are saved by the native ModConfig system under the save folder:
ModConfigs\realtime_shop.json
Restart after changing settings. A restored day keeps its saved timing; new
days use the configured timing. Ranges: DayMinutes 1-1440, PatienceSeconds
1-86400, ClosingGraceSeconds 0-86400. Defaults are 30/120/60, Diagnostics OFF.
The old MelonPreferences.cfg is not modified or imported automatically.
RealTimeShop.cfg.example documents the defaults; do not overwrite native JSON
or another mod's settings with that historical-format example.

UNINSTALL / ROLLBACK
Close the game and move Mods\RealTimeShop outside Mods, or deactivate it in
the game's Mods menu and restart. Preserve saves, both sidecars and backups.
Native behavior resumes without the mod; removing it does not rewind progress.
To roll back progress restore the entire matched native save and both sidecars
from one backup with the game closed. A native save changed while the mod was
absent may no longer match its previous consent/timers. Timer schema version 2
still reads version 1. Demo version 1.0.1 requires the demo runtime and is not
a rollback binary for the full game.

VALIDATION / EFFECTS / PRIVACY
Read RELEASE-NOTES-2.0.0.md, VALIDATION.txt and releases/2.0.0/COMPATIBILITY.md
for exact completed checks and limits. Historical demo tests do not establish
full-game acceptance. Timed departures, available trading time and late waking
can change potential income. Native rent/security/expenses remain in charge.
Other clock, customer, closing, sleep, save or setup UI mods may conflict.

The mod makes no network requests and transmits no player data. Diagnostics
are bounded local Unity/player log output. Original mod code is MIT licensed.
No game, loader, runtime or dependency binaries are included. Source and build
instructions: https://github.com/jxmtoma/ProbablyStolenRealTimeMods
Release uploads contain only the player ZIP and checksums, not a manually
uploaded source ZIP. See LICENSE-NOTICE.txt and DEPENDENCIES.txt.
