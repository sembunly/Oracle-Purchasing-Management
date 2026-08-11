using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace OracleProject
{
    public partial class NewOrderForm : Form
    {
        private readonly Dictionary<string, QuoteOption> _selectedQuotes =
            new Dictionary<string, QuoteOption>(StringComparer.OrdinalIgnoreCase);

        private sealed class QuoteOption
        {
            public int RequestId { get; set; }
            public int QuotationId { get; set; }
            public decimal TotalAmount { get; set; }
        }

        public NewOrderForm()
        {
            InitializeComponent();
        }

        private void NewOrderForm_Load(object sender, EventArgs e)
        {
            // Initialize form controls and load reference data (suppliers, etc.)
            lblSupplier.Text = "Approved quotation:";
            SetDefaultValues();
            LoadSuppliers();
            LoadProducts();
        }

        // =========================================================
        // INITIALIZATION & DEFAULTS
        // =========================================================
        private void SetDefaultValues()
        {
            dtpOrderDate.Value = DateTime.Now;
            dtpExpectedDelivery.Value = DateTime.Now.AddDays(7);
            cmbStatus.SelectedIndex = 0; // "Pending"
            txtSubtotal.Text = "0.00";
            txtTax.Text = "0.00";
            txtTotal.Text = "0.00";
        }

        private void LoadSuppliers()
        {
            cmbSupplier.Items.Clear();
            cmbSupplier.Items.Add("Select an approved quotation...");

            _selectedQuotes.Clear();
            const string sql = @"
                SELECT q.quotation_id,
                       q.request_id,
                       s.supplier_name,
                       q.total_amount
                  FROM quotations q
                  JOIN suppliers s ON s.supplier_id = q.supplier_id
                 WHERE q.status = 'SELECTED'
                   AND s.status = 'ACTIVE'
                 ORDER BY q.quotation_date DESC";

            DataTable quotes = OracleDb.Query(sql);
            foreach (DataRow row in quotes.Rows)
            {
                string supplierName = Convert.ToString(row["SUPPLIER_NAME"]);
                var option = new QuoteOption
                {
                    RequestId = Convert.ToInt32(row["REQUEST_ID"]),
                    QuotationId = Convert.ToInt32(row["QUOTATION_ID"]),
                    TotalAmount = Convert.ToDecimal(row["TOTAL_AMOUNT"])
                };

                string display = supplierName + " (Quote #" + option.QuotationId + ")";
                _selectedQuotes[display] = option;
                cmbSupplier.Items.Add(display);
            }

            cmbSupplier.SelectedIndex = 0;
            cmbSupplier.SelectedIndexChanged += cmbSupplier_SelectedIndexChanged;
        }

        private void LoadProducts()
        {
            // PO items are copied by Oracle's sp_create_po from the selected quotation.
        }

        private void cmbSupplier_SelectedIndexChanged(object sender, EventArgs e)
        {
            QuoteOption option;
            if (_selectedQuotes.TryGetValue(cmbSupplier.Text, out option))
            {
                txtSubtotal.Text = option.TotalAmount.ToString("F2");
                CalculateTotal();
            }
        }

        // =========================================================
        // FORM CONTROLS
        // =========================================================
        private void ButtonOK_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        // =========================================================
        // VALIDATION
        // =========================================================
        private bool ValidateForm()
        {
            // Validate supplier selection
            if (cmbSupplier.SelectedIndex <= 0)
            {
                MessageBox.Show("Please select a supplier.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbSupplier.Focus();
                return false;
            }

            // Validate PO Number (if manually entered)
            if (string.IsNullOrWhiteSpace(txtPONumber.Text))
            {
                MessageBox.Show("PO Number is required.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPONumber.Focus();
                return false;
            }

            // Validate order date
            if (dtpOrderDate.Value > dtpExpectedDelivery.Value)
            {
                MessageBox.Show("Order date cannot be later than expected delivery date.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpOrderDate.Focus();
                return false;
            }

            // Validate total amount
            if (!decimal.TryParse(txtTotal.Text, out decimal total) || total <= 0)
            {
                MessageBox.Show("Total amount must be greater than 0.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTotal.Focus();
                return false;
            }

            return true;
        }

        // =========================================================
        // CALCULATION & EVENTS
        // =========================================================
        private void txtSubtotal_TextChanged(object sender, EventArgs e)
        {
            CalculateTotal();
        }

        private void CalculateTotal()
        {
            if (decimal.TryParse(txtSubtotal.Text, out decimal subtotal))
            {
                if (decimal.TryParse(txtTax.Text, out decimal tax))
                {
                    decimal total = subtotal + tax;
                    txtTotal.Text = total.ToString("F2");
                }
            }
        }

        // =========================================================
        // PUBLIC PROPERTIES (for parent form to retrieve data)
        // =========================================================
        public string PONumber => txtPONumber.Text;
        public string SupplierName => cmbSupplier.SelectedItem?.ToString() ?? string.Empty;
        public int SelectedRequestId
        {
            get
            {
                QuoteOption option;
                return _selectedQuotes.TryGetValue(cmbSupplier.Text, out option) ? option.RequestId : 0;
            }
        }

        public int SelectedQuotationId
        {
            get
            {
                QuoteOption option;
                return _selectedQuotes.TryGetValue(cmbSupplier.Text, out option) ? option.QuotationId : 0;
            }
        }

        public DateTime OrderDate => dtpOrderDate.Value;
        public DateTime ExpectedDelivery => dtpExpectedDelivery.Value;
        public decimal Subtotal => decimal.TryParse(txtSubtotal.Text, out decimal s) ? s : 0;
        public decimal Tax => decimal.TryParse(txtTax.Text, out decimal t) ? t : 0;
        public decimal Total => decimal.TryParse(txtTotal.Text, out decimal tot) ? tot : 0;
        public string Status => cmbStatus.SelectedItem?.ToString() ?? "Pending";
        public string RequestedBy => txtRequestedBy.Text;
    }
}
