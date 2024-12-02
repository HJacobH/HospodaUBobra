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

            LoadPositionsSalary();
        }

        private void LoadEmployees()
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM ZAMESTNANCI";

                using (OracleCommand cmd = new OracleCommand(query, conn))
                {
                    using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dgvZamestnanci.DataSource = dt;
                    }
                }
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
                        cbPozice.SelectedIndex = -1; // Clear selection
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
                        cbPoziceSalary.SelectedIndex = -1; // Clear selection
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

            string firstName = txtJmeno.Text;
            string lastName = txtPrijmeni.Text;
            DateTime dob = dateTimePickerNarozeni.Value;
            decimal salary;
            if (!decimal.TryParse(txtVyplata.Text, out salary))
            {
                MessageBox.Show("Špatný formát výplaty.");
                return;
            }
            DateTime startDate = dateTimePickerStartWorking.Value;
            string favBeer = txtFavBeer.Text;
            int positionId = Convert.ToInt32(cbPozice.SelectedValue);

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();

                using (OracleCommand cmd = new OracleCommand("sprava_zamestnanci", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("p_identifikator", OracleDbType.Int32).Value = 1; // Non-NULL for update
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
            string firstName = txtJmeno.Text;
            string lastName = txtPrijmeni.Text;
            DateTime dob = dateTimePickerNarozeni.Value;
            decimal salary;
            if (!decimal.TryParse(txtVyplata.Text, out salary))
            {
                MessageBox.Show("Špatný formát výplaty.");
                return;
            }
            DateTime startDate = dateTimePickerStartWorking.Value;
            string favBeer = txtFavBeer.Text;
            int positionId = Convert.ToInt32(cbPozice.SelectedValue);

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();

                using (OracleCommand cmd = new OracleCommand("sprava_zamestnanci", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("p_identifikator", OracleDbType.Int32).Value = DBNull.Value; // NULL for insert
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
                selectedZamestnanecId = Convert.ToInt32(dgvZamestnanci.CurrentRow.Cells["id_zamestnance"].Value);
                txtJmeno.Text = dgvZamestnanci.CurrentRow.Cells["JMENO"].Value.ToString();
                txtPrijmeni.Text = dgvZamestnanci.CurrentRow.Cells["PRIJMENI"].Value.ToString();
                dateTimePickerNarozeni.Value = Convert.ToDateTime(dgvZamestnanci.CurrentRow.Cells["DATUM_NAROZENI"].Value);
                cbPozice.SelectedValue = dgvZamestnanci.CurrentRow.Cells["POZICE"].Value;
                txtVyplata.Text = dgvZamestnanci.CurrentRow.Cells["PLAT"].Value.ToString();
                dateTimePickerStartWorking.Value = Convert.ToDateTime(dgvZamestnanci.CurrentRow.Cells["DATUM_NASTUPU"].Value);
                txtFavBeer.Text = dgvZamestnanci.CurrentRow.Cells["OBLIBENA_PIVA"].Value.ToString();
            }
        }

        private void btnIncrease_Click(object sender, EventArgs e)
        {
            if (cbPoziceSalary.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a position before updating salaries.", "No Position Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Get the selected position ID from the ComboBox
            int selectedPositionId = Convert.ToInt32(cbPoziceSalary.SelectedValue);

            // Execute the procedure with the selected position ID
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    using (OracleCommand cmd = new OracleCommand("IncreasePayBasedOnTenureAndPosition", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Add the position ID parameter
                        OracleParameter roleIdParam = new OracleParameter("p_role_id", OracleDbType.Int32);
                        roleIdParam.Value = selectedPositionId;
                        cmd.Parameters.Add(roleIdParam);

                        // Execute the procedure
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Salary update completed successfully for the selected position.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadEmployees();
                    }
                }
                catch (OracleException ex)
                {
                    // Handle Oracle exceptions
                    MessageBox.Show($"Database error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    // Handle general exceptions
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
