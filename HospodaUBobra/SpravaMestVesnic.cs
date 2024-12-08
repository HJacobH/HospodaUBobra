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
    public partial class SpravaMestVesnic : Form
    {
        string connectionString = $"User Id=st69639;Password=Server2022;Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=fei-sql3.upceucebny.cz)(PORT=1521)))(CONNECT_DATA=(SID=BDAS)));";

        private int selectedMestoId = -1;

        public SpravaMestVesnic()
        {
            InitializeComponent();
            LoadMestaVesniceData();
            LoadKraje();
            LoadOkresy();

            dgvMestaVesnice.ReadOnly = true;
            cbKraje.DropDownStyle = ComboBoxStyle.DropDownList;
            cbOkresy.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void LoadMestaVesniceData()
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    string query = @"SELECT * FROM A_DGV_MESTA_VESNICE";

                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        DataGridViewFilterHelper.BindData(dgvMestaVesnice, dataTable);
                        dgvMestaVesnice.DataSource = dataTable;

                        if (dgvMestaVesnice.Columns.Contains("Town_ID"))
                        {
                            dgvMestaVesnice.Columns["Town_ID"].Visible = false;
                        }

                        dgvMestaVesnice.Columns["Name"].HeaderText = "Název";
                        dgvMestaVesnice.Columns["Population"].HeaderText = "Počet obyvatel";
                        dgvMestaVesnice.Columns["Postal_Code"].HeaderText = "PSČ";
                        dgvMestaVesnice.Columns["District"].HeaderText = "Okres";
                        dgvMestaVesnice.Columns["Region"].HeaderText = "Kraj";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Chyba při načítání měst a vesnic: " + ex.Message);
                }
            }
        }

        private void LoadKraje()
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT * FROM A_CB_KRAJE";

                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        List<Tuple<int, string>> kraje = new List<Tuple<int, string>>();
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            string name = reader.GetString(1);
                            kraje.Add(new Tuple<int, string>(id, name));
                        }

                        cbKraje.DataSource = kraje;
                        cbKraje.DisplayMember = "Item2";
                        cbKraje.ValueMember = "Item1";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Chyba při načítání krajů: " + ex.Message);
                }
            }
        }

        private void LoadOkresy()
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT * FROM A_CB_OKRESY";

                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        List<Tuple<int, string>> okresy = new List<Tuple<int, string>>();
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            string name = reader.GetString(1);
                            okresy.Add(new Tuple<int, string>(id, name));
                        }

                        cbOkresy.DataSource = okresy;
                        cbOkresy.DisplayMember = "Item2";
                        cbOkresy.ValueMember = "Item1";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Chyba při načítání okresů: " + ex.Message);
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs())
            {
                return;
            }

            int pocetObyvatel = int.Parse(txtPocetObyvatel.Text);
            int okresId = ((Tuple<int, string>)cbOkresy.SelectedItem).Item1;
            int krajId = ((Tuple<int, string>)cbKraje.SelectedItem).Item1;

            ManageMestaVesnice(null, txtNazev.Text, pocetObyvatel, txtPSC.Text, okresId, krajId);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedMestoId == -1)
            {
                MessageBox.Show("Vyberte záznam pro aktualizaci.", "Chyba aktualizace", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidateInputs())
            {
                return;
            }

            int pocetObyvatel = int.Parse(txtPocetObyvatel.Text);
            int okresId = ((Tuple<int, string>)cbOkresy.SelectedItem).Item1;
            int krajId = ((Tuple<int, string>)cbKraje.SelectedItem).Item1;

            ManageMestaVesnice(selectedMestoId, txtNazev.Text, pocetObyvatel, txtPSC.Text, okresId, krajId);

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedMestoId == -1)
            {
                MessageBox.Show("Prosím vyberte město/vesnici pro odstranění.");
                return;
            }

            DialogResult result = MessageBox.Show("Opravdu chcete odstranit toto město/vesnici?", "Potvrdit odstranění", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                using (OracleConnection conn = new OracleConnection(connectionString))
                {
                    try
                    {
                        conn.Open();
                        string query = "DELETE FROM MESTA_VESNICE WHERE ID_MES_VES = :id_mes_ves";

                        using (OracleCommand cmd = new OracleCommand(query, conn))
                        {
                            cmd.Parameters.Add(new OracleParameter(":id_mes_ves", OracleDbType.Int32)).Value = selectedMestoId;
                            cmd.ExecuteNonQuery();

                            MessageBox.Show("Město/Vesnice odstraněna úspěšně!");
                            LoadMestaVesniceData();
                            ClearFormFields();
                            selectedMestoId = -1;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Chyba pří odstranění: " + ex.Message);
                    }
                }
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFormFields();
        }

        private void dgvMestaVesnice_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvMestaVesnice.CurrentRow != null && dgvMestaVesnice.CurrentRow.Cells["Town_ID"].Value != DBNull.Value)
            {
                selectedMestoId = dgvMestaVesnice.CurrentRow.Cells["Town_ID"].Value != DBNull.Value
                    ? Convert.ToInt32(dgvMestaVesnice.CurrentRow.Cells["Town_ID"].Value)
                    : -1;

                txtNazev.Text = dgvMestaVesnice.CurrentRow.Cells["Name"].Value?.ToString() ?? string.Empty;
                txtPocetObyvatel.Text = dgvMestaVesnice.CurrentRow.Cells["Population"].Value?.ToString() ?? string.Empty;
                txtPSC.Text = dgvMestaVesnice.CurrentRow.Cells["Postal_Code"].Value?.ToString() ?? string.Empty;

                cbOkresy.SelectedIndex = cbOkresy.FindStringExact(dgvMestaVesnice.CurrentRow.Cells["District"].Value?.ToString() ?? string.Empty);
                cbKraje.SelectedIndex = cbKraje.FindStringExact(dgvMestaVesnice.CurrentRow.Cells["Region"].Value?.ToString() ?? string.Empty);
            }
            else
            {
                ClearFormFields();
            }
        }

        private void ManageMestaVesnice(int? idMesVes, string nazev, int pocetObyvatel, string psc, int okresId, int krajId)
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();
                using (OracleCommand cmd = new OracleCommand("sprava_procedures_pkg.sprava_mesta_vesnice", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new OracleParameter("p_identifikator", OracleDbType.Int32)).Value = idMesVes.HasValue ? idMesVes : DBNull.Value;
                    cmd.Parameters.Add(new OracleParameter("p_id_mes_ves", OracleDbType.Int32)).Value = idMesVes;
                    cmd.Parameters.Add(new OracleParameter("p_nazev", OracleDbType.Varchar2)).Value = nazev;
                    cmd.Parameters.Add(new OracleParameter("p_pocet_obyvatel", OracleDbType.Int32)).Value = pocetObyvatel;
                    cmd.Parameters.Add(new OracleParameter("p_psc", OracleDbType.Varchar2)).Value = psc;
                    cmd.Parameters.Add(new OracleParameter("p_okres_id_okresu", OracleDbType.Int32)).Value = okresId;
                    cmd.Parameters.Add(new OracleParameter("p_kraj_id_kraje", OracleDbType.Int32)).Value = krajId;

                    try
                    {
                        cmd.ExecuteNonQuery();
                        string message = idMesVes.HasValue ? "Město/Vesnice aktualizována úspěšně!" : "Město/Vesnice úspěšně přidána!";
                        MessageBox.Show(message);
                        LoadMestaVesniceData();
                        ClearFormFields();
                        selectedMestoId = -1;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Chyba: " + ex.Message);
                    }
                }
            }
        }

        private void ClearFormFields()
        {
            txtNazev.Clear();
            txtPocetObyvatel.Clear();
            txtPSC.Clear();
            cbOkresy.SelectedIndex = -1;
            cbKraje.SelectedIndex = -1;
        }

        private bool ValidateInputs()
        {
            StringBuilder errorMessage = new StringBuilder();

            if (string.IsNullOrWhiteSpace(txtNazev.Text))
            {
                errorMessage.AppendLine("Zadejte platný název města nebo vesnice.");
            }

            if (!int.TryParse(txtPocetObyvatel.Text, out int population) || population <= 0)
            {
                errorMessage.AppendLine("Zadejte platný počet obyvatel (kladné celé číslo).");
            }

            if (string.IsNullOrWhiteSpace(txtPSC.Text) || !txtPSC.Text.All(char.IsDigit))
            {
                errorMessage.AppendLine("Zadejte platné PSČ (pouze čísla).");
            }

            if (cbOkresy.SelectedIndex == -1)
            {
                errorMessage.AppendLine("Vyberte platný okres.");
            }

            if (cbKraje.SelectedIndex == -1)
            {
                errorMessage.AppendLine("Vyberte platný kraj.");
            }

            if (errorMessage.Length > 0)
            {
                MessageBox.Show(errorMessage.ToString(), "Chyba validace", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            DataGridViewFilterHelper.FilterData(dgvMestaVesnice, txtSearch);
        }
    }
}
