using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospodaUBobra
{
    public static class DataGridViewFilterHelper
    {
        private static DataTable originalDataTable;

        /// <summary>
        /// Binds the initial data to the DataGridView and stores it for filtering.
        /// </summary>
        /// <param name="dataGridView">The DataGridView to display data.</param>
        /// <param name="dataTable">The DataTable containing the data.</param>
        public static void BindData(DataGridView dataGridView, DataTable dataTable)
        {
            originalDataTable = dataTable.Copy();
            dataGridView.DataSource = dataTable;
        }

        /// <summary>
        /// Filters the DataGridView rows based on the search text.
        /// </summary>
        /// <param name="dataGridView">The DataGridView to filter.</param>
        /// <param name="textBox">The TextBox containing the search text.</param>
        public static void FilterData(DataGridView dataGridView, TextBox textBox)
        {
            if (originalDataTable == null) return;

            string filterText = textBox.Text.Trim().ToLower();

            if (string.IsNullOrEmpty(filterText))
            {
                dataGridView.DataSource = originalDataTable;
            }
            else
            {
                DataTable filteredTable = originalDataTable.Clone();

                foreach (DataRow row in originalDataTable.Rows)
                {
                    bool matches = originalDataTable.Columns.Cast<DataColumn>().Any(col =>
                    {
                        if (row[col] != DBNull.Value)
                        {
                            return row[col].ToString().ToLower().Contains(filterText);
                        }
                        return false;
                    });

                    if (matches)
                    {
                        filteredTable.ImportRow(row);
                    }
                }

                dataGridView.DataSource = filteredTable;
            }
        }
    }
}
