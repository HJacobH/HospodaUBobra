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
                UserSession.Username = username;
                UserSession.Role = userRole;
                LoggedInUserRole = userRole;
                currentUsername = username;
                LogUserAction("PRIHLASENI", "Uživatel se přihlásil úspěšně.", username, userRole.ToString());
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                LogUserAction("SELHANE_PRIHLASENI", "Nepovedený pokus přihlášení.", username, userRole.ToString());
                MessageBox.Show("Neplatné údaje. Přihlášení selhalo.");
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
                MessageBox.Show("Oracle chyba: " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Obecná chyba: " + ex.Message);
            }

            MessageBox.Show("Neplatné údaje. Přihlášení selhalo.");
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

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
