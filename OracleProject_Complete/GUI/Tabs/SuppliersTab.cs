using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace OracleProject
{
    /// <summary>
    /// Suppliers tab: supplier management.
    /// </summary>
    internal sealed class SuppliersTab : DashboardTabBase
    {
        private Panel toolbar;
        private Button btnNewSupplier, btnEditSupplier, btnDeleteSupplier;
        private Label lblSupplierSearch;
        private TextBox txtSupplierSearch;
        private DataGridView dgvSuppliers;

        public SuppliersTab()
        {
            TabTitle = "Suppliers";
            RequiredPermission = "SUPPLIERS_VIEW";
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            BackColor = Color.FromArgb(247, 250, 252);
            Dock = DockStyle.Fill;

            // Toolbar
            toolbar = new Panel
            {
                BackColor = Color.White,
                Dock = DockStyle.Top,
                Height = 94
            };

            // Buttons
            btnNewSupplier = CreatePageButton("Add Supplier", Color.FromArgb(49, 130, 206), 15, 22);
            btnNewSupplier.Size = new Size(180, 50);
            btnNewSupplier.Click += BtnNewSupplier_Click;

            btnEditSupplier = CreatePageButton("Edit", Color.FromArgb(72, 187, 120), 210, 22);
            btnEditSupplier.Size = new Size(120, 50);
            btnEditSupplier.Click += BtnEditSupplier_Click;

            btnDeleteSupplier = CreatePageButton("Delete", Color.FromArgb(245, 101, 101), 345, 22);
            btnDeleteSupplier.Size = new Size(120, 50);
            btnDeleteSupplier.Click += BtnDeleteSupplier_Click;

            // Search label
            lblSupplierSearch = new Label
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                AutoSize = true,
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.Gray,
                Location = new Point(0, 34),
                Text = "Search:"
            };

            // Search textbox
            txtSupplierSearch = new TextBox
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Segoe UI", 9.5F),
                Location = new Point(0, 28),
                Size = new Size(329, 41)
            };

            toolbar.Controls.Add(btnNewSupplier);
            toolbar.Controls.Add(btnEditSupplier);
            toolbar.Controls.Add(btnDeleteSupplier);
            toolbar.Controls.Add(lblSupplierSearch);
            toolbar.Controls.Add(txtSupplierSearch);

            // Grid
            dgvSuppliers = new DataGridView
            {
                AllowUserToAddRows = false,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                EnableHeadersVisualStyles = false,
                GridColor = Color.FromArgb(226, 232, 240),
                Location = new Point(0, 94),
                ReadOnly = true,
                RowHeadersVisible = false,
                RowTemplate = { Height = 36 },
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            var headerStyle = new DataGridViewCellStyle
            {
                Alignment = DataGridViewContentAlignment.MiddleLeft,
                BackColor = Color.FromArgb(247, 250, 252),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(113, 128, 150)
            };
            dgvSuppliers.ColumnHeadersDefaultCellStyle = headerStyle;
            dgvSuppliers.ColumnHeadersHeight = 46;

            var cellStyle = new DataGridViewCellStyle
            {
                Alignment = DataGridViewContentAlignment.MiddleLeft,
                BackColor = Color.White,
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = Color.Black
            };
            dgvSuppliers.DefaultCellStyle = cellStyle;

            ApplyStatusColor(dgvSuppliers, 10);

            Controls.Add(dgvSuppliers);
            Controls.Add(toolbar);

            // Layout search controls on resize
            toolbar.Resize += Toolbar_Resize;
        }

        private void Toolbar_Resize(object sender, EventArgs e)
        {
            txtSupplierSearch.Location = new Point(toolbar.Width - txtSupplierSearch.Width - 20, 28);
            lblSupplierSearch.Location = new Point(toolbar.Width - txtSupplierSearch.Width - 110, 34);
        }

        public override void ApplyPermissions()
        {
            btnNewSupplier.Visible = HasPermission("SUPPLIERS_ADD");
            btnEditSupplier.Visible = HasPermission("SUPPLIERS_EDIT");
            btnDeleteSupplier.Visible = HasPermission("SUPPLIERS_DELETE");
        }

        public override void OnActivated()
        {
            RefreshData();
        }

        public override void RefreshData()
        {
            try
            {
                LoadSuppliers();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Refresh Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
        }

        private void BtnNewSupplier_Click(object sender, EventArgs e)
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

                    RefreshData();
                    MessageBox.Show(this, "Supplier saved to Oracle.", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, ex.Message, "Create Supplier Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnEditSupplier_Click(object sender, EventArgs e)
        {
            if (!RequirePermission("SUPPLIERS_EDIT"))
                return;

            if (dgvSuppliers.SelectedRows.Count == 0)
            {
                MessageBox.Show(this, "Please select a supplier to edit.", "No Selection",
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

                    RefreshData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, ex.Message, "Update Supplier Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnDeleteSupplier_Click(object sender, EventArgs e)
        {
            if (!RequirePermission("SUPPLIERS_DELETE"))
                return;

            if (dgvSuppliers.SelectedRows.Count == 0)
            {
                MessageBox.Show(this, "Please select a supplier to deactivate.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string code = Convert.ToString(dgvSuppliers.SelectedRows[0].Cells[0].Value);
            if (MessageBox.Show(this, "Deactivate supplier " + code + "?", "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                OracleDb.Execute(
                    "UPDATE suppliers SET status = 0 WHERE supplier_code = :supplier_code",
                    OracleDb.Parameter("supplier_code", code));
                RefreshData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Deactivate Supplier Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
