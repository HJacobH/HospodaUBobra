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
                    string query = "SELECT * FROM A_CB_DRUHY_PODNIKU";

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

            string defaultPassword = txtPassword.Text.Trim();
            string salt = PasswordHelper.GenerateSalt();
            string hashedPassword = PasswordHelper.HashPassword(defaultPassword, salt);

            try
            {
                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    connection.Open();

                    using (OracleCommand cmd = new OracleCommand("sprava_procedures_pkg.sprava_klienti", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("p_identifikator", OracleDbType.Int32).Value = DBNull.Value;
                        cmd.Parameters.Add("p_id_klienta", OracleDbType.Int32).Value = DBNull.Value; 
                        cmd.Parameters.Add("p_jmeno", OracleDbType.Varchar2).Value = jmeno;
                        cmd.Parameters.Add("p_prijmeni", OracleDbType.Varchar2).Value = prijmeni;
                        cmd.Parameters.Add("p_nazev", OracleDbType.Varchar2).Value = nazev;
                        cmd.Parameters.Add("p_email", OracleDbType.Varchar2).Value = email;
                        cmd.Parameters.Add("p_telefon", OracleDbType.Varchar2).Value = telefon;
                        cmd.Parameters.Add("p_druh_podniku_id", OracleDbType.Int32).Value = druhPodnikuId;
                        cmd.Parameters.Add("p_datum_registrace", OracleDbType.Date).Value = DateTime.Now;
                        cmd.Parameters.Add("p_heslo", OracleDbType.Varchar2).Value = hashedPassword;
                        cmd.Parameters.Add("p_sul", OracleDbType.Varchar2).Value = salt;
                        cmd.Parameters.Add("p_role_id", OracleDbType.Int32).Value = 2; 
                        cmd.Parameters.Add("p_profilovy_obrazek_id", OracleDbType.Int32).Value = DBNull.Value; 

                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Nový klient úspěšně přidán.");
                        LoadKlienti();
                        ClearFields();
                    }
                }
            }
            catch (OracleException ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedKlientId == -1)
            {
                MessageBox.Show("Žádný klient nebyl vybrán. Prosím vyberte klienta.", "Aktualizace Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidateInputs(skipEmailCheck: true))
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

            string currentPassword = null;
            string currentSalt = null;
            int? currentProfilePictureId = null;

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT HESLO, SUL, PROFILE_OBRAZKY_ID FROM KLIENTI WHERE ID_KLIENTA = :idKlienta";
                using (OracleCommand cmd = new OracleCommand(query, conn))
                {
                    cmd.Parameters.Add(new OracleParameter("idKlienta", OracleDbType.Int32)).Value = selectedKlientId;
                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            currentPassword = reader["HESLO"] as string;
                            currentSalt = reader["SUL"] as string;
                            currentProfilePictureId = reader["PROFILE_OBRAZKY_ID"] == DBNull.Value
                                ? (int?)null
                                : Convert.ToInt32(reader["PROFILE_OBRAZKY_ID"]);
                        }
                    }
                }
            }

            string newPassword = txtPassword.Text.Trim();
            string salt = currentSalt; 
            string hashedPassword = currentPassword; 

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
                        CALL sprava_procedures_pkg.sprava_klienti(
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
                        cmd.Parameters.Add(new OracleParameter("sul", OracleDbType.Varchar2)).Value = salt ?? (object)DBNull.Value;

                        cmd.Parameters.Add(new OracleParameter("roleId", OracleDbType.Int32)).Value = roleId;
                        cmd.Parameters.Add(new OracleParameter("profilovyObrazekId", OracleDbType.Int32))
                        .Value = profilovyObrazekId ?? currentProfilePictureId ?? (object)DBNull.Value;

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Klient úspěšně aktualizován!", "Úspěch", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                OracleTransaction transaction = null;

                try
                {
                    conn.Open();
                    transaction = conn.BeginTransaction();

                    int profileImageId = 0;
                    string selectProfileImageQuery = "SELECT PROFILE_OBRAZKY_ID FROM KLIENTI WHERE ID_KLIENTA = :klientId";
                    using (OracleCommand cmdSelect = new OracleCommand(selectProfileImageQuery, conn))
                    {
                        cmdSelect.Transaction = transaction;
                        cmdSelect.Parameters.Add(new OracleParameter("klientId", OracleDbType.Int32)).Value = selectedKlientId;

                        object result = cmdSelect.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            profileImageId = Convert.ToInt32(result);
                        }
                    }

                    string deleteClientQuery = "DELETE FROM KLIENTI WHERE ID_KLIENTA = :klientId";
                    using (OracleCommand cmdDeleteClient = new OracleCommand(deleteClientQuery, conn))
                    {
                        cmdDeleteClient.Transaction = transaction;
                        cmdDeleteClient.Parameters.Add(new OracleParameter("klientId", OracleDbType.Int32)).Value = selectedKlientId;
                        cmdDeleteClient.ExecuteNonQuery();
                    }

                    if (profileImageId > 0)
                    {
                        string deleteProfileImageQuery = "DELETE FROM PROFILOVE_OBRAZKY WHERE ID_PICTURE = :profileImageId";
                        using (OracleCommand cmdDeleteProfileImage = new OracleCommand(deleteProfileImageQuery, conn))
                        {
                            cmdDeleteProfileImage.Transaction = transaction;
                            cmdDeleteProfileImage.Parameters.Add(new OracleParameter("profileImageId", OracleDbType.Int32)).Value = profileImageId;
                            cmdDeleteProfileImage.ExecuteNonQuery();
                        }
                    }

                    transaction.Commit();
                    MessageBox.Show("Klient a jeho profilový obrázek byli úspěšně smazáni!", "Úspěch", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LoadKlienti();
                }
                catch (OracleException ex)
                {
                    transaction?.Rollback();
                    MessageBox.Show($"Oracle error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    transaction?.Rollback();
                    MessageBox.Show($"General error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private bool ValidateInputs(bool skipEmailCheck = false)
        {
            StringBuilder errorMessage = new StringBuilder();

            if (string.IsNullOrWhiteSpace(txtUsername.Text) && string.IsNullOrWhiteSpace(txtPrijmeni.Text) && string.IsNullOrWhiteSpace(txtNazev.Text))
            {
                errorMessage.AppendLine("Vyplňte alespoň 'Jméno a Příjmení' nebo 'Název'.");
            }

            if (!skipEmailCheck)
            {
                if (string.IsNullOrWhiteSpace(txtEmail.Text) || !IsValidEmail(txtEmail.Text))
                {
                    errorMessage.AppendLine("Zadejte platný email.");
                }
                else
                {
                    if (IsEmailInUse(txtEmail.Text))
                    {
                        errorMessage.AppendLine("Tento email je již použitý pro jiného klienta nebo uživatele.");
                    }
                }
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
        private bool IsEmailInUse(string email)
        {
            bool isInUse = false;

            try
            {
                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                    SELECT COUNT(*) 
                    FROM (
                        SELECT EMAIL FROM KLIENTI
                        UNION
                        SELECT EMAIL FROM UZIVATELE
                    ) 
                    WHERE UPPER(EMAIL) = :email";

                    using (OracleCommand cmd = new OracleCommand(query, connection))
                    {
                        cmd.Parameters.Add("email", OracleDbType.Varchar2).Value = email.ToUpper();

                        object result = cmd.ExecuteScalar();
                        isInUse = Convert.ToInt32(result) > 0;
                    }
                }
            }
            catch (OracleException ex)
            {
                MessageBox.Show("Error checking email: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return isInUse;
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
                if (dgvKlienti.CurrentRow.Cells["ID_KLIENTA"].Value != DBNull.Value)
                {
                    selectedKlientId = Convert.ToInt32(dgvKlienti.CurrentRow.Cells["ID_KLIENTA"].Value);
                }
                else
                {
                    selectedKlientId = -1; 
                }

                txtUsername.Text = dgvKlienti.CurrentRow.Cells["JMENO"].Value != DBNull.Value
                    ? dgvKlienti.CurrentRow.Cells["JMENO"].Value.ToString()
                    : string.Empty;

                txtPrijmeni.Text = dgvKlienti.CurrentRow.Cells["PRIJMENI"].Value != DBNull.Value
                    ? dgvKlienti.CurrentRow.Cells["PRIJMENI"].Value.ToString()
                    : string.Empty;

                txtNazev.Text = dgvKlienti.CurrentRow.Cells["NAZEV"].Value != DBNull.Value
                    ? dgvKlienti.CurrentRow.Cells["NAZEV"].Value.ToString()
                    : string.Empty;

                txtEmail.Text = dgvKlienti.CurrentRow.Cells["EMAIL"].Value != DBNull.Value
                    ? dgvKlienti.CurrentRow.Cells["EMAIL"].Value.ToString()
                    : string.Empty;

                txtTelefon.Text = dgvKlienti.CurrentRow.Cells["TELEFON"].Value != DBNull.Value
                    ? dgvKlienti.CurrentRow.Cells["TELEFON"].Value.ToString()
                    : string.Empty;

                if (dgvKlienti.CurrentRow.Cells["DATUM_REGISTRACE"].Value != DBNull.Value)
                {
                    dtpDatumRegistrace.Value = Convert.ToDateTime(dgvKlienti.CurrentRow.Cells["DATUM_REGISTRACE"].Value);
                }
                else
                {
                    dtpDatumRegistrace.Value = DateTime.Now;
                }

                string druhPodniku = dgvKlienti.CurrentRow.Cells["DRUH_PODNIKU"].Value != DBNull.Value
                    ? dgvKlienti.CurrentRow.Cells["DRUH_PODNIKU"].Value.ToString()
                    : string.Empty;

                cbDruhPodniku.SelectedIndex = cbDruhPodniku.FindStringExact(druhPodniku);
            }
            else
            {
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
