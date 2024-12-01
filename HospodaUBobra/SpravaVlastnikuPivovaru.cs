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
    public partial class SpravaVlastnikuPivovaru : Form
    {
        string connectionString = $"User Id=st69639;Password=Server2022;Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=fei-sql3.upceucebny.cz)(PORT=1521)))(CONNECT_DATA=(SID=BDAS)));";
        private int selectedOwnerId = -1;

        public SpravaVlastnikuPivovaru()
        {
            InitializeComponent();
            LoadOwners();
            LoadMestoVesnice();
            LoadDruhVlastnika();

            dgvOwners.AutoGenerateColumns = true;
        }

        private void LoadDruhVlastnika()
        {
            var druhVlastnika = new List<Tuple<string, string>>()
    {
        new Tuple<string, string>("FO", "Physical Person"),
        new Tuple<string, string>("PO", "Legal Person")
    };

            cbDruhVlastnika.DataSource = druhVlastnika;
            cbDruhVlastnika.DisplayMember = "Item2"; // Display FO/PO
            cbDruhVlastnika.ValueMember = "Item1";  // Use FO/PO as the actual value
            cbDruhVlastnika.SelectedIndex = -1;
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

        private void LoadOwners()
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = @"
                SELECT 
                    V.ID_VLASTNIKA, 
                    V.JMENO_NAZEV, 
                    V.PRIJMENI, 
                    V.ULICE, 
                    V.CISLO_POPISNE, 
                    V.ICO, 
                    V.DIC, 
                    MV.NAZEV AS MESTO_VESNICE, 
                    V.DRUH_VLASTNIKA
                FROM VLASTNICI_PIVOVARU V
                LEFT JOIN MESTA_VESNICE MV ON V.MESTO_VESNICE_ID_MES_VES = MV.ID_MES_VES";

                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                    {
                        DataTable ownersTable = new DataTable();
                        adapter.Fill(ownersTable);

                        dgvOwners.DataSource = ownersTable;

                        dgvOwners.Columns["ID_VLASTNIKA"].HeaderText = "Owner ID";
                        dgvOwners.Columns["JMENO_NAZEV"].HeaderText = "Name/Company";
                        dgvOwners.Columns["PRIJMENI"].HeaderText = "Surname";
                        dgvOwners.Columns["ULICE"].HeaderText = "Street";
                        dgvOwners.Columns["CISLO_POPISNE"].HeaderText = "Street Number";
                        dgvOwners.Columns["ICO"].HeaderText = "ICO";
                        dgvOwners.Columns["DIC"].HeaderText = "DIC";
                        dgvOwners.Columns["MESTO_VESNICE"].HeaderText = "City/Village";
                        dgvOwners.Columns["DRUH_VLASTNIKA"].HeaderText = "Owner Type";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading owners: " + ex.Message);
                }
            }
        }


        private void dgvOwners_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvOwners.CurrentRow != null)
            {
                selectedOwnerId = Convert.ToInt32(dgvOwners.CurrentRow.Cells["ID_VLASTNIKA"].Value);
                txtJmenoNazev.Text = dgvOwners.CurrentRow.Cells["JMENO_NAZEV"].Value?.ToString();
                txtPrijmeni.Text = dgvOwners.CurrentRow.Cells["PRIJMENI"].Value?.ToString();
                txtUlice.Text = dgvOwners.CurrentRow.Cells["ULICE"].Value?.ToString();
                txtCisloPopisne.Text = dgvOwners.CurrentRow.Cells["CISLO_POPISNE"].Value?.ToString();
                txtICO.Text = dgvOwners.CurrentRow.Cells["ICO"].Value?.ToString();
                txtDIC.Text = dgvOwners.CurrentRow.Cells["DIC"].Value?.ToString();

                string mestoVesnice = dgvOwners.CurrentRow.Cells["MESTO_VESNICE"].Value?.ToString();
                var selectedCity = ((List<Tuple<int, string>>)cbMestoVesnice.DataSource)
                    .FirstOrDefault(x => x.Item2 == mestoVesnice);
                cbMestoVesnice.SelectedValue = selectedCity?.Item1;

                string druhVlastnika = dgvOwners.CurrentRow.Cells["DRUH_VLASTNIKA"].Value?.ToString();
                cbDruhVlastnika.SelectedValue = druhVlastnika;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateFields())
            {
                return; // Stop if validation fails
            }

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (OracleCommand cmd = new OracleCommand("sprava_vlastnici_pivovaru", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("p_identifikator", OracleDbType.Int32).Value = DBNull.Value; // Add new
                        cmd.Parameters.Add("p_id_vlastnika", OracleDbType.Int32).Value = DBNull.Value;
                        cmd.Parameters.Add("p_jmeno_nazev", OracleDbType.Varchar2).Value = txtJmenoNazev.Text.Trim();
                        cmd.Parameters.Add("p_prijmeni", OracleDbType.Varchar2).Value = txtPrijmeni.Text.Trim();
                        cmd.Parameters.Add("p_ulice", OracleDbType.Varchar2).Value = txtUlice.Text.Trim();
                        cmd.Parameters.Add("p_cislo_popisne", OracleDbType.Varchar2).Value = txtCisloPopisne.Text.Trim();
                        cmd.Parameters.Add("p_ico", OracleDbType.Varchar2).Value = txtICO.Text.Trim();
                        cmd.Parameters.Add("p_dic", OracleDbType.Varchar2).Value = txtDIC.Text.Trim();
                        cmd.Parameters.Add("p_mesto_vesnice_id", OracleDbType.Int32).Value = ((Tuple<int, string>)cbMestoVesnice.SelectedItem).Item1;
                        cmd.Parameters.Add("p_druh_vlastnika", OracleDbType.Varchar2).Value = cbDruhVlastnika.SelectedValue.ToString();

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Owner added successfully.");
                        LoadOwners();
                        ClearFields();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error adding owner: " + ex.Message);
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedOwnerId == -1)
            {
                MessageBox.Show("No owner selected for update.");
                return;
            }

            if (!ValidateFields())
            {
                return; // Stop if validation fails
            }

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (OracleCommand cmd = new OracleCommand("sprava_vlastnici_pivovaru", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("p_identifikator", OracleDbType.Int32).Value = selectedOwnerId; // Update
                        cmd.Parameters.Add("p_id_vlastnika", OracleDbType.Int32).Value = selectedOwnerId;
                        cmd.Parameters.Add("p_jmeno_nazev", OracleDbType.Varchar2).Value = txtJmenoNazev.Text.Trim();
                        cmd.Parameters.Add("p_prijmeni", OracleDbType.Varchar2).Value = txtPrijmeni.Text.Trim();
                        cmd.Parameters.Add("p_ulice", OracleDbType.Varchar2).Value = txtUlice.Text.Trim();
                        cmd.Parameters.Add("p_cislo_popisne", OracleDbType.Varchar2).Value = txtCisloPopisne.Text.Trim();
                        cmd.Parameters.Add("p_ico", OracleDbType.Varchar2).Value = txtICO.Text.Trim();
                        cmd.Parameters.Add("p_dic", OracleDbType.Varchar2).Value = txtDIC.Text.Trim();
                        cmd.Parameters.Add("p_mesto_vesnice_id", OracleDbType.Int32).Value = ((Tuple<int, string>)cbMestoVesnice.SelectedItem).Item1;
                        cmd.Parameters.Add("p_druh_vlastnika", OracleDbType.Varchar2).Value = cbDruhVlastnika.SelectedValue.ToString();

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Owner updated successfully.");
                        LoadOwners();
                        ClearFields();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating owner: " + ex.Message);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedOwnerId == -1)
            {
                MessageBox.Show("No owner selected for deletion.");
                return;
            }

            DialogResult confirmResult = MessageBox.Show("Are you sure you want to delete this owner?", "Confirm Delete", MessageBoxButtons.YesNo);
            if (confirmResult != DialogResult.Yes)
            {
                return; // Stop if user cancels
            }

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "DELETE FROM VLASTNICI_PIVOVARU WHERE ID_VLASTNIKA = :id";
                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    {
                        cmd.Parameters.Add("id", OracleDbType.Int32).Value = selectedOwnerId;
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Owner deleted successfully.");
                        LoadOwners();
                        ClearFields();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error deleting owner: " + ex.Message);
                }
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool ValidateFields()
        {
            // Check if required fields are filled
            if (string.IsNullOrWhiteSpace(txtJmenoNazev.Text))
            {
                MessageBox.Show("Name/Company is required.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtICO.Text) || txtICO.Text.Length != 8 || !long.TryParse(txtICO.Text, out _))
            {
                MessageBox.Show("Valid ICO (8 digits) is required.");
                return false;
            }

            if (cbMestoVesnice.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a City/Village.");
                return false;
            }

            if (cbDruhVlastnika.SelectedIndex == -1)
            {
                MessageBox.Show("Please select an Owner Type.");
                return false;
            }

            // Optional fields can be validated if needed (e.g., PRIJMENI, DIC)
            if (!string.IsNullOrWhiteSpace(txtDIC.Text) && txtDIC.Text.Length > 12)
            {
                MessageBox.Show("DIC cannot exceed 12 characters.");
                return false;
            }

            return true;
        }

        private void ClearFields()
        {
            txtJmenoNazev.Clear();
            txtPrijmeni.Clear();
            txtUlice.Clear();
            txtCisloPopisne.Clear();
            txtICO.Clear();
            txtDIC.Clear();
            cbMestoVesnice.SelectedIndex = -1;
            cbDruhVlastnika.SelectedIndex = -1;

            selectedOwnerId = -1; // Reset the selected owner ID
        }


        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string ownerType = null;
            int? cityId = 20;
            int? breweryId = null;

            // Call the method to load the data into the DataGridView
            LoadOwnershipAudit(ownerType, cityId, breweryId);
        }

        private void LoadOwnershipAudit(string ownerType, int? cityId, int? breweryId)
        {
            string logFilePath = "debug_log.txt";

            try
            {
                using (OracleConnection conn = new OracleConnection(connectionString))
                {
                    conn.Open();

                    using (OracleCommand cmd = new OracleCommand("AuditBreweryOwnership", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("p_owner_type", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(ownerType) ? DBNull.Value : ownerType;
                        cmd.Parameters.Add("p_city_id", OracleDbType.Int32).Value = cityId.HasValue ? (object)cityId.Value : DBNull.Value;
                        cmd.Parameters.Add("p_brewery_id", OracleDbType.Int32).Value = breweryId.HasValue ? (object)breweryId.Value : DBNull.Value;

                        cmd.Parameters.Add("p_results", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                        using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                        {
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);

                            // Debug log for all rows
                            using (StreamWriter writer = new StreamWriter(logFilePath, true))
                            {
                                writer.WriteLine("=== DEBUG LOG ===");
                                writer.WriteLine($"Timestamp: {DateTime.Now}");

                                foreach (DataRow row in dataTable.Rows)
                                {
                                    writer.WriteLine("Row: " + string.Join(", ", row.ItemArray.Select(item => item ?? "NULL")));
                                }

                                writer.WriteLine("=== END OF LOG ===");
                            }

                            // Bind the DataTable to the DataGridView
                            dgvOwners.DataSource = null; // Clear previous binding
                            dgvOwners.Rows.Clear();
                            dgvOwners.Columns.Clear();

                            dgvOwners.DataSource = dataTable;
                            dgvOwners.AutoGenerateColumns = true;
                            dgvOwners.Refresh();

                            // Debug DataGridView rows
                            Console.WriteLine($"Rows in DataGridView: {dgvOwners.Rows.Count}");
                            foreach (DataGridViewRow row in dgvOwners.Rows)
                            {
                                if (!row.IsNewRow) // Exclude empty new row
                                {
                                    Console.WriteLine($"Row: {row.Cells["Owner_ID"].Value}, {row.Cells["First_Name"].Value}, {row.Cells["Last_Name"].Value}");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                using (StreamWriter writer = new StreamWriter(logFilePath, true))
                {
                    writer.WriteLine("=== ERROR LOG ===");
                    writer.WriteLine($"Timestamp: {DateTime.Now}");
                    writer.WriteLine($"Error: {ex.Message}");
                    writer.WriteLine("=== END OF ERROR LOG ===");
                }

                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

    }
}
