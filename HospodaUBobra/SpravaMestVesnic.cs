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
    public partial class SpravaMestVesnic : Form
    {
        string connectionString = $"User Id=st69639;Password=Server2022;Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=fei-sql3.upceucebny.cz)(PORT=1521)))(CONNECT_DATA=(SID=BDAS)));";

        private int selectedMestoId = -1;

        public SpravaMestVesnic()
        {
            InitializeComponent();
            LoadMestaVesniceData();
            LoadKraje();
            LoadOkresy();
        }

        private void LoadMestaVesniceData()
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT * FROM MESTA_VESNICE";

                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        dgvMestaVesnice.DataSource = dataTable;
                        dgvMestaVesnice.Columns["ID_MES_VES"].HeaderText = "ID";
                        dgvMestaVesnice.Columns["NAZEV"].HeaderText = "Name";
                        dgvMestaVesnice.Columns["POCET_OBYVATEL"].HeaderText = "Population";
                        dgvMestaVesnice.Columns["PSC"].HeaderText = "Postal Code";
                        dgvMestaVesnice.Columns["OKRES_ID_OKRESU"].HeaderText = "District ID";
                        dgvMestaVesnice.Columns["KRAJ_ID_KRAJE"].HeaderText = "Region ID";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading data: " + ex.Message);
                }
            }
        }
        private void LoadKraje()
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT ID_KRAJE, NAZEV FROM KRAJE";

                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        List<Tuple<int, string>> kraje = new List<Tuple<int, string>>();
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            string name = reader.GetString(1);
                            kraje.Add(new Tuple<int, string>(id, name));
                        }

                        cbKraje.DataSource = kraje;
                        cbKraje.DisplayMember = "Item2"; // Display the name of the region
                        cbKraje.ValueMember = "Item1";  // Use the ID as the value
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading regions: " + ex.Message);
                }
            }
        }

        private void LoadOkresy()
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT ID_OKRESU, NAZEV FROM OKRESY";

                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        List<Tuple<int, string>> okresy = new List<Tuple<int, string>>();
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            string name = reader.GetString(1);
                            okresy.Add(new Tuple<int, string>(id, name));
                        }

                        cbOkresy.DataSource = okresy;
                        cbOkresy.DisplayMember = "Item2"; // Display the name of the district
                        cbOkresy.ValueMember = "Item1";  // Use the ID as the value
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading districts: " + ex.Message);
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNazev.Text) ||
            !int.TryParse(txtPocetObyvatel.Text, out int pocetObyvatel) ||
            string.IsNullOrWhiteSpace(txtPSC.Text) ||
            cbOkresy.SelectedItem == null ||
            cbKraje.SelectedItem == null)
            {
                MessageBox.Show("Please enter valid data for all fields.");
                return;
            }

            int okresId = ((Tuple<int, string>)cbOkresy.SelectedItem).Item1;
            int krajId = ((Tuple<int, string>)cbKraje.SelectedItem).Item1;

            ManageMestaVesnice(null, txtNazev.Text, pocetObyvatel, txtPSC.Text, okresId, krajId);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedMestoId == -1)
            {
                MessageBox.Show("Please select a record to update.");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtNazev.Text) ||
                !int.TryParse(txtPocetObyvatel.Text, out int pocetObyvatel) ||
                string.IsNullOrWhiteSpace(txtPSC.Text) ||
                cbOkresy.SelectedItem == null ||
                cbKraje.SelectedItem == null)
            {
                MessageBox.Show("Please enter valid data for all fields.");
                return;
            }

            int okresId = ((Tuple<int, string>)cbOkresy.SelectedItem).Item1;
            int krajId = ((Tuple<int, string>)cbKraje.SelectedItem).Item1;

            ManageMestaVesnice(selectedMestoId, txtNazev.Text, pocetObyvatel, txtPSC.Text, okresId, krajId);

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedMestoId == -1)
            {
                MessageBox.Show("Please select a record to delete.");
                return;
            }

            DialogResult result = MessageBox.Show("Are you sure you want to delete this record?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                using (OracleConnection conn = new OracleConnection(connectionString))
                {
                    try
                    {
                        conn.Open();
                        string query = "DELETE FROM MESTA_VESNICE WHERE ID_MES_VES = :id_mes_ves";

                        using (OracleCommand cmd = new OracleCommand(query, conn))
                        {
                            cmd.Parameters.Add(new OracleParameter(":id_mes_ves", OracleDbType.Int32)).Value = selectedMestoId;
                            cmd.ExecuteNonQuery();

                            MessageBox.Show("Record deleted successfully!");
                            LoadMestaVesniceData(); // Refresh data
                            ClearFormFields();
                            selectedMestoId = -1; // Reset selection
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error deleting record: " + ex.Message);
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
            ClearFormFields();
        }

        private void dgvMestaVesnice_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvMestaVesnice.CurrentRow != null)
            {
                selectedMestoId = Convert.ToInt32(dgvMestaVesnice.CurrentRow.Cells["ID_MES_VES"].Value);
                txtNazev.Text = dgvMestaVesnice.CurrentRow.Cells["NAZEV"].Value.ToString();
                txtPocetObyvatel.Text = dgvMestaVesnice.CurrentRow.Cells["POCET_OBYVATEL"].Value.ToString();
                txtPSC.Text = dgvMestaVesnice.CurrentRow.Cells["PSC"].Value.ToString();
                cbOkresy.SelectedValue = Convert.ToInt32(dgvMestaVesnice.CurrentRow.Cells["OKRES_ID_OKRESU"].Value);
                cbKraje.SelectedValue = Convert.ToInt32(dgvMestaVesnice.CurrentRow.Cells["KRAJ_ID_KRAJE"].Value);
            }
        }

        private void ManageMestaVesnice(int? idMesVes, string nazev, int pocetObyvatel, string psc, int okresId, int krajId)
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();
                using (OracleCommand cmd = new OracleCommand("sprava_mesta_vesnice", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new OracleParameter("p_identifikator", OracleDbType.Int32)).Value = idMesVes.HasValue ? idMesVes : DBNull.Value;
                    cmd.Parameters.Add(new OracleParameter("p_id_mes_ves", OracleDbType.Int32)).Value = idMesVes;
                    cmd.Parameters.Add(new OracleParameter("p_nazev", OracleDbType.Varchar2)).Value = nazev;
                    cmd.Parameters.Add(new OracleParameter("p_pocet_obyvatel", OracleDbType.Int32)).Value = pocetObyvatel;
                    cmd.Parameters.Add(new OracleParameter("p_psc", OracleDbType.Varchar2)).Value = psc;
                    cmd.Parameters.Add(new OracleParameter("p_okres_id_okresu", OracleDbType.Int32)).Value = okresId;
                    cmd.Parameters.Add(new OracleParameter("p_kraj_id_kraje", OracleDbType.Int32)).Value = krajId;

                    try
                    {
                        cmd.ExecuteNonQuery();
                        string message = idMesVes.HasValue ? "Record updated successfully!" : "Record added successfully!";
                        MessageBox.Show(message);
                        LoadMestaVesniceData(); // Refresh data
                        ClearFormFields(); // Clear form fields
                        selectedMestoId = -1; // Reset selection
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error managing record: " + ex.Message);
                    }
                }
            }
        }

        private void ClearFormFields()
        {
            txtNazev.Clear();
            txtPocetObyvatel.Clear();
            txtPSC.Clear();
            cbOkresy.SelectedIndex = -1;
            cbKraje.SelectedIndex = -1;
        }
    }
}