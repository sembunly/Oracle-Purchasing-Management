using System;
using System.Drawing;
using System.Windows.Forms;

namespace OracleProject
{
    public partial class Form1 : Form
    {
        public Form1()
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

            string connectionError;
            if (!OracleDb.TestConnection(txtUsername.Text.Trim(), txtPassword.Text, out connectionError))
            {
                MessageBox.Show(
                    "Oracle connection failed. Check App.config, Oracle service/PDB, username and password.\n\n"
                    + connectionError,
                    "Database Login Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            // The classroom schema has no password column for application employees;
            // verify the configured Oracle account, then use the typed name for display.
            Dashboard dashboard = new Dashboard(txtUsername.Text);
            dashboard.Show();
            this.Hide();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
