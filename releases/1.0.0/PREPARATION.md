# Real-Time Shop 1.0.0 preparation

Prepared locally on 2026-10-18 (America/Los_Angeles). No release tag, GitHub Release or Nexus upload has been created. Player and source archives are verified local deliverables.

Exact source commit for both archives: **`cdaa07941a0a18eeae50714206dccb733642b9d7`**. Later documentation commits record packaging and local installation results without changing that archive source identity.

The gameplay fixes are committed in `f8c662190409737c9f7c4fb7f898cb566f271cf3`. The 1.0.0 promotion changes assembly/Melon version metadata and startup identification from RC.25. Packaging tools, the SDK pin and release documentation are also updated.

| Build | SHA-256 |
|---|---|
| 1.0.0 mod DLL, assembly/file version 1.0.0.0 | `27A568AE67C434B392FB674D5D0B5DEB9124258886E266D3D2D465324552ED70` |
| Native-tested RC.25 reference DLL | `407A6270050333FA1DF66F863A0F960A761DF10FA98C32D1757A4090F9D02F36` |
| RealTimeShop-1.0.0.zip | `046D19B2A4C70ACB471924D410474F3975CFAFC0B54CA4360C503B80DBF11B0A` |
| RealTimeShop-1.0.0-source.zip | `14284E7581930BD92375439A9B4B3FA88EB42287CC38A38FF01AC3C69954DD61` |

## Build and managed checks

The standard `Build.ps1 -Rebuild` succeeds with zero warnings and errors. `Test.ps1` passes **31,127 managed checks and 38 security assertions**. A clean export of the archived source rebuilt the exact DLL hash above and passed the same checks using cached reference packages with online sources cleared; see [SOURCE-REBUILD.json](SOURCE-REBUILD.json). Checks use synthetic policy inputs; they do not execute the game.

Release toolchain: Windows, PowerShell 7.6.5 / .NET 10.0.11, SDK **10.0.401**, and .NET **6.0.36** reference packages. `global.json` pins the SDK without roll-forward. The workspace-local SDK archive was downloaded from Microsoft's release distribution and matched its published SHA-512. Reference packages were restored from NuGet and cached locally. No machine-wide SDK installation is required by this preparation.

Build references are the separately installed supported demo, MelonLoader 0.7.3, HarmonyX 2.10.2 and Il2CppInterop.Runtime 1.5.1-ci.845, with exact dependency hashes in [DEPENDENCY-HASHES.json](../../DEPENDENCY-HASHES.json). The mod targets .NET 6; check programs target .NET 10. Release compilation is deterministic, without PDB or Git informational-version suffix, with a source path map.

```powershell
./Build.ps1 -GameDir '<supported demo directory>' -Rebuild
./Test.ps1
```

Both scripts accept `-RestoreConfigFile` for an existing offline NuGet configuration. The SDK, dependencies, game files and private test fixtures are excluded from distribution.

## Relationship to the tested RC.25 build

`tools/Compare-ReleaseBodies.ps1` compares all **653** `RealTimeShop` method bodies, including nested compiler-generated methods. Both the SDK-built RC.25 reference and the actual native-tested RC.25 DLL match the 1.0.0 candidate, with no missing, added or changed bodies after normalization. The comparison resolves instruction/variable indices and member identities and normalizes version text only in `Plugin.OnInitializeMelon`.

The retained comparison report describes its limits: this read-only comparison does not execute either assembly, compare all metadata or prove native gameplay equivalence. [The RC.25 native test record](../../release-rc25-native-test.txt) identifies its exact DLL and focused scenarios: idle-offer customer timeout with native item/money preservation, closed and bedroll time, pause/resume, automatic 03:00 sleep and a playable next morning at 11:00 with one native overnight cost.

## Exact 1.0.0 native smoke check

The unmodified 1.0.0 DLL above was launched in a local copy of the supported demo. Only the copy's Unity company/product identity was changed to select a disposable save/settings profile. A test-only loader identity alias allowed the unmodified candidate to load; a separate QA helper validated the native save path, blocked Steam init/restart/stat/achievement calls and recorded state. Helpers, copied game files, screenshots and logs remain private and are excluded from packages. This setup verifies isolated native behavior; it does not exercise live Steam integration.

Observed results:

- Loader identifies **Real-Time Shop 1.0.0** and the exact DLL hash above; initialization succeeds. An existing Il2CppInterop class-init substitute warning remains in the loader log.
- Native Load Game opens the prior disposable **Day 2 / 545 credits** save from the RC.25 test. Consent and timer sidecars are accepted; the native notice confirms timers restored.
- Closing the morning report resumes the clock at approximately 11:01. It advances to 11:27 with shutters closed, while day and cash remain unchanged. Native pause holds it at 11:27 until exit.
- Native Exit Game completes with process exit code 0. No duplicate overnight cost/day advance is observed.
- All **21** normal native save/index/sidecar files and installed mod preferences match the pre-test backup byte-for-byte. Unity's normal-profile player logs and analytics cache changed during startup/shutdown; this record does not claim the entire profile directory was untouched.

This smoke check uses test timing 3 / 20 / 10 and diagnostics enabled only in the disposable copy. Player defaults remain **30 active minutes / 120 seconds patience / 60 seconds closing grace**, diagnostics off, and real-time mode off until explicit new-game opt-in. Existing vanilla saves are not converted.

Every transaction variant, protected/scripted visitor, recovery/error path, mod combination and long campaign has not been exhaustively tested. RC.25 timing evidence and this exact-binary smoke check are reported separately. The full game and other builds remain unverified.

## Packaging and publication

`tools/Package-Release.ps1` requires the exact committed source and DLL hash. It emits `RealTimeShop-1.0.0.zip` and `RealTimeShop-1.0.0-source.zip`, repeats each deterministic ZIP write and verifies every extracted player entry against staging. Player contents include one mod DLL, documentation, configuration example and license notices. Source exports exclude the historical recovered DLL, native/loader files, build outputs, saves, logs and private fixtures.

Both deterministic ZIP reproductions match. All **18** extracted player entries match their staging hashes. See [PACKAGE-VERIFICATION.json](PACKAGE-VERIFICATION.json), [PACKAGE-INVENTORY.json](PACKAGE-INVENTORY.json) and [SHA256SUMS.txt](SHA256SUMS.txt). The ZIPs are retained locally under `artifacts/release-1.0.0/` rather than committed as generated binaries. The source archive identifies the exact source commit above; this final report was added after packaging.

The verified extracted 1.0.0 DLL is installed locally. Its installed hash matches the table above; the prior RC.25 DLL and preferences were backed up under `backups/install-1.0.0-20261018-174522/`. The earlier full save backup remains under `backups/install-20261019-000939/`. Preferences were preserved. Native smoke testing used the isolated copy described above; the normal player campaign was not loaded for testing.

Keep the package report and SHA256SUMS with the ZIPs. Read [release notes](../../RELEASE-NOTES-1.0.0.md) and [installation/rollback instructions](../../USER-INSTALL-AND-ACCEPTANCE.md) before installation. Public release publication is a separate action.
