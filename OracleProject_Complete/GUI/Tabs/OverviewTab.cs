using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace OracleProject
{
    /// <summary>
    /// Overview tab: stat cards and recent orders.
    /// </summary>
    internal sealed class OverviewTab : DashboardTabBase
    {
        private Panel cardPanel1, cardPanel2, cardPanel3, cardPanel4;
        private Panel panelCard1Accent, panelCard2Accent, panelCard3Accent, panelCard4Accent;
        private Label lblCard1Title, lblCard1Value, lblCard1Sub;
        private Label lblCard2Title, lblCard2Value, lblCard2Sub;
        private Label lblCard3Title, lblCard3Value, lblCard3Sub;
        private Label lblCard4Title, lblCard4Value, lblCard4Sub;
        private Label lblRecentOrdersTitle;
        private DataGridView dgvRecentOrders;

        public OverviewTab()
        {
            TabTitle = "Overview";
            RequiredPermission = "OVERVIEW_VIEW";
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            // Tab properties
            BackColor = Color.FromArgb(247, 250, 252);
            Dock = DockStyle.Fill;

            // Card 1 - Total Orders
            cardPanel1 = CreateCardPanel(Color.FromArgb(49, 130, 206));
            panelCard1Accent = CreateAccentPanel(Color.FromArgb(49, 130, 206));
            lblCard1Title = CreateCardLabel("TOTAL ORDERS", Color.FromArgb(113, 128, 150), true);
            lblCard1Value = CreateCardValue("0", Color.FromArgb(49, 130, 206));
            lblCard1Sub = CreateCardLabel("This Month", Color.FromArgb(160, 174, 192), false);

            cardPanel1.Controls.Add(panelCard1Accent);
            cardPanel1.Controls.Add(lblCard1Title);
            cardPanel1.Controls.Add(lblCard1Value);
            cardPanel1.Controls.Add(lblCard1Sub);

            // Card 2 - Pending Approval
            cardPanel2 = CreateCardPanel(Color.FromArgb(237, 137, 54));
            panelCard2Accent = CreateAccentPanel(Color.FromArgb(237, 137, 54));
            lblCard2Title = CreateCardLabel("PENDING APPROVAL", Color.FromArgb(113, 128, 150), true);
            lblCard2Value = CreateCardValue("0", Color.FromArgb(237, 137, 54));
            lblCard2Sub = CreateCardLabel("Requires Action", Color.FromArgb(160, 174, 192), false);

            cardPanel2.Controls.Add(panelCard2Accent);
            cardPanel2.Controls.Add(lblCard2Title);
            cardPanel2.Controls.Add(lblCard2Value);
            cardPanel2.Controls.Add(lblCard2Sub);

            // Card 3 - Total Suppliers
            cardPanel3 = CreateCardPanel(Color.FromArgb(72, 187, 120));
            panelCard3Accent = CreateAccentPanel(Color.FromArgb(72, 187, 120));
            lblCard3Title = CreateCardLabel("TOTAL SUPPLIERS", Color.FromArgb(113, 128, 150), true);
            lblCard3Value = CreateCardValue("0", Color.FromArgb(72, 187, 120));
            lblCard3Sub = CreateCardLabel("Active Vendors", Color.FromArgb(160, 174, 192), false);

            cardPanel3.Controls.Add(panelCard3Accent);
            cardPanel3.Controls.Add(lblCard3Title);
            cardPanel3.Controls.Add(lblCard3Value);
            cardPanel3.Controls.Add(lblCard3Sub);

            // Card 4 - Monthly Spend
            cardPanel4 = CreateCardPanel(Color.FromArgb(159, 122, 234));
            panelCard4Accent = CreateAccentPanel(Color.FromArgb(159, 122, 234));
            lblCard4Title = CreateCardLabel("MONTHLY SPEND", Color.FromArgb(113, 128, 150), true);
            lblCard4Value = CreateCardValue("$0", Color.FromArgb(159, 122, 234));
            lblCard4Sub = CreateCardLabel("This Month", Color.FromArgb(160, 174, 192), false);

            cardPanel4.Controls.Add(panelCard4Accent);
            cardPanel4.Controls.Add(lblCard4Title);
            cardPanel4.Controls.Add(lblCard4Value);
            cardPanel4.Controls.Add(lblCard4Sub);

            // Recent Orders Title
            lblRecentOrdersTitle = new Label
            {
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.FromArgb(26, 32, 44),
                Location = new Point(30, 266),
                Size = new Size(450, 47),
                Text = "Recent Purchase Orders"
            };

            // Recent Orders Grid
            dgvRecentOrders = new DataGridView
            {
                AllowUserToAddRows = false,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                EnableHeadersVisualStyles = false,
                GridColor = Color.FromArgb(226, 232, 240),
                Location = new Point(30, 320),
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
            dgvRecentOrders.ColumnHeadersDefaultCellStyle = headerStyle;
            dgvRecentOrders.ColumnHeadersHeight = 46;

            var cellStyle = new DataGridViewCellStyle
            {
                Alignment = DataGridViewContentAlignment.MiddleLeft,
                BackColor = Color.White,
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = Color.Black
            };
            dgvRecentOrders.DefaultCellStyle = cellStyle;

            ApplyStatusColor(dgvRecentOrders, 5);

            // Add controls
            Controls.Add(cardPanel1);
            Controls.Add(cardPanel2);
            Controls.Add(cardPanel3);
            Controls.Add(cardPanel4);
            Controls.Add(lblRecentOrdersTitle);
            Controls.Add(dgvRecentOrders);

            // Initial layout
            Resize += OverviewTab_Resize;
        }

        private Panel CreateCardPanel(Color accentColor)
        {
            return new Panel
            {
                BackColor = Color.White,
                Margin = new Padding(4, 5, 4, 5),
                Size = new Size(375, 203)
            };
        }

        private Panel CreateAccentPanel(Color color)
        {
            return new Panel
            {
                BackColor = color,
                Location = new Point(0, 0),
                Margin = new Padding(4, 5, 4, 5),
                Size = new Size(8, 203)
            };
        }

        private Label CreateCardLabel(string text, Color color, bool bold)
        {
            return new Label
            {
                AutoSize = false,
                Font = new Font("Segoe UI", bold ? 9F : 8.5F, bold ? FontStyle.Bold : FontStyle.Regular),
                ForeColor = color,
                Location = new Point(30, bold ? 28 : 153),
                Size = new Size(330, bold ? 31 : 31),
                Text = text
            };
        }

        private Label CreateCardValue(string text, Color color)
        {
            return new Label
            {
                AutoSize = false,
                Font = new Font("Segoe UI", 26F, FontStyle.Bold),
                ForeColor = color,
                Location = new Point(30, 62),
                Size = new Size(330, 81),
                Text = text
            };
        }

        private void OverviewTab_Resize(object sender, EventArgs e)
        {
            LayoutCards();
        }

        private void LayoutCards()
        {
            const int outerMargin = 20;
            const int gap = 20;
            const int cardCount = 4;

            int availableWidth = ClientSize.Width - (outerMargin * 2) - (gap * (cardCount - 1));
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
                    ? ClientSize.Width - outerMargin - left
                    : cardWidth;

                cards[i].SetBounds(left, outerMargin, width, cards[i].Height);
                accents[i].Height = cards[i].ClientSize.Height;

                int labelWidth = Math.Max(0, width - 30);
                titles[i].Width = labelWidth;
                values[i].Width = labelWidth;
                subtitles[i].Width = labelWidth;
            }

            // Resize grid
            dgvRecentOrders.Width = ClientSize.Width - 60;
            dgvRecentOrders.Height = Math.Max(200, ClientSize.Height - dgvRecentOrders.Top - 30);
        }

        public override void OnActivated()
        {
            RefreshData();
        }

        public override void RefreshData()
        {
            try
            {
                LoadCardData();
                LoadRecentOrders();
                LayoutCards();
            }
            catch (Exception ex)
            {
                // Silently handle errors - just show default values
                System.Diagnostics.Debug.WriteLine("OverviewTab refresh error: " + ex.Message);
            }
        }

        private void LoadCardData()
        {
            try
            {
                // Total Orders (this month)
                var totalOrders = OracleDb.Scalar(@"
                    SELECT COUNT(*) FROM purchase_orders 
                    WHERE TRUNC(po_date, 'MM') = TRUNC(SYSDATE, 'MM')");
                if (lblCard1Value != null)
                    lblCard1Value.Text = totalOrders != null ? Convert.ToInt32(totalOrders).ToString() : "0";

                // Pending Approval
                var pending = OracleDb.Scalar(@"
                    SELECT COUNT(*) FROM purchase_orders 
                    WHERE status = 0");
                if (lblCard2Value != null)
                    lblCard2Value.Text = pending != null ? Convert.ToInt32(pending).ToString() : "0";

                // Total Suppliers (active)
                var suppliers = OracleDb.Scalar(@"
                    SELECT COUNT(*) FROM suppliers WHERE status = 1");
                if (lblCard3Value != null)
                    lblCard3Value.Text = suppliers != null ? Convert.ToInt32(suppliers).ToString() : "0";

                // Monthly Spend
                var spend = OracleDb.Scalar(@"
                    SELECT NVL(SUM(total_amount), 0) FROM purchase_orders 
                    WHERE TRUNC(po_date, 'MM') = TRUNC(SYSDATE, 'MM') AND status <> 4");
                if (lblCard4Value != null)
                    lblCard4Value.Text = spend != null ? $"{Convert.ToDecimal(spend):C0}" : "$0";
            }
            catch
            {
                // Set defaults on error
                if (lblCard1Value != null) lblCard1Value.Text = "0";
                if (lblCard2Value != null) lblCard2Value.Text = "0";
                if (lblCard3Value != null) lblCard3Value.Text = "0";
                if (lblCard4Value != null) lblCard4Value.Text = "$0";
            }
        }

        private void LoadRecentOrders()
        {
            try
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

                if (dgvRecentOrders != null)
                    dgvRecentOrders.DataSource = OracleDb.Query(sql);
            }
            catch
            {
                // Leave grid empty on error
                if (dgvRecentOrders != null)
                    dgvRecentOrders.DataSource = null;
            }
        }
    }
}
