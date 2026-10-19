# Real-Time Shop 1.0.0

An opt-in real-time shop clock and customer patience system for the supported Probably Stolen Windows Steam Demo.

- Ordinary customers cancel idle offer and bargaining panels and leave when patience expires or closing time arrives.
- Time continues with the shutters closed, before opening, after early closure, and while the bedroll screen is open.
- Confirmations, active trade commits, dragging, unknown item ownership and protected visits still defer departure. Native dialogue, pause menus, focus loss and protected transitions hold timers.
- At 03:00, native sleep runs when its normal requirements are met. Late sleep shifts the next opening as late as 11:00; native overnight costs and events remain active.
- Localized customer patience speech, native presentation references, save opt-in, matched sidecars and diagnostics-off defaults are retained.

Defaults are **30 active minutes per shop day / 120 seconds patience / 60 seconds closing grace**. Real-time mode defaults off and is selected when creating a new game. Existing vanilla saves are not converted.

Supports only Windows Steam Demo app **4349200**, public build **25382790**, visible **049-Rev5-L** (internal Unity version **0.46D**), with **MelonLoader 0.7.3** and its .NET 6 prerequisites. The full game and other builds are unverified. The pinned GameAssembly hash is checked at startup.

Close the game and back up native saves, both sidecars, the old mod DLL and preferences before upgrading. Install only `Mods/ProbablyStolen.RealTimeShop.dll` from the player ZIP; retain one copy of the mod. Read README.txt for configuration and rollback.

The 1.0.0 promotion changes version metadata and startup identification from the tested RC.25 behavior. Build/package hashes and exact validation scope belong in [the 1.0.0 preparation record](releases/1.0.0/PREPARATION.md); [the RC.25 native test record](release-rc25-native-test.txt) remains historical evidence for its exact DLL. Focused tests do not certify every trade, protected event, save/reload path or long campaign.

Player/source archives are prepared locally. No release tag, GitHub Release or Nexus publication is implied by this preparation.
