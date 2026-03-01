# CipherLua

CipherLua is a WPF desktop app (Windows) with a custom neon-themed UI.

## Standalone app output (no Visual Studio needed for end users)
This repo is configured to publish CipherLua as a **self-contained Windows `.exe`** and package it as a **ZIP**.

What end users do:
1. Download `CipherLua-win-x64.zip`
2. Extract it
3. Run `CipherLua.exe`

End users do **not** need Visual Studio, .NET SDK, or any compiler/runtime installed.

## For maintainers: create the distributable ZIP
### Option A (PowerShell, Windows)
```powershell
./publish-standalone.ps1
```

### Option B (Bash)
```bash
./publish-standalone.sh
```

Both scripts:
- restore/publish with profile `Standalone-Win64`
- generate standalone output in `artifacts/CipherLua-win-x64/`
- create ZIP: `artifacts/CipherLua-win-x64.zip`

## Publish profile details
Publish profile file:
- `Properties/PublishProfiles/Standalone-Win64.pubxml`

Important settings:
- `RuntimeIdentifier=win-x64`
- `SelfContained=true`
- `PublishSingleFile=true`
- `UseAppHost=true`
- `PublishTrimmed=false`

## Development build
```bash
dotnet build
```
