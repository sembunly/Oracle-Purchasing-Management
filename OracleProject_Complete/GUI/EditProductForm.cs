using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace OracleProject
{
    public partial class EditProductForm : Form
    {
        private int _productId;

        /// <summary>
        /// EditProductForm - Opens existing product for editing
        /// Pass the Product ID to load existing product data (optional)
        /// </summary>
        public EditProductForm(int productId = 0)
        {
            _productId = productId;
            InitializeComponent();
        }

        private void EditProductForm_Load(object sender, EventArgs e)
        {
            LoadCategories();
            if (_productId > 0)
                LoadProductData(_productId);
            SetReadOnlyFields();
            PopulateExternalFieldsFromInternal(); // sync extra controls for external access
        }

        // =========================================================
        // DATA LOADING
        // =========================================================
        private void LoadProductData(int productId)
        {
            const string sql = @"
                SELECT product_id, product_code, product_name,
                       category, unit_price, stock_qty, status
                  FROM products
                 WHERE product_id = :product_id";
            DataTable table = OracleDb.Query(sql, OracleDb.Parameter("product_id", productId));
            if (table.Rows.Count == 0)
                throw new InvalidOperationException("Product was not found in Oracle.");

            DataRow row = table.Rows[0];
            txtProductID.Text = Convert.ToString(row["PRODUCT_ID"]);
            txtProductName.Text = Convert.ToString(row["PRODUCT_NAME"]);
            txtSKU.Text = Convert.ToString(row["PRODUCT_CODE"]);
            string category = Convert.ToString(row["CATEGORY"]);
            if (cmbCategory.Items.Contains(category))
                cmbCategory.SelectedItem = category;
            else
                cmbCategory.Text = category;
            txtUnitPrice.Text = Convert.ToString(row["UNIT_PRICE"]);
            decimal stock = Convert.ToDecimal(row["STOCK_QTY"]);
            numQuantity.Value = Math.Max(numQuantity.Minimum, Math.Min(numQuantity.Maximum, stock));
            chkActive.Checked = Convert.ToInt32(row["STATUS"]) == 1;

            // ensure mirrored controls reflect the loaded data
            PopulateExternalFieldsFromInternal();
        }

        private void LoadCategories()
        {
            cmbCategory.Items.Clear();
            cmbCategory.Items.Add("Electronics");
            cmbCategory.Items.Add("Office Supplies");
            cmbCategory.Items.Add("Raw Materials");
            cmbCategory.Items.Add("Components");
            cmbCategory.Items.Add("Other");
        }

        // =========================================================
        // READ-ONLY FIELDS
        // =========================================================
        private void SetReadOnlyFields()
        {
            // Product ID cannot be changed
            txtProductID.ReadOnly = true;
            txtProductID.BackColor = SystemColors.Control;
        }

        // =========================================================
        // FORM CONTROLS
        // =========================================================
        private void ButtonOK_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                // Apply any edits made on the mirrored controls back to canonical controls
                ApplyExternalFieldsToInternal();

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
            // Validate product name
            if (string.IsNullOrWhiteSpace(txtProductName.Text))
            {
                MessageBox.Show("Product name is required.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtProductName.Focus();
                return false;
            }

            // Validate SKU
            if (string.IsNullOrWhiteSpace(txtSKU.Text))
            {
                MessageBox.Show("SKU is required.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSKU.Focus();
                return false;
            }

            // Validate unit price
            if (!decimal.TryParse(txtUnitPrice.Text, NumberStyles.Currency | NumberStyles.Number, CultureInfo.CurrentCulture, out decimal price) || price <= 0)
            {
                MessageBox.Show("Unit price must be greater than 0.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUnitPrice.Focus();
                return false;
            }

            // Validate quantity
            if (!int.TryParse(txtQuantity.Text, out int q) && numQuantity.Value < 0)
            {
                MessageBox.Show("Quantity cannot be negative.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtQuantity.Focus();
                return false;
            }

            // Validate description (optional but should not be too long)
            if (txtDescription.Text.Length > 500)
            {
                MessageBox.Show("Description cannot exceed 500 characters.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDescription.Focus();
                return false;
            }

            return true;
        }

        // =========================================================
        // MIRROR / SYNC HELPERS
        // =========================================================
        // Copy canonical (internal) control values into the mirrored controls used by other code
        private void PopulateExternalFieldsFromInternal()
        {
            // Category
            if (cmbCategory.SelectedItem != null)
                txtCategory.Text = cmbCategory.SelectedItem.ToString();
            else
                txtCategory.Text = string.Empty;

            // Price
            txtPrice.Text = txtUnitPrice.Text;

            // Quantity
            txtQuantity.Text = numQuantity.Value.ToString();

            // Stock status
            var stock = numQuantity.Value > 10 ? "In Stock" : (numQuantity.Value > 0 ? "Low Stock" : "Out of Stock");
            if (cmbStockStatus.Items.Contains(stock))
                cmbStockStatus.SelectedItem = stock;
            else
                cmbStockStatus.Text = stock;

            // Status
            var st = chkActive.Checked ? "Active" : "Inactive";
            if (cmbStatus.Items.Contains(st))
                cmbStatus.SelectedItem = st;
            else
                cmbStatus.Text = st;
        }

        // Copy mirrored control values back into canonical/internal controls before save
        private void ApplyExternalFieldsToInternal()
        {
            // Category: if txtCategory matches an existing item, select it; otherwise keep current selection and optionally add
            var categoryText = txtCategory.Text?.Trim();
            if (!string.IsNullOrEmpty(categoryText))
            {
                if (!cmbCategory.Items.Contains(categoryText))
                {
                    cmbCategory.Items.Add(categoryText);
                }
                cmbCategory.SelectedItem = categoryText;
            }

            // Price
            if (!string.IsNullOrWhiteSpace(txtPrice.Text))
            {
                txtUnitPrice.Text = txtPrice.Text;
            }

            // Quantity
            if (int.TryParse(txtQuantity.Text, out int q))
            {
                if (q < numQuantity.Minimum) q = (int)numQuantity.Minimum;
                if (q > numQuantity.Maximum) q = (int)numQuantity.Maximum;
                numQuantity.Value = q;
            }

            // Status -> chkActive
            var s = cmbStatus.Text?.Trim().ToLowerInvariant();
            chkActive.Checked = (s == "active" || s == "true") || chkActive.Checked;

            // Stock status is informational; no direct field to write to besides leaving selected text
        }

        // =========================================================
        // PUBLIC PROPERTIES (for parent form to retrieve data)
        // =========================================================
        public int ProductID => int.TryParse(txtProductID.Text, out int id) ? id : 0;
        public new string ProductName => txtProductName.Text;
        public string SKU => txtSKU.Text;
        public string Category => cmbCategory.SelectedItem?.ToString() ?? txtCategory.Text ?? string.Empty;
        public string Description => txtDescription.Text;
        public decimal UnitPrice => decimal.TryParse(txtUnitPrice.Text, NumberStyles.Currency | NumberStyles.Number, CultureInfo.CurrentCulture, out decimal p) ? p : 0;
        public int QuantityOnHand => (int)numQuantity.Value;
        public bool IsActive => chkActive.Checked;
        public string Price => UnitPrice.ToString("C2");
        public string StockStatus => numQuantity.Value > 10 ? "In Stock" : (numQuantity.Value > 0 ? "Low Stock" : "Out of Stock");
        public string Quantity => numQuantity.Value.ToString();
        public string Status => chkActive.Checked ? "Active" : "Inactive";
    }
}
