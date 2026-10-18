[CmdletBinding()]
param([string]$RestoreConfigFile)
$ErrorActionPreference='Stop'
foreach($project in @('checks/Checks.csproj','security/Checks.csproj')){
 $path=Join-Path $PSScriptRoot $project
 $restore=@('restore',$path,'-p:NuGetAudit=false')
 if($RestoreConfigFile){$restore+=@('--configfile',[IO.Path]::GetFullPath($RestoreConfigFile))}
 & dotnet @restore
 if($LASTEXITCODE){throw "Restore failed: $project"}
 & dotnet run --project $path -c Release --no-restore -p:UseSharedCompilation=false
 if($LASTEXITCODE){throw "Checks failed: $project"}
}
