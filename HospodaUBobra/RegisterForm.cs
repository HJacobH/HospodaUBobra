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
                    btnRegister_Click(sender, e);
                }
            };
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs(out string errorMessage))
            {
                MessageBox.Show(errorMessage, "Chyba ověření vstupů", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string username = txtUsername.Text.Trim();
            string email = txtEmail.Text.Trim();
            string telefon = txtTelefon.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (UsernameExists(email))
            {
                MessageBox.Show("Uživatel s tímto emailem již existuje.", "Chyba registrace", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (RegisterUser(username, email, telefon, password, UserRole.User.ToString()))
            {
                MessageBox.Show("Registrace úspěšná!", "Úspěch", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show("Registrace selhala. Zkuste to znovu.", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool UsernameExists(string email)
        {
            try
            {
                using (OracleConnection conn = new OracleConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM UZIVATELE WHERE email = :email";

                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    {
                        cmd.Parameters.Add(new OracleParameter("email", OracleDbType.Varchar2)).Value = email;

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

        private bool RegisterUser(string username, string email, string telefon, string password, string role)
        {
            string salt = PasswordHelper.GenerateSalt();
            string hashedPassword = PasswordHelper.HashPassword(password, salt);

            try
            {
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
                        cmd.Parameters.Add("p_role_id", OracleDbType.Int32).Value = 3;
                        cmd.Parameters.Add("p_profile_picture", OracleDbType.Int32).Value = DBNull.Value;

                        cmd.ExecuteNonQuery();

                        return true;
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

        private bool ValidateInputs(out string errorMessage)
        {
            errorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                errorMessage = "Zadejte prosím uživatelské jméno.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtEmail.Text) || !txtEmail.Text.Contains("@"))
            {
                errorMessage = "Zadejte prosím platný email.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtTelefon.Text) || txtTelefon.Text.Length < 9 || !txtTelefon.Text.All(char.IsDigit))
            {
                errorMessage = "Zadejte prosím platné telefonní číslo (minimálně 9 číslic).";
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text) || txtPassword.Text.Length < 8)
            {
                errorMessage = "Zadejte prosím heslo (minimálně 8 znaků).";
                return false;
            }

            return true;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
