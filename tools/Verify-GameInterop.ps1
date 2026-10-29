[CmdletBinding()]
param(
 [Parameter(Mandatory=$true)][string]$GameDir,
 [Parameter(Mandatory=$true)][string]$DllPath,
 [Parameter(Mandatory=$true)][string]$ReportPath
)
$ErrorActionPreference='Stop'
$game=[IO.Path]::GetFullPath($GameDir)
$interop=Join-Path $game 'Probably Stolen_Data/Managed'
# The full game supplies Cecil with Burst. No demo/loader installation is needed.
Add-Type -Path (Join-Path $interop 'Unity.Burst.Cecil.dll')
$resolver=[Mono.Cecil.DefaultAssemblyResolver]::new()
$resolver.AddSearchDirectory($interop)
$parameters=[Mono.Cecil.ReaderParameters]::new()
$parameters.AssemblyResolver=$resolver
$module=[Mono.Cecil.ModuleDefinition]::ReadModule([IO.Path]::GetFullPath($DllPath),$parameters)
try {
 $generated=@{}
 Get-ChildItem -LiteralPath $interop -Filter '*.dll' -File | ForEach-Object {$generated[$_.BaseName]=$true}
 $members=@($module.GetMemberReferences() | Where-Object {$generated.ContainsKey($_.DeclaringType.Scope.Name)} | Sort-Object FullName -Unique)
 if(!$members.Count){throw 'No managed game/Unity references were found.'}
 if(@($module.AssemblyReferences | Where-Object {$_.Name -match '^(MelonLoader|Il2Cpp)'}).Count){throw 'Full-game mod contains a legacy loader/IL2CPP reference.'}
 $failures=@()
 $resolved=0
 foreach($member in $members){
  try {
   $definition=$member.Resolve()
   if($null -eq $definition){throw 'Member did not resolve'}
   if($definition -is [Mono.Cecil.MethodDefinition] -and $definition.HasBody){
    $stub=@($definition.Body.Instructions | Where-Object {
     $_.OpCode.Code -eq [Mono.Cecil.Cil.Code]::Ldstr -and $_.Operand -match '(?i)(unstripp|not supported by il2cpp)'
    })
    if($stub.Count){throw 'Method contains an unstripping/unsupported stub'}
   }
   $resolved++
  } catch {$failures+=([ordered]@{member=$member.FullName;error=$_.Exception.Message})}
 }
 $report=[ordered]@{
  dllSha256=(Get-FileHash -LiteralPath $DllPath -Algorithm SHA256).Hash
  gameAssemblyPath='Probably Stolen_Data/Managed/Assembly-CSharp.dll'
  gameAssemblySha256=(Get-FileHash -LiteralPath (Join-Path $interop 'Assembly-CSharp.dll') -Algorithm SHA256).Hash
  legacyLoaderReferences=$false
  referencedMembers=$members.Count
  resolvedMembers=$resolved
  failures=@($failures)
  passed=(!$failures.Count)
  methodology='Read-only Cecil resolution of all mod member references into the full game managed assemblies, including compiler-generated code. Reject legacy MelonLoader/IL2CPP references and unsupported stubs. Does not validate reflection targets, execute game methods, verify their semantics or replace gameplay testing.'
 }
 $report | ConvertTo-Json -Depth 6 | Set-Content -LiteralPath $ReportPath -Encoding utf8NoBOM
 $report | ConvertTo-Json -Depth 6
 if(!$report.passed){throw 'Game interop verification failed; inspect the report.'}
} finally {$module.Dispose();$resolver.Dispose()}
