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
        }
        private void LoadBeers()
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT ID_PIVA, NAZEV, CENA FROM PIVA";

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
                        cbBeer.DisplayMember = "Item2"; // Display beer name
                        cbBeer.ValueMember = "Item1"; // Use beer ID as value
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading beers: " + ex.Message);
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
                    string query = "SELECT ID_JEDN_OBJ, NAZEV FROM JEDNOTKY_OBJ";

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
                        cbUnit.DisplayMember = "Item2"; // Display unit name
                        cbUnit.ValueMember = "Item1"; // Use unit ID as value
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading units: " + ex.Message);
                }
            }
        }

        private void LoadEvidenceData()
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    // Updated query to include joins for Beer name, Unit name, and Client email
                    string query = @"
                SELECT 
                    e.ID_EVIDENCE,
                    e.MNOZSTVI,
                    e.DATUM_OBJEDNAVKY,
                    p.NAZEV AS BEER_NAME,
                    u.NAZEV AS UNIT_NAME,
                    e.CENA_OBJEDNAVKY,
                    k.EMAIL AS CLIENT_EMAIL
                FROM 
                    EVIDENCE e
                LEFT JOIN 
                    PIVA p ON e.PIVO_ID_PIVA = p.ID_PIVA
                LEFT JOIN 
                    JEDNOTKY_OBJ u ON e.JEDNOTKA_OBJ_ID_JEDN_OBJ = u.ID_JEDN_OBJ
                LEFT JOIN 
                    OBJEDNAVKY o ON e.OBJEDNAVKA_ID_OBJEDNAVKY1 = o.ID_OBJEDNAVKY
                LEFT JOIN 
                    KLIENTI k ON o.KLIENT_ID = k.ID_KLIENTA";

                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        dgvEvidence.DataSource = dataTable;

                        // Rename column headers for clarity
                        dgvEvidence.Columns["ID_EVIDENCE"].HeaderText = "Evidence ID";
                        dgvEvidence.Columns["MNOZSTVI"].HeaderText = "Quantity";
                        dgvEvidence.Columns["DATUM_OBJEDNAVKY"].HeaderText = "Order Date";
                        dgvEvidence.Columns["BEER_NAME"].HeaderText = "Beer Name";
                        dgvEvidence.Columns["UNIT_NAME"].HeaderText = "Unit Name";
                        dgvEvidence.Columns["CENA_OBJEDNAVKY"].HeaderText = "Order Price";
                        dgvEvidence.Columns["CLIENT_EMAIL"].HeaderText = "Client Email";

                        // Hide the Evidence ID column
                        if (dgvEvidence.Columns.Contains("ID_EVIDENCE"))
                        {
                            dgvEvidence.Columns["ID_EVIDENCE"].Visible = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading evidence data: " + ex.Message);
                }
            }
        }


        private void CalculateOrderPrice()
        {
            if (cbBeer.SelectedItem is Tuple<int, string, decimal> selectedBeer && int.TryParse(txtQuantity.Text, out int quantity))
            {
                decimal pricePerUnit = selectedBeer.Item3; // Get the price from the selected beer
                txtOrderPrice.Text = (pricePerUnit * quantity).ToString("F2"); // Calculate total price
            }
        }

        private void txtQuantity_TextChanged(object sender, EventArgs e)
        {
            CalculateOrderPrice(); // Recalculate order price when quantity changes
        }

        private void dgvEvidence_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvEvidence.CurrentRow != null)
            {
                selectedEvidenceId = Convert.ToInt32(dgvEvidence.CurrentRow.Cells["ID_EVIDENCE"].Value);
                txtQuantity.Text = dgvEvidence.CurrentRow.Cells["MNOZSTVI"].Value.ToString();
                dtpOrderDate.Value = Convert.ToDateTime(dgvEvidence.CurrentRow.Cells["DATUM_OBJEDNAVKY"].Value);

                // Find and select the Beer in the ComboBox
                string beerName = dgvEvidence.CurrentRow.Cells["BEER_NAME"].Value?.ToString();
                cbBeer.SelectedIndex = cbBeer.FindStringExact(beerName);

                // Find and select the Unit in the ComboBox
                string unitName = dgvEvidence.CurrentRow.Cells["UNIT_NAME"].Value?.ToString();
                cbUnit.SelectedIndex = cbUnit.FindStringExact(unitName);

                txtOrderPrice.Text = dgvEvidence.CurrentRow.Cells["CENA_OBJEDNAVKY"].Value.ToString();

                // Display the Client's Email
                txtOrderId.Text = dgvEvidence.CurrentRow.Cells["CLIENT_EMAIL"].Value.ToString();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtQuantity.Text, out int quantity) ||
            cbBeer.SelectedItem == null ||
            cbUnit.SelectedItem == null)
            {
                MessageBox.Show("Please enter valid data.");
                return;
            }

            // Correctly cast SelectedItem for cbBeer and cbUnit
            int beerId = ((Tuple<int, string, decimal>)cbBeer.SelectedItem).Item1; // Beer ID
            int unitId = ((Tuple<int, string>)cbUnit.SelectedItem).Item1; // Unit ID

            decimal orderPrice = decimal.Parse(txtOrderPrice.Text);
            DateTime orderDate = dtpOrderDate.Value;
            int orderId = int.Parse(txtOrderId.Text);

            // Call the ManageEvidence method
            ManageEvidence(null, quantity, orderDate, beerId, unitId, orderPrice, orderId);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedEvidenceId == -1)
            {
                MessageBox.Show("No evidence selected for update. Please select a record.");
                return;
            }

            int quantity, beerId, unitId, orderId;
            decimal orderPrice;
            DateTime orderDate = dtpOrderDate.Value;

            // Validate input
            if (!int.TryParse(txtQuantity.Text, out quantity) ||
                cbBeer.SelectedItem == null ||
                cbUnit.SelectedItem == null ||
                !decimal.TryParse(txtOrderPrice.Text, out orderPrice) ||
                !int.TryParse(txtOrderId.Text, out orderId))
            {
                MessageBox.Show("Please enter valid values for all fields.");
                return;
            }

            beerId = ((Tuple<int, string, decimal>)cbBeer.SelectedItem).Item1;
            unitId = ((Tuple<int, string>)cbUnit.SelectedItem).Item1;

            // Update the evidence
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
                            // Add parameter for ID_EVIDENCE
                            cmd.Parameters.Add(new OracleParameter(":id_evidence", OracleDbType.Int32)).Value = selectedEvidenceId;

                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Evidence record deleted successfully!");
                                LoadEvidenceData(); // Refresh the DataGridView
                                selectedEvidenceId = -1; // Reset the selection
                                ClearFormFields(); // Clear the form fields
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
            // Ensure a beer is selected and the quantity is valid
            if (cbBeer.SelectedItem is Tuple<int, string, decimal> selectedBeer &&
                int.TryParse(txtQuantity.Text, out int quantity))
            {
                decimal pricePerUnit = selectedBeer.Item3; // Get the beer price
                decimal totalPrice = pricePerUnit * quantity; // Calculate total price
                txtOrderPrice.Text = totalPrice.ToString("F2"); // Format as 2 decimal places
            }
            else
            {
                // Clear the order price if input is invalid
                txtOrderPrice.Text = string.Empty;
            }
        }
    }
}
