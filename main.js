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

function createWindow() {
  const config = loadConfig();

  mainWindow = new BrowserWindow({
    width: 1200,
    height: 800,
    minWidth: 880,
    minHeight: 620,
    backgroundColor: '#050505',
    icon: iconPath,
    titleBarStyle: 'hidden',
    titleBarOverlay: {
      color: 'rgba(0, 0, 0, 0)',
      symbolColor: '#f5f5f5',
      height: 34
    },
    autoHideMenuBar: true,
    webPreferences: {
      preload: path.join(__dirname, 'preload.js')
    }
  });

  Menu.setApplicationMenu(null);

  mainWindow.webContents.on('before-input-event', (event, input) => {
    if (input.control && typeof input.key === 'string' && input.key.toLowerCase() === 'l') {
      event.preventDefault();
      showSetup();
    }
  });

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

ipcMain.handle('change-url', () => {
  showSetup();
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
