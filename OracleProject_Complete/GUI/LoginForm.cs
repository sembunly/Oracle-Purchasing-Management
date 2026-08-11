using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace OracleProject
{
    public partial class LoginForm : Form
    {
        private Panel _loginCard;
        private Button _closeButton;
        private Label _productName;
        private Label _heroTitle;
        private Label _heroDescription;
        private Label _features;
        private Label _securityLabel;
        private bool _isLayingOut;

        public LoginForm()
        {
            InitializeComponent();
            ConfigureFullScreenLayout();
        }

        private void ConfigureFullScreenLayout()
        {
            SuspendLayout();
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            MinimumSize = new Size(1100, 680);
            BackColor = Color.FromArgb(242, 247, 252);
            AcceptButton = btnLogin;
            CancelButton = btnCancel;

            panelLeft.BackColor = Color.FromArgb(6, 22, 40);
            panelRight.BackColor = Color.FromArgb(242, 247, 252);
            panelRight.AutoScroll = true;

            CreateLoginCard();
            ConfigureBrandContent();
            ConfigureLoginControls();

            panelLeft.Paint += panelLeft_Paint;
            panelMain.Resize += responsivePanel_Resize;
            panelLeft.Resize += responsivePanel_Resize;
            panelRight.Resize += responsivePanel_Resize;
            Shown += LoginForm_Shown;

            PositionResponsiveLayout();
            ResumeLayout(true);
        }

        private void CreateLoginCard()
        {
            _loginCard = new Panel
            {
                BackColor = Color.White,
                BorderStyle = BorderStyle.None,
                Size = new Size(460, 660),
                TabIndex = 0
            };

            _productName = CreateLabel("PROCUREEASE", 10F, FontStyle.Bold, Color.FromArgb(20, 34, 54), ContentAlignment.MiddleCenter);
            _securityLabel = CreateLabel("Secure purchasing workspace", 9F, FontStyle.Regular, Color.FromArgb(111, 124, 145), ContentAlignment.MiddleCenter);

            Control[] loginControls =
            {
                pictureBoxLogo,
                _productName,
                lblWelcome,
                lblSignIn,
                lblUsername,
                txtUsername,
                lblPassword,
                txtPassword,
                chkShowPassword,
                btnLogin,
                btnCancel,
                _securityLabel,
                lblCopyright
            };

            foreach (Control control in loginControls)
            {
                control.Parent = _loginCard;
                _loginCard.Controls.Add(control);
            }

            panelRight.Controls.Add(_loginCard);

            _closeButton = new Button
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                BackColor = Color.FromArgb(242, 247, 252),
                Cursor = Cursors.Hand,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 18F, FontStyle.Regular),
                ForeColor = Color.FromArgb(72, 86, 109),
                Size = new Size(44, 44),
                TabStop = false,
                Text = "X",
                UseVisualStyleBackColor = false
            };
            _closeButton.FlatAppearance.BorderSize = 0;
            _closeButton.Click += btnCancel_Click;
            panelRight.Controls.Add(_closeButton);
            _closeButton.BringToFront();
        }

        private void ConfigureBrandContent()
        {
            lblAppTitle.Text = "PROCUREEASE";
            lblAppTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblAppTitle.ForeColor = Color.White;
            lblAppTitle.TextAlign = ContentAlignment.MiddleLeft;

            lblAppSubtitle.Text = "PURCHASING MANAGEMENT";
            lblAppSubtitle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblAppSubtitle.ForeColor = Color.FromArgb(137, 201, 235);
            lblAppSubtitle.TextAlign = ContentAlignment.MiddleLeft;

            _heroTitle = CreateLabel("PURCHASING\r\nMADE SIMPLE", 34F, FontStyle.Bold, Color.White, ContentAlignment.MiddleLeft);
            _heroDescription = CreateLabel(
                "Manage purchase orders, suppliers, products, stock and reports from one secure workspace.",
                12F,
                FontStyle.Regular,
                Color.FromArgb(201, 218, 235),
                ContentAlignment.MiddleLeft);
            _features = CreateLabel(
                "REQUEST  |  APPROVE  |  RECEIVE  |  REPORT",
                9.5F,
                FontStyle.Bold,
                Color.FromArgb(247, 184, 84),
                ContentAlignment.MiddleLeft);

            panelLeft.Controls.Add(_heroTitle);
            panelLeft.Controls.Add(_heroDescription);
            panelLeft.Controls.Add(_features);
            lblAppTitle.BringToFront();
            lblAppSubtitle.BringToFront();
            _heroTitle.BringToFront();
            _heroDescription.BringToFront();
            _features.BringToFront();
        }

        private void ConfigureLoginControls()
        {
            pictureBoxLogo.Size = new Size(84, 84);
            pictureBoxLogo.BackColor = Color.Transparent;

            lblWelcome.Text = "Welcome";
            lblWelcome.Font = new Font("Segoe UI", 23F, FontStyle.Bold);
            lblWelcome.ForeColor = Color.FromArgb(20, 34, 54);
            lblWelcome.TextAlign = ContentAlignment.MiddleCenter;

            lblSignIn.Text = "Please sign in to your purchasing account";
            lblSignIn.Font = new Font("Segoe UI", 10.5F, FontStyle.Regular);
            lblSignIn.ForeColor = Color.FromArgb(112, 124, 145);
            lblSignIn.TextAlign = ContentAlignment.MiddleCenter;

            lblUsername.Text = "Username *";
            lblPassword.Text = "Password *";
            lblUsername.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblPassword.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblUsername.ForeColor = Color.FromArgb(25, 37, 55);
            lblPassword.ForeColor = Color.FromArgb(25, 37, 55);

            txtUsername.Font = new Font("Segoe UI", 12F, FontStyle.Regular);
            txtPassword.Font = new Font("Segoe UI", 12F, FontStyle.Regular);
            txtPassword.PasswordChar = '*';

            chkShowPassword.Text = "Show password";
            chkShowPassword.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            chkShowPassword.ForeColor = Color.FromArgb(111, 124, 145);

            btnLogin.Text = "SIGN IN";
            btnLogin.BackColor = Color.FromArgb(45, 101, 181);
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Font = new Font("Segoe UI", 11F, FontStyle.Bold);

            btnCancel.Text = "EXIT APPLICATION";
            btnCancel.BackColor = Color.White;
            btnCancel.ForeColor = Color.FromArgb(90, 104, 126);
            btnCancel.FlatAppearance.BorderColor = Color.FromArgb(192, 205, 220);
            btnCancel.FlatAppearance.BorderSize = 1;
            btnCancel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);

            lblCopyright.Text = "Powered by Bunli Khit Rak Lean Daro.";
            lblCopyright.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblCopyright.ForeColor = Color.FromArgb(124, 137, 154);
            lblCopyright.TextAlign = ContentAlignment.MiddleCenter;
        }

        private static Label CreateLabel(string text, float size, FontStyle style, Color color, ContentAlignment alignment)
        {
            return new Label
            {
                AutoSize = false,
                BackColor = Color.Transparent,
                Font = new Font("Segoe UI", size, style),
                ForeColor = color,
                Text = text,
                TextAlign = alignment
            };
        }

        private void PositionResponsiveLayout()
        {
            if (_isLayingOut || _loginCard == null || panelMain.ClientSize.Width <= 0)
                return;

            _isLayingOut = true;
            try
            {
                int totalWidth = panelMain.ClientSize.Width;
                int desiredLeftWidth = Math.Max(620, (int)(totalWidth * 0.66));
                panelLeft.Width = Math.Min(desiredLeftWidth, Math.Max(460, totalWidth - 420));

                int leftPad = Math.Max(58, panelLeft.ClientSize.Width / 14);
                lblAppTitle.Bounds = new Rectangle(leftPad, 70, panelLeft.ClientSize.Width - leftPad * 2, 34);
                lblAppSubtitle.Bounds = new Rectangle(leftPad, 104, panelLeft.ClientSize.Width - leftPad * 2, 26);

                int heroTop = Math.Max(190, panelLeft.ClientSize.Height / 3 - 25);
                _heroTitle.Bounds = new Rectangle(leftPad, heroTop, panelLeft.ClientSize.Width - leftPad * 2, 128);
                _heroDescription.Bounds = new Rectangle(leftPad, heroTop + 148, Math.Min(620, panelLeft.ClientSize.Width - leftPad * 2), 70);
                _features.Bounds = new Rectangle(leftPad, panelLeft.ClientSize.Height - 112, panelLeft.ClientSize.Width - leftPad * 2, 32);

                int cardWidth = Math.Min(470, Math.Max(360, panelRight.ClientSize.Width - 96));
                int cardHeight = 660;
                int cardLeft = Math.Max(36, (panelRight.ClientSize.Width - cardWidth) / 2);
                int cardTop = Math.Max(42, (panelRight.ClientSize.Height - cardHeight) / 2);

                _loginCard.Bounds = new Rectangle(cardLeft, cardTop, cardWidth, cardHeight);
                panelRight.AutoScrollMinSize = new Size(cardWidth + 72, cardHeight + 84);

                int contentLeft = 42;
                int contentWidth = cardWidth - contentLeft * 2;
                pictureBoxLogo.Location = new Point((cardWidth - pictureBoxLogo.Width) / 2, 30);
                _productName.Bounds = new Rectangle(contentLeft, 112, contentWidth, 26);
                lblWelcome.Bounds = new Rectangle(contentLeft, 146, contentWidth, 48);
                lblSignIn.Bounds = new Rectangle(contentLeft, 199, contentWidth, 30);
                lblUsername.Bounds = new Rectangle(contentLeft, 262, contentWidth, 22);
                txtUsername.Bounds = new Rectangle(contentLeft, 290, contentWidth, 34);
                lblPassword.Bounds = new Rectangle(contentLeft, 342, contentWidth, 22);
                txtPassword.Bounds = new Rectangle(contentLeft, 370, contentWidth, 34);
                chkShowPassword.Bounds = new Rectangle(contentLeft, 414, contentWidth, 25);
                btnLogin.Bounds = new Rectangle(contentLeft, 458, contentWidth, 52);
                btnCancel.Bounds = new Rectangle(contentLeft, 524, contentWidth, 40);
                _securityLabel.Bounds = new Rectangle(contentLeft, 584, contentWidth, 24);
                lblCopyright.Bounds = new Rectangle(contentLeft, 618, contentWidth, 24);

                _closeButton.Location = new Point(Math.Max(8, panelRight.ClientSize.Width - _closeButton.Width - 16), 14);

                ApplyRoundedRegion(_loginCard, 8);
                ApplyRoundedRegion(btnLogin, 6);
                ApplyRoundedRegion(btnCancel, 6);
                panelLeft.Invalidate();
                pictureBoxLogo.Invalidate();
            }
            finally
            {
                _isLayingOut = false;
            }
        }

        private static void ApplyRoundedRegion(Control control, int radius)
        {
            if (control.Width <= 0 || control.Height <= 0)
                return;

            Rectangle bounds = new Rectangle(0, 0, control.Width, control.Height);
            int diameter = radius * 2;

            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddArc(bounds.Left, bounds.Top, diameter, diameter, 180, 90);
                path.AddArc(bounds.Right - diameter, bounds.Top, diameter, diameter, 270, 90);
                path.AddArc(bounds.Right - diameter, bounds.Bottom - diameter, diameter, diameter, 0, 90);
                path.AddArc(bounds.Left, bounds.Bottom - diameter, diameter, diameter, 90, 90);
                path.CloseFigure();
                Region oldRegion = control.Region;
                control.Region = new Region(path);
                if (oldRegion != null)
                    oldRegion.Dispose();
            }
        }

        private void responsivePanel_Resize(object sender, EventArgs e)
        {
            PositionResponsiveLayout();
        }

        private void LoginForm_Shown(object sender, EventArgs e)
        {
            PositionResponsiveLayout();
            txtUsername.Focus();
        }

        private void panelLeft_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle area = panelLeft.ClientRectangle;
            if (area.Width <= 0 || area.Height <= 0)
                return;

            using (LinearGradientBrush background = new LinearGradientBrush(
                       area,
                       Color.FromArgb(5, 18, 34),
                       Color.FromArgb(14, 50, 82),
                       LinearGradientMode.ForwardDiagonal))
            {
                g.FillRectangle(background, area);
            }

            DrawHeroGrid(g, area);
            DrawProcurementScene(g, area);
        }

        private static void DrawHeroGrid(Graphics g, Rectangle area)
        {
            using (Pen gridPen = new Pen(Color.FromArgb(26, 82, 122), 1F))
            {
                for (int x = 0; x < area.Width; x += 56)
                    g.DrawLine(gridPen, x, 0, x, area.Height);

                for (int y = 0; y < area.Height; y += 56)
                    g.DrawLine(gridPen, 0, y, area.Width, y);
            }
        }

        private static void DrawProcurementScene(Graphics g, Rectangle area)
        {
            int baseY = area.Height - 220;
            int right = area.Width - Math.Max(70, area.Width / 12);
            int panelWidth = Math.Min(360, Math.Max(230, area.Width / 3));

            using (SolidBrush glow = new SolidBrush(Color.FromArgb(34, 69, 153, 214)))
            using (SolidBrush screen = new SolidBrush(Color.FromArgb(210, 12, 32, 55)))
            using (Pen accent = new Pen(Color.FromArgb(90, 75, 187, 223), 2F))
            using (Pen gold = new Pen(Color.FromArgb(190, 247, 184, 84), 2F))
            {
                g.FillEllipse(glow, right - panelWidth - 80, baseY - 95, panelWidth + 150, 240);

                Rectangle monitor = new Rectangle(right - panelWidth, baseY - 30, panelWidth, 135);
                g.FillRectangle(screen, monitor);
                g.DrawRectangle(accent, monitor);
                g.DrawLine(accent, monitor.Left + 30, monitor.Top + 34, monitor.Right - 34, monitor.Top + 34);
                g.DrawLine(accent, monitor.Left + 30, monitor.Top + 70, monitor.Right - 52, monitor.Top + 70);
                g.DrawLine(accent, monitor.Left + 30, monitor.Top + 104, monitor.Right - 82, monitor.Top + 104);
                g.DrawLine(gold, monitor.Left + 34, monitor.Top + 36, monitor.Left + 86, monitor.Top + 78);
                g.DrawLine(gold, monitor.Left + 86, monitor.Top + 78, monitor.Left + 154, monitor.Top + 46);
                g.DrawLine(gold, monitor.Left + 154, monitor.Top + 46, monitor.Left + 230, monitor.Top + 92);

                Rectangle document = new Rectangle(monitor.Left + panelWidth / 2 - 52, baseY - 140, 104, 138);
                using (SolidBrush paper = new SolidBrush(Color.FromArgb(235, 241, 247)))
                using (Pen paperPen = new Pen(Color.FromArgb(145, 174, 203), 2F))
                {
                    g.FillRectangle(paper, document);
                    g.DrawRectangle(paperPen, document);
                    g.DrawLine(paperPen, document.Left + 18, document.Top + 34, document.Right - 18, document.Top + 34);
                    g.DrawLine(paperPen, document.Left + 18, document.Top + 62, document.Right - 18, document.Top + 62);
                    g.DrawLine(paperPen, document.Left + 18, document.Top + 90, document.Right - 34, document.Top + 90);
                    g.DrawEllipse(gold, document.Right - 38, document.Bottom - 42, 24, 24);
                }
            }
        }

        private void pictureBoxLogo_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle outer = new Rectangle(4, 4, pictureBoxLogo.Width - 8, pictureBoxLogo.Height - 8);
            using (LinearGradientBrush brush = new LinearGradientBrush(
                       outer,
                       Color.FromArgb(36, 118, 216),
                       Color.FromArgb(57, 176, 196),
                       LinearGradientMode.ForwardDiagonal))
            using (Pen ring = new Pen(Color.FromArgb(235, 246, 255), 3F))
            {
                g.FillEllipse(brush, outer);
                g.DrawEllipse(ring, outer);
            }

            float scaleX = pictureBoxLogo.Width / 100F;
            float scaleY = pictureBoxLogo.Height / 100F;
            PointF[] cart =
            {
                new PointF(28 * scaleX, 34 * scaleY),
                new PointF(34 * scaleX, 62 * scaleY),
                new PointF(72 * scaleX, 62 * scaleY),
                new PointF(78 * scaleX, 40 * scaleY),
                new PointF(31 * scaleX, 40 * scaleY)
            };

            using (Pen pen = new Pen(Color.White, 3F))
            using (SolidBrush wheel = new SolidBrush(Color.White))
            {
                g.DrawLine(pen, 20 * scaleX, 34 * scaleY, 29 * scaleX, 34 * scaleY);
                g.DrawLines(pen, cart);
                g.DrawLine(pen, 43 * scaleX, 48 * scaleY, 66 * scaleX, 48 * scaleY);
                g.FillEllipse(wheel, 38 * scaleX, 67 * scaleY, 9 * scaleX, 9 * scaleY);
                g.FillEllipse(wheel, 62 * scaleX, 67 * scaleY, 9 * scaleX, 9 * scaleY);
            }
        }

        private void chkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.PasswordChar = chkShowPassword.Checked ? '\0' : '*';
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Please enter your username and password.", "Login Required",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string displayName;
            string roleCode;
            string loginError;
            if (!OracleDb.AuthenticateAppUser(
                    txtUsername.Text.Trim(),
                    txtPassword.Text,
                    out displayName,
                    out roleCode,
                    out loginError))
            {
                MessageBox.Show(
                    "Application login failed.\n\n"
                    + loginError,
                    "Login Failed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }
            Dashboard dashboard = new Dashboard(displayName, roleCode);
            dashboard.FormClosed += delegate { Close(); };
            dashboard.Show();
            this.Hide();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
