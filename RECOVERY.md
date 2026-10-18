# RC.22 source recovery

This branch reconstructs missing source from the project's MIT-licensed installed mod. It is a new candidate, not an assertion that the historical RC.22 source or original build commit was found.

Base: `2d8d22dd56ec0d35cba479d269a317aa3e584090` (repository main). Its gameplay source is identical to `6255a33c5fc6a4dc478ec06330a9288035ff0864` / version 0.2.0. The later commits preserved RC.22 documentation and binary only.

Reference RC.22 SHA-256: `F58F216FE70BD7D6535561DB99D9F244D9DFC5B16A45A8F16AC698F50062812F`. Assembly 0.2.1.0; informational/Melon label 0.2.1-rc.22. No original source commit, PDB or SourceLink was embedded. The reference binary is preserved under `recovered/installed/` in Git history and excluded from the new player/source archives.

Recovered behavior includes localized customer speech, six seconds of fully readable lifetime with a fifteen-second wall limit, Unicode-aware typing and native-style sound/layout, customer visual departure cleanup, proven nested customer goods and purchase invalidation, native-ready tutorial sleep exceptions and the same-day shutter closure lock exception. Existing clock, consent, sidecar, pause, protected visitor and native overnight logic is retained.

Only original mod IL was reconstructed. Native assemblies provide separately obtained interface references; game and third-party implementation bodies were not copied. Detailed speech renderer, shutter, nested-goods and sleep-block diagnostic telemetry was omitted deliberately. A separate release change gates remaining diagnostic output. Error and speech failure reporting remains available.

Before the final release metadata/logging change, static comparison found 645 common method signatures, 625 identical normalized IL bodies and 20 different bodies. There were no missing functional top-level types. The differences were reviewed against reference mod IL: equivalent expressions/field initialization, an extracted warning helper call, integration calls, removed diagnostic construction and the compiler-generated tutorial lambda rename. `SleepBlockFacts` is diagnostic only and was intentionally omitted. This comparison is supporting evidence, not proof of runtime equivalence.

The private differential fixture retains only eight pure managed policy types from RC.22. It removes native/loader types and assembly attributes, rejects non-BCL/non-policy member references, then compares seeded state transitions, typing, translations, sleep hints, shutter locks, visibility and UI masks. No game/native code executes. The test fixture is excluded from release archives. Ordinary public checks run without it; set `RC22_PURE_REFERENCE` to the audited fixture path to enable comparisons.

Recovery verification on 2026-10-18: build against the installed supported game succeeded with zero warnings/errors; 164,987 assertions with the differential fixture and 38 security assertions passed. Static audit resolved 337 reachable interop references with no unresolved or unstripping-stub references. No fresh native gameplay or screenshot capture has been performed for this reconstructed source. Historical 0.2.0/RC.22 logs are not validation of this candidate. See VALIDATION.txt for the final candidate's evidence and limits.
