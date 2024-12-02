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
    public partial class SpravaKlientu : Form
    {
        string connectionString = $"User Id=st69639;Password=Server2022;Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=fei-sql3.upceucebny.cz)(PORT=1521)))(CONNECT_DATA=(SID=BDAS)));";
        private int selectedKlientId = -1;

        public SpravaKlientu()
        {
            InitializeComponent();
            LoadDruhPodnikuOptions();
            //LoadProfileImageOptions();
            LoadKlienti();
        }

        private void LoadDruhPodnikuOptions()
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT ID_DRUHU, DRUH_PODNIKU FROM DRUHY_PODNIKU";
                using (OracleCommand cmd = new OracleCommand(query, conn))
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        cbDruhPodniku.Items.Add(new
                        {
                            ID = reader.GetInt32(0),
                            Name = reader.GetString(1)
                        });
                    }
                }
            }
            cbDruhPodniku.DisplayMember = "Name";
            cbDruhPodniku.ValueMember = "ID";
        }

        /*private void LoadProfileImageOptions()
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT ID_OBRAZKU, NAZEV FROM PROFILOVE_OBRAZKY";
                using (OracleCommand cmd = new OracleCommand(query, conn))
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        cbProfilovyObrazek.Items.Add(new
                        {
                            ID = reader.GetInt32(0),
                            Name = reader.GetString(1)
                        });
                    }
                }
            }
            cbProfilovyObrazek.DisplayMember = "Name";
            cbProfilovyObrazek.ValueMember = "ID";
        }*/

        private void LoadKlienti()
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    // Query to retrieve data
                    string query = @"
                SELECT 
                    k.ID_KLIENTA, 
                    k.JMENO, 
                    k.PRIJMENI, 
                    k.NAZEV, 
                    k.EMAIL, 
                    k.TELEFON, 
                    k.DATUM_REGISTRACE, 
                    dp.DRUH_PODNIKU, 
                    r.ROLE_NAME
                FROM KLIENTI k
                LEFT JOIN DRUHY_PODNIKU dp ON k.DRUH_PODNIKU_ID_DRUHU = dp.ID_DRUHU
                LEFT JOIN ROLE r ON k.ROLE_ID = r.ROLE_ID";

                    using (OracleDataAdapter adapter = new OracleDataAdapter(query, conn))
                    {
                        DataTable klientiTable = new DataTable();
                        adapter.Fill(klientiTable);

                        // Bind data to DataGridView
                        dgvKlienti.DataSource = klientiTable;

                        // Hide the ID_KLIENTA column
                        if (dgvKlienti.Columns.Contains("ID_KLIENTA"))
                        {
                            dgvKlienti.Columns["ID_KLIENTA"].Visible = false;
                        }

                        // Optional: Set more user-friendly column headers
                        dgvKlienti.Columns["JMENO"].HeaderText = "First Name";
                        dgvKlienti.Columns["PRIJMENI"].HeaderText = "Last Name";
                        dgvKlienti.Columns["NAZEV"].HeaderText = "Business Name";
                        dgvKlienti.Columns["EMAIL"].HeaderText = "Email";
                        dgvKlienti.Columns["TELEFON"].HeaderText = "Phone";
                        dgvKlienti.Columns["DATUM_REGISTRACE"].HeaderText = "Registration Date";
                        dgvKlienti.Columns["DRUH_PODNIKU"].HeaderText = "Business Type";
                        dgvKlienti.Columns["ROLE_NAME"].HeaderText = "Role";
                    }
                }
                catch (OracleException ex)
                {
                    MessageBox.Show($"Oracle error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"General error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }






        private void btnCreate_Click(object sender, EventArgs e)
        {
            if(!ValidateInputs())
    {
                MessageBox.Show("Please fill in all required fields correctly.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string jmeno = txtUsername.Text.Trim();
            string prijmeni = txtPrijmeni.Text.Trim();
            string nazev = txtNazev.Text.Trim();
            string email = txtEmail.Text.Trim();
            string telefon = txtTelefon.Text.Trim();
            int? druhPodnikuId = cbDruhPodniku.SelectedValue as int?;
            int? profilovyObrazekId = null;
            DateTime datumRegistrace = DateTime.Now; // Current timestamp as registration date
            int roleId = 2; // Fixed role ID for clients (Klient role)

            // Generate password and salt
            string defaultPassword = "DefaultPassword123"; // Replace with your desired default password
            string salt = PasswordHelper.GenerateSalt();
            string hashedPassword = PasswordHelper.HashPassword(defaultPassword, salt);

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    string query = "CALL sprava_klienti(null, :idKlienta, :jmeno, :prijmeni, :nazev, :email, :telefon, :druhPodnikuId, :datumRegistrace, :heslo, :sul, :roleId, :profilovyObrazekId)";
                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    {
                        // Parameters for the procedure
                        cmd.Parameters.Add(new OracleParameter("idKlienta", OracleDbType.Int32)).Value = DBNull.Value; // Let the trigger handle ID generation
                        cmd.Parameters.Add(new OracleParameter("jmeno", OracleDbType.Varchar2)).Value = string.IsNullOrEmpty(jmeno) ? DBNull.Value : jmeno;
                        cmd.Parameters.Add(new OracleParameter("prijmeni", OracleDbType.Varchar2)).Value = string.IsNullOrEmpty(prijmeni) ? DBNull.Value : prijmeni;
                        cmd.Parameters.Add(new OracleParameter("nazev", OracleDbType.Varchar2)).Value = string.IsNullOrEmpty(nazev) ? DBNull.Value : nazev;
                        cmd.Parameters.Add(new OracleParameter("email", OracleDbType.Varchar2)).Value = email;
                        cmd.Parameters.Add(new OracleParameter("telefon", OracleDbType.Varchar2)).Value = telefon;
                        cmd.Parameters.Add(new OracleParameter("druhPodnikuId", OracleDbType.Int32)).Value = (object)druhPodnikuId ?? DBNull.Value;
                        cmd.Parameters.Add(new OracleParameter("datumRegistrace", OracleDbType.Date)).Value = datumRegistrace;
                        cmd.Parameters.Add(new OracleParameter("heslo", OracleDbType.Varchar2)).Value = hashedPassword;
                        cmd.Parameters.Add(new OracleParameter("sul", OracleDbType.Varchar2)).Value = salt;
                        cmd.Parameters.Add(new OracleParameter("roleId", OracleDbType.Int32)).Value = roleId;
                        cmd.Parameters.Add(new OracleParameter("profilovyObrazekId", OracleDbType.Int32)).Value = (object)profilovyObrazekId ?? DBNull.Value;

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Client created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadKlienti(); // Refresh the data grid
                        ClearFields(); // Clear input fields
                    }
                }
                catch (OracleException ex)
                {
                    MessageBox.Show($"Oracle error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedKlientId == -1)
            {
                MessageBox.Show("No client selected for update. Please select a client.", "Update Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidateInputs())
            {
                MessageBox.Show("Please fill in all required fields correctly.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string jmeno = txtUsername.Text.Trim();
            string prijmeni = txtPrijmeni.Text.Trim();
            string nazev = txtNazev.Text.Trim();
            string email = txtEmail.Text.Trim();
            string telefon = txtTelefon.Text.Trim();
            int? druhPodnikuId = cbDruhPodniku.SelectedValue as int?;
            int? profilovyObrazekId = null;
            DateTime datumRegistrace = DateTime.Now; // Current timestamp for update
            int roleId = 2; // Fixed role ID for clients (Klient role)
            string newPassword = txtPassword.Text.Trim(); // Get password field value

            // Declare variables for password and salt
            string hashedPassword = null;
            string salt = null;

            // Only hash and salt the password if the user entered a new one
            if (!string.IsNullOrEmpty(newPassword))
            {
                salt = PasswordHelper.GenerateSalt();
                hashedPassword = PasswordHelper.HashPassword(newPassword, salt);
            }

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    string query = @"
                CALL sprava_klienti(
                    :identifikator, 
                    :idKlienta, 
                    :jmeno, 
                    :prijmeni, 
                    :nazev, 
                    :email, 
                    :telefon, 
                    :druhPodnikuId, 
                    :datumRegistrace, 
                    :heslo, 
                    :sul, 
                    :roleId, 
                    :profilovyObrazekId
                )";

                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    {
                        // Parameters for the procedure
                        cmd.Parameters.Add(new OracleParameter("identifikator", OracleDbType.Int32)).Value = selectedKlientId; // Pass client ID for update
                        cmd.Parameters.Add(new OracleParameter("idKlienta", OracleDbType.Int32)).Value = selectedKlientId;
                        cmd.Parameters.Add(new OracleParameter("jmeno", OracleDbType.Varchar2)).Value = string.IsNullOrEmpty(jmeno) ? DBNull.Value : jmeno;
                        cmd.Parameters.Add(new OracleParameter("prijmeni", OracleDbType.Varchar2)).Value = string.IsNullOrEmpty(prijmeni) ? DBNull.Value : prijmeni;
                        cmd.Parameters.Add(new OracleParameter("nazev", OracleDbType.Varchar2)).Value = string.IsNullOrEmpty(nazev) ? DBNull.Value : nazev;
                        cmd.Parameters.Add(new OracleParameter("email", OracleDbType.Varchar2)).Value = email;
                        cmd.Parameters.Add(new OracleParameter("telefon", OracleDbType.Varchar2)).Value = telefon;
                        cmd.Parameters.Add(new OracleParameter("druhPodnikuId", OracleDbType.Int32)).Value = (object)druhPodnikuId ?? DBNull.Value;
                        cmd.Parameters.Add(new OracleParameter("datumRegistrace", OracleDbType.Date)).Value = datumRegistrace;

                        // Only pass password and salt if they are updated
                        if (!string.IsNullOrEmpty(newPassword))
                        {
                            cmd.Parameters.Add(new OracleParameter("heslo", OracleDbType.Varchar2)).Value = hashedPassword;
                            cmd.Parameters.Add(new OracleParameter("sul", OracleDbType.Varchar2)).Value = salt;
                        }
                        else
                        {
                            cmd.Parameters.Add(new OracleParameter("heslo", OracleDbType.Varchar2)).Value = DBNull.Value;
                            cmd.Parameters.Add(new OracleParameter("sul", OracleDbType.Varchar2)).Value = DBNull.Value;
                        }

                        cmd.Parameters.Add(new OracleParameter("roleId", OracleDbType.Int32)).Value = roleId;
                        cmd.Parameters.Add(new OracleParameter("profilovyObrazekId", OracleDbType.Int32)).Value = (object)profilovyObrazekId ?? DBNull.Value;

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Client updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadKlienti(); // Refresh the data grid
                        ClearFields(); // Clear input fields
                    }
                }
                catch (OracleException ex)
                {
                    MessageBox.Show($"Oracle error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedKlientId == -1)
            {
                MessageBox.Show("Please select a Klient to delete.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete this Klient?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.No)
            {
                return;
            }

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    string query = "DELETE FROM KLIENTI WHERE ID_KLIENTA = :klientId";
                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    {
                        cmd.Parameters.Add(new OracleParameter("klientId", OracleDbType.Int32)).Value = selectedKlientId;

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Klient successfully deleted!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    LoadKlienti();
                }
                catch (OracleException ex)
                {
                    MessageBox.Show($"Oracle error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"General error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtEmail.Text) || string.IsNullOrWhiteSpace(txtTelefon.Text) || cbDruhPodniku.SelectedIndex == -1)
            {
                return false;
            }

            // Either 'Jmeno' and 'Prijmeni' or 'Nazev' must be filled
            if ((string.IsNullOrWhiteSpace(txtUsername.Text) && string.IsNullOrWhiteSpace(txtPrijmeni.Text)) && string.IsNullOrWhiteSpace(txtNazev.Text))
            {
                return false;
            }

            return true;
        }

        private void dataGridViewUsers_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvKlienti.CurrentRow != null)
            {
                // Use the hidden ID_KLIENTA value programmatically
                selectedKlientId = Convert.ToInt32(dgvKlienti.CurrentRow.Cells["ID_KLIENTA"].Value);

                // Populate other fields in the form
                txtUsername.Text = dgvKlienti.CurrentRow.Cells["JMENO"].Value?.ToString() ?? string.Empty;
                txtPrijmeni.Text = dgvKlienti.CurrentRow.Cells["PRIJMENI"].Value?.ToString() ?? string.Empty;
                txtNazev.Text = dgvKlienti.CurrentRow.Cells["NAZEV"].Value?.ToString() ?? string.Empty;
                txtEmail.Text = dgvKlienti.CurrentRow.Cells["EMAIL"].Value?.ToString() ?? string.Empty;
                txtTelefon.Text = dgvKlienti.CurrentRow.Cells["TELEFON"].Value?.ToString() ?? string.Empty;
                dtpDatumRegistrace.Value = Convert.ToDateTime(dgvKlienti.CurrentRow.Cells["DATUM_REGISTRACE"].Value);

                cbDruhPodniku.SelectedIndex = cbDruhPodniku.FindStringExact(dgvKlienti.CurrentRow.Cells["DRUH_PODNIKU"].Value?.ToString() ?? string.Empty);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void ClearFields()
        {
            txtUsername.Clear();
            txtPrijmeni.Clear();
            txtNazev.Clear();
            txtEmail.Clear();
            txtTelefon.Clear();
            dtpDatumRegistrace.Value = DateTime.Now;
            cbDruhPodniku.SelectedIndex = -1;
            //cbRole.SelectedIndex = -1;
            //cbProfilovyObrazek.SelectedIndex = -1;
        }
    }
}
