[CmdletBinding()]
param(
 [Parameter(Mandatory=$true)][string]$GameDir,
 [Parameter(Mandatory=$true)][string]$DllPath,
 [Parameter(Mandatory=$true)][string]$ReportPath
)
$ErrorActionPreference='Stop'
$game=[IO.Path]::GetFullPath($GameDir)
$interop=Join-Path $game 'MelonLoader/Il2CppAssemblies'
$loader=Join-Path $game 'MelonLoader/net6'
Add-Type -Path (Join-Path $loader 'Mono.Cecil.dll')
$resolver=[Mono.Cecil.DefaultAssemblyResolver]::new()
$resolver.AddSearchDirectory($interop)
$resolver.AddSearchDirectory($loader)
$parameters=[Mono.Cecil.ReaderParameters]::new()
$parameters.AssemblyResolver=$resolver
$module=[Mono.Cecil.ModuleDefinition]::ReadModule([IO.Path]::GetFullPath($DllPath),$parameters)
try {
 $generated=@{}
 Get-ChildItem -LiteralPath $interop -Filter '*.dll' -File | ForEach-Object {$generated[$_.BaseName]=$true}
 $members=@($module.GetMemberReferences() | Where-Object {$generated.ContainsKey($_.DeclaringType.Scope.Name)} | Sort-Object FullName -Unique)
 if(!$members.Count){throw 'No generated game/Unity references were found.'}
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
    if($stub.Count){throw 'Generated method contains an unstripping/unsupported stub'}
   }
   $resolved++
  } catch {$failures+=([ordered]@{member=$member.FullName;error=$_.Exception.Message})}
 }
 $report=[ordered]@{
  dllSha256=(Get-FileHash -LiteralPath $DllPath -Algorithm SHA256).Hash
  gameAssemblySha256=(Get-FileHash -LiteralPath (Join-Path $game 'GameAssembly.dll') -Algorithm SHA256).Hash
  metadataSha256=(Get-FileHash -LiteralPath (Join-Path $game 'Probably Stolen_Data/il2cpp_data/Metadata/global-metadata.dat') -Algorithm SHA256).Hash
  referencedMembers=$members.Count
  resolvedMembers=$resolved
  failures=@($failures)
  passed=(!$failures.Count)
  methodology='Read-only Cecil resolution of all mod member references into generated game/Unity assemblies, including compiler-generated code. Reject known unstripping/unsupported stub text. Does not execute native methods, verify their semantics or replace gameplay testing.'
 }
 $report | ConvertTo-Json -Depth 6 | Set-Content -LiteralPath $ReportPath -Encoding utf8NoBOM
 $report | ConvertTo-Json -Depth 6
 if(!$report.passed){throw 'Game interop verification failed; inspect the report.'}
} finally {$module.Dispose();$resolver.Dispose()}
