# Swing Music Windows App

A simple Windows desktop wrapper for a self-hosted [Swing Music](https://github.com/swing-opensource/swingmusic) server.

This app opens your Swing Music Web UI inside a dedicated Electron window. On first launch, it asks for the IP address or URL of your Swing Music web panel, saves it locally, and then opens that server automatically on future launches.

It is useful if you want Swing Music to feel like a normal Windows application instead of keeping it pinned inside a browser tab.

## Features

- Clean black setup screen for entering your Swing Music server URL.
- Automatically adds `http://` when you enter only an IP address or domain.
- Saves the server URL in the user's app data folder.
- Single application menu option: `Change the URL`.
- `Ctrl+L` shortcut to return to the URL setup screen.
- Custom Swing Music icon for the app and installer.
- Windows installer built with NSIS.
- Installer allows choosing the installation directory.

## Requirements

To run or build from source, install:

- [Node.js](https://nodejs.org/) 18 or newer
- npm, included with Node.js
- Windows 10 or newer

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

## Build the Windows Installer

Create the Windows executable and installer:

```bash
npm run build
```

The build output will be created in:

```text
dist/
```

The main installer file will look like:

```text
dist/Swing Music Setup 1.1.0.exe
```

## Project Structure

```text
assets/              App and installer icons
renderer/            Setup screen HTML and renderer JavaScript
config.js            Local config loading and saving
main.js              Electron main process and app menu
preload.js           Safe bridge between Electron and the setup page
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
