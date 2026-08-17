using System;
using System.Drawing;
using System.Windows.Forms;

namespace OracleProject
{
    /// <summary>
    /// Settings tab: hosts the PermissionForm.
    /// </summary>
    internal sealed class SettingsTab : DashboardTabBase
    {
        private PermissionForm permissionForm;

        public SettingsTab()
        {
            TabTitle = "Settings";
            RequiredPermission = "SETTINGS_VIEW";
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            BackColor = Color.FromArgb(247, 250, 252);
            Dock = DockStyle.Fill;

            // The PermissionForm will be embedded here
            // It gets created in OnActivated when we have Context
        }

        public override void ApplyPermissions()
        {
            // PermissionForm handles its own permissions
        }

        public override void OnActivated()
        {
            if (permissionForm != null && !permissionForm.IsDisposed)
                return;

            // Create and embed the PermissionForm
            bool canManage = Context != null && Context.HasPermission("SETTINGS_MANAGE");
            permissionForm = new PermissionForm(canManage);
            permissionForm.ConfigureForEmbedded();
            permissionForm.Dock = DockStyle.Fill;
            permissionForm.TopLevel = false;
            permissionForm.FormBorderStyle = FormBorderStyle.None;
            
            Controls.Clear();
            Controls.Add(permissionForm);
            permissionForm.Show();
        }

        public override void RefreshData()
        {
            if (permissionForm != null && !permissionForm.IsDisposed)
                permissionForm.RefreshPermissionData();
        }
    }
}
