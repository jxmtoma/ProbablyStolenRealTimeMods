# Real-Time Shop 0.2.1-rc.22

> Historical artifact, superseded by RC.24. The original RC.22 source/build commit remains unknown. Current reconstruction, provenance and validation limits are documented in the repository root and [RC.24 release record](../releases/0.2.1-rc.24-recovered/PROVENANCE.md).

**Recovered installed build.** This is a byte-for-byte copy of the mod DLL that was installed in the Probably Stolen Demo on 2026-10-18. The available source tree is version 0.2.0; source matching this binary was not recovered, so this artifact cannot be rebuilt from the repository.

## Asset

- [ProbablyStolen.RealTimeShop.dll](installed/ProbablyStolen.RealTimeShop.dll)
- SHA-256: `F58F216FE70BD7D6535561DB99D9F244D9DFC5B16A45A8F16AC698F50062812F`

## Compatibility and installation

- Probably Stolen Steam Demo, app 4349200, public build 25382790
- MelonLoader 0.7.3 Open-Beta
- Copy the DLL into the game's `Mods` folder. Back up saves first and read [README.txt](../README.txt).
- Real-time behavior requires the mod's new-game opt-in.

## Verification limits

The captured MelonLoader log confirms that the assembly loaded and that the pause/focus timer hook ran. No error or unhandled-exception entries appeared in that captured session. This does not confirm the opt-in flow or a full gameplay day.

The repository's managed and security assertions passed against the checked-in 0.2.0 source. They do not validate this recovered binary. A build was attempted against the installed game but could not restore because the offline SDK lacked the .NET 6.0.36 reference packs.
