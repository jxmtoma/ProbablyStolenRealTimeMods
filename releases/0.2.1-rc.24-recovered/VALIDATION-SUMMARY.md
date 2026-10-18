# RC.24 validation status

## Passed source/build checks

- Two independent clean offline builds: zero warnings/errors, identical DLL bytes.
- **24,957 ordinary managed assertions**, including 21 focused warning/lifetime interaction checks.
- **165,008 total managed assertions** with the optional RC.22 pure-policy differential fixture; this total includes the ordinary assertions.
- **38** separate security regression assertions, pure managed simulation.
- **512** bounded comparisons of the original RC.22 warning IL branch against the candidate dispatcher, with compiled Plugin.OnLateUpdate input/branch/call wiring verified. No original mod, loader or native method executes.
- **337** resolved static interop references; zero unresolved references or unstripping stubs. No embedded resources, P/Invoke imports, direct System.Net calls, private-path or secret-pattern matches in the candidate audit.
- Player/source archives reproduce byte-for-byte; every player extraction hash matches the staged payload. Content checks exclude game/loader/generated interop/runtime binaries, saves, private logs/fixtures and build outputs.

The warning now requires PatienceWarning present, Dismiss absent and a non-protected frame. Other restored integration details include cleanup order, pause notice-queue behavior, cached closing notice decision and nested/speech evaluation order. The remaining changed methods were reviewed; no further omitted gameplay branch was found. Equivalent expressions, compiler naming and intentional diagnostic reductions remain, so this is not proof of full native equivalence or recovery of the historical source commit.

RC.22's 353 versus RC.24's 337 references differ by sixteen diagnostic-only CustomerBarkView getters. Neither graph has unresolved/stub references. These counts include diagnostic method bodies and recursively referenced wrappers; they are not gameplay coverage scores.

## File installation evidence

On the exact supported installation, the current artifact was copied to the actual Mods path and its SHA-256/ProductVersion checked. A bounded rollback check successfully restored RC.22. At a subsequent explicit user request, RC.24 was installed again and **left installed**; a single active mod DLL was confirmed. Preferences and all personal saves/sidecars were preserved and hash/timestamp checked; original RC.22 and the complete player folder remain in a private local rollback backup.

The game was not launched during these checks. File placement/version verification is **not** loader startup, visual or gameplay validation. No private save/log/backup content is included in this repository.

## Native acceptance remains pending

Native game-window capture/input was unavailable, and the existing isolated tester profile could not be accessed without changing access settings. No access/security change or bypass was attempted. Therefore all of these remain **untested**:

- Game startup, loader initialization and new-game opt-in.
- Actual clock/native dialogue/menu/focus timing.
- Patience speech readability, typing, font/layout/audio and visual departure cleanup.
- In-game dismissal/protected-frame behavior and native trade/item/money integrity.
- Early closure sleep, 01:00/03:00 sleep/wake behavior and unsafe-interaction deferral.
- Native save/load, matched sidecar restoration and menu/exit lifecycle.
- Quiet native UI/logging and genuine gameplay screenshots.

Historical logs/runs on other versions and current managed/IL checks are not native passes for this candidate. Other game builds/full game, long campaigns and mod combinations remain unverified. The Nexus game/listing/destination also needs confirmation before any later publication.

Exact target: Windows Steam Demo **4349200**, public build **25382790**, visible **049-Rev5-L**, internal Application.version **0.46D**, Unity **2021.3.45f2**; MelonLoader **0.7.3**, .NET **6**, HarmonyX **2.10.2**, Il2CppInterop.Runtime **1.5.1-ci.845**. See the root README and dependency manifest for native hashes and requirements.
