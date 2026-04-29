const { contextBridge, ipcRenderer } = require('electron');

contextBridge.exposeInMainWorld('api', {
  saveUrl: (url) => ipcRenderer.invoke('save-url', url),
  getUrl: () => ipcRenderer.invoke('get-url')
});
