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

            if (ValidateLogin(username, password, out string roleName))
            {
                UserSession.Username = username;
                UserSession.Role = roleName;

                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid credentials. Please try again.");
            }
        }


        private bool ValidateLogin(string username, string password, out string roleName)
        {
            roleName = "Anonymous"; 

            try
            {
                using (OracleConnection conn = new OracleConnection(connectionString))
                {
                    conn.Open();

                    string query = @"
                        SELECT u.PASSWORD, u.SALT, r.ROLE_NAME
                        FROM Users u
                        JOIN Role r ON u.ROLE_ID = r.ROLE_ID
                        WHERE u.USERNAME = :username";

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

                                string hashedInputPassword = HashPassword(password, salt);


                                if (hashedInputPassword == storedHashedPassword)
                                {
                                    return true;
                                }
                                else
                                {
                                    MessageBox.Show("Špatné heslo.");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Uživatel s tímto jménem nebyl nalezen.");
                            }
                        }
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
                MessageBox.Show("General error: " + ex.Message);
            }

            return false; 
        }


        private void LogUserAction(string actionType, string actionDesc, string username, string role)
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();

                string insertLogQuery = "INSERT INTO User_logs (ACTION_TYPE, ACTION_DESC, ACTION_DATE, USER_ID, ROLE) VALUES (:actionType, :actionDesc, :actionDate, :userId, :role)";
                using (OracleCommand cmd = new OracleCommand(insertLogQuery, conn))
                {
                    cmd.Parameters.Add(new OracleParameter("actionType", OracleDbType.Varchar2)).Value = actionType;
                    cmd.Parameters.Add(new OracleParameter("actionDesc", OracleDbType.Varchar2)).Value = actionDesc;
                    cmd.Parameters.Add(new OracleParameter("actionDate", OracleDbType.Date)).Value = DateTime.Now;
                    cmd.Parameters.Add(new OracleParameter("userId", OracleDbType.Varchar2)).Value = username;
                    cmd.Parameters.Add(new OracleParameter("role", OracleDbType.Varchar2)).Value = role;

                    cmd.ExecuteNonQuery();
                }

                conn.Close();
            }
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
