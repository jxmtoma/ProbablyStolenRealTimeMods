# Native checks on demo build 26017150

Tested 2026-10-20 using the exact 1.0.1 DLL recorded in COMPATIBILITY.md. The visible game label is VERSION 050REV5-L (the save list spells it 050rev5-L); Unity still reports Application.version 0.46D.

The game ran in a local copy with a separate Unity company/product save profile. Steam was running and the copy had the documented local-test app ID configuration. A private identity alias allowed the unmodified mod to load. A QA helper verified the disposable profile and blocked Steam init/restart/stat/achievement writes. No helper or game/interop binary is distributed.

Observed checks:

- Loader identifies 1.0.1, installs all hooks and remains enabled without a Real-Time Shop exception.
- On an ordinary specialization setup page, Real-time mode is visible, defaults off and accepts an explicit click. Creating a disposable Rat Wrangler save with the native Skip Intro option and no selected perks enables real time only after the native initial save. Both sidecars match the native save fingerprint. Player defaults are 30 / 120 / 60.
- With the shutters closed, the new save's clock advances from 08:00. Native pause holds the exact elapsed value 19.3742517 for more than 14 wall seconds.
- The prior build's disposable Apprentice Day 2 / 545-credit save restores explicit consent and its saved elapsed value 45. Its saved test timing is 3 active minutes / 20-second patience / 10-second grace. The native morning report holds time.
- Calling the native save writer through the isolated QA helper writes a new native save and both matching sidecars, preserving day, cash and timer identity.
- A scripted DIALOGUE visitor receives no ordinary patience and is not automatically dismissed even after 20:00 and exhausted grace. Dialogue holds time. In a separate run the completed visitor is dismissed through the native method only after native dismissability, zero interaction locks, no suspension and no closing blocker are observed.
- The native queue's ordinary scavCrate seller presents eight unsold soda items in an idle wholesale offer. Its 20-second patience expires while the offer is open. The mod closes the panel and performs native dismissal automatically. Credits remain 545, both owned table items remain, and all eight unsold customer items leave.
- Automatic closing occurs at 20:00 after the native CanEndDay check permits it. Evening time advances with closed shutters and with the native sleep screen open, including across midnight.
- At 03:00 the mod requests native sleep once. One native day transition reaches Day 3 at 11:00; the native power charge occurs once, changing credits from 545 to 540. The next day uses configured player defaults (30 / 120 / 60), so its 11:00 elapsed value is 450 seconds. The morning report holds time, and closing it yields a playable morning with advancing time.
- The native overnight autosave and both sidecars have matching fingerprints. Reload through the native save list restores Day 3 / 540 credits / 11:00 without another charge or day advance. Native Exit Game completes without a mod error; the external watcher did not provide an exit code.

The retained NATIVE-SMOKE.json records the 03:00 sleep, next-morning, reload and final exit results. These are focused integration checks, not exhaustive transaction, speech-readability, scripted/security/rent, campaign, mod-combination or live Steam acceptance.

Test controls: setup, pause, load, dialogue and bedroll interactions used the native UI. The QA helper called native TryCallNextClient, a guarded completed-visit dismissal and SaveGame because the available mouse automation did not reliably complete the game's hold-based customer control. It did not change clocks, money, item ownership, consent, game outcomes or game security requirements directly. Automatic timeout, closing and 03:00 sleep were left to the unmodified mod and native game. Raw logs, test saves, screenshots and helper source/binaries remain private under artifacts and backups.
