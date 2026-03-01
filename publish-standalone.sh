#!/usr/bin/env bash
set -euo pipefail

PROJECT="./CipherLua.csproj"
PUBLISH_PROFILE="Standalone-Win64"
OUTPUT_DIR="artifacts/CipherLua-win-x64"
ZIP_PATH="artifacts/CipherLua-win-x64.zip"

rm -rf "$OUTPUT_DIR"
rm -f "$ZIP_PATH"

dotnet restore "$PROJECT"
dotnet publish "$PROJECT" -c Release -p:PublishProfile="$PUBLISH_PROFILE"

(
  cd artifacts
  zip -r "$(basename "$ZIP_PATH")" "$(basename "$OUTPUT_DIR")"
)

echo "Standalone EXE output: $OUTPUT_DIR"
echo "ZIP package: $ZIP_PATH"
