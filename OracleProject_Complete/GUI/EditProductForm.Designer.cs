namespace OracleProject
{
    partial class EditProductForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblProductID = new System.Windows.Forms.Label();
            this.txtProductID = new System.Windows.Forms.TextBox();
            this.lblProductName = new System.Windows.Forms.Label();
            this.txtProductName = new System.Windows.Forms.TextBox();
            this.lblSKU = new System.Windows.Forms.Label();
            this.txtSKU = new System.Windows.Forms.TextBox();
            this.lblCategory = new System.Windows.Forms.Label();
            this.cmbCategory = new System.Windows.Forms.ComboBox();
            this.txtCategory = new System.Windows.Forms.TextBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.lblUnitPrice = new System.Windows.Forms.Label();
            this.txtUnitPrice = new System.Windows.Forms.TextBox();
            this.txtPrice = new System.Windows.Forms.TextBox();
            this.lblQuantity = new System.Windows.Forms.Label();
            this.numQuantity = new System.Windows.Forms.NumericUpDown();
            this.txtQuantity = new System.Windows.Forms.TextBox();
            this.chkActive = new System.Windows.Forms.CheckBox();
            this.cmbStockStatus = new System.Windows.Forms.ComboBox();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.ButtonOK = new System.Windows.Forms.Button();
            this.ButtonCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numQuantity)).BeginInit();
            this.SuspendLayout();

            // lblProductID
            this.lblProductID.AutoSize = true;
            this.lblProductID.Location = new System.Drawing.Point(12, 15);
            this.lblProductID.Name = "lblProductID";
            this.lblProductID.Size = new System.Drawing.Size(61, 13);
            this.lblProductID.TabIndex = 0;
            this.lblProductID.Text = "Product ID:";

            // txtProductID
            this.txtProductID.Location = new System.Drawing.Point(110, 12);
            this.txtProductID.Name = "txtProductID";
            this.txtProductID.ReadOnly = true;
            this.txtProductID.Size = new System.Drawing.Size(240, 20);
            this.txtProductID.TabIndex = 1;

            // lblProductName
            this.lblProductName.AutoSize = true;
            this.lblProductName.Location = new System.Drawing.Point(12, 45);
            this.lblProductName.Name = "lblProductName";
            this.lblProductName.Size = new System.Drawing.Size(81, 13);
            this.lblProductName.TabIndex = 2;
            this.lblProductName.Text = "Product Name:";

            // txtProductName
            this.txtProductName.Location = new System.Drawing.Point(110, 42);
            this.txtProductName.Name = "txtProductName";
            this.txtProductName.Size = new System.Drawing.Size(240, 20);
            this.txtProductName.TabIndex = 3;

            // lblSKU
            this.lblSKU.AutoSize = true;
            this.lblSKU.Location = new System.Drawing.Point(12, 75);
            this.lblSKU.Name = "lblSKU";
            this.lblSKU.Size = new System.Drawing.Size(33, 13);
            this.lblSKU.TabIndex = 4;
            this.lblSKU.Text = "SKU:";

            // txtSKU
            this.txtSKU.Location = new System.Drawing.Point(110, 72);
            this.txtSKU.Name = "txtSKU";
            this.txtSKU.Size = new System.Drawing.Size(240, 20);
            this.txtSKU.TabIndex = 5;

            // lblCategory
            this.lblCategory.AutoSize = true;
            this.lblCategory.Location = new System.Drawing.Point(12, 105);
            this.lblCategory.Name = "lblCategory";
            this.lblCategory.Size = new System.Drawing.Size(52, 13);
            this.lblCategory.TabIndex = 6;
            this.lblCategory.Text = "Category:";

            // cmbCategory
            this.cmbCategory.FormattingEnabled = true;
            this.cmbCategory.Items.AddRange(new object[] { "Electronics", "Office Supplies", "Raw Materials", "Components", "Other" });
            this.cmbCategory.Location = new System.Drawing.Point(110, 102);
            this.cmbCategory.Name = "cmbCategory";
            this.cmbCategory.Size = new System.Drawing.Size(120, 21);
            this.cmbCategory.TabIndex = 7;

            // txtCategory (additional, internal for external access)
            this.txtCategory.Location = new System.Drawing.Point(236, 102);
            this.txtCategory.Name = "txtCategory";
            this.txtCategory.Size = new System.Drawing.Size(114, 20);
            this.txtCategory.TabIndex = 8;

            // lblDescription
            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new System.Drawing.Point(12, 135);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(63, 13);
            this.lblDescription.TabIndex = 9;
            this.lblDescription.Text = "Description:";

            // txtDescription
            this.txtDescription.Location = new System.Drawing.Point(110, 132);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(240, 60);
            this.txtDescription.TabIndex = 10;

            // lblUnitPrice
            this.lblUnitPrice.AutoSize = true;
            this.lblUnitPrice.Location = new System.Drawing.Point(12, 200);
            this.lblUnitPrice.Name = "lblUnitPrice";
            this.lblUnitPrice.Size = new System.Drawing.Size(56, 13);
            this.lblUnitPrice.TabIndex = 11;
            this.lblUnitPrice.Text = "Unit Price:";

            // txtUnitPrice
            this.txtUnitPrice.Location = new System.Drawing.Point(110, 197);
            this.txtUnitPrice.Name = "txtUnitPrice";
            this.txtUnitPrice.Size = new System.Drawing.Size(120, 20);
            this.txtUnitPrice.TabIndex = 12;

            // txtPrice (mirror for external access)
            this.txtPrice.Location = new System.Drawing.Point(236, 197);
            this.txtPrice.Name = "txtPrice";
            this.txtPrice.Size = new System.Drawing.Size(114, 20);
            this.txtPrice.TabIndex = 13;

            // lblQuantity
            this.lblQuantity.AutoSize = true;
            this.lblQuantity.Location = new System.Drawing.Point(12, 230);
            this.lblQuantity.Name = "lblQuantity";
            this.lblQuantity.Size = new System.Drawing.Size(46, 13);
            this.lblQuantity.TabIndex = 14;
            this.lblQuantity.Text = "Quantity:";

            // numQuantity
            this.numQuantity.Location = new System.Drawing.Point(110, 227);
            this.numQuantity.Name = "numQuantity";
            this.numQuantity.Size = new System.Drawing.Size(120, 20);
            this.numQuantity.TabIndex = 15;

            // txtQuantity (mirror for external access)
            this.txtQuantity.Location = new System.Drawing.Point(236, 227);
            this.txtQuantity.Name = "txtQuantity";
            this.txtQuantity.Size = new System.Drawing.Size(114, 20);
            this.txtQuantity.TabIndex = 16;

            // chkActive
            this.chkActive.AutoSize = true;
            this.chkActive.Location = new System.Drawing.Point(110, 257);
            this.chkActive.Name = "chkActive";
            this.chkActive.Size = new System.Drawing.Size(56, 17);
            this.chkActive.TabIndex = 17;
            this.chkActive.Text = "Active";
            this.chkActive.UseVisualStyleBackColor = true;

            // cmbStockStatus (internal)
            this.cmbStockStatus.FormattingEnabled = true;
            this.cmbStockStatus.Items.AddRange(new object[] { "In Stock", "Low Stock", "Out of Stock" });
            this.cmbStockStatus.Location = new System.Drawing.Point(236, 257);
            this.cmbStockStatus.Name = "cmbStockStatus";
            this.cmbStockStatus.Size = new System.Drawing.Size(114, 21);
            this.cmbStockStatus.TabIndex = 18;

            // cmbStatus (internal)
            this.cmbStatus.FormattingEnabled = true;
            this.cmbStatus.Items.AddRange(new object[] { "Active", "Inactive" });
            this.cmbStatus.Location = new System.Drawing.Point(110, 283);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(120, 21);
            this.cmbStatus.TabIndex = 19;

            // btnOK
            this.ButtonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ButtonOK.Location = new System.Drawing.Point(190, 310);
            this.ButtonOK.Name = "ButtonOK";
            this.ButtonOK.Size = new System.Drawing.Size(75, 23);
            this.ButtonOK.TabIndex = 20;
            this.ButtonOK.Text = "OK";
            this.ButtonOK.UseVisualStyleBackColor = true;
            this.ButtonOK.Click += new System.EventHandler(this.ButtonOK_Click);

            // btnCancel
            this.ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ButtonCancel.Location = new System.Drawing.Point(275, 310);
            this.ButtonCancel.Name = "ButtonCancel";
            this.ButtonCancel.Size = new System.Drawing.Size(75, 23);
            this.ButtonCancel.TabIndex = 21;
            this.ButtonCancel.Text = "Cancel";
            this.ButtonCancel.UseVisualStyleBackColor = true;
            this.ButtonCancel.Click += new System.EventHandler(this.ButtonCancel_Click);

            // EditProductForm
            this.AcceptButton = this.ButtonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.ButtonCancel;
            this.ClientSize = new System.Drawing.Size(370, 345);
            this.Controls.Add(this.ButtonCancel);
            this.Controls.Add(this.ButtonOK);
            this.Controls.Add(this.cmbStatus);
            this.Controls.Add(this.cmbStockStatus);
            this.Controls.Add(this.chkActive);
            this.Controls.Add(this.txtQuantity);
            this.Controls.Add(this.numQuantity);
            this.Controls.Add(this.lblQuantity);
            this.Controls.Add(this.txtPrice);
            this.Controls.Add(this.txtUnitPrice);
            this.Controls.Add(this.lblUnitPrice);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.txtCategory);
            this.Controls.Add(this.cmbCategory);
            this.Controls.Add(this.lblCategory);
            this.Controls.Add(this.txtSKU);
            this.Controls.Add(this.lblSKU);
            this.Controls.Add(this.txtProductName);
            this.Controls.Add(this.lblProductName);
            this.Controls.Add(this.txtProductID);
            this.Controls.Add(this.lblProductID);
            this.Name = "EditProductForm";
            this.Text = "Edit Product";
            this.Load += new System.EventHandler(this.EditProductForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numQuantity)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblProductID;
        private System.Windows.Forms.TextBox txtProductID;
        private System.Windows.Forms.Label lblProductName;
        internal System.Windows.Forms.TextBox txtProductName;
        private System.Windows.Forms.Label lblSKU;
        internal System.Windows.Forms.TextBox txtSKU;
        private System.Windows.Forms.Label lblCategory;
        internal System.Windows.Forms.ComboBox cmbCategory;
        internal System.Windows.Forms.TextBox txtCategory;
        private System.Windows.Forms.Label lblDescription;
        internal System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label lblUnitPrice;
        internal System.Windows.Forms.TextBox txtUnitPrice;
        internal System.Windows.Forms.TextBox txtPrice;
        private System.Windows.Forms.Label lblQuantity;
        internal System.Windows.Forms.NumericUpDown numQuantity;
        internal System.Windows.Forms.TextBox txtQuantity;
        internal System.Windows.Forms.CheckBox chkActive;
        internal System.Windows.Forms.ComboBox cmbStockStatus;
        internal System.Windows.Forms.ComboBox cmbStatus;
        internal System.Windows.Forms.Button ButtonOK;
        internal System.Windows.Forms.Button ButtonCancel;
    }
}
