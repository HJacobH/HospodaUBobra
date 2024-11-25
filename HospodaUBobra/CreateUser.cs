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

                    // Parameters for the sprava_uzivatele procedure
                    cmd.Parameters.Add("p_identifikator", OracleDbType.Int32).Value = DBNull.Value; 
                    cmd.Parameters.Add("p_id_uzivatele", OracleDbType.Int32).Value = GetNextUserId(); 
                    cmd.Parameters.Add("p_uzivatelske_jmeno", OracleDbType.Varchar2).Value = username;
                    cmd.Parameters.Add("p_email", OracleDbType.Varchar2).Value =email; 
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
                        this.Close();
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
    }
}
