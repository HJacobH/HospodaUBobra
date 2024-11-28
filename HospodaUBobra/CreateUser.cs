using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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
            LoadRoles();
            LoadUsers();
        }

        private void LoadRoles()
        {
            comboBoxRole.Items.Clear();
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT ROLE_NAME FROM ROLE WHERE ROLE_NAME != 'Anonymous'";

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
                string query = "SELECT ID_UZIVATELE, UZIVATELSKE_JMENO, EMAIL, TELEFON, DATUM_REGISTRACE, ROLE_ID FROM UZIVATELE";

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
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string email = txtEmail.Text.Trim();
            string telefon = txtTelefon.Text.Trim();
            string password = txtPassword.Text;
            string role = comboBoxRole.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(telefon) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(role))
            {
                MessageBox.Show("Vyplňte všechna pole.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            switch (roleName.ToLower())
            {
                case "admin":
                    return 1;
                case "klient":
                    return 2;
                case "user":
                    return 3;
                default:
                    throw new ArgumentException("Neplatná role: " + roleName);
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

            // Fetch existing user data from the database to preserve missing fields
            string currentPassword = null;
            string currentSalt = null;
            DateTime? currentRegistrationDate = null;

            byte[] currentProfilePicture = null; // Ensure currentProfilePicture is defined

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT PASSWORD, SALT, DATUM_REGISTRACE FROM UZIVATELE WHERE ID_UZIVATELE = :userId";

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

                // Load profile picture from the PROFILOVE_OBRAZKY table
                query = "SELECT PICTURE FROM PROFILOVE_OBRAZKY WHERE ID_UZIVATELE = :userId";

                using (OracleCommand cmd = new OracleCommand(query, conn))
                {
                    cmd.Parameters.Add(new OracleParameter("userId", OracleDbType.Int32)).Value = userId;

                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            currentProfilePicture = reader["PICTURE"] as byte[];
                        }
                    }
                }
            }

            // Collect updated data from the UI
            string username = txtUsername.Text.Trim();
            string email = txtEmail.Text.Trim();
            string telefon = txtTelefon.Text.Trim();
            string password = txtPassword.Text; // Optional: leave blank if not updating
            int roleId = GetRoleId(comboBoxRole.SelectedItem?.ToString());

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(telefon))
            {
                MessageBox.Show("Vyplňte všechna pole.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Use the provided password or keep the current password and salt
            string salt = string.IsNullOrEmpty(password) ? currentSalt : PasswordHelper.GenerateSalt();
            string hashedPassword = string.IsNullOrEmpty(password) ? currentPassword : PasswordHelper.HashPassword(password, salt);

            // Preserve existing registration date
            DateTime registrationDate = currentRegistrationDate ?? DateTime.Now;

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();
                OracleTransaction transaction = conn.BeginTransaction();

                try
                {
                    // Call sprava_uzivatele procedure
                    using (OracleCommand cmd = new OracleCommand("sprava_uzivatele", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("p_identifikator", OracleDbType.Int32).Value = 1; // Update
                        cmd.Parameters.Add("p_id_uzivatele", OracleDbType.Int32).Value = userId;
                        cmd.Parameters.Add("p_uzivatelske_jmeno", OracleDbType.Varchar2).Value = username;
                        cmd.Parameters.Add("p_email", OracleDbType.Varchar2).Value = email;
                        cmd.Parameters.Add("p_telefon", OracleDbType.Varchar2).Value = telefon;
                        cmd.Parameters.Add("p_datum_registrace", OracleDbType.Date).Value = registrationDate;
                        cmd.Parameters.Add("p_password", OracleDbType.Varchar2).Value = hashedPassword;
                        cmd.Parameters.Add("p_salt", OracleDbType.Varchar2).Value = salt;
                        cmd.Parameters.Add("p_role_id", OracleDbType.Int32).Value = roleId;
                        cmd.Parameters.Add("p_profile_picture", OracleDbType.Int32).Value = DBNull.Value; // Profile picture handled separately

                        cmd.ExecuteNonQuery();
                    }

                    // Handle profile picture update using sprava_profilove_obrazky
                    if (currentProfilePicture != null)
                    {
                        using (OracleCommand profileCmd = new OracleCommand("sprava_profilove_obrazky", conn))
                        {
                            profileCmd.CommandType = CommandType.StoredProcedure;

                            profileCmd.Parameters.Add("p_identifikator", OracleDbType.Int32).Value = 1; // Update
                            profileCmd.Parameters.Add("p_id_picture", OracleDbType.Int32).Value = userId; // Assuming profile picture ID matches user ID
                            profileCmd.Parameters.Add("p_id_uzivatele", OracleDbType.Int32).Value = userId;
                            profileCmd.Parameters.Add("p_picture", OracleDbType.Blob).Value = currentProfilePicture;
                            profileCmd.Parameters.Add("p_file_name", OracleDbType.Varchar2).Value = "profile_pic.jpg"; // Example
                            profileCmd.Parameters.Add("p_file_type", OracleDbType.Varchar2).Value = "image/jpeg"; // Example
                            profileCmd.Parameters.Add("p_file_extension", OracleDbType.Varchar2).Value = ".jpg"; // Example
                            profileCmd.Parameters.Add("p_upload_date", OracleDbType.Date).Value = DateTime.Now;

                            profileCmd.ExecuteNonQuery();
                        }
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
                comboBoxRole.SelectedIndex = Convert.ToInt32(dataGridViewUsers.CurrentRow.Cells["ROLE_ID"].Value) - 1;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {

        }
    }
}
