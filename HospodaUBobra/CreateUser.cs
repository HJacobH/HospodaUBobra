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
        }

        private void LoadRoles()
        {
            ComboBox comboBoxRole = this.Controls["comboBoxRole"] as ComboBox;

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

        private void btnCreate_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;
            string role = comboBoxRole.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(role))
            {
                MessageBox.Show("Vyplňte všechna pole.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string salt = PasswordHelper.GenerateSalt();
            string hashedPassword = PasswordHelper.HashPassword(password, salt);

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();

                using (OracleCommand cmd = new OracleCommand("InsertUser", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("p_username", OracleDbType.Varchar2).Value = username;
                    cmd.Parameters.Add("p_password", OracleDbType.Varchar2).Value = hashedPassword;
                    cmd.Parameters.Add("p_salt", OracleDbType.Varchar2).Value = salt;
                    cmd.Parameters.Add("p_role_name", OracleDbType.Varchar2).Value = role;

                    try
                    {
                        cmd.ExecuteNonQuery();
                        LogUserAction("Vytvoření uživatele", $"Uživatel '{username}' s rolí {role} vytvořen.", username, role);


                        MessageBox.Show("Uživatel úspěšně přidán!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                    catch (OracleException ex)
                    {
                        LogUserAction("Selhané vytvoření uživatele", $"Nepodařilo se vytvořit uživatele '{username}': {ex.Message}", username, role);

                        MessageBox.Show("Chyba při vytváření uživatele: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void LogUserAction(string actionType, string actionDesc, string username, string role = "Admin")
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
            }
        }


        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
