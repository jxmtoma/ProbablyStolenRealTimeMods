# Real-Time Shop 0.2.1-rc.24-recovered

**Review candidate. Native gameplay acceptance and genuine screenshots remain pending.** No Nexus publication is confirmed or authorized by this record. RC.24 supersedes RC.23 and fixes its missing warning dispatch guards.

Repository: https://github.com/jxmtoma/ProbablyStolenRealTimeMods

Exact source commit used for the preserved archives: **`0b25336932edaf21c2b6b6039726c928aae8d917`**.
Guard restoration commit: `213396c72a280dc19d8e849619ffc5404d927f10`.
Assembly version: **0.2.1.0**; informational/Melon version: **0.2.1-rc.24-recovered**.

| Artifact | SHA-256 |
|---|---|
| RealTimeShop-0.2.1-rc.24-recovered-review.zip | `0ABCB22EB25383C955F3818AE6F56C69A8EC9556518D4994646DB455D6846599` |
| RealTimeShop-0.2.1-rc.24-recovered-source.zip | `BFE71AD8CD4EC328F0852E7B1E926285A316E2904E2382BC340E7DE0F904BD1A` |
| Mods/ProbablyStolen.RealTimeShop.dll inside player ZIP | `5564F136DE73FB23CCA44BAB630913A5B70663EDE642EB137CB9F3DF470E3987` |

The ZIPs are retained separately as user deliverables. They are not committed as generated repository outputs. The player archive contains 17 entries: the sole mod DLL plus instructions/provenance and license notices. The source archive contains 60 entries and excludes the historical reference DLL and all build outputs. Later main documentation commits do not change the exact archived source identity above.

## Reproduction

Use Windows, PowerShell, SDK **10.0.401**, .NET **6.0.36** reference packages, a licensed exact supported demo and the separately obtained dependencies/hashes in [DEPENDENCY-HASHES.json](../../DEPENDENCY-HASHES.json). No game, loader, runtime or generated interop is supplied. No SDK is required to install/play the binary.

```powershell
./Build.ps1 -GameDir '<supported demo directory>' -Rebuild
./Test.ps1
```

Both accept `-RestoreConfigFile '<existing offline NuGet config>'`. The preparation used only an existing SDK/cached packages with online package sources cleared. Build outputs are local bin/obj files; the scripts do not install the mod or launch the game.

Compilation uses deterministic Release settings, no PDB, a source path map and no Git informational-version suffix. Two independent clean exports built identical DLL bytes. `tools/Package-Release.ps1` accepts the DLL, exact source commit/hash and a fresh output directory; it produces fixed-timestamp, ordinal-sorted ZIP entries, repeats the ZIP write and verifies extraction hashes. Both independently staged player archives and repeated source archives were byte-identical. Exact compression was recorded with **PowerShell 7.6.5 / .NET 10.0.11**.

See [VALIDATION-SUMMARY.md](VALIDATION-SUMMARY.md), [VALIDATION.txt](../../VALIDATION.txt), [RECOVERY.md](../../RECOVERY.md) and [RECOVERY-FIDELITY-REVIEW.md](../../RECOVERY-FIDELITY-REVIEW.md) for source recovery, reference checks and limits.

Original mod code is MIT, including commercial reuse. Dependency licenses apply separately. The historical RC.22 DLL in recovered/installed is the project's owned mod reference, already preserved in repository history; it is not the current release or a bundled game dependency.
