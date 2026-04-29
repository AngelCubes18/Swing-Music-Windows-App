const { contextBridge, ipcRenderer } = require('electron');

contextBridge.exposeInMainWorld('api', {
  saveUrl: (url) => ipcRenderer.invoke('save-url', url),
  getUrl: () => ipcRenderer.invoke('get-url'),
  changeUrl: () => ipcRenderer.invoke('change-url')
});

function installShellControls() {
  if (window.location.protocol === 'file:') {
    return;
  }

  const style = document.createElement('style');
  style.textContent = `
    #swing-shell-change-url {
      position: fixed;
      top: 7px;
      left: 10px;
      z-index: 2147483647;
      height: 28px;
      padding: 0 12px;
      border: 1px solid rgba(255, 255, 255, 0.16);
      border-radius: 999px;
      background: rgba(0, 0, 0, 0.18);
      color: rgba(255, 255, 255, 0.72);
      cursor: pointer;
      font: 600 12px/1 system-ui, -apple-system, BlinkMacSystemFont, "Segoe UI", sans-serif;
      opacity: 0.18;
      backdrop-filter: blur(12px);
      -webkit-app-region: no-drag;
      transition: opacity 140ms ease, background 140ms ease, color 140ms ease, transform 140ms ease;
    }

    #swing-shell-change-url:hover,
    #swing-shell-change-url:focus-visible {
      opacity: 1;
      background: rgba(0, 0, 0, 0.62);
      color: #fff;
      outline: none;
      transform: translateY(1px);
    }
  `;

  const button = document.createElement('button');
  button.id = 'swing-shell-change-url';
  button.type = 'button';
  button.title = 'Change server URL (Ctrl+L)';
  button.textContent = 'Change URL';
  button.addEventListener('click', () => ipcRenderer.invoke('change-url'));

  document.documentElement.appendChild(style);
  document.body.appendChild(button);
}

window.addEventListener('DOMContentLoaded', installShellControls);
