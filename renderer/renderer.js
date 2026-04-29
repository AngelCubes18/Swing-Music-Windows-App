async function validate(url) {
  url = url.trim();

  if (!url) {
    throw new Error('Enter the Swing Music server URL.');
  }

  if (!url.startsWith('http')) {
    url = 'http://' + url;
  }

  try {
    const res = await fetch(url, { method: 'GET' });

    if (!res.ok) {
      throw new Error("Server error: " + res.status);
    }

    const text = await res.text();

    // Basic detection (adjust if needed)
    if (!text.toLowerCase().includes("swing")) {
      throw new Error("Not a valid Swing Music panel");
    }

    return url;

  } catch (err) {
    throw new Error(err.message || 'Unable to connect to this server.');
  }
}

async function submit() {
  const inputEl = document.getElementById('url');
  const errorEl = document.getElementById('error');
  const buttonEl = document.getElementById('connect');

  errorEl.innerText = '';
  buttonEl.disabled = true;
  buttonEl.innerText = 'Connecting...';

  try {
    const validUrl = await validate(inputEl.value);
    window.api.saveUrl(validUrl);
  } catch (e) {
    errorEl.innerText = e.message;
    buttonEl.disabled = false;
    buttonEl.innerText = 'Connect';
  }
}

window.addEventListener('DOMContentLoaded', async () => {
  const form = document.getElementById('server-form');
  const inputEl = document.getElementById('url');

  inputEl.value = await window.api.getUrl();
  inputEl.focus();
  inputEl.select();

  form.addEventListener('submit', (event) => {
    event.preventDefault();
    submit();
  });
});
