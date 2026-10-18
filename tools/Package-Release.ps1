[CmdletBinding()]
param(
 [Parameter(Mandatory=$true)][string]$DllPath,
 [Parameter(Mandatory=$true)][ValidatePattern('^[0-9a-fA-F]{40}$')][string]$SourceCommit,
 [Parameter(Mandatory=$true)][ValidatePattern('^[0-9a-fA-F]{64}$')][string]$ExpectedDllSha256,
 [Parameter(Mandatory=$true)][string]$OutputDirectory
)
$ErrorActionPreference='Stop'
$source=Split-Path -Parent $PSScriptRoot
$out=[IO.Path]::GetFullPath($OutputDirectory)
if(Test-Path -LiteralPath $out){throw 'Use a new output directory; existing artifacts are never overwritten.'}
if((Get-FileHash -LiteralPath $DllPath -Algorithm SHA256).Hash -ne $ExpectedDllSha256){throw 'DLL hash mismatch'}
$version='0.2.1-rc.24-recovered'
$stage=Join-Path $out 'player-staging'
New-Item -ItemType Directory -Path "$stage/Mods","$stage/licenses","$out/verification" -Force | Out-Null
Copy-Item -LiteralPath $DllPath -Destination "$stage/Mods/ProbablyStolen.RealTimeShop.dll"
foreach($name in @('README.txt','CHANGELOG.txt','DEPENDENCIES.txt','DEPENDENCY-HASHES.json','LICENSE.txt','LICENSE-NOTICE.txt','VALIDATION.txt','RECOVERY.md','USER-INSTALL-AND-ACCEPTANCE.md','RealTimeShop.cfg.example')){
 Copy-Item -LiteralPath (Join-Path $source $name) -Destination (Join-Path $stage $name)
}
Get-ChildItem -LiteralPath "$source/licenses" -Filter '*.txt' -File | ForEach-Object {Copy-Item -LiteralPath $_.FullName -Destination "$stage/licenses/$($_.Name)"}
$utf8=[Text.UTF8Encoding]::new($false)
$provenance=@"
LOCAL REVIEW CANDIDATE - NATIVE GAMEPLAY ACCEPTANCE PENDING
Version: $version
Repository: https://github.com/jxmtoma/ProbablyStolenRealTimeMods
Source commit: $SourceCommit
DLL SHA-256: $($ExpectedDllSha256.ToUpperInvariant())

This is reconstructed source and a new build, not the historical RC.22 DLL.
No fresh native installation, startup, gameplay or screenshots are certified.
Read VALIDATION.txt and README.txt before testing in a disposable profile.

Rebuild from the matching source archive using Windows PowerShell, SDK
10.0.401, .NET 6.0.36 reference packages, the exact dependency versions/hashes
in DEPENDENCY-HASHES.json and a licensed supported game installation.
Run Build.ps1 -GameDir '<supported game directory>' -Rebuild.
Run Test.ps1. Both accept -RestoreConfigFile for an existing offline NuGet
configuration. The preparation used cached packages with online sources cleared.
Dependencies, native/game binaries, interop, saves and private fixtures are
not distributed. SDK checks target .NET 10; the mod targets .NET 6.

Informational version has no Git suffix. Compilation is deterministic with
no PDB and a source path map. Package with tools/Package-Release.ps1 using
this commit and DLL hash. ZIP entries are ordinal-sorted, fixed at
2000-01-01 UTC, external attributes zero, ZipArchive Optimal compression.
Exact compression bytes depend on the recorded PowerShell/.NET runtime.
The source package excludes recovered binary artifacts and build outputs.
MIT covers original mod code only; dependency notices apply separately.
"@
[IO.File]::WriteAllText("$stage/SOURCE.txt",$provenance.Replace("`r`n","`n")+"`n",$utf8)
$names=[string[]]@(Get-ChildItem -LiteralPath $stage -File -Recurse | ForEach-Object {$_.FullName.Substring($stage.Length+1).Replace('\','/')})
[Array]::Sort($names,[StringComparer]::Ordinal)
$lines=@($names | ForEach-Object {((Get-FileHash -LiteralPath (Join-Path $stage $_) -Algorithm SHA256).Hash.ToLowerInvariant())+'  '+$_})
[IO.File]::WriteAllText("$stage/PACKAGE-CONTENTS.sha256",($lines -join "`n")+"`n",$utf8)
$zip=Join-Path $out "RealTimeShop-$version-review.zip"
$repeat=Join-Path $out 'verification/reproduced-player.zip'
& "$PSScriptRoot/New-DeterministicZip.ps1" -InputDirectory $stage -Destination $zip | Out-Null
& "$PSScriptRoot/New-DeterministicZip.ps1" -InputDirectory $stage -Destination $repeat | Out-Null
$first=(Get-FileHash -LiteralPath $zip -Algorithm SHA256).Hash
if($first -ne (Get-FileHash -LiteralPath $repeat -Algorithm SHA256).Hash){throw 'ZIP reproduction mismatch'}
$extract=Join-Path $out 'verification/extracted-player'
[IO.Compression.ZipFile]::ExtractToDirectory($zip,$extract)
$inventory=@(Get-ChildItem -LiteralPath $stage -File -Recurse | ForEach-Object {
 $rel=$_.FullName.Substring($stage.Length+1).Replace('\','/')
 $hash=(Get-FileHash -LiteralPath $_.FullName -Algorithm SHA256).Hash
 if($hash -ne (Get-FileHash -LiteralPath (Join-Path $extract $rel) -Algorithm SHA256).Hash){throw "Extraction mismatch: $rel"}
 [ordered]@{path=$rel;bytes=$_.Length;sha256=$hash}
})
$inventory | ConvertTo-Json | Set-Content -LiteralPath "$out/PACKAGE-INVENTORY.json" -Encoding utf8NoBOM
$report=[ordered]@{version=$version;sourceCommit=$SourceCommit;dllSha256=$ExpectedDllSha256.ToUpperInvariant();playerZipSha256=$first;zipReproduced=$true;extractedEntriesVerified=$inventory.Count;powerShell=$PSVersionTable.PSVersion.ToString();packagingRuntime=[Runtime.InteropServices.RuntimeInformation]::FrameworkDescription}
$report | ConvertTo-Json | Set-Content -LiteralPath "$out/PACKAGE-VERIFICATION.json" -Encoding utf8NoBOM
$report | ConvertTo-Json
