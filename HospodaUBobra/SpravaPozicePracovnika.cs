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
    public partial class SpravaPozicePracovnika : Form
    {
        string connectionString = $"User Id=st69639;Password=Server2022;Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=fei-sql3.upceucebny.cz)(PORT=1521)))(CONNECT_DATA=(SID=BDAS)));";

        public SpravaPozicePracovnika()
        {
            InitializeComponent();
            LoadData();

            dataGridViewPozice.ReadOnly = true;
            comboBoxNazevPozice.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxParentPozice.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void LoadData()
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    string query = "SELECT * FROM A_CB_POZICE_PRACOVNIKA";
                    using (OracleDataAdapter adapter = new OracleDataAdapter(query, conn))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        DataGridViewFilterHelper.BindData(dataGridViewPozice, dt);
                        dataGridViewPozice.DataSource = dt;

                        dataGridViewPozice.Columns["ID_POZICE"].Visible = false;
                    }

                    string poziceQuery = "SELECT * FROM A_CB_POZICE_PRACOVNIKA_NO_PARENT";
                    using (OracleCommand cmd = new OracleCommand(poziceQuery, conn))
                    {
                        using (OracleDataReader reader = cmd.ExecuteReader())
                        {
                            DataTable poziceTable = new DataTable();
                            poziceTable.Load(reader);

                            comboBoxNazevPozice.DataSource = poziceTable;
                            comboBoxNazevPozice.DisplayMember = "NAZEV_POZICE";
                            comboBoxNazevPozice.ValueMember = "ID_POZICE";

                            comboBoxParentPozice.DataSource = poziceTable.Copy();
                            comboBoxParentPozice.DisplayMember = "NAZEV_POZICE";
                            comboBoxParentPozice.ValueMember = "ID_POZICE";
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string nazevPozice = textBoxNazevPozice.Text.Trim();
            int? parentId = comboBoxParentPozice.SelectedValue as int?;

            if (string.IsNullOrEmpty(nazevPozice))
            {
                MessageBox.Show("Prosím zadejte název pozice.", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    using (OracleCommand cmd = new OracleCommand("sprava_procedures_pkg.SPRAVA_POZICE_PRACOVNIKA", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("p_identifikator", OracleDbType.Int32).Value = DBNull.Value;
                        cmd.Parameters.Add("p_id_pozice", OracleDbType.Int32).Value = GetNextPoziceId(conn);
                        cmd.Parameters.Add("p_nazev_pozice", OracleDbType.Varchar2).Value = nazevPozice;
                        cmd.Parameters.Add("p_parent_id", OracleDbType.Int32).Value = DBNull.Value;

                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Nová pozice byla úspěšně přidána.", "Úspěch", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        textBoxNazevPozice.Clear();
                        LoadData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string nazevPozice = textBoxNazevPozice.Text.Trim();
            if (string.IsNullOrEmpty(nazevPozice))
            {
                MessageBox.Show("Prosím zadejte nový název pozice.", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dataGridViewPozice.CurrentRow == null)
            {
                MessageBox.Show("Prosím vyberte pozici k aktualizaci.", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                DataRowView selectedRowView = (DataRowView)dataGridViewPozice.CurrentRow.DataBoundItem;
                int selectedPoziceId = Convert.ToInt32(selectedRowView["ID_POZICE"]);

                int? parentId = comboBoxParentPozice.SelectedValue != null ? Convert.ToInt32(comboBoxParentPozice.SelectedValue) : -1;
                MessageBox.Show(parentId.ToString());

                using (OracleConnection conn = new OracleConnection(connectionString))
                {
                    conn.Open();

                    using (OracleCommand cmd = new OracleCommand("sprava_procedures_pkg.SPRAVA_POZICE_PRACOVNIKA", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("p_identifikator", OracleDbType.Int32).Value = selectedPoziceId; 
                        cmd.Parameters.Add("p_id_pozice", OracleDbType.Int32).Value = selectedPoziceId; 
                        cmd.Parameters.Add("p_nazev_pozice", OracleDbType.Varchar2).Value = nazevPozice;
                        cmd.Parameters.Add("p_parent_id", OracleDbType.Int32).Value = parentId;

                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Pozice byla úspěšně aktualizována.", "Úspěch", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        LoadData();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridViewPozice.CurrentRow == null)
            {
                MessageBox.Show("Prosím vyberte pozici ke smazání.", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataRowView selectedRowView = (DataRowView)dataGridViewPozice.CurrentRow.DataBoundItem;
            int poziceId = Convert.ToInt32(selectedRowView["ID_POZICE"]);

            var result = MessageBox.Show(
                "Opravdu chcete smazat tuto pozici? Zaměstnancům bude přiřazeno NULL.",
                "Potvrzení",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result == DialogResult.Yes)
            {
                DeletePoziceAndSetNull(poziceId);
            }
        }

        private void DeletePoziceAndSetNull(int poziceId)
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    using (OracleCommand cmd = new OracleCommand("DELETE_POZICE_AND_SET_NULL", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("p_pozice_id", OracleDbType.Int32).Value = poziceId;

                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Pozice byla úspěšně smazána a propojení zaměstnanci byli aktualizováni.",
                                        "Úspěch", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        LoadData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridViewPozice_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewPozice.CurrentRow != null)
            {
                DataGridViewRow selectedRow = dataGridViewPozice.CurrentRow;

                textBoxNazevPozice.Text = selectedRow.Cells["NAZEV_POZICE"].Value.ToString();

                comboBoxNazevPozice.SelectedIndex = comboBoxNazevPozice.FindStringExact(selectedRow.Cells["NAZEV_POZICE"].Value.ToString());

                if (selectedRow.Cells["PARENT_ID"].Value != DBNull.Value)
                {
                    comboBoxParentPozice.SelectedValue = Convert.ToInt32(selectedRow.Cells["PARENT_ID"].Value);
                }
                else
                {
                    comboBoxParentPozice.SelectedIndex = -1;
                }
            }
        }
        private int GetNextPoziceId(OracleConnection conn)
        {
            string query = "SELECT NVL(MAX(ID_POZICE), 0) + 1 FROM POZICE_PRACOVNIKA";
            using (OracleCommand cmd = new OracleCommand(query, conn))
            {
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            DataGridViewFilterHelper.FilterData(dataGridViewPozice, txtSearch);
        }
    }
}
