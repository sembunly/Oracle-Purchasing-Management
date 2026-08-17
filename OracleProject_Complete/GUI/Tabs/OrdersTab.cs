using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace OracleProject
{
    /// <summary>
    /// Orders tab: purchase orders management.
    /// </summary>
    internal sealed class OrdersTab : DashboardTabBase
    {
        private Panel toolbar;
        private Button btnNewOrder, btnEditOrder, btnDeleteOrder, btnPrintOrder;
        private Label lblOrderSearch, lblOrderStatus;
        private TextBox txtOrderSearch;
        private ComboBox cmbOrderStatus;
        private DataGridView dgvOrders;

        public OrdersTab()
        {
            TabTitle = "Purchase Orders";
            RequiredPermission = "ORDERS_VIEW";
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
            btnNewOrder = CreatePageButton("New Order", Color.FromArgb(49, 130, 206), 15, 22);
            btnNewOrder.Size = new Size(165, 50);
            btnNewOrder.Click += BtnNewOrder_Click;

            btnEditOrder = CreatePageButton("Edit", Color.FromArgb(72, 187, 120), 195, 22);
            btnEditOrder.Size = new Size(120, 50);
            btnEditOrder.Click += BtnEditOrder_Click;

            btnDeleteOrder = CreatePageButton("Delete", Color.FromArgb(245, 101, 101), 330, 22);
            btnDeleteOrder.Size = new Size(120, 50);
            btnDeleteOrder.Click += BtnDeleteOrder_Click;

            btnPrintOrder = CreatePageButton("Print / Export", Color.FromArgb(113, 128, 150), 465, 22);
            btnPrintOrder.Size = new Size(165, 50);
            btnPrintOrder.Click += BtnPrintOrder_Click;

            // Search label
            lblOrderSearch = new Label
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                AutoSize = true,
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.Gray,
                Location = new Point(0, 34), // Will be positioned by layout
                Text = "Search:"
            };

            // Search textbox
            txtOrderSearch = new TextBox
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Segoe UI", 9.5F),
                Location = new Point(0, 28),
                Size = new Size(299, 41)
            };

            // Status label
            lblOrderStatus = new Label
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                AutoSize = true,
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.Gray,
                Location = new Point(0, 34),
                Text = "Status:"
            };

            // Status combo
            cmbOrderStatus = new ComboBox
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 9.5F),
                Items = { "All", "Pending", "Approved", "Received", "Cancelled" },
                Location = new Point(0, 28),
                Size = new Size(208, 43)
            };
            cmbOrderStatus.SelectedIndex = 0;

            toolbar.Controls.Add(btnNewOrder);
            toolbar.Controls.Add(btnEditOrder);
            toolbar.Controls.Add(btnDeleteOrder);
            toolbar.Controls.Add(btnPrintOrder);
            toolbar.Controls.Add(lblOrderSearch);
            toolbar.Controls.Add(txtOrderSearch);
            toolbar.Controls.Add(lblOrderStatus);
            toolbar.Controls.Add(cmbOrderStatus);

            // Grid
            dgvOrders = new DataGridView
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
            dgvOrders.ColumnHeadersDefaultCellStyle = headerStyle;
            dgvOrders.ColumnHeadersHeight = 46;

            var cellStyle = new DataGridViewCellStyle
            {
                Alignment = DataGridViewContentAlignment.MiddleLeft,
                BackColor = Color.White,
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = Color.Black
            };
            dgvOrders.DefaultCellStyle = cellStyle;

            ApplyStatusColor(dgvOrders, 8);

            Controls.Add(dgvOrders);
            Controls.Add(toolbar);

            // Layout search controls on resize
            toolbar.Resize += Toolbar_Resize;
        }

        private void Toolbar_Resize(object sender, EventArgs e)
        {
            // Position search controls on the right side of toolbar
            int right = toolbar.Width - 20;
            
            cmbOrderStatus.Location = new Point(right - cmbOrderStatus.Width, 28);
            lblOrderStatus.Location = new Point(right - cmbOrderStatus.Width - 90, 34);
            
            right = right - cmbOrderStatus.Width - 120;
            txtOrderSearch.Location = new Point(right - txtOrderSearch.Width, 28);
            lblOrderSearch.Location = new Point(right - txtOrderSearch.Width - 70, 34);
        }

        public override void ApplyPermissions()
        {
            btnNewOrder.Visible = HasPermission("ORDERS_ADD");
            btnEditOrder.Visible = HasPermission("ORDERS_EDIT");
            btnDeleteOrder.Visible = HasPermission("ORDERS_DELETE");
            btnPrintOrder.Visible = HasPermission("ORDERS_PRINT");
        }

        public override void OnActivated()
        {
            RefreshData();
        }

        public override void RefreshData()
        {
            try
            {
                LoadOrders();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Refresh Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
        }

        private void BtnNewOrder_Click(object sender, EventArgs e)
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
                    RefreshData();
                    MessageBox.Show(this, "Purchase order created in Oracle.", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, ex.Message, "Create PO Error",
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
                command.Parameters.Add("p_po_no", Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2).Value = form.PONumber;
                command.Parameters.Add("p_request_id", Oracle.ManagedDataAccess.Client.OracleDbType.Decimal).Value = form.SelectedRequestId;
                command.Parameters.Add("p_quotation_id", Oracle.ManagedDataAccess.Client.OracleDbType.Decimal).Value = form.SelectedQuotationId;
                command.Parameters.Add("p_expected_delivery_date", Oracle.ManagedDataAccess.Client.OracleDbType.Date).Value = form.ExpectedDelivery;
                command.Parameters.Add("p_created_by", Oracle.ManagedDataAccess.Client.OracleDbType.Decimal).Value = Convert.ToDecimal(employee);
                command.Parameters.Add("p_tax_amount", Oracle.ManagedDataAccess.Client.OracleDbType.Decimal).Value = form.Tax;

                var output = new Oracle.ManagedDataAccess.Client.OracleParameter("p_po_id", Oracle.ManagedDataAccess.Client.OracleDbType.Decimal)
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

        private void BtnEditOrder_Click(object sender, EventArgs e)
        {
            if (!RequirePermission("ORDERS_EDIT"))
                return;

            if (dgvOrders.SelectedRows.Count == 0)
            {
                MessageBox.Show(this, "Please select an order to edit.", "No Selection",
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

                    RefreshData();
                    MessageBox.Show(this, "Purchase order updated in Oracle.", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, ex.Message, "Update PO Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnDeleteOrder_Click(object sender, EventArgs e)
        {
            if (!RequirePermission("ORDERS_DELETE"))
                return;

            if (dgvOrders.SelectedRows.Count == 0)
            {
                MessageBox.Show(this, "Please select an order to cancel.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string poNo = Convert.ToString(dgvOrders.SelectedRows[0].Cells[0].Value);
            if (MessageBox.Show(this, "Cancel PO " + poNo + "? History will be preserved.", "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                OracleDb.Execute(
                    "UPDATE purchase_orders SET status = 4 WHERE po_no = :po_no",
                    OracleDb.Parameter("po_no", poNo));
                RefreshData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Cancel PO Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnPrintOrder_Click(object sender, EventArgs e)
        {
            if (!RequirePermission("ORDERS_PRINT"))
                return;

            if (dgvOrders.SelectedRows.Count == 0)
            {
                MessageBox.Show(this, "Please select an order to export.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string poNo = Convert.ToString(dgvOrders.SelectedRows[0].Cells[0].Value);
            DashboardUiHelper.ExportGridRows(
                dgvOrders,
                new[] { dgvOrders.SelectedRows[0] },
                "purchase-order-" + DashboardUiHelper.SafeFileName(poNo) + ".csv",
                "Purchase order exported.");
        }
    }
}
