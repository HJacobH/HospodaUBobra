using Oracle.ManagedDataAccess.Client;
using System.Data;
using System;
using System.Windows.Forms;



namespace HospodaUBobra
{
    public partial class Form1 : Form
    {
        string connectionString = "User Id=st69607;Password=;Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=fei-sql3.upceucebny.cz)(PORT=1521)))(CONNECT_DATA=(SID=BDAS)));";

        public Form1()
        {
            InitializeComponent();
            Log("Zacatek");
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                Log("Attempting to connect to the Oracle database...");

                using (OracleConnection conn = new OracleConnection(connectionString))
                {
                    Log("Opening Oracle connection...");

                    conn.Open();

                    Log("Successfully connected to the Oracle database.");

                    using (OracleCommand cmd = new OracleCommand("SELECT * FROM pracovnik", conn))
                    {
                        Log("Executing SQL query: SELECT * FROM pracovnik");

                        using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                        {
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);

                            Log("Data retrieved successfully.");

                            dataGridView1.DataSource = dataTable;

                            Log("DataGridView updated with data.");
                        }
                    }

                    conn.Close();

                    Log("Oracle connection closed.");
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
            MessageBox.Show(message);
        }
    }
}
