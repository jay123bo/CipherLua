# CipherLua

CipherLua is a WPF desktop app (Windows) with a custom neon-themed UI.

## What was updated
- Upgraded project target to **.NET 8**.
- Configured **Release** builds to publish as a **standalone (self-contained), single-file** app.
- Added a reusable publish profile: `Standalone-Win64`.
- Added helper scripts for quick standalone publishing.
- UI/XAML layout and styling were not changed.

## Build (development)
```bash
dotnet build
```

## Publish standalone app (Windows x64)
Using the publish profile directly:
```bash
dotnet publish ./CipherLua.csproj -c Release -p:PublishProfile=Standalone-Win64
```

Or use helper scripts:
- PowerShell: `./publish-standalone.ps1`
- Bash: `./publish-standalone.sh`

Published output:
- `bin/Release/publish/win-x64/`

## Notes
- The standalone output includes the .NET runtime, so end users do not need to install .NET separately.
- Trimming is disabled to avoid breaking WPF reflection/resource behaviors.
