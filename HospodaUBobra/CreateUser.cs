using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HospodaUBobra
{
    public partial class CreateUser : Form
    {
        private string connectionString;
        public CreateUser(string connectionString)
        {
            InitializeComponent();
            this.connectionString = connectionString;

            dataGridViewUsers.ReadOnly = true;
            comboBoxRole.DropDownStyle = ComboBoxStyle.DropDownList;


            LoadRoles();
            LoadUsers();
        }

        private void LoadRoles()
        {
            comboBoxRole.Items.Clear();
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM A_SPRAVA_UZIVATELE_ROLE";

                using (OracleCommand cmd = new OracleCommand(query, conn))
                {
                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            comboBoxRole.Items.Add(reader.GetString(0));
                        }
                    }
                }
            }
        }

        private void LoadUsers()
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();

                string query = @"SELECT * from A_DGV_UZIVATELE";

                using (OracleCommand cmd = new OracleCommand(query, conn))
                {
                    using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        DataGridViewFilterHelper.BindData(dataGridViewUsers, dt);
                        dataGridViewUsers.DataSource = dt;
                    }
                }
            }

            if (dataGridViewUsers.Columns.Contains("ID_UZIVATELE"))
            {
                dataGridViewUsers.Columns["ID_UZIVATELE"].Visible = false;
            }
        }



        private void btnCreate_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string email = txtEmail.Text.Trim();
            string telefon = txtTelefon.Text.Trim();
            string password = txtPassword.Text;
            string role = comboBoxRole.SelectedItem?.ToString();

            if (!ValidateInputs(false))
            {
                return;
            }

            string salt = PasswordHelper.GenerateSalt();
            string hashedPassword = PasswordHelper.HashPassword(password, salt);

            int roleId = GetRoleId(role);

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();

                using (OracleCommand cmd = new OracleCommand("sprava_uzivatele", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("p_identifikator", OracleDbType.Int32).Value = DBNull.Value;
                    cmd.Parameters.Add("p_id_uzivatele", OracleDbType.Int32).Value = GetNextUserId();
                    cmd.Parameters.Add("p_uzivatelske_jmeno", OracleDbType.Varchar2).Value = username;
                    cmd.Parameters.Add("p_email", OracleDbType.Varchar2).Value = email;
                    cmd.Parameters.Add("p_telefon", OracleDbType.Varchar2).Value = telefon;
                    cmd.Parameters.Add("p_datum_registrace", OracleDbType.Date).Value = DateTime.Now;
                    cmd.Parameters.Add("p_password", OracleDbType.Varchar2).Value = hashedPassword;
                    cmd.Parameters.Add("p_salt", OracleDbType.Varchar2).Value = salt;
                    cmd.Parameters.Add("p_role_id", OracleDbType.Int32).Value = roleId;
                    cmd.Parameters.Add("p_profile_picture", OracleDbType.Int32).Value = DBNull.Value;

                    try
                    {
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Uživatel úspěšně přidán!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadUsers();
                    }
                    catch (OracleException ex)
                    {
                        MessageBox.Show("Chyba při vytváření uživatele: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private bool ValidateInputs(bool skipEmailCheck = false)
        {
            StringBuilder errorMessage = new StringBuilder();

            if (string.IsNullOrWhiteSpace(txtUsername.Text))
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

        private int GetNextUserId()
        {
            int nextId = 0;

            try
            {
                using (OracleConnection conn = new OracleConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT NVL(MAX(ID_UZIVATELE), 0) + 1 FROM UZIVATELE";

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
                MessageBox.Show("Chyba při získávání ID uživatele: " + ex.Message);
            }

            return nextId;
        }

        private int GetRoleId(string roleName)
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT ROLE_ID FROM ROLE WHERE ROLE_NAME = :roleName";

                using (OracleCommand cmd = new OracleCommand(query, conn))
                {
                    cmd.Parameters.Add(new OracleParameter("roleName", OracleDbType.Varchar2)).Value = roleName;

                    object result = cmd.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : -1;
                }
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridViewUsers.CurrentRow == null)
            {
                MessageBox.Show("Vyberte uživatele k aktualizaci.", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int userId = Convert.ToInt32(dataGridViewUsers.CurrentRow.Cells["ID_UZIVATELE"].Value);

            if (!ValidateInputs(skipEmailCheck: true))
            {
                return;
            }

            string username = txtUsername.Text.Trim();
            string email = txtEmail.Text.Trim();
            string telefon = txtTelefon.Text.Trim();
            string password = txtPassword.Text.Trim();
            string role = comboBoxRole.SelectedItem?.ToString();

            // Variables to store current values
            string currentPassword = null;
            string currentSalt = null;
            DateTime datumRegistrace = DateTime.Now;
            int? currentProfilePictureId = null;

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT PASSWORD, SALT, PROFILE_OBRAZKY_ID FROM UZIVATELE WHERE ID_UZIVATELE = :userId";
                using (OracleCommand cmd = new OracleCommand(query, conn))
                {
                    cmd.Parameters.Add(new OracleParameter("userId", OracleDbType.Int32)).Value = userId;
                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            currentPassword = reader["PASSWORD"] as string;
                            currentSalt = reader["SALT"] as string;
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
                salt = PasswordHelper.GenerateSalt(); // Generate new salt
                hashedPassword = PasswordHelper.HashPassword(newPassword, salt); // Hash new password
            }

             int roleId = GetRoleId(role);

            // Step 4: Update user using stored procedure
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string updateQuery = @"
            CALL sprava_uzivatele(
                :identifikator, 
                :id_uzivatele, 
                :uzivatelske_jmeno, 
                :email, 
                :telefon, 
                :datum_registrace, 
                :password, 
                :salt, 
                :role_id, 
                :profile_obrazky_id
            )";

                    using (OracleCommand cmdUpdate = new OracleCommand(updateQuery, conn))
                    {
                        cmdUpdate.Parameters.Add("identifikator", OracleDbType.Int32).Value = userId;
                        cmdUpdate.Parameters.Add("id_uzivatele", OracleDbType.Int32).Value = userId;
                        cmdUpdate.Parameters.Add("uzivatelske_jmeno", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(username) ? DBNull.Value : username;
                        cmdUpdate.Parameters.Add("email", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(email) ? DBNull.Value : email;
                        cmdUpdate.Parameters.Add("telefon", OracleDbType.Varchar2).Value = string.IsNullOrEmpty(telefon) ? DBNull.Value : telefon;
                        cmdUpdate.Parameters.Add("datum_registrace", OracleDbType.Date).Value = datumRegistrace;

                        cmdUpdate.Parameters.Add("password", OracleDbType.Varchar2).Value = hashedPassword ?? (object)DBNull.Value;
                        cmdUpdate.Parameters.Add("salt", OracleDbType.Varchar2).Value = salt ?? (object)DBNull.Value;
                        cmdUpdate.Parameters.Add("role_id", OracleDbType.Int32).Value = roleId;

                        cmdUpdate.Parameters.Add("profile_obrazky_id", OracleDbType.Int32)
                            .Value = currentProfilePictureId ?? (object)DBNull.Value;

                        cmdUpdate.ExecuteNonQuery();
                    }

                    MessageBox.Show("Uživatel úspěšně aktualizován!", "Úspěch", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadUsers();
                }
                catch (OracleException ex)
                {
                    MessageBox.Show("Chyba při aktualizaci uživatele: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridViewUsers.CurrentRow == null)
            {
                MessageBox.Show("Vyberte uživatele k odstranění.", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int userId = Convert.ToInt32(dataGridViewUsers.CurrentRow.Cells["ID_UZIVATELE"].Value);

            if (MessageBox.Show("Opravdu chcete odstranit tohoto uživatele?", "Potvrzení", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                using (OracleConnection conn = new OracleConnection(connectionString))
                {
                    conn.Open();
                    OracleTransaction transaction = conn.BeginTransaction();

                    try
                    {
                        string deleteReviewsQuery = "DELETE FROM RECENZE WHERE ID_UZIVATELE = :userId";
                        using (OracleCommand cmd = new OracleCommand(deleteReviewsQuery, conn))
                        {
                            cmd.Parameters.Add(new OracleParameter("userId", OracleDbType.Int32)).Value = userId;
                            cmd.ExecuteNonQuery();
                        }

                        using (OracleCommand cmd = new OracleCommand("DELETE FROM UZIVATELE WHERE ID_UZIVATELE = :userId", conn))
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.Add("userId", OracleDbType.Int32).Value = userId;

                            cmd.ExecuteNonQuery();
                        }


                        transaction.Commit();
                        MessageBox.Show("Uživatel úspěšně odstraněn.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadUsers();
                    }
                    catch (OracleException ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show("Chyba při mazání uživatele: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void dataGridViewUsers_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewUsers.CurrentRow != null)
            {
                txtUsername.Text = dataGridViewUsers.CurrentRow.Cells["UZIVATELSKE_JMENO"].Value.ToString();
                txtEmail.Text = dataGridViewUsers.CurrentRow.Cells["EMAIL"].Value.ToString();
                txtTelefon.Text = dataGridViewUsers.CurrentRow.Cells["TELEFON"].Value.ToString();

                string roleName = dataGridViewUsers.CurrentRow.Cells["ROLE_NAME"].Value.ToString();

                int roleIndex = comboBoxRole.Items.IndexOf(roleName);
                comboBoxRole.SelectedIndex = roleIndex;
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            DataGridViewFilterHelper.FilterData(dataGridViewUsers, txtSearch);
        }
    }
}
