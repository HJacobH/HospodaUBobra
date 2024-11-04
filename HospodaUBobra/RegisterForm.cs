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
    public partial class RegisterForm : Form
    {
        string connectionString = $"User Id=st69639;Password=Server2022;Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=fei-sql3.upceucebny.cz)(PORT=1521)))(CONNECT_DATA=(SID=BDAS)));";

        public RegisterForm()
        {
            InitializeComponent();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vyplňte všechna pole.");
                return;
            }

            if (UsernameExists(username))
            {
                MessageBox.Show("Uživatelské jméno již existuje, vyberte si jiné.");
                return;
            }

            if (RegisterUser(username, password, UserRole.User.ToString()))
            {
                MessageBox.Show("Registrace úspěšná!");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Registrace selhala. Zkuste to znovu.");
            }
        }

        private bool UsernameExists(string username)
        {
            try
            {
                using (OracleConnection conn = new OracleConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM Users WHERE username = :username";

                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    {
                        cmd.Parameters.Add(new OracleParameter("username", OracleDbType.Varchar2)).Value = username;

                        int userCount = Convert.ToInt32(cmd.ExecuteScalar());

                        return userCount > 0;
                    }
                }
            }
            catch (OracleException ex)
            {
                MessageBox.Show("Oracle chyba: " + ex.Message);
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Obecná chyba: " + ex.Message);
                return false;
            }
        }

        private bool RegisterUser(string username, string password, string role)
        {
            string salt = PasswordHelper.GenerateSalt();
            string hashedPassword = PasswordHelper.HashPassword(password, salt);

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();
                OracleTransaction transaction = conn.BeginTransaction();

                try
                {
                    string checkQuery = "SELECT COUNT(*) FROM Users WHERE username = :username";
                    using (OracleCommand checkCmd = new OracleCommand(checkQuery, conn))
                    {
                        checkCmd.Transaction = transaction;
                        checkCmd.Parameters.Add(new OracleParameter("username", OracleDbType.Varchar2)).Value = username;
                        int userCount = Convert.ToInt32(checkCmd.ExecuteScalar());

                        if (userCount > 0)
                        {
                            MessageBox.Show("Uživatelské jméno již existuje, vyberte si jiné.");
                            transaction.Rollback();
                            return false;
                        }
                    }

                    string insertQuery = "INSERT INTO Users (username, password, salt, role) VALUES (:username, :password, :salt, :role)";
                    using (OracleCommand cmd = new OracleCommand(insertQuery, conn))
                    {
                        cmd.Transaction = transaction;
                        cmd.Parameters.Add(new OracleParameter("username", OracleDbType.Varchar2)).Value = username;
                        cmd.Parameters.Add(new OracleParameter("password", OracleDbType.Varchar2)).Value = hashedPassword;
                        cmd.Parameters.Add(new OracleParameter("salt", OracleDbType.Varchar2)).Value = salt;
                        cmd.Parameters.Add(new OracleParameter("role", OracleDbType.Varchar2)).Value = role;

                        cmd.ExecuteNonQuery();
                    }

                    transaction.Commit();

                    LogUserAction("REGISTRACE", "Uživatel se registroval úspěšně.", username, role);

                    return true;
                }
                catch (OracleException ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("Oracle chyba: " + ex.Message);
                    return false;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("Obecná chyba: " + ex.Message);
                    return false;
                }
            }
        }



        private void LogUserAction(string actionType, string actionDesc, string userId, string role)
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO User_logs (ACTION_TYPE, ACTION_DESC, ACTION_DATE, USER_ID, ROLE) VALUES (:actionType, :actionDesc, :actionDate, :userId, :role)";

                using (OracleCommand cmd = new OracleCommand(query, conn))
                {
                    cmd.Parameters.Add(new OracleParameter("actionType", OracleDbType.Varchar2)).Value = actionType;
                    cmd.Parameters.Add(new OracleParameter("actionDesc", OracleDbType.Varchar2)).Value = actionDesc;
                    cmd.Parameters.Add(new OracleParameter("actionDate", OracleDbType.Date)).Value = DateTime.Now;
                    cmd.Parameters.Add(new OracleParameter("userId", OracleDbType.Varchar2)).Value = userId; // assuming userId corresponds to username
                    cmd.Parameters.Add(new OracleParameter("role", OracleDbType.Varchar2)).Value = role;

                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
