﻿using Oracle.ManagedDataAccess.Client;
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

            dgvBreweries.ReadOnly = true;
            cbDruhPodniku.DropDownStyle = ComboBoxStyle.DropDownList;
            cbMestoVesnice.DropDownStyle = ComboBoxStyle.DropDownList;
            cbProvozAkci.DropDownStyle = ComboBoxStyle.DropDownList;
            cbProvozProhlidek.DropDownStyle = ComboBoxStyle.DropDownList;
            cbRokZalozeni.DropDownStyle = ComboBoxStyle.DropDownList;
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
                CASE 
                    WHEN P.PROVOZ_PROHLIDEK = 1 THEN 'Ano' 
                    WHEN P.PROVOZ_PROHLIDEK = 2 THEN 'Ne' 
                    ELSE 'Neznámé'
                END AS PROVOZ_PROHLIDEK, 
                CASE 
                    WHEN P.PROVOZ_AKCI = 1 THEN 'Ano' 
                    WHEN P.PROVOZ_AKCI = 2 THEN 'Ne' 
                    ELSE 'Neznámé'
                END AS PROVOZ_AKCI, 
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

                        dgvBreweries.Columns["ID_PIVOVARU"].Visible = false;
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
                        cbDruhPodniku.DisplayMember = "Item2";
                        cbDruhPodniku.ValueMember = "Item1";
                        cbDruhPodniku.SelectedIndex = -1;
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
                        cbMestoVesnice.DisplayMember = "Item2";
                        cbMestoVesnice.ValueMember = "Item1";  
                        cbMestoVesnice.SelectedIndex = -1;
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
            for (int year = 1800; year <= currentYear; year++)
            {
                years.Add(year);
            }
            cbRokZalozeni.DataSource = years;
        }

        private void LoadYesNoOptions()
        {
            var yesNoOptions = new List<Tuple<int, string>>()
            {
                new Tuple<int, string>(1, "Ano"),
                new Tuple<int, string>(2, "Ne")
            };

            cbProvozProhlidek.DataSource = new List<Tuple<int, string>>(yesNoOptions);
            cbProvozProhlidek.DisplayMember = "Item2";
            cbProvozProhlidek.ValueMember = "Item1";

            cbProvozAkci.DataSource = new List<Tuple<int, string>>(yesNoOptions);
            cbProvozAkci.DisplayMember = "Item2";
            cbProvozAkci.ValueMember = "Item1";
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs())
                return;

            int druhPodnikuId = ((Tuple<int, string>)cbDruhPodniku.SelectedItem).Item1;
            int mestoVesniceId = ((Tuple<int, string>)cbMestoVesnice.SelectedItem).Item1;
            int rokZalozeni = (int)cbRokZalozeni.SelectedItem;
            int provozProhlidek = ((Tuple<int, string>)cbProvozProhlidek.SelectedItem).Item1;
            int provozAkci = ((Tuple<int, string>)cbProvozAkci.SelectedItem).Item1;

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
                    MessageBox.Show("Pivovar byl úspěšně přidán!");
                    LoadBreweries();
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedBreweryId == -1)
            {
                MessageBox.Show("Vyberte pivovar pro aktualizaci.", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidateInputs())
                return;

            int druhPodnikuId = ((Tuple<int, string>)cbDruhPodniku.SelectedItem).Item1;
            int mestoVesniceId = ((Tuple<int, string>)cbMestoVesnice.SelectedItem).Item1;
            int rokZalozeni = (int)cbRokZalozeni.SelectedItem;
            int provozProhlidek = ((Tuple<int, string>)cbProvozProhlidek.SelectedItem).Item1;
            int provozAkci = ((Tuple<int, string>)cbProvozAkci.SelectedItem).Item1;

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
                    MessageBox.Show("Pivovar byl úspěšně aktualizován!");
                    LoadBreweries();
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedBreweryId == -1)
            {
                MessageBox.Show("Vyberte pivovar pro odstranění.");
                return;
            }

            DialogResult result = MessageBox.Show("Opravdu chcete odstranit tento pivovar?", "Potvrdit odstranění", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
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

                        MessageBox.Show("Pivovar úspěšně odstraněn!");
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

            cbRokZalozeni.SelectedIndex = -1;
            cbProvozProhlidek.SelectedIndex = -1; 
            cbProvozAkci.SelectedIndex = -1;
            cbDruhPodniku.SelectedIndex = -1;
            cbMestoVesnice.SelectedIndex = -1;

            selectedBreweryId = -1;
        }

        private void dgvBreweries_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvBreweries.CurrentRow != null)
            {
                selectedBreweryId = Convert.ToInt32(dgvBreweries.CurrentRow.Cells["ID_PIVOVARU"].Value);

                txtNazev.Text = dgvBreweries.CurrentRow.Cells["NAZEV"].Value.ToString();

                int selectedYear = Convert.ToInt32(dgvBreweries.CurrentRow.Cells["ROK_ZALOZENI"].Value);
                cbRokZalozeni.SelectedItem = selectedYear;

                string prohlidekValue = dgvBreweries.CurrentRow.Cells["PROVOZ_PROHLIDEK"].Value.ToString();
                cbProvozProhlidek.SelectedValue = prohlidekValue == "Ano" ? 1 : 2;

                string akciValue = dgvBreweries.CurrentRow.Cells["PROVOZ_AKCI"].Value.ToString();
                cbProvozAkci.SelectedValue = akciValue == "Ano" ? 1 : 2;

                txtPopisAkci.Text = dgvBreweries.CurrentRow.Cells["POPIS_AKCI"].Value?.ToString();

                string druhPodnikuName = dgvBreweries.CurrentRow.Cells["DRUH_PODNIKU"].Value.ToString();
                var druhPodnikuItem = ((List<Tuple<int, string>>)cbDruhPodniku.DataSource)
                    .FirstOrDefault(item => item.Item2 == druhPodnikuName);
                if (druhPodnikuItem != null)
                {
                    cbDruhPodniku.SelectedValue = druhPodnikuItem.Item1;
                }

                string mestoVesniceName = dgvBreweries.CurrentRow.Cells["MESTO_VESNICE"].Value.ToString();
                var mestoVesniceItem = ((List<Tuple<int, string>>)cbMestoVesnice.DataSource)
                    .FirstOrDefault(item => item.Item2 == mestoVesniceName);
                if (mestoVesniceItem != null)
                {
                    cbMestoVesnice.SelectedValue = mestoVesniceItem.Item1;
                }
            }
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtNazev.Text))
            {
                MessageBox.Show("Zadejte název pivovaru.", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (cbRokZalozeni.SelectedIndex == -1)
            {
                MessageBox.Show("Vyberte rok založení pivovaru.", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (cbProvozProhlidek.SelectedIndex == -1)
            {
                MessageBox.Show("Vyberte možnost, zda pivovar provozuje prohlídky.", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (cbProvozAkci.SelectedIndex == -1)
            {
                MessageBox.Show("Vyberte možnost, zda pivovar provozuje akce.", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (cbDruhPodniku.SelectedIndex == -1)
            {
                MessageBox.Show("Vyberte druh podniku.", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (cbMestoVesnice.SelectedIndex == -1)
            {
                MessageBox.Show("Vyberte město nebo vesnici.", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }
    }
}
