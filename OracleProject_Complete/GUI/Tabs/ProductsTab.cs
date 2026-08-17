using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace OracleProject
{
    /// <summary>
    /// Products tab: product management with inline editor.
    /// </summary>
    internal sealed class ProductsTab : DashboardTabBase
    {
        private Panel toolbar;
        private Button btnNewProduct, btnEditProduct, btnDeleteProduct;
        private Label lblProductSearch, lblProductCategory;
        private TextBox txtProductSearch;
        private ComboBox cmbProductCategory;
        private DataGridView dgvProducts;
        
        // Inline editor
        private Panel editorPanel;
        private TextBox txtProductCode, txtProductName, txtUnitPrice;
        private ComboBox cmbCategory;
        private NumericUpDown numStock;
        private CheckBox chkActive;
        private Button btnSave, btnCancel;
        private string editingProductCode;

        public ProductsTab()
        {
            TabTitle = "Products";
            RequiredPermission = "PRODUCTS_VIEW";
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            BackColor = Color.FromArgb(247, 250, 252);
            Dock = DockStyle.Fill;

            // Toolbar
            toolbar = new Panel
            {
                BackColor = Color.White,
                Dock = DockStyle.Top,
                Height = 94
            };

            // Buttons
            btnNewProduct = CreatePageButton("Add Product", Color.FromArgb(49, 130, 206), 15, 22);
            btnNewProduct.Size = new Size(180, 50);
            btnNewProduct.Click += BtnNewProduct_Click;

            btnEditProduct = CreatePageButton("Edit", Color.FromArgb(72, 187, 120), 210, 22);
            btnEditProduct.Size = new Size(120, 50);
            btnEditProduct.Click += BtnEditProduct_Click;

            btnDeleteProduct = CreatePageButton("Delete", Color.FromArgb(245, 101, 101), 345, 22);
            btnDeleteProduct.Size = new Size(120, 50);
            btnDeleteProduct.Click += BtnDeleteProduct_Click;

            // Search label
            lblProductSearch = new Label
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                AutoSize = true,
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.Gray,
                Location = new Point(0, 34),
                Text = "Search:"
            };

            // Search textbox
            txtProductSearch = new TextBox
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Segoe UI", 9.5F),
                Location = new Point(0, 28),
                Size = new Size(269, 41)
            };

            // Category label
            lblProductCategory = new Label
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                AutoSize = true,
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.Gray,
                Location = new Point(0, 34),
                Text = "Category:"
            };

            // Category combo
            cmbProductCategory = new ComboBox
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 9.5F),
                Items = { "All", "Office Supplies", "IT Equipment", "Furniture", "Raw Materials", "Services" },
                Location = new Point(0, 28),
                Size = new Size(238, 43)
            };
            cmbProductCategory.SelectedIndex = 0;

            toolbar.Controls.Add(btnNewProduct);
            toolbar.Controls.Add(btnEditProduct);
            toolbar.Controls.Add(btnDeleteProduct);
            toolbar.Controls.Add(lblProductSearch);
            toolbar.Controls.Add(txtProductSearch);
            toolbar.Controls.Add(lblProductCategory);
            toolbar.Controls.Add(cmbProductCategory);

            // Inline Editor Panel
            editorPanel = new Panel
            {
                BackColor = Color.White,
                Dock = DockStyle.Top,
                Height = 140,
                Visible = false
            };

            // Editor controls
            txtProductCode = CreateEditorTextBox(20, 40, 160);
            txtProductName = CreateEditorTextBox(200, 40, 240);
            cmbCategory = CreateEditorCombo(460, 40, 180);
            cmbCategory.Items.AddRange(new object[]
            {
                "Electronics",
                "Office Supplies",
                "Raw Materials",
                "Components",
                "Other"
            });
            txtUnitPrice = CreateEditorTextBox(660, 40, 140);
            numStock = new NumericUpDown
            {
                Location = new Point(820, 40),
                Maximum = 999999,
                DecimalPlaces = 0,
                Size = new Size(120, 30)
            };
            chkActive = new CheckBox
            {
                Location = new Point(20, 96),
                Size = new Size(120, 28),
                Text = "Active",
                Checked = true
            };
            btnSave = CreatePageButton("Save", Color.FromArgb(45, 101, 181), 660, 88);
            btnSave.Size = new Size(90, 40);
            btnSave.Click += BtnSave_Click;
            btnCancel = CreatePageButton("Cancel", Color.FromArgb(107, 114, 128), 760, 88);
            btnCancel.Size = new Size(90, 40);
            btnCancel.Click += BtnCancel_Click;

            editorPanel.Controls.Add(CreateEditorLabel("Product Code", 20, 16));
            editorPanel.Controls.Add(txtProductCode);
            editorPanel.Controls.Add(CreateEditorLabel("Product Name", 200, 16));
            editorPanel.Controls.Add(txtProductName);
            editorPanel.Controls.Add(CreateEditorLabel("Category", 460, 16));
            editorPanel.Controls.Add(cmbCategory);
            editorPanel.Controls.Add(CreateEditorLabel("Unit Price", 660, 16));
            editorPanel.Controls.Add(txtUnitPrice);
            editorPanel.Controls.Add(CreateEditorLabel("Stock Qty", 820, 16));
            editorPanel.Controls.Add(numStock);
            editorPanel.Controls.Add(chkActive);
            editorPanel.Controls.Add(btnSave);
            editorPanel.Controls.Add(btnCancel);

            // Grid
            dgvProducts = new DataGridView
            {
                AllowUserToAddRows = false,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                EnableHeadersVisualStyles = false,
                GridColor = Color.FromArgb(226, 232, 240),
                Location = new Point(0, 94),
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
            dgvProducts.ColumnHeadersDefaultCellStyle = headerStyle;
            dgvProducts.ColumnHeadersHeight = 46;

            var cellStyle = new DataGridViewCellStyle
            {
                Alignment = DataGridViewContentAlignment.MiddleLeft,
                BackColor = Color.White,
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = Color.Black
            };
            dgvProducts.DefaultCellStyle = cellStyle;

            ApplyStockColor(dgvProducts, 9);

            Controls.Add(dgvProducts);
            Controls.Add(editorPanel);
            Controls.Add(toolbar);

            // Layout
            toolbar.Resize += Toolbar_Resize;
            Resize += ProductsTab_Resize;
        }

        private void Toolbar_Resize(object sender, EventArgs e)
        {
            cmbProductCategory.Location = new Point(toolbar.Width - cmbProductCategory.Width - 20, 28);
            lblProductCategory.Location = new Point(toolbar.Width - cmbProductCategory.Width - 130, 34);
            txtProductSearch.Location = new Point(toolbar.Width - cmbProductCategory.Width - txtProductSearch.Width - 140, 28);
            lblProductSearch.Location = new Point(toolbar.Width - cmbProductCategory.Width - txtProductSearch.Width - 230, 34);
        }

        private void ProductsTab_Resize(object sender, EventArgs e)
        {
            LayoutEditor();
        }

        private void LayoutEditor()
        {
            dgvProducts.Top = editorPanel.Visible ? toolbar.Height + editorPanel.Height : toolbar.Height;
            dgvProducts.Height = Math.Max(200, ClientSize.Height - dgvProducts.Top);
        }

        public override void ApplyPermissions()
        {
            btnNewProduct.Visible = HasPermission("PRODUCTS_ADD");
            btnEditProduct.Visible = HasPermission("PRODUCTS_EDIT");
            btnDeleteProduct.Visible = HasPermission("PRODUCTS_DELETE");
        }

        public override void OnActivated()
        {
            RefreshData();
        }

        public override void RefreshData()
        {
            try
            {
                LoadProducts();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Refresh Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadProducts()
        {
            const string sql = @"
                SELECT product_code AS ""Product ID"",
                       product_name AS ""Product Name"",
                       category AS ""Category"",
                       unit AS ""Unit"",
                       unit_price AS ""Unit Price"",
                       stock_qty AS ""Stock Qty"",
                       reorder_level AS ""Reorder Level"",
                       preferred_supplier AS ""Preferred Supplier"",
                       CAST(NULL AS DATE) AS ""Last Ordered"",
                       stock_status AS ""Status""
                  FROM vw_stock_report
                 ORDER BY product_name";

            dgvProducts.DataSource = OracleDb.Query(sql);
        }

        private void BtnNewProduct_Click(object sender, EventArgs e)
        {
            if (!RequirePermission("PRODUCTS_ADD"))
                return;

            ShowEditor(null);
        }

        private void BtnEditProduct_Click(object sender, EventArgs e)
        {
            if (!RequirePermission("PRODUCTS_EDIT"))
                return;

            if (dgvProducts.SelectedRows.Count == 0)
            {
                MessageBox.Show(this, "Please select a product to edit.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ShowEditor(dgvProducts.SelectedRows[0]);
        }

        private void ShowEditor(DataGridViewRow row)
        {
            bool isEdit = row != null;
            editingProductCode = isEdit ? Convert.ToString(row.Cells[0].Value) : null;
            txtProductCode.Text = isEdit ? editingProductCode : string.Empty;
            txtProductCode.ReadOnly = isEdit;
            txtProductName.Text = isEdit ? Convert.ToString(row.Cells[1].Value) : string.Empty;

            string category = isEdit ? Convert.ToString(row.Cells[2].Value) : "Other";
            if (cmbCategory.Items.Contains(category))
                cmbCategory.SelectedItem = category;
            else
                cmbCategory.Text = category;

            txtUnitPrice.Text = isEdit ? Convert.ToString(row.Cells[4].Value) : "0";
            decimal stock;
            numStock.Value = isEdit && decimal.TryParse(Convert.ToString(row.Cells[5].Value), out stock)
                ? Math.Max(numStock.Minimum, Math.Min(numStock.Maximum, stock))
                : 0;
            chkActive.Checked = true;

            editorPanel.Visible = true;
            LayoutEditor();
            txtProductName.Focus();
        }

        private void HideEditor()
        {
            editingProductCode = null;
            txtProductCode.ReadOnly = false;
            txtProductCode.Clear();
            txtProductName.Clear();
            txtUnitPrice.Text = "0";
            numStock.Value = 0;
            chkActive.Checked = true;
            editorPanel.Visible = false;
            LayoutEditor();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(editingProductCode) && !RequirePermission("PRODUCTS_ADD"))
                return;
            if (!string.IsNullOrWhiteSpace(editingProductCode) && !RequirePermission("PRODUCTS_EDIT"))
                return;

            string code = txtProductCode.Text.Trim();
            string name = txtProductName.Text.Trim();
            string category = Convert.ToString(cmbCategory.Text).Trim();
            decimal price;

            if (string.IsNullOrWhiteSpace(code))
            {
                MessageBox.Show(this, "Product code is required.", "Products",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show(this, "Product name is required.", "Products",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!decimal.TryParse(txtUnitPrice.Text, out price) || price < 0)
            {
                MessageBox.Show(this, "Unit price must be a valid number.", "Products",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (string.IsNullOrWhiteSpace(editingProductCode))
                {
                    OracleDb.Execute(@"
                        INSERT INTO products (
                            product_id, product_code, product_name, category,
                            unit, unit_price, stock_qty, status
                        ) VALUES (
                            product_seq.NEXTVAL, :product_code, :product_name,
                            :category, 'UNIT', :unit_price, :stock_qty, :status
                        )",
                        OracleDb.Parameter("product_code", code),
                        OracleDb.Parameter("product_name", name),
                        OracleDb.Parameter("category", category),
                        OracleDb.Parameter("unit_price", price),
                        OracleDb.Parameter("stock_qty", numStock.Value),
                        OracleDb.Parameter("status", chkActive.Checked ? 1 : 0));
                }
                else
                {
                    OracleDb.Execute(@"
                        UPDATE products
                           SET product_name = :product_name,
                               category = :category,
                               unit_price = :unit_price,
                               stock_qty = :stock_qty,
                               status = :status
                         WHERE product_code = :product_code",
                        OracleDb.Parameter("product_name", name),
                        OracleDb.Parameter("category", category),
                        OracleDb.Parameter("unit_price", price),
                        OracleDb.Parameter("stock_qty", numStock.Value),
                        OracleDb.Parameter("status", chkActive.Checked ? 1 : 0),
                        OracleDb.Parameter("product_code", editingProductCode));
                }

                HideEditor();
                LoadProducts();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Save Product Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            HideEditor();
        }

        private void BtnDeleteProduct_Click(object sender, EventArgs e)
        {
            if (!RequirePermission("PRODUCTS_DELETE"))
                return;

            if (dgvProducts.SelectedRows.Count == 0)
            {
                MessageBox.Show(this, "Please select a product to deactivate.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string code = Convert.ToString(dgvProducts.SelectedRows[0].Cells[0].Value);
            if (MessageBox.Show(this, "Deactivate product " + code + "?", "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                OracleDb.Execute(
                    "UPDATE products SET status = 0 WHERE product_code = :product_code",
                    OracleDb.Parameter("product_code", code));
                RefreshData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Deactivate Product Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
