# Real-Time Shop 1.0.1 demo compatibility update

Adds the exact Windows Steam Demo **26017150** GameAssembly hash to the supported builds. The previous **25382790** hash remains accepted. Unknown game binaries, non-Windows platforms and unsupported MelonLoader versions still disable the mod. Timing, save/consent formats, defaults and gameplay hooks are unchanged.

Rebuilt against the updated demo's loader-generated interop using SDK 10.0.401 and the existing .NET 6 reference packages. The build has zero warnings/errors, **31,133 managed checks and 38 security assertions** pass, and all **339** referenced game/Unity members resolve. Six new assertions exercise the compatibility boundary.

The exact DLL passes **focused native checks on build 26017150 / 050rev5-L**: explicit new-game opt-in, matched old-save restore and native save, pause, idle wholesale timeout, automatic 20:00 closing, evening/bedroll time, one 03:00 sleep, an 11:00 next morning and overnight autosave reload. The timeout preserves two owned table items and 545 credits while removing eight unsold customer items. Sleep charges native power once, leaving 540 credits on Day 3. See [the test record](releases/1.0.1/NATIVE-TESTS.md) and [compatibility evidence](releases/1.0.1/COMPATIBILITY.md).

The verified DLL is installed locally with a rollback backup. Normal campaign saves and mod preferences are preserved. Tests used a disposable profile, private helpers and blocked Steam writes; live Steam gameplay, exhaustive transaction/speech coverage, scripted/security/rent variants, campaigns and mod combinations remain unverified. Before installing on another setup, preserve a matched backup and use a disposable save first. See [installation and acceptance](USER-INSTALL-AND-ACCEPTANCE.md).

Published as [GitHub v1.0.1](https://github.com/jxmtoma/ProbablyStolenRealTimeMods/releases/tag/v1.0.1), with the player mod also available on [Nexus Mods](https://www.nexusmods.com/probablystolen/mods/608?tab=files).

The manually uploaded `RealTimeShop-1.0.1-source.zip` asset was deleted from the GitHub release. The quarantined Nexus source ZIP was archived, removing it from active downloads. The player mod ZIP is unchanged, and `SHA256SUMS.txt` now lists only `RealTimeShop-1.0.1.zip`.

Future release uploads must include only the player mod ZIP and checksums. Do not manually upload a separate source ZIP to either site; use [the tagged source](https://github.com/jxmtoma/ProbablyStolenRealTimeMods/tree/v1.0.1) and its build instructions. GitHub's automatically generated source archives remain available. Locally prepared source ZIPs are reproducibility artifacts only.
