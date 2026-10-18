[CmdletBinding()]
param(
    [Parameter(Mandatory=$true)][string]$InputDirectory,
    [Parameter(Mandatory=$true)][string]$Destination
)
$ErrorActionPreference='Stop'
$root=[IO.Path]::GetFullPath($InputDirectory).TrimEnd('\','/')
$files=@(Get-ChildItem -LiteralPath $root -Recurse -File)
$names=[string[]]@($files | ForEach-Object {$_.FullName.Substring($root.Length+1).Replace('\','/')})
[Array]::Sort($names,[StringComparer]::Ordinal)
$stream=[IO.File]::Open([IO.Path]::GetFullPath($Destination),[IO.FileMode]::CreateNew,[IO.FileAccess]::ReadWrite,[IO.FileShare]::None)
try {
    $zip=[IO.Compression.ZipArchive]::new($stream,[IO.Compression.ZipArchiveMode]::Create,$true)
    try {
        foreach($name in $names){
            $entry=$zip.CreateEntry($name,[IO.Compression.CompressionLevel]::Optimal)
            $entry.LastWriteTime=[DateTimeOffset]::new(2000,1,1,0,0,0,[TimeSpan]::Zero)
            $entry.ExternalAttributes=0
            $zipReadStream=[IO.File]::OpenRead((Join-Path $root $name))
            $zipWriteStream=$entry.Open()
            try {$zipReadStream.CopyTo($zipWriteStream)} finally {$zipWriteStream.Dispose();$zipReadStream.Dispose()}
        }
    } finally {$zip.Dispose()}
} finally {$stream.Dispose()}
Get-FileHash -LiteralPath $Destination -Algorithm SHA256
