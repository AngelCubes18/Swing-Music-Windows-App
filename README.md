# Swing Music Desktop App

A simple desktop wrapper for a self-hosted [Swing Music](https://github.com/swing-opensource/swingmusic) server.

This app opens your Swing Music Web UI inside a dedicated Electron window. On first launch, it asks for the IP address or URL of your Swing Music web panel, saves it locally, and then opens that server automatically on future launches.

It is useful if you want Swing Music to feel like a normal desktop application instead of keeping it pinned inside a browser tab.

## Features

- Clean black setup screen for entering your Swing Music server URL.
- Automatically adds `http://` when you enter only an IP address or domain.
- Saves the server URL in the user's app data folder.
- Hidden native title bar with Windows controls overlaid on the app.
- Transparent in-app `Change URL` control instead of a full menu bar.
- `Ctrl+L` shortcut to return to the URL setup screen.
- Custom Swing Music icon for the app and installer.
- Windows installer built with NSIS.
- Windows portable build.
- Linux tar.gz build.
- Installer allows choosing the installation directory.
- Optimized Electron packaging with ASAR, maximum compression, a small file allowlist, and unused Electron locales removed.

## Requirements

To run or build from source, install:

- [Node.js](https://nodejs.org/) 18 or newer
- npm, included with Node.js
- Windows 10 or newer, or a modern Linux desktop

You also need a running Swing Music server. This app does not include the Swing Music server itself; it only opens the web interface.

## Install Dependencies

Clone the repository and install the dependencies:

```bash
git clone https://github.com/AngelCubes18/Swing-Music-Windows-App.git
cd Swing-Music-Windows-App
npm install
```

## Run in Development

Start the Electron app locally:

```bash
npm start
```

If no server URL has been saved yet, the app will open the setup screen. Enter your Swing Music URL, for example:

```text
192.168.1.50:1970
```

or:

```text
http://192.168.1.50:1970
```

## Build From Source

Create both Windows and Linux builds:

```bash
npm run build
```

Build only Windows:

```bash
npm run build:win
```

Build only Linux:

```bash
npm run build:linux
```

The build output will be created in:

```text
dist/
```

The Windows output includes an NSIS installer and a portable executable:

```text
dist/Swing Music-1.2.0-windows-x64-setup.exe
dist/Swing Music-1.2.0-windows-x64-portable.exe
```

The Linux output includes:

```text
dist/Swing Music-1.2.0-linux-x64.tar.gz
```

The Linux build uses `tar.gz` because it can be produced from Windows and Linux without extra AppImage tooling. If you later want an AppImage too, build that target on Linux or in WSL.

## Electron Size Notes

This app still uses Electron, so the final executable includes Chromium and Node.js. That means it cannot be as small as a native WebView app, but this project is configured to keep the Electron build as small as reasonably possible:

- Only the app files needed at runtime are packaged.
- App source is packed into an ASAR archive.
- Build compression is set to maximum.
- Electron locales are limited to `en-US`.
- `node_modules/` and `dist/` are ignored and should not be committed.

## Project Structure

```text
assets/              App and installer icons
renderer/            Setup screen HTML and renderer JavaScript
config.js            Local config loading and saving
main.js              Electron main process and window controls
preload.js           Safe bridge and transparent in-app URL control
package.json         App scripts and electron-builder config
```

## Notes for Contributors

Do not commit generated files or installed dependencies:

```text
node_modules/
dist/
```

Those folders are ignored because they are large and can be recreated with `npm install` and `npm run build`.

## License

This project is licensed under the GNU General Public License v3.0. See [LICENSE](LICENSE) for details.
