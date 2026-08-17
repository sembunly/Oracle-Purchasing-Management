using System;
using System.Drawing;
using System.Windows.Forms;

namespace OracleProject
{
    /// <summary>
    /// Base class for all dashboard tab UserControls.
    /// Provides common services: context, permission checks, and lifecycle hooks.
    /// </summary>
    internal class DashboardTabBase : UserControl
    {
        public DashboardContext Context { get; internal set; }

        public string TabTitle { get; protected set; }

        public string RequiredPermission { get; protected set; }

        /// <summary>
        /// Called when this tab is activated (shown).
        /// Override to load/refresh data.
        /// </summary>
        public virtual void OnActivated()
        {
        }

        /// <summary>
        /// Called when the global Refresh button is clicked.
        /// Override to refresh the current view.
        /// </summary>
        public virtual void RefreshData()
        {
        }

        /// <summary>
        /// Apply permissions to UI controls (hide buttons based on permissions).
        /// Called by the host after the tab is created.
        /// </summary>
        public virtual void ApplyPermissions()
        {
        }

        protected bool HasPermission(string permissionCode)
        {
            return Context != null && Context.HasPermission(permissionCode);
        }

        protected bool RequirePermission(string permissionCode)
        {
            if (Context == null)
                return false;
            return Context.RequirePermission(permissionCode, this);
        }

        /// <summary>
        /// Helper to create a standard page button.
        /// </summary>
        protected static Button CreatePageButton(string text, Color backColor, int left, int top)
        {
            var button = new Button
            {
                BackColor = backColor,
                Cursor = Cursors.Hand,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(left, top),
                Size = new Size(105, 48),
                Text = text,
                UseVisualStyleBackColor = false
            };
            button.FlatAppearance.BorderSize = 0;
            return button;
        }

        /// <summary>
        /// Helper to create a standard editor label.
        /// </summary>
        protected static Label CreateEditorLabel(string text, int left, int top)
        {
            return new Label
            {
                Location = new Point(left, top),
                Size = new Size(180, 22),
                Text = text,
                ForeColor = Color.FromArgb(55, 65, 81),
                Font = new Font("Segoe UI", 9F, FontStyle.Bold)
            };
        }

        /// <summary>
        /// Helper to create a standard editor combo box.
        /// </summary>
        protected static ComboBox CreateEditorCombo(int left, int top, int width)
        {
            return new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Location = new Point(left, top),
                Size = new Size(width, 30)
            };
        }

        /// <summary>
        /// Helper to create a standard editor text box.
        /// </summary>
        protected static TextBox CreateEditorTextBox(int left, int top, int width)
        {
            return new TextBox
            {
                Location = new Point(left, top),
                Size = new Size(width, 30)
            };
        }

        /// <summary>
        /// Helper to fill a combo box from a DataTable.
        /// </summary>
        protected static void FillCombo(
            ComboBox combo,
            System.Data.DataTable table,
            string idColumn,
            string textColumn,
            int? selectedId)
        {
            combo.Items.Clear();
            foreach (System.Data.DataRow row in table.Rows)
            {
                var item = new ComboItem(Convert.ToInt32(row[idColumn]), Convert.ToString(row[textColumn]));
                combo.Items.Add(item);
                if (selectedId.HasValue && item.Id == selectedId.Value)
                    combo.SelectedItem = item;
            }

            if (combo.SelectedIndex < 0 && combo.Items.Count > 0)
                combo.SelectedIndex = 0;
        }

        /// <summary>
        /// Apply status color formatting to a grid.
        /// </summary>
        protected static void ApplyStatusColor(DataGridView grid, int statusColumn)
        {
            grid.CellFormatting += (sender, e) =>
            {
                if (e.ColumnIndex != statusColumn || e.Value == null)
                    return;

                switch (Convert.ToString(e.Value).ToUpperInvariant())
                {
                    case "1":
                    case "APPROVED":
                    case "ACTIVE":
                    case "PAID":
                        e.CellStyle.ForeColor = Color.FromArgb(39, 174, 96);
                        break;
                    case "0":
                    case "PENDING":
                    case "PARTIAL":
                    case "PARTIALLY_RECEIVED":
                        e.CellStyle.ForeColor = Color.FromArgb(230, 126, 34);
                        break;
                    case "3":
                    case "RECEIVED":
                    case "CLOSED":
                        e.CellStyle.ForeColor = Color.FromArgb(49, 130, 206);
                        break;
                    case "4":
                    case "CANCELLED":
                    case "INACTIVE":
                    case "REJECTED":
                        e.CellStyle.ForeColor = Color.FromArgb(231, 76, 60);
                        break;
                }

                e.CellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            };
        }

        /// <summary>
        /// Apply stock status color formatting to a grid.
        /// </summary>
        protected static void ApplyStockColor(DataGridView grid, int statusColumn)
        {
            grid.CellFormatting += (sender, e) =>
            {
                if (e.ColumnIndex != statusColumn || e.Value == null)
                    return;

                switch (Convert.ToString(e.Value).ToUpperInvariant())
                {
                    case "IN STOCK":
                        e.CellStyle.ForeColor = Color.FromArgb(39, 174, 96);
                        break;
                    case "LOW STOCK":
                        e.CellStyle.ForeColor = Color.FromArgb(230, 126, 34);
                        break;
                    case "OUT OF STOCK":
                        e.CellStyle.ForeColor = Color.FromArgb(231, 76, 60);
                        break;
                }

                e.CellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            };
        }

        /// <summary>
        /// Hide a column in a DataGridView if it exists.
        /// </summary>
        protected static void HideGridColumn(DataGridView grid, string columnName)
        {
            if (grid.Columns.Contains(columnName))
                grid.Columns[columnName].Visible = false;
        }

        /// <summary>
        /// Set a DateTimePicker value from an object.
        /// </summary>
        protected static void SetDate(DateTimePicker picker, object value)
        {
            DateTime parsed;
            if (value != null && DateTime.TryParse(Convert.ToString(value), out parsed))
                picker.Value = parsed;
        }

        /// <summary>
        /// Get status text for purchase order status codes.
        /// </summary>
        protected static string PurchaseOrderStatusText(object value)
        {
            int code;
            if (!int.TryParse(Convert.ToString(value), out code))
                return Convert.ToString(value);

            switch (code)
            {
                case 0: return "DRAFT";
                case 1: return "APPROVED";
                case 2: return "PARTIALLY_RECEIVED";
                case 3: return "RECEIVED";
                case 4: return "CANCELLED";
                case 5: return "CLOSED";
                default: return Convert.ToString(value);
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // DashboardTabBase
            // 
            this.Name = "DashboardTabBase";
            this.Size = new System.Drawing.Size(354, 150);
            this.ResumeLayout(false);

        }
    }
}
