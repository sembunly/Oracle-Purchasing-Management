using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace OracleProject
{
    public partial class EditSupplierForm : Form
    {
        private int _supplierId;

        /// <summary>
        /// EditSupplierForm - Opens existing supplier for editing
        /// Pass the Supplier ID to load existing supplier data (optional)
        /// </summary>
        public EditSupplierForm(int supplierId = 0)
        {
            _supplierId = supplierId;
            InitializeComponent();
        }

        private void EditSupplierForm_Load(object sender, EventArgs e)
        {
            LoadCategories();
            if (_supplierId > 0)
                LoadSupplierData(_supplierId);
            SetReadOnlyFields();

            // If external code (Dashboard) set cmbStatus before showing the form,
            // apply it to the internal chkActive checkbox.
            ApplyExternalStatusToInternal();
        }

        // =========================================================
        // DATA LOADING
        // =========================================================
        private void LoadSupplierData(int supplierId)
        {
            const string sql = @"
                SELECT supplier_id, supplier_name, contact_person,
                       email, phone, address, status
                  FROM suppliers
                 WHERE supplier_id = :supplier_id";
            DataTable table = OracleDb.Query(sql, OracleDb.Parameter("supplier_id", supplierId));
            if (table.Rows.Count == 0)
                throw new InvalidOperationException("Supplier was not found in Oracle.");

            DataRow row = table.Rows[0];
            txtSupplierID.Text = Convert.ToString(row["SUPPLIER_ID"]);
            txtCompanyName.Text = Convert.ToString(row["SUPPLIER_NAME"]);
            txtContactPerson.Text = Convert.ToString(row["CONTACT_PERSON"]);
            txtEmail.Text = Convert.ToString(row["EMAIL"]);
            txtPhone.Text = Convert.ToString(row["PHONE"]);
            txtAddress.Text = Convert.ToString(row["ADDRESS"]);
            numRating.Value = 5;
            chkActive.Checked = string.Equals(Convert.ToString(row["STATUS"]), "ACTIVE",
                StringComparison.OrdinalIgnoreCase);

            // Keep external aliases in sync
            if (cmbStatus != null)
                cmbStatus.Text = chkActive.Checked ? "Active" : "Inactive";
            if (txtSupplierName != null)
                txtSupplierName.Text = txtCompanyName.Text;
            if (txtCity != null)
                txtCity.Text = txtAddress.Text;
        }

        private void LoadCategories()
        {
            cmbCategory.Items.Clear();
            cmbCategory.Items.Add("Electronics");
            cmbCategory.Items.Add("Office Supplies");
            cmbCategory.Items.Add("Raw Materials");
            cmbCategory.Items.Add("Components");
            cmbCategory.Items.Add("Services");
            cmbCategory.Items.Add("Other");
        }

        // =========================================================
        // READ-ONLY FIELDS
        // =========================================================
        private void SetReadOnlyFields()
        {
            // Supplier ID cannot be changed
            txtSupplierID.ReadOnly = true;
            txtSupplierID.BackColor = SystemColors.Control;
        }

        // =========================================================
        // FORM CONTROLS
        // =========================================================
        private void ButtonOK_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                // Apply any external control values back to canonical fields
                ApplyExternalToInternal();

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
            // Validate company name
            if (string.IsNullOrWhiteSpace(txtCompanyName.Text))
            {
                MessageBox.Show("Company name is required.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCompanyName.Focus();
                return false;
            }

            // Validate contact person
            if (string.IsNullOrWhiteSpace(txtContactPerson.Text))
            {
                MessageBox.Show("Contact person is required.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtContactPerson.Focus();
                return false;
            }

            // Validate email format
            if (!string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                if (!IsValidEmail(txtEmail.Text))
                {
                    MessageBox.Show("Please enter a valid email address.", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtEmail.Focus();
                    return false;
                }
            }

            // Validate phone number
            if (string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                MessageBox.Show("Phone number is required.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPhone.Focus();
                return false;
            }

            // Validate address
            if (string.IsNullOrWhiteSpace(txtAddress.Text))
            {
                MessageBox.Show("Address is required.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtAddress.Focus();
                return false;
            }

            return true;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        // =========================================================
        // EXTERNAL / INTERNAL SYNC
        // =========================================================
        // When Dashboard sets cmbStatus before showing this form, apply it here.
        private void ApplyExternalStatusToInternal()
        {
            try
            {
                if (cmbStatus == null) return;
                var text = (cmbStatus.Text ?? string.Empty).Trim().ToLowerInvariant();
                chkActive.Checked = (text == "active" || text == "true");
            }
            catch
            {
                // ignore malformed values
            }
        }

        // Copy external alias values back to canonical controls before save
        private void ApplyExternalToInternal()
        {
            if (txtSupplierName != null)
                txtCompanyName.Text = txtSupplierName.Text;

            if (txtCity != null)
                txtAddress.Text = txtCity.Text;

            if (cmbStatus != null)
            {
                var text = (cmbStatus.Text ?? string.Empty).Trim().ToLowerInvariant();
                chkActive.Checked = (text == "active" || text == "true");
            }
        }

        // =========================================================
        // PUBLIC PROPERTIES (for parent form to retrieve data)
        // =========================================================
        public int SupplierID => int.TryParse(txtSupplierID.Text, out int id) ? id : 0;
        public new string CompanyName => txtCompanyName.Text;
        public string SupplierName => txtCompanyName.Text; // Alias for CompanyName
        public string ContactPerson => txtContactPerson.Text;
        public string Email => txtEmail.Text;
        public string Phone => txtPhone.Text;
        public string Address => txtAddress.Text;
        public string City => txtAddress.Text; // Using Address as City for now
        public string Category => cmbCategory.SelectedItem?.ToString() ?? string.Empty;
        public int Rating => (int)numRating.Value;
        public bool IsActive => chkActive.Checked;
        public string Status => chkActive.Checked ? "Active" : "Inactive";
    }
}
