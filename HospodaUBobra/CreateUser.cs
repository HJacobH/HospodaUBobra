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
                string query = "SELECT ROLE_NAME FROM ROLE WHERE ROLE_NAME != 'Anonymous' AND ROLE_NAME != 'Klient'";

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

                string query = @"
            SELECT 
                U.ID_UZIVATELE, 
                U.UZIVATELSKE_JMENO, 
                U.EMAIL, 
                U.TELEFON, 
                U.DATUM_REGISTRACE, 
                R.ROLE_NAME AS ROLE_NAME
            FROM 
                UZIVATELE U
            INNER JOIN 
                ROLE R ON U.ROLE_ID = R.ROLE_ID";

                using (OracleCommand cmd = new OracleCommand(query, conn))
                {
                    using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
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

            if (!ValidateInputs(username, email, telefon, password, role))
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
                    cmd.Parameters.Add("p_profile_picture", OracleDbType.Blob).Value = DBNull.Value;

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

        private bool ValidateInputs(string username, string email, string telefon, string password, string role)
        {
            if (string.IsNullOrWhiteSpace(username) || username.Length > 50 || !Regex.IsMatch(username, @"^[a-zA-Z0-9]+$"))
            {
                MessageBox.Show("Uživatelské jméno musí být alfanumerické a kratší než 50 znaků.", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(email) || !Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show("Zadejte platnou e-mailovou adresu.", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(telefon) || telefon.Length > 15 || !Regex.IsMatch(telefon, @"^\d+$"))
            {
                MessageBox.Show("Telefonní číslo musí obsahovat pouze čísla a být kratší než 15 znaků.", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(password) || password.Length < 8 || password.Length > 50)
            {
                MessageBox.Show("Heslo musí být mezi 8 a 50 znaky.", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrEmpty(role))
            {
                MessageBox.Show("Vyberte roli uživatele.", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
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

            string currentPassword = null;
            string currentSalt = null;
            DateTime? currentRegistrationDate = null;
            int currentProfilePicture = 0;

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT PASSWORD, SALT, DATUM_REGISTRACE FROM UZIVATELE WHERE ID_UZIVATELE = :userId";
                string pictureQuery = @"
                    SELECT 
                        P.ID_PICTURE
                    FROM 
                        PROFILOVE_OBRAZKY P
                    WHERE 
                        P.ID_PICTURE = (SELECT U.PROFILE_OBRAZKY_ID FROM UZIVATELE U WHERE U.ID_UZIVATELE = :userId)";




                using (OracleCommand cmd = new OracleCommand(query, conn))
                {
                    cmd.Parameters.Add(new OracleParameter("userId", OracleDbType.Int32)).Value = userId;

                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            currentPassword = reader["PASSWORD"] as string;
                            currentSalt = reader["SALT"] as string;
                            currentRegistrationDate = reader["DATUM_REGISTRACE"] as DateTime?;
                        }
                    }
                }
                using (OracleCommand cmd = new OracleCommand(pictureQuery, conn))
                {
                    cmd.Parameters.Add(new OracleParameter("userId", OracleDbType.Int32)).Value = userId;

                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            currentProfilePicture = Convert.ToInt32(reader["ID_PICTURE"]);
                        }
                    }
                }
            }

            string username = txtUsername.Text.Trim();
            string email = txtEmail.Text.Trim();
            string telefon = txtTelefon.Text.Trim();
            string password = txtPassword.Text;
            string role = comboBoxRole.SelectedItem?.ToString();

            if (!ValidateInputs(username, email, telefon, password, role))
            {
                return;
            }

            string salt = string.IsNullOrEmpty(password) ? currentSalt : PasswordHelper.GenerateSalt();
            string hashedPassword = string.IsNullOrEmpty(password) ? currentPassword : PasswordHelper.HashPassword(password, salt);

            DateTime registrationDate = currentRegistrationDate ?? DateTime.Now;

            int roleId = GetRoleId(role);

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();
                OracleTransaction transaction = conn.BeginTransaction();

                try
                {
                    using (OracleCommand cmd = new OracleCommand("sprava_uzivatele", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("p_identifikator", OracleDbType.Int32).Value = 1;
                        cmd.Parameters.Add("p_id_uzivatele", OracleDbType.Int32).Value = userId;
                        cmd.Parameters.Add("p_uzivatelske_jmeno", OracleDbType.Varchar2).Value = username ?? (object)DBNull.Value;
                        cmd.Parameters.Add("p_email", OracleDbType.Varchar2).Value = email ?? (object)DBNull.Value;
                        cmd.Parameters.Add("p_telefon", OracleDbType.Varchar2).Value = telefon ?? (object)DBNull.Value;
                        cmd.Parameters.Add("p_datum_registrace", OracleDbType.Date).Value = registrationDate;
                        cmd.Parameters.Add("p_password", OracleDbType.Varchar2).Value = hashedPassword ?? (object)DBNull.Value;
                        cmd.Parameters.Add("p_salt", OracleDbType.Varchar2).Value = salt ?? (object)DBNull.Value;
                        cmd.Parameters.Add("p_role_id", OracleDbType.Int32).Value = roleId;
                        cmd.Parameters.Add("p_profile_obrazky_id", OracleDbType.Int32).Value = currentProfilePicture;

                        cmd.ExecuteNonQuery();
                    }

                    transaction.Commit();
                    MessageBox.Show("Uživatel úspěšně aktualizován!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadUsers();
                }
                catch (OracleException ex)
                {
                    transaction.Rollback();
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
    }
}
