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

        private void LoadKlienti()
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();

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

                        dgvKlienti.DataSource = klientiTable;

                        if (dgvKlienti.Columns.Contains("ID_KLIENTA"))
                        {
                            dgvKlienti.Columns["ID_KLIENTA"].Visible = false;
                        }

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
            if (!ValidateInputs())
            {
                return;
            }

            string jmeno = txtUsername.Text.Trim();
            string prijmeni = txtPrijmeni.Text.Trim();
            string nazev = txtNazev.Text.Trim();
            string email = txtEmail.Text.Trim();
            string telefon = txtTelefon.Text.Trim();
            int? druhPodnikuId = cbDruhPodniku.SelectedValue as int?;
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
            int? druhPodnikuId = cbDruhPodniku.SelectedValue as int?;
            int? profilovyObrazekId = null;
            DateTime datumRegistrace = DateTime.Now;
            int roleId = 2; 
            string newPassword = txtPassword.Text.Trim(); 

            string hashedPassword = null;
            string salt = null;

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
                        cmd.Parameters.Add(new OracleParameter("identifikator", OracleDbType.Int32)).Value = selectedKlientId;
                        cmd.Parameters.Add(new OracleParameter("idKlienta", OracleDbType.Int32)).Value = selectedKlientId;
                        cmd.Parameters.Add(new OracleParameter("jmeno", OracleDbType.Varchar2)).Value = string.IsNullOrEmpty(jmeno) ? DBNull.Value : jmeno;
                        cmd.Parameters.Add(new OracleParameter("prijmeni", OracleDbType.Varchar2)).Value = string.IsNullOrEmpty(prijmeni) ? DBNull.Value : prijmeni;
                        cmd.Parameters.Add(new OracleParameter("nazev", OracleDbType.Varchar2)).Value = string.IsNullOrEmpty(nazev) ? DBNull.Value : nazev;
                        cmd.Parameters.Add(new OracleParameter("email", OracleDbType.Varchar2)).Value = email;
                        cmd.Parameters.Add(new OracleParameter("telefon", OracleDbType.Varchar2)).Value = telefon;
                        cmd.Parameters.Add(new OracleParameter("druhPodnikuId", OracleDbType.Int32)).Value = (object)druhPodnikuId ?? DBNull.Value;
                        cmd.Parameters.Add(new OracleParameter("datumRegistrace", OracleDbType.Date)).Value = datumRegistrace;

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
                selectedKlientId = Convert.ToInt32(dgvKlienti.CurrentRow.Cells["ID_KLIENTA"].Value);

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
        }
    }
}
