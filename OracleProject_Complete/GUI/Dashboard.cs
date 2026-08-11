using System;
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
        private Button _activeNavButton;

        public Dashboard(string username = "Admin")
        {
            _currentUser = username;
            InitializeComponent();
            pageOverview.Resize += pageOverview_Resize;
            LayoutOverviewCards();
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
            lblUserGreeting.Text = "Welcome, " + _currentUser;
            timerClock.Start();
            timerClock_Tick(null, null);
            dtpFrom.Value = new DateTime(DateTime.Today.Year, 1, 1);
            dtpTo.Value = DateTime.Today;

            try
            {
                LoadDatabaseData();
                SetActivePage(pageOverview, btnNavOverview, "Overview");
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

            ResetNavButton(btnNavOverview);
            ResetNavButton(btnNavOrders);
            ResetNavButton(btnNavSuppliers);
            ResetNavButton(btnNavProducts);
            ResetNavButton(btnNavReports);
            ResetNavButton(btnNavSettings);

            page.Visible = true;
            HighlightNavButton(navBtn);
            lblPageTitle.Text = title;
            _activeNavButton = navBtn;
        }

        private void ResetNavButton(Button btn)
        {
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

        private void btnNavSettings_Click(object sender, EventArgs e)
            => SetActivePage(pageOverview, btnNavSettings, "Settings");

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

        private void ReloadAfterChange()
        {
            try
            {
                LoadDatabaseData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("The database was changed, but the grid could not refresh.\n\n" + ex.Message,
                    "Refresh Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // =========================================================
        // PURCHASE ORDER OPERATIONS
        // =========================================================
        private void btnNewOrder_Click(object sender, EventArgs e)
        {
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

        // =========================================================
        // SUPPLIER CRUD
        // =========================================================
        private void btnNewSupplier_Click(object sender, EventArgs e)
        {
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
            using (var form = new NewProductForm())
            {
                if (form.ShowDialog() != DialogResult.OK)
                    return;

                try
                {
                    OracleDb.Execute(@"
                        INSERT INTO products (
                            product_id, product_code, product_name, category,
                            unit, unit_price, stock_qty, status
                        ) VALUES (
                            product_seq.NEXTVAL, :product_code, :product_name,
                            :category, 'UNIT', :unit_price, :stock_qty, CASE UPPER(:status) WHEN 'ACTIVE' THEN 1 ELSE 0 END
                        )",
                        OracleDb.Parameter("product_code", form.SKU),
                        OracleDb.Parameter("product_name", form.ProductName),
                        OracleDb.Parameter("category", form.Category),
                        OracleDb.Parameter("unit_price", form.UnitPrice),
                        OracleDb.Parameter("stock_qty", form.QuantityOnHand),
                        OracleDb.Parameter("status", form.Status));
                    ReloadAfterChange();
                    MessageBox.Show("Product saved to Oracle.", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Create Product Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnEditProduct_Click(object sender, EventArgs e)
        {
            if (dgvProducts.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a product to edit.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var row = dgvProducts.SelectedRows[0];
            string code = Convert.ToString(row.Cells[0].Value);
            using (var form = new EditProductForm())
            {
                form.txtProductName.Text = Convert.ToString(row.Cells[1].Value);
                form.txtSKU.Text = code;
                string category = Convert.ToString(row.Cells[2].Value);
                if (form.cmbCategory.Items.Contains(category))
                    form.cmbCategory.SelectedItem = category;
                else
                    form.cmbCategory.Text = category;
                form.txtUnitPrice.Text = Convert.ToString(row.Cells[4].Value);
                decimal stock;
                if (decimal.TryParse(Convert.ToString(row.Cells[5].Value), out stock))
                    form.numQuantity.Value = Math.Max(form.numQuantity.Minimum,
                        Math.Min(form.numQuantity.Maximum, stock));
                form.chkActive.Checked = !string.Equals(
                    Convert.ToString(row.Cells[9].Value), "INACTIVE", StringComparison.OrdinalIgnoreCase);

                if (form.ShowDialog() != DialogResult.OK)
                    return;

                try
                {
                    OracleDb.Execute(@"
                        UPDATE products
                           SET product_name = :product_name,
                               category = :category,
                               unit_price = :unit_price,
                               stock_qty = :stock_qty,
                               status = CASE UPPER(:status) WHEN 'ACTIVE' THEN 1 WHEN 'INACTIVE' THEN 0 WHEN 'DRAFT' THEN 0 WHEN 'APPROVED' THEN 1 WHEN 'PARTIALLY_RECEIVED' THEN 2 WHEN 'RECEIVED' THEN 3 WHEN 'CANCELLED' THEN 4 WHEN 'CLOSED' THEN 5 ELSE status END
                         WHERE product_code = :product_code",
                        OracleDb.Parameter("product_name", form.ProductName),
                        OracleDb.Parameter("category", form.Category),
                        OracleDb.Parameter("unit_price", form.UnitPrice),
                        OracleDb.Parameter("stock_qty", form.QuantityOnHand),
                        OracleDb.Parameter("status", form.Status),
                        OracleDb.Parameter("product_code", code));
                    ReloadAfterChange();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Update Product Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnDeleteProduct_Click(object sender, EventArgs e)
        {
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

        private void lblCard1Value_Click(object sender, EventArgs e)
        {

        }
    }
}
