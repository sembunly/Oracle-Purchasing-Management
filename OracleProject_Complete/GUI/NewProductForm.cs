using System;
using System.Windows.Forms;

namespace OracleProject
{
    public partial class NewProductForm : Form
    {
        public NewProductForm()
        {
            InitializeComponent();
        }

        private void NewProductForm_Load(object sender, EventArgs e)
        {
            LoadCategories();
            SetDefaultValues();
        }

        // =========================================================
        // INITIALIZATION & DEFAULTS
        // =========================================================
        private void SetDefaultValues()
        {
            cmbCategory.SelectedIndex = 0;
            numQuantity.Value = 0;
            txtUnitPrice.Text = "0.00";
            chkActive.Checked = true;
        }

        private void LoadCategories()
        {
            cmbCategory.Items.Clear();
            cmbCategory.Items.Add("Electronics");
            cmbCategory.Items.Add("Office Supplies");
            cmbCategory.Items.Add("Raw Materials");
            cmbCategory.Items.Add("Components");
            cmbCategory.Items.Add("Other");
            cmbCategory.SelectedIndex = 0;
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
            if (!decimal.TryParse(txtUnitPrice.Text, out decimal price) || price <= 0)
            {
                MessageBox.Show("Unit price must be greater than 0.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUnitPrice.Focus();
                return false;
            }

            // Validate quantity
            if (numQuantity.Value < 0)
            {
                MessageBox.Show("Quantity cannot be negative.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                numQuantity.Focus();
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
        // PUBLIC PROPERTIES (for parent form to retrieve data)
        // =========================================================
        public new string ProductName => txtProductName.Text;
        public string SKU => txtSKU.Text;
        public string Category => cmbCategory.SelectedItem?.ToString() ?? string.Empty;
        public string Description => txtDescription.Text;
        public decimal UnitPrice => decimal.TryParse(txtUnitPrice.Text, out decimal p) ? p : 0;
        public int QuantityOnHand => (int)numQuantity.Value;
        public bool IsActive => chkActive.Checked;
        public string Price => UnitPrice.ToString("C2");
        public string StockStatus => numQuantity.Value > 10 ? "In Stock" : (numQuantity.Value > 0 ? "Low Stock" : "Out of Stock");
        public string Quantity => numQuantity.Value.ToString();
        public string Status => chkActive.Checked ? "Active" : "Inactive";
    }
}
