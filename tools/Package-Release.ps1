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
& git -C $source diff --quiet $SourceCommit -- .
if($LASTEXITCODE){throw 'Tracked source must match SourceCommit before packaging.'}
[xml]$project=Get-Content -Raw -LiteralPath (Join-Path $source 'src/RealTimeShop/RealTimeShop.csproj')
$version=[string]$project.Project.PropertyGroup.Version
if($version -notmatch '^\d+\.\d+\.\d+(?:-[A-Za-z0-9.-]+)?$'){throw 'Invalid project version'}
if([Diagnostics.FileVersionInfo]::GetVersionInfo([IO.Path]::GetFullPath($DllPath)).ProductVersion -ne $version){throw 'DLL/project version mismatch'}
$stage=Join-Path $out 'player-staging'
New-Item -ItemType Directory -Path "$stage/Mods","$stage/licenses","$out/verification" -Force | Out-Null
Copy-Item -LiteralPath $DllPath -Destination "$stage/Mods/ProbablyStolen.RealTimeShop.dll"
foreach($name in @('README.txt','CHANGELOG.txt','DEPENDENCIES.txt','DEPENDENCY-HASHES.json','LICENSE.txt','LICENSE-NOTICE.txt','VALIDATION.txt','RECOVERY.md','RELEASE-NOTES-1.0.0.md','USER-INSTALL-AND-ACCEPTANCE.md','RealTimeShop.cfg.example')){
 Copy-Item -LiteralPath (Join-Path $source $name) -Destination (Join-Path $stage $name)
}
Get-ChildItem -LiteralPath "$source/licenses" -Filter '*.txt' -File | ForEach-Object {Copy-Item -LiteralPath $_.FullName -Destination "$stage/licenses/$($_.Name)"}
$utf8=[Text.UTF8Encoding]::new($false)
$provenance=@"
PREPARED RELEASE - NOT PUBLISHED
Version: $version
Repository: https://github.com/jxmtoma/ProbablyStolenRealTimeMods
Source commit: $SourceCommit
DLL SHA-256: $($ExpectedDllSha256.ToUpperInvariant())

This is reconstructed source and a new build, not the historical RC.22 DLL.
Read VALIDATION.txt, RELEASE-NOTES-1.0.0.md and README.txt for validation scope.
RC.25 native results apply to its recorded binary; consult the 1.0.0 preparation
record for this build. No blanket gameplay acceptance is claimed.

Rebuild from the matching source archive using Windows PowerShell, SDK
10.0.401 (global.json), .NET 6.0.36 reference packages, the dependency versions/hashes
in DEPENDENCY-HASHES.json and a licensed supported game installation.
Run Build.ps1 -GameDir '<supported game directory>' -Rebuild.
Run Test.ps1. Both accept -RestoreConfigFile for an existing offline NuGet
configuration. See the preparation record for the toolchain and restore method.
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
$zip=Join-Path $out "RealTimeShop-$version.zip"
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
$export=Join-Path $out 'verification/source-export.zip'
& git -C $source archive --format=zip "--output=$export" $SourceCommit . ':!recovered/installed'
if($LASTEXITCODE){throw 'Source export failed'}
$sourceStage=Join-Path $out 'source-staging'
[IO.Compression.ZipFile]::ExtractToDirectory($export,$sourceStage)
$forbidden=@(Get-ChildItem -LiteralPath $sourceStage -Recurse -File | Where-Object {$_.Extension -in @('.dll','.exe','.pdb','.es3','.log','.zip') -or $_.Name -like '*.realtime-*.json'})
if($forbidden.Count){throw 'Source package contains a forbidden binary or private artifact'}
$sourceZip=Join-Path $out "RealTimeShop-$version-source.zip"
& "$PSScriptRoot/New-DeterministicZip.ps1" -InputDirectory $sourceStage -Destination $sourceZip | Out-Null
& "$PSScriptRoot/New-DeterministicZip.ps1" -InputDirectory $sourceStage -Destination "$out/verification/reproduced-source.zip" | Out-Null
$sourceHash=(Get-FileHash -LiteralPath $sourceZip -Algorithm SHA256).Hash
if($sourceHash -ne (Get-FileHash -LiteralPath "$out/verification/reproduced-source.zip" -Algorithm SHA256).Hash){throw 'Source ZIP reproduction mismatch'}
$inventory | ConvertTo-Json | Set-Content -LiteralPath "$out/PACKAGE-INVENTORY.json" -Encoding utf8NoBOM
$report=[ordered]@{version=$version;sourceCommit=$SourceCommit;dllSha256=$ExpectedDllSha256.ToUpperInvariant();playerZipSha256=$first;sourceZipSha256=$sourceHash;zipReproduced=$true;sourceZipReproduced=$true;extractedEntriesVerified=$inventory.Count;powerShell=$PSVersionTable.PSVersion.ToString();packagingRuntime=[Runtime.InteropServices.RuntimeInformation]::FrameworkDescription}
$report | ConvertTo-Json | Set-Content -LiteralPath "$out/PACKAGE-VERIFICATION.json" -Encoding utf8NoBOM
[IO.File]::WriteAllText("$out/SHA256SUMS.txt",$first.ToLowerInvariant()+"  "+[IO.Path]::GetFileName($zip)+"`n"+$sourceHash.ToLowerInvariant()+"  "+[IO.Path]::GetFileName($sourceZip)+"`n",$utf8)
$report | ConvertTo-Json
