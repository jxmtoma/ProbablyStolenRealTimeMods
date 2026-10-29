[CmdletBinding()]
param([Parameter(Mandatory=$true)][string]$GameDir,[switch]$Rebuild,[string]$RestoreConfigFile)
$ErrorActionPreference='Stop'
$game=[IO.Path]::GetFullPath($GameDir)
$project=Join-Path $PSScriptRoot 'src/RealTimeShop/RealTimeShop.csproj'
$restore=@('restore',$project,"-p:GameDir=$game",'-p:NuGetAudit=false')
if($RestoreConfigFile){$restore+=@('--configfile',[IO.Path]::GetFullPath($RestoreConfigFile))}
& dotnet @restore
if($LASTEXITCODE){throw 'Restore failed'}
$target=if($Rebuild){'Rebuild'}else{'Build'}
& dotnet build $project -c Release --no-restore "-p:GameDir=$game" -p:UseSharedCompilation=false "-t:$target"
if($LASTEXITCODE){throw 'Build failed'}
Get-FileHash (Join-Path $PSScriptRoot 'src/RealTimeShop/bin/Release/netstandard2.1/ProbablyStolen.RealTimeShop.dll') -Algorithm SHA256
