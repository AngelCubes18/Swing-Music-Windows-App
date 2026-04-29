const fs = require('fs');
const path = require('path');

const configPath = path.join(
  process.env.APPDATA,
  'SwingMusicApp',
  'config.json'
);

function ensureDir() {
  const dir = path.dirname(configPath);
  if (!fs.existsSync(dir)) {
    fs.mkdirSync(dir, { recursive: true });
  }
}

function loadConfig() {
  try {
    return JSON.parse(fs.readFileSync(configPath));
  } catch {
    return null;
  }
}

function saveConfig(data) {
  ensureDir();
  fs.writeFileSync(configPath, JSON.stringify(data));
}

module.exports = { loadConfig, saveConfig };