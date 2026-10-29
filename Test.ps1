[CmdletBinding()]
param([Parameter(Mandatory=$true)][string]$GameDir,[string]$RestoreConfigFile)
$ErrorActionPreference='Stop'
foreach($project in @('checks/Checks.csproj','security/Checks.csproj')){
 $path=Join-Path $PSScriptRoot $project
 $restore=@('restore',$path,'-p:NuGetAudit=false',"-p:GameDir=$([IO.Path]::GetFullPath($GameDir))")
 if($RestoreConfigFile){$restore+=@('--configfile',[IO.Path]::GetFullPath($RestoreConfigFile))}
 & dotnet @restore
 if($LASTEXITCODE){throw "Restore failed: $project"}
 & dotnet run --project $path -c Release --no-restore -p:UseSharedCompilation=false "-p:GameDir=$([IO.Path]::GetFullPath($GameDir))"
 if($LASTEXITCODE){throw "Checks failed: $project"}
}
