using System;
using System.Drawing;
using System.Windows.Forms;

namespace OracleProject
{
    /// <summary>
    /// Form 001 tab: placeholder/starter page.
    /// </summary>
    internal sealed class Form001Tab : DashboardTabBase
    {
        private Button btnAdd, btnEdit, btnDelete;

        public Form001Tab()
        {
            TabTitle = "Form 001";
            RequiredPermission = "FORM001_VIEW";
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            BackColor = Color.FromArgb(247, 250, 252);
            Dock = DockStyle.Fill;

            // Toolbar
            var toolbar = new Panel
            {
                BackColor = Color.White,
                Dock = DockStyle.Top,
                Height = 90
            };

            // Title
            var title = new Label
            {
                AutoSize = false,
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(31, 41, 55),
                Location = new Point(20, 16),
                Size = new Size(360, 34),
                Text = "Form 001"
            };

            var subtitle = new Label
            {
                AutoSize = false,
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(113, 128, 150),
                Location = new Point(22, 52),
                Size = new Size(600, 24),
                Text = "Starter page. Rename this module when your real form is ready."
            };

            // Buttons
            btnAdd = CreatePageButton("Add", Color.FromArgb(49, 130, 206), 640, 20);
            btnAdd.Click += (s, e) => RunAction("FORM001_ADD", "Add");

            btnEdit = CreatePageButton("Edit", Color.FromArgb(72, 187, 120), 760, 20);
            btnEdit.Click += (s, e) => RunAction("FORM001_EDIT", "Edit");

            btnDelete = CreatePageButton("Delete", Color.FromArgb(245, 101, 101), 880, 20);
            btnDelete.Click += (s, e) => RunAction("FORM001_DELETE", "Delete");

            toolbar.Controls.Add(title);
            toolbar.Controls.Add(subtitle);
            toolbar.Controls.Add(btnAdd);
            toolbar.Controls.Add(btnEdit);
            toolbar.Controls.Add(btnDelete);

            // Placeholder content
            var placeholder = new Label
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                AutoSize = false,
                BackColor = Color.White,
                Font = new Font("Segoe UI", 12F),
                ForeColor = Color.FromArgb(75, 85, 99),
                Location = new Point(30, 145),
                Size = new Size(900, 360),
                Text = "This is your new Form 001 workspace.\r\n\r\nPermissions ready:\r\nFORM001_VIEW, FORM001_ADD, FORM001_EDIT, FORM001_DELETE",
                TextAlign = ContentAlignment.MiddleCenter
            };

            Controls.Add(placeholder);
            Controls.Add(toolbar);
        }

        public override void ApplyPermissions()
        {
            btnAdd.Visible = HasPermission("FORM001_ADD");
            btnEdit.Visible = HasPermission("FORM001_EDIT");
            btnDelete.Visible = HasPermission("FORM001_DELETE");
        }

        private void RunAction(string permissionCode, string actionName)
        {
            if (!RequirePermission(permissionCode))
                return;

            MessageBox.Show(this,
                "Form 001 " + actionName + " action is ready. Connect your real form code here.",
                "Form 001",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
    }
}
