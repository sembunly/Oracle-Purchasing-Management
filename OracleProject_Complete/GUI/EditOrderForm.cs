using System;
using System.Data;
using System.Windows.Forms;

namespace OracleProject
{
    public partial class EditOrderForm : Form
    {
        public EditOrderForm()
        {
            InitializeComponent();
        }

        private void EditOrderForm_Load(object sender, EventArgs e)
        {
            LoadSuppliers();
        }

        private void LoadSuppliers()
        {
            cmbSupplier.Items.Clear();
            DataTable table = OracleDb.Query(
                "SELECT supplier_name FROM suppliers WHERE status = 'ACTIVE' ORDER BY supplier_name");
            foreach (DataRow row in table.Rows)
                cmbSupplier.Items.Add(Convert.ToString(row["SUPPLIER_NAME"]));
        }

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

        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(txtPONumber.Text))
            {
                MessageBox.Show("PO Number is required.", "Validation Error");
                return false;
            }
            return true;
        }

        public string PONumber => txtPONumber.Text;
        public string SupplierName => cmbSupplier.Text;
        public DateTime OrderDate => dtpOrderDate.Value;
        public DateTime ExpectedDelivery => dtpExpectedDelivery.Value;
        public string Status => cmbStatus.Text;
        public decimal Subtotal
        {
            get
            {
                return decimal.Parse(txtSubtotal.Text ?? "0");
            }
        }

        public decimal Tax => decimal.Parse(txtTax.Text ?? "0");
        public decimal Total => decimal.Parse(txtTotal.Text ?? "0");
        public string RequestedBy => txtRequestedBy.Text;
        public string ApprovedBy => txtApprovedBy.Text;
    }
}
