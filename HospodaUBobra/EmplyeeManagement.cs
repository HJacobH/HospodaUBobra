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
    public partial class EmplyeeManagement : Form
    {
        private string connectionString;
        private int selectedZamestnanecId;

        public EmplyeeManagement(string connectionString)
        {
            InitializeComponent();
            this.connectionString = connectionString;
            LoadEmployees();
            LoadPositions();

            dgvZamestnanci.ReadOnly = true;
            cbPozice.DropDownStyle = ComboBoxStyle.DropDownList;
            cbPoziceSalary.DropDownStyle = ComboBoxStyle.DropDownList;

            LoadPositionsSalary();
        }

        private void LoadEmployees()
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();

                string query = @"
                    SELECT * FROM A_DGV_ZAMESTNANCI";

                using (OracleCommand cmd = new OracleCommand(query, conn))
                {
                    using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt); 
                        DataGridViewFilterHelper.BindData(dgvZamestnanci, dt);
                        dgvZamestnanci.DataSource = dt;
                    }
                }
            }

            if (dgvZamestnanci.Columns.Contains("ID_ZAMESTNANCE"))
            {
                dgvZamestnanci.Columns["ID_ZAMESTNANCE"].Visible = false;
            }
        }


        private void LoadPositions()
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT ID_POZICE, NAZEV_POZICE FROM POZICE_PRACOVNIKA";

                using (OracleCommand cmd = new OracleCommand(query, conn))
                {
                    using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        cbPozice.DataSource = dt;
                        cbPozice.DisplayMember = "NAZEV_POZICE";
                        cbPozice.ValueMember = "ID_POZICE";
                        cbPozice.SelectedIndex = -1;
                    }
                }
            }
        }

        private void LoadPositionsSalary()
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT ID_POZICE, NAZEV_POZICE FROM POZICE_PRACOVNIKA";

                using (OracleCommand cmd = new OracleCommand(query, conn))
                {
                    using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        cbPoziceSalary.DataSource = dt;
                        cbPoziceSalary.DisplayMember = "NAZEV_POZICE";
                        cbPoziceSalary.ValueMember = "ID_POZICE";
                        cbPoziceSalary.SelectedIndex = -1;
                    }
                }
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedZamestnanecId <= 0)
            {
                MessageBox.Show("Vyberte zaměstnance ke smazání.");
                return;
            }

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();

                string query = "DELETE FROM ZAMESTNANCI WHERE ID_ZAMESTNANCE = :employeeId";

                using (OracleCommand cmd = new OracleCommand(query, conn))
                {
                    cmd.Parameters.Add("employeeId", OracleDbType.Int32).Value = selectedZamestnanecId;

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Zaměstnanec odstraněn úspěšně!");
                    LoadEmployees();
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedZamestnanecId <= 0)
            {
                MessageBox.Show("Vyberte zaměstnance k aktualizaci.");
                return;
            }

            if (!ValidateInputs(out string firstName, out string lastName, out DateTime dob, out decimal salary, out DateTime startDate, out string favBeer, out int positionId))
            {
                return;
            }

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();

                using (OracleCommand cmd = new OracleCommand("sprava_zamestnanci", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("p_identifikator", OracleDbType.Int32).Value = 1;
                    cmd.Parameters.Add("p_id_zamestnance", OracleDbType.Int32).Value = selectedZamestnanecId;
                    cmd.Parameters.Add("p_jmeno", OracleDbType.Varchar2).Value = firstName;
                    cmd.Parameters.Add("p_prijmeni", OracleDbType.Varchar2).Value = lastName;
                    cmd.Parameters.Add("p_datum_narozeni", OracleDbType.Date).Value = dob;
                    cmd.Parameters.Add("p_pozice", OracleDbType.Int32).Value = positionId;
                    cmd.Parameters.Add("p_plat", OracleDbType.Decimal).Value = salary;
                    cmd.Parameters.Add("p_datum_nastupu", OracleDbType.Date).Value = startDate;
                    cmd.Parameters.Add("p_oblibena_piva", OracleDbType.Clob).Value = favBeer;

                    try
                    {
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Zaměstnanec úspěšně aktualizován!");
                        LoadEmployees();
                    }
                    catch (OracleException ex)
                    {
                        MessageBox.Show("Error updating employee: " + ex.Message);
                    }
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs(out string firstName, out string lastName, out DateTime dob, out decimal salary, out DateTime startDate, out string favBeer, out int positionId))
            {
                return; 
            }

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();

                using (OracleCommand cmd = new OracleCommand("sprava_zamestnanci", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("p_identifikator", OracleDbType.Int32).Value = DBNull.Value;
                    cmd.Parameters.Add("p_id_zamestnance", OracleDbType.Int32).Value = GetNextEmployeeId();
                    cmd.Parameters.Add("p_jmeno", OracleDbType.Varchar2).Value = firstName;
                    cmd.Parameters.Add("p_prijmeni", OracleDbType.Varchar2).Value = lastName;
                    cmd.Parameters.Add("p_datum_narozeni", OracleDbType.Date).Value = dob;
                    cmd.Parameters.Add("p_pozice", OracleDbType.Int32).Value = positionId;
                    cmd.Parameters.Add("p_plat", OracleDbType.Decimal).Value = salary;
                    cmd.Parameters.Add("p_datum_nastupu", OracleDbType.Date).Value = startDate;
                    cmd.Parameters.Add("p_oblibena_piva", OracleDbType.Clob).Value = favBeer;

                    try
                    {
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Zaměstnanec úspěšně přidán!");
                        LoadEmployees();
                    }
                    catch (OracleException ex)
                    {
                        MessageBox.Show("Error adding employee: " + ex.Message);
                    }
                }
            }
        }

        private int GetNextEmployeeId()
        {
            int nextId = 0;

            try
            {
                using (OracleConnection conn = new OracleConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT NVL(MAX(ID_ZAMESTNANCE), 0) + 1 FROM ZAMESTNANCI";

                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    {
                        object result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            nextId = Convert.ToInt32(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chyba při získávání ID zaměstnance: " + ex.Message);
            }

            return nextId;
        }


        private void dgvZamestnanci_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvZamestnanci.CurrentRow != null)
            {
                selectedZamestnanecId = dgvZamestnanci.CurrentRow.Cells["ID_ZAMESTNANCE"].Value != DBNull.Value
                    ? Convert.ToInt32(dgvZamestnanci.CurrentRow.Cells["ID_ZAMESTNANCE"].Value)
                    : -1;

                txtJmeno.Text = dgvZamestnanci.CurrentRow.Cells["JMENO"].Value?.ToString() ?? string.Empty;
                txtPrijmeni.Text = dgvZamestnanci.CurrentRow.Cells["PRIJMENI"].Value?.ToString() ?? string.Empty;

                dateTimePickerNarozeni.Value = dgvZamestnanci.CurrentRow.Cells["DATUM_NAROZENI"].Value != DBNull.Value
                    ? Convert.ToDateTime(dgvZamestnanci.CurrentRow.Cells["DATUM_NAROZENI"].Value)
                    : DateTime.Now;

                txtVyplata.Text = dgvZamestnanci.CurrentRow.Cells["PLAT"].Value?.ToString() ?? "0";

                dateTimePickerStartWorking.Value = dgvZamestnanci.CurrentRow.Cells["DATUM_NASTUPU"].Value != DBNull.Value
                    ? Convert.ToDateTime(dgvZamestnanci.CurrentRow.Cells["DATUM_NASTUPU"].Value)
                    : DateTime.Now;

                txtFavBeer.Text = dgvZamestnanci.CurrentRow.Cells["OBLIBENA_PIVA"].Value?.ToString() ?? string.Empty;

                string positionName = dgvZamestnanci.CurrentRow.Cells["POZICE"].Value?.ToString() ?? string.Empty;
                cbPozice.SelectedIndex = cbPozice.FindStringExact(positionName);
            }
        }

        private void btnIncrease_Click(object sender, EventArgs e)
        {
            if (cbPoziceSalary.SelectedIndex == -1)
            {
                MessageBox.Show("Vyberte pozici před zvýšením platu.", "Žádná pozice nebyla vybrána", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int selectedPositionId = Convert.ToInt32(cbPoziceSalary.SelectedValue);

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    //FUNKCE 1 IncreasePayBasedOnTenureAndPosition

                    using (OracleCommand cmd = new OracleCommand("IncreasePayBasedOnTenureAndPosition", conn))
                    {
                        cmd.CommandType = CommandType.Text;

                        cmd.CommandText = $"SELECT IncreasePayBasedOnTenureAndPosition(:p_role_id) FROM DUAL";

                        OracleParameter roleIdParam = new OracleParameter("p_role_id", OracleDbType.Int32);
                        roleIdParam.Value = selectedPositionId;
                        cmd.Parameters.Add(roleIdParam);

                        int updatedCount = Convert.ToInt32(cmd.ExecuteScalar());

                        MessageBox.Show($"Výplata byla zvýšena pro {updatedCount} pracovníky v dané pozici.", "Úspěch", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadEmployees();
                    }
                }
                catch (OracleException ex)
                {
                    MessageBox.Show($"Database error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private bool ValidateInputs(out string firstName, out string lastName, out DateTime dob, out decimal salary, out DateTime startDate, out string favBeer, out int positionId)
        {
            firstName = txtJmeno.Text.Trim();
            lastName = txtPrijmeni.Text.Trim();
            favBeer = txtFavBeer.Text.Trim();
            dob = dateTimePickerNarozeni.Value;
            startDate = dateTimePickerStartWorking.Value;

            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
            {
                MessageBox.Show("Jméno a příjmení jsou povinná pole.", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                positionId = 0;
                salary = 0;
                return false;
            }

            if (!decimal.TryParse(txtVyplata.Text.Trim(), out salary) || salary <= 0)
            {
                MessageBox.Show("Špatný formát výplaty.", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                positionId = 0;
                return false;
            }

            positionId = -1;
            if (cbPozice.SelectedValue == null || !int.TryParse(cbPozice.SelectedValue.ToString(), out positionId))
            {
                MessageBox.Show("Vyberte platnou pozici.", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            DataGridViewFilterHelper.FilterData(dgvZamestnanci, txtSearch);
        }
    }
}
