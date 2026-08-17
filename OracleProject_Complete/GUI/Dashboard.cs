using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace OracleProject
{
    /// <summary>
    /// Main dashboard form - now a thin shell that hosts tab UserControls.
    /// Handles navigation, sidebar, top bar, permissions, and global refresh.
    /// </summary>
    public partial class Dashboard : Form
    {
        private readonly DashboardContext _context;
        private readonly Dictionary<string, DashboardTabBase> _tabs = new Dictionary<string, DashboardTabBase>();
        private DashboardTabBase _activeTab;
        private Button _activeNavButton;

        // Dynamic nav buttons
        private Button _btnNavUsers;
        private Button _btnNavForm001;

        public Dashboard(string username = "Admin", string roleCode = "ADMIN")
        {
            _context = new DashboardContext(username, roleCode);
            InitializeComponent();
            InitializeTabs();
            CreateDynamicNavButtons();
            CreateGlobalRefreshButton();
        }

        private void InitializeTabs()
        {
            // Create all tab UserControls
            _tabs["Overview"] = new OverviewTab();
            _tabs["Orders"] = new OrdersTab();
            _tabs["Suppliers"] = new SuppliersTab();
            _tabs["Products"] = new ProductsTab();
            _tabs["Reports"] = new ReportsTab();
            _tabs["Users"] = new UsersTab();
            _tabs["Form001"] = new Form001Tab();
            _tabs["Settings"] = new SettingsTab();

            // Set context on all tabs
            foreach (var tab in _tabs.Values)
            {
                tab.Context = _context;
                tab.Visible = false;
                tab.Dock = DockStyle.Fill;
                panelContent.Controls.Add(tab);
            }
        }

        private void CreateDynamicNavButtons()
        {
            // Users nav button
            _btnNavUsers = new Button
            {
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(160, 174, 192),
                Location = btnNavSettings.Location,
                Margin = btnNavSettings.Margin,
                Name = "btnNavUsers",
                Size = btnNavSettings.Size,
                TabIndex = btnNavSettings.TabIndex,
                Text = "   👤  Users",
                TextAlign = ContentAlignment.MiddleLeft,
                UseVisualStyleBackColor = false
            };
            _btnNavUsers.FlatAppearance.BorderSize = 0;
            _btnNavUsers.FlatAppearance.MouseOverBackColor = Color.FromArgb(45, 55, 72);
            _btnNavUsers.Click += (s, e) => SetActiveTab("Users", _btnNavUsers);
            panelSidebar.Controls.Add(_btnNavUsers);
            _btnNavUsers.BringToFront();

            // Form001 nav button
            _btnNavForm001 = new Button
            {
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(160, 174, 192),
                Location = btnNavSettings.Location,
                Margin = btnNavSettings.Margin,
                Name = "btnNavForm001",
                Size = btnNavSettings.Size,
                TabIndex = btnNavSettings.TabIndex,
                Text = "   001  Form 001",
                TextAlign = ContentAlignment.MiddleLeft,
                UseVisualStyleBackColor = false
            };
            _btnNavForm001.FlatAppearance.BorderSize = 0;
            _btnNavForm001.FlatAppearance.MouseOverBackColor = Color.FromArgb(45, 55, 72);
            _btnNavForm001.Click += (s, e) => SetActiveTab("Form001", _btnNavForm001);
            panelSidebar.Controls.Add(_btnNavForm001);
            _btnNavForm001.BringToFront();
        }

        private void CreateGlobalRefreshButton()
        {
            var btnRefresh = new Button
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                BackColor = Color.FromArgb(107, 114, 128),
                Cursor = Cursors.Hand,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(1000, 32),
                Size = new Size(120, 40),
                Text = "🔄 Refresh",
                TextAlign = ContentAlignment.MiddleCenter,
                UseVisualStyleBackColor = false
            };
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.Click += (s, e) =>
            {
                if (_activeTab != null)
                    _activeTab.RefreshData();
            };
            panelTopBar.Controls.Add(btnRefresh);
            btnRefresh.BringToFront();
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {
            lblUserGreeting.Text = "Welcome, " + _context.CurrentUser + " (" + _context.CurrentRoleCode + ")";
            timerClock.Start();
            timerClock_Tick(null, null);

            try
            {
                LoadPermissions();
                ApplyPermissions();
                OpenFirstAllowedTab();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Oracle database could not be loaded.\n\n" + ex.Message,
                    "Oracle Connection Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void LoadPermissions()
        {
            try
            {
                _context.AllowedPermissions = OracleDb.GetAllowedPermissions(_context.CurrentRoleCode);
            }
            catch (Oracle.ManagedDataAccess.Client.OracleException ex) when (ex.Number == 942)
            {
                if (string.Equals(_context.CurrentRoleCode, "ADMIN", StringComparison.OrdinalIgnoreCase))
                    _context.AllowAllKnownPermissions();
                else
                    _context.AllowedPermissions.Clear();
            }
        }

        private void ApplyPermissions()
        {
            // Apply to nav buttons
            btnNavOverview.Visible = _context.HasPermission("OVERVIEW_VIEW");
            btnNavOrders.Visible = _context.HasPermission("ORDERS_VIEW");
            btnNavSuppliers.Visible = _context.HasPermission("SUPPLIERS_VIEW");
            btnNavProducts.Visible = _context.HasPermission("PRODUCTS_VIEW");
            btnNavReports.Visible = _context.HasPermission("REPORTS_VIEW");
            _btnNavUsers.Visible = _context.HasPermission("USERS_VIEW");
            _btnNavForm001.Visible = _context.HasPermission("FORM001_VIEW");
            btnNavSettings.Visible = _context.HasPermission("SETTINGS_VIEW") || _context.HasPermission("SETTINGS_MANAGE");

            // Apply to each tab's internal controls
            foreach (var tab in _tabs.Values)
            {
                tab.ApplyPermissions();
            }

            // Layout sidebar
            LayoutSidebarNavigation();
        }

        private void LayoutSidebarNavigation()
        {
            Button[] navButtons =
            {
                btnNavOverview,
                btnNavOrders,
                btnNavSuppliers,
                btnNavProducts,
                btnNavReports,
                _btnNavUsers,
                _btnNavForm001,
                btnNavSettings
            };

            int top = btnNavOverview.Top;
            foreach (Button button in navButtons)
            {
                if (!button.Visible)
                    continue;

                button.Top = top;
                button.Left = 0;
                button.Width = panelSidebar.ClientSize.Width;
                top += button.Height;
            }
        }

        private void OpenFirstAllowedTab()
        {
            if (btnNavOverview.Visible)
                SetActiveTab("Overview", btnNavOverview);
            else if (btnNavOrders.Visible)
                SetActiveTab("Orders", btnNavOrders);
            else if (btnNavSuppliers.Visible)
                SetActiveTab("Suppliers", btnNavSuppliers);
            else if (btnNavProducts.Visible)
                SetActiveTab("Products", btnNavProducts);
            else if (btnNavReports.Visible)
                SetActiveTab("Reports", btnNavReports);
            else if (_btnNavUsers.Visible)
                SetActiveTab("Users", _btnNavUsers);
            else if (_btnNavForm001.Visible)
                SetActiveTab("Form001", _btnNavForm001);
            else if (btnNavSettings.Visible)
                SetActiveTab("Settings", btnNavSettings);
            else
            {
                MessageBox.Show(
                    "This user has no application permissions. Please login as admin and assign rights.",
                    "No Permissions",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        private void SetActiveTab(string tabName, Button navBtn)
        {
            // Check permission
            var tab = _tabs[tabName];
            if (!string.IsNullOrEmpty(tab.RequiredPermission) && !_context.HasPermission(tab.RequiredPermission))
            {
                MessageBox.Show(
                    "Your role does not have permission: " + tab.RequiredPermission,
                    "Permission Denied",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            // Hide current tab
            if (_activeTab != null)
                _activeTab.Visible = false;

            // Reset all nav buttons
            ResetNavButton(btnNavOverview);
            ResetNavButton(btnNavOrders);
            ResetNavButton(btnNavSuppliers);
            ResetNavButton(btnNavProducts);
            ResetNavButton(btnNavReports);
            ResetNavButton(_btnNavUsers);
            ResetNavButton(_btnNavForm001);
            ResetNavButton(btnNavSettings);

            // Show new tab
            _activeTab = tab;
            _activeTab.Visible = true;
            _activeTab.BringToFront();
            _activeTab.OnActivated();

            // Highlight nav button
            HighlightNavButton(navBtn);
            lblPageTitle.Text = tab.TabTitle;
            _activeNavButton = navBtn;
        }

        private void ResetNavButton(Button btn)
        {
            if (btn == null)
                return;

            btn.BackColor = Color.Transparent;
            btn.ForeColor = Color.FromArgb(160, 174, 192);
            btn.Font = new Font("Segoe UI", 10F);
        }

        private void HighlightNavButton(Button btn)
        {
            btn.BackColor = Color.FromArgb(45, 55, 72);
            btn.ForeColor = Color.White;
            btn.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        }

        // Navigation event handlers
        private void btnNavOverview_Click(object sender, EventArgs e)
            => SetActiveTab("Overview", btnNavOverview);

        private void btnNavOrders_Click(object sender, EventArgs e)
            => SetActiveTab("Orders", btnNavOrders);

        private void btnNavSuppliers_Click(object sender, EventArgs e)
            => SetActiveTab("Suppliers", btnNavSuppliers);

        private void btnNavProducts_Click(object sender, EventArgs e)
            => SetActiveTab("Products", btnNavProducts);

        private void btnNavReports_Click(object sender, EventArgs e)
            => SetActiveTab("Reports", btnNavReports);

        private void btnNavSettings_Click(object sender, EventArgs e)
            => SetActiveTab("Settings", btnNavSettings);

        private void btnLogout_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "Are you sure you want to logout?",
                "Confirm Logout",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
                Application.Restart();
        }

        private void timerClock_Tick(object sender, EventArgs e)
        {
            lblDateTime.Text = DateTime.Now.ToString("dddd, MMMM dd yyyy   hh:mm tt");
        }
    }
}
