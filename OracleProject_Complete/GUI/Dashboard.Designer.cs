namespace OracleProject
{
    partial class Dashboard
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.timerClock = new System.Windows.Forms.Timer(this.components);
            // ── Sidebar ──────────────────────────────────────────────
            this.panelSidebar = new System.Windows.Forms.Panel();
            this.lblSidebarTitle = new System.Windows.Forms.Label();
            this.lblSidebarSub = new System.Windows.Forms.Label();
            this.panelSidebarDivider = new System.Windows.Forms.Panel();
            this.btnNavOverview = new System.Windows.Forms.Button();
            this.btnNavOrders = new System.Windows.Forms.Button();
            this.btnNavSuppliers = new System.Windows.Forms.Button();
            this.btnNavProducts = new System.Windows.Forms.Button();
            this.btnNavReports = new System.Windows.Forms.Button();
            this.btnNavSettings = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();
            // ── Top bar ──────────────────────────────────────────────
            this.panelTopBar = new System.Windows.Forms.Panel();
            this.lblPageTitle = new System.Windows.Forms.Label();
            this.lblUserGreeting = new System.Windows.Forms.Label();
            this.lblDateTime = new System.Windows.Forms.Label();
            // ── Content ──────────────────────────────────────────────
            this.panelContent = new System.Windows.Forms.Panel();
            // ── Overview page ────────────────────────────────────────
            this.pageOverview = new System.Windows.Forms.Panel();
            this.cardPanel1 = new System.Windows.Forms.Panel();
            this.panelCard1Accent = new System.Windows.Forms.Panel();
            this.lblCard1Title = new System.Windows.Forms.Label();
            this.lblCard1Value = new System.Windows.Forms.Label();
            this.lblCard1Sub = new System.Windows.Forms.Label();
            this.cardPanel2 = new System.Windows.Forms.Panel();
            this.panelCard2Accent = new System.Windows.Forms.Panel();
            this.lblCard2Title = new System.Windows.Forms.Label();
            this.lblCard2Value = new System.Windows.Forms.Label();
            this.lblCard2Sub = new System.Windows.Forms.Label();
            this.cardPanel3 = new System.Windows.Forms.Panel();
            this.panelCard3Accent = new System.Windows.Forms.Panel();
            this.lblCard3Title = new System.Windows.Forms.Label();
            this.lblCard3Value = new System.Windows.Forms.Label();
            this.lblCard3Sub = new System.Windows.Forms.Label();
            this.cardPanel4 = new System.Windows.Forms.Panel();
            this.panelCard4Accent = new System.Windows.Forms.Panel();
            this.lblCard4Title = new System.Windows.Forms.Label();
            this.lblCard4Value = new System.Windows.Forms.Label();
            this.lblCard4Sub = new System.Windows.Forms.Label();
            this.lblRecentOrdersTitle = new System.Windows.Forms.Label();
            this.dgvRecentOrders = new System.Windows.Forms.DataGridView();
            // ── Orders page ──────────────────────────────────────────
            this.pageOrders = new System.Windows.Forms.Panel();
            this.panelOrdersToolbar = new System.Windows.Forms.Panel();
            this.btnNewOrder = new System.Windows.Forms.Button();
            this.btnEditOrder = new System.Windows.Forms.Button();
            this.btnDeleteOrder = new System.Windows.Forms.Button();
            this.btnPrintOrder = new System.Windows.Forms.Button();
            this.lblOrderSearch = new System.Windows.Forms.Label();
            this.txtOrderSearch = new System.Windows.Forms.TextBox();
            this.lblOrderStatus = new System.Windows.Forms.Label();
            this.cmbOrderStatus = new System.Windows.Forms.ComboBox();
            this.dgvOrders = new System.Windows.Forms.DataGridView();
            // ── Suppliers page ───────────────────────────────────────
            this.pageSuppliers = new System.Windows.Forms.Panel();
            this.panelSuppliersToolbar = new System.Windows.Forms.Panel();
            this.btnNewSupplier = new System.Windows.Forms.Button();
            this.btnEditSupplier = new System.Windows.Forms.Button();
            this.btnDeleteSupplier = new System.Windows.Forms.Button();
            this.lblSupplierSearch = new System.Windows.Forms.Label();
            this.txtSupplierSearch = new System.Windows.Forms.TextBox();
            this.dgvSuppliers = new System.Windows.Forms.DataGridView();
            // ── Products page ────────────────────────────────────────
            this.pageProducts = new System.Windows.Forms.Panel();
            this.panelProductsToolbar = new System.Windows.Forms.Panel();
            this.btnNewProduct = new System.Windows.Forms.Button();
            this.btnEditProduct = new System.Windows.Forms.Button();
            this.btnDeleteProduct = new System.Windows.Forms.Button();
            this.lblProductSearch = new System.Windows.Forms.Label();
            this.txtProductSearch = new System.Windows.Forms.TextBox();
            this.lblProductCategory = new System.Windows.Forms.Label();
            this.cmbProductCategory = new System.Windows.Forms.ComboBox();
            this.dgvProducts = new System.Windows.Forms.DataGridView();
            // ── Reports page ─────────────────────────────────────────
            this.pageReports = new System.Windows.Forms.Panel();
            this.panelReportsFilter = new System.Windows.Forms.Panel();
            this.lblReportType = new System.Windows.Forms.Label();
            this.cmbReportType = new System.Windows.Forms.ComboBox();
            this.lblDateFrom = new System.Windows.Forms.Label();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.lblDateTo = new System.Windows.Forms.Label();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.btnGenerateReport = new System.Windows.Forms.Button();
            this.btnExportReport = new System.Windows.Forms.Button();
            this.lblReportSummary = new System.Windows.Forms.Label();
            this.dgvReport = new System.Windows.Forms.DataGridView();

            this.panelSidebar.SuspendLayout();
            this.panelTopBar.SuspendLayout();
            this.panelContent.SuspendLayout();
            this.pageOverview.SuspendLayout();
            this.cardPanel1.SuspendLayout();
            this.cardPanel2.SuspendLayout();
            this.cardPanel3.SuspendLayout();
            this.cardPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecentOrders)).BeginInit();
            this.pageOrders.SuspendLayout();
            this.panelOrdersToolbar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrders)).BeginInit();
            this.pageSuppliers.SuspendLayout();
            this.panelSuppliersToolbar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSuppliers)).BeginInit();
            this.pageProducts.SuspendLayout();
            this.panelProductsToolbar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProducts)).BeginInit();
            this.pageReports.SuspendLayout();
            this.panelReportsFilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReport)).BeginInit();
            this.SuspendLayout();

            // ════════════════════════════════════════════════════════
            // TIMER
            // ════════════════════════════════════════════════════════
            this.timerClock.Interval = 1000;
            this.timerClock.Tick += new System.EventHandler(this.timerClock_Tick);

            // ════════════════════════════════════════════════════════
            // SIDEBAR
            // ════════════════════════════════════════════════════════
            this.panelSidebar.BackColor = System.Drawing.Color.FromArgb(26, 32, 44);
            this.panelSidebar.Controls.Add(this.lblSidebarTitle);
            this.panelSidebar.Controls.Add(this.lblSidebarSub);
            this.panelSidebar.Controls.Add(this.panelSidebarDivider);
            this.panelSidebar.Controls.Add(this.btnNavOverview);
            this.panelSidebar.Controls.Add(this.btnNavOrders);
            this.panelSidebar.Controls.Add(this.btnNavSuppliers);
            this.panelSidebar.Controls.Add(this.btnNavProducts);
            this.panelSidebar.Controls.Add(this.btnNavReports);
            this.panelSidebar.Controls.Add(this.btnNavSettings);
            this.panelSidebar.Controls.Add(this.btnLogout);
            this.panelSidebar.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelSidebar.Name = "panelSidebar";
            this.panelSidebar.Size = new System.Drawing.Size(220, 700);
            this.panelSidebar.TabIndex = 0;

            this.lblSidebarTitle.AutoSize = false;
            this.lblSidebarTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblSidebarTitle.ForeColor = System.Drawing.Color.White;
            this.lblSidebarTitle.Location = new System.Drawing.Point(0, 25);
            this.lblSidebarTitle.Name = "lblSidebarTitle";
            this.lblSidebarTitle.Size = new System.Drawing.Size(220, 35);
            this.lblSidebarTitle.TabIndex = 0;
            this.lblSidebarTitle.Text = "ProcureEase";
            this.lblSidebarTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            this.lblSidebarSub.AutoSize = false;
            this.lblSidebarSub.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblSidebarSub.ForeColor = System.Drawing.Color.FromArgb(113, 128, 150);
            this.lblSidebarSub.Location = new System.Drawing.Point(0, 58);
            this.lblSidebarSub.Name = "lblSidebarSub";
            this.lblSidebarSub.Size = new System.Drawing.Size(220, 20);
            this.lblSidebarSub.TabIndex = 1;
            this.lblSidebarSub.Text = "Purchasing Management";
            this.lblSidebarSub.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            this.panelSidebarDivider.BackColor = System.Drawing.Color.FromArgb(45, 55, 72);
            this.panelSidebarDivider.Location = new System.Drawing.Point(15, 88);
            this.panelSidebarDivider.Name = "panelSidebarDivider";
            this.panelSidebarDivider.Size = new System.Drawing.Size(190, 1);
            this.panelSidebarDivider.TabIndex = 2;

            // btnNavOverview
            this.btnNavOverview.BackColor = System.Drawing.Color.FromArgb(45, 55, 72);
            this.btnNavOverview.FlatAppearance.BorderSize = 0;
            this.btnNavOverview.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(55, 65, 82);
            this.btnNavOverview.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNavOverview.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnNavOverview.ForeColor = System.Drawing.Color.White;
            this.btnNavOverview.Location = new System.Drawing.Point(0, 100);
            this.btnNavOverview.Name = "btnNavOverview";
            this.btnNavOverview.Size = new System.Drawing.Size(220, 45);
            this.btnNavOverview.TabIndex = 3;
            this.btnNavOverview.Text = "   \u25A6  Overview";
            this.btnNavOverview.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNavOverview.UseVisualStyleBackColor = false;
            this.btnNavOverview.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNavOverview.Click += new System.EventHandler(this.btnNavOverview_Click);

            // btnNavOrders
            this.btnNavOrders.BackColor = System.Drawing.Color.Transparent;
            this.btnNavOrders.FlatAppearance.BorderSize = 0;
            this.btnNavOrders.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(45, 55, 72);
            this.btnNavOrders.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNavOrders.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnNavOrders.ForeColor = System.Drawing.Color.FromArgb(160, 174, 192);
            this.btnNavOrders.Location = new System.Drawing.Point(0, 150);
            this.btnNavOrders.Name = "btnNavOrders";
            this.btnNavOrders.Size = new System.Drawing.Size(220, 45);
            this.btnNavOrders.TabIndex = 4;
            this.btnNavOrders.Text = "   \u25A4  Purchase Orders";
            this.btnNavOrders.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNavOrders.UseVisualStyleBackColor = false;
            this.btnNavOrders.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNavOrders.Click += new System.EventHandler(this.btnNavOrders_Click);

            // btnNavSuppliers
            this.btnNavSuppliers.BackColor = System.Drawing.Color.Transparent;
            this.btnNavSuppliers.FlatAppearance.BorderSize = 0;
            this.btnNavSuppliers.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(45, 55, 72);
            this.btnNavSuppliers.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNavSuppliers.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnNavSuppliers.ForeColor = System.Drawing.Color.FromArgb(160, 174, 192);
            this.btnNavSuppliers.Location = new System.Drawing.Point(0, 200);
            this.btnNavSuppliers.Name = "btnNavSuppliers";
            this.btnNavSuppliers.Size = new System.Drawing.Size(220, 45);
            this.btnNavSuppliers.TabIndex = 5;
            this.btnNavSuppliers.Text = "   \u25A3  Suppliers";
            this.btnNavSuppliers.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNavSuppliers.UseVisualStyleBackColor = false;
            this.btnNavSuppliers.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNavSuppliers.Click += new System.EventHandler(this.btnNavSuppliers_Click);

            // btnNavProducts
            this.btnNavProducts.BackColor = System.Drawing.Color.Transparent;
            this.btnNavProducts.FlatAppearance.BorderSize = 0;
            this.btnNavProducts.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(45, 55, 72);
            this.btnNavProducts.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNavProducts.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnNavProducts.ForeColor = System.Drawing.Color.FromArgb(160, 174, 192);
            this.btnNavProducts.Location = new System.Drawing.Point(0, 250);
            this.btnNavProducts.Name = "btnNavProducts";
            this.btnNavProducts.Size = new System.Drawing.Size(220, 45);
            this.btnNavProducts.TabIndex = 6;
            this.btnNavProducts.Text = "   \u25A1  Products";
            this.btnNavProducts.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNavProducts.UseVisualStyleBackColor = false;
            this.btnNavProducts.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNavProducts.Click += new System.EventHandler(this.btnNavProducts_Click);

            // btnNavReports
            this.btnNavReports.BackColor = System.Drawing.Color.Transparent;
            this.btnNavReports.FlatAppearance.BorderSize = 0;
            this.btnNavReports.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(45, 55, 72);
            this.btnNavReports.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNavReports.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnNavReports.ForeColor = System.Drawing.Color.FromArgb(160, 174, 192);
            this.btnNavReports.Location = new System.Drawing.Point(0, 300);
            this.btnNavReports.Name = "btnNavReports";
            this.btnNavReports.Size = new System.Drawing.Size(220, 45);
            this.btnNavReports.TabIndex = 7;
            this.btnNavReports.Text = "   \u25A5  Reports";
            this.btnNavReports.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNavReports.UseVisualStyleBackColor = false;
            this.btnNavReports.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNavReports.Click += new System.EventHandler(this.btnNavReports_Click);

            // btnNavSettings
            this.btnNavSettings.BackColor = System.Drawing.Color.Transparent;
            this.btnNavSettings.FlatAppearance.BorderSize = 0;
            this.btnNavSettings.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(45, 55, 72);
            this.btnNavSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNavSettings.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnNavSettings.ForeColor = System.Drawing.Color.FromArgb(160, 174, 192);
            this.btnNavSettings.Location = new System.Drawing.Point(0, 360);
            this.btnNavSettings.Name = "btnNavSettings";
            this.btnNavSettings.Size = new System.Drawing.Size(220, 45);
            this.btnNavSettings.TabIndex = 8;
            this.btnNavSettings.Text = "   \u2699  Settings";
            this.btnNavSettings.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNavSettings.UseVisualStyleBackColor = false;
            this.btnNavSettings.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNavSettings.Click += new System.EventHandler(this.btnNavSettings_Click);

            // btnLogout
            this.btnLogout.BackColor = System.Drawing.Color.FromArgb(197, 48, 48);
            this.btnLogout.FlatAppearance.BorderSize = 0;
            this.btnLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogout.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnLogout.ForeColor = System.Drawing.Color.White;
            this.btnLogout.Location = new System.Drawing.Point(15, 645);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(190, 36);
            this.btnLogout.TabIndex = 9;
            this.btnLogout.Text = "   \u2B73  Logout";
            this.btnLogout.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLogout.UseVisualStyleBackColor = false;
            this.btnLogout.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);

            // ════════════════════════════════════════════════════════
            // TOP BAR
            // ════════════════════════════════════════════════════════
            this.panelTopBar.BackColor = System.Drawing.Color.White;
            this.panelTopBar.Controls.Add(this.lblPageTitle);
            this.panelTopBar.Controls.Add(this.lblUserGreeting);
            this.panelTopBar.Controls.Add(this.lblDateTime);
            this.panelTopBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTopBar.Name = "panelTopBar";
            this.panelTopBar.Size = new System.Drawing.Size(1100, 65);
            this.panelTopBar.TabIndex = 0;

            this.lblPageTitle.AutoSize = false;
            this.lblPageTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblPageTitle.ForeColor = System.Drawing.Color.FromArgb(26, 32, 44);
            this.lblPageTitle.Location = new System.Drawing.Point(20, 15);
            this.lblPageTitle.Name = "lblPageTitle";
            this.lblPageTitle.Size = new System.Drawing.Size(400, 35);
            this.lblPageTitle.TabIndex = 0;
            this.lblPageTitle.Text = "Overview";

            this.lblUserGreeting.AutoSize = false;
            this.lblUserGreeting.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblUserGreeting.ForeColor = System.Drawing.Color.FromArgb(113, 128, 150);
            this.lblUserGreeting.Location = new System.Drawing.Point(750, 15);
            this.lblUserGreeting.Name = "lblUserGreeting";
            this.lblUserGreeting.Size = new System.Drawing.Size(330, 20);
            this.lblUserGreeting.TabIndex = 1;
            this.lblUserGreeting.Text = "Welcome, Admin";
            this.lblUserGreeting.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            this.lblDateTime.AutoSize = false;
            this.lblDateTime.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblDateTime.ForeColor = System.Drawing.Color.FromArgb(113, 128, 150);
            this.lblDateTime.Location = new System.Drawing.Point(750, 37);
            this.lblDateTime.Name = "lblDateTime";
            this.lblDateTime.Size = new System.Drawing.Size(330, 18);
            this.lblDateTime.TabIndex = 2;
            this.lblDateTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            // ════════════════════════════════════════════════════════
            // CONTENT PANEL
            // ════════════════════════════════════════════════════════
            this.panelContent.BackColor = System.Drawing.Color.FromArgb(247, 250, 252);
            this.panelContent.Controls.Add(this.pageOverview);
            this.panelContent.Controls.Add(this.pageOrders);
            this.panelContent.Controls.Add(this.pageSuppliers);
            this.panelContent.Controls.Add(this.pageProducts);
            this.panelContent.Controls.Add(this.pageReports);
            this.panelContent.Controls.Add(this.panelTopBar);
            this.panelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContent.Name = "panelContent";
            this.panelContent.TabIndex = 1;

            // ════════════════════════════════════════════════════════
            // PAGE: OVERVIEW
            // ════════════════════════════════════════════════════════
            this.pageOverview.BackColor = System.Drawing.Color.FromArgb(247, 250, 252);
            this.pageOverview.Controls.Add(this.cardPanel1);
            this.pageOverview.Controls.Add(this.cardPanel2);
            this.pageOverview.Controls.Add(this.cardPanel3);
            this.pageOverview.Controls.Add(this.cardPanel4);
            this.pageOverview.Controls.Add(this.lblRecentOrdersTitle);
            this.pageOverview.Controls.Add(this.dgvRecentOrders);
            this.pageOverview.Location = new System.Drawing.Point(0, 65);
            this.pageOverview.Name = "pageOverview";
            this.pageOverview.Size = new System.Drawing.Size(1100, 635);
            this.pageOverview.TabIndex = 1;
            this.pageOverview.Visible = true;

            // ── Card 1 ───────────────────────────────────────────────
            this.cardPanel1.BackColor = System.Drawing.Color.White;
            this.cardPanel1.Controls.Add(this.panelCard1Accent);
            this.cardPanel1.Controls.Add(this.lblCard1Title);
            this.cardPanel1.Controls.Add(this.lblCard1Value);
            this.cardPanel1.Controls.Add(this.lblCard1Sub);
            this.cardPanel1.Location = new System.Drawing.Point(20, 20);
            this.cardPanel1.Name = "cardPanel1";
            this.cardPanel1.Size = new System.Drawing.Size(250, 130);
            this.cardPanel1.TabIndex = 0;

            this.panelCard1Accent.BackColor = System.Drawing.Color.FromArgb(49, 130, 206);
            this.panelCard1Accent.Location = new System.Drawing.Point(0, 0);
            this.panelCard1Accent.Name = "panelCard1Accent";
            this.panelCard1Accent.Size = new System.Drawing.Size(5, 130);
            this.panelCard1Accent.TabIndex = 0;

            this.lblCard1Title.AutoSize = false;
            this.lblCard1Title.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblCard1Title.ForeColor = System.Drawing.Color.FromArgb(113, 128, 150);
            this.lblCard1Title.Location = new System.Drawing.Point(20, 18);
            this.lblCard1Title.Name = "lblCard1Title";
            this.lblCard1Title.Size = new System.Drawing.Size(220, 20);
            this.lblCard1Title.TabIndex = 1;
            this.lblCard1Title.Text = "TOTAL ORDERS";

            this.lblCard1Value.AutoSize = false;
            this.lblCard1Value.Font = new System.Drawing.Font("Segoe UI", 26F, System.Drawing.FontStyle.Bold);
            this.lblCard1Value.ForeColor = System.Drawing.Color.FromArgb(49, 130, 206);
            this.lblCard1Value.Location = new System.Drawing.Point(20, 40);
            this.lblCard1Value.Name = "lblCard1Value";
            this.lblCard1Value.Size = new System.Drawing.Size(220, 52);
            this.lblCard1Value.TabIndex = 2;
            this.lblCard1Value.Text = "248";

            this.lblCard1Sub.AutoSize = false;
            this.lblCard1Sub.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblCard1Sub.ForeColor = System.Drawing.Color.FromArgb(160, 174, 192);
            this.lblCard1Sub.Location = new System.Drawing.Point(20, 98);
            this.lblCard1Sub.Name = "lblCard1Sub";
            this.lblCard1Sub.Size = new System.Drawing.Size(220, 20);
            this.lblCard1Sub.TabIndex = 3;
            this.lblCard1Sub.Text = "This Month";

            // ── Card 2 ───────────────────────────────────────────────
            this.cardPanel2.BackColor = System.Drawing.Color.White;
            this.cardPanel2.Controls.Add(this.panelCard2Accent);
            this.cardPanel2.Controls.Add(this.lblCard2Title);
            this.cardPanel2.Controls.Add(this.lblCard2Value);
            this.cardPanel2.Controls.Add(this.lblCard2Sub);
            this.cardPanel2.Location = new System.Drawing.Point(290, 20);
            this.cardPanel2.Name = "cardPanel2";
            this.cardPanel2.Size = new System.Drawing.Size(250, 130);
            this.cardPanel2.TabIndex = 1;

            this.panelCard2Accent.BackColor = System.Drawing.Color.FromArgb(237, 137, 54);
            this.panelCard2Accent.Location = new System.Drawing.Point(0, 0);
            this.panelCard2Accent.Name = "panelCard2Accent";
            this.panelCard2Accent.Size = new System.Drawing.Size(5, 130);
            this.panelCard2Accent.TabIndex = 0;

            this.lblCard2Title.AutoSize = false;
            this.lblCard2Title.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblCard2Title.ForeColor = System.Drawing.Color.FromArgb(113, 128, 150);
            this.lblCard2Title.Location = new System.Drawing.Point(20, 18);
            this.lblCard2Title.Name = "lblCard2Title";
            this.lblCard2Title.Size = new System.Drawing.Size(220, 20);
            this.lblCard2Title.TabIndex = 1;
            this.lblCard2Title.Text = "PENDING APPROVAL";

            this.lblCard2Value.AutoSize = false;
            this.lblCard2Value.Font = new System.Drawing.Font("Segoe UI", 26F, System.Drawing.FontStyle.Bold);
            this.lblCard2Value.ForeColor = System.Drawing.Color.FromArgb(237, 137, 54);
            this.lblCard2Value.Location = new System.Drawing.Point(20, 40);
            this.lblCard2Value.Name = "lblCard2Value";
            this.lblCard2Value.Size = new System.Drawing.Size(220, 52);
            this.lblCard2Value.TabIndex = 2;
            this.lblCard2Value.Text = "17";

            this.lblCard2Sub.AutoSize = false;
            this.lblCard2Sub.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblCard2Sub.ForeColor = System.Drawing.Color.FromArgb(160, 174, 192);
            this.lblCard2Sub.Location = new System.Drawing.Point(20, 98);
            this.lblCard2Sub.Name = "lblCard2Sub";
            this.lblCard2Sub.Size = new System.Drawing.Size(220, 20);
            this.lblCard2Sub.TabIndex = 3;
            this.lblCard2Sub.Text = "Requires Action";

            // ── Card 3 ───────────────────────────────────────────────
            this.cardPanel3.BackColor = System.Drawing.Color.White;
            this.cardPanel3.Controls.Add(this.panelCard3Accent);
            this.cardPanel3.Controls.Add(this.lblCard3Title);
            this.cardPanel3.Controls.Add(this.lblCard3Value);
            this.cardPanel3.Controls.Add(this.lblCard3Sub);
            this.cardPanel3.Location = new System.Drawing.Point(560, 20);
            this.cardPanel3.Name = "cardPanel3";
            this.cardPanel3.Size = new System.Drawing.Size(250, 130);
            this.cardPanel3.TabIndex = 2;

            this.panelCard3Accent.BackColor = System.Drawing.Color.FromArgb(72, 187, 120);
            this.panelCard3Accent.Location = new System.Drawing.Point(0, 0);
            this.panelCard3Accent.Name = "panelCard3Accent";
            this.panelCard3Accent.Size = new System.Drawing.Size(5, 130);
            this.panelCard3Accent.TabIndex = 0;

            this.lblCard3Title.AutoSize = false;
            this.lblCard3Title.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblCard3Title.ForeColor = System.Drawing.Color.FromArgb(113, 128, 150);
            this.lblCard3Title.Location = new System.Drawing.Point(20, 18);
            this.lblCard3Title.Name = "lblCard3Title";
            this.lblCard3Title.Size = new System.Drawing.Size(220, 20);
            this.lblCard3Title.TabIndex = 1;
            this.lblCard3Title.Text = "TOTAL SUPPLIERS";

            this.lblCard3Value.AutoSize = false;
            this.lblCard3Value.Font = new System.Drawing.Font("Segoe UI", 26F, System.Drawing.FontStyle.Bold);
            this.lblCard3Value.ForeColor = System.Drawing.Color.FromArgb(72, 187, 120);
            this.lblCard3Value.Location = new System.Drawing.Point(20, 40);
            this.lblCard3Value.Name = "lblCard3Value";
            this.lblCard3Value.Size = new System.Drawing.Size(220, 52);
            this.lblCard3Value.TabIndex = 2;
            this.lblCard3Value.Text = "64";

            this.lblCard3Sub.AutoSize = false;
            this.lblCard3Sub.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblCard3Sub.ForeColor = System.Drawing.Color.FromArgb(160, 174, 192);
            this.lblCard3Sub.Location = new System.Drawing.Point(20, 98);
            this.lblCard3Sub.Name = "lblCard3Sub";
            this.lblCard3Sub.Size = new System.Drawing.Size(220, 20);
            this.lblCard3Sub.TabIndex = 3;
            this.lblCard3Sub.Text = "Active Vendors";

            // ── Card 4 ───────────────────────────────────────────────
            this.cardPanel4.BackColor = System.Drawing.Color.White;
            this.cardPanel4.Controls.Add(this.panelCard4Accent);
            this.cardPanel4.Controls.Add(this.lblCard4Title);
            this.cardPanel4.Controls.Add(this.lblCard4Value);
            this.cardPanel4.Controls.Add(this.lblCard4Sub);
            this.cardPanel4.Location = new System.Drawing.Point(830, 20);
            this.cardPanel4.Name = "cardPanel4";
            this.cardPanel4.Size = new System.Drawing.Size(250, 130);
            this.cardPanel4.TabIndex = 3;

            this.panelCard4Accent.BackColor = System.Drawing.Color.FromArgb(159, 122, 234);
            this.panelCard4Accent.Location = new System.Drawing.Point(0, 0);
            this.panelCard4Accent.Name = "panelCard4Accent";
            this.panelCard4Accent.Size = new System.Drawing.Size(5, 130);
            this.panelCard4Accent.TabIndex = 0;

            this.lblCard4Title.AutoSize = false;
            this.lblCard4Title.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblCard4Title.ForeColor = System.Drawing.Color.FromArgb(113, 128, 150);
            this.lblCard4Title.Location = new System.Drawing.Point(20, 18);
            this.lblCard4Title.Name = "lblCard4Title";
            this.lblCard4Title.Size = new System.Drawing.Size(220, 20);
            this.lblCard4Title.TabIndex = 1;
            this.lblCard4Title.Text = "MONTHLY SPEND";

            this.lblCard4Value.AutoSize = false;
            this.lblCard4Value.Font = new System.Drawing.Font("Segoe UI", 22F, System.Drawing.FontStyle.Bold);
            this.lblCard4Value.ForeColor = System.Drawing.Color.FromArgb(159, 122, 234);
            this.lblCard4Value.Location = new System.Drawing.Point(20, 40);
            this.lblCard4Value.Name = "lblCard4Value";
            this.lblCard4Value.Size = new System.Drawing.Size(220, 52);
            this.lblCard4Value.TabIndex = 2;
            this.lblCard4Value.Text = "$84,320";

            this.lblCard4Sub.AutoSize = false;
            this.lblCard4Sub.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblCard4Sub.ForeColor = System.Drawing.Color.FromArgb(160, 174, 192);
            this.lblCard4Sub.Location = new System.Drawing.Point(20, 98);
            this.lblCard4Sub.Name = "lblCard4Sub";
            this.lblCard4Sub.Size = new System.Drawing.Size(220, 20);
            this.lblCard4Sub.TabIndex = 3;
            this.lblCard4Sub.Text = "+5.2% vs Last Month";

            // ── Recent Orders grid ───────────────────────────────────
            this.lblRecentOrdersTitle.AutoSize = false;
            this.lblRecentOrdersTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblRecentOrdersTitle.ForeColor = System.Drawing.Color.FromArgb(26, 32, 44);
            this.lblRecentOrdersTitle.Location = new System.Drawing.Point(20, 170);
            this.lblRecentOrdersTitle.Name = "lblRecentOrdersTitle";
            this.lblRecentOrdersTitle.Size = new System.Drawing.Size(300, 30);
            this.lblRecentOrdersTitle.TabIndex = 4;
            this.lblRecentOrdersTitle.Text = "Recent Purchase Orders";

            this.dgvRecentOrders.AllowUserToAddRows = false;
            this.dgvRecentOrders.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvRecentOrders.BackgroundColor = System.Drawing.Color.White;
            this.dgvRecentOrders.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvRecentOrders.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(247, 250, 252);
            this.dgvRecentOrders.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.dgvRecentOrders.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(113, 128, 150);
            this.dgvRecentOrders.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.dgvRecentOrders.EnableHeadersVisualStyles = false;
            this.dgvRecentOrders.GridColor = System.Drawing.Color.FromArgb(226, 232, 240);
            this.dgvRecentOrders.Location = new System.Drawing.Point(20, 205);
            this.dgvRecentOrders.Name = "dgvRecentOrders";
            this.dgvRecentOrders.ReadOnly = true;
            this.dgvRecentOrders.RowHeadersVisible = false;
            this.dgvRecentOrders.RowTemplate.Height = 36;
            this.dgvRecentOrders.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvRecentOrders.Size = new System.Drawing.Size(1055, 400);
            this.dgvRecentOrders.TabIndex = 5;

            // ════════════════════════════════════════════════════════
            // PAGE: PURCHASE ORDERS
            // ════════════════════════════════════════════════════════
            this.pageOrders.BackColor = System.Drawing.Color.FromArgb(247, 250, 252);
            this.pageOrders.Controls.Add(this.panelOrdersToolbar);
            this.pageOrders.Controls.Add(this.dgvOrders);
            this.pageOrders.Location = new System.Drawing.Point(0, 65);
            this.pageOrders.Name = "pageOrders";
            this.pageOrders.Size = new System.Drawing.Size(1100, 635);
            this.pageOrders.TabIndex = 2;
            this.pageOrders.Visible = false;

            this.panelOrdersToolbar.BackColor = System.Drawing.Color.White;
            this.panelOrdersToolbar.Controls.Add(this.btnNewOrder);
            this.panelOrdersToolbar.Controls.Add(this.btnEditOrder);
            this.panelOrdersToolbar.Controls.Add(this.btnDeleteOrder);
            this.panelOrdersToolbar.Controls.Add(this.btnPrintOrder);
            this.panelOrdersToolbar.Controls.Add(this.lblOrderSearch);
            this.panelOrdersToolbar.Controls.Add(this.txtOrderSearch);
            this.panelOrdersToolbar.Controls.Add(this.lblOrderStatus);
            this.panelOrdersToolbar.Controls.Add(this.cmbOrderStatus);
            this.panelOrdersToolbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelOrdersToolbar.Name = "panelOrdersToolbar";
            this.panelOrdersToolbar.Size = new System.Drawing.Size(1100, 60);
            this.panelOrdersToolbar.TabIndex = 0;

            this.btnNewOrder.BackColor = System.Drawing.Color.FromArgb(49, 130, 206);
            this.btnNewOrder.FlatAppearance.BorderSize = 0;
            this.btnNewOrder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNewOrder.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnNewOrder.ForeColor = System.Drawing.Color.White;
            this.btnNewOrder.Location = new System.Drawing.Point(10, 14);
            this.btnNewOrder.Name = "btnNewOrder";
            this.btnNewOrder.Size = new System.Drawing.Size(110, 32);
            this.btnNewOrder.TabIndex = 0;
            this.btnNewOrder.Text = "New Order";
            this.btnNewOrder.UseVisualStyleBackColor = false;
            this.btnNewOrder.Cursor = System.Windows.Forms.Cursors.Hand;

            this.btnEditOrder.BackColor = System.Drawing.Color.FromArgb(72, 187, 120);
            this.btnEditOrder.FlatAppearance.BorderSize = 0;
            this.btnEditOrder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEditOrder.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnEditOrder.ForeColor = System.Drawing.Color.White;
            this.btnEditOrder.Location = new System.Drawing.Point(130, 14);
            this.btnEditOrder.Name = "btnEditOrder";
            this.btnEditOrder.Size = new System.Drawing.Size(80, 32);
            this.btnEditOrder.TabIndex = 1;
            this.btnEditOrder.Text = "Edit";
            this.btnEditOrder.UseVisualStyleBackColor = false;
            this.btnEditOrder.Cursor = System.Windows.Forms.Cursors.Hand;

            this.btnDeleteOrder.BackColor = System.Drawing.Color.FromArgb(245, 101, 101);
            this.btnDeleteOrder.FlatAppearance.BorderSize = 0;
            this.btnDeleteOrder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteOrder.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnDeleteOrder.ForeColor = System.Drawing.Color.White;
            this.btnDeleteOrder.Location = new System.Drawing.Point(220, 14);
            this.btnDeleteOrder.Name = "btnDeleteOrder";
            this.btnDeleteOrder.Size = new System.Drawing.Size(80, 32);
            this.btnDeleteOrder.TabIndex = 2;
            this.btnDeleteOrder.Text = "Delete";
            this.btnDeleteOrder.UseVisualStyleBackColor = false;
            this.btnDeleteOrder.Cursor = System.Windows.Forms.Cursors.Hand;

            this.btnPrintOrder.BackColor = System.Drawing.Color.FromArgb(113, 128, 150);
            this.btnPrintOrder.FlatAppearance.BorderSize = 0;
            this.btnPrintOrder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrintOrder.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnPrintOrder.ForeColor = System.Drawing.Color.White;
            this.btnPrintOrder.Location = new System.Drawing.Point(310, 14);
            this.btnPrintOrder.Name = "btnPrintOrder";
            this.btnPrintOrder.Size = new System.Drawing.Size(110, 32);
            this.btnPrintOrder.TabIndex = 3;
            this.btnPrintOrder.Text = "Print / Export";
            this.btnPrintOrder.UseVisualStyleBackColor = false;
            this.btnPrintOrder.Cursor = System.Windows.Forms.Cursors.Hand;

            this.lblOrderSearch.AutoSize = true;
            this.lblOrderSearch.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblOrderSearch.ForeColor = System.Drawing.Color.Gray;
            this.lblOrderSearch.Location = new System.Drawing.Point(445, 22);
            this.lblOrderSearch.Name = "lblOrderSearch";
            this.lblOrderSearch.TabIndex = 4;
            this.lblOrderSearch.Text = "Search:";

            this.txtOrderSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtOrderSearch.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtOrderSearch.Location = new System.Drawing.Point(498, 18);
            this.txtOrderSearch.Name = "txtOrderSearch";
            this.txtOrderSearch.Size = new System.Drawing.Size(200, 25);
            this.txtOrderSearch.TabIndex = 5;

            this.lblOrderStatus.AutoSize = true;
            this.lblOrderStatus.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblOrderStatus.ForeColor = System.Drawing.Color.Gray;
            this.lblOrderStatus.Location = new System.Drawing.Point(714, 22);
            this.lblOrderStatus.Name = "lblOrderStatus";
            this.lblOrderStatus.TabIndex = 6;
            this.lblOrderStatus.Text = "Status:";

            this.cmbOrderStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbOrderStatus.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.cmbOrderStatus.Items.AddRange(new object[] { "All", "Pending", "Approved", "Received", "Cancelled" });
            this.cmbOrderStatus.Location = new System.Drawing.Point(762, 18);
            this.cmbOrderStatus.Name = "cmbOrderStatus";
            this.cmbOrderStatus.Size = new System.Drawing.Size(140, 25);
            this.cmbOrderStatus.TabIndex = 7;
            this.cmbOrderStatus.SelectedIndex = 0;

            this.dgvOrders.AllowUserToAddRows = false;
            this.dgvOrders.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvOrders.BackgroundColor = System.Drawing.Color.White;
            this.dgvOrders.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvOrders.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(247, 250, 252);
            this.dgvOrders.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.dgvOrders.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(113, 128, 150);
            this.dgvOrders.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.dgvOrders.EnableHeadersVisualStyles = false;
            this.dgvOrders.GridColor = System.Drawing.Color.FromArgb(226, 232, 240);
            this.dgvOrders.Location = new System.Drawing.Point(0, 60);
            this.dgvOrders.Name = "dgvOrders";
            this.dgvOrders.ReadOnly = true;
            this.dgvOrders.RowHeadersVisible = false;
            this.dgvOrders.RowTemplate.Height = 36;
            this.dgvOrders.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvOrders.Size = new System.Drawing.Size(1100, 575);
            this.dgvOrders.TabIndex = 1;

            // ════════════════════════════════════════════════════════
            // PAGE: SUPPLIERS
            // ════════════════════════════════════════════════════════
            this.pageSuppliers.BackColor = System.Drawing.Color.FromArgb(247, 250, 252);
            this.pageSuppliers.Controls.Add(this.panelSuppliersToolbar);
            this.pageSuppliers.Controls.Add(this.dgvSuppliers);
            this.pageSuppliers.Location = new System.Drawing.Point(0, 65);
            this.pageSuppliers.Name = "pageSuppliers";
            this.pageSuppliers.Size = new System.Drawing.Size(1100, 635);
            this.pageSuppliers.TabIndex = 3;
            this.pageSuppliers.Visible = false;

            this.panelSuppliersToolbar.BackColor = System.Drawing.Color.White;
            this.panelSuppliersToolbar.Controls.Add(this.btnNewSupplier);
            this.panelSuppliersToolbar.Controls.Add(this.btnEditSupplier);
            this.panelSuppliersToolbar.Controls.Add(this.btnDeleteSupplier);
            this.panelSuppliersToolbar.Controls.Add(this.lblSupplierSearch);
            this.panelSuppliersToolbar.Controls.Add(this.txtSupplierSearch);
            this.panelSuppliersToolbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSuppliersToolbar.Name = "panelSuppliersToolbar";
            this.panelSuppliersToolbar.Size = new System.Drawing.Size(1100, 60);
            this.panelSuppliersToolbar.TabIndex = 0;

            this.btnNewSupplier.BackColor = System.Drawing.Color.FromArgb(49, 130, 206);
            this.btnNewSupplier.FlatAppearance.BorderSize = 0;
            this.btnNewSupplier.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNewSupplier.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnNewSupplier.ForeColor = System.Drawing.Color.White;
            this.btnNewSupplier.Location = new System.Drawing.Point(10, 14);
            this.btnNewSupplier.Name = "btnNewSupplier";
            this.btnNewSupplier.Size = new System.Drawing.Size(120, 32);
            this.btnNewSupplier.TabIndex = 0;
            this.btnNewSupplier.Text = "Add Supplier";
            this.btnNewSupplier.UseVisualStyleBackColor = false;
            this.btnNewSupplier.Cursor = System.Windows.Forms.Cursors.Hand;

            this.btnEditSupplier.BackColor = System.Drawing.Color.FromArgb(72, 187, 120);
            this.btnEditSupplier.FlatAppearance.BorderSize = 0;
            this.btnEditSupplier.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEditSupplier.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnEditSupplier.ForeColor = System.Drawing.Color.White;
            this.btnEditSupplier.Location = new System.Drawing.Point(140, 14);
            this.btnEditSupplier.Name = "btnEditSupplier";
            this.btnEditSupplier.Size = new System.Drawing.Size(80, 32);
            this.btnEditSupplier.TabIndex = 1;
            this.btnEditSupplier.Text = "Edit";
            this.btnEditSupplier.UseVisualStyleBackColor = false;
            this.btnEditSupplier.Cursor = System.Windows.Forms.Cursors.Hand;

            this.btnDeleteSupplier.BackColor = System.Drawing.Color.FromArgb(245, 101, 101);
            this.btnDeleteSupplier.FlatAppearance.BorderSize = 0;
            this.btnDeleteSupplier.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteSupplier.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnDeleteSupplier.ForeColor = System.Drawing.Color.White;
            this.btnDeleteSupplier.Location = new System.Drawing.Point(230, 14);
            this.btnDeleteSupplier.Name = "btnDeleteSupplier";
            this.btnDeleteSupplier.Size = new System.Drawing.Size(80, 32);
            this.btnDeleteSupplier.TabIndex = 2;
            this.btnDeleteSupplier.Text = "Delete";
            this.btnDeleteSupplier.UseVisualStyleBackColor = false;
            this.btnDeleteSupplier.Cursor = System.Windows.Forms.Cursors.Hand;

            this.lblSupplierSearch.AutoSize = true;
            this.lblSupplierSearch.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblSupplierSearch.ForeColor = System.Drawing.Color.Gray;
            this.lblSupplierSearch.Location = new System.Drawing.Point(360, 22);
            this.lblSupplierSearch.Name = "lblSupplierSearch";
            this.lblSupplierSearch.TabIndex = 3;
            this.lblSupplierSearch.Text = "Search:";

            this.txtSupplierSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSupplierSearch.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtSupplierSearch.Location = new System.Drawing.Point(413, 18);
            this.txtSupplierSearch.Name = "txtSupplierSearch";
            this.txtSupplierSearch.Size = new System.Drawing.Size(220, 25);
            this.txtSupplierSearch.TabIndex = 4;

            this.dgvSuppliers.AllowUserToAddRows = false;
            this.dgvSuppliers.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvSuppliers.BackgroundColor = System.Drawing.Color.White;
            this.dgvSuppliers.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvSuppliers.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(247, 250, 252);
            this.dgvSuppliers.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.dgvSuppliers.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(113, 128, 150);
            this.dgvSuppliers.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.dgvSuppliers.EnableHeadersVisualStyles = false;
            this.dgvSuppliers.GridColor = System.Drawing.Color.FromArgb(226, 232, 240);
            this.dgvSuppliers.Location = new System.Drawing.Point(0, 60);
            this.dgvSuppliers.Name = "dgvSuppliers";
            this.dgvSuppliers.ReadOnly = true;
            this.dgvSuppliers.RowHeadersVisible = false;
            this.dgvSuppliers.RowTemplate.Height = 36;
            this.dgvSuppliers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSuppliers.Size = new System.Drawing.Size(1100, 575);
            this.dgvSuppliers.TabIndex = 1;

            // ════════════════════════════════════════════════════════
            // PAGE: PRODUCTS
            // ════════════════════════════════════════════════════════
            this.pageProducts.BackColor = System.Drawing.Color.FromArgb(247, 250, 252);
            this.pageProducts.Controls.Add(this.panelProductsToolbar);
            this.pageProducts.Controls.Add(this.dgvProducts);
            this.pageProducts.Location = new System.Drawing.Point(0, 65);
            this.pageProducts.Name = "pageProducts";
            this.pageProducts.Size = new System.Drawing.Size(1100, 635);
            this.pageProducts.TabIndex = 4;
            this.pageProducts.Visible = false;

            this.panelProductsToolbar.BackColor = System.Drawing.Color.White;
            this.panelProductsToolbar.Controls.Add(this.btnNewProduct);
            this.panelProductsToolbar.Controls.Add(this.btnEditProduct);
            this.panelProductsToolbar.Controls.Add(this.btnDeleteProduct);
            this.panelProductsToolbar.Controls.Add(this.lblProductSearch);
            this.panelProductsToolbar.Controls.Add(this.txtProductSearch);
            this.panelProductsToolbar.Controls.Add(this.lblProductCategory);
            this.panelProductsToolbar.Controls.Add(this.cmbProductCategory);
            this.panelProductsToolbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelProductsToolbar.Name = "panelProductsToolbar";
            this.panelProductsToolbar.Size = new System.Drawing.Size(1100, 60);
            this.panelProductsToolbar.TabIndex = 0;

            this.btnNewProduct.BackColor = System.Drawing.Color.FromArgb(49, 130, 206);
            this.btnNewProduct.FlatAppearance.BorderSize = 0;
            this.btnNewProduct.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNewProduct.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnNewProduct.ForeColor = System.Drawing.Color.White;
            this.btnNewProduct.Location = new System.Drawing.Point(10, 14);
            this.btnNewProduct.Name = "btnNewProduct";
            this.btnNewProduct.Size = new System.Drawing.Size(120, 32);
            this.btnNewProduct.TabIndex = 0;
            this.btnNewProduct.Text = "Add Product";
            this.btnNewProduct.UseVisualStyleBackColor = false;
            this.btnNewProduct.Cursor = System.Windows.Forms.Cursors.Hand;

            this.btnEditProduct.BackColor = System.Drawing.Color.FromArgb(72, 187, 120);
            this.btnEditProduct.FlatAppearance.BorderSize = 0;
            this.btnEditProduct.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEditProduct.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnEditProduct.ForeColor = System.Drawing.Color.White;
            this.btnEditProduct.Location = new System.Drawing.Point(140, 14);
            this.btnEditProduct.Name = "btnEditProduct";
            this.btnEditProduct.Size = new System.Drawing.Size(80, 32);
            this.btnEditProduct.TabIndex = 1;
            this.btnEditProduct.Text = "Edit";
            this.btnEditProduct.UseVisualStyleBackColor = false;
            this.btnEditProduct.Cursor = System.Windows.Forms.Cursors.Hand;

            this.btnDeleteProduct.BackColor = System.Drawing.Color.FromArgb(245, 101, 101);
            this.btnDeleteProduct.FlatAppearance.BorderSize = 0;
            this.btnDeleteProduct.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteProduct.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnDeleteProduct.ForeColor = System.Drawing.Color.White;
            this.btnDeleteProduct.Location = new System.Drawing.Point(230, 14);
            this.btnDeleteProduct.Name = "btnDeleteProduct";
            this.btnDeleteProduct.Size = new System.Drawing.Size(80, 32);
            this.btnDeleteProduct.TabIndex = 2;
            this.btnDeleteProduct.Text = "Delete";
            this.btnDeleteProduct.UseVisualStyleBackColor = false;
            this.btnDeleteProduct.Cursor = System.Windows.Forms.Cursors.Hand;

            this.lblProductSearch.AutoSize = true;
            this.lblProductSearch.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblProductSearch.ForeColor = System.Drawing.Color.Gray;
            this.lblProductSearch.Location = new System.Drawing.Point(330, 22);
            this.lblProductSearch.Name = "lblProductSearch";
            this.lblProductSearch.TabIndex = 3;
            this.lblProductSearch.Text = "Search:";

            this.txtProductSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtProductSearch.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.txtProductSearch.Location = new System.Drawing.Point(383, 18);
            this.txtProductSearch.Name = "txtProductSearch";
            this.txtProductSearch.Size = new System.Drawing.Size(180, 25);
            this.txtProductSearch.TabIndex = 4;

            this.lblProductCategory.AutoSize = true;
            this.lblProductCategory.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblProductCategory.ForeColor = System.Drawing.Color.Gray;
            this.lblProductCategory.Location = new System.Drawing.Point(578, 22);
            this.lblProductCategory.Name = "lblProductCategory";
            this.lblProductCategory.TabIndex = 5;
            this.lblProductCategory.Text = "Category:";

            this.cmbProductCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProductCategory.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.cmbProductCategory.Items.AddRange(new object[] { "All", "Office Supplies", "IT Equipment", "Furniture", "Raw Materials", "Services" });
            this.cmbProductCategory.Location = new System.Drawing.Point(638, 18);
            this.cmbProductCategory.Name = "cmbProductCategory";
            this.cmbProductCategory.Size = new System.Drawing.Size(160, 25);
            this.cmbProductCategory.TabIndex = 6;
            this.cmbProductCategory.SelectedIndex = 0;

            this.dgvProducts.AllowUserToAddRows = false;
            this.dgvProducts.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvProducts.BackgroundColor = System.Drawing.Color.White;
            this.dgvProducts.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvProducts.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(247, 250, 252);
            this.dgvProducts.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.dgvProducts.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(113, 128, 150);
            this.dgvProducts.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.dgvProducts.EnableHeadersVisualStyles = false;
            this.dgvProducts.GridColor = System.Drawing.Color.FromArgb(226, 232, 240);
            this.dgvProducts.Location = new System.Drawing.Point(0, 60);
            this.dgvProducts.Name = "dgvProducts";
            this.dgvProducts.ReadOnly = true;
            this.dgvProducts.RowHeadersVisible = false;
            this.dgvProducts.RowTemplate.Height = 36;
            this.dgvProducts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvProducts.Size = new System.Drawing.Size(1100, 575);
            this.dgvProducts.TabIndex = 1;

            // ════════════════════════════════════════════════════════
            // PAGE: REPORTS
            // ════════════════════════════════════════════════════════
            this.pageReports.BackColor = System.Drawing.Color.FromArgb(247, 250, 252);
            this.pageReports.Controls.Add(this.panelReportsFilter);
            this.pageReports.Controls.Add(this.lblReportSummary);
            this.pageReports.Controls.Add(this.dgvReport);
            this.pageReports.Location = new System.Drawing.Point(0, 65);
            this.pageReports.Name = "pageReports";
            this.pageReports.Size = new System.Drawing.Size(1100, 635);
            this.pageReports.TabIndex = 5;
            this.pageReports.Visible = false;

            this.panelReportsFilter.BackColor = System.Drawing.Color.White;
            this.panelReportsFilter.Controls.Add(this.lblReportType);
            this.panelReportsFilter.Controls.Add(this.cmbReportType);
            this.panelReportsFilter.Controls.Add(this.lblDateFrom);
            this.panelReportsFilter.Controls.Add(this.dtpFrom);
            this.panelReportsFilter.Controls.Add(this.lblDateTo);
            this.panelReportsFilter.Controls.Add(this.dtpTo);
            this.panelReportsFilter.Controls.Add(this.btnGenerateReport);
            this.panelReportsFilter.Controls.Add(this.btnExportReport);
            this.panelReportsFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelReportsFilter.Name = "panelReportsFilter";
            this.panelReportsFilter.Size = new System.Drawing.Size(1100, 60);
            this.panelReportsFilter.TabIndex = 0;

            this.lblReportType.AutoSize = true;
            this.lblReportType.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblReportType.ForeColor = System.Drawing.Color.Gray;
            this.lblReportType.Location = new System.Drawing.Point(10, 22);
            this.lblReportType.Name = "lblReportType";
            this.lblReportType.TabIndex = 0;
            this.lblReportType.Text = "Report Type:";

            this.cmbReportType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbReportType.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.cmbReportType.Items.AddRange(new object[] { "Purchase Order Summary", "Supplier Performance", "Spend by Category", "Monthly Expenditure", "Pending Approvals" });
            this.cmbReportType.Location = new System.Drawing.Point(100, 18);
            this.cmbReportType.Name = "cmbReportType";
            this.cmbReportType.Size = new System.Drawing.Size(210, 25);
            this.cmbReportType.TabIndex = 1;
            this.cmbReportType.SelectedIndex = 0;

            this.lblDateFrom.AutoSize = true;
            this.lblDateFrom.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblDateFrom.ForeColor = System.Drawing.Color.Gray;
            this.lblDateFrom.Location = new System.Drawing.Point(325, 22);
            this.lblDateFrom.Name = "lblDateFrom";
            this.lblDateFrom.TabIndex = 2;
            this.lblDateFrom.Text = "From:";

            this.dtpFrom.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFrom.Location = new System.Drawing.Point(368, 18);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(120, 25);
            this.dtpFrom.TabIndex = 3;
            this.dtpFrom.Value = new System.DateTime(2026, 1, 1, 0, 0, 0, 0);

            this.lblDateTo.AutoSize = true;
            this.lblDateTo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblDateTo.ForeColor = System.Drawing.Color.Gray;
            this.lblDateTo.Location = new System.Drawing.Point(500, 22);
            this.lblDateTo.Name = "lblDateTo";
            this.lblDateTo.TabIndex = 4;
            this.lblDateTo.Text = "To:";

            this.dtpTo.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpTo.Location = new System.Drawing.Point(522, 18);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(120, 25);
            this.dtpTo.TabIndex = 5;
            this.dtpTo.Value = new System.DateTime(2026, 7, 21, 0, 0, 0, 0);

            this.btnGenerateReport.BackColor = System.Drawing.Color.FromArgb(49, 130, 206);
            this.btnGenerateReport.FlatAppearance.BorderSize = 0;
            this.btnGenerateReport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGenerateReport.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnGenerateReport.ForeColor = System.Drawing.Color.White;
            this.btnGenerateReport.Location = new System.Drawing.Point(658, 14);
            this.btnGenerateReport.Name = "btnGenerateReport";
            this.btnGenerateReport.Size = new System.Drawing.Size(100, 32);
            this.btnGenerateReport.TabIndex = 6;
            this.btnGenerateReport.Text = "Generate";
            this.btnGenerateReport.UseVisualStyleBackColor = false;
            this.btnGenerateReport.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGenerateReport.Click += new System.EventHandler(this.btnGenerateReport_Click);

            this.btnExportReport.BackColor = System.Drawing.Color.FromArgb(72, 187, 120);
            this.btnExportReport.FlatAppearance.BorderSize = 0;
            this.btnExportReport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportReport.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnExportReport.ForeColor = System.Drawing.Color.White;
            this.btnExportReport.Location = new System.Drawing.Point(768, 14);
            this.btnExportReport.Name = "btnExportReport";
            this.btnExportReport.Size = new System.Drawing.Size(100, 32);
            this.btnExportReport.TabIndex = 7;
            this.btnExportReport.Text = "Export CSV";
            this.btnExportReport.UseVisualStyleBackColor = false;
            this.btnExportReport.Cursor = System.Windows.Forms.Cursors.Hand;

            this.lblReportSummary.AutoSize = false;
            this.lblReportSummary.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblReportSummary.ForeColor = System.Drawing.Color.FromArgb(113, 128, 150);
            this.lblReportSummary.Location = new System.Drawing.Point(5, 65);
            this.lblReportSummary.Name = "lblReportSummary";
            this.lblReportSummary.Size = new System.Drawing.Size(1085, 25);
            this.lblReportSummary.TabIndex = 1;
            this.lblReportSummary.Text = "Select a report type and date range, then click Generate.";

            this.dgvReport.AllowUserToAddRows = false;
            this.dgvReport.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvReport.BackgroundColor = System.Drawing.Color.White;
            this.dgvReport.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvReport.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(247, 250, 252);
            this.dgvReport.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.dgvReport.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(113, 128, 150);
            this.dgvReport.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.dgvReport.EnableHeadersVisualStyles = false;
            this.dgvReport.GridColor = System.Drawing.Color.FromArgb(226, 232, 240);
            this.dgvReport.Location = new System.Drawing.Point(0, 95);
            this.dgvReport.Name = "dgvReport";
            this.dgvReport.ReadOnly = true;
            this.dgvReport.RowHeadersVisible = false;
            this.dgvReport.RowTemplate.Height = 36;
            this.dgvReport.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvReport.Size = new System.Drawing.Size(1100, 540);
            this.dgvReport.TabIndex = 2;

            // ════════════════════════════════════════════════════════
            // MAIN FORM
            // ════════════════════════════════════════════════════════
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1320, 700);
            this.Controls.Add(this.panelContent);
            this.Controls.Add(this.panelSidebar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.MinimumSize = new System.Drawing.Size(1000, 650);
            this.Name = "Dashboard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ProcureEase - Purchasing Management System";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Dashboard_Load);

            this.panelSidebar.ResumeLayout(false);
            this.panelTopBar.ResumeLayout(false);
            this.panelContent.ResumeLayout(false);
            this.pageOverview.ResumeLayout(false);
            this.cardPanel1.ResumeLayout(false);
            this.cardPanel2.ResumeLayout(false);
            this.cardPanel3.ResumeLayout(false);
            this.cardPanel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecentOrders)).EndInit();
            this.pageOrders.ResumeLayout(false);
            this.panelOrdersToolbar.ResumeLayout(false);
            this.panelOrdersToolbar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrders)).EndInit();
            this.pageSuppliers.ResumeLayout(false);
            this.panelSuppliersToolbar.ResumeLayout(false);
            this.panelSuppliersToolbar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSuppliers)).EndInit();
            this.pageProducts.ResumeLayout(false);
            this.panelProductsToolbar.ResumeLayout(false);
            this.panelProductsToolbar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProducts)).EndInit();
            this.pageReports.ResumeLayout(false);
            this.panelReportsFilter.ResumeLayout(false);
            this.panelReportsFilter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReport)).EndInit();

            // ════════════════════════════════════════════════════════
            // WIRE CRUD BUTTON CLICK EVENTS
            // ════════════════════════════════════════════════════════
            // Order Buttons
            this.btnNewOrder.Click += new System.EventHandler(this.btnNewOrder_Click);
            this.btnEditOrder.Click += new System.EventHandler(this.btnEditOrder_Click);
            this.btnDeleteOrder.Click += new System.EventHandler(this.btnDeleteOrder_Click);

            // Supplier Buttons
            this.btnNewSupplier.Click += new System.EventHandler(this.btnNewSupplier_Click);
            this.btnEditSupplier.Click += new System.EventHandler(this.btnEditSupplier_Click);
            this.btnDeleteSupplier.Click += new System.EventHandler(this.btnDeleteSupplier_Click);

            // Product Buttons
            this.btnNewProduct.Click += new System.EventHandler(this.btnNewProduct_Click);
            this.btnEditProduct.Click += new System.EventHandler(this.btnEditProduct_Click);
            this.btnDeleteProduct.Click += new System.EventHandler(this.btnDeleteProduct_Click);

            this.ResumeLayout(false);
        }

        #endregion

        // Timer
        private System.Windows.Forms.Timer timerClock;
        // Sidebar
        private System.Windows.Forms.Panel panelSidebar;
        private System.Windows.Forms.Label lblSidebarTitle;
        private System.Windows.Forms.Label lblSidebarSub;
        private System.Windows.Forms.Panel panelSidebarDivider;
        private System.Windows.Forms.Button btnNavOverview;
        private System.Windows.Forms.Button btnNavOrders;
        private System.Windows.Forms.Button btnNavSuppliers;
        private System.Windows.Forms.Button btnNavProducts;
        private System.Windows.Forms.Button btnNavReports;
        private System.Windows.Forms.Button btnNavSettings;
        private System.Windows.Forms.Button btnLogout;
        // Top bar
        private System.Windows.Forms.Panel panelTopBar;
        private System.Windows.Forms.Label lblPageTitle;
        private System.Windows.Forms.Label lblUserGreeting;
        private System.Windows.Forms.Label lblDateTime;
        // Content
        private System.Windows.Forms.Panel panelContent;
        // Overview
        private System.Windows.Forms.Panel pageOverview;
        private System.Windows.Forms.Panel cardPanel1, cardPanel2, cardPanel3, cardPanel4;
        private System.Windows.Forms.Panel panelCard1Accent, panelCard2Accent, panelCard3Accent, panelCard4Accent;
        private System.Windows.Forms.Label lblCard1Title, lblCard1Value, lblCard1Sub;
        private System.Windows.Forms.Label lblCard2Title, lblCard2Value, lblCard2Sub;
        private System.Windows.Forms.Label lblCard3Title, lblCard3Value, lblCard3Sub;
        private System.Windows.Forms.Label lblCard4Title, lblCard4Value, lblCard4Sub;
        private System.Windows.Forms.Label lblRecentOrdersTitle;
        private System.Windows.Forms.DataGridView dgvRecentOrders;
        // Orders
        private System.Windows.Forms.Panel pageOrders;
        private System.Windows.Forms.Panel panelOrdersToolbar;
        private System.Windows.Forms.Button btnNewOrder, btnEditOrder, btnDeleteOrder, btnPrintOrder;
        private System.Windows.Forms.Label lblOrderSearch, lblOrderStatus;
        private System.Windows.Forms.TextBox txtOrderSearch;
        private System.Windows.Forms.ComboBox cmbOrderStatus;
        private System.Windows.Forms.DataGridView dgvOrders;
        // Suppliers
        private System.Windows.Forms.Panel pageSuppliers;
        private System.Windows.Forms.Panel panelSuppliersToolbar;
        private System.Windows.Forms.Button btnNewSupplier, btnEditSupplier, btnDeleteSupplier;
        private System.Windows.Forms.Label lblSupplierSearch;
        private System.Windows.Forms.TextBox txtSupplierSearch;
        private System.Windows.Forms.DataGridView dgvSuppliers;
        // Products
        private System.Windows.Forms.Panel pageProducts;
        private System.Windows.Forms.Panel panelProductsToolbar;
        private System.Windows.Forms.Button btnNewProduct, btnEditProduct, btnDeleteProduct;
        private System.Windows.Forms.Label lblProductSearch, lblProductCategory;
        private System.Windows.Forms.TextBox txtProductSearch;
        private System.Windows.Forms.ComboBox cmbProductCategory;
        private System.Windows.Forms.DataGridView dgvProducts;
        // Reports
        private System.Windows.Forms.Panel pageReports;
        private System.Windows.Forms.Panel panelReportsFilter;
        private System.Windows.Forms.Label lblReportType, lblDateFrom, lblDateTo, lblReportSummary;
        private System.Windows.Forms.ComboBox cmbReportType;
        private System.Windows.Forms.DateTimePicker dtpFrom, dtpTo;
        private System.Windows.Forms.Button btnGenerateReport, btnExportReport;
        private System.Windows.Forms.DataGridView dgvReport;
    }
}
