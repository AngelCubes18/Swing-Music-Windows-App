const { app, BrowserWindow, Menu, ipcMain } = require('electron');
const path = require('path');
const { loadConfig, saveConfig } = require('./config');

let mainWindow;
const iconPath = path.join(__dirname, 'assets', 'swingmusiclogo.ico');

function showSetup() {
  if (!mainWindow) {
    return;
  }

  mainWindow.loadFile(path.join(__dirname, 'renderer', 'setup.html'));
}

function createMenu() {
  const menu = Menu.buildFromTemplate([
    {
      label: 'Change the URL',
      submenu: [
        {
          label: 'Change the URL',
          accelerator: 'Ctrl+L',
          click: showSetup
        }
      ]
    }
  ]);

  Menu.setApplicationMenu(menu);
}

function createWindow() {
  const config = loadConfig();

  mainWindow = new BrowserWindow({
    width: 1200,
    height: 800,
    minWidth: 880,
    minHeight: 620,
    backgroundColor: '#050505',
    icon: iconPath,
    webPreferences: {
      preload: path.join(__dirname, 'preload.js')
    }
  });

  createMenu();

  if (config && config.url) {
    mainWindow.loadURL(config.url);
  } else {
    showSetup();
  }
}

ipcMain.handle('save-url', (event, url) => {
  saveConfig({ url });
  mainWindow.loadURL(url);
});

ipcMain.handle('get-url', () => {
  const config = loadConfig();
  return config && config.url ? config.url : '';
});

app.whenReady().then(createWindow);

app.on('window-all-closed', () => {
  if (process.platform !== 'darwin') {
    app.quit();
  }
});

app.on('activate', () => {
  if (BrowserWindow.getAllWindows().length === 0) {
    createWindow();
  }
});
