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

                    // Updated query to include joins with BALENI_PIV and JEDNOTKY_PIV
                    string query = @"
                SELECT 
                    p.ID_PIVA, 
                    p.NAZEV, 
                    p.OBSAH_ALKOHOLU, 
                    p.OBJEM, 
                    p.CENA, 
                    p.POCET_KS_SKLADEM, 
                    b.BALENI AS PACKAGING,
                    j.JEDNOTKA AS UNIT
                FROM 
                    PIVA p
                LEFT JOIN 
                    BALENI_PIV b ON p.BALENI_PIVA_ID_BALENI = b.ID_BALENI
                LEFT JOIN 
                    JEDNOTKY_PIV j ON p.JEDNOTKA_PIVA_ID_JEDN = j.ID_JEDN";

                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        dgvPiva.DataSource = dataTable;

                        // Rename column headers for clarity
                        dgvPiva.Columns["ID_PIVA"].HeaderText = "Beer ID";
                        dgvPiva.Columns["NAZEV"].HeaderText = "Name";
                        dgvPiva.Columns["OBSAH_ALKOHOLU"].HeaderText = "Alcohol Content (%)";
                        dgvPiva.Columns["OBJEM"].HeaderText = "Volume (ml)";
                        dgvPiva.Columns["CENA"].HeaderText = "Price (CZK)";
                        dgvPiva.Columns["POCET_KS_SKLADEM"].HeaderText = "Stock Quantity";
                        dgvPiva.Columns["PACKAGING"].HeaderText = "Packaging";
                        dgvPiva.Columns["UNIT"].HeaderText = "Unit";

                        // Hide the ID_PIVA column
                        if (dgvPiva.Columns.Contains("ID_PIVA"))
                        {
                            dgvPiva.Columns["ID_PIVA"].Visible = false;
                        }
                    }
                }
                catch (OracleException ex)
                {
                    MessageBox.Show("Chyba při získávání dat.: " + ex.Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Chyba: " + ex.Message);
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
                MessageBox.Show("Prosím zadejte správné hodnoty.");
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
                MessageBox.Show("Nebylo vybráno žádné pivo.");
                return;
            }

            string nazev = txtBeerName.Text.Trim();
            decimal obsahAlkoholu;
            decimal objem;
            decimal cena;
            int pocetKsSkladem;
            int baleniPivaId = ((Tuple<int, string>)comboBoxPackaging.SelectedItem).Item1;
            int jednotkaPivaId = ((Tuple<int, string>)comboBoxUnit.SelectedItem).Item1;

            if (string.IsNullOrEmpty(nazev) ||
                !decimal.TryParse(txtAlcoholContent.Text, out obsahAlkoholu) ||
                !decimal.TryParse(txtVolume.Text, out objem) ||
                !decimal.TryParse(txtPrice.Text, out cena) ||
                !int.TryParse(txtStockQuantity.Text, out pocetKsSkladem))
            {
                MessageBox.Show("Prosím zadejte správné hodnoty.");
                return;
            }

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
                        string message = idPiva.HasValue ? "Pivo úspěšně aktualizováno!" : "Pivo úspěšně přidáno!";
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
                            MessageBox.Show("Pivo odstraněno úspěšně!");
                            LoadPivaData(); 
                            selectedBeerId = -1; 
                            LoadPivaData();
                        }
                        else
                        {
                            MessageBox.Show("Pivo s tímto ID nebylo nalezeno.");
                        }
                    }
                }
                catch (OracleException ex)
                {
                    MessageBox.Show("Oracle chyba: " + ex.Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Chyba: " + ex.Message);
                }
            }
        }    

        private void dgvPiva_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvPiva.CurrentRow != null)
            {
                selectedBeerId = Convert.ToInt32(dgvPiva.CurrentRow.Cells["ID_PIVA"].Value);

                txtBeerName.Text = dgvPiva.CurrentRow.Cells["NAZEV"].Value.ToString();
                txtAlcoholContent.Text = dgvPiva.CurrentRow.Cells["OBSAH_ALKOHOLU"].Value.ToString();
                txtVolume.Text = dgvPiva.CurrentRow.Cells["OBJEM"].Value.ToString();
                txtPrice.Text = dgvPiva.CurrentRow.Cells["CENA"].Value.ToString();
                txtStockQuantity.Text = dgvPiva.CurrentRow.Cells["POCET_KS_SKLADEM"].Value.ToString();

                // Find and select the corresponding packaging in the ComboBox
                string packagingName = dgvPiva.CurrentRow.Cells["PACKAGING"].Value.ToString();
                comboBoxPackaging.SelectedIndex = comboBoxPackaging.FindStringExact(packagingName);

                // Find and select the corresponding unit in the ComboBox
                string unitName = dgvPiva.CurrentRow.Cells["UNIT"].Value.ToString();
                comboBoxUnit.SelectedIndex = comboBoxUnit.FindStringExact(unitName);
            }
        }
    }
}
