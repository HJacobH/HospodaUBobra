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

            LoadDruhVlastnikaAudit();
            LoadMestaAudit();
            LoadPivovarAudit();

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

                        // Set column headers
                        dgvOwners.Columns["ID_VLASTNIKA"].HeaderText = "Owner ID";
                        dgvOwners.Columns["JMENO_NAZEV"].HeaderText = "Name/Company";
                        dgvOwners.Columns["PRIJMENI"].HeaderText = "Surname";
                        dgvOwners.Columns["ULICE"].HeaderText = "Street";
                        dgvOwners.Columns["CISLO_POPISNE"].HeaderText = "Street Number";
                        dgvOwners.Columns["ICO"].HeaderText = "ICO";
                        dgvOwners.Columns["DIC"].HeaderText = "DIC";
                        dgvOwners.Columns["MESTO_VESNICE"].HeaderText = "City/Village";
                        dgvOwners.Columns["DRUH_VLASTNIKA"].HeaderText = "Owner Type";

                        // Hide the Owner ID column
                        dgvOwners.Columns["ID_VLASTNIKA"].Visible = false;
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
            try
            {
                // Get selected values from the comboboxes
                string ownerType = cbDruhVlastnikaAudit.SelectedValue?.ToString();
                int? cityId = cbMestaAudit.SelectedValue as int?;
                int? breweryId = cbPivovarAudit.SelectedValue as int?;

                using (var connection = new OracleConnection(connectionString))
                {
                    connection.Open();

                    using (var command = new OracleCommand("AuditBreweryOwnership", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Define the procedure parameters with selected combobox values
                        command.Parameters.Add("p_owner_type", OracleDbType.Varchar2).Value = (object)ownerType ?? DBNull.Value;
                        command.Parameters.Add("p_city_id", OracleDbType.Int32).Value = (object)cityId ?? DBNull.Value;
                        command.Parameters.Add("p_brewery_id", OracleDbType.Int32).Value = (object)breweryId ?? DBNull.Value;

                        // Define the output parameter for the ref cursor
                        var resultsParam = new OracleParameter("p_results", OracleDbType.RefCursor)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(resultsParam);

                        // Execute the procedure and retrieve the results
                        using (var reader = command.ExecuteReader())
                        {
                            // Load results into a DataTable
                            var dataTable = new DataTable();
                            dataTable.Load(reader);

                            // Display results in a DataGridView inside a popup window
                            DisplayResultsInPopup(dataTable);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void DisplayResultsInPopup(DataTable dataTable)
        {
            // Create a new form for displaying results
            var form = new Form
            {
                Text = "Audit Brewery Ownership Results",
                Width = 800,
                Height = 600
            };

            // Create a DataGridView to display the data
            var dataGridView = new DataGridView
            {
                DataSource = dataTable,
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true
            };

            // Add the DataGridView to the form
            form.Controls.Add(dataGridView);

            // Hide the ID_VLASTNIKA column after adding the DataGridView to the form
            form.Shown += (s, e) =>
            {
                if (dataGridView.Columns.Contains("ID_VLASTNIKA"))
                {
                    dataGridView.Columns["ID_VLASTNIKA"].Visible = false;
                }
            };

            // Show the form as a modal dialog
            form.ShowDialog();
        }



        private void LoadDruhVlastnikaAudit()
        {
            // Define a list of values including a blank option for unfiltered searches
            var druhVlastnikaAudit = new List<Tuple<string, string>>()
    {
        new Tuple<string, string>("", "All (No Filter)"), // Blank option
        new Tuple<string, string>("FO", "Physical Person"), // Option for FO
        new Tuple<string, string>("PO", "Legal Person") // Option for PO
    };

            // Bind the list to the ComboBox
            cbDruhVlastnikaAudit.DataSource = druhVlastnikaAudit;
            cbDruhVlastnikaAudit.DisplayMember = "Item2"; // Display text (e.g., "All (No Filter)")
            cbDruhVlastnikaAudit.ValueMember = "Item1"; // Actual value (e.g., "", "FO", "PO")
            cbDruhVlastnikaAudit.SelectedIndex = 0; // Default selection to "All (No Filter)"
        }

        private void LoadMestaAudit()
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
                        var mestaList = new List<Tuple<int?, string>>();

                        // Add a blank option for no filter
                        mestaList.Add(new Tuple<int?, string>(null, "All (No Filter)"));

                        // Add the rest of the cities from the database
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            string name = reader.GetString(1);
                            mestaList.Add(new Tuple<int?, string>(id, name));
                        }

                        // Bind the list to the ComboBox
                        cbMestaAudit.DataSource = mestaList;
                        cbMestaAudit.DisplayMember = "Item2"; // Display the city name
                        cbMestaAudit.ValueMember = "Item1";   // Use the city ID as the value
                        cbMestaAudit.SelectedIndex = 0;      // Default to "All (No Filter)"
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading cities: " + ex.Message);
                }
            }
        }

        private void LoadPivovarAudit()
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT ID_PIVOVARU, NAZEV FROM PIVOVARY";

                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        var pivovarList = new List<Tuple<int?, string>>();

                        // Add a blank option for no filter
                        pivovarList.Add(new Tuple<int?, string>(null, "All (No Filter)"));

                        // Add the rest of the breweries from the database
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            string name = reader.GetString(1);
                            pivovarList.Add(new Tuple<int?, string>(id, name));
                        }

                        // Bind the list to the ComboBox
                        cbPivovarAudit.DataSource = pivovarList;
                        cbPivovarAudit.DisplayMember = "Item2"; // Display the brewery name
                        cbPivovarAudit.ValueMember = "Item1";   // Use the brewery ID as the value
                        cbPivovarAudit.SelectedIndex = 0;      // Default to "All (No Filter)"
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading breweries: " + ex.Message);
                }
            }
        }


    }
}
