using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HospodaUBobra
{
    public partial class LoginForm : Form
    {
        string connectionString = $"User Id=st69639;Password=Server2022;Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=fei-sql3.upceucebny.cz)(PORT=1521)))(CONNECT_DATA=(SID=BDAS)));";

        public UserRole LoggedInUserRole { get; private set; } = UserRole.Anonymous;
        public string currentUsername { get; private set; } = "Anonymous";

        public LoginForm()
        {
            InitializeComponent();
            KeyPreview = true;
            KeyDown += (sender, e) =>
            {
                if (e.KeyCode == Keys.Escape)
                {
                    Close();
                }
            };

            KeyDown += (sender, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    btnLogin_Click(sender, e);
                }
            };
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (!ValidateInputs(username, password))
            {
                return;
            }

            if (ValidateLogin(username, password, out string roleName, out int userId, out string tableName))
            {
                UserSession.Role = roleName;
                UserSession.UserID = userId;
                UserSession.TableName = tableName; 

                this.Close();
            }
            else
            {
                MessageBox.Show("Neplatné přihlašovací údaje.", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateInputs(string username, string password)
        {
            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("Uživatelské jméno nesmí být prázdné.", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Heslo nesmí být prázdné.", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!IsValidEmail(username))
            {
                MessageBox.Show("Zadejte platnou e-mailovou adresu.", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private bool ValidateLogin(string username, string password, out string roleName, out int userId, out string tableName)
        {
            roleName = "Anonymous";
            userId = -1;
            tableName = string.Empty;

            try
            {
                using (OracleConnection conn = new OracleConnection(connectionString))
                {
                    conn.Open();

                    if (TryValidateLogin(conn, "UZIVATELE", username, password, out roleName, out userId))
                    {
                        tableName = "UZIVATELE";
                        return true;
                    }

                    if (TryValidateLogin(conn, "KLIENTI", username, password, out roleName, out userId))
                    {
                        tableName = "KLIENTI";
                        return true;
                    }

                    conn.Close();
                }
            }
            catch (OracleException ex)
            {
                MessageBox.Show("Oracle error: " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Obecný error: " + ex.Message);
            }

            return false;
        }

        private bool TryValidateLogin(OracleConnection conn, string tableName, string username, string password, out string roleName, out int userId)
        {
            roleName = "Anonymous";
            userId = -1;

            string idColumn = tableName == "UZIVATELE" ? "ID_UZIVATELE" : "ID_KLIENTA"; 
            string passwordColumn = tableName == "UZIVATELE" ? "PASSWORD" : "HESLO";
            string saltColumn = tableName == "UZIVATELE" ? "SALT" : "SUL"; 
            bool isUzivatele = tableName == "UZIVATELE";

            string query = isUzivatele
                ? $@"
            SELECT u.{passwordColumn}, u.{saltColumn}, r.ROLE_NAME, u.{idColumn}
            FROM {tableName} u
            JOIN Role r ON u.ROLE_ID = r.ROLE_ID
            WHERE u.EMAIL = :username"
                : $@"
            SELECT u.{passwordColumn}, u.{saltColumn}, r.ROLE_NAME, u.{idColumn}
            FROM {tableName} u
            JOIN Role r ON u.ROLE_ID = r.ROLE_ID
            WHERE u.EMAIL = :username";

            using (OracleCommand cmd = new OracleCommand(query, conn))
            {
                cmd.Parameters.Add(new OracleParameter("username", OracleDbType.Varchar2)).Value = username;

                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string storedHashedPassword = reader.GetString(0);
                        string salt = reader.GetString(1);
                        roleName = reader.GetString(2);
                        userId = reader.GetInt32(3);

                        string hashedInputPassword = HashPassword(password, salt);

                        if (hashedInputPassword == storedHashedPassword)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private string HashPassword(string password, string salt)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password + salt);
                byte[] hash = sha256.ComputeHash(bytes);

                StringBuilder result = new StringBuilder();
                foreach (byte b in hash)
                {
                    result.Append(b.ToString("x2"));
                }
                return result.ToString();
            }
        }
        private void btnBack_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
