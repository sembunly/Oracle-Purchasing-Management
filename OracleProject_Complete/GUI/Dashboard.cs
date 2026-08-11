using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace OracleProject
{
    public partial class Dashboard : Form
    {
        private readonly string _currentUser;
        private readonly string _currentRoleCode;
        private HashSet<string> _allowedPermissions = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        private Button _activeNavButton;
        private Button _btnRefreshCurrentPage;
        private Panel _pageSettings;
        private PermissionForm _permissionForm;
        private Panel _pageUsers;
        private Button _btnNavUsers;
        private DataGridView _dgvUsers;
        private Button _btnAddUser;
        private Button _btnEditUser;
        private Button _btnDeactivateUser;
        private Button _btnRefreshUsers;
        private Panel _userEditorPanel;
        private ComboBox _cboUserEmployee;
        private TextBox _txtUserUsername;
        private TextBox _txtUserPassword;
        private ComboBox _cboUserRole;
        private CheckBox _chkUserActive;
        private Button _btnSaveUserInline;
        private Button _btnCancelUserInline;
        private int? _editingUserId;
        private Panel _pageForm001;
        private Button _btnNavForm001;
        private Button _btnForm001Add;
        private Button _btnForm001Edit;
        private Button _btnForm001Delete;
        private Panel _productEditorPanel;
        private TextBox _txtProductCodeInline;
        private TextBox _txtProductNameInline;
        private ComboBox _cboProductCategoryInline;
        private TextBox _txtProductUnitPriceInline;
        private NumericUpDown _numProductStockInline;
        private CheckBox _chkProductActiveInline;
        private Button _btnSaveProductInline;
        private Button _btnCancelProductInline;
        private string _editingProductCode;

        public Dashboard(string username = "Admin", string roleCode = "ADMIN")
        {
            _currentUser = username;
            _currentRoleCode = string.IsNullOrWhiteSpace(roleCode) ? "REQUESTER" : roleCode;
            InitializeComponent();
            CreateGlobalRefreshButton();
            CreateUsersNavButton();
            CreateUsersPage();
            CreateForm001NavButton();
            CreateForm001Page();
            CreateSettingsPage();
            CreateProductInlineEditor();
            btnPrintOrder.Click += btnPrintOrder_Click;
            btnExportReport.Click += btnExportReport_Click;
            pageOverview.Resize += pageOverview_Resize;
            LayoutOverviewCards();
        }

        private void CreateGlobalRefreshButton()
        {
            _btnRefreshCurrentPage = new Button
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Left,
                BackColor = Color.FromArgb(107, 114, 128),
                Cursor = Cursors.Hand,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(650, 32),
                Size = new Size(110, 38),
                Text = "Refresh",
                UseVisualStyleBackColor = false
            };
            _btnRefreshCurrentPage.FlatAppearance.BorderSize = 0;
            _btnRefreshCurrentPage.Click += delegate { RefreshCurrentPage(); };
            panelTopBar.Controls.Add(_btnRefreshCurrentPage);
            _btnRefreshCurrentPage.BringToFront();
        }

        private void RefreshCurrentPage()
        {
            try
            {
                if (pageOverview.Visible)
                {
                    LoadRecentOrders();
                    LayoutOverviewCards();
                }
                else if (pageOrders.Visible)
                    LoadOrders();
                else if (pageSuppliers.Visible)
                    LoadSuppliers();
                else if (pageProducts.Visible)
                    LoadProducts();
                else if (pageReports.Visible)
                    btnGenerateReport_Click(null, EventArgs.Empty);
                else if (_pageUsers != null && _pageUsers.Visible)
                    LoadUsers();
                else if (_permissionForm != null && !_permissionForm.IsDisposed)
                    _permissionForm.RefreshPermissionData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Refresh Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CreateUsersNavButton()
        {
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
            _btnNavUsers.Click += btnNavUsers_Click;
            panelSidebar.Controls.Add(_btnNavUsers);
            _btnNavUsers.BringToFront();
        }

        private void CreateUsersPage()
        {
            _pageUsers = new Panel
            {
                Anchor = pageOverview.Anchor,
                BackColor = Color.FromArgb(247, 250, 252),
                Location = pageOverview.Location,
                Margin = pageOverview.Margin,
                Name = "pageUsers",
                Size = pageOverview.Size,
                TabIndex = 6,
                Visible = false
            };

            var toolbar = new Panel
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                BackColor = Color.White,
                Location = new Point(30, 30),
                Size = new Size(Math.Max(720, _pageUsers.Width - 60), 90)
            };

            var title = new Label
            {
                AutoSize = false,
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(31, 41, 55),
                Location = new Point(20, 16),
                Size = new Size(300, 34),
                Text = "Application Users"
            };

            var subtitle = new Label
            {
                AutoSize = false,
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(113, 128, 150),
                Location = new Point(22, 52),
                Size = new Size(600, 24),
                Text = "Create users, assign employee and choose role."
            };

            var actions = new FlowLayoutPanel
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                FlowDirection = FlowDirection.LeftToRight,
                Location = new Point(Math.Max(350, toolbar.Width - 500), 20),
                Size = new Size(480, 55)
            };

            _btnAddUser = CreatePageButton("Add User", Color.FromArgb(49, 130, 206), 0);
            _btnEditUser = CreatePageButton("Edit", Color.FromArgb(72, 187, 120), 0);
            _btnDeactivateUser = CreatePageButton("Deactivate", Color.FromArgb(245, 101, 101), 0);
            _btnRefreshUsers = CreatePageButton("Refresh", Color.FromArgb(107, 114, 128), 0);

            _btnAddUser.Size = new Size(115, 48);
            _btnEditUser.Size = new Size(90, 48);
            _btnDeactivateUser.Size = new Size(125, 48);
            _btnRefreshUsers.Size = new Size(115, 48);
            _btnAddUser.Click += btnAddUser_Click;
            _btnEditUser.Click += btnEditUser_Click;
            _btnDeactivateUser.Click += btnDeactivateUser_Click;
            _btnRefreshUsers.Click += delegate { LoadUsers(); };
            actions.Controls.Add(_btnAddUser);
            actions.Controls.Add(_btnEditUser);
            actions.Controls.Add(_btnDeactivateUser);
            actions.Controls.Add(_btnRefreshUsers);

            toolbar.Controls.Add(title);
            toolbar.Controls.Add(subtitle);
            toolbar.Controls.Add(actions);
            _pageUsers.Controls.Add(toolbar);

            _userEditorPanel = CreateUserEditorPanel();
            _pageUsers.Controls.Add(_userEditorPanel);

            _dgvUsers = new DataGridView
            {
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                Location = new Point(30, 330),
                ReadOnly = true,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                Size = new Size(Math.Max(720, _pageUsers.Width - 60), Math.Max(260, _pageUsers.Height - 360))
            };
            _pageUsers.Controls.Add(_dgvUsers);
            _pageUsers.Resize += delegate { LayoutUsersPage(); };

            panelContent.Controls.Add(_pageUsers);
            _pageUsers.BringToFront();
            panelTopBar.BringToFront();
        }

        private Panel CreateUserEditorPanel()
        {
            var panel = new Panel
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                BackColor = Color.White,
                Location = new Point(30, 130),
                Size = new Size(Math.Max(720, _pageUsers.Width - 60), 180),
                Visible = false
            };

            _cboUserEmployee = CreateEditorCombo(20, 40, 300);
            _txtUserUsername = CreateEditorTextBox(340, 40, 160);
            _txtUserPassword = CreateEditorTextBox(520, 40, 160);
            _txtUserPassword.UseSystemPasswordChar = true;
            _cboUserRole = CreateEditorCombo(20, 110, 300);
            _chkUserActive = new CheckBox
            {
                Location = new Point(340, 116),
                Size = new Size(120, 28),
                Text = "Active",
                Checked = true
            };
            _btnSaveUserInline = CreatePageButton("Save", Color.FromArgb(45, 101, 181), 520);
            _btnCancelUserInline = CreatePageButton("Cancel", Color.FromArgb(107, 114, 128), 620);
            _btnSaveUserInline.Size = new Size(90, 48);
            _btnCancelUserInline.Size = new Size(90, 48);
            _btnSaveUserInline.Location = new Point(520, 108);
            _btnCancelUserInline.Location = new Point(620, 108);
            _btnSaveUserInline.Click += btnSaveUserInline_Click;
            _btnCancelUserInline.Click += delegate { HideUserEditor(); };

            panel.Controls.Add(CreateEditorLabel("Employee", 20, 16));
            panel.Controls.Add(_cboUserEmployee);
            panel.Controls.Add(CreateEditorLabel("Username", 340, 16));
            panel.Controls.Add(_txtUserUsername);
            panel.Controls.Add(CreateEditorLabel("Password", 520, 16));
            panel.Controls.Add(_txtUserPassword);
            panel.Controls.Add(CreateEditorLabel("Role", 20, 86));
            panel.Controls.Add(_cboUserRole);
            panel.Controls.Add(_chkUserActive);
            panel.Controls.Add(_btnSaveUserInline);
            panel.Controls.Add(_btnCancelUserInline);
            return panel;
        }

        private static Label CreateEditorLabel(string text, int left, int top)
        {
            return new Label
            {
                Location = new Point(left, top),
                Size = new Size(180, 22),
                Text = text,
                ForeColor = Color.FromArgb(55, 65, 81),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold)
            };
        }

        private static ComboBox CreateEditorCombo(int left, int top, int width)
        {
            return new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Location = new Point(left, top),
                Size = new Size(width, 30)
            };
        }

        private static TextBox CreateEditorTextBox(int left, int top, int width)
        {
            return new TextBox
            {
                Location = new Point(left, top),
                Size = new Size(width, 30)
            };
        }

        private void LayoutUsersPage()
        {
            int pageWidth = Math.Max(720, _pageUsers.Width - 60);
            foreach (Control control in _pageUsers.Controls)
            {
                if (control.Anchor.HasFlag(AnchorStyles.Right))
                    continue;
            }

            if (_userEditorPanel != null)
                _userEditorPanel.Size = new Size(pageWidth, 180);

            if (_dgvUsers != null)
            {
                _dgvUsers.Width = pageWidth;
                _dgvUsers.Top = _userEditorPanel != null && _userEditorPanel.Visible ? 330 : 145;
                _dgvUsers.Height = Math.Max(260, _pageUsers.Height - _dgvUsers.Top - 30);
            }
        }

        private void CreateProductInlineEditor()
        {
            _productEditorPanel = new Panel
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                BackColor = Color.White,
                Location = new Point(0, panelProductsToolbar.Height),
                Size = new Size(pageProducts.Width, 140),
                Visible = false
            };

            _txtProductCodeInline = CreateEditorTextBox(20, 40, 160);
            _txtProductNameInline = CreateEditorTextBox(200, 40, 240);
            _cboProductCategoryInline = CreateEditorCombo(460, 40, 180);
            _cboProductCategoryInline.Items.AddRange(new object[]
            {
                "Electronics",
                "Office Supplies",
                "Raw Materials",
                "Components",
                "Other"
            });
            _txtProductUnitPriceInline = CreateEditorTextBox(660, 40, 140);
            _numProductStockInline = new NumericUpDown
            {
                Location = new Point(820, 40),
                Maximum = 999999,
                DecimalPlaces = 0,
                Size = new Size(120, 30)
            };
            _chkProductActiveInline = new CheckBox
            {
                Location = new Point(20, 96),
                Size = new Size(120, 28),
                Text = "Active",
                Checked = true
            };
            _btnSaveProductInline = CreatePageButton("Save", Color.FromArgb(45, 101, 181), 660);
            _btnCancelProductInline = CreatePageButton("Cancel", Color.FromArgb(107, 114, 128), 760);
            _btnSaveProductInline.Size = new Size(90, 40);
            _btnCancelProductInline.Size = new Size(90, 40);
            _btnSaveProductInline.Location = new Point(660, 88);
            _btnCancelProductInline.Location = new Point(760, 88);
            _btnSaveProductInline.Click += btnSaveProductInline_Click;
            _btnCancelProductInline.Click += delegate { HideProductEditor(); };

            _productEditorPanel.Controls.Add(CreateEditorLabel("Product Code", 20, 16));
            _productEditorPanel.Controls.Add(_txtProductCodeInline);
            _productEditorPanel.Controls.Add(CreateEditorLabel("Product Name", 200, 16));
            _productEditorPanel.Controls.Add(_txtProductNameInline);
            _productEditorPanel.Controls.Add(CreateEditorLabel("Category", 460, 16));
            _productEditorPanel.Controls.Add(_cboProductCategoryInline);
            _productEditorPanel.Controls.Add(CreateEditorLabel("Unit Price", 660, 16));
            _productEditorPanel.Controls.Add(_txtProductUnitPriceInline);
            _productEditorPanel.Controls.Add(CreateEditorLabel("Stock Qty", 820, 16));
            _productEditorPanel.Controls.Add(_numProductStockInline);
            _productEditorPanel.Controls.Add(_chkProductActiveInline);
            _productEditorPanel.Controls.Add(_btnSaveProductInline);
            _productEditorPanel.Controls.Add(_btnCancelProductInline);

            pageProducts.Controls.Add(_productEditorPanel);
            _productEditorPanel.BringToFront();
            pageProducts.Resize += delegate { LayoutProductEditor(); };
        }

        private void LayoutProductEditor()
        {
            if (_productEditorPanel == null)
                return;

            _productEditorPanel.Width = pageProducts.ClientSize.Width;
            dgvProducts.Top = _productEditorPanel.Visible
                ? panelProductsToolbar.Height + _productEditorPanel.Height
                : panelProductsToolbar.Height;
            dgvProducts.Height = Math.Max(200, pageProducts.ClientSize.Height - dgvProducts.Top);
        }

        private void CreateForm001NavButton()
        {
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
            _btnNavForm001.Click += btnNavForm001_Click;
            panelSidebar.Controls.Add(_btnNavForm001);
            _btnNavForm001.BringToFront();
        }

        private void CreateForm001Page()
        {
            _pageForm001 = new Panel
            {
                Anchor = pageOverview.Anchor,
                BackColor = Color.FromArgb(247, 250, 252),
                Location = pageOverview.Location,
                Margin = pageOverview.Margin,
                Name = "pageForm001",
                Size = pageOverview.Size,
                TabIndex = 6,
                Visible = false
            };

            var toolbar = new Panel
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                BackColor = Color.White,
                Location = new Point(30, 30),
                Size = new Size(Math.Max(900, _pageForm001.Width - 60), 90)
            };

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

            _btnForm001Add = CreatePageButton("Add", Color.FromArgb(49, 130, 206), 640);
            _btnForm001Edit = CreatePageButton("Edit", Color.FromArgb(72, 187, 120), 760);
            _btnForm001Delete = CreatePageButton("Delete", Color.FromArgb(245, 101, 101), 880);
            _btnForm001Add.Click += (sender, e) => RunForm001Action("FORM001_ADD", "Add");
            _btnForm001Edit.Click += (sender, e) => RunForm001Action("FORM001_EDIT", "Edit");
            _btnForm001Delete.Click += (sender, e) => RunForm001Action("FORM001_DELETE", "Delete");

            toolbar.Controls.Add(title);
            toolbar.Controls.Add(subtitle);
            toolbar.Controls.Add(_btnForm001Add);
            toolbar.Controls.Add(_btnForm001Edit);
            toolbar.Controls.Add(_btnForm001Delete);
            _pageForm001.Controls.Add(toolbar);

            var placeholder = new Label
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                AutoSize = false,
                BackColor = Color.White,
                Font = new Font("Segoe UI", 12F),
                ForeColor = Color.FromArgb(75, 85, 99),
                Location = new Point(30, 145),
                Size = new Size(Math.Max(900, _pageForm001.Width - 60), 360),
                Text = "This is your new Form 001 workspace.\r\n\r\nPermissions ready:\r\nFORM001_VIEW, FORM001_ADD, FORM001_EDIT, FORM001_DELETE",
                TextAlign = ContentAlignment.MiddleCenter
            };
            _pageForm001.Controls.Add(placeholder);

            panelContent.Controls.Add(_pageForm001);
            _pageForm001.BringToFront();
            panelTopBar.BringToFront();
        }

        private static Button CreatePageButton(string text, Color backColor, int left)
        {
            var button = new Button
            {
                BackColor = backColor,
                Cursor = Cursors.Hand,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(left, 20),
                Size = new Size(105, 48),
                Text = text,
                UseVisualStyleBackColor = false
            };
            button.FlatAppearance.BorderSize = 0;
            return button;
        }

        private void CreateSettingsPage()
        {
            _pageSettings = new Panel
            {
                Anchor = pageOverview.Anchor,
                BackColor = Color.FromArgb(247, 250, 252),
                Location = pageOverview.Location,
                Margin = pageOverview.Margin,
                Name = "pageSettings",
                Size = pageOverview.Size,
                TabIndex = 6,
                Visible = false
            };

            panelContent.Controls.Add(_pageSettings);
            _pageSettings.BringToFront();
            panelTopBar.BringToFront();
        }

        private void pageOverview_Resize(object sender, EventArgs e)
        {
            LayoutOverviewCards();
        }

        private void LayoutOverviewCards()
        {
            const int outerMargin = 20;
            const int gap = 20;
            const int cardCount = 4;

            int availableWidth = pageOverview.ClientSize.Width
                - (outerMargin * 2)
                - (gap * (cardCount - 1));

            if (availableWidth <= 0)
                return;

            int cardWidth = availableWidth / cardCount;
            Panel[] cards = { cardPanel1, cardPanel2, cardPanel3, cardPanel4 };
            Panel[] accents = { panelCard1Accent, panelCard2Accent, panelCard3Accent, panelCard4Accent };
            Label[] titles = { lblCard1Title, lblCard2Title, lblCard3Title, lblCard4Title };
            Label[] values = { lblCard1Value, lblCard2Value, lblCard3Value, lblCard4Value };
            Label[] subtitles = { lblCard1Sub, lblCard2Sub, lblCard3Sub, lblCard4Sub };

            for (int i = 0; i < cards.Length; i++)
            {
                int left = outerMargin + (i * (cardWidth + gap));
                int width = i == cards.Length - 1
                    ? pageOverview.ClientSize.Width - outerMargin - left
                    : cardWidth;

                cards[i].SetBounds(left, outerMargin, width, cards[i].Height);
                accents[i].Height = cards[i].ClientSize.Height;

                int labelWidth = Math.Max(0, width - 30);
                titles[i].Width = labelWidth;
                values[i].Width = labelWidth;
                subtitles[i].Width = labelWidth;
            }
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {
            lblUserGreeting.Text = "Welcome, " + _currentUser + " (" + _currentRoleCode + ")";
            timerClock.Start();
            timerClock_Tick(null, null);
            dtpFrom.Value = new DateTime(DateTime.Today.Year, 1, 1);
            dtpTo.Value = DateTime.Today;

            try
            {
                LoadDatabaseData();
                ApplyCurrentUserPermissions();
                OpenFirstAllowedPage();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Oracle database could not be loaded.\n\n" + ex.Message,
                    "Oracle Connection Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                ClearDataGrids();
            }
        }

        private void timerClock_Tick(object sender, EventArgs e)
        {
            lblDateTime.Text = DateTime.Now.ToString("dddd, MMMM dd yyyy   hh:mm tt");
        }

        // =========================================================
        // NAVIGATION
        // =========================================================
        private void SetActivePage(Panel page, Button navBtn, string title)
        {
            pageOverview.Visible = false;
            pageOrders.Visible = false;
            pageSuppliers.Visible = false;
            pageProducts.Visible = false;
            pageReports.Visible = false;
            if (_pageUsers != null)
                _pageUsers.Visible = false;
            if (_pageForm001 != null)
                _pageForm001.Visible = false;
            if (_pageSettings != null)
                _pageSettings.Visible = false;

            ResetNavButton(btnNavOverview);
            ResetNavButton(btnNavOrders);
            ResetNavButton(btnNavSuppliers);
            ResetNavButton(btnNavProducts);
            ResetNavButton(btnNavReports);
            ResetNavButton(_btnNavUsers);
            ResetNavButton(_btnNavForm001);
            ResetNavButton(btnNavSettings);

            page.Visible = true;
            page.BringToFront();
            panelTopBar.BringToFront();
            HighlightNavButton(navBtn);
            lblPageTitle.Text = title;
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

        private void btnNavOverview_Click(object sender, EventArgs e)
            => SetActivePage(pageOverview, btnNavOverview, "Overview");

        private void btnNavOrders_Click(object sender, EventArgs e)
            => SetActivePage(pageOrders, btnNavOrders, "Purchase Orders");

        private void btnNavSuppliers_Click(object sender, EventArgs e)
            => SetActivePage(pageSuppliers, btnNavSuppliers, "Suppliers");

        private void btnNavProducts_Click(object sender, EventArgs e)
            => SetActivePage(pageProducts, btnNavProducts, "Products");

        private void btnNavReports_Click(object sender, EventArgs e)
            => SetActivePage(pageReports, btnNavReports, "Reports");

        private void btnNavUsers_Click(object sender, EventArgs e)
        {
            if (!RequirePermission("USERS_VIEW"))
                return;

            LoadUsers();
            SetActivePage(_pageUsers, _btnNavUsers, "Users");
        }

        private void btnNavForm001_Click(object sender, EventArgs e)
            => SetActivePage(_pageForm001, _btnNavForm001, "Form 001");

        private void btnNavSettings_Click(object sender, EventArgs e)
        {
            if (!HasPermission("SETTINGS_VIEW") && !HasPermission("SETTINGS_MANAGE"))
                return;

            LoadPermissionFormInSettingsPage();
            SetActivePage(_pageSettings, btnNavSettings, "Settings");
        }

        private void LoadPermissionFormInSettingsPage()
        {
            if (_permissionForm != null && !_permissionForm.IsDisposed)
                return;

            _permissionForm = new PermissionForm(HasPermission("SETTINGS_MANAGE"));
            _permissionForm.ConfigureForEmbedded();
            _pageSettings.Controls.Clear();
            _pageSettings.Controls.Add(_permissionForm);
            _permissionForm.Show();
        }

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

        private void ApplyCurrentUserPermissions()
        {
            try
            {
                _allowedPermissions = OracleDb.GetAllowedPermissions(_currentRoleCode);
            }
            catch (OracleException ex) when (ex.Number == 942)
            {
                if (string.Equals(_currentRoleCode, "ADMIN", StringComparison.OrdinalIgnoreCase))
                    AllowAllKnownPermissions();
                else
                    _allowedPermissions.Clear();
            }

            btnNavOverview.Visible = HasPermission("OVERVIEW_VIEW");
            btnNavOrders.Visible = HasPermission("ORDERS_VIEW");
            btnNavSuppliers.Visible = HasPermission("SUPPLIERS_VIEW");
            btnNavProducts.Visible = HasPermission("PRODUCTS_VIEW");
            btnNavReports.Visible = HasPermission("REPORTS_VIEW");
            _btnNavUsers.Visible = HasPermission("USERS_VIEW");
            _btnNavForm001.Visible = HasPermission("FORM001_VIEW");
            btnNavSettings.Visible = HasPermission("SETTINGS_VIEW") || HasPermission("SETTINGS_MANAGE");

            btnNewOrder.Visible = HasPermission("ORDERS_ADD");
            btnEditOrder.Visible = HasPermission("ORDERS_EDIT");
            btnDeleteOrder.Visible = HasPermission("ORDERS_DELETE");
            btnPrintOrder.Visible = HasPermission("ORDERS_PRINT");

            btnNewSupplier.Visible = HasPermission("SUPPLIERS_ADD");
            btnEditSupplier.Visible = HasPermission("SUPPLIERS_EDIT");
            btnDeleteSupplier.Visible = HasPermission("SUPPLIERS_DELETE");

            btnNewProduct.Visible = HasPermission("PRODUCTS_ADD");
            btnEditProduct.Visible = HasPermission("PRODUCTS_EDIT");
            btnDeleteProduct.Visible = HasPermission("PRODUCTS_DELETE");

            btnGenerateReport.Visible = HasPermission("REPORTS_GENERATE");
            btnExportReport.Visible = HasPermission("REPORTS_EXPORT");

            _btnAddUser.Visible = HasPermission("USERS_ADD");
            _btnEditUser.Visible = HasPermission("USERS_EDIT");
            _btnDeactivateUser.Visible = HasPermission("USERS_DELETE");
            _btnRefreshUsers.Visible = HasPermission("USERS_VIEW");

            _btnForm001Add.Visible = HasPermission("FORM001_ADD");
            _btnForm001Edit.Visible = HasPermission("FORM001_EDIT");
            _btnForm001Delete.Visible = HasPermission("FORM001_DELETE");

            LayoutSidebarNavigation();
            LayoutActionButtons();
        }

        private bool HasPermission(string permissionCode)
        {
            return _allowedPermissions.Contains(permissionCode);
        }

        private bool RequirePermission(string permissionCode)
        {
            if (HasPermission(permissionCode))
                return true;

            MessageBox.Show(
                "Your role does not have permission: " + permissionCode,
                "Permission Denied",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return false;
        }

        private void AllowAllKnownPermissions()
        {
            _allowedPermissions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "OVERVIEW_VIEW",
                "ORDERS_VIEW",
                "ORDERS_ADD",
                "ORDERS_EDIT",
                "ORDERS_DELETE",
                "ORDERS_PRINT",
                "SUPPLIERS_VIEW",
                "SUPPLIERS_ADD",
                "SUPPLIERS_EDIT",
                "SUPPLIERS_DELETE",
                "PRODUCTS_VIEW",
                "PRODUCTS_ADD",
                "PRODUCTS_EDIT",
                "PRODUCTS_DELETE",
                "REPORTS_VIEW",
                "REPORTS_GENERATE",
                "REPORTS_EXPORT",
                "USERS_VIEW",
                "USERS_ADD",
                "USERS_EDIT",
                "USERS_DELETE",
                "SETTINGS_VIEW",
                "SETTINGS_MANAGE",
                "FORM001_VIEW",
                "FORM001_ADD",
                "FORM001_EDIT",
                "FORM001_DELETE"
            };
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

        private void LayoutActionButtons()
        {
            LayoutVisibleButtons(btnNewOrder, btnEditOrder, btnDeleteOrder, btnPrintOrder);
            LayoutVisibleButtons(btnNewSupplier, btnEditSupplier, btnDeleteSupplier);
            LayoutVisibleButtons(btnNewProduct, btnEditProduct, btnDeleteProduct);
            LayoutVisibleButtons(btnGenerateReport, btnExportReport);
            LayoutVisibleButtons(_btnAddUser, _btnEditUser, _btnDeactivateUser, _btnRefreshUsers);
            LayoutVisibleButtons(_btnForm001Add, _btnForm001Edit, _btnForm001Delete);
        }

        private static void LayoutVisibleButtons(params Button[] buttons)
        {
            if (buttons == null || buttons.Length == 0 || buttons[0] == null)
                return;

            int left = buttons[0].Left;
            int top = buttons[0].Top;
            const int gap = 15;

            foreach (Button button in buttons)
            {
                if (button == null || !button.Visible)
                    continue;

                button.Left = left;
                button.Top = top;
                left += button.Width + gap;
            }
        }

        private void OpenFirstAllowedPage()
        {
            if (btnNavOverview.Visible)
                SetActivePage(pageOverview, btnNavOverview, "Overview");
            else if (btnNavOrders.Visible)
                SetActivePage(pageOrders, btnNavOrders, "Purchase Orders");
            else if (btnNavSuppliers.Visible)
                SetActivePage(pageSuppliers, btnNavSuppliers, "Suppliers");
            else if (btnNavProducts.Visible)
                SetActivePage(pageProducts, btnNavProducts, "Products");
            else if (btnNavReports.Visible)
                SetActivePage(pageReports, btnNavReports, "Reports");
            else if (_btnNavUsers.Visible)
            {
                LoadUsers();
                SetActivePage(_pageUsers, _btnNavUsers, "Users");
            }
            else if (_btnNavForm001.Visible)
                SetActivePage(_pageForm001, _btnNavForm001, "Form 001");
            else if (btnNavSettings.Visible)
            {
                LoadPermissionFormInSettingsPage();
                SetActivePage(_pageSettings, btnNavSettings, "Settings");
            }
            else
            {
                MessageBox.Show(
                    "This user has no application permissions. Please login as admin and assign rights.",
                    "No Permissions",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        private void RunForm001Action(string permissionCode, string actionName)
        {
            if (!RequirePermission(permissionCode))
                return;

            MessageBox.Show(
                "Form 001 " + actionName + " action is ready. Connect your real form code here.",
                "Form 001",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        // =========================================================
        // DATABASE LOADING
        // =========================================================
        private void LoadDatabaseData()
        {
            LoadRecentOrders();
            LoadOrders();
            LoadSuppliers();
            LoadProducts();
        }

        private void ClearDataGrids()
        {
            dgvRecentOrders.DataSource = null;
            dgvOrders.DataSource = null;
            dgvSuppliers.DataSource = null;
            dgvProducts.DataSource = null;
            dgvReport.DataSource = null;
        }

        private void LoadRecentOrders()
        {
            const string sql = @"
                SELECT po_no AS ""PO Number"",
                       supplier_name AS ""Supplier"",
                       po_date AS ""Date"",
                       item_lines AS ""Items"",
                       total_amount AS ""Total Amount"",
                       status AS ""Status"",
                       requested_by AS ""Requested By""
                  FROM (
                      SELECT r.*
                        FROM vw_purchase_report r
                       ORDER BY po_date DESC
                  )
                 WHERE ROWNUM <= 7";

            dgvRecentOrders.DataSource = OracleDb.Query(sql);
            ApplyStatusColor(dgvRecentOrders, 5);
        }

        private void LoadOrders()
        {
            const string sql = @"
                SELECT po_no AS ""PO Number"",
                       supplier_name AS ""Supplier"",
                       po_date AS ""Order Date"",
                       expected_delivery_date AS ""Expected Delivery"",
                       item_lines AS ""Items"",
                       subtotal_amount AS ""Subtotal"",
                       tax_amount AS ""Tax"",
                       total_amount AS ""Total Amount"",
                       status AS ""Status"",
                       requested_by AS ""Requested By"",
                       '-' AS ""Approved By""
                  FROM vw_purchase_report
                 ORDER BY po_date DESC";

            dgvOrders.DataSource = OracleDb.Query(sql);
            ApplyStatusColor(dgvOrders, 8);
        }

        private void LoadSuppliers()
        {
            const string sql = @"
                SELECT s.supplier_code AS ""Supplier ID"",
                       s.supplier_name AS ""Company Name"",
                       s.contact_person AS ""Contact Person"",
                       s.email AS ""Email"",
                       s.phone AS ""Phone"",
                       s.address AS ""Address"",
                       CAST(NULL AS VARCHAR2(50)) AS ""Category"",
                       NVL(p.total_orders, 0) AS ""Total Orders"",
                       NVL(p.total_order_amount, 0) AS ""Total Spend"",
                       CAST(NULL AS VARCHAR2(20)) AS ""Rating"",
                       s.status AS ""Status""
                  FROM suppliers s
                  LEFT JOIN vw_supplier_performance p
                    ON p.supplier_code = s.supplier_code
                 ORDER BY s.supplier_name";

            dgvSuppliers.DataSource = OracleDb.Query(sql);
            ApplyStatusColor(dgvSuppliers, 10);
        }

        private void LoadProducts()
        {
            const string sql = @"
                SELECT product_code AS ""Product ID"",
                       product_name AS ""Product Name"",
                       category AS ""Category"",
                       unit AS ""Unit"",
                       unit_price AS ""Unit Price"",
                       stock_qty AS ""Stock Qty"",
                       reorder_level AS ""Reorder Level"",
                       preferred_supplier AS ""Preferred Supplier"",
                       CAST(NULL AS DATE) AS ""Last Ordered"",
                       stock_status AS ""Status""
                  FROM vw_stock_report
                 ORDER BY product_name";

            dgvProducts.DataSource = OracleDb.Query(sql);
            ApplyStockColor(dgvProducts, 9);
        }

        private void LoadUsers()
        {
            const string sql = @"
                SELECT u.user_id AS ""User ID"",
                       u.username AS ""Username"",
                       e.employee_code || ' - ' || e.full_name AS ""Employee"",
                       r.role_name AS ""Role"",
                       r.role_code AS ""Role Code"",
                       CASE u.status WHEN 1 THEN 'ACTIVE' ELSE 'INACTIVE' END AS ""Status"",
                       u.created_at AS ""Created At"",
                       u.employee_id AS ""Employee ID"",
                       u.role_id AS ""Role ID""
                  FROM app_users u
                  JOIN employees e
                    ON e.employee_id = u.employee_id
                  JOIN app_roles r
                    ON r.role_id = u.role_id
                 ORDER BY u.user_id";

            _dgvUsers.DataSource = OracleDb.Query(sql);
            HideUsersInternalColumns();
        }

        private void HideUsersInternalColumns()
        {
            if (_dgvUsers == null || _dgvUsers.Columns.Count == 0)
                return;

            HideGridColumn(_dgvUsers, "User ID");
            HideGridColumn(_dgvUsers, "Employee ID");
            HideGridColumn(_dgvUsers, "Role ID");
        }

        private static void HideGridColumn(DataGridView grid, string columnName)
        {
            if (grid.Columns.Contains(columnName))
                grid.Columns[columnName].Visible = false;
        }

        private void ReloadAfterChange()
        {
            try
            {
                LoadDatabaseData();
                if (_pageUsers != null && _pageUsers.Visible && HasPermission("USERS_VIEW"))
                    LoadUsers();
            }
            catch (Exception ex)
            {
                MessageBox.Show("The database was changed, but the grid could not refresh.\n\n" + ex.Message,
                    "Refresh Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // =========================================================
        // USER MANAGEMENT
        // =========================================================
        private void btnAddUser_Click(object sender, EventArgs e)
        {
            if (!RequirePermission("USERS_ADD"))
                return;

            ShowUserEditor(null);
        }

        private void btnEditUser_Click(object sender, EventArgs e)
        {
            if (!RequirePermission("USERS_EDIT"))
                return;

            DataRow row;
            if (!TryGetSelectedUserRow(out row))
                return;

            ShowUserEditor(row);
        }

        private void btnDeactivateUser_Click(object sender, EventArgs e)
        {
            if (!RequirePermission("USERS_DELETE"))
                return;

            DataRow row;
            if (!TryGetSelectedUserRow(out row))
                return;

            string username = Convert.ToString(row["Username"]);
            if (string.Equals(username, _currentUser, StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("You cannot deactivate your own login while you are using it.",
                    "Users", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Deactivate user " + username + "? Data will be kept.",
                    "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                OracleDb.Execute(
                    "UPDATE app_users SET status = 0 WHERE user_id = :userId",
                    OracleDb.Parameter("userId", Convert.ToInt32(row["User ID"])));
                LoadUsers();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Deactivate User Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowUserEditor(DataRow row)
        {
            bool isEdit = row != null;
            _editingUserId = isEdit ? (int?)Convert.ToInt32(row["User ID"]) : null;
            FillCombo(_cboUserEmployee, LoadEmployeeChoices(_editingUserId), "EMPLOYEE_ID", "DISPLAY_NAME",
                isEdit ? (int?)Convert.ToInt32(row["Employee ID"]) : null);
            FillCombo(_cboUserRole, LoadRoleChoices(), "ROLE_ID", "DISPLAY_NAME",
                isEdit ? (int?)Convert.ToInt32(row["Role ID"]) : null);

            _txtUserUsername.Text = isEdit ? Convert.ToString(row["Username"]) : string.Empty;
            _txtUserPassword.Text = string.Empty;
            _chkUserActive.Checked = !isEdit ||
                string.Equals(Convert.ToString(row["Status"]), "ACTIVE", StringComparison.OrdinalIgnoreCase);

            _userEditorPanel.Visible = true;
            LayoutUsersPage();
            _txtUserUsername.Focus();
        }

        private void HideUserEditor()
        {
            _editingUserId = null;
            _txtUserUsername.Clear();
            _txtUserPassword.Clear();
            _userEditorPanel.Visible = false;
            LayoutUsersPage();
        }

        private void btnSaveUserInline_Click(object sender, EventArgs e)
        {
            if (_editingUserId.HasValue && !RequirePermission("USERS_EDIT"))
                return;
            if (!_editingUserId.HasValue && !RequirePermission("USERS_ADD"))
                return;

            var employee = _cboUserEmployee.SelectedItem as ComboItem;
            var role = _cboUserRole.SelectedItem as ComboItem;
            string username = _txtUserUsername.Text.Trim();
            string password = _txtUserPassword.Text;

            if (employee == null)
            {
                MessageBox.Show("Please choose an employee.", "Users", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(username))
            {
                MessageBox.Show("Please enter username.", "Users", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!_editingUserId.HasValue && string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter password.", "Users", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (role == null)
            {
                MessageBox.Show("Please choose a role.", "Users", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (!_editingUserId.HasValue)
                {
                    OracleDb.Execute(@"
                        INSERT INTO app_users (
                            user_id, employee_id, username,
                            password_hash, role_id, status
                        ) VALUES (
                            app_user_seq.NEXTVAL, :employeeId, :username,
                            RAWTOHEX(STANDARD_HASH(:password, 'SHA256')),
                            :roleId, :status
                        )",
                        OracleDb.Parameter("employeeId", employee.Id),
                        OracleDb.Parameter("username", username),
                        OracleDb.Parameter("password", password),
                        OracleDb.Parameter("roleId", role.Id),
                        OracleDb.Parameter("status", _chkUserActive.Checked ? 1 : 0));
                }
                else if (string.IsNullOrWhiteSpace(password))
                {
                    OracleDb.Execute(@"
                        UPDATE app_users
                           SET employee_id = :employeeId,
                               username = :username,
                               role_id = :roleId,
                               status = :status
                         WHERE user_id = :userId",
                        OracleDb.Parameter("employeeId", employee.Id),
                        OracleDb.Parameter("username", username),
                        OracleDb.Parameter("roleId", role.Id),
                        OracleDb.Parameter("status", _chkUserActive.Checked ? 1 : 0),
                        OracleDb.Parameter("userId", _editingUserId.Value));
                }
                else
                {
                    OracleDb.Execute(@"
                        UPDATE app_users
                           SET employee_id = :employeeId,
                               username = :username,
                               password_hash = RAWTOHEX(STANDARD_HASH(:password, 'SHA256')),
                               role_id = :roleId,
                               status = :status
                         WHERE user_id = :userId",
                        OracleDb.Parameter("employeeId", employee.Id),
                        OracleDb.Parameter("username", username),
                        OracleDb.Parameter("password", password),
                        OracleDb.Parameter("roleId", role.Id),
                        OracleDb.Parameter("status", _chkUserActive.Checked ? 1 : 0),
                        OracleDb.Parameter("userId", _editingUserId.Value));
                }

                HideUserEditor();
                LoadUsers();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Save User Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void FillCombo(
            ComboBox combo,
            DataTable table,
            string idColumn,
            string textColumn,
            int? selectedId)
        {
            combo.Items.Clear();
            foreach (DataRow row in table.Rows)
            {
                var item = new ComboItem(Convert.ToInt32(row[idColumn]), Convert.ToString(row[textColumn]));
                combo.Items.Add(item);
                if (selectedId.HasValue && item.Id == selectedId.Value)
                    combo.SelectedItem = item;
            }

            if (combo.SelectedIndex < 0 && combo.Items.Count > 0)
                combo.SelectedIndex = 0;
        }

        private sealed class ComboItem
        {
            public ComboItem(int id, string text)
            {
                Id = id;
                Text = text;
            }

            public int Id { get; private set; }
            private string Text { get; set; }

            public override string ToString()
            {
                return Text;
            }
        }

        private DataTable LoadEmployeeChoices(int? editingUserId)
        {
            return OracleDb.Query(@"
                SELECT e.employee_id,
                       e.employee_code || ' - ' || e.full_name AS display_name
                  FROM employees e
                 WHERE e.status = 1
                   AND NOT EXISTS (
                       SELECT 1
                         FROM app_users u
                        WHERE u.employee_id = e.employee_id
                          AND u.user_id <> NVL(:editingUserId, -1)
                   )
                 ORDER BY e.full_name",
                OracleDb.Parameter("editingUserId", editingUserId.HasValue ? (object)editingUserId.Value : DBNull.Value));
        }

        private DataTable LoadRoleChoices()
        {
            return OracleDb.Query(@"
                SELECT role_id,
                       role_name || ' (' || role_code || ')' AS display_name
                  FROM app_roles
                 WHERE status = 1
                 ORDER BY role_id");
        }

        private bool TryGetSelectedUserRow(out DataRow row)
        {
            row = null;
            if (_dgvUsers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a user.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            var view = _dgvUsers.SelectedRows[0].DataBoundItem as DataRowView;
            if (view == null)
                return false;

            row = view.Row;
            return true;
        }

        // =========================================================
        // PURCHASE ORDER OPERATIONS
        // =========================================================
        private void btnNewOrder_Click(object sender, EventArgs e)
        {
            if (!RequirePermission("ORDERS_ADD"))
                return;

            using (var form = new NewOrderForm())
            {
                if (form.ShowDialog() != DialogResult.OK)
                    return;

                try
                {
                    CreatePurchaseOrder(form);
                    ReloadAfterChange();
                    MessageBox.Show("Purchase order created in Oracle.", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Create PO Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void CreatePurchaseOrder(NewOrderForm form)
        {
            object employee = OracleDb.Scalar(
                "SELECT employee_id FROM employees WHERE status = 1 AND ROWNUM = 1");
            if (employee == null || employee == DBNull.Value)
                throw new InvalidOperationException("No active employee exists for PO creation.");

            using (var connection = OracleDb.OpenConnection())
            using (var transaction = connection.BeginTransaction())
            using (var command = connection.CreateCommand())
            {
                command.BindByName = true;
                command.Transaction = transaction;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "sp_create_po";
                command.Parameters.Add("p_po_no", OracleDbType.Varchar2).Value = form.PONumber;
                command.Parameters.Add("p_request_id", OracleDbType.Decimal).Value = form.SelectedRequestId;
                command.Parameters.Add("p_quotation_id", OracleDbType.Decimal).Value = form.SelectedQuotationId;
                command.Parameters.Add("p_expected_delivery_date", OracleDbType.Date).Value = form.ExpectedDelivery;
                command.Parameters.Add("p_created_by", OracleDbType.Decimal).Value = Convert.ToDecimal(employee);
                command.Parameters.Add("p_tax_amount", OracleDbType.Decimal).Value = form.Tax;

                var output = new OracleParameter("p_po_id", OracleDbType.Decimal)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(output);
                try
                {
                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        private void btnEditOrder_Click(object sender, EventArgs e)
        {
            if (!RequirePermission("ORDERS_EDIT"))
                return;

            if (dgvOrders.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an order to edit.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string poNo = Convert.ToString(dgvOrders.SelectedRows[0].Cells[0].Value);
            using (var form = new EditOrderForm())
            {
                form.txtPONumber.Text = poNo;
                form.cmbSupplier.Text = Convert.ToString(dgvOrders.SelectedRows[0].Cells[1].Value);
                SetDate(form.dtpOrderDate, dgvOrders.SelectedRows[0].Cells[2].Value);
                SetDate(form.dtpExpectedDelivery, dgvOrders.SelectedRows[0].Cells[3].Value);
                form.txtSubtotal.Text = Convert.ToString(dgvOrders.SelectedRows[0].Cells[5].Value);
                form.txtTax.Text = Convert.ToString(dgvOrders.SelectedRows[0].Cells[6].Value);
                form.txtTotal.Text = Convert.ToString(dgvOrders.SelectedRows[0].Cells[7].Value);
                form.cmbStatus.Text = PurchaseOrderStatusText(dgvOrders.SelectedRows[0].Cells[8].Value);

                if (form.ShowDialog() != DialogResult.OK)
                    return;

                try
                {
                    OracleDb.Execute(@"
                        UPDATE purchase_orders
                           SET expected_delivery_date = :expected_date,
                               tax_amount = :tax_amount,
                               total_amount = subtotal_amount + :tax_amount,
                               status = CASE UPPER(:status) WHEN 'ACTIVE' THEN 1 WHEN 'INACTIVE' THEN 0 WHEN 'DRAFT' THEN 0 WHEN 'APPROVED' THEN 1 WHEN 'PARTIALLY_RECEIVED' THEN 2 WHEN 'RECEIVED' THEN 3 WHEN 'CANCELLED' THEN 4 WHEN 'CLOSED' THEN 5 ELSE status END
                         WHERE po_no = :po_no",
                        OracleDb.Parameter("expected_date", form.ExpectedDelivery),
                        OracleDb.Parameter("tax_amount", form.Tax),
                        OracleDb.Parameter("status", form.Status),
                        OracleDb.Parameter("po_no", poNo));

                    ReloadAfterChange();
                    MessageBox.Show("Purchase order updated in Oracle.", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Update PO Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnDeleteOrder_Click(object sender, EventArgs e)
        {
            if (!RequirePermission("ORDERS_DELETE"))
                return;

            if (dgvOrders.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an order to cancel.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string poNo = Convert.ToString(dgvOrders.SelectedRows[0].Cells[0].Value);
            if (MessageBox.Show("Cancel PO " + poNo + "? History will be preserved.", "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                OracleDb.Execute(
                    "UPDATE purchase_orders SET status = 4 WHERE po_no = :po_no",
                    OracleDb.Parameter("po_no", poNo));
                ReloadAfterChange();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Cancel PO Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnPrintOrder_Click(object sender, EventArgs e)
        {
            if (!RequirePermission("ORDERS_PRINT"))
                return;

            if (dgvOrders.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an order to export.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string poNo = Convert.ToString(dgvOrders.SelectedRows[0].Cells[0].Value);
            ExportGridRows(
                dgvOrders,
                new[] { dgvOrders.SelectedRows[0] },
                "purchase-order-" + SafeFileName(poNo) + ".csv",
                "Purchase order exported.");
        }

        // =========================================================
        // SUPPLIER CRUD
        // =========================================================
        private void btnNewSupplier_Click(object sender, EventArgs e)
        {
            if (!RequirePermission("SUPPLIERS_ADD"))
                return;

            using (var form = new NewSupplierForm())
            {
                if (form.ShowDialog() != DialogResult.OK)
                    return;

                try
                {
                    OracleDb.Execute(@"
                        INSERT INTO suppliers (
                            supplier_id, supplier_code, supplier_name, contact_person,
                            phone, email, address, status
                        ) VALUES (
                            supplier_seq.NEXTVAL,
                            'SUP-' || TO_CHAR(supplier_seq.CURRVAL),
                            :supplier_name, :contact_person, :phone, :email,
                            :address, CASE UPPER(:status) WHEN 'ACTIVE' THEN 1 ELSE 0 END
                        )",
                        OracleDb.Parameter("supplier_name", form.SupplierName),
                        OracleDb.Parameter("contact_person", form.ContactPerson),
                        OracleDb.Parameter("phone", form.Phone),
                        OracleDb.Parameter("email", form.Email),
                        OracleDb.Parameter("address", form.Address),
                        OracleDb.Parameter("status", form.Status));
                    ReloadAfterChange();
                    MessageBox.Show("Supplier saved to Oracle.", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Create Supplier Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnEditSupplier_Click(object sender, EventArgs e)
        {
            if (!RequirePermission("SUPPLIERS_EDIT"))
                return;

            if (dgvSuppliers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a supplier to edit.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var row = dgvSuppliers.SelectedRows[0];
            string code = Convert.ToString(row.Cells[0].Value);
            using (var form = new EditSupplierForm())
            {
                form.txtSupplierName.Text = Convert.ToString(row.Cells[1].Value);
                form.txtContactPerson.Text = Convert.ToString(row.Cells[2].Value);
                form.txtEmail.Text = Convert.ToString(row.Cells[3].Value);
                form.txtPhone.Text = Convert.ToString(row.Cells[4].Value);
                form.txtCity.Text = Convert.ToString(row.Cells[5].Value);
                form.cmbStatus.Text = Convert.ToInt32(row.Cells[10].Value) == 1 ? "Active" : "Inactive";

                if (form.ShowDialog() != DialogResult.OK)
                    return;

                try
                {
                    OracleDb.Execute(@"
                        UPDATE suppliers
                           SET supplier_name = :supplier_name,
                               contact_person = :contact_person,
                               email = :email,
                               phone = :phone,
                               address = :address,
                               status = CASE UPPER(:status) WHEN 'ACTIVE' THEN 1 WHEN 'INACTIVE' THEN 0 WHEN 'DRAFT' THEN 0 WHEN 'APPROVED' THEN 1 WHEN 'PARTIALLY_RECEIVED' THEN 2 WHEN 'RECEIVED' THEN 3 WHEN 'CANCELLED' THEN 4 WHEN 'CLOSED' THEN 5 ELSE status END
                         WHERE supplier_code = :supplier_code",
                        OracleDb.Parameter("supplier_name", form.SupplierName),
                        OracleDb.Parameter("contact_person", form.ContactPerson),
                        OracleDb.Parameter("email", form.Email),
                        OracleDb.Parameter("phone", form.Phone),
                        OracleDb.Parameter("address", form.Address),
                        OracleDb.Parameter("status", form.Status),
                        OracleDb.Parameter("supplier_code", code));
                    ReloadAfterChange();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Update Supplier Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnDeleteSupplier_Click(object sender, EventArgs e)
        {
            if (!RequirePermission("SUPPLIERS_DELETE"))
                return;

            if (dgvSuppliers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a supplier to deactivate.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string code = Convert.ToString(dgvSuppliers.SelectedRows[0].Cells[0].Value);
            if (MessageBox.Show("Deactivate supplier " + code + "?", "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                OracleDb.Execute(
                    "UPDATE suppliers SET status = 0 WHERE supplier_code = :supplier_code",
                    OracleDb.Parameter("supplier_code", code));
                ReloadAfterChange();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Deactivate Supplier Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // =========================================================
        // PRODUCT CRUD
        // =========================================================
        private void btnNewProduct_Click(object sender, EventArgs e)
        {
            if (!RequirePermission("PRODUCTS_ADD"))
                return;

            ShowProductEditor(null);
        }

        private void btnEditProduct_Click(object sender, EventArgs e)
        {
            if (!RequirePermission("PRODUCTS_EDIT"))
                return;

            if (dgvProducts.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a product to edit.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var row = dgvProducts.SelectedRows[0];
            ShowProductEditor(row);
        }

        private void ShowProductEditor(DataGridViewRow row)
        {
            bool isEdit = row != null;
            _editingProductCode = isEdit ? Convert.ToString(row.Cells[0].Value) : null;
            _txtProductCodeInline.Text = isEdit ? _editingProductCode : string.Empty;
            _txtProductCodeInline.ReadOnly = isEdit;
            _txtProductNameInline.Text = isEdit ? Convert.ToString(row.Cells[1].Value) : string.Empty;

            string category = isEdit ? Convert.ToString(row.Cells[2].Value) : "Other";
            if (_cboProductCategoryInline.Items.Contains(category))
                _cboProductCategoryInline.SelectedItem = category;
            else
                _cboProductCategoryInline.Text = category;

            _txtProductUnitPriceInline.Text = isEdit ? Convert.ToString(row.Cells[4].Value) : "0";
            decimal stock;
            _numProductStockInline.Value = isEdit &&
                decimal.TryParse(Convert.ToString(row.Cells[5].Value), out stock)
                ? Math.Max(_numProductStockInline.Minimum, Math.Min(_numProductStockInline.Maximum, stock))
                : 0;
            _chkProductActiveInline.Checked = true;
            _productEditorPanel.Visible = true;
            LayoutProductEditor();
            _txtProductNameInline.Focus();
        }

        private void HideProductEditor()
        {
            _editingProductCode = null;
            _txtProductCodeInline.ReadOnly = false;
            _txtProductCodeInline.Clear();
            _txtProductNameInline.Clear();
            _txtProductUnitPriceInline.Text = "0";
            _numProductStockInline.Value = 0;
            _chkProductActiveInline.Checked = true;
            _productEditorPanel.Visible = false;
            LayoutProductEditor();
        }

        private void btnSaveProductInline_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_editingProductCode) && !RequirePermission("PRODUCTS_ADD"))
                return;
            if (!string.IsNullOrWhiteSpace(_editingProductCode) && !RequirePermission("PRODUCTS_EDIT"))
                return;

            string code = _txtProductCodeInline.Text.Trim();
            string name = _txtProductNameInline.Text.Trim();
            string category = Convert.ToString(_cboProductCategoryInline.Text).Trim();
            decimal price;
            if (string.IsNullOrWhiteSpace(code))
            {
                MessageBox.Show("Product code is required.", "Products", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Product name is required.", "Products", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!decimal.TryParse(_txtProductUnitPriceInline.Text, out price) || price < 0)
            {
                MessageBox.Show("Unit price must be a valid number.", "Products", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (string.IsNullOrWhiteSpace(_editingProductCode))
                {
                    OracleDb.Execute(@"
                        INSERT INTO products (
                            product_id, product_code, product_name, category,
                            unit, unit_price, stock_qty, status
                        ) VALUES (
                            product_seq.NEXTVAL, :product_code, :product_name,
                            :category, 'UNIT', :unit_price, :stock_qty, :status
                        )",
                        OracleDb.Parameter("product_code", code),
                        OracleDb.Parameter("product_name", name),
                        OracleDb.Parameter("category", category),
                        OracleDb.Parameter("unit_price", price),
                        OracleDb.Parameter("stock_qty", _numProductStockInline.Value),
                        OracleDb.Parameter("status", _chkProductActiveInline.Checked ? 1 : 0));
                }
                else
                {
                    OracleDb.Execute(@"
                        UPDATE products
                           SET product_name = :product_name,
                               category = :category,
                               unit_price = :unit_price,
                               stock_qty = :stock_qty,
                               status = :status
                         WHERE product_code = :product_code",
                        OracleDb.Parameter("product_name", name),
                        OracleDb.Parameter("category", category),
                        OracleDb.Parameter("unit_price", price),
                        OracleDb.Parameter("stock_qty", _numProductStockInline.Value),
                        OracleDb.Parameter("status", _chkProductActiveInline.Checked ? 1 : 0),
                        OracleDb.Parameter("product_code", _editingProductCode));
                }

                HideProductEditor();
                LoadProducts();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Save Product Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDeleteProduct_Click(object sender, EventArgs e)
        {
            if (!RequirePermission("PRODUCTS_DELETE"))
                return;

            if (dgvProducts.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a product to deactivate.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string code = Convert.ToString(dgvProducts.SelectedRows[0].Cells[0].Value);
            if (MessageBox.Show("Deactivate product " + code + "?", "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                OracleDb.Execute(
                    "UPDATE products SET status = 0 WHERE product_code = :product_code",
                    OracleDb.Parameter("product_code", code));
                ReloadAfterChange();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Deactivate Product Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // =========================================================
        // GRID FORMATTING
        // =========================================================
        private static string PurchaseOrderStatusText(object value)
        {
            int code;
            if (!int.TryParse(Convert.ToString(value), out code))
                return Convert.ToString(value);

            switch (code)
            {
                case 0: return "DRAFT";
                case 1: return "APPROVED";
                case 2: return "PARTIALLY_RECEIVED";
                case 3: return "RECEIVED";
                case 4: return "CANCELLED";
                case 5: return "CLOSED";
                default: return Convert.ToString(value);
            }
        }
        private void ApplyStatusColor(DataGridView grid, int statusColumn)
        {
            grid.CellFormatting += (sender, e) =>
            {
                if (e.ColumnIndex != statusColumn || e.Value == null)
                    return;

                switch (Convert.ToString(e.Value).ToUpperInvariant())
                {
                    case "1":
                    case "APPROVED":
                    case "ACTIVE":
                    case "PAID":
                        e.CellStyle.ForeColor = Color.FromArgb(39, 174, 96);
                        break;
                    case "0":
                    case "PENDING":
                    case "PARTIAL":
                    case "PARTIALLY_RECEIVED":
                        e.CellStyle.ForeColor = Color.FromArgb(230, 126, 34);
                        break;
                    case "3":
                    case "RECEIVED":
                    case "CLOSED":
                        e.CellStyle.ForeColor = Color.FromArgb(49, 130, 206);
                        break;
                    case "4":
                    case "CANCELLED":
                    case "INACTIVE":
                    case "REJECTED":
                        e.CellStyle.ForeColor = Color.FromArgb(231, 76, 60);
                        break;
                }

                e.CellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            };
        }

        private void ApplyStockColor(DataGridView grid, int statusColumn)
        {
            grid.CellFormatting += (sender, e) =>
            {
                if (e.ColumnIndex != statusColumn || e.Value == null)
                    return;

                switch (Convert.ToString(e.Value).ToUpperInvariant())
                {
                    case "IN STOCK":
                        e.CellStyle.ForeColor = Color.FromArgb(39, 174, 96);
                        break;
                    case "LOW STOCK":
                        e.CellStyle.ForeColor = Color.FromArgb(230, 126, 34);
                        break;
                    case "OUT OF STOCK":
                        e.CellStyle.ForeColor = Color.FromArgb(231, 76, 60);
                        break;
                }

                e.CellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            };
        }

        // =========================================================
        // REPORTS
        // =========================================================
        private void btnGenerateReport_Click(object sender, EventArgs e)
        {
            if (!RequirePermission("REPORTS_GENERATE"))
                return;

            try
            {
                var from = dtpFrom.Value.Date;
                var to = dtpTo.Value.Date.AddDays(1);
                DataTable table;

                switch (cmbReportType.SelectedIndex)
                {
                    case 0:
                        table = OracleDb.Query(@"
                            SELECT po_no AS ""PO Number"", supplier_name AS ""Supplier"",
                                   po_date AS ""Order Date"", total_amount AS ""Total Amount"",
                                   status AS ""Status""
                              FROM vw_purchase_report
                             WHERE po_date >= :from_date AND po_date < :to_date
                             ORDER BY po_date DESC",
                            OracleDb.Parameter("from_date", from),
                            OracleDb.Parameter("to_date", to));
                        ApplyStatusColorAfterReport(table, 4);
                        SetSummary("Purchase Order Summary", table, from, to);
                        break;

                    case 1:
                        table = OracleDb.Query(@"
                            SELECT supplier_code AS ""Supplier Code"",
                                   supplier_name AS ""Supplier"",
                                   total_orders AS ""Total Orders"",
                                   total_order_amount AS ""Total Spend"",
                                   completed_orders AS ""Completed Orders"",
                                   last_order_date AS ""Last Order""
                              FROM vw_supplier_performance
                             ORDER BY total_order_amount DESC");
                        SetSummary("Supplier Performance", table, from, to);
                        break;

                    case 2:
                        table = OracleDb.Query(@"
                            SELECT p.category AS ""Category"",
                                   COUNT(DISTINCT po.po_id) AS ""No. of Orders"",
                                   SUM(poi.quantity) AS ""Total Items"",
                                   SUM(poi.subtotal) AS ""Total Spend"",
                                   ROUND(
                                       100 * SUM(poi.subtotal)
                                       / NULLIF(SUM(SUM(poi.subtotal)) OVER (), 0), 1
                                   ) AS ""Percent of Spend""
                              FROM purchase_order_items poi
                              JOIN purchase_orders po ON po.po_id = poi.po_id
                              JOIN products p ON p.product_id = poi.product_id
                             WHERE po.po_date >= :from_date
                               AND po.po_date < :to_date
                               AND po.status <> 4
                             GROUP BY p.category
                             ORDER BY SUM(poi.subtotal) DESC",
                            OracleDb.Parameter("from_date", from),
                            OracleDb.Parameter("to_date", to));
                        SetSummary("Spend by Category", table, from, to);
                        break;

                    case 3:
                        table = OracleDb.Query(@"
                            SELECT TO_CHAR(TRUNC(po_date, 'MM'), 'FMMonth YYYY') AS ""Month"",
                                   COUNT(*) AS ""No. of POs"",
                                   SUM(CASE WHEN status = 1 THEN 1 ELSE 0 END) AS ""Approved"",
                                   SUM(total_amount) AS ""Total Spend""
                              FROM purchase_orders
                             WHERE po_date >= :from_date
                               AND po_date < :to_date
                               AND status <> 4
                             GROUP BY TRUNC(po_date, 'MM')
                             ORDER BY TRUNC(po_date, 'MM')",
                            OracleDb.Parameter("from_date", from),
                            OracleDb.Parameter("to_date", to));
                        SetSummary("Monthly Expenditure", table, from, to);
                        break;

                    default:
                        table = OracleDb.Query(@"
                            SELECT pr.request_no AS ""Request No"",
                                   requester.full_name AS ""Requested By"",
                                   pr.request_date AS ""Date Submitted"",
                                   TRUNC(SYSDATE) - TRUNC(pr.request_date) AS ""Days Pending"",
                                   NVL(SUM(pri.quantity * pri.estimated_unit_price), 0) AS ""Estimated Amount"",
                                   a.decision AS ""Status""
                              FROM purchase_request_approvals a
                              JOIN purchase_requests pr ON pr.request_id = a.request_id
                              JOIN employees requester ON requester.employee_id = pr.requested_by
                              LEFT JOIN purchase_request_items pri ON pri.request_id = pr.request_id
                             WHERE a.decision = 0
                               AND pr.request_date >= :from_date
                               AND pr.request_date < :to_date
                             GROUP BY pr.request_no, requester.full_name, pr.request_date, a.decision
                             ORDER BY pr.request_date",
                            OracleDb.Parameter("from_date", from),
                            OracleDb.Parameter("to_date", to));
                        ApplyStatusColorAfterReport(table, 5);
                        SetSummary("Pending Approvals", table, from, to);
                        break;
                }

                dgvReport.DataSource = table;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Report Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExportReport_Click(object sender, EventArgs e)
        {
            if (!RequirePermission("REPORTS_EXPORT"))
                return;

            if (dgvReport.Rows.Count == 0 || dgvReport.DataSource == null)
            {
                MessageBox.Show("Please generate a report before exporting.", "No Report",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var rows = new List<DataGridViewRow>();
            foreach (DataGridViewRow row in dgvReport.Rows)
            {
                if (!row.IsNewRow)
                    rows.Add(row);
            }

            ExportGridRows(
                dgvReport,
                rows,
                "report-" + DateTime.Now.ToString("yyyyMMdd-HHmmss", CultureInfo.InvariantCulture) + ".csv",
                "Report exported.");
        }

        private void ExportGridRows(
            DataGridView grid,
            IEnumerable<DataGridViewRow> rows,
            string defaultFileName,
            string successMessage)
        {
            using (var dialog = new SaveFileDialog())
            {
                dialog.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
                dialog.FileName = defaultFileName;
                dialog.Title = "Export CSV";

                if (dialog.ShowDialog() != DialogResult.OK)
                    return;

                System.IO.File.WriteAllText(
                    dialog.FileName,
                    BuildCsv(grid, rows),
                    System.Text.Encoding.UTF8);

                MessageBox.Show(successMessage, "Export Complete",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private static string BuildCsv(DataGridView grid, IEnumerable<DataGridViewRow> rows)
        {
            var csv = new System.Text.StringBuilder();
            bool firstColumn = true;

            foreach (DataGridViewColumn column in grid.Columns)
            {
                if (!column.Visible)
                    continue;

                if (!firstColumn)
                    csv.Append(",");

                csv.Append(EscapeCsv(column.HeaderText));
                firstColumn = false;
            }
            csv.AppendLine();

            foreach (DataGridViewRow row in rows)
            {
                firstColumn = true;
                foreach (DataGridViewColumn column in grid.Columns)
                {
                    if (!column.Visible)
                        continue;

                    if (!firstColumn)
                        csv.Append(",");

                    csv.Append(EscapeCsv(Convert.ToString(row.Cells[column.Index].Value)));
                    firstColumn = false;
                }
                csv.AppendLine();
            }

            return csv.ToString();
        }

        private static string EscapeCsv(string value)
        {
            if (value == null)
                return string.Empty;

            return "\"" + value.Replace("\"", "\"\"") + "\"";
        }

        private static string SafeFileName(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return "selected";

            foreach (char invalid in System.IO.Path.GetInvalidFileNameChars())
                value = value.Replace(invalid, '-');

            return value;
        }

        private void ApplyStatusColorAfterReport(DataTable table, int statusColumn)
        {
            dgvReport.DataSource = table;
            ApplyStatusColor(dgvReport, statusColumn);
        }

        private void SetSummary(string title, DataTable table, DateTime from, DateTime to)
        {
            decimal total = 0;
            if (table.Columns.Contains("Total Amount"))
                total = SumColumn(table, "Total Amount");
            else if (table.Columns.Contains("Total Spend"))
                total = SumColumn(table, "Total Spend");
            else if (table.Columns.Contains("Estimated Amount"))
                total = SumColumn(table, "Estimated Amount");

            lblReportSummary.Text = string.Format(
                CultureInfo.CurrentCulture,
                "{0} | Period: {1:MMM dd, yyyy} - {2:MMM dd, yyyy} | Records: {3} | Amount: {4:C2}",
                title, from, to.AddDays(-1), table.Rows.Count, total);
        }

        private decimal SumColumn(DataTable table, string columnName)
        {
            decimal sum = 0;
            foreach (DataRow row in table.Rows)
            {
                if (row[columnName] != DBNull.Value)
                    sum += Convert.ToDecimal(row[columnName], CultureInfo.InvariantCulture);
            }
            return sum;
        }

        private static void SetDate(DateTimePicker picker, object value)
        {
            DateTime parsed;
            if (value != null && DateTime.TryParse(Convert.ToString(value), out parsed))
                picker.Value = parsed;
        }

        private sealed class UserEditorDialog : Form
        {
            private readonly ComboBox _cboEmployee;
            private readonly TextBox _txtUsername;
            private readonly TextBox _txtPassword;
            private readonly ComboBox _cboRole;
            private readonly CheckBox _chkActive;
            private readonly bool _passwordRequired;

            public UserEditorDialog(
                string title,
                DataTable employees,
                DataTable roles,
                int? selectedEmployeeId,
                int? selectedRoleId,
                string username,
                bool isActive)
            {
                _passwordRequired = string.IsNullOrWhiteSpace(username);

                Text = title;
                StartPosition = FormStartPosition.CenterParent;
                FormBorderStyle = FormBorderStyle.FixedDialog;
                MaximizeBox = false;
                MinimizeBox = false;
                ClientSize = new Size(520, 330);
                BackColor = Color.White;
                Font = new Font("Segoe UI", 9F);

                var layout = new TableLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    ColumnCount = 2,
                    RowCount = 6,
                    Padding = new Padding(18)
                };
                layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130F));
                layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
                layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
                layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
                layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
                layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
                layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
                layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
                Controls.Add(layout);

                _cboEmployee = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList };
                _txtUsername = new TextBox { Dock = DockStyle.Fill };
                _txtPassword = new TextBox { Dock = DockStyle.Fill, UseSystemPasswordChar = true };
                _cboRole = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList };
                _chkActive = new CheckBox { Text = "Active", Checked = isActive, Dock = DockStyle.Fill };

                AddLabel(layout, "Employee", 0);
                layout.Controls.Add(_cboEmployee, 1, 0);
                AddLabel(layout, "Username", 1);
                layout.Controls.Add(_txtUsername, 1, 1);
                AddLabel(layout, _passwordRequired ? "Password" : "New Password", 2);
                layout.Controls.Add(_txtPassword, 1, 2);
                AddLabel(layout, "Role", 3);
                layout.Controls.Add(_cboRole, 1, 3);
                AddLabel(layout, "Status", 4);
                layout.Controls.Add(_chkActive, 1, 4);

                var buttons = new FlowLayoutPanel
                {
                    Dock = DockStyle.Right,
                    FlowDirection = FlowDirection.LeftToRight,
                    Width = 210
                };
                var btnOk = new Button
                {
                    DialogResult = DialogResult.OK,
                    Text = "Save",
                    Width = 90,
                    Height = 34,
                    BackColor = Color.FromArgb(45, 101, 181),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat
                };
                var btnCancel = new Button
                {
                    DialogResult = DialogResult.Cancel,
                    Text = "Cancel",
                    Width = 90,
                    Height = 34,
                    FlatStyle = FlatStyle.Flat
                };
                btnOk.FlatAppearance.BorderSize = 0;
                buttons.Controls.Add(btnOk);
                buttons.Controls.Add(btnCancel);
                layout.Controls.Add(buttons, 1, 5);

                AcceptButton = btnOk;
                CancelButton = btnCancel;

                LoadCombo(_cboEmployee, employees, "EMPLOYEE_ID", "DISPLAY_NAME", selectedEmployeeId);
                LoadCombo(_cboRole, roles, "ROLE_ID", "DISPLAY_NAME", selectedRoleId);
                _txtUsername.Text = username ?? string.Empty;
            }

            public int EmployeeId
            {
                get { return ((ComboItem)_cboEmployee.SelectedItem).Id; }
            }

            public string Username
            {
                get { return _txtUsername.Text.Trim(); }
            }

            public string Password
            {
                get { return _txtPassword.Text; }
            }

            public int RoleId
            {
                get { return ((ComboItem)_cboRole.SelectedItem).Id; }
            }

            public bool IsActive
            {
                get { return _chkActive.Checked; }
            }

            protected override void OnFormClosing(FormClosingEventArgs e)
            {
                if (DialogResult == DialogResult.OK)
                {
                    if (_cboEmployee.SelectedItem == null)
                    {
                        MessageBox.Show("Please choose an employee.", "Users",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        e.Cancel = true;
                    }
                    else if (string.IsNullOrWhiteSpace(Username))
                    {
                        MessageBox.Show("Please enter username.", "Users",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        e.Cancel = true;
                    }
                    else if (_passwordRequired && string.IsNullOrWhiteSpace(Password))
                    {
                        MessageBox.Show("Please enter password.", "Users",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        e.Cancel = true;
                    }
                    else if (_cboRole.SelectedItem == null)
                    {
                        MessageBox.Show("Please choose a role.", "Users",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        e.Cancel = true;
                    }
                }

                base.OnFormClosing(e);
            }

            private static void AddLabel(TableLayoutPanel layout, string text, int row)
            {
                layout.Controls.Add(new Label
                {
                    AutoSize = false,
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Text = text
                }, 0, row);
            }

            private static void LoadCombo(
                ComboBox combo,
                DataTable table,
                string idColumn,
                string displayColumn,
                int? selectedId)
            {
                foreach (DataRow row in table.Rows)
                {
                    var item = new ComboItem(
                        Convert.ToInt32(row[idColumn]),
                        Convert.ToString(row[displayColumn]));
                    combo.Items.Add(item);

                    if (selectedId.HasValue && item.Id == selectedId.Value)
                        combo.SelectedItem = item;
                }

                if (combo.SelectedIndex < 0 && combo.Items.Count > 0)
                    combo.SelectedIndex = 0;
            }

            private sealed class ComboItem
            {
                public ComboItem(int id, string text)
                {
                    Id = id;
                    Text = text;
                }

                public int Id { get; private set; }
                private string Text { get; set; }

                public override string ToString()
                {
                    return Text;
                }
            }
        }

        private void lblCard1Value_Click(object sender, EventArgs e)
        {

        }
    }
}
