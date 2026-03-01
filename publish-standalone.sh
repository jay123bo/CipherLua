#!/usr/bin/env bash
set -euo pipefail

dotnet restore
dotnet publish ./CipherLua.csproj -c Release -p:PublishProfile=Standalone-Win64

echo "Standalone build created at: bin/Release/publish/win-x64/"
