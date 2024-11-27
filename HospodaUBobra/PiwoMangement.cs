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
    public partial class PiwoMangement : Form
    {
        string connectionString = $"User Id=st69639;Password=Server2022;Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=fei-sql3.upceucebny.cz)(PORT=1521)))(CONNECT_DATA=(SID=BDAS)));";

        private List<Tuple<int, string>> packagingOptions;
        private List<Tuple<int, string>> unitOptions;
        int selectedBeerId;

        public PiwoMangement()
        {
            InitializeComponent();
            LoadPackagingOptions();
            LoadUnitOptions();
            LoadPivaData();
        }

        private void LoadPivaData()
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT * FROM PIVA"; // Query to fetch all rows from PIVA table

                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Bind the DataTable to the DataGridView
                        dgvPiva.DataSource = dataTable;

                        // Optionally, hide some columns or format the DataGridView
                        dgvPiva.Columns["ID_PIVA"].HeaderText = "Beer ID";
                        dgvPiva.Columns["NAZEV"].HeaderText = "Name";
                        dgvPiva.Columns["OBSAH_ALKOHOLU"].HeaderText = "Alcohol Content (%)";
                        dgvPiva.Columns["OBJEM"].HeaderText = "Volume (ml)";
                        dgvPiva.Columns["CENA"].HeaderText = "Price (CZK)";
                        dgvPiva.Columns["POCET_KS_SKLADEM"].HeaderText = "Stock Quantity";
                        dgvPiva.Columns["BALENI_PIVA_ID_BALENI"].HeaderText = "Packaging";
                        dgvPiva.Columns["JEDNOTKA_PIVA_ID_JEDN"].HeaderText = "Unit";
                    }
                }
                catch (OracleException ex)
                {
                    MessageBox.Show("Error fetching beer data: " + ex.Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("General error: " + ex.Message);
                }
            }
        }


        private void LoadPackagingOptions()
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT id_baleni, baleni FROM BALENI_PIV";
                using (OracleCommand cmd = new OracleCommand(query, conn))
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    packagingOptions = new List<Tuple<int, string>>();
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        packagingOptions.Add(new Tuple<int, string>(id, name));
                    }
                }
            }

            comboBoxPackaging.DataSource = packagingOptions;
            comboBoxPackaging.DisplayMember = "Item2";
            comboBoxPackaging.ValueMember = "Item1";
        }

        private void LoadUnitOptions()
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT id_jedn, jednotka FROM JEDNOTKY_PIV";
                using (OracleCommand cmd = new OracleCommand(query, conn))
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    unitOptions = new List<Tuple<int, string>>();
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        unitOptions.Add(new Tuple<int, string>(id, name));
                    }
                }
            }

            comboBoxUnit.DataSource = unitOptions;
            comboBoxUnit.DisplayMember = "Item2";
            comboBoxUnit.ValueMember = "Item1";
        }


        private void btnAddPiwo_Click(object sender, EventArgs e)
        {
            string nazev = txtBeerName.Text.Trim();
            decimal obsahAlkoholu;
            decimal objem;
            decimal cena;
            int pocetKsSkladem;
            int baleniPivaId = ((Tuple<int, string>)comboBoxPackaging.SelectedItem).Item1;
            int jednotkaPivaId = ((Tuple<int, string>)comboBoxUnit.SelectedItem).Item1;

            // Validate input
            if (string.IsNullOrEmpty(nazev) ||
                !decimal.TryParse(txtAlcoholContent.Text, out obsahAlkoholu) ||
                !decimal.TryParse(txtVolume.Text, out objem) ||
                !decimal.TryParse(txtPrice.Text, out cena) ||
                !int.TryParse(txtStockQuantity.Text, out pocetKsSkladem))
            {
                MessageBox.Show("Please enter valid values for all fields.");
                return;
            }

            ManageBeer(null, nazev, obsahAlkoholu, objem, cena, pocetKsSkladem, baleniPivaId, jednotkaPivaId);
            
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedBeerId == -1)
            {
                MessageBox.Show("No beer selected for update. Please select a beer.");
                return;
            }

            // Collect data from the form
            string nazev = txtBeerName.Text.Trim();
            decimal obsahAlkoholu;
            decimal objem;
            decimal cena;
            int pocetKsSkladem;
            int baleniPivaId = ((Tuple<int, string>)comboBoxPackaging.SelectedItem).Item1;
            int jednotkaPivaId = ((Tuple<int, string>)comboBoxUnit.SelectedItem).Item1;

            // Validate input
            if (string.IsNullOrEmpty(nazev) ||
                !decimal.TryParse(txtAlcoholContent.Text, out obsahAlkoholu) ||
                !decimal.TryParse(txtVolume.Text, out objem) ||
                !decimal.TryParse(txtPrice.Text, out cena) ||
                !int.TryParse(txtStockQuantity.Text, out pocetKsSkladem))
            {
                MessageBox.Show("Please enter valid values for all fields.");
                return;
            }

            // Update the selected beer
            ManageBeer(selectedBeerId, nazev, obsahAlkoholu, objem, cena, pocetKsSkladem, baleniPivaId, jednotkaPivaId);

        }

        private void ManageBeer(int? idPiva, string nazev, decimal obsahAlkoholu, decimal objem, decimal cena, int pocetKsSkladem, int baleniPivaId, int jednotkaPivaId)
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();
                using (OracleCommand cmd = new OracleCommand("sprava_piva", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Add parameters
                    cmd.Parameters.Add(new OracleParameter("p_identifikator", OracleDbType.Int32)).Value = idPiva.HasValue ? idPiva : DBNull.Value;
                    cmd.Parameters.Add(new OracleParameter("p_id_piva", OracleDbType.Int32)).Value = idPiva;
                    cmd.Parameters.Add(new OracleParameter("p_nazev", OracleDbType.Varchar2)).Value = nazev;
                    cmd.Parameters.Add(new OracleParameter("p_obsah_alkoholu", OracleDbType.Decimal)).Value = obsahAlkoholu;
                    cmd.Parameters.Add(new OracleParameter("p_objem", OracleDbType.Decimal)).Value = objem;
                    cmd.Parameters.Add(new OracleParameter("p_cena", OracleDbType.Decimal)).Value = cena;
                    cmd.Parameters.Add(new OracleParameter("p_pocet_ks_skladem", OracleDbType.Int32)).Value = pocetKsSkladem;
                    cmd.Parameters.Add(new OracleParameter("p_baleni_piva_id_baleni", OracleDbType.Int32)).Value = baleniPivaId;
                    cmd.Parameters.Add(new OracleParameter("p_jednotka_piva_id_jedn", OracleDbType.Int32)).Value = jednotkaPivaId;

                    try
                    {
                        cmd.ExecuteNonQuery();
                        string message = idPiva.HasValue ? "Beer updated successfully!" : "Beer added successfully!";
                        LoadPivaData();
                        MessageBox.Show(message);
                    }
                    catch (OracleException ex)
                    {
                        MessageBox.Show("Oracle error: " + ex.Message);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("General error: " + ex.Message);
                    }
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "DELETE FROM PIVA WHERE ID_PIVA = :id_piva";

                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    {
                        cmd.Parameters.Add(new OracleParameter(":id_piva", OracleDbType.Int32)).Value = selectedBeerId;
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Beer deleted successfully!");
                            LoadPivaData(); // Refresh the DataGridView
                            selectedBeerId = -1; // Reset the selectedBeerId
                            LoadPivaData();
                        }
                        else
                        {
                            MessageBox.Show("No beer found with the selected ID.");
                        }
                    }
                }
                catch (OracleException ex)
                {
                    MessageBox.Show("Oracle error: " + ex.Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("General error: " + ex.Message);
                }
            }
        }
    

        private void dgvPiva_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvPiva.CurrentRow != null)
            {
                selectedBeerId = Convert.ToInt32(dgvPiva.CurrentRow.Cells["id_piva"].Value);

                txtBeerName.Text = dgvPiva.CurrentRow.Cells["nazev"].Value.ToString();
                txtAlcoholContent.Text = dgvPiva.CurrentRow.Cells["obsah_alkoholu"].Value.ToString();
                txtVolume.Text = dgvPiva.CurrentRow.Cells["objem"].Value.ToString();
                txtPrice.Text = dgvPiva.CurrentRow.Cells["cena"].Value.ToString();
                txtStockQuantity.Text = dgvPiva.CurrentRow.Cells["pocet_ks_skladem"].Value.ToString();
                comboBoxPackaging.SelectedValue = Convert.ToInt32(dgvPiva.CurrentRow.Cells["baleni_piva_id_baleni"].Value);
                comboBoxUnit.SelectedValue = Convert.ToInt32(dgvPiva.CurrentRow.Cells["jednotka_piva_id_jedn"].Value);
            }
        }
    }
}
