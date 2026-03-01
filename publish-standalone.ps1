$ErrorActionPreference = "Stop"

$project = ".\CipherLua.csproj"
$publishProfile = "Standalone-Win64"
$outputDir = "artifacts\CipherLua-win-x64"
$zipPath = "artifacts\CipherLua-win-x64.zip"

if (Test-Path $outputDir) {
  Remove-Item $outputDir -Recurse -Force
}

if (Test-Path $zipPath) {
  Remove-Item $zipPath -Force
}

dotnet restore $project
dotnet publish $project -c Release -p:PublishProfile=$publishProfile

Compress-Archive -Path "$outputDir\*" -DestinationPath $zipPath -Force

Write-Host "Standalone EXE output: $outputDir"
Write-Host "ZIP package: $zipPath"
