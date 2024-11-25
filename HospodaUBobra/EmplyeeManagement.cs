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
            string position = txtPozice.Text;
            decimal salary;
            if (!decimal.TryParse(txtVyplata.Text, out salary))
            {
                MessageBox.Show("Špatný formát výplaty.");
                return;
            }
            DateTime startDate = dateTimePickerStartWorking.Value;
            string favBeer = txtFavBeer.Text;

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();

                string query = @"
            UPDATE ZAMESTNANCI 
            SET JMENO = :firstName, 
                PRIJMENI = :lastName, 
                DATUM_NAROZENI = :dob, 
                POZICE = :position, 
                PLAT = :salary, 
                DATUM_NASTUPU = :startDate, 
                OBLIBENA_PIVA = :favBeer 
            WHERE ID_ZAMESTNANCE = :employeeId";

                using (OracleCommand cmd = new OracleCommand(query, conn))
                {
                    cmd.Parameters.Add("firstName", OracleDbType.Varchar2).Value = firstName;
                    cmd.Parameters.Add("lastName", OracleDbType.Varchar2).Value = lastName;
                    cmd.Parameters.Add("dob", OracleDbType.Date).Value = dob;
                    cmd.Parameters.Add("position", OracleDbType.Varchar2).Value = position;
                    cmd.Parameters.Add("salary", OracleDbType.Decimal).Value = salary;
                    cmd.Parameters.Add("startDate", OracleDbType.Date).Value = startDate;
                    cmd.Parameters.Add("favBeer", OracleDbType.Varchar2).Value = favBeer;
                    cmd.Parameters.Add("employeeId", OracleDbType.Int32).Value = selectedZamestnanecId;

                    try
                    {
                        cmd.ExecuteNonQuery();
                        
                        MessageBox.Show("Zaměstnanec aktualizován úspěšně!");
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
            string position = txtPozice.Text;
            decimal salary;
            if (!decimal.TryParse(txtVyplata.Text, out salary))
            {
                MessageBox.Show("Špatný formát výplaty.");
                return;
            }
            DateTime startDate = dateTimePickerStartWorking.Value;
            string favBeer = txtFavBeer.Text;

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();

                string query = "INSERT INTO ZAMESTNANCI (JMENO, PRIJMENI, DATUM_NAROZENI, POZICE, PLAT, DATUM_NASTUPU, OBLIBENA_PIVA) " +
                               "VALUES (:firstName, :lastName, :dob, :position, :salary, :startDate, :favBeer)";

                using (OracleCommand cmd = new OracleCommand(query, conn))
                {
                    cmd.Parameters.Add("firstName", OracleDbType.Varchar2).Value = firstName;
                    cmd.Parameters.Add("lastName", OracleDbType.Varchar2).Value = lastName;
                    cmd.Parameters.Add("dob", OracleDbType.Date).Value = dob;
                    cmd.Parameters.Add("position", OracleDbType.Varchar2).Value = position;
                    cmd.Parameters.Add("salary", OracleDbType.Decimal).Value = salary;
                    cmd.Parameters.Add("startDate", OracleDbType.Date).Value = startDate;
                    cmd.Parameters.Add("favBeer", OracleDbType.Varchar2).Value = favBeer;

                    cmd.ExecuteNonQuery();
                    
                    MessageBox.Show("Zaměstnanec úspěšně přidán!");
                    LoadEmployees();
                }
            }
        }

        private void dgvZamestnanci_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvZamestnanci.CurrentRow != null)
            {
                selectedZamestnanecId = Convert.ToInt32(dgvZamestnanci.CurrentRow.Cells["id_zamestnance"].Value);
                txtJmeno.Text = dgvZamestnanci.CurrentRow.Cells["JMENO"].Value.ToString();
                txtPrijmeni.Text = dgvZamestnanci.CurrentRow.Cells["PRIJMENI"].Value.ToString();
                dateTimePickerNarozeni.Value = Convert.ToDateTime(dgvZamestnanci.CurrentRow.Cells["DATUM_NAROZENI"].Value);
                txtPozice.Text = dgvZamestnanci.CurrentRow.Cells["POZICE"].Value.ToString();
                txtVyplata.Text = dgvZamestnanci.CurrentRow.Cells["PLAT"].Value.ToString();
                dateTimePickerStartWorking.Value = Convert.ToDateTime(dgvZamestnanci.CurrentRow.Cells["DATUM_NASTUPU"].Value);
                txtFavBeer.Text = dgvZamestnanci.CurrentRow.Cells["OBLIBENA_PIVA"].Value.ToString();
            }
        }
    }
}
