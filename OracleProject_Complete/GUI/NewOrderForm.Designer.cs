namespace OracleProject
{
    partial class NewOrderForm
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
            this.lblSupplier = new System.Windows.Forms.Label();
            this.cmbSupplier = new System.Windows.Forms.ComboBox();
            this.lblOrderDate = new System.Windows.Forms.Label();
            this.dtpOrderDate = new System.Windows.Forms.DateTimePicker();
            this.lblExpectedDelivery = new System.Windows.Forms.Label();
            this.dtpExpectedDelivery = new System.Windows.Forms.DateTimePicker();
            this.lblPONumber = new System.Windows.Forms.Label();
            this.txtPONumber = new System.Windows.Forms.TextBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.lblSubtotal = new System.Windows.Forms.Label();
            this.txtSubtotal = new System.Windows.Forms.TextBox();
            this.lblTax = new System.Windows.Forms.Label();
            this.txtTax = new System.Windows.Forms.TextBox();
            this.lblTotal = new System.Windows.Forms.Label();
            this.txtTotal = new System.Windows.Forms.TextBox();
            this.lblRequestedBy = new System.Windows.Forms.Label();
            this.txtRequestedBy = new System.Windows.Forms.TextBox();
            this.ButtonOK = new System.Windows.Forms.Button();
            this.ButtonCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();

            // lblSupplier
            this.lblSupplier.AutoSize = true;
            this.lblSupplier.Location = new System.Drawing.Point(12, 15);
            this.lblSupplier.Name = "lblSupplier";
            this.lblSupplier.Size = new System.Drawing.Size(54, 13);
            this.lblSupplier.TabIndex = 0;
            this.lblSupplier.Text = "Supplier:";

            // cmbSupplier
            this.cmbSupplier.FormattingEnabled = true;
            this.cmbSupplier.Location = new System.Drawing.Point(100, 12);
            this.cmbSupplier.Name = "cmbSupplier";
            this.cmbSupplier.Size = new System.Drawing.Size(250, 21);
            this.cmbSupplier.TabIndex = 1;

            // lblOrderDate
            this.lblOrderDate.AutoSize = true;
            this.lblOrderDate.Location = new System.Drawing.Point(12, 45);
            this.lblOrderDate.Name = "lblOrderDate";
            this.lblOrderDate.Size = new System.Drawing.Size(69, 13);
            this.lblOrderDate.TabIndex = 2;
            this.lblOrderDate.Text = "Order Date:";

            // dtpOrderDate
            this.dtpOrderDate.Location = new System.Drawing.Point(100, 39);
            this.dtpOrderDate.Name = "dtpOrderDate";
            this.dtpOrderDate.Size = new System.Drawing.Size(250, 20);
            this.dtpOrderDate.TabIndex = 3;

            // lblExpectedDelivery
            this.lblExpectedDelivery.AutoSize = true;
            this.lblExpectedDelivery.Location = new System.Drawing.Point(12, 75);
            this.lblExpectedDelivery.Name = "lblExpectedDelivery";
            this.lblExpectedDelivery.Size = new System.Drawing.Size(106, 13);
            this.lblExpectedDelivery.TabIndex = 4;
            this.lblExpectedDelivery.Text = "Expected Delivery:";

            // dtpExpectedDelivery
            this.dtpExpectedDelivery.Location = new System.Drawing.Point(100, 69);
            this.dtpExpectedDelivery.Name = "dtpExpectedDelivery";
            this.dtpExpectedDelivery.Size = new System.Drawing.Size(250, 20);
            this.dtpExpectedDelivery.TabIndex = 5;

            // lblPONumber
            this.lblPONumber.AutoSize = true;
            this.lblPONumber.Location = new System.Drawing.Point(12, 105);
            this.lblPONumber.Name = "lblPONumber";
            this.lblPONumber.Size = new System.Drawing.Size(66, 13);
            this.lblPONumber.TabIndex = 6;
            this.lblPONumber.Text = "PO Number:";

            // txtPONumber
            this.txtPONumber.Location = new System.Drawing.Point(100, 102);
            this.txtPONumber.Name = "txtPONumber";
            this.txtPONumber.Size = new System.Drawing.Size(250, 20);
            this.txtPONumber.TabIndex = 7;

            // lblStatus
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(12, 135);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(41, 13);
            this.lblStatus.TabIndex = 8;
            this.lblStatus.Text = "Status:";

            // cmbStatus
            this.cmbStatus.FormattingEnabled = true;
            this.cmbStatus.Items.AddRange(new object[] { "Pending", "Confirmed", "Shipped", "Delivered", "Cancelled" });
            this.cmbStatus.Location = new System.Drawing.Point(100, 132);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(250, 21);
            this.cmbStatus.TabIndex = 9;

            // lblSubtotal
            this.lblSubtotal.AutoSize = true;
            this.lblSubtotal.Location = new System.Drawing.Point(12, 165);
            this.lblSubtotal.Name = "lblSubtotal";
            this.lblSubtotal.Size = new System.Drawing.Size(52, 13);
            this.lblSubtotal.TabIndex = 10;
            this.lblSubtotal.Text = "Subtotal:";

            // txtSubtotal
            this.txtSubtotal.Location = new System.Drawing.Point(100, 162);
            this.txtSubtotal.Name = "txtSubtotal";
            this.txtSubtotal.Size = new System.Drawing.Size(250, 20);
            this.txtSubtotal.TabIndex = 11;

            // lblTax
            this.lblTax.AutoSize = true;
            this.lblTax.Location = new System.Drawing.Point(12, 195);
            this.lblTax.Name = "lblTax";
            this.lblTax.Size = new System.Drawing.Size(28, 13);
            this.lblTax.TabIndex = 12;
            this.lblTax.Text = "Tax:";

            // txtTax
            this.txtTax.Location = new System.Drawing.Point(100, 192);
            this.txtTax.Name = "txtTax";
            this.txtTax.Size = new System.Drawing.Size(250, 20);
            this.txtTax.TabIndex = 13;

            // lblTotal
            this.lblTotal.AutoSize = true;
            this.lblTotal.Location = new System.Drawing.Point(12, 225);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(38, 13);
            this.lblTotal.TabIndex = 14;
            this.lblTotal.Text = "Total:";

            // txtTotal
            this.txtTotal.Location = new System.Drawing.Point(100, 222);
            this.txtTotal.Name = "txtTotal";
            this.txtTotal.Size = new System.Drawing.Size(250, 20);
            this.txtTotal.TabIndex = 15;

            // lblRequestedBy
            this.lblRequestedBy.AutoSize = true;
            this.lblRequestedBy.Location = new System.Drawing.Point(12, 255);
            this.lblRequestedBy.Name = "lblRequestedBy";
            this.lblRequestedBy.Size = new System.Drawing.Size(73, 13);
            this.lblRequestedBy.TabIndex = 16;
            this.lblRequestedBy.Text = "Requested By:";

            // txtRequestedBy
            this.txtRequestedBy.Location = new System.Drawing.Point(100, 252);
            this.txtRequestedBy.Name = "txtRequestedBy";
            this.txtRequestedBy.Size = new System.Drawing.Size(250, 20);
            this.txtRequestedBy.TabIndex = 17;

            // btnOK
            this.ButtonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ButtonOK.Location = new System.Drawing.Point(180, 290);
            this.ButtonOK.Name = "ButtonOK";
            this.ButtonOK.Size = new System.Drawing.Size(75, 23);
            this.ButtonOK.TabIndex = 18;
            this.ButtonOK.Text = "OK";
            this.ButtonOK.UseVisualStyleBackColor = true;
            this.ButtonOK.Click += new System.EventHandler(this.ButtonOK_Click);

            // btnCancel
            this.ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ButtonCancel.Location = new System.Drawing.Point(275, 290);
            this.ButtonCancel.Name = "ButtonCancel";
            this.ButtonCancel.Size = new System.Drawing.Size(75, 23);
            this.ButtonCancel.TabIndex = 19;
            this.ButtonCancel.Text = "Cancel";
            this.ButtonCancel.UseVisualStyleBackColor = true;
            this.ButtonCancel.Click += new System.EventHandler(this.ButtonCancel_Click);

            // NewOrderForm
            this.AcceptButton = this.ButtonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.ButtonCancel;
            this.ClientSize = new System.Drawing.Size(370, 325);
            this.Controls.Add(this.ButtonCancel);
            this.Controls.Add(this.ButtonOK);
            this.Controls.Add(this.txtRequestedBy);
            this.Controls.Add(this.lblRequestedBy);
            this.Controls.Add(this.txtTotal);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.txtTax);
            this.Controls.Add(this.lblTax);
            this.Controls.Add(this.txtSubtotal);
            this.Controls.Add(this.lblSubtotal);
            this.Controls.Add(this.cmbStatus);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.txtPONumber);
            this.Controls.Add(this.lblPONumber);
            this.Controls.Add(this.dtpExpectedDelivery);
            this.Controls.Add(this.lblExpectedDelivery);
            this.Controls.Add(this.dtpOrderDate);
            this.Controls.Add(this.lblOrderDate);
            this.Controls.Add(this.cmbSupplier);
            this.Controls.Add(this.lblSupplier);
            this.Name = "NewOrderForm";
            this.Text = "New Order";
            this.Load += new System.EventHandler(this.NewOrderForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblSupplier;
        private System.Windows.Forms.ComboBox cmbSupplier;
        private System.Windows.Forms.Label lblOrderDate;
        private System.Windows.Forms.DateTimePicker dtpOrderDate;
        private System.Windows.Forms.Label lblExpectedDelivery;
        private System.Windows.Forms.DateTimePicker dtpExpectedDelivery;
        private System.Windows.Forms.Label lblPONumber;
        private System.Windows.Forms.TextBox txtPONumber;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ComboBox cmbStatus;
        private System.Windows.Forms.Label lblSubtotal;
        private System.Windows.Forms.TextBox txtSubtotal;
        private System.Windows.Forms.Label lblTax;
        private System.Windows.Forms.TextBox txtTax;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.TextBox txtTotal;
        private System.Windows.Forms.Label lblRequestedBy;
        private System.Windows.Forms.TextBox txtRequestedBy;
        private System.Windows.Forms.Button ButtonOK;
        private System.Windows.Forms.Button ButtonCancel;
    }
}
