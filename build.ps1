$ErrorActionPreference = "Stop"

dotnet restore .\SwingMusic.csproj
dotnet publish .\SwingMusic.csproj `
  -c Release `
  -r win-x64 `
  --self-contained false `
  -p:PublishSingleFile=true `
  -p:IncludeNativeLibrariesForSelfExtract=true `
  -p:DebugType=None `
  -p:DebugSymbols=false `
  -o .\native-single

Write-Host "Built .\native-single\Swing Music.exe"
