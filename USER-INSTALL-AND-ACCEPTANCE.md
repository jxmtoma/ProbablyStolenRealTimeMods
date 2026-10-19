# Installation and acceptance

## Prepared 1.0.0 package

Use `RealTimeShop-1.0.0.zip` for installation; the `-source.zip` archive is for rebuilding. Check its hash against SHA256SUMS.txt and the DLL against the package's SOURCE.txt. The supported demo and prerequisites below still apply. Close the game and back up the entire native save folder with both sidecars, current mod DLL and mod preferences. Install only `Mods/ProbablyStolen.RealTimeShop.dll` and retain one copy. Confirm the loader identifies **1.0.0**, then use a disposable real-time save first. Existing vanilla saves stay vanilla. Defaults remain **30 / 120 / 60**, with diagnostics off.

The standard build, managed checks and exact 1.0.0 native startup/save restore smoke check pass. See [the preparation record](releases/1.0.0/PREPARATION.md) for precise evidence and limits. No release tag or public upload has been made. Roll back with your matched pre-install backup as described below.

RC.25 has been installed locally and passed focused native timing tests. See [the installation and test record](release-rc25-native-test.txt) for its hash, backup, isolated test setup and exact coverage. Your normal saves/options and mod preferences were verified unchanged. The existing RC.24 archive does not contain these fixes; the archive-specific instructions below describe that historical package. There is no RC.25 public ZIP yet.

## Historical RC.24 package instructions

The following instructions and original pending-acceptance checklist describe the archived RC.24 review package, not the installed RC.25 DLL. RC.24 restores the missing RC.22 warning guards and supersedes RC.23. Its exact package/DLL hashes are recorded in PACKAGE-CONTENTS.sha256 and SOURCE.txt. Use the RC.25 test record above for current observed results.

## Confirm before changing anything

- Use `RealTimeShop-0.2.1-rc.24-recovered-review.zip`; verify its ZIP hash against the accompanying SHA256SUMS.txt. The source ZIP is for rebuilding, not installation.
- Exact target: Windows Steam Demo app **4349200**, public build **25382790**, visible **049-Rev5-L**, internal Unity version **0.46D**. Other builds/full game are unverified.
- Required separately: **MelonLoader 0.7.3**, **.NET 6 runtime** and loader-generated interop. These already exist on the inspected PC: .NET 6.0.36, HarmonyX 2.10.2 and Il2CppInterop.Runtime 1.5.1-ci.845. An SDK is not needed to play. If prerequisites differ, stop; this checklist does not install them.
- The actual ZIP has **no enclosing mod folder**: its sole installable binary is `Mods/ProbablyStolen.RealTimeShop.dll` . Other entries are text instructions, hashes and license notices. There are no game/loader/runtime binaries.

## Backup and install

1. Close the game. Keep backups outside the game and `Mods` folders. Copy the entire normal save folder `%USERPROFILE%\AppData\LocalLow\Questing Goose Studio\Probably Stolen`, including native `.es3` files and BOTH `.realtime-shop.json` and `.realtime-choice.json` sidecars. Also back up `Mods\ProbablyStolen.RealTimeShop.dll` and `UserData\MelonPreferences.cfg` from the game directory. Confirm the copies exist before replacing anything. Keep this pre-test backup unchanged.
2. Use an **already verified disposable test profile/save environment** with isolated saves/preferences/progression writes. Back up that profile too. Never load an important campaign for these checks. If isolation is unavailable, stop before launching; a backup alone does not establish isolation.
3. Extract the player ZIP into a temporary folder. Check the extracted DLL:

   ```powershell
   Get-FileHash '<extracted folder>\Mods\ProbablyStolen.RealTimeShop.dll' -Algorithm SHA256
   ```

   Expected: the DLL SHA-256 recorded in the extracted SOURCE.txt and PACKAGE-CONTENTS.sha256.
4. With the game closed, copy **only that DLL** to `C:\Program Files (x86)\Steam\steamapps\common\Probably Stolen Demo\Mods\ProbablyStolen.RealTimeShop.dll`, replacing the already backed-up version. Keep exactly one copy in `Mods`; do not make `Mods\Mods`. Do not overwrite the entire preferences file with `RealTimeShop.cfg.example`.
5. Start normally under the disposable test environment. Confirm the loader identifies **0.2.1-rc.24-recovered**, with no initialization/disable error. New-game **Real-time mode** must default off; create a disposable game with explicit opt-in. Leave the ordinary saves untouched. Keep default timing **30 / 120 / 60**; if editing the mod section, ensure `Diagnostics=false` and restart.

## Acceptance — record each result, do not assume a pass

| Check | Pass condition |
|---|---|
| Clock/dialogue/menu/focus | Calendar advances with active time; actual native dialogue, pause/options and focus loss hold timers, with no catch-up after resume. |
| Trading | Time continues with trade panels open when there is no native dialogue. An ordinary customer cancels an idle offer/bargaining panel and leaves at patience expiry or closing time; confirmations, commits, dragging and protected visits still defer departure. Verify no deal is accepted by cancellation and purchased/owned/nested items and money match the native result. |
| Closed clock | Keep shutters closed before opening and after early closure: the calendar keeps advancing. Leave the bedroll screen open and verify evening time continues toward 03:00. Native pause/dialogue/morning reports still hold time. |
| Patience | On an ordinary ready customer, check the warning near 30 seconds remaining (default 120-second patience). Record typing and actual readability: policy provides six fully readable seconds, capped at fifteen seconds wall age. Speech clears when that customer's visual leaves and never transfers to the next visitor. |
| Warning edge cases | No warning on a protected visit or inappropriate warning on a dismissal frame. RC.24 restores these guards; a normal playthrough alone cannot prove all edge cases. |
| Early closure | Close before 20:00, use the native bedroll and enabled sleep button, advance the native day exactly once and reach a playable next morning. |
| Late/forced sleep | On separate ordinary nights without the native outing flag, 01:00 sleep wakes at 09:00; 03:00 at 11:00. At 03:00, unsafe dialogue/trade/pause/outing/rent must defer sleep; no disabled native button is bypassed. Morning report holds time. |
| Save/load/menu | Save and reload a matched native save plus both sidecars; clock and consent return without duplicate overnight expense/day advance. Check pause, exit confirmation, save to menu, reload and normal exit. |
| Quiet output/screenshots | No recurring debug/hold/resume spam. Useful errors and actionable gameplay notices remain. Capture genuine game-window screenshots/timed speech footage with candidate hash/build and factual captions. |

Stop on any exception, disabled-mod error, incorrect item/money/day/save result, missing speech, or unsafe sleep. Retain relevant loader errors and test recordings privately. No test above has yet passed natively for this DLL.

## Roll back

Close the game. Move the candidate DLL out of `Mods`, then restore the backed-up original DLL and preferences. The original inspected RC.22 DLL SHA-256 is `F58F216FE70BD7D6535561DB99D9F244D9DFC5B16A45A8F16AC698F50062812F`. Keep only one mod copy in `Mods`.

If restoring progress, first preserve the candidate test-data folder separately, then restore the **entire matched pre-test data set for that same profile**, including native saves and BOTH sidecars. Do not mix files from different times/profiles, edit hashes, delete your only backup, or merge mismatched sidecars. Removing the mod alone restores native behavior but does not rewind save progress.
