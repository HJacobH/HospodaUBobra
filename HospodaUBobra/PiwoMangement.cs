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
    public partial class PiwoMangement : Form
    {
        string connectionString = $"User Id=st69639;Password=Server2022;Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=fei-sql3.upceucebny.cz)(PORT=1521)))(CONNECT_DATA=(SID=BDAS)));";

        private List<Tuple<int, string>> packagingOptions;
        private List<Tuple<int, string>> unitOptions;

        public PiwoMangement()
        {
            InitializeComponent();
            LoadPackagingOptions();
            LoadUnitOptions();
        }

        private void LoadPackagingOptions()
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT id_baleni, baleni FROM BALENI_PIV";
                using (OracleCommand cmd = new OracleCommand(query, conn))
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    packagingOptions = new List<Tuple<int, string>>();
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        packagingOptions.Add(new Tuple<int, string>(id, name));
                    }
                }
            }

            comboBoxPackaging.DataSource = packagingOptions;
            comboBoxPackaging.DisplayMember = "Item2";
            comboBoxPackaging.ValueMember = "Item1";
        }

        private void LoadUnitOptions()
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT id_jedn, jednotka FROM JEDNOTKY_PIV";
                using (OracleCommand cmd = new OracleCommand(query, conn))
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    unitOptions = new List<Tuple<int, string>>();
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        unitOptions.Add(new Tuple<int, string>(id, name));
                    }
                }
            }

            comboBoxUnit.DataSource = unitOptions;
            comboBoxUnit.DisplayMember = "Item2";
            comboBoxUnit.ValueMember = "Item1";
        }


        private void btnAddPiwo_Click(object sender, EventArgs e)
        {
            string nazev = txtBeerName.Text.Trim();
            decimal obsahAlkoholu;
            decimal objem;
            decimal cena;
            int pocetKsSkladem;
            int baleniPivaId = ((Tuple<int, string>)comboBoxPackaging.SelectedItem).Item1;
            int jednotkaPivaId = ((Tuple<int, string>)comboBoxUnit.SelectedItem).Item1;

            // Validate input
            if (string.IsNullOrEmpty(nazev) ||
                !decimal.TryParse(txtAlcoholContent.Text, out obsahAlkoholu) ||
                !decimal.TryParse(txtVolume.Text, out objem) ||
                !decimal.TryParse(txtPrice.Text, out cena) ||
                !int.TryParse(txtStockQuantity.Text, out pocetKsSkladem))
            {
                MessageBox.Show("Please enter valid values for all fields.");
                return;
            }

            // Call the stored procedure to insert the new beer
            InsertNewBeer(nazev, obsahAlkoholu, objem, cena, pocetKsSkladem, baleniPivaId, jednotkaPivaId);

        }

        private void InsertNewBeer(string nazev, decimal obsahAlkoholu, decimal objem, decimal cena, int pocetKsSkladem, int baleniPivaId, int jednotkaPivaId)
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();
                using (OracleCommand cmd = new OracleCommand("InsertNewBeer", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Add parameters
                    cmd.Parameters.Add(new OracleParameter("p_nazev", OracleDbType.Varchar2)).Value = nazev;
                    cmd.Parameters.Add(new OracleParameter("p_obsah_alkoholu", OracleDbType.Decimal)).Value = obsahAlkoholu;
                    cmd.Parameters.Add(new OracleParameter("p_objem", OracleDbType.Decimal)).Value = objem;
                    cmd.Parameters.Add(new OracleParameter("p_cena", OracleDbType.Decimal)).Value = cena;
                    cmd.Parameters.Add(new OracleParameter("p_pocet_ks_skladem", OracleDbType.Int32)).Value = pocetKsSkladem;
                    cmd.Parameters.Add(new OracleParameter("p_balenipiva_id_baleny", OracleDbType.Int32)).Value = baleniPivaId;
                    cmd.Parameters.Add(new OracleParameter("p_jednotkapiva_id_jedn", OracleDbType.Int32)).Value = jednotkaPivaId;

                    try
                    {
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Beer added successfully!");

                        LogUserAction("Add Beer", $"Added beer: {nazev}, Alcohol Content: {obsahAlkoholu}, Volume: {objem}, Price: {cena}, Stock: {pocetKsSkladem}");

                        this.Close();
                    }
                    catch (OracleException ex)
                    {
                        MessageBox.Show("Oracle error: " + ex.Message);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("General error: " + ex.Message);
                    }
                }
            }
        }

        private void LogUserAction(string actionType, string actionDesc)
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();
                string logQuery = "INSERT INTO User_logs (ACTION_TYPE, ACTION_DESC, ACTION_DATE, USER_ID, ROLE) VALUES (:actionType, :actionDesc, SYSDATE, :username, :role)";

                using (OracleCommand cmd = new OracleCommand(logQuery, conn))
                {
                    cmd.Parameters.Add(new OracleParameter("actionType", OracleDbType.Varchar2)).Value = actionType;
                    cmd.Parameters.Add(new OracleParameter("actionDesc", OracleDbType.Varchar2)).Value = actionDesc;
                    cmd.Parameters.Add(new OracleParameter("username", OracleDbType.Varchar2)).Value = UserSession.Username;
                    cmd.Parameters.Add(new OracleParameter("role", OracleDbType.Varchar2)).Value = UserSession.Role.ToString(); 

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
