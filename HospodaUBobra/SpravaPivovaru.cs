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
    public partial class SpravaPivovaru : Form
    {
        string connectionString = $"User Id=st69639;Password=Server2022;Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=fei-sql3.upceucebny.cz)(PORT=1521)))(CONNECT_DATA=(SID=BDAS)));";
        private int selectedBreweryId = -1;

        public SpravaPivovaru()
        {
            InitializeComponent();
            LoadBreweries();
            LoadDruhPodniku();
            LoadMestoVesnice();
            LoadYears();
            LoadYesNoOptions();
        }

        private void LoadBreweries()
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = @"
                SELECT 
                    P.ID_PIVOVARU, 
                    P.NAZEV, 
                    P.ROK_ZALOZENI, 
                    P.PROVOZ_PROHLIDEK, 
                    P.PROVOZ_AKCI, 
                    P.POPIS_AKCI, 
                    DP.DRUH_PODNIKU AS DRUH_PODNIKU, 
                    MV.NAZEV AS MESTO_VESNICE
                FROM PIVOVARY P
                LEFT JOIN DRUHY_PODNIKU DP ON P.DRUH_PODNIKU_ID_DRUHU = DP.ID_DRUHU
                LEFT JOIN MESTA_VESNICE MV ON P.MESTO_VESNICE_ID_MES_VES = MV.ID_MES_VES";

                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                    {
                        DataTable breweriesTable = new DataTable();
                        adapter.Fill(breweriesTable);

                        dgvBreweries.DataSource = breweriesTable;

                        dgvBreweries.Columns["ID_PIVOVARU"].HeaderText = "Brewery ID";
                        dgvBreweries.Columns["NAZEV"].HeaderText = "Name";
                        dgvBreweries.Columns["ROK_ZALOZENI"].HeaderText = "Year Established";
                        dgvBreweries.Columns["PROVOZ_PROHLIDEK"].HeaderText = "Tour Operation";
                        dgvBreweries.Columns["PROVOZ_AKCI"].HeaderText = "Event Operation";
                        dgvBreweries.Columns["POPIS_AKCI"].HeaderText = "Event Description";
                        dgvBreweries.Columns["DRUH_PODNIKU"].HeaderText = "Type";
                        dgvBreweries.Columns["MESTO_VESNICE"].HeaderText = "City/Village";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading breweries: " + ex.Message);
                }
            }
        }



        private void LoadDruhPodniku()
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT ID_DRUHU, DRUH_PODNIKU FROM DRUHY_PODNIKU";

                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        var druhPodniku = new List<Tuple<int, string>>();
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            string name = reader.GetString(1);
                            druhPodniku.Add(new Tuple<int, string>(id, name));
                        }

                        cbDruhPodniku.DataSource = druhPodniku;
                        cbDruhPodniku.DisplayMember = "Item2"; // Display DRUH_PODNIKU
                        cbDruhPodniku.ValueMember = "Item1";  // Use ID_DRUHU
                        cbDruhPodniku.SelectedIndex = -1; // Ensure no item is selected initially
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading types: " + ex.Message);
                }
            }
        }



        private void LoadMestoVesnice()
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT ID_MES_VES, NAZEV FROM MESTA_VESNICE";

                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        var mestoVesnice = new List<Tuple<int, string>>();
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            string name = reader.GetString(1);
                            mestoVesnice.Add(new Tuple<int, string>(id, name));
                        }

                        cbMestoVesnice.DataSource = mestoVesnice;
                        cbMestoVesnice.DisplayMember = "Item2"; // Display NAZEV
                        cbMestoVesnice.ValueMember = "Item1";  // Use ID_MES_VES
                        cbMestoVesnice.SelectedIndex = -1; // Ensure no item is selected initially
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading cities/villages: " + ex.Message);
                }
            }
        }



        private void LoadYears()
        {
            List<int> years = new List<int>();
            int currentYear = DateTime.Now.Year;
            for (int year = 1800; year <= currentYear; year++) // From 1800 to current year
            {
                years.Add(year);
            }
            cbRokZalozeni.DataSource = years; // Bind list to ComboBox
        }

        private void LoadYesNoOptions()
        {
            // Options for "Ano" (1) and "Ne" (2)
            var yesNoOptions = new List<Tuple<int, string>>()
            {
                new Tuple<int, string>(1, "Ano"),
                new Tuple<int, string>(2, "Ne")
            };

                    // Bind options to both "Provoz Prohlidek" and "Provoz Akci"
                    cbProvozProhlidek.DataSource = new List<Tuple<int, string>>(yesNoOptions);
                    cbProvozProhlidek.DisplayMember = "Item2"; // Display "Ano" or "Ne"
                    cbProvozProhlidek.ValueMember = "Item1";  // Use 1 or 2 as the value

                    cbProvozAkci.DataSource = new List<Tuple<int, string>>(yesNoOptions);
                    cbProvozAkci.DisplayMember = "Item2"; // Display "Ano" or "Ne"
                    cbProvozAkci.ValueMember = "Item1";  // Use 1 or 2 as the value
                }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            int druhPodnikuId = ((Tuple<int, string>)cbDruhPodniku.SelectedItem).Item1;
            int mestoVesniceId = ((Tuple<int, string>)cbMestoVesnice.SelectedItem).Item1;
            int rokZalozeni = (int)cbRokZalozeni.SelectedItem; // Get year from ComboBox
            int provozProhlidek = ((Tuple<int, string>)cbProvozProhlidek.SelectedItem).Item1; // Get "Ano" or "Ne"
            int provozAkci = ((Tuple<int, string>)cbProvozAkci.SelectedItem).Item1; // Get "Ano" or "Ne"

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();
                using (OracleCommand cmd = new OracleCommand("sprava_pivovaru", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new OracleParameter("p_identifikator", OracleDbType.Int32)).Value = DBNull.Value;
                    cmd.Parameters.Add(new OracleParameter("p_id_pivovaru", OracleDbType.Int32)).Value = DBNull.Value;
                    cmd.Parameters.Add(new OracleParameter("p_nazev", OracleDbType.Varchar2)).Value = txtNazev.Text.Trim();
                    cmd.Parameters.Add(new OracleParameter("p_rok_zalozeni", OracleDbType.Int32)).Value = rokZalozeni;
                    cmd.Parameters.Add(new OracleParameter("p_provoz_prohlidek", OracleDbType.Int32)).Value = provozProhlidek;
                    cmd.Parameters.Add(new OracleParameter("p_provoz_akci", OracleDbType.Int32)).Value = provozAkci;
                    cmd.Parameters.Add(new OracleParameter("p_popis_akci", OracleDbType.Clob)).Value = txtPopisAkci.Text.Trim();
                    cmd.Parameters.Add(new OracleParameter("p_druh_podniku_id", OracleDbType.Int32)).Value = druhPodnikuId;
                    cmd.Parameters.Add(new OracleParameter("p_mesto_vesnice_id", OracleDbType.Int32)).Value = mestoVesniceId;

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Brewery added successfully!");
                    LoadBreweries();
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedBreweryId == -1)
            {
                MessageBox.Show("Please select a brewery to update.");
                return;
            }

            int druhPodnikuId = ((Tuple<int, string>)cbDruhPodniku.SelectedItem).Item1;
            int mestoVesniceId = ((Tuple<int, string>)cbMestoVesnice.SelectedItem).Item1;
            int rokZalozeni = (int)cbRokZalozeni.SelectedItem; // Get year from ComboBox
            int provozProhlidek = ((Tuple<int, string>)cbProvozProhlidek.SelectedItem).Item1; // Get "Ano" or "Ne"
            int provozAkci = ((Tuple<int, string>)cbProvozAkci.SelectedItem).Item1; // Get "Ano" or "Ne"

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();
                using (OracleCommand cmd = new OracleCommand("sprava_pivovaru", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new OracleParameter("p_identifikator", OracleDbType.Int32)).Value = selectedBreweryId;
                    cmd.Parameters.Add(new OracleParameter("p_id_pivovaru", OracleDbType.Int32)).Value = selectedBreweryId;
                    cmd.Parameters.Add(new OracleParameter("p_nazev", OracleDbType.Varchar2)).Value = txtNazev.Text.Trim();
                    cmd.Parameters.Add(new OracleParameter("p_rok_zalozeni", OracleDbType.Int32)).Value = rokZalozeni;
                    cmd.Parameters.Add(new OracleParameter("p_provoz_prohlidek", OracleDbType.Int32)).Value = provozProhlidek;
                    cmd.Parameters.Add(new OracleParameter("p_provoz_akci", OracleDbType.Int32)).Value = provozAkci;
                    cmd.Parameters.Add(new OracleParameter("p_popis_akci", OracleDbType.Clob)).Value = txtPopisAkci.Text.Trim();
                    cmd.Parameters.Add(new OracleParameter("p_druh_podniku_id", OracleDbType.Int32)).Value = druhPodnikuId;
                    cmd.Parameters.Add(new OracleParameter("p_mesto_vesnice_id", OracleDbType.Int32)).Value = mestoVesniceId;

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Brewery updated successfully!");
                    LoadBreweries();
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedBreweryId == -1)
            {
                MessageBox.Show("Please select a brewery to delete.");
                return;
            }

            DialogResult result = MessageBox.Show("Are you sure you want to delete this brewery?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                using (OracleConnection conn = new OracleConnection(connectionString))
                {
                    conn.Open();
                    string query = "DELETE FROM PIVOVARY WHERE ID_PIVOVARU = :id_pivovaru";

                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    {
                        cmd.Parameters.Add(new OracleParameter(":id_pivovaru", OracleDbType.Int32)).Value = selectedBreweryId;
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Brewery deleted successfully!");
                        LoadBreweries();
                    }
                }
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtNazev.Clear();
            txtPopisAkci.Clear();

            // Reset combo boxes
            cbRokZalozeni.SelectedIndex = -1;  // Clear "Rok Zalozeni"
            cbProvozProhlidek.SelectedIndex = -1;  // Clear "Provoz Prohlidek"
            cbProvozAkci.SelectedIndex = -1;  // Clear "Provoz Akci"
            cbDruhPodniku.SelectedIndex = -1;  // Clear "Druh Podniku"
            cbMestoVesnice.SelectedIndex = -1;  // Clear "Mesto/Vesnice"

            // Reset the selected brewery ID
            selectedBreweryId = -1;
        }

        private void dgvBreweries_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvBreweries.CurrentRow != null)
            {
                selectedBreweryId = Convert.ToInt32(dgvBreweries.CurrentRow.Cells["ID_PIVOVARU"].Value);
                txtNazev.Text = dgvBreweries.CurrentRow.Cells["NAZEV"].Value.ToString();

                // Set year in the "Rok Zalozeni" combo box
                int selectedYear = Convert.ToInt32(dgvBreweries.CurrentRow.Cells["ROK_ZALOZENI"].Value);
                cbRokZalozeni.SelectedItem = selectedYear;

                // Set "Ano"/"Ne" values for "Provoz Prohlidek"
                int prohlidekValue = dgvBreweries.CurrentRow.Cells["PROVOZ_PROHLIDEK"].Value == DBNull.Value ? 2 : Convert.ToInt32(dgvBreweries.CurrentRow.Cells["PROVOZ_PROHLIDEK"].Value);
                cbProvozProhlidek.SelectedValue = prohlidekValue;

                // Set "Ano"/"Ne" values for "Provoz Akci"
                int akciValue = dgvBreweries.CurrentRow.Cells["PROVOZ_AKCI"].Value == DBNull.Value ? 2 : Convert.ToInt32(dgvBreweries.CurrentRow.Cells["PROVOZ_AKCI"].Value);
                cbProvozAkci.SelectedValue = akciValue;

                txtPopisAkci.Text = dgvBreweries.CurrentRow.Cells["POPIS_AKCI"].Value?.ToString();

                // Set "Druh Podniku" combo box
                string druhPodnikuName = dgvBreweries.CurrentRow.Cells["DRUH_PODNIKU"].Value.ToString();
                var druhPodnikuItem = ((List<Tuple<int, string>>)cbDruhPodniku.DataSource)
                    .FirstOrDefault(item => item.Item2 == druhPodnikuName);
                if (druhPodnikuItem != null)
                {
                    cbDruhPodniku.SelectedValue = druhPodnikuItem.Item1;
                }

                // Set "Mesto/Vesnice" combo box
                string mestoVesniceName = dgvBreweries.CurrentRow.Cells["MESTO_VESNICE"].Value.ToString();
                var mestoVesniceItem = ((List<Tuple<int, string>>)cbMestoVesnice.DataSource)
                    .FirstOrDefault(item => item.Item2 == mestoVesniceName);
                if (mestoVesniceItem != null)
                {
                    cbMestoVesnice.SelectedValue = mestoVesniceItem.Item1;
                }
            }
        }
    }
}
