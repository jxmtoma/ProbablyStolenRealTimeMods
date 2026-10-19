[CmdletBinding()]
param(
 [Parameter(Mandatory=$true)][string]$ReferenceDll,
 [Parameter(Mandatory=$true)][string]$CandidateDll,
 [Parameter(Mandatory=$true)][string]$CecilPath,
 [Parameter(Mandatory=$true)][string]$ReportPath,
 [string]$ReferenceVersion='0.2.1-rc.25-recovered',
 [string]$CandidateVersion='1.0.0'
)
$ErrorActionPreference='Stop'
Add-Type -Path ([IO.Path]::GetFullPath($CecilPath))
function Get-ModTypes($types) {
 foreach($type in $types){
  if($type.FullName.StartsWith('RealTimeShop.',[StringComparison]::Ordinal)){$type}
  Get-ModTypes $type.NestedTypes
 }
}
function Get-Body($method) {
 if(!$method.HasBody){return '<no body>'}
 $code=$method.Body.Instructions
 $parts=foreach($instruction in $code){
  $operand=$instruction.Operand
  $value=switch($operand){
   {$_ -is [Mono.Cecil.Cil.Instruction]} {'branch:'+ $code.IndexOf($_);break}
   {$_ -is [Mono.Cecil.Cil.Instruction[]]} {'switch:'+((@($_|ForEach-Object {$code.IndexOf($_)})) -join ',');break}
   {$_ -is [Mono.Cecil.ParameterDefinition]} {'arg:'+ $_.Index;break}
   {$_ -is [Mono.Cecil.Cil.VariableDefinition]} {'local:'+ $_.Index;break}
   {$_ -is [Mono.Cecil.MemberReference]} {$_.FullName;break}
   {$_ -is [string]} {
    if($method.DeclaringType.FullName -eq 'RealTimeShop.Plugin' -and $method.Name -eq 'OnInitializeMelon'){$_.Replace($ReferenceVersion,'<version>').Replace($CandidateVersion,'<version>')}else{$_}
    break
   }
   default {if($null -eq $_){''}else{[Convert]::ToString($_,[Globalization.CultureInfo]::InvariantCulture)}}
  }
  $instruction.OpCode.Code.ToString()+':'+$value
 }
 return ($parts -join "`n")
}
$reference=[Mono.Cecil.ModuleDefinition]::ReadModule([IO.Path]::GetFullPath($ReferenceDll))
$candidate=[Mono.Cecil.ModuleDefinition]::ReadModule([IO.Path]::GetFullPath($CandidateDll))
try {
 $left=@{}; $right=@{}
 Get-ModTypes $reference.Types | ForEach-Object {foreach($method in $_.Methods){$left[$method.FullName]=$method}}
 Get-ModTypes $candidate.Types | ForEach-Object {foreach($method in $_.Methods){$right[$method.FullName]=$method}}
 $missing=@($left.Keys|Where-Object {!$right.ContainsKey($_)})
 $added=@($right.Keys|Where-Object {!$left.ContainsKey($_)})
 $changed=@($left.Keys|Where-Object {$right.ContainsKey($_) -and (Get-Body $left[$_]) -cne (Get-Body $right[$_])}|Sort-Object)
 $report=[ordered]@{referenceSha256=(Get-FileHash -LiteralPath $ReferenceDll).Hash;candidateSha256=(Get-FileHash -LiteralPath $CandidateDll).Hash;comparedMethods=$left.Count;missingMethods=$missing;addedMethods=$added;changedBodies=$changed;passed=(!$missing.Count -and !$added.Count -and !$changed.Count);methodology='Read-only Cecil comparison of all RealTimeShop methods, including nested compiler-generated methods. Normalize branch/local/parameter indices and member names; normalize only startup version text in Plugin.OnInitializeMelon. Does not execute either assembly, compare all metadata, or prove native gameplay equivalence.'}
 $report|ConvertTo-Json -Depth 5|Set-Content -LiteralPath $ReportPath -Encoding utf8NoBOM
 $report|ConvertTo-Json -Depth 5
 if(!$report.passed){throw 'Release method comparison found differences; review the report before promotion.'}
} finally {$reference.Dispose();$candidate.Dispose()}
