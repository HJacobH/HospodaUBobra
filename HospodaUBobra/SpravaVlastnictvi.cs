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
    public partial class SpravaVlastnictvi : Form
    {
        string connectionString = $"User Id=st69639;Password=Server2022;Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=fei-sql3.upceucebny.cz)(PORT=1521)))(CONNECT_DATA=(SID=BDAS)));";

        public SpravaVlastnictvi()
        {
            InitializeComponent();

            LoadPivovary();
            LoadVlastnici();
            LoadVlastnictvi();
        }

        private void LoadPivovary()
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT * FROM A_CB_PIVOVARY_VIEW";
                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    {
                        OracleDataAdapter adapter = new OracleDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        cmbPivovary.DisplayMember = "NAZEV";
                        cmbPivovary.ValueMember = "ID_PIVOVARU";
                        cmbPivovary.DataSource = dt;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading Pivovary: {ex.Message}");
                }
            }
        }

        private void LoadVlastnici()
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    // Query to concatenate JMENO_NAZEV and PRIJMENI
                    string query = "SELECT * FROM A_CB_VLASTNICI_PIVOVARU_VIEW";

                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    {
                        OracleDataAdapter adapter = new OracleDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        // Set ComboBox Display and Value Members
                        cmbVlastnici.DisplayMember = "FULL_NAME"; // Display concatenated name
                        cmbVlastnici.ValueMember = "ID_VLASTNIKA"; // Use ID as the value
                        cmbVlastnici.DataSource = dt;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading Vlastnici: {ex.Message}");
                }
            }
        }


        // Load data into DataGridView
        private void LoadVlastnictvi()
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT * FROM A_DGV_VLASTNIVCTVI_VIEW";

                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    {
                        OracleDataAdapter adapter = new OracleDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        // Bind data to the DataGridView
                        DataGridViewFilterHelper.BindData(dataGridViewVlastnictvi, dt);
                        dataGridViewVlastnictvi.DataSource = dt;

                        // Hide the ID column
                        dataGridViewVlastnictvi.Columns["ID"].Visible = false;

                        // Set display names for the columns
                        dataGridViewVlastnictvi.Columns["PIVOVAR_NAME"].HeaderText = "Pivovar";
                        dataGridViewVlastnictvi.Columns["VLASTNIK_NAME"].HeaderText = "Vlastník";
                        dataGridViewVlastnictvi.Columns["DATUM_POCATKU_VLASTNICTVI"].HeaderText = "Datum Počátku Vlastnictví";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading Vlastnictvi: {ex.Message}");
                }
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            DataGridViewFilterHelper.FilterData(dataGridViewVlastnictvi, txtSearch);
        }

        private void dataGridViewVlastnictvi_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewVlastnictvi.CurrentRow != null && dataGridViewVlastnictvi.CurrentRow.Index >= 0)
            {
                DataGridViewRow row = dataGridViewVlastnictvi.CurrentRow;

                // Set ComboBox values
                cmbPivovary.Text = row.Cells["PIVOVAR_NAME"].Value.ToString();
                cmbVlastnici.Text = row.Cells["VLASTNIK_NAME"].Value.ToString();

                // Set the DateTimePicker value
                if (row.Cells["DATUM_POCATKU_VLASTNICTVI"].Value != DBNull.Value)
                {
                    dtpDatumPocatku.Value = Convert.ToDateTime(row.Cells["DATUM_POCATKU_VLASTNICTVI"].Value);
                }
                else
                {
                    dtpDatumPocatku.Value = DateTime.Now; // Default value if null
                }
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                using (OracleConnection conn = new OracleConnection(connectionString))
                {
                    conn.Open();
                    using (OracleCommand cmd = new OracleCommand("sprava_vlastnictvi", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Parameters
                        cmd.Parameters.Add("p_id", OracleDbType.Int32).Value = DBNull.Value; // NULL for insert
                        cmd.Parameters.Add("p_datum_pocatku", OracleDbType.Date).Value = dtpDatumPocatku.Value;
                        cmd.Parameters.Add("p_pivovar_id", OracleDbType.Int32).Value = cmbPivovary.SelectedValue;
                        cmd.Parameters.Add("p_vlastnik_id", OracleDbType.Int32).Value = cmbVlastnici.SelectedValue;

                        // Execute the procedure
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Nové vlastnictví bylo úspěšně přidáno.", "Úspěch", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadVlastnictvi(); // Refresh the DataGridView
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Chyba při přidávání vlastnictví: {ex.Message}", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridViewVlastnictvi.CurrentRow != null)
            {
                int selectedId = Convert.ToInt32(dataGridViewVlastnictvi.CurrentRow.Cells["ID"].Value);

                try
                {
                    using (OracleConnection conn = new OracleConnection(connectionString))
                    {
                        conn.Open();
                        using (OracleCommand cmd = new OracleCommand("sprava_vlastnictvi", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            // Parameters
                            cmd.Parameters.Add("p_id", OracleDbType.Int32).Value = selectedId; // ID of the record to update
                            cmd.Parameters.Add("p_datum_pocatku", OracleDbType.Date).Value = dtpDatumPocatku.Value;
                            cmd.Parameters.Add("p_pivovar_id", OracleDbType.Int32).Value = cmbPivovary.SelectedValue;
                            cmd.Parameters.Add("p_vlastnik_id", OracleDbType.Int32).Value = cmbVlastnici.SelectedValue;

                            // Execute the procedure
                            cmd.ExecuteNonQuery();

                            MessageBox.Show("Vlastnictví bylo úspěšně aktualizováno.", "Úspěch", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadVlastnictvi(); // Refresh the DataGridView
                        }
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Chyba při aktualizaci vlastnictví: {ex.Message}", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            else
            {
                MessageBox.Show("Vyberte řádek pro aktualizaci.", "Upozornění", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridViewVlastnictvi.CurrentRow != null)
            {
                int selectedId = Convert.ToInt32(dataGridViewVlastnictvi.CurrentRow.Cells["ID"].Value);

                var result = MessageBox.Show("Opravdu chcete tento záznam smazat?", "Potvrzení smazání",
                                             MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        using (OracleConnection conn = new OracleConnection(connectionString))
                        {
                            conn.Open();

                            // Simple DELETE command
                            string query = "DELETE FROM VLASTNICTVI WHERE ID = :id";

                            using (OracleCommand cmd = new OracleCommand(query, conn))
                            {
                                cmd.Parameters.Add("id", OracleDbType.Int32).Value = selectedId;

                                int rowsAffected = cmd.ExecuteNonQuery();
                                if (rowsAffected > 0)
                                {
                                    MessageBox.Show("Záznam vlastnictví byl úspěšně smazán.", "Úspěch", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    LoadVlastnictvi();
                                }
                                else
                                {
                                    MessageBox.Show("Záznam s tímto ID nebyl nalezen.", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Chyba při mazání vlastnictví: {ex.Message}", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Vyberte řádek pro smazání.", "Upozornění", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
