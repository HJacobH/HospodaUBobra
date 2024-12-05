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
            LoadKlienti();

            dgvKlienti.ReadOnly = true;
            cbDruhPodniku.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void LoadDruhPodnikuOptions()
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT * FROM A_CB_DRUHY_PODKNIKU";

                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        DataTable druhPodnikuTable = new DataTable();
                        druhPodnikuTable.Load(reader);

                        cbDruhPodniku.DataSource = druhPodnikuTable;
                        cbDruhPodniku.DisplayMember = "DRUH_PODNIKU"; 
                        cbDruhPodniku.ValueMember = "ID_DRUHU";       
                        cbDruhPodniku.SelectedIndex = -1;          
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading Druh Podniku: {ex.Message}");
                }
            }
        }

        private void LoadKlienti()
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    string query = @"SELECT * FROM A_DGR_KLIENTI";

                    using (OracleDataAdapter adapter = new OracleDataAdapter(query, conn))
                    {
                        DataTable klientiTable = new DataTable();
                        adapter.Fill(klientiTable);

                        DataGridViewFilterHelper.BindData(dgvKlienti, klientiTable);
                        dgvKlienti.DataSource = klientiTable;

                        if (dgvKlienti.Columns.Contains("ID_KLIENTA"))
                        {
                            dgvKlienti.Columns["ID_KLIENTA"].Visible = false;
                        }

                        dgvKlienti.Columns["JMENO"].HeaderText = "Jméno";
                        dgvKlienti.Columns["PRIJMENI"].HeaderText = "Příjmení";
                        dgvKlienti.Columns["NAZEV"].HeaderText = "Název podniku";
                        dgvKlienti.Columns["EMAIL"].HeaderText = "Email";
                        dgvKlienti.Columns["TELEFON"].HeaderText = "Telefon";
                        dgvKlienti.Columns["DATUM_REGISTRACE"].HeaderText = "Datum registrace";
                        dgvKlienti.Columns["DRUH_PODNIKU"].HeaderText = "Druh podniku";
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
            if (!ValidateInputs())
            {
                return;
            }

            string jmeno = txtUsername.Text.Trim();
            string prijmeni = txtPrijmeni.Text.Trim();
            string nazev = txtNazev.Text.Trim();
            string email = txtEmail.Text.Trim();
            string telefon = txtTelefon.Text.Trim();
            int druhPodnikuId = cbDruhPodniku.SelectedValue != null ? Convert.ToInt32(cbDruhPodniku.SelectedValue) : -1;
            int? profilovyObrazekId = null;
            DateTime datumRegistrace = DateTime.Now;
            int roleId = 2;

            string defaultPassword = "DefaultPassword123";
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
                        cmd.Parameters.Add(new OracleParameter("idKlienta", OracleDbType.Int32)).Value = DBNull.Value;
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
                        MessageBox.Show("Klient úspěšně vytvořen!", "Úspěch", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadKlienti();
                        ClearFields();
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
                MessageBox.Show("Žádný klient nebyl vybrán. Prosím vyberte klienta.", "Aktualizace Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidateInputs())
            {
                return;
            }

            string jmeno = txtUsername.Text.Trim();
            string prijmeni = txtPrijmeni.Text.Trim();
            string nazev = txtNazev.Text.Trim();
            string email = txtEmail.Text.Trim();
            string telefon = txtTelefon.Text.Trim();
            int druhPodnikuId = cbDruhPodniku.SelectedValue != null ? Convert.ToInt32(cbDruhPodniku.SelectedValue) : -1;
            if (druhPodnikuId == null)
            {
                MessageBox.Show("Chyba: Druh Podniku není vybrán nebo je neplatný.", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int? profilovyObrazekId = null;
            DateTime datumRegistrace = DateTime.Now;
            int roleId = 2; // Fixed role ID for clients (Klient role)

            string currentPassword = null;
            string currentSalt = null;

            // Retrieve current password and salt from the database if the password field is empty
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT HESLO, SUL FROM KLIENTI WHERE ID_KLIENTA = :idKlienta";
                using (OracleCommand cmd = new OracleCommand(query, conn))
                {
                    cmd.Parameters.Add(new OracleParameter("idKlienta", OracleDbType.Int32)).Value = selectedKlientId;
                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            currentPassword = reader["HESLO"] as string;
                            currentSalt = reader["SUL"] as string;
                        }
                    }
                }
            }

            string newPassword = txtPassword.Text.Trim();
            string hashedPassword = string.IsNullOrEmpty(newPassword) ? currentPassword : PasswordHelper.HashPassword(newPassword, PasswordHelper.GenerateSalt());

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
                        cmd.Parameters.Add(new OracleParameter("identifikator", OracleDbType.Int32)).Value = selectedKlientId;
                        cmd.Parameters.Add(new OracleParameter("idKlienta", OracleDbType.Int32)).Value = selectedKlientId;
                        cmd.Parameters.Add(new OracleParameter("jmeno", OracleDbType.Varchar2)).Value = string.IsNullOrEmpty(jmeno) ? DBNull.Value : jmeno;
                        cmd.Parameters.Add(new OracleParameter("prijmeni", OracleDbType.Varchar2)).Value = string.IsNullOrEmpty(prijmeni) ? DBNull.Value : prijmeni;
                        cmd.Parameters.Add(new OracleParameter("nazev", OracleDbType.Varchar2)).Value = string.IsNullOrEmpty(nazev) ? DBNull.Value : nazev;
                        cmd.Parameters.Add(new OracleParameter("email", OracleDbType.Varchar2)).Value = email;
                        cmd.Parameters.Add(new OracleParameter("telefon", OracleDbType.Varchar2)).Value = telefon;
                        cmd.Parameters.Add(new OracleParameter("druhPodnikuId", OracleDbType.Int32)).Value = druhPodnikuId;
                        cmd.Parameters.Add(new OracleParameter("datumRegistrace", OracleDbType.Date)).Value = datumRegistrace;

                        cmd.Parameters.Add(new OracleParameter("heslo", OracleDbType.Varchar2)).Value = hashedPassword ?? (object)DBNull.Value;
                        cmd.Parameters.Add(new OracleParameter("sul", OracleDbType.Varchar2)).Value = string.IsNullOrEmpty(newPassword) ? currentSalt : PasswordHelper.GenerateSalt();

                        cmd.Parameters.Add(new OracleParameter("roleId", OracleDbType.Int32)).Value = roleId;
                        cmd.Parameters.Add(new OracleParameter("profilovyObrazekId", OracleDbType.Int32)).Value = (object)profilovyObrazekId ?? DBNull.Value;

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Kleint úspěšně aktualizován!", "Úspěch", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadKlienti();
                        ClearFields();
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
                MessageBox.Show("Prosím vyberte klienta ke smazání.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult dialogResult = MessageBox.Show("Opravdu chcete odstranit tohoto klienta?", "Potvrdit Smazání", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
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
                        MessageBox.Show("Klient úspěšně smazán!", "Úspěch", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            StringBuilder errorMessage = new StringBuilder();

            if (string.IsNullOrWhiteSpace(txtUsername.Text) && string.IsNullOrWhiteSpace(txtPrijmeni.Text) && string.IsNullOrWhiteSpace(txtNazev.Text))
            {
                errorMessage.AppendLine("Vyplňte alespoň 'Jméno a Příjmení' nebo 'Název'.");
            }

            if (string.IsNullOrWhiteSpace(txtEmail.Text) || !IsValidEmail(txtEmail.Text))
            {
                errorMessage.AppendLine("Zadejte platný email.");
            }

            if (string.IsNullOrWhiteSpace(txtTelefon.Text) || !IsValidPhoneNumber(txtTelefon.Text))
            {
                errorMessage.AppendLine("Zadejte platné telefonní číslo.");
            }

            if (cbDruhPodniku.SelectedIndex == -1)
            {
                errorMessage.AppendLine("Vyberte platný 'Druh Podniku'.");
            }

            if (errorMessage.Length > 0)
            {
                MessageBox.Show(errorMessage.ToString(), "Chyba validace", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var mailAddress = new System.Net.Mail.MailAddress(email);
                return mailAddress.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private bool IsValidPhoneNumber(string phoneNumber)
        {
            return phoneNumber.All(char.IsDigit) && phoneNumber.Length >= 9 && phoneNumber.Length <= 15;
        }

        private void dataGridViewUsers_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvKlienti.CurrentRow != null)
            {
                // Check and handle ID_KLIENTA
                if (dgvKlienti.CurrentRow.Cells["ID_KLIENTA"].Value != DBNull.Value)
                {
                    selectedKlientId = Convert.ToInt32(dgvKlienti.CurrentRow.Cells["ID_KLIENTA"].Value);
                }
                else
                {
                    selectedKlientId = -1; // Default value
                }

                // Handle JMENO
                txtUsername.Text = dgvKlienti.CurrentRow.Cells["JMENO"].Value != DBNull.Value
                    ? dgvKlienti.CurrentRow.Cells["JMENO"].Value.ToString()
                    : string.Empty;

                // Handle PRIJMENI
                txtPrijmeni.Text = dgvKlienti.CurrentRow.Cells["PRIJMENI"].Value != DBNull.Value
                    ? dgvKlienti.CurrentRow.Cells["PRIJMENI"].Value.ToString()
                    : string.Empty;

                // Handle NAZEV
                txtNazev.Text = dgvKlienti.CurrentRow.Cells["NAZEV"].Value != DBNull.Value
                    ? dgvKlienti.CurrentRow.Cells["NAZEV"].Value.ToString()
                    : string.Empty;

                // Handle EMAIL
                txtEmail.Text = dgvKlienti.CurrentRow.Cells["EMAIL"].Value != DBNull.Value
                    ? dgvKlienti.CurrentRow.Cells["EMAIL"].Value.ToString()
                    : string.Empty;

                // Handle TELEFON
                txtTelefon.Text = dgvKlienti.CurrentRow.Cells["TELEFON"].Value != DBNull.Value
                    ? dgvKlienti.CurrentRow.Cells["TELEFON"].Value.ToString()
                    : string.Empty;

                // Handle DATUM_REGISTRACE
                if (dgvKlienti.CurrentRow.Cells["DATUM_REGISTRACE"].Value != DBNull.Value)
                {
                    dtpDatumRegistrace.Value = Convert.ToDateTime(dgvKlienti.CurrentRow.Cells["DATUM_REGISTRACE"].Value);
                }
                else
                {
                    dtpDatumRegistrace.Value = DateTime.Now; // Default date
                }

                // Handle DRUH_PODNIKU
                string druhPodniku = dgvKlienti.CurrentRow.Cells["DRUH_PODNIKU"].Value != DBNull.Value
                    ? dgvKlienti.CurrentRow.Cells["DRUH_PODNIKU"].Value.ToString()
                    : string.Empty;

                cbDruhPodniku.SelectedIndex = cbDruhPodniku.FindStringExact(druhPodniku);
            }
            else
            {
                // Reset all fields if no row is selected
                selectedKlientId = -1;
                txtUsername.Clear();
                txtPrijmeni.Clear();
                txtNazev.Clear();
                txtEmail.Clear();
                txtTelefon.Clear();
                dtpDatumRegistrace.Value = DateTime.Now;
                cbDruhPodniku.SelectedIndex = -1;
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
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            DataGridViewFilterHelper.FilterData(dgvKlienti, txtSearch);
        }
    }
}
