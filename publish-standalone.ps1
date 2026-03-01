$ErrorActionPreference = "Stop"

dotnet restore
dotnet publish .\CipherLua.csproj -c Release -p:PublishProfile=Standalone-Win64

Write-Host "Standalone build created at: bin\\Release\\publish\\win-x64\\"
