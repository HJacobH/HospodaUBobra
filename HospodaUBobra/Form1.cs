using Oracle.ManagedDataAccess.Client;
using System.Data;
using System;
using System.Windows.Forms;

namespace HospodaUBobra
{
    public partial class Form1 : Form
    {
        string st = "";
        string heslo = "";
        string connectionString;

        public enum UserRole
        {
            Admin,
            User,
            Anonymous
        }

        private UserRole currentRole;

        private Dictionary<UserRole, List<string>> roleTables;

        public Form1(UserRole role)
        {
            InitializeComponent();
            comboBoxTables.SelectedIndexChanged += new EventHandler(comboBoxTables_SelectedIndexChanged);
            connectionString = $"User Id={st};Password={heslo};Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=fei-sql3.upceucebny.cz)(PORT=1521)))(CONNECT_DATA=(SID=BDAS)));";

            currentRole = role;
            roleTables = new Dictionary<UserRole, List<string>>
            {
                { UserRole.Admin, null },
                { UserRole.User, new List<string> { "PIVA", "PIVOVARY", "KRAJE" } },
                { UserRole.Anonymous, new List<string> { "OKRESY" } }
            };

            roleLabel.Text = role.ToString();

            PopulateTableList();
        }

        private void PopulateTableList()
        {
            try
            {
                using (OracleConnection conn = new OracleConnection(connectionString))
                {
                    conn.Open();

                    string query = $"SELECT table_name FROM all_tables WHERE owner = UPPER('{st}')";

                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    {
                        using (OracleDataReader reader = cmd.ExecuteReader())
                        {
                            comboBoxTables.Items.Clear();

                            while (reader.Read())
                            {
                                string tableName = reader.GetString(0);

                                if (CanAccessTable(tableName))
                                {
                                    comboBoxTables.Items.Add(tableName);
                                }
                            }
                        }
                    }

                    conn.Close();
                }
            }
            catch (OracleException ex)
            {
                Log("Oracle error while fetching table names: " + ex.Message);
                MessageBox.Show("Oracle error: " + ex.Message);
            }
            catch (Exception ex)
            {
                Log("General error: " + ex.Message);
                MessageBox.Show("General error: " + ex.Message);
            }
        }

        private bool CanAccessTable(string tableName)
        {
            if (currentRole == UserRole.Admin)
            {
                return true;
            }

            if (roleTables.ContainsKey(currentRole) && roleTables[currentRole] != null)
            {
                return roleTables[currentRole].Contains(tableName);
            }

            return false;
        }

        private void comboBoxTables_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedTable = comboBoxTables.SelectedItem.ToString();
            LoadTableData(selectedTable);
        }

        private void LoadTableData(string tableName)
        {
            try
            {
                using (OracleConnection conn = new OracleConnection(connectionString))
                {
                    conn.Open();

                    string query = $"SELECT * FROM {tableName}";

                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    {
                        using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                        {
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);

                            dataGridView1.DataSource = dataTable;
                        }
                    }

                    conn.Close();
                }
            }
            catch (OracleException ex)
            {
                Log("Oracle error: " + ex.Message);
                MessageBox.Show("Oracle error: " + ex.Message);
            }
            catch (Exception ex)
            {
                Log("General error: " + ex.Message);
                MessageBox.Show("General error: " + ex.Message);
            }
        }

        private void Log(string message)
        {
            MessageBox.Show(DateTime.Now + ": " + message);
        }

        private void btnPrihlasit_Click(object sender, EventArgs e)
        {
            LoginForm loginForm = new LoginForm();
            loginForm.ShowDialog();
            this.Hide();
        }
    }
}