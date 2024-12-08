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
    public partial class SpravaPracovniku : Form
    {
        string connectionString = $"User Id=st69639;Password=Server2022;Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=fei-sql3.upceucebny.cz)(PORT=1521)))(CONNECT_DATA=(SID=BDAS)));";

        public SpravaPracovniku()
        {
            InitializeComponent();

            dataGridViewPracovnici.ReadOnly = true;
            cbPivovary.DropDownStyle = ComboBoxStyle.DropDownList;
            cbZamestnanci.DropDownStyle = ComboBoxStyle.DropDownList;

            LoadData();
        }

        private void LoadData()
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    string zamestnanciQuery = "SELECT * FROM A_CB_JMENO_PRIJMENI_ZAMESTNANCI";
                    using (OracleDataAdapter adapter = new OracleDataAdapter(zamestnanciQuery, conn))
                    {
                        DataTable zamestnanciTable = new DataTable();
                        adapter.Fill(zamestnanciTable);
                        cbZamestnanci.DataSource = zamestnanciTable;
                        cbZamestnanci.DisplayMember = "FULL_NAME";
                        cbZamestnanci.ValueMember = "ID_ZAMESTNANCE";
                    }

                    string pivovaryQuery = "SELECT * FROM A_CB_NAZEV_PIVOVARU";
                    using (OracleDataAdapter adapter = new OracleDataAdapter(pivovaryQuery, conn))
                    {
                        DataTable pivovaryTable = new DataTable();
                        adapter.Fill(pivovaryTable);
                        cbPivovary.DataSource = pivovaryTable;
                        cbPivovary.DisplayMember = "NAZEV";
                        cbPivovary.ValueMember = "ID_PIVOVARU";
                    }

                    string pracovniciQuery = @"SELECT * FROM A_DGV_PRACOVNICI";
                    using (OracleDataAdapter adapter = new OracleDataAdapter(pracovniciQuery, conn))
                    {
                        DataTable pracovniciTable = new DataTable();
                        adapter.Fill(pracovniciTable);
                        DataGridViewFilterHelper.BindData(dataGridViewPracovnici, pracovniciTable);
                        dataGridViewPracovnici.DataSource = pracovniciTable;

                        if (dataGridViewPracovnici.Columns["ID"] != null)
                        {
                            dataGridViewPracovnici.Columns["ID"].Visible = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dataGridViewPracovnici_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewPracovnici.CurrentRow != null)
            {
                DataGridViewRow selectedRow = dataGridViewPracovnici.CurrentRow;

                string selectedPivovar = selectedRow.Cells["Pivovar"].Value.ToString();
                string selectedZamestnanec = selectedRow.Cells["Zamestnanec"].Value.ToString();

                cbPivovary.SelectedIndex = cbPivovary.FindStringExact(selectedPivovar);
                cbZamestnanci.SelectedIndex = cbZamestnanci.FindStringExact(selectedZamestnanec);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (cbZamestnanci.SelectedValue == null || cbPivovary.SelectedValue == null)
            {
                MessageBox.Show("Prosím vyberte zaměstnance a pivovar.", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int zamestnanecId = Convert.ToInt32(cbZamestnanci.SelectedValue);
            int pivovarId = Convert.ToInt32(cbPivovary.SelectedValue);

            if (EmployeeAlreadyWorksAtBrewery(zamestnanecId, pivovarId))
            {
                MessageBox.Show("Tento zaměstnanec již pracuje v tomto pivovaru.", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    using (OracleCommand cmd = new OracleCommand("sprava_pracovnici", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("p_identifikator", OracleDbType.Int32).Value = DBNull.Value; 
                        cmd.Parameters.Add("p_id", OracleDbType.Int32).Value = GenerateNewId(conn); 
                        cmd.Parameters.Add("p_pivovar_id", OracleDbType.Int32).Value = pivovarId;
                        cmd.Parameters.Add("p_zamestnanec_id", OracleDbType.Int32).Value = zamestnanecId;

                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Nové přiřazení zaměstnance k pivovaru bylo úspěšně přidáno.", "Úspěch", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        LoadData(); 
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private bool EmployeeAlreadyWorksAtBrewery(int zamestnanecId, int pivovarId)
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    string checkQuery = @"
                SELECT COUNT(*) 
                FROM PRACOVNICI 
                WHERE ZAMESTNANEC_ID_ZAMESTNANCE = :zamestnanecId
                  AND PIVOVAR_ID_PIVOVARU = :pivovarId";

                    using (OracleCommand cmd = new OracleCommand(checkQuery, conn))
                    {
                        cmd.Parameters.Add("zamestnanecId", OracleDbType.Int32).Value = zamestnanecId;
                        cmd.Parameters.Add("pivovarId", OracleDbType.Int32).Value = pivovarId;

                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        return count > 0; 
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Chyba při kontrole zda pracovník již pracuje v pivovaru: " + ex.Message, "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return true; 
                }
            }
        }


        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (cbZamestnanci.SelectedValue == null || cbPivovary.SelectedValue == null)
            {
                MessageBox.Show("Prosím vyberte zaměstnance a pivovar.", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int zamestnanecId = Convert.ToInt32(cbZamestnanci.SelectedValue);
            int pivovarId = Convert.ToInt32(cbPivovary.SelectedValue);

            if (EmployeeAlreadyWorksAtBrewery(zamestnanecId, pivovarId))
            {
                MessageBox.Show("Tento zaměstnanec již pracuje v tomto pivovaru.", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    using (OracleCommand cmd = new OracleCommand("sprava_pracovnici", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        int? selectedRowId = GetSelectedRowId();
                        cmd.Parameters.Add("p_identifikator", OracleDbType.Int32).Value = selectedRowId.HasValue ? selectedRowId : (object)DBNull.Value;
                        cmd.Parameters.Add("p_id", OracleDbType.Int32).Value = selectedRowId ?? GenerateNewId(conn);
                        cmd.Parameters.Add("p_pivovar_id", OracleDbType.Int32).Value = cbPivovary.SelectedValue;
                        cmd.Parameters.Add("p_zamestnanec_id", OracleDbType.Int32).Value = cbZamestnanci.SelectedValue;

                        cmd.ExecuteNonQuery();

                        if (selectedRowId.HasValue)
                        {
                            MessageBox.Show("Pracovník úspěšně aktualizován.", "Úspěch", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Nový pracovník úspěšně přidán.", "Úspěch", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                        LoadData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private int? GetSelectedRowId()
        {
            if (dataGridViewPracovnici.CurrentRow != null && dataGridViewPracovnici.Columns["ID"] != null)
            {
                DataGridViewRow selectedRow = dataGridViewPracovnici.CurrentRow;
                if (selectedRow.Cells["ID"] != null && selectedRow.Cells["ID"].Value != DBNull.Value)
                {
                    return Convert.ToInt32(selectedRow.Cells["ID"].Value);
                }
            }
            return null;
        }

        private int GenerateNewId(OracleConnection conn)
        {
            string query = "SELECT NVL(MAX(ID), 0) + 1 FROM PRACOVNICI";
            using (OracleCommand cmd = new OracleCommand(query, conn))
            {
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridViewPracovnici.CurrentRow == null)
            {
                MessageBox.Show("Prosím vyberte řádek k odstranění.", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow selectedRow = dataGridViewPracovnici.CurrentRow;
            int selectedId = Convert.ToInt32(selectedRow.Cells["ID"].Value);

            var result = MessageBox.Show(
                "Opravdu chcete smazat tento záznam?",
                "Potvrzení",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result == DialogResult.Yes)
            {
                using (OracleConnection conn = new OracleConnection(connectionString))
                {
                    try
                    {
                        conn.Open();

                        string deleteQuery = "DELETE FROM PRACOVNICI WHERE ID = :id";

                        using (OracleCommand cmd = new OracleCommand(deleteQuery, conn))
                        {
                            cmd.Parameters.Add("id", OracleDbType.Int32).Value = selectedId;

                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Záznam byl úspěšně odstraněn.", "Úspěch", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LoadData();
                            }
                            else
                            {
                                MessageBox.Show("Záznam nebyl nalezen.", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            DataGridViewFilterHelper.FilterData(dataGridViewPracovnici, txtSearch);
        }
    }
}
