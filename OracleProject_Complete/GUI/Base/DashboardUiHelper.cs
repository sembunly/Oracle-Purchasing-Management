using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OracleProject
{
    /// <summary>
    /// Shared UI helper methods for dashboard operations.
    /// </summary>
    internal static class DashboardUiHelper
    {
        /// <summary>
        /// Export grid rows to CSV file.
        /// </summary>
        public static void ExportGridRows(
            DataGridView grid,
            IEnumerable<DataGridViewRow> rows,
            string defaultFileName,
            string successMessage)
        {
            using (var dialog = new SaveFileDialog())
            {
                dialog.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
                dialog.FileName = defaultFileName;
                dialog.Title = "Export CSV";

                if (dialog.ShowDialog() != DialogResult.OK)
                    return;

                File.WriteAllText(
                    dialog.FileName,
                    BuildCsv(grid, rows),
                    Encoding.UTF8);

                MessageBox.Show(successMessage, "Export Complete",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// Build CSV content from grid rows.
        /// </summary>
        public static string BuildCsv(DataGridView grid, IEnumerable<DataGridViewRow> rows)
        {
            var csv = new StringBuilder();
            bool firstColumn = true;

            foreach (DataGridViewColumn column in grid.Columns)
            {
                if (!column.Visible)
                    continue;

                if (!firstColumn)
                    csv.Append(",");

                csv.Append(EscapeCsv(column.HeaderText));
                firstColumn = false;
            }
            csv.AppendLine();

            foreach (DataGridViewRow row in rows)
            {
                firstColumn = true;
                foreach (DataGridViewColumn column in grid.Columns)
                {
                    if (!column.Visible)
                        continue;

                    if (!firstColumn)
                        csv.Append(",");

                    csv.Append(EscapeCsv(Convert.ToString(row.Cells[column.Index].Value)));
                    firstColumn = false;
                }
                csv.AppendLine();
            }

            return csv.ToString();
        }

        /// <summary>
        /// Escape a value for CSV output.
        /// </summary>
        public static string EscapeCsv(string value)
        {
            if (value == null)
                return string.Empty;

            return "\"" + value.Replace("\"", "\"\"") + "\"";
        }

        /// <summary>
        /// Make a string safe for use as a file name.
        /// </summary>
        public static string SafeFileName(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return "selected";

            foreach (char invalid in Path.GetInvalidFileNameChars())
                value = value.Replace(invalid, '-');

            return value;
        }

        /// <summary>
        /// Sum a numeric column in a DataTable.
        /// </summary>
        public static decimal SumColumn(System.Data.DataTable table, string columnName)
        {
            decimal sum = 0;
            foreach (System.Data.DataRow row in table.Rows)
            {
                if (row[columnName] != DBNull.Value)
                    sum += Convert.ToDecimal(row[columnName], CultureInfo.InvariantCulture);
            }
            return sum;
        }
    }
}
