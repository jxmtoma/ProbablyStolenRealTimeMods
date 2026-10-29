# Full-release installation and acceptance

Version **2.0.0** uses the full game's built-in mod loader and Mono runtime. The old 1.0.1 demo package is incompatible with that runtime. Exact tested game/mod hashes and completed checks are in [the evidence record](releases/2.0.0/COMPATIBILITY.md).

## Install

1. Close the game. Back up the entire native save folder, including all .es3 files and both .realtime-shop.json and .realtime-choice.json sidecars. Keep the current mod folder and configuration separately. Default save folder: %USERPROFILE%\\AppData\\LocalLow\\Questing Goose Studio\\Probably Stolen.
2. Verify the player ZIP against SHA256SUMS.txt, then extract it into the full game's directory. Install only Mods/RealTimeShop/manifest.xml and Mods/RealTimeShop/ProbablyStolen.RealTimeShop.dll. Keep one version. Game/dependency binaries are excluded.
3. In the game's Mods menu, activate **Real-Time Shop**, review file/reflection permissions, and restart. Do not install MelonLoader for this package. Unknown managed game binaries deliberately disable the mod and report their hash.
4. Use a disposable new save first. Choose scenario and perks, then select **Real-time mode** on the Difficulty screen before starting. The checkbox defaults OFF and resets on back navigation. Leaving it OFF must preserve vanilla behavior.
5. Leave settings at **30 / 120 / 60**, diagnostics OFF. Configure through the native Mods settings UI and restart after changes. Native settings live in ModConfigs/realtime_shop.json under the save folder. Previous demo preferences are not automatically imported.

## Acceptance

| Check | Expected result |
| --- | --- |
| New-game consent | Checkbox defaults OFF; back/reopen resets it; explicit selection enables only the created run. |
| Clock and pause | Calendar advances during safe active time, including closed-shutter preparation. Native dialogue, pause/options and focus loss hold timers without catch-up. |
| Trading | Idle offer/bargain panels allow time to pass. Ordinary timeout cancels safely without accepting a deal or changing owned goods/cash. Confirmation, dragging, commits and protected visits defer departure. |
| Closing | Ordinary admission stops at 20:00. Native safe closure still respects outstanding security, scripted and rent work. |
| Evening and sleep | Closed/bedroll time progresses through midnight. At 03:00 safe native sleep advances one day, with one native expense charge and an 11:00 wake on an ordinary night. |
| Save/load | Native save and both sidecars restore the same slot/run/day clock. Historical files without their own matched sidecars stay vanilla. Overnight reload must not duplicate a charge/day advance. |
| Speech and UI | Customer warnings remain readable and clear when the visual leaves. Setup controls do not overlap native controls or change native options. |

Inspect the version-specific evidence before treating any row as verified. Long campaigns, every trade/security/rent/scripted variant, mod combinations and live Steam integration require additional acceptance. Retain relevant errors and test captures privately; remove personal data before sharing logs.

## Uninstall and rollback

Close the game and move Mods/RealTimeShop out of Mods, or deactivate it and restart. Keep native saves and both sidecars. Removing the mod restores native timing without reversing saved progress. To restore progress, first retain the current files separately, then restore the entire matched native save plus both sidecars from one backup. Never mix snapshots or edit hashes. Demo 1.0.1 cannot run on the full game's Mono runtime.
