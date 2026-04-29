using Microsoft.Web.WebView2.WinForms;
using System;
using System.Drawing;
using System.Net.Http;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SwingMusic
{
    internal sealed class MainForm : Form
    {
        private const int TitleBarHeight = 34;
        private readonly Panel titleBar;
        private readonly Panel contentPanel;
        private Button changeUrlButton;
        private Button minimizeButton;
        private Button maximizeButton;
        private Button closeButton;
        private WebView2 webView;
        private Panel setupPanel;
        private TextBox urlTextBox;
        private Label errorLabel;
        private Button connectButton;

        public MainForm()
        {
            Text = "Swing Music";
            Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            BackColor = Color.Black;
            ForeColor = Color.White;
            MinimumSize = new Size(880, 620);
            Size = new Size(1200, 800);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.None;
            KeyPreview = true;

            titleBar = BuildTitleBar();
            contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Black
            };

            Controls.Add(contentPanel);
            Controls.Add(titleBar);

            KeyDown += MainForm_KeyDown;
            Load += MainForm_Load;
        }

        private Panel BuildTitleBar()
        {
            var bar = new Panel
            {
                Dock = DockStyle.Top,
                Height = TitleBarHeight,
                BackColor = Color.FromArgb(12, 12, 12)
            };

            changeUrlButton = BuildChromeButton("Change URL", 96);
            changeUrlButton.Left = 8;
            changeUrlButton.Top = 3;
            changeUrlButton.ForeColor = Color.FromArgb(190, 255, 255, 255);
            changeUrlButton.BackColor = Color.FromArgb(26, 26, 26);
            changeUrlButton.Click += (sender, args) => ShowSetup();

            closeButton = BuildChromeButton("X", 46);
            closeButton.Click += (sender, args) => Close();

            maximizeButton = BuildChromeButton("□", 46);
            maximizeButton.Click += (sender, args) => ToggleMaximize();

            minimizeButton = BuildChromeButton("_", 46);
            minimizeButton.Click += (sender, args) => WindowState = FormWindowState.Minimized;

            bar.Controls.Add(changeUrlButton);
            bar.Controls.Add(minimizeButton);
            bar.Controls.Add(maximizeButton);
            bar.Controls.Add(closeButton);
            bar.MouseDown += TitleBar_MouseDown;
            bar.Resize += (sender, args) => LayoutWindowButtons();

            return bar;
        }

        private static Button BuildChromeButton(string text, int width)
        {
            return new Button
            {
                Width = width,
                Height = 28,
                Text = text,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.Black,
                ForeColor = Color.White,
                TabStop = false,
                Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point)
            };
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            LayoutWindowButtons();

            var savedUrl = ConfigStore.LoadUrl();
            if (string.IsNullOrWhiteSpace(savedUrl))
            {
                ShowSetup();
                return;
            }

            await ShowWebView(savedUrl);
        }

        private void LayoutWindowButtons()
        {
            closeButton.Left = titleBar.Width - closeButton.Width;
            maximizeButton.Left = closeButton.Left - maximizeButton.Width;
            minimizeButton.Left = maximizeButton.Left - minimizeButton.Width;
            closeButton.Top = 3;
            maximizeButton.Top = 3;
            minimizeButton.Top = 3;
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.L)
            {
                e.SuppressKeyPress = true;
                ShowSetup();
            }
        }

        private async Task ShowWebView(string url)
        {
            contentPanel.Controls.Clear();
            setupPanel = null;

            webView = new WebView2
            {
                Dock = DockStyle.Fill,
                DefaultBackgroundColor = Color.Black
            };

            contentPanel.Controls.Add(webView);
            await webView.EnsureCoreWebView2Async();
            webView.Source = new Uri(url);
        }

        private void ShowSetup()
        {
            contentPanel.Controls.Clear();

            setupPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Black
            };

            var card = new Panel
            {
                Width = 440,
                Height = 410,
                BackColor = Color.Black
            };

            var logo = new PictureBox
            {
                Width = 88,
                Height = 88,
                SizeMode = PictureBoxSizeMode.Zoom,
                Image = LoadLogoImage()
            };

            var heading = new Label
            {
                AutoSize = false,
                Text = "Swing Music",
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 28F, FontStyle.Bold, GraphicsUnit.Point),
                Height = 58
            };

            var subtitle = new Label
            {
                AutoSize = false,
                Text = "Connect to your self-hosted web panel.",
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.FromArgb(170, 170, 170),
                Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point),
                Height = 30
            };

            urlTextBox = new TextBox
            {
                Height = 34,
                Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point),
                BackColor = Color.FromArgb(18, 18, 18),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Text = ConfigStore.LoadUrl()
            };

            connectButton = new Button
            {
                Height = 44,
                Text = "Connect",
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.White,
                ForeColor = Color.Black,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point)
            };
            connectButton.Click += async (sender, args) => await Connect();

            errorLabel = new Label
            {
                AutoSize = false,
                Height = 42,
                ForeColor = Color.FromArgb(255, 143, 143),
                Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point)
            };

            setupPanel.Controls.Add(card);
            card.Controls.Add(logo);
            card.Controls.Add(heading);
            card.Controls.Add(subtitle);
            card.Controls.Add(urlTextBox);
            card.Controls.Add(connectButton);
            card.Controls.Add(errorLabel);

            setupPanel.Resize += (sender, args) => LayoutSetup(card, logo, heading, subtitle);
            LayoutSetup(card, logo, heading, subtitle);
            contentPanel.Controls.Add(setupPanel);

            urlTextBox.Focus();
            urlTextBox.SelectAll();
        }

        private void LayoutSetup(Panel card, PictureBox logo, Label heading, Label subtitle)
        {
            card.Left = Math.Max(0, (setupPanel.Width - card.Width) / 2);
            card.Top = Math.Max(0, (setupPanel.Height - card.Height) / 2);

            logo.Left = (card.Width - logo.Width) / 2;
            logo.Top = 0;

            heading.Left = 0;
            heading.Top = logo.Bottom + 18;
            heading.Width = card.Width;

            subtitle.Left = 0;
            subtitle.Top = heading.Bottom + 2;
            subtitle.Width = card.Width;

            urlTextBox.Left = 0;
            urlTextBox.Top = subtitle.Bottom + 32;
            urlTextBox.Width = card.Width;

            connectButton.Left = 0;
            connectButton.Top = urlTextBox.Bottom + 14;
            connectButton.Width = card.Width;

            errorLabel.Left = 0;
            errorLabel.Top = connectButton.Bottom + 12;
            errorLabel.Width = card.Width;
        }

        private async Task Connect()
        {
            errorLabel.Text = string.Empty;
            connectButton.Enabled = false;
            connectButton.Text = "Connecting...";

            try
            {
                var validUrl = await ValidateUrl(urlTextBox.Text);
                ConfigStore.SaveUrl(validUrl);
                await ShowWebView(validUrl);
            }
            catch (Exception ex)
            {
                errorLabel.Text = ex.Message;
                connectButton.Enabled = true;
                connectButton.Text = "Connect";
            }
        }

        private static async Task<string> ValidateUrl(string rawUrl)
        {
            var url = (rawUrl ?? string.Empty).Trim();
            if (url.Length == 0)
            {
                throw new InvalidOperationException("Enter the Swing Music server URL.");
            }

            if (!url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) &&
                !url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                url = "http://" + url;
            }

            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(8);
                var html = await client.GetStringAsync(url);
                if (html.IndexOf("swing", StringComparison.OrdinalIgnoreCase) < 0)
                {
                    throw new InvalidOperationException("This does not look like a Swing Music web panel.");
                }
            }

            return url;
        }

        private void ToggleMaximize()
        {
            WindowState = WindowState == FormWindowState.Maximized
                ? FormWindowState.Normal
                : FormWindowState.Maximized;
        }

        private void TitleBar_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                return;
            }

            ReleaseCapture();
            SendMessage(Handle, 0xA1, new IntPtr(0x2), IntPtr.Zero);
        }

        protected override void WndProc(ref Message m)
        {
            const int wmNchittest = 0x84;
            const int htLeft = 10;
            const int htRight = 11;
            const int htTop = 12;
            const int htTopLeft = 13;
            const int htTopRight = 14;
            const int htBottom = 15;
            const int htBottomLeft = 16;
            const int htBottomRight = 17;

            base.WndProc(ref m);

            if (m.Msg != wmNchittest || WindowState == FormWindowState.Maximized)
            {
                return;
            }

            var cursor = PointToClient(Cursor.Position);
            const int grip = 7;

            if (cursor.X <= grip && cursor.Y <= grip)
            {
                m.Result = new IntPtr(htTopLeft);
            }
            else if (cursor.X >= ClientSize.Width - grip && cursor.Y <= grip)
            {
                m.Result = new IntPtr(htTopRight);
            }
            else if (cursor.X <= grip && cursor.Y >= ClientSize.Height - grip)
            {
                m.Result = new IntPtr(htBottomLeft);
            }
            else if (cursor.X >= ClientSize.Width - grip && cursor.Y >= ClientSize.Height - grip)
            {
                m.Result = new IntPtr(htBottomRight);
            }
            else if (cursor.X <= grip)
            {
                m.Result = new IntPtr(htLeft);
            }
            else if (cursor.X >= ClientSize.Width - grip)
            {
                m.Result = new IntPtr(htRight);
            }
            else if (cursor.Y <= grip)
            {
                m.Result = new IntPtr(htTop);
            }
            else if (cursor.Y >= ClientSize.Height - grip)
            {
                m.Result = new IntPtr(htBottom);
            }
        }

        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        private static Image LoadLogoImage()
        {
            var assembly = Assembly.GetExecutingAssembly();
            foreach (var resourceName in assembly.GetManifestResourceNames())
            {
                if (!resourceName.EndsWith("swingmusiclogo.png", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                using (var stream = assembly.GetManifestResourceStream(resourceName))
                {
                    if (stream != null)
                    {
                        return Image.FromStream(stream);
                    }
                }
            }

            return null;
        }
    }
}
