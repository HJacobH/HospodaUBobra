using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
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
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();
            UserRole userRole = UserRole.Anonymous;

            if (ValidateLogin(username, password, out userRole))
            {
                LoggedInUserRole = userRole;  // Set the user role
                currentUsername = username;
                this.DialogResult = DialogResult.OK;  // Signal that login succeeded
                this.Close(); // Close LoginForm
            }
            else
            {
                MessageBox.Show("Invalid credentials. Please try again.");
            }
        }

        private bool ValidateLogin(string username, string password, out UserRole role)
        {
            role = UserRole.Anonymous;

            try
            {
                using (OracleConnection conn = new OracleConnection(connectionString))
                {
                    conn.Open();

                    string query = "SELECT password, salt, role FROM Users WHERE username = :username";

                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    {
                        cmd.Parameters.Add(new OracleParameter("username", OracleDbType.Varchar2)).Value = username;

                        using (OracleDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string storedHashedPassword = reader.GetString(0);
                                string salt = reader.GetString(1);
                                string userRole = reader.GetString(2);

                                // Hash the input password with the stored salt
                                string hashedInputPassword = PasswordHelper.HashPassword(password, salt);

                                if (hashedInputPassword == storedHashedPassword)
                                {
                                    role = Enum.Parse<UserRole>(userRole, true);
                                    return true;
                                }
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

            MessageBox.Show("Invalid credentials. Login failed.");
            return false;
        }


    }
}
