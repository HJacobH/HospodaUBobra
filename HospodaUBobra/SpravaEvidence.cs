﻿using Oracle.ManagedDataAccess.Client;
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
    public partial class SpravaEvidence : Form
    {
        string connectionString = $"User Id=st69639;Password=Server2022;Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=fei-sql3.upceucebny.cz)(PORT=1521)))(CONNECT_DATA=(SID=BDAS)));";

        private int selectedEvidenceId = -1;

        public SpravaEvidence()
        {
            InitializeComponent();
            LoadBeers();
            LoadUnits();
            LoadEvidenceData();

            dgvEvidence.ReadOnly = true;
            cbBeer.DropDownStyle = ComboBoxStyle.DropDownList;
            cbUnit.DropDownStyle = ComboBoxStyle.DropDownList;
        }
        private void LoadBeers()
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT * FROM A_EVIDENCE_CB_PIVA_VIEW";

                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        List<Tuple<int, string, decimal>> beers = new List<Tuple<int, string, decimal>>();
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            string name = reader.GetString(1);
                            decimal price = reader.GetDecimal(2);
                            beers.Add(new Tuple<int, string, decimal>(id, name, price));
                        }

                        cbBeer.DataSource = beers;
                        cbBeer.DisplayMember = "Item2";
                        cbBeer.ValueMember = "Item1";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Chyba při načítání piv: " + ex.Message);
                }
            }
        }

        private void LoadUnits()
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT * FROM A_EVIDENCE_CB_JEDNOTKY_OBJ_VIEW";

                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        List<Tuple<int, string>> units = new List<Tuple<int, string>>();
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            string name = reader.GetString(1);
                            units.Add(new Tuple<int, string>(id, name));
                        }

                        cbUnit.DataSource = units;
                        cbUnit.DisplayMember = "Item2";
                        cbUnit.ValueMember = "Item1";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Chyba při načítaní jednotek: " + ex.Message);
                }
            }
        }

        private void LoadEvidenceData()
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                bool isAdmin = UserSession.Role == "Admin";
                bool isEmulatingKlient = UserSession.EmulatedRole == "Klient";

                try
                {
                    conn.Open();

                    string query;

                    if (isAdmin)
                    {
                        query = @"SELECT * FROM A_EVIDENCE_ADMIN_VIEW";
                    }
                    else
                    {
                        query = @"SELECT * FROM A_EVIDENCE_KLIENT_VIEW WHERE CLIENT_ID = :userID";
                    }

                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    {
                        if (!isAdmin)
                        {
                            cmd.Parameters.Add(new OracleParameter(":userID", OracleDbType.Int32)).Value = UserSession.UserID;
                        }

                        using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                        {
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);

                            if (isEmulatingKlient && dataTable.Columns.Contains("CLIENT_EMAIL"))
                            {
                                foreach (DataRow row in dataTable.Rows)
                                {
                                    string email = row["CLIENT_EMAIL"].ToString();
                                    row["CLIENT_EMAIL"] = CensureEmail(email);
                                }
                            }

                            DataGridViewFilterHelper.BindData(dgvEvidence, dataTable);
                            dgvEvidence.DataSource = dataTable;

                            dgvEvidence.Columns["ID_EVIDENCE"].HeaderText = "Evidence ID";
                            dgvEvidence.Columns["MNOZSTVI"].HeaderText = "Množství";
                            dgvEvidence.Columns["DATUM_OBJEDNAVKY"].HeaderText = "Datum objednávky";
                            dgvEvidence.Columns["BEER_NAME"].HeaderText = "Pivo";
                            dgvEvidence.Columns["UNIT_NAME"].HeaderText = "Jednotky";
                            dgvEvidence.Columns["CENA_OBJEDNAVKY"].HeaderText = "Cena objednavky";
                            dgvEvidence.Columns["CLIENT_EMAIL"].HeaderText = "Email klienta";

                            if (dgvEvidence.Columns.Contains("ID_EVIDENCE"))
                            {
                                dgvEvidence.Columns["ID_EVIDENCE"].Visible = false;
                            }
                            if (dgvEvidence.Columns.Contains("CLIENT_ID"))
                            {
                                dgvEvidence.Columns["CLIENT_ID"].Visible = false;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Chyba při načítaní evidencí: " + ex.Message, "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private string CensureEmail(string email)
        {
            if (string.IsNullOrEmpty(email) || !email.Contains("@"))
            {
                return email;
            }

            var parts = email.Split('@');
            var username = parts[0];
            var domain = parts[1];

            if (username.Length > 2)
            {
                username = username.First() + new string('*', username.Length - 2) + username.Last();
            }
            else
            {
                username = new string('*', username.Length);
            }

            var domainParts = domain.Split('.');
            if (domainParts.Length > 1)
            {
                var baseDomain = domainParts[0]; 
                var topLevelDomain = string.Join(".", domainParts.Skip(1)); 

                if (baseDomain.Length > 1)
                {
                    baseDomain = baseDomain.First() + new string('*', baseDomain.Length - 1);
                }
                else
                {
                    baseDomain = "*";
                }

                domain = $"{baseDomain}.{topLevelDomain}";
            }
            else
            {
                domain = domain.First() + new string('*', domain.Length - 1);
            }

            return $"{username}@{domain}";
        }


        private void CalculateOrderPrice()
        {
            if (cbBeer.SelectedItem is Tuple<int, string, decimal> selectedBeer && int.TryParse(txtQuantity.Text, out int quantity))
            {
                decimal pricePerUnit = selectedBeer.Item3;
                txtOrderPrice.Text = (pricePerUnit * quantity).ToString("F2");
            }
        }

        private void txtQuantity_TextChanged(object sender, EventArgs e)
        {
            CalculateOrderPrice();
        }

        private void dgvEvidence_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvEvidence.CurrentRow != null)
            {
                var currentRow = dgvEvidence.CurrentRow;

                selectedEvidenceId = currentRow.Cells["ID_EVIDENCE"].Value != DBNull.Value
                    ? Convert.ToInt32(currentRow.Cells["ID_EVIDENCE"].Value)
                    : -1;

                txtQuantity.Text = currentRow.Cells["MNOZSTVI"].Value != DBNull.Value
                    ? currentRow.Cells["MNOZSTVI"].Value.ToString()
                    : string.Empty;

                dtpOrderDate.Value = currentRow.Cells["DATUM_OBJEDNAVKY"].Value != DBNull.Value
                    ? Convert.ToDateTime(currentRow.Cells["DATUM_OBJEDNAVKY"].Value)
                    : DateTime.Now;

                string beerName = currentRow.Cells["BEER_NAME"].Value != DBNull.Value
                    ? currentRow.Cells["BEER_NAME"].Value.ToString()
                    : string.Empty;
                cbBeer.SelectedIndex = cbBeer.FindStringExact(beerName);

                string unitName = currentRow.Cells["UNIT_NAME"].Value != DBNull.Value
                    ? currentRow.Cells["UNIT_NAME"].Value.ToString()
                    : string.Empty;
                cbUnit.SelectedIndex = cbUnit.FindStringExact(unitName);

                txtOrderPrice.Text = currentRow.Cells["CENA_OBJEDNAVKY"].Value != DBNull.Value
                    ? currentRow.Cells["CENA_OBJEDNAVKY"].Value.ToString()
                    : string.Empty;

                txtOrderId.Text = currentRow.Cells["CLIENT_EMAIL"].Value != DBNull.Value
                    ? currentRow.Cells["CLIENT_EMAIL"].Value.ToString()
                    : string.Empty;
            }
            else
            {
                ClearFormFields();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs(out string errorMessage))
            {
                MessageBox.Show(errorMessage, "Chyba ověření vstupů", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int quantity = int.Parse(txtQuantity.Text.Trim());
            int beerId = ((Tuple<int, string, decimal>)cbBeer.SelectedItem).Item1;
            int unitId = ((Tuple<int, string>)cbUnit.SelectedItem).Item1;
            decimal orderPrice = decimal.Parse(txtOrderPrice.Text.Trim());
            DateTime orderDate = dtpOrderDate.Value;
            int orderId = int.Parse(txtOrderId.Text.Trim());

            ManageEvidence(null, quantity, orderDate, beerId, unitId, orderPrice, orderId);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedEvidenceId == -1)
            {
                MessageBox.Show("Není vybrán žádný záznam k aktualizaci.", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidateInputs(out string errorMessage))
            {
                MessageBox.Show(errorMessage, "Chyba ověření vstupů", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int quantity = int.Parse(txtQuantity.Text.Trim());
            int beerId = ((Tuple<int, string, decimal>)cbBeer.SelectedItem).Item1;
            int unitId = ((Tuple<int, string>)cbUnit.SelectedItem).Item1;
            decimal orderPrice = decimal.Parse(txtOrderPrice.Text.Trim());
            DateTime orderDate = dtpOrderDate.Value;
            int orderId = int.Parse(txtOrderId.Text.Trim());

            ManageEvidence(selectedEvidenceId, quantity, orderDate, beerId, unitId, orderPrice, orderId);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedEvidenceId == -1)
            {
                MessageBox.Show("Please select an evidence record to delete.");
                return;
            }

            DialogResult result = MessageBox.Show(
                "Are you sure you want to delete this evidence record?",
                "Confirm Deletion",
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
                        string query = "DELETE FROM EVIDENCE WHERE ID_EVIDENCE = :id_evidence";

                        using (OracleCommand cmd = new OracleCommand(query, conn))
                        {
                            cmd.Parameters.Add(new OracleParameter(":id_evidence", OracleDbType.Int32)).Value = selectedEvidenceId;

                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Evidence record deleted successfully!");
                                LoadEvidenceData();
                                selectedEvidenceId = -1;
                                ClearFormFields();
                            }
                            else
                            {
                                MessageBox.Show("No evidence record found with the selected ID.");
                            }
                        }
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

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFormFields();
        }

        private void ClearFormFields()
        {
            txtQuantity.Clear();
            dtpOrderDate.Value = DateTime.Now;
            cbBeer.SelectedIndex = -1;
            cbUnit.SelectedIndex = -1;
            txtOrderPrice.Clear();
            txtOrderId.Clear();
        }


        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbBeer_SelectedIndexChanged(object sender, EventArgs e)
        {
            CalculateOrderPrice();
        }

        private void ManageEvidence(int? idEvidence, int quantity, DateTime orderDate, int beerId, int unitId, decimal orderPrice, int orderId)
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();
                using (OracleCommand cmd = new OracleCommand("sprava_evidence", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new OracleParameter("p_identifikator", OracleDbType.Int32)).Value = idEvidence.HasValue ? idEvidence : DBNull.Value;
                    cmd.Parameters.Add(new OracleParameter("p_id_evidence", OracleDbType.Int32)).Value = idEvidence;
                    cmd.Parameters.Add(new OracleParameter("p_mnozstvi", OracleDbType.Int32)).Value = quantity;
                    cmd.Parameters.Add(new OracleParameter("p_datum_objednavky", OracleDbType.Date)).Value = orderDate;
                    cmd.Parameters.Add(new OracleParameter("p_pivo_id_piva", OracleDbType.Int32)).Value = beerId;
                    cmd.Parameters.Add(new OracleParameter("p_jednotka_obj_id_jedn_obj", OracleDbType.Int32)).Value = unitId;
                    cmd.Parameters.Add(new OracleParameter("p_cena_objednavky", OracleDbType.Decimal)).Value = orderPrice;
                    cmd.Parameters.Add(new OracleParameter("p_objednavka_id_objednavky", OracleDbType.Int32)).Value = orderId;

                    try
                    {
                        cmd.ExecuteNonQuery();
                        string message = idEvidence.HasValue ? "Evidence updated successfully!" : "Evidence added successfully!";
                        MessageBox.Show(message);
                        LoadEvidenceData();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error managing evidence: " + ex.Message);
                    }
                }
            }
        }

        private void txtQuantity_TextChanged_1(object sender, EventArgs e)
        {
            UpdateOrderPrice();
        }

        private void UpdateOrderPrice()
        {
            if (cbBeer.SelectedItem is Tuple<int, string, decimal> selectedBeer &&
                int.TryParse(txtQuantity.Text, out int quantity))
            {
                decimal pricePerUnit = selectedBeer.Item3;
                decimal totalPrice = pricePerUnit * quantity;
                txtOrderPrice.Text = totalPrice.ToString("F2");
            }
            else
            {
                txtOrderPrice.Text = string.Empty;
            }
        }

        private bool ValidateInputs(out string errorMessage)
        {
            errorMessage = string.Empty;

            if (!int.TryParse(txtQuantity.Text.Trim(), out int quantity) || quantity <= 0)
            {
                errorMessage = "Prosím, zadejte platné množství (celé číslo větší než nula).";
                return false;
            }

            if (dtpOrderDate.Value > DateTime.Now)
            {
                errorMessage = "Datum objednávky nemůže být v budoucnosti.";
                return false;
            }

            if (cbBeer.SelectedItem == null)
            {
                errorMessage = "Vyberte prosím pivo.";
                return false;
            }

            if (cbUnit.SelectedItem == null)
            {
                errorMessage = "Vyberte prosím jednotku.";
                return false;
            }

            if (!decimal.TryParse(txtOrderPrice.Text.Trim(), out decimal orderPrice) || orderPrice <= 0)
            {
                errorMessage = "Prosím, zadejte platnou cenu objednávky.";
                return false;
            }

            return true;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            DataGridViewFilterHelper.FilterData(dgvEvidence, txtSearch);
        }
    }
}
