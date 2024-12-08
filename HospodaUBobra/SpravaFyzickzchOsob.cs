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
                    string query = "SELECT * FROM A_FO_VLASTNICI_VIEW";

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
                    MessageBox.Show($"Chyba při načítání vlastníků pivovarů: {ex.Message}", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                    string query = @"SELECT * FROM A_DGR_FYZICKE_OSOBY";

                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        DataGridViewFilterHelper.BindData(dgvFyzickeOsoby, dt);
                        dgvFyzickeOsoby.DataSource = dt;

                        dgvFyzickeOsoby.Columns["JMENO_A_PRIJMENI"].HeaderText = "Jméno a příjmení";
                        dgvFyzickeOsoby.Columns["DATUM_NAROZENI"].HeaderText = "Datum Narození";
                        dgvFyzickeOsoby.Columns["RODNE_CISLO"].HeaderText = "Rodné číslo";

                        if (dgvFyzickeOsoby.Columns.Contains("ID_FYZICKE_OSOBY"))
                        {
                            dgvFyzickeOsoby.Columns["ID_FYZICKE_OSOBY"].Visible = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Chyba při načítání fyzických osob: {ex.Message}", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    using (OracleCommand cmd = new OracleCommand("sprava_procedures_pkg.SPRAVA_FYZICKE_OSOBY", conn))
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
                    using (OracleCommand cmd = new OracleCommand("sprava_procedures_pkg.SPRAVA_FYZICKE_OSOBY", conn))
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
            int fyzickaOsobaId;

            if (dgvFyzickeOsoby.CurrentRow != null)
            {
                if (dgvFyzickeOsoby.CurrentRow.Cells["ID_FYZICKE_OSOBY"].Value != DBNull.Value)
                {
                    fyzickaOsobaId = Convert.ToInt32(dgvFyzickeOsoby.CurrentRow.Cells["ID_FYZICKE_OSOBY"].Value);
                    comboBoxVlastnikPivovaru.SelectedValue = fyzickaOsobaId;
                }
                else
                {
                    fyzickaOsobaId = -1; 
                    comboBoxVlastnikPivovaru.SelectedIndex = -1; 
                }

                if (dgvFyzickeOsoby.CurrentRow.Cells["DATUM_NAROZENI"].Value != DBNull.Value)
                {
                    dateTimePickerDatumNarozeni.Value = Convert.ToDateTime(dgvFyzickeOsoby.CurrentRow.Cells["DATUM_NAROZENI"].Value);
                }
                else
                {
                    dateTimePickerDatumNarozeni.Value = DateTime.Now; 
                }

                if (dgvFyzickeOsoby.CurrentRow.Cells["RODNE_CISLO"].Value != DBNull.Value)
                {
                    textBoxRodneCislo.Text = dgvFyzickeOsoby.CurrentRow.Cells["RODNE_CISLO"].Value.ToString();
                }
                else
                {
                    textBoxRodneCislo.Clear();
                }
            }
            else
            {
                fyzickaOsobaId = -1;
                comboBoxVlastnikPivovaru.SelectedIndex = -1;
                dateTimePickerDatumNarozeni.Value = DateTime.Now;
                textBoxRodneCislo.Clear();
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

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            DataGridViewFilterHelper.FilterData(dgvFyzickeOsoby, txtSearch);
        }
    }
}
