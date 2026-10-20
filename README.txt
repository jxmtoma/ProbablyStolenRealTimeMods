Real-Time Shop 1.0.1 - DEMO COMPATIBILITY UPDATE

Targets updated Steam Demo build 26017150 and retains build 25382790.
The SDK build passes 31,133 managed checks and 38 security assertions.
All 339 referenced game/Unity members resolve against the updated interop.
The exact DLL passes focused isolated native opt-in, save/restore, pause,
idle-trade timeout, closing, evening time, 03:00 sleep and morning reload checks.
1.0.1 is installed locally with preferences preserved. No public release has
been created. See RELEASE-NOTES-1.0.1.md and VALIDATION.txt for coverage limits.
Historical 1.0.0 / RC.25 native results apply to their recorded older build.
Read USER-INSTALL-AND-ACCEPTANCE.md for user-run installation and rollback.

Adds an opt-in active-time shop clock to Probably Stolen. Choose Real-time
mode when creating a NEW game; the choice defaults OFF. Existing vanilla
saves are not automatically converted. The calendar shows the clock.

Default day: 30 active minutes, mapping 08:00 to 20:00. Ordinary customer
patience: 120 seconds. Closing grace: 60 seconds. Dialogue, native pause,
focus loss and protected transitions hold time. Idle trading panels let time
advance and are cancelled when an ordinary customer's patience expires or
closing time arrives. Confirmation, trade commits and other unsafe work still
defer departure/closure. Ordinary patrol visits can
leave safely; unresolved rent and other protected interactions block closure.

AFTER CLOSING
Closed shutters do not pause daytime preparation. After closing, evening time
advances at the same rate as the shop day, through midnight, including while
the bedroll screen is open. Use the native bedroll and
sleep screen when ready. Midnight alone does not advance the native day.
At 02:00 a reminder appears. At 03:00 the clock stops advancing and requests
native sleep as soon as interactions, pause, transitions and native sleep
requirements allow it. It does not dismiss protected visitors, return you
from an outing, resolve rent, pay fines or bypass a disabled sleep button.

Sleep gives at least eight hours of rest, with earliest wake at 08:00 and
latest at 11:00. A 01:00 bedtime wakes at 09:00; 03:00 wakes at 11:00.
Closing remains 20:00, so late sleep shortens the next trading day. Bedtime
is rounded down to its displayed minute. The native overnight sequence,
expenses, day counter and save run once. The morning report holds time;
closing it resumes time. There is no added fine or patience penalty.

On nights marked by the game's native outing flag, wake remains 08:00 to
avoid stacking this new delay on the existing native customer penalty.
The native outing logic is untouched. Short Sleeper was dormant in the
previous tested build; availability in the update/full game is unverified.
The mod does not activate or add it. Outing time still advances;
forced sleep waits until you return safely to the shop.

Compatibility is restricted to the Probably Stolen Windows Demo:
Steam app 4349200, public branch, builds 26017150 and 25382790.
Build 26017150: compile/managed/interop and focused native checks pass.
Build 25382790: prior native evidence is recorded for mod 1.0.0 / RC.25.
Visible versions: 050rev5-L (updated build), 049-Rev5-L (previous build).
Internal Unity Application.version remains 0.46D in the updated loader log.
Runtime: Unity 2021.3.45f2, MelonLoader 0.7.3 Open-Beta, .NET 6.
Only these exact native binaries are accepted. Build 26017150:
GameAssembly.dll SHA-256:
A2CAA0CECBF197CCD6B12DDEC7BB9E6FA0BC2CD2A5C52ABEF627AA919E2B9BFD
IL2CPP global-metadata.dat SHA-256:
8067B2F4EE069FDBA6FB2E051ACF7C465C9A275CDDC8D8CA15B313D9FC54B92C
Previous build 25382790 GameAssembly.dll SHA-256:
3BEA17EEEC77ADAB6418918C28A8AA9A06F290582A2972639A48BD7E5A01AB44
Verified IL2CPP global-metadata.dat SHA-256:
EFDCA95772FDCE327B51E0C7E8D320E7055917BE5C669DB47C1F7B36389F1F41
The mod checks the GameAssembly hash and loader version and disables itself
on mismatch. The metadata hash above records provenance; it is not a new guard.
Other game builds, platforms, loader versions and mod combinations are not
certified. Own a normally licensed game copy; this package includes no game
or loader files and does not change Steam ownership or achievement behavior.

BEFORE INSTALLING: close the game and back up the entire save folder,
including every .es3 file and both .realtime-shop.json and
.realtime-choice.json sidecars, plus your current mod DLL and configuration.
Default Windows save location:
%USERPROFILE%\AppData\LocalLow\Questing Goose Studio\Probably Stolen
Keep matched native saves and sidecars together. A mismatched/missing consent
or timer sidecar prevents restoring real-time mode; do not edit their hashes.

INSTALL
1. Obtain MelonLoader 0.7.3 and its .NET 6 prerequisites separately. Start
   the supported game once with the loader to generate native interop files.
2. With the game closed, copy Mods\ProbablyStolen.RealTimeShop.dll into
   the game's Mods folder. Keep only one copy/version of this mod.
3. Start normally. Create a new game and explicitly select Real-time mode.
   Leave it unchecked for normal game timing. No special launch flags needed.

CONFIGURATION
Defaults are created under [RealTimeShop] in UserData\MelonPreferences.cfg.
RealTimeShop.cfg.example is a reference: merge that section if needed;
do not overwrite settings for other mods. Restart after changes. A restored
day retains its saved timing; new days use the configured timing. Valid
ranges: DayMinutes 1–1440, PatienceSeconds 1–86400,
ClosingGraceSeconds 0–86400. This release ships 30/120/60, not test timing.
Use the game's normal pause/menu controls; there is no custom F8 shortcut.
Diagnostics defaults false. Set Diagnostics=true only for bounded local
debug logging. Routine pause/resume UI messages are silent. Useful warnings,
errors, startup identification and actionable gameplay notices remain.

UNINSTALL / ROLLBACK
Close the game. Move this mod DLL out of Mods. Preserve your backups and
sidecars; do not delete native saves. Without the mod, native game behavior
applies. Timer sidecars now use version 2 and read version 1. 0.1.0-rc.1 cannot
read version 2.
To roll back to 0.1.0-rc.1, restore the backed-up DLL, configuration and matched
native save plus BOTH sidecars from before this upgrade. If you
also roll back progress, restore a matched native save plus BOTH sidecars
from the same backup, with the game closed. Restore only your own files.
Re-enabling against a native save changed while the mod was absent may not
restore its previous real-time consent/timing. Avoid overwriting the backup.

VALIDATION / LIMITS
1.0.1 has its own focused native checks on build 26017150. A disposable new
game creates explicit consent and matched sidecars. An older opted-in save
restores correctly. Idle wholesale timeout removes eight unsold customer items,
preserves two owned table items and keeps cash at 545. Closing occurs at 20:00;
03:00 sleep advances one day, wakes at 11:00 and charges native power once
(545 to 540). The overnight autosave reloads with matched consent/timer state.
See releases/1.0.1/NATIVE-TESTS.md for test controls, exact scope and limits.
Historical version 1.0.0 passes the standard SDK build, managed/security checks and exact-DLL
native startup/save/consent/timer restore smoke check. Normal profile files were
unchanged. All 653 normalized mod method bodies match native-tested RC.25,
whose focused timing tests cover idle offer timeout, closed/bedroll time,
pause/resume and automatic sleep. This is limited evidence, not exhaustive native
equivalence. Full trade, speech, protected visitor and recovery coverage remains
outside the focused tests. Long campaigns, every scripted visitor,
inspections, rent outcomes, mod combinations and full-game compatibility are
not certified.
Report mod problems with this version, game/loader version and relevant
MelonLoader log lines; remove personal identifiers before sharing logs.



LICENSE AND PRIVACY
Original mod code is MIT licensed, including commercial reuse. See LICENSE.txt
and LICENSE-NOTICE.txt. The mod makes no network requests and transmits no
player data. Diagnostics stay in local MelonLoader logs.

SAVE / ECONOMY / CONFLICT EFFECTS
Opted-in runs use the native save writer and two hash-bound sidecars. Timed
customer departures, admission cutoff and shorter late-start trading days
can change the number of trades and income. Native overnight expenses and
security outcomes still apply. Keep a matched backup before installing.
Other clock, customer, shop-closing, sleep, save or new-game UI mods may
conflict through shared Harmony patches. No combinations are certified.
This package uses MelonLoader's Mods folder, not the native IMod/manifest.xml
installation described in the game's separate modding API guide.
