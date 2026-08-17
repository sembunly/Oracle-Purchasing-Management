using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace OracleProject
{
    /// <summary>
    /// Reports tab: report generation and export.
    /// </summary>
    internal sealed class ReportsTab : DashboardTabBase
    {
        private Panel filterPanel;
        private Label lblReportType, lblDateFrom, lblDateTo, lblReportSummary;
        private ComboBox cmbReportType;
        private DateTimePicker dtpFrom, dtpTo;
        private Button btnGenerateReport, btnExportReport;
        private DataGridView dgvReport;

        public ReportsTab()
        {
            TabTitle = "Reports";
            RequiredPermission = "REPORTS_VIEW";
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            BackColor = Color.FromArgb(247, 250, 252);
            Dock = DockStyle.Fill;

            // Filter panel
            filterPanel = new Panel
            {
                BackColor = Color.White,
                Dock = DockStyle.Top,
                Height = 94
            };

            // Report Type
            lblReportType = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.Gray,
                Location = new Point(15, 34),
                Text = "Report Type:"
            };

            cmbReportType = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 9.5F),
                Items = { "Purchase Order Summary", "Supplier Performance", "Spend by Category", "Monthly Expenditure", "Pending Approvals" },
                Location = new Point(150, 28),
                Size = new Size(313, 43)
            };
            cmbReportType.SelectedIndex = 0;

            // Date From
            lblDateFrom = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.Gray,
                Location = new Point(488, 34),
                Text = "From:"
            };

            dtpFrom = new DateTimePicker
            {
                Font = new Font("Segoe UI", 9.5F),
                Format = DateTimePickerFormat.Short,
                Location = new Point(552, 28),
                Size = new Size(178, 41),
                Value = new DateTime(DateTime.Today.Year, 1, 1)
            };

            // Date To
            lblDateTo = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.Gray,
                Location = new Point(750, 34),
                Text = "To:"
            };

            dtpTo = new DateTimePicker
            {
                Font = new Font("Segoe UI", 9.5F),
                Format = DateTimePickerFormat.Short,
                Location = new Point(783, 28),
                Size = new Size(178, 41),
                Value = DateTime.Today
            };

            // Generate button
            btnGenerateReport = CreatePageButton("Generate", Color.FromArgb(49, 130, 206), 987, 22);
            btnGenerateReport.Size = new Size(150, 50);
            btnGenerateReport.Click += BtnGenerateReport_Click;

            // Export button
            btnExportReport = CreatePageButton("Export CSV", Color.FromArgb(72, 187, 120), 1152, 22);
            btnExportReport.Size = new Size(150, 50);
            btnExportReport.Click += BtnExportReport_Click;

            filterPanel.Controls.Add(lblReportType);
            filterPanel.Controls.Add(cmbReportType);
            filterPanel.Controls.Add(lblDateFrom);
            filterPanel.Controls.Add(dtpFrom);
            filterPanel.Controls.Add(lblDateTo);
            filterPanel.Controls.Add(dtpTo);
            filterPanel.Controls.Add(btnGenerateReport);
            filterPanel.Controls.Add(btnExportReport);

            // Summary label
            lblReportSummary = new Label
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = Color.FromArgb(113, 128, 150),
                Location = new Point(8, 102),
                Size = new Size(1000, 39),
                Text = "Select a report type and date range, then click Generate."
            };

            // Grid
            dgvReport = new DataGridView
            {
                AllowUserToAddRows = false,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                EnableHeadersVisualStyles = false,
                GridColor = Color.FromArgb(226, 232, 240),
                Location = new Point(0, 148),
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
            dgvReport.ColumnHeadersDefaultCellStyle = headerStyle;
            dgvReport.ColumnHeadersHeight = 46;

            var cellStyle = new DataGridViewCellStyle
            {
                Alignment = DataGridViewContentAlignment.MiddleLeft,
                BackColor = Color.White,
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = Color.Black
            };
            dgvReport.DefaultCellStyle = cellStyle;

            Controls.Add(dgvReport);
            Controls.Add(lblReportSummary);
            Controls.Add(filterPanel);

            // Resize handler
            Resize += ReportsTab_Resize;
        }

        private void ReportsTab_Resize(object sender, EventArgs e)
        {
            lblReportSummary.Width = ClientSize.Width - 20;
            dgvReport.Width = ClientSize.Width;
            dgvReport.Height = Math.Max(200, ClientSize.Height - dgvReport.Top);
        }

        public override void ApplyPermissions()
        {
            btnGenerateReport.Visible = HasPermission("REPORTS_GENERATE");
            btnExportReport.Visible = HasPermission("REPORTS_EXPORT");
        }

        public override void OnActivated()
        {
            // Don't auto-generate on activation
        }

        public override void RefreshData()
        {
            // Re-generate current report if one exists
            if (dgvReport.DataSource != null)
            {
                BtnGenerateReport_Click(null, EventArgs.Empty);
            }
        }

        private void BtnGenerateReport_Click(object sender, EventArgs e)
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
                        ApplyStatusColor(dgvReport, 4);
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
                        ApplyStatusColor(dgvReport, 5);
                        SetSummary("Pending Approvals", table, from, to);
                        break;
                }

                dgvReport.DataSource = table;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Report Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetSummary(string title, DataTable table, DateTime from, DateTime to)
        {
            decimal total = 0;
            if (table.Columns.Contains("Total Amount"))
                total = DashboardUiHelper.SumColumn(table, "Total Amount");
            else if (table.Columns.Contains("Total Spend"))
                total = DashboardUiHelper.SumColumn(table, "Total Spend");
            else if (table.Columns.Contains("Estimated Amount"))
                total = DashboardUiHelper.SumColumn(table, "Estimated Amount");

            lblReportSummary.Text = string.Format(
                CultureInfo.CurrentCulture,
                "{0} | Period: {1:MMM dd, yyyy} - {2:MMM dd, yyyy} | Records: {3} | Amount: {4:C2}",
                title, from, to.AddDays(-1), table.Rows.Count, total);
        }

        private void BtnExportReport_Click(object sender, EventArgs e)
        {
            if (!RequirePermission("REPORTS_EXPORT"))
                return;

            if (dgvReport.Rows.Count == 0 || dgvReport.DataSource == null)
            {
                MessageBox.Show(this, "Please generate a report before exporting.", "No Report",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var rows = new List<DataGridViewRow>();
            foreach (DataGridViewRow row in dgvReport.Rows)
            {
                if (!row.IsNewRow)
                    rows.Add(row);
            }

            DashboardUiHelper.ExportGridRows(
                dgvReport,
                rows,
                "report-" + DateTime.Now.ToString("yyyyMMdd-HHmmss", CultureInfo.InvariantCulture) + ".csv",
                "Report exported.");
        }
    }
}
