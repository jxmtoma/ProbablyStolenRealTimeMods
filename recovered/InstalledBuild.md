# Recovered installed build: Real-Time Shop 0.2.1-rc.22

> Historical recovery record. The source/build statements below describe the initial capture, when the repository still contained 0.2.0 source. The current main tree contains the separately versioned RC.24 reconstruction; its exact package source is recorded under [releases/0.2.1-rc.24-recovered](../releases/0.2.1-rc.24-recovered/PROVENANCE.md). The historical RC.22 binary is retained as an owned-mod reference for the optional policy/IL comparisons, not as the current install target. No game or third-party runtime binary is bundled.

`installed/ProbablyStolen.RealTimeShop.dll` is a byte-for-byte copy of the mod DLL found in the installed game’s `Mods` directory on 2026-10-18. The installed copy was left in place.

| Field | Value |
| --- | --- |
| Assembly identity | `ProbablyStolen.RealTimeShop, Version=0.2.1.0` |
| MelonLoader label | `Real-Time Shop v0.2.1-rc.22` |
| File size | 129,536 bytes |
| SHA-256 | `F58F216FE70BD7D6535561DB99D9F244D9DFC5B16A45A8F16AC698F50062812F` |
| Game | Steam Demo app 4349200, build 25382790 |
| Loader | MelonLoader 0.7.3 Open-Beta |

## Runtime evidence

The game’s `MelonLoader/Latest.log` loaded this assembly and recorded:

- `Real-Time Shop 0.2.1-rc.22 loaded; real time requires new-game opt-in.`
- `Native pause/focus state holds Real-Time Shop timers; native UI input remains active.` (twice in the captured session)

No error or unhandled-exception entries appeared in that log session. This confirms loader initialization and pause/focus hook activity. It does not confirm the opt-in new-game flow or a full gameplay day; the log explicitly says real-time behavior requires a new-game opt-in.

## Source and build status

The checked-in project is Real-Time Shop 0.2.0, whose validated DLL hash is recorded in `VALIDATION.txt`. Its git history and the local workspaces available during recovery contained no matching 0.2.1-rc.22 source or DLL. This recovered binary therefore preserves the installed artifact, but it cannot be rebuilt from the current source tree.

The repository checks passed with .NET SDK 10.0.401: 24,916 managed assertions and 38 security assertions. `Build.ps1` was also run against the installed game, but restore stopped because the offline SDK installation lacks the .NET 6.0.36 reference packs (`Microsoft.NETCore.App.Ref`, `Microsoft.WindowsDesktop.App.Ref`, and `Microsoft.AspNetCore.App.Ref`). No game files or saves were changed.
