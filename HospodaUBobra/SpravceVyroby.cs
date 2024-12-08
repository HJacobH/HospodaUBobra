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
    public partial class SpravceVyroby : Form
    {
        string connectionString = "User Id=st69639;Password=Server2022;Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=fei-sql3.upceucebny.cz)(PORT=1521)))(CONNECT_DATA=(SID=BDAS)));";

        public SpravceVyroby()
        {
            InitializeComponent();
            dataGridView1.ReadOnly = true;

            cbPiva.DropDownStyle = ComboBoxStyle.DropDownList;
            cbPivovary.DropDownStyle = ComboBoxStyle.DropDownList;

            LoadComboboxes();
            LoadData();
        }

        private void LoadComboboxes()
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    string pivovaryQuery = "SELECT * FROM A_CB_NAZEV_PIVOVARU";
                    using (OracleDataAdapter adapter = new OracleDataAdapter(pivovaryQuery, conn))
                    {
                        DataTable pivovaryTable = new DataTable();
                        adapter.Fill(pivovaryTable);
                        cbPivovary.DataSource = pivovaryTable;
                        cbPivovary.DisplayMember = "NAZEV";
                        cbPivovary.ValueMember = "ID_PIVOVARU";
                    }

                    string pivaQuery = "SELECT * FROM A_CB_PIVA_RECENZE";
                    using (OracleDataAdapter adapter = new OracleDataAdapter(pivaQuery, conn))
                    {
                        DataTable pivaTable = new DataTable();
                        adapter.Fill(pivaTable);
                        cbPiva.DataSource = pivaTable;
                        cbPiva.DisplayMember = "NAZEV";
                        cbPiva.ValueMember = "ID_PIVA";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Chyba při načítání dat: " + ex.Message, "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void LoadData()
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    string query = @"SELECT * FROM A_DGV_VYROBA";

                    using (OracleDataAdapter adapter = new OracleDataAdapter(query, conn))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        DataGridViewFilterHelper.BindData(dataGridView1, dt);
                        dataGridView1.DataSource = dt;

                        if (dataGridView1.Columns["ID"] != null)
                        {
                            dataGridView1.Columns["ID"].Visible = false;
                        }
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Chyba při načítání dat: " + ex.Message, "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            int pivovarId = cbPivovary.SelectedValue != null ? Convert.ToInt32(cbPivovary.SelectedValue) : -1;
            int pivoId = cbPiva.SelectedValue != null ? Convert.ToInt32(cbPiva.SelectedValue) : -1;
            int litryZaDen;

            if (!int.TryParse(txtLitryZaDen.Text.Trim(), out litryZaDen) || litryZaDen <= 0)
            {
                MessageBox.Show("Zadejte platné množství litrů za den.", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (CombinationExists(pivovarId, pivoId))
            {
                MessageBox.Show("Tato kombinace pivovaru a piva již existuje.", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    using (OracleCommand cmd = new OracleCommand("sprava_vyroby", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("p_identifikator", OracleDbType.Int32).Value = DBNull.Value; 
                        cmd.Parameters.Add("p_litry_za_den", OracleDbType.Int32).Value = litryZaDen;
                        cmd.Parameters.Add("p_pivovar_id", OracleDbType.Int32).Value = pivovarId;
                        cmd.Parameters.Add("p_pivo_id", OracleDbType.Int32).Value = pivoId;
                        cmd.Parameters.Add("p_id", OracleDbType.Int32).Value = DBNull.Value; 

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Nový záznam byl úspěšně přidán.", "Úspěch", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error adding record: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Vyberte řádek k aktualizaci.", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int selectedId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["ID"].Value);
            int pivovarId = cbPivovary.SelectedValue != null ? Convert.ToInt32(cbPivovary.SelectedValue) : -1;
            int pivoId = cbPiva.SelectedValue != null ? Convert.ToInt32(cbPiva.SelectedValue) : -1;
            int litryZaDen;

            if (!int.TryParse(txtLitryZaDen.Text.Trim(), out litryZaDen) || litryZaDen <= 0)
            {
                MessageBox.Show("Zadejte platné množství litrů za den.", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (CombinationExists(pivovarId, pivoId, selectedId))
            {
                MessageBox.Show("Tato kombinace pivovaru a piva již existuje.", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    using (OracleCommand cmd = new OracleCommand("sprava_vyroby", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("p_identifikator", OracleDbType.Int32).Value = selectedId; 
                        cmd.Parameters.Add("p_litry_za_den", OracleDbType.Int32).Value = litryZaDen;
                        cmd.Parameters.Add("p_pivovar_id", OracleDbType.Int32).Value = pivovarId;
                        cmd.Parameters.Add("p_pivo_id", OracleDbType.Int32).Value = pivoId;
                        cmd.Parameters.Add("p_id", OracleDbType.Int32).Value = selectedId;

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Záznam byl úspěšně aktualizován.", "Úspěch", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Chyby pří aktualizaci výroby: " + ex.Message, "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private bool CombinationExists(int pivovarId, int pivoId, int? excludeId = null)
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM VYROBY WHERE PIVOVAR_ID_PIVOVARU = :pivovarId AND PIVO_ID_PIVA = :pivoId";

                    if (excludeId.HasValue)
                    {
                        query += " AND ID != :excludeId";
                    }

                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    {
                        cmd.Parameters.Add("pivovarId", OracleDbType.Int32).Value = pivovarId;
                        cmd.Parameters.Add("pivoId", OracleDbType.Int32).Value = pivoId;

                        if (excludeId.HasValue)
                        {
                            cmd.Parameters.Add("excludeId", OracleDbType.Int32).Value = excludeId.Value;
                        }

                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        return count > 0; 
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Chyba při kontrole kombinace: " + ex.Message, "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return true; 
                }
            }
        }


        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Vyberte řádek ke smazání.", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int selectedId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["ID"].Value);

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

                        string deleteQuery = "DELETE FROM VYROBY WHERE ID = :id";
                        using (OracleCommand cmd = new OracleCommand(deleteQuery, conn))
                        {
                            cmd.Parameters.Add("id", OracleDbType.Int32).Value = selectedId;

                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Záznam byl úspěšně smazán.", "Úspěch", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadData();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error deleting record: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                DataGridViewRow selectedRow = dataGridView1.CurrentRow;

                txtLitryZaDen.Text = selectedRow.Cells["Litry za Den"].Value.ToString();

                cbPivovary.SelectedIndex = cbPivovary.FindStringExact(selectedRow.Cells["Pivovar"].Value.ToString());

                cbPiva.SelectedIndex = cbPiva.FindStringExact(selectedRow.Cells["Pivo"].Value.ToString());
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            DataGridViewFilterHelper.FilterData(dataGridView1, txtSearch);
        }
    }
}
