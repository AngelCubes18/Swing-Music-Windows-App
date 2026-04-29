# Swing Music Windows App

A lightweight Windows desktop wrapper for a self-hosted [Swing Music](https://github.com/swing-opensource/swingmusic) server.

This app opens your Swing Music Web UI in a native Windows window using Microsoft WebView2. It asks for your Swing Music server URL on first launch, saves it locally, and opens that server automatically after that.

This version replaces the earlier Electron build. The Electron installer was around 95 MB because it bundled Chromium. The native WebView2 build uses the browser runtime already installed on most Windows systems, so the compiled single-file app is about 2 MB on this machine.

## Features

- Native C# WinForms app using Microsoft WebView2.
- Much smaller than the old Electron version.
- Clean black setup screen for entering your Swing Music server URL.
- Automatically adds `http://` when you enter only an IP address or domain.
- Saves the server URL in the user's app data folder.
- Borderless app window with integrated window controls.
- `Change URL` button inside the app chrome.
- `Ctrl+L` shortcut to return to the URL setup screen.
- Custom Swing Music app icon.

## Requirements

To run the framework-dependent build, Windows needs:

- Windows 10 or newer
- Microsoft Edge WebView2 Runtime
- .NET Core Desktop Runtime 3.1

To build from source, install:

- .NET SDK 3.1 or newer with Windows desktop support

This app does not include the Swing Music server itself. It only opens the web interface of a server you already run.

## Build From Source

Clone the repository:

```powershell
git clone https://github.com/AngelCubes18/Swing-Music-Windows-App.git
cd Swing-Music-Windows-App
```

Build the lightweight single-file executable:

```powershell
.\build.ps1
```

The output will be created here:

```text
native-single/Swing Music.exe
```

You can also run the build command manually:

```powershell
dotnet publish .\SwingMusic.csproj -c Release -r win-x64 --self-contained false -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -p:DebugType=None -p:DebugSymbols=false -o .\native-single
```

## Development Run

Run the app directly from source:

```powershell
dotnet run --project .\SwingMusic.csproj
```

When the setup screen opens, enter your Swing Music URL, for example:

```text
192.168.1.50:1970
```

or:

```text
http://192.168.1.50:1970
```

## Project Structure

```text
assets/              App icon and setup logo
ConfigStore.cs       Local config loading and saving
MainForm.cs          WinForms UI, WebView2 host, window controls
Program.cs           Application entry point
SwingMusic.csproj    .NET project and WebView2 dependency
build.ps1            Release build script
```

## Notes For Contributors

Do not commit generated build output:

```text
bin/
obj/
native-dist/
native-single/
```

Those folders are ignored because they can be recreated from source.

## License

This project is licensed under the GNU General Public License v3.0. See [LICENSE](LICENSE) for details.
