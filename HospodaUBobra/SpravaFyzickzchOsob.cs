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
    public partial class SpravaFyzickzchOsob : Form
    {
        private string connectionString = $"User Id=st69639;Password=Server2022;Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=fei-sql3.upceucebny.cz)(PORT=1521)))(CONNECT_DATA=(SID=BDAS)));";

        public SpravaFyzickzchOsob()
        {
            InitializeComponent();
            LoadVlastniciPivovaru();
            LoadFyzickeOsoby();

            dgvFyzickeOsoby.ReadOnly = true;
            comboBoxVlastnikPivovaru.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void LoadVlastniciPivovaru()
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT ID_VLASTNIKA, JMENO_NAZEV FROM VLASTNICI_PIVOVARU WHERE DRUH_VLASTNIKA = 'FO'";

                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    {
                        using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            comboBoxVlastnikPivovaru.DataSource = dt;
                            comboBoxVlastnikPivovaru.DisplayMember = "JMENO_NAZEV";
                            comboBoxVlastnikPivovaru.ValueMember = "ID_VLASTNIKA";
                            comboBoxVlastnikPivovaru.SelectedIndex = -1;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading VLASTNICI_PIVOVARU: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void LoadFyzickeOsoby()
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    string query = @"
                SELECT 
                    f.ID_FYZICKE_OSOBY,
                    v.JMENO_NAZEV AS JMENO_A_PRIJMENI,
                    f.DATUM_NAROZENI,
                    f.RODNE_CISLO
                FROM 
                    FYZICKE_OSOBY f
                LEFT JOIN 
                    VLASTNICI_PIVOVARU v ON f.ID = v.ID_VLASTNIKA";

                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        dgvFyzickeOsoby.DataSource = dt;

                        dgvFyzickeOsoby.Columns["JMENO_A_PRIJMENI"].HeaderText = "Name and Surname";
                        dgvFyzickeOsoby.Columns["DATUM_NAROZENI"].HeaderText = "Date of Birth";
                        dgvFyzickeOsoby.Columns["RODNE_CISLO"].HeaderText = "Personal ID";

                        if (dgvFyzickeOsoby.Columns.Contains("ID_FYZICKE_OSOBY"))
                        {
                            dgvFyzickeOsoby.Columns["ID_FYZICKE_OSOBY"].Visible = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading FYZICKE_OSOBY: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs(out string errorMessage))
            {
                MessageBox.Show(errorMessage, "Chyba ověření", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (OracleCommand cmd = new OracleCommand("SPRAVA_FYZICKE_OSOBY", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("p_identifikator", OracleDbType.Int32).Value = DBNull.Value;
                        cmd.Parameters.Add("p_id_fyzicke_osoby", OracleDbType.Int32).Value = DBNull.Value;
                        cmd.Parameters.Add("p_id", OracleDbType.Int32).Value = comboBoxVlastnikPivovaru.SelectedValue;
                        cmd.Parameters.Add("p_datum_narozeni", OracleDbType.Date).Value = dateTimePickerDatumNarozeni.Value;
                        cmd.Parameters.Add("p_rodne_cislo", OracleDbType.Varchar2).Value = textBoxRodneCislo.Text;

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Nová fyzická osoba byla přidána.", "Úspěch", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadFyzickeOsoby();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error adding record: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvFyzickeOsoby.CurrentRow == null)
            {
                MessageBox.Show("Vyberte řádek pro aktualizaci.", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidateInputs(out string errorMessage))
            {
                MessageBox.Show(errorMessage, "Chyba ověření", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (OracleCommand cmd = new OracleCommand("SPRAVA_FYZICKE_OSOBY", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("p_identifikator", OracleDbType.Int32).Value = 1;
                        cmd.Parameters.Add("p_id_fyzicke_osoby", OracleDbType.Int32).Value = dgvFyzickeOsoby.CurrentRow.Cells["ID_FYZICKE_OSOBY"].Value;
                        cmd.Parameters.Add("p_id", OracleDbType.Int32).Value = comboBoxVlastnikPivovaru.SelectedValue;
                        cmd.Parameters.Add("p_datum_narozeni", OracleDbType.Date).Value = dateTimePickerDatumNarozeni.Value;
                        cmd.Parameters.Add("p_rodne_cislo", OracleDbType.Varchar2).Value = textBoxRodneCislo.Text;

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Fyzická osoba byla aktualizována.", "Úspěch", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadFyzickeOsoby(); 
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error updating record: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvFyzickeOsoby.CurrentRow == null)
            {
                MessageBox.Show("Vyberte řádek pro odstranění.", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult confirmResult = MessageBox.Show(
                "Opravdu chcete odstranit tuto fyzickou osobu?",
                "Potvrzení odstranění",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmResult != DialogResult.Yes)
                return;

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (OracleCommand cmd = new OracleCommand("DELETE FROM FYZICKE_OSOBY WHERE ID_FYZICKE_OSOBY = :id_fyzicke_osoby", conn))
                    {
                        cmd.Parameters.Add(":id_fyzicke_osoby", OracleDbType.Int32).Value = dgvFyzickeOsoby.CurrentRow.Cells["ID_FYZICKE_OSOBY"].Value;

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Fyzická osoba byla odstraněna.", "Úspěch", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadFyzickeOsoby(); 
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting record: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvFyzickeOsoby_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvFyzickeOsoby.CurrentRow != null)
            {
                int fyzickaOsobaId = Convert.ToInt32(dgvFyzickeOsoby.CurrentRow.Cells["ID_FYZICKE_OSOBY"].Value);

                comboBoxVlastnikPivovaru.SelectedValue = fyzickaOsobaId;
                dateTimePickerDatumNarozeni.Value = Convert.ToDateTime(dgvFyzickeOsoby.CurrentRow.Cells["DATUM_NAROZENI"].Value);
                textBoxRodneCislo.Text = dgvFyzickeOsoby.CurrentRow.Cells["RODNE_CISLO"].Value.ToString();
            }
        }

        private bool ValidateInputs(out string errorMessage)
        {
            errorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(textBoxRodneCislo.Text))
            {
                errorMessage = "Prosím, zadejte rodné číslo.";
                return false;
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(textBoxRodneCislo.Text, @"^\d{6}/\d{4}$"))
            {
                errorMessage = "Rodné číslo musí mít formát YYMMDD/XXXX.";
                return false;
            }

            if (dateTimePickerDatumNarozeni.Value > DateTime.Now)
            {
                errorMessage = "Datum narození nemůže být v budoucnosti.";
                return false;
            }

            if (comboBoxVlastnikPivovaru.SelectedItem == null)
            {
                errorMessage = "Vyberte vlastníka pivovaru.";
                return false;
            }

            return true;
        }
    }
}
