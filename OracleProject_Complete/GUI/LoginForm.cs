using System;
using System.Drawing;
using System.Windows.Forms;

namespace OracleProject
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void pictureBoxLogo_Paint(object sender, PaintEventArgs e)
        {
            // Draw a simple shopping cart / box icon as logo
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Outer circle
            using (Pen pen = new Pen(Color.White, 3))
            using (SolidBrush brush = new SolidBrush(Color.FromArgb(41, 128, 185)))
            {
                g.FillEllipse(brush, 5, 5, 90, 90);
                g.DrawEllipse(pen, 5, 5, 90, 90);
            }

            // Shopping cart icon (simplified)
            using (Pen pen = new Pen(Color.White, 3))
            {
                // Cart handle
                g.DrawLine(pen, 20, 30, 30, 30);
                // Cart body
                Point[] cart = { new Point(30, 30), new Point(35, 60), new Point(75, 60), new Point(80, 35), new Point(30, 35) };
                g.DrawLines(pen, cart);
                // Wheels
                g.FillEllipse(Brushes.White, 38, 63, 10, 10);
                g.FillEllipse(Brushes.White, 62, 63, 10, 10);
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
                    "Application login failed. Make sure APP_USERS is installed and the credentials are correct.\n\n"
                    + loginError,
                    "Login Failed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }
            Dashboard dashboard = new Dashboard(displayName);
            dashboard.Show();
            this.Hide();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
