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

        public LoginForm()
        {
            InitializeComponent();
            btnLogin.Click += new EventHandler(btnLogin_Click);
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (ValidateLogin(username, password, out Form1.UserRole userRole))
            {
                MessageBox.Show("Login successful!");
                Form1 mainForm = new Form1(userRole);
                mainForm.Show();
                this.Hide(); 
            }
            else
            {
                MessageBox.Show("Invalid credentials. Please try again.");
            }
        }


        private bool ValidateLogin(string username, string password, out Form1.UserRole role)
        {
            role = Form1.UserRole.Anonymous; 

            try
            {
                using (OracleConnection conn = new OracleConnection(connectionString))
                {
                    conn.Open();

                    string query = "SELECT role FROM Users WHERE username = :username AND password = :password";


                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    {
                        string trimmedUsername = username.Trim();
                        string trimmedPassword = password.Trim();

                        cmd.Parameters.Add(new OracleParameter("username", OracleDbType.Varchar2)).Value = trimmedUsername;
                        cmd.Parameters.Add(new OracleParameter("password", OracleDbType.Varchar2)).Value = trimmedPassword;

                        using (OracleDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            { 
                                string userRole = reader.GetString(0);

                                switch (userRole.ToLower())
                                {
                                    case "admin":
                                        role = Form1.UserRole.Admin;
                                        break;
                                    case "user":
                                        role = Form1.UserRole.User;
                                        break;
                                    case "anonymous":
                                        role = Form1.UserRole.Anonymous;
                                        break;
                                    default:
                                        MessageBox.Show($"Unknown role found: {userRole}");
                                        return false; 
                                }

                                return true; 
                            }
                            else
                            {
                                MessageBox.Show("No matching credentials found in the database.");
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

            MessageBox.Show("Login failed. Returning false.");
            return false; 
        }

    }
}
