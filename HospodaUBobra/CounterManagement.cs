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
    public partial class CounterManagement : Form
    {
        string connectionString = $"User Id=st69639;Password=Server2022;Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=fei-sql3.upceucebny.cz)(PORT=1521)))(CONNECT_DATA=(SID=BDAS)));";
        private string selectedTable;

        public CounterManagement()
        {
            InitializeComponent();

            LoadTableNames();
        }

        private void LoadTableNames()
        {
            List<string> tables = new List<string>
    {
        "BALENI_PIV",
        "DRUHY_PODNIKU",
        "JEDNOTKY_OBJ",
        "JEDNOTKY_PIV",
        "KRAJE",
        "OKRESY",
        "POZICE_PRACOVNIKA",
        "PRAVNICKE_OSOBY",
        "ROLE",
        "STAVY_OBJEDNAVEK"
    };

            cbTables.DataSource = tables;
        }


        private void buttonBack_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void cbTables_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedTable = cbTables.SelectedItem.ToString();
            LoadTableData(selectedTable);
        }

        private void LoadTableData(string tableName)
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();
                string query = $"SELECT * FROM {tableName}";

                using (OracleCommand cmd = new OracleCommand(query, conn))
                {
                    using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dgvTableData.DataSource = dt;
                    }
                }
            }

            lblStatus.Text = $"Aktuální číselník: {tableName}";
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtAktualni.Text))
            {
                MessageBox.Show("Vyplňte hodnotu pro přidání.", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();

                using (OracleCommand cmd = new OracleCommand($"SPRAVA_{selectedTable}", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("p_identifikator", OracleDbType.Int32).Value = DBNull.Value; 
                    cmd.Parameters.Add("p_id", OracleDbType.Int32).Value = GetNextIdForTable(selectedTable);
                    cmd.Parameters.Add("p_value", OracleDbType.Varchar2).Value = txtAktualni.Text;

                    cmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Hodnota byla úspěšně přidána.");
            LoadTableData(selectedTable);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvTableData.CurrentRow == null || string.IsNullOrEmpty(txtAktualni.Text))
            {
                MessageBox.Show("Vyberte řádek k aktualizaci a vyplňte hodnotu.", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = Convert.ToInt32(dgvTableData.CurrentRow.Cells[0].Value);

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();

                using (OracleCommand cmd = new OracleCommand($"SPRAVA_{selectedTable}", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("p_identifikator", OracleDbType.Int32).Value = 1; 
                    cmd.Parameters.Add("p_id", OracleDbType.Int32).Value = id;
                    cmd.Parameters.Add("p_value", OracleDbType.Varchar2).Value = txtAktualni.Text;

                    cmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Hodnota byla úspěšně aktualizována.");
            LoadTableData(selectedTable);
        }

        private int GetNextIdForTable(string tableName)
        {
            int nextId = 0;

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();

                string primaryKeyColumn = GetPrimaryKeyColumnForTable(tableName);

                string query = $"SELECT NVL(MAX({primaryKeyColumn}), 0) + 1 FROM {tableName}";

                using (OracleCommand cmd = new OracleCommand(query, conn))
                {
                    object result = cmd.ExecuteScalar();
                    nextId = Convert.ToInt32(result);
                }
            }

            return nextId;
        }

        private string GetPrimaryKeyColumnForTable(string tableName)
        {
            Dictionary<string, string> tablePrimaryKeyMap = new Dictionary<string, string>
            {
                { "BALENI_PIV", "ID_BALENI" },
                { "DRUHY_PODNIKU", "ID_DRUHU" },
                { "JEDNOTKY_OBJ", "ID_JEDN_OBJ" },
                { "JEDNOTKY_PIV", "ID_JEDN" },
                { "KRAJE", "ID_KRAJE" },
                { "MESTA_VESNICE", "ID_MES_VES" },
                { "OKRESY", "ID_OKRESU" },
                { "POZICE_PRACOVNIKA", "ID_POZICE" },
                { "PRAVNICKE_OSOBY", "ID" },
                { "ROLE", "ROLE_ID" },
                { "STAVY_OBJEDNAVEK", "ID_STAVU" }
            };

            if (tablePrimaryKeyMap.ContainsKey(tableName))
            {
                return tablePrimaryKeyMap[tableName];
            }

            throw new ArgumentException($"Primary key column not defined for table: {tableName}");
        }


        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvTableData.CurrentRow == null)
            {
                MessageBox.Show("Vyberte řádek, který chcete odstranit.", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string selectedTable = cbTables.SelectedItem.ToString();
            string primaryKeyColumn = GetPrimaryKeyColumnForTable(selectedTable);

            object primaryKeyValue = dgvTableData.CurrentRow.Cells[primaryKeyColumn].Value;

            if (primaryKeyValue == null)
            {
                MessageBox.Show("Nelze odstranit tento řádek, protože nemá platný primární klíč.", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult confirmResult = MessageBox.Show(
                $"Opravdu chcete odstranit tento řádek z tabulky {selectedTable}?",
                "Potvrdit odstranění",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirmResult != DialogResult.Yes)
            {
                return;
            }

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string deleteQuery = $"DELETE FROM {selectedTable} WHERE {primaryKeyColumn} = :primaryKeyValue";

                    using (OracleCommand cmd = new OracleCommand(deleteQuery, conn))
                    {
                        cmd.Parameters.Add(new OracleParameter("primaryKeyValue", primaryKeyValue));
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Řádek byl úspěšně odstraněn!", "Úspěch", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LoadTableData(selectedTable);
                }
                catch (OracleException ex)
                {
                    MessageBox.Show($"Chyba při mazání řádku: {ex.Message}", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dgvTableData_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvTableData.CurrentRow != null)
            {
                var selectedValue = dgvTableData.CurrentRow.Cells[1].Value.ToString();
                txtAktualni.Text = selectedValue;
            }
        }
    }
}
