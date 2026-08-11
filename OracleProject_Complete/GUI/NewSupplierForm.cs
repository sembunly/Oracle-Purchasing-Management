using System;
using System.Windows.Forms;

namespace OracleProject
{
    public partial class NewSupplierForm : Form
    {
        public NewSupplierForm()
        {
            InitializeComponent();
        }

        private void NewSupplierForm_Load(object sender, EventArgs e)
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
            numRating.Value = 5;
            chkActive.Checked = true;
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
        // PUBLIC PROPERTIES (for parent form to retrieve data)
        // =========================================================
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
