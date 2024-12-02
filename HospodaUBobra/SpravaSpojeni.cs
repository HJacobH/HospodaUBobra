using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HospodaUBobra
{
    public partial class SpravaSpojeni : Form
    {
        string connectionString = $"User Id=st69639;Password=Server2022;Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=fei-sql3.upceucebny.cz)(PORT=1521)))(CONNECT_DATA=(SID=BDAS)));";
        private string selectedTable;
        public SpravaSpojeni()
        {
            InitializeComponent();

            LoadTableNames();
        }
        private void LoadTableNames()
        {
            List<string> tables = new List<string>
    {
        "FYZICKE_OSOBY",
        "PODROBNOSTI_RECENZE",
        "PRACOVNICI",
        "VLASTNICTVI",
        "VYROBY",
        "POZICE_PRACOVNIKA"
    };

            cbTables.DataSource = tables;
        }
        private void cbTables_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedTable = cbTables.SelectedItem.ToString();
            LoadTableData(selectedTable);
        }

        private void LoadTableData(string tableName)
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();
                string query = $"SELECT * FROM {tableName}";

                using (OracleCommand cmd = new OracleCommand(query, conn))
                {
                    using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dgvTableData.DataSource = dt;
                    }
                }
            }

            txtAktualni.Text = $"Aktuální číselník: {tableName}";
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string selectedTable = cbTables.SelectedItem.ToString();

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (OracleCommand cmd = new OracleCommand($"SPRAVA_{selectedTable.ToUpper()}", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Add parameters dynamically based on the table
                        switch (selectedTable)
                        {
                            case "FYZICKE_OSOBY":
                                cmd.CommandType = CommandType.StoredProcedure;

                                // Set parameters for the procedure
                                cmd.Parameters.Add("p_identifikator", OracleDbType.Int32).Value = DBNull.Value; // New record
                                cmd.Parameters.Add("p_id", OracleDbType.Int32).Value = GetNextIdForTable("FYZICKE_OSOBY"); // Generate new ID
                                cmd.Parameters.Add("p_datum_narozeni", OracleDbType.Date).Value = dateTimePicker1.Value;
                                cmd.Parameters.Add("p_rodne_cislo", OracleDbType.Varchar2).Value = textBox1.Text;

                                break;

                            case "PODROBNOSTI_RECENZE":
                                cmd.Parameters.Add("p_identifikator", OracleDbType.Int32).Value = DBNull.Value; // New record
                                cmd.Parameters.Add("p_id_recenzni", OracleDbType.Int32).Value = DBNull.Value; // Auto-generate
                                cmd.Parameters.Add("p_datum", OracleDbType.Date).Value = dateTimePicker1.Value;
                                cmd.Parameters.Add("p_uzivatel_id", OracleDbType.Int32).Value = comboBox1.SelectedValue;
                                cmd.Parameters.Add("p_recenze_id", OracleDbType.Int32).Value = comboBox2.SelectedValue;
                                break;

                            case "PRACOVNICI":
                                cmd.Parameters.Add("p_identifikator", OracleDbType.Int32).Value = DBNull.Value; // New record
                                cmd.Parameters.Add("p_pivovar_id", OracleDbType.Int32).Value = comboBox1.SelectedValue;
                                cmd.Parameters.Add("p_zamestnanec_id", OracleDbType.Int32).Value = comboBox2.SelectedValue;
                                break;

                            case "VLASTNICTVI":
                                cmd.Parameters.Add("p_identifikator", OracleDbType.Int32).Value = DBNull.Value; // New record
                                cmd.Parameters.Add("p_datum", OracleDbType.Date).Value = dateTimePicker1.Value;
                                cmd.Parameters.Add("p_pivovar_id", OracleDbType.Int32).Value = comboBox1.SelectedValue;
                                cmd.Parameters.Add("p_vlastnik_id", OracleDbType.Int32).Value = comboBox2.SelectedValue;
                                break;

                            case "VYROBY":
                                cmd.Parameters.Add("p_identifikator", OracleDbType.Int32).Value = DBNull.Value; // New record
                                cmd.Parameters.Add("p_litry", OracleDbType.Int32).Value = int.Parse(textBox1.Text);
                                cmd.Parameters.Add("p_pivovar_id", OracleDbType.Int32).Value = comboBox1.SelectedValue;
                                cmd.Parameters.Add("p_pivo_id", OracleDbType.Int32).Value = comboBox2.SelectedValue;
                                break;

                            case "POZICE_PRACOVNIKA":
                                cmd.Parameters.Add("p_identifikator", OracleDbType.Int32).Value = DBNull.Value; // New record
                                cmd.Parameters.Add("p_id_pozice", OracleDbType.Int32).Value = DBNull.Value;
                                cmd.Parameters.Add("p_nazev_pozice", OracleDbType.Int32).Value = comboBox1.SelectedValue;
                                cmd.Parameters.Add("p_parent_id", OracleDbType.Int32).Value = comboBox2.SelectedValue;
                                break;

                            default:
                                MessageBox.Show("Adding is not supported for this table.");
                                return;
                        }

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Record successfully added!");
                        LoadTableData(selectedTable); // Reload the table to reflect changes
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error adding record: {ex.Message}");
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string selectedTable = cbTables.SelectedItem.ToString();

            if (dgvTableData.CurrentRow == null)
            {
                MessageBox.Show("Select a row to update.");
                return;
            }

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (OracleCommand cmd = new OracleCommand($"SPRAVA_{selectedTable.ToUpper()}", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Add parameters dynamically based on the table
                        switch (selectedTable)
                        {
                            case "FYZICKE_OSOBY":
                                cmd.Parameters.Add("p_identifikator", OracleDbType.Int32).Value = 1; // Update record
                                cmd.Parameters.Add("p_id", OracleDbType.Int32).Value = dgvTableData.CurrentRow.Cells["ID"].Value;
                                cmd.Parameters.Add("p_datum_narozeni", OracleDbType.Date).Value = dateTimePicker1.Value;
                                cmd.Parameters.Add("p_rodne_cislo", OracleDbType.Varchar2).Value = textBox1.Text;

                                break;

                            case "PODROBNOSTI_RECENZE":
                                cmd.Parameters.Add("p_identifikator", OracleDbType.Int32).Value = 1; // Update record
                                cmd.Parameters.Add("p_id_recenzni", OracleDbType.Int32).Value = dgvTableData.CurrentRow.Cells["ID"].Value;
                                cmd.Parameters.Add("p_datum", OracleDbType.Date).Value = dateTimePicker1.Value;
                                cmd.Parameters.Add("p_uzivatel_id", OracleDbType.Int32).Value = comboBox1.SelectedValue;
                                cmd.Parameters.Add("p_recenze_id", OracleDbType.Int32).Value = comboBox2.SelectedValue;
                                break;

                            case "PRACOVNICI":
                                cmd.Parameters.Add("p_identifikator", OracleDbType.Int32).Value = 1; // Update record
                                cmd.Parameters.Add("p_pivovar_id", OracleDbType.Int32).Value = comboBox1.SelectedValue;
                                cmd.Parameters.Add("p_zamestnanec_id", OracleDbType.Int32).Value = comboBox2.SelectedValue;
                                break;

                            case "VLASTNICTVI":
                                cmd.Parameters.Add("p_identifikator", OracleDbType.Int32).Value = 1; // Update record
                                cmd.Parameters.Add("p_datum", OracleDbType.Date).Value = dateTimePicker1.Value;
                                cmd.Parameters.Add("p_pivovar_id", OracleDbType.Int32).Value = comboBox1.SelectedValue;
                                cmd.Parameters.Add("p_vlastnik_id", OracleDbType.Int32).Value = comboBox2.SelectedValue;
                                break;

                            case "VYROBY":
                                cmd.Parameters.Add("p_identifikator", OracleDbType.Int32).Value = 1; // Update record
                                cmd.Parameters.Add("p_litry", OracleDbType.Int32).Value = int.Parse(textBox1.Text);
                                cmd.Parameters.Add("p_pivovar_id", OracleDbType.Int32).Value = comboBox1.SelectedValue;
                                cmd.Parameters.Add("p_pivo_id", OracleDbType.Int32).Value = comboBox2.SelectedValue;
                                break;

                            default:
                                MessageBox.Show("Updating is not supported for this table.");
                                return;
                        }

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Record successfully updated!");
                        LoadTableData(selectedTable); // Reload the table to reflect changes
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error updating record: {ex.Message}");
                }
            }
        }


        private void btnDelete_Click(object sender, EventArgs e)
        {
            string selectedTable = cbTables.SelectedItem.ToString();

            if (dgvTableData.CurrentRow == null)
            {
                MessageBox.Show("Select a row to delete.");
                return;
            }

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (OracleCommand cmd = new OracleCommand($"SPRAVA_{selectedTable.ToUpper()}", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Delete record by ID
                        switch (selectedTable)
                        {
                            case "FYZICKE_OSOBY":
                                cmd.Parameters.Add("p_identifikator", OracleDbType.Int32).Value = -1; // Delete record
                                cmd.Parameters.Add("p_id", OracleDbType.Int32).Value = dgvTableData.CurrentRow.Cells["ID"].Value;

                                break;

                            case "PODROBNOSTI_RECENZE":
                                cmd.Parameters.Add("p_identifikator", OracleDbType.Int32).Value = -1; // Delete record
                                cmd.Parameters.Add("p_id_recenzni", OracleDbType.Int32).Value = dgvTableData.CurrentRow.Cells["ID"].Value;
                                break;

                            case "PRACOVNICI":
                                cmd.Parameters.Add("p_identifikator", OracleDbType.Int32).Value = -1; // Delete record
                                cmd.Parameters.Add("p_pivovar_id", OracleDbType.Int32).Value = dgvTableData.CurrentRow.Cells["PIVOVAR_ID_PIVOVARU"].Value;
                                cmd.Parameters.Add("p_zamestnanec_id", OracleDbType.Int32).Value = dgvTableData.CurrentRow.Cells["ZAMESTNANEC_ID_ZAMESTNANCE"].Value;
                                break;

                            case "VLASTNICTVI":
                                cmd.Parameters.Add("p_identifikator", OracleDbType.Int32).Value = -1; // Delete record
                                cmd.Parameters.Add("p_pivovar_id", OracleDbType.Int32).Value = dgvTableData.CurrentRow.Cells["PIVOVAR_ID_PIVOVARU"].Value;
                                cmd.Parameters.Add("p_vlastnik_id", OracleDbType.Int32).Value = dgvTableData.CurrentRow.Cells["VLASTNIK_PIVOVARU_ID_VLASTNIKA"].Value;
                                break;

                            case "VYROBY":
                                cmd.Parameters.Add("p_identifikator", OracleDbType.Int32).Value = -1; // Delete record
                                cmd.Parameters.Add("p_pivovar_id", OracleDbType.Int32).Value = dgvTableData.CurrentRow.Cells["PIVOVAR_ID_PIVOVARU"].Value;
                                cmd.Parameters.Add("p_pivo_id", OracleDbType.Int32).Value = dgvTableData.CurrentRow.Cells["PIVO_ID_PIVA"].Value;
                                break;

                            default:
                                MessageBox.Show("Deleting is not supported for this table.");
                                return;
                        }

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Record successfully deleted!");
                        LoadTableData(selectedTable); // Reload the table to reflect changes
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting record: {ex.Message}");
                }
            }
        }

        private int GetNextIdForTable(string tableName)
        {
            int nextId = 0;

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    string query = $"SELECT NVL(MAX(ID), 0) + 1 FROM {tableName}";

                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    {
                        object result = cmd.ExecuteScalar();
                        nextId = Convert.ToInt32(result);
                    }
                }
                catch (OracleException ex)
                {
                    MessageBox.Show($"Error generating next ID: {ex.Message}");
                }
            }

            return nextId;
        }


        private void buttonBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvTableData_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvTableData.CurrentRow != null)
            {
                // Clear previous values
                textBox1.Text = string.Empty;
                comboBox1.DataSource = null;
                comboBox2.DataSource = null;
                dateTimePicker1.Value = DateTime.Now; // Default value

                // Get selected table from ComboBox
                string selectedTable = cbTables.SelectedItem.ToString();

                if (selectedTable == "PODROBNOSTI_RECENZE")
                {
                    // Populate ComboBox1 with UZIVATELE
                    PopulateComboBox("UZIVATELE", "ID_UZIVATELE", "UZIVATELSKE_JMENO", comboBox1);

                    // Populate ComboBox2 with RECENZE
                    PopulateComboBox("RECENZE", "ID_RECENZE", "TITULEK", comboBox2);
                }
                else if (selectedTable == "PRACOVNICI")
                {
                    // Populate ComboBox1 with PIVOVARY
                    PopulateComboBox("PIVOVARY", "ID_PIVOVARU", "NAZEV", comboBox1);

                    // Populate ComboBox2 with ZAMESTNANCI
                    PopulateComboBox("ZAMESTNANCI", "ID_ZAMESTNANCE", "PRIJMENI", comboBox2);
                }
                else if (selectedTable == "VLASTNICTVI")
                {
                    // Populate ComboBox1 with PIVOVARY
                    PopulateComboBox("PIVOVARY", "ID_PIVOVARU", "NAZEV", comboBox1);

                    // Populate ComboBox2 with VLASTNICI_PIVOVARU
                    PopulateComboBox("VLASTNICI_PIVOVARU", "ID_VLASTNIKA", "JMENO_NAZEV", comboBox2);
                }
                else if (selectedTable == "VYROBY")
                {
                    // Populate ComboBox1 with PIVOVARY
                    PopulateComboBox("PIVOVARY", "ID_PIVOVARU", "NAZEV", comboBox1);

                    // Populate ComboBox2 with PIVA
                    PopulateComboBox("PIVA", "ID_PIVA", "NAZEV", comboBox2);
                }
                else if (selectedTable == "POZICE_PRACOVNIKA")
                {
                    PopulateComboBox("POZICE_PRACOVNIKA", "ID_POZICE", "NAZEV_POZICE", comboBox1);

                    PopulateComboBox("POZICE_PRACOVNIKA", "ID_POZICE", "NAZEV_POZICE", comboBox2);
                }

                    // Update ComboBoxes and other controls based on selected row
                for (int i = 0; i < dgvTableData.CurrentRow.Cells.Count; i++)
                {
                    var columnName = dgvTableData.Columns[i].HeaderText;
                    var cellValue = dgvTableData.CurrentRow.Cells[i].Value;

                    if (selectedTable == "PODROBNOSTI_RECENZE")
                    {
                        // Update selected values for ComboBox1 and ComboBox2
                        if (columnName.ToUpper() == "UZIVATEL_ID_UZIVATELE")
                        {
                            comboBox1.SelectedValue = cellValue; // Preselect UZIVATEL_ID
                        }
                        else if (columnName.ToUpper() == "RECENZE_ID_RECENZE")
                        {
                            comboBox2.SelectedValue = cellValue; // Preselect RECENZE_ID
                        }
                        else if (columnName.ToUpper() == "DATUM_ZVEREJNENI") // Update DateTimePicker
                        {
                            if (DateTime.TryParse(cellValue?.ToString(), out DateTime dateValue))
                            {
                                dateTimePicker1.Value = dateValue;
                            }
                        }
                    }
                    else if (selectedTable == "PRACOVNICI")
                    {
                        if (columnName.ToUpper() == "PIVOVAR_ID_PIVOVARU")
                        {
                            comboBox1.SelectedValue = cellValue; // Preselect PIVOVAR_ID
                        }
                        else if (columnName.ToUpper() == "ZAMESTNANEC_ID_ZAMESTNANCE")
                        {
                            comboBox2.SelectedValue = cellValue; // Preselect ZAMESTNANEC_ID
                        }
                    }
                    else if (selectedTable == "VLASTNICTVI")
                    {
                        // Update selected values for ComboBox1 and ComboBox2
                        if (columnName.ToUpper() == "PIVOVAR_ID_PIVOVARU")
                        {
                            comboBox1.SelectedValue = cellValue; // Preselect PIVOVAR_ID
                        }
                        else if (columnName.ToUpper() == "VLASTNIK_PIVOVARU_ID_VLASTNIKA")
                        {
                            comboBox2.SelectedValue = cellValue; // Preselect VLASTNIK_ID
                        }
                        else if (columnName.ToUpper() == "DATUM_POCATKU_VLASTNICTVI") // Update DateTimePicker
                        {
                            if (DateTime.TryParse(cellValue?.ToString(), out DateTime dateValue))
                            {
                                dateTimePicker1.Value = dateValue;
                            }
                        }
                    }
                    else if (selectedTable == "VYROBY")
                    {
                        if (columnName.ToUpper() == "LITRY_ZA_DEN")
                        {
                            textBox1.Text = cellValue?.ToString(); // Display LITRY_ZA_DEN in textBox1
                        }
                        else if (columnName.ToUpper() == "PIVOVAR_ID_PIVOVARU")
                        {
                            comboBox1.SelectedValue = cellValue; // Preselect PIVOVAR_ID in comboBox1
                        }
                        else if (columnName.ToUpper() == "PIVO_ID_PIVA")
                        {
                            comboBox2.SelectedValue = cellValue; // Preselect PIVO_ID in comboBox2
                        }
                    }
                    else if (selectedTable == "POZICE_PRACOVNIKA")
                    {
                        if (columnName.ToUpper() == "ID_POZICE")
                        {
                            comboBox1.SelectedValue = cellValue; // Preselect PIVOVAR_ID
                        }
                        else if (columnName.ToUpper() == "PARENT_ID")
                        {
                            comboBox2.SelectedValue = cellValue; // Preselect ZAMESTNANEC_ID
                        }
                    }
                    else if (cellValue is DateTime || DateTime.TryParse(cellValue?.ToString(), out _))
                    {
                        // Map DateTime to DateTimePicker
                        dateTimePicker1.Value = Convert.ToDateTime(cellValue);
                    }
                    else
                    {
                        // Map other columns to TextBox
                        textBox1.Text = cellValue?.ToString();
                    }
                }
            }

        }
        private void PopulateComboBox(string tableName, string idColumn, string displayColumn, ComboBox comboBox)
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    // Query to fetch rows for the ComboBox
                    string query = $"SELECT {idColumn}, {displayColumn} FROM {tableName}";

                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    {
                        using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            // Bind the ComboBox to the data
                            comboBox.DataSource = dt;
                            comboBox.DisplayMember = displayColumn; // Display column
                            comboBox.ValueMember = idColumn; // Value column
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error populating ComboBox for {tableName}: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

    }
}
