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
    public partial class SpravaObjednavek : Form
    {
        string connectionString = $"User Id=st69639;Password=Server2022;Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=fei-sql3.upceucebny.cz)(PORT=1521)))(CONNECT_DATA=(SID=BDAS)));";

        private int selectedOrderId = -1;

        public SpravaObjednavek()
        {
            InitializeComponent();
            LoadOrders();
            LoadClients();
            LoadOrderStatuses();

            dgvOrders.ReadOnly = true;
            cbClients.DropDownStyle = ComboBoxStyle.DropDownList;
            cbOrderStatuses.DropDownStyle = ComboBoxStyle.DropDownList;

            if(UserSession.Role == "Klient" || UserSession.EmulatedRole == "Klient")
            {
                btnDeleteCancelledOrders.Visible = false;
            }
            else
            {
                btnDeleteCancelledOrders.Visible = true;
            }
        }
        private void LoadOrders()
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    string query = "SELECT * FROM A_DGV_OBJEDNAVKY_VIEW";

                    if (UserSession.Role != "Admin")
                    {
                        query += " WHERE ClientID = :loggedInClientId"; 
                    }


                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    {
                        if (UserSession.Role != "Admin")
                        {
                            cmd.Parameters.Add(new OracleParameter(":loggedInClientId", OracleDbType.Int32)).Value = UserSession.UserID;
                        }

                        using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                        {
                            DataTable ordersTable = new DataTable();
                            adapter.Fill(ordersTable);

                            if (UserSession.EmulatedRole == "Klient" && ordersTable.Columns.Contains("ClientName"))
                            {
                                foreach (DataRow row in ordersTable.Rows)
                                {
                                    if (row["ClientName"] != DBNull.Value)
                                    {
                                        string originalName = row["ClientName"].ToString();
                                        string[] nameParts = originalName.Split(' ');

                                        if (nameParts.Length > 1)
                                        {
                                            string firstNameInitial = nameParts[0].Length > 0 ? nameParts[0][0] + "." : string.Empty;
                                            string censoredLastName = new string('*', nameParts[1].Length);
                                            row["ClientName"] = $"{firstNameInitial} {censoredLastName}";
                                        }
                                        else
                                        {
                                            row["ClientName"] = new string('*', originalName.Length);
                                        }
                                    }
                                }
                            }

                            DataGridViewFilterHelper.BindData(dgvOrders, ordersTable);
                            dgvOrders.DataSource = ordersTable;

                            dgvOrders.Columns["OrderID"].HeaderText = "Order ID";
                            dgvOrders.Columns["ClientName"].HeaderText = "Jméno klienta";
                            dgvOrders.Columns["OrderStatus"].HeaderText = "Status objednávky";
                            dgvOrders.Columns["OrderDate"].HeaderText = "Datum vytvoření objednávky";
                            dgvOrders.Columns["DeliveryDate"].HeaderText = "Datum doručení objednávky";


                            dgvOrders.Columns["CLIENTID"].Visible = false;
                            dgvOrders.Columns["OrderID"].Visible = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading orders: " + ex.Message);
                }
            }

        }
        private void LoadClients()
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    string query = "SELECT * FROM A_CB_KLIENTI_VIEW";

                    if (UserSession.Role != "Admin")
                    {
                        query += " WHERE ID_KLIENTA = :loggedInClientId"; 
                    }

                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    {
                        if (UserSession.Role != "Admin")
                        {
                            cmd.Parameters.Add(new OracleParameter(":loggedInClientId", OracleDbType.Int32)).Value = UserSession.UserID;
                        }

                        using (OracleDataReader reader = cmd.ExecuteReader())
                        {
                            List<Tuple<int, string>> clients = new List<Tuple<int, string>>();
                            while (reader.Read())
                            {
                                int id = reader.GetInt32(0);
                                string name = reader.GetString(1);
                                clients.Add(new Tuple<int, string>(id, name));
                            }

                            cbClients.DataSource = clients;
                            cbClients.DisplayMember = "Item2"; 
                            cbClients.ValueMember = "Item1";  
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading clients: " + ex.Message);
                }
            }
        }

        private void LoadOrderStatuses()
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT * FROM A_CB_STAVY_OBJEDNAVEK";

                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        List<Tuple<int, string>> statuses = new List<Tuple<int, string>>();
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            string status = reader.GetString(1);
                            statuses.Add(new Tuple<int, string>(id, status));
                        }

                        cbOrderStatuses.DataSource = statuses;
                        cbOrderStatuses.DisplayMember = "Item2"; // Display order status
                        cbOrderStatuses.ValueMember = "Item1";  // Use status ID as the value
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading order statuses: " + ex.Message);
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (cbClients.SelectedItem == null || cbOrderStatuses.SelectedItem == null)
            {
                MessageBox.Show("Please select valid client and order status.");
                return;
            }

            int clientId = ((Tuple<int, string>)cbClients.SelectedItem).Item1;
            int orderStatusId = ((Tuple<int, string>)cbOrderStatuses.SelectedItem).Item1;
            DateTime orderDate = dtpOrderDate.Value;
            DateTime? deliveryDate = dtpDeliveryDate.Checked ? (DateTime?)dtpDeliveryDate.Value : null;


            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();
                using (OracleCommand cmd = new OracleCommand("sprava_objednavky", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Pass parameters for adding a new order
                    cmd.Parameters.Add(new OracleParameter("p_identifikator", OracleDbType.Int32)).Value = DBNull.Value;
                    cmd.Parameters.Add(new OracleParameter("p_id_objednavky", OracleDbType.Int32)).Value = DBNull.Value; // Let the procedure handle the ID
                    cmd.Parameters.Add(new OracleParameter("p_klient_id", OracleDbType.Int32)).Value = clientId;
                    cmd.Parameters.Add(new OracleParameter("p_stav_objednavky_id", OracleDbType.Int32)).Value = orderStatusId;
                    cmd.Parameters.Add(new OracleParameter("p_datum_obj", OracleDbType.Date)).Value = orderDate;
                    cmd.Parameters.Add(new OracleParameter("p_datum_dod", OracleDbType.Date)).Value = deliveryDate.HasValue ? (object)deliveryDate.Value : DBNull.Value;

                    try
                    {
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Order added successfully!");
                        LoadOrders();
                        ClearFormFields();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error adding order: " + ex.Message);
                    }
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedOrderId == -1)
            {
                MessageBox.Show("Please select an order to update.");
                return;
            }

            if (cbClients.SelectedItem == null || cbOrderStatuses.SelectedItem == null)
            {
                MessageBox.Show("Please select valid client and order status.");
                return;
            }

            int clientId = ((Tuple<int, string>)cbClients.SelectedItem).Item1;
            int orderStatusId = ((Tuple<int, string>)cbOrderStatuses.SelectedItem).Item1;
            DateTime orderDate = dtpOrderDate.Value;
            DateTime? deliveryDate = dtpDeliveryDate.Checked ? (DateTime?)dtpDeliveryDate.Value : null;

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();
                using (OracleCommand cmd = new OracleCommand("sprava_objednavky", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("p_identifikator", OracleDbType.Int32).Value = selectedOrderId;
                    cmd.Parameters.Add("p_id_objednavky", OracleDbType.Int32).Value = selectedOrderId;
                    cmd.Parameters.Add("p_klient_id", OracleDbType.Int32).Value = clientId;
                    cmd.Parameters.Add("p_stav_objednavky_id", OracleDbType.Int32).Value = orderStatusId;
                    cmd.Parameters.Add("p_datum_obj", OracleDbType.Date).Value = orderDate;
                    cmd.Parameters.Add("p_datum_dod", OracleDbType.Date).Value = deliveryDate.HasValue ? (object)deliveryDate.Value : DBNull.Value;

                    try
                    {
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Order updated successfully!");
                        LoadOrders();
                        ClearFormFields();
                        selectedOrderId = -1;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error updating order: " + ex.Message);
                    }
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedOrderId == -1)
            {
                MessageBox.Show("Please select an order to delete.");
                return;
            }

            DialogResult result = MessageBox.Show("Are you sure you want to delete this order?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                using (OracleConnection conn = new OracleConnection(connectionString))
                {
                    try
                    {
                        conn.Open();
                        string query = "DELETE FROM OBJEDNAVKY WHERE ID_OBJEDNAVKY = :id_objednavky";

                        using (OracleCommand cmd = new OracleCommand(query, conn))
                        {
                            cmd.Parameters.Add(new OracleParameter(":id_objednavky", OracleDbType.Int32)).Value = selectedOrderId;
                            cmd.ExecuteNonQuery();

                            MessageBox.Show("Order deleted successfully!");
                            LoadOrders();
                            ClearFormFields();
                            selectedOrderId = -1;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error deleting order: " + ex.Message);
                    }
                }
            }
        }

        private void ClearFormFields()
        {
            cbClients.SelectedIndex = -1;
            cbOrderStatuses.SelectedIndex = -1;
            dtpOrderDate.Value = DateTime.Now;
            dtpDeliveryDate.Value = DateTime.Now;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvOrders_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvOrders.CurrentRow != null && dgvOrders.CurrentRow.Cells["OrderID"].Value != DBNull.Value)
            {
                try
                {
                    selectedOrderId = Convert.ToInt32(dgvOrders.CurrentRow.Cells["OrderID"].Value);

                    string clientName = dgvOrders.CurrentRow.Cells["ClientName"].Value?.ToString() ?? string.Empty;
                    string orderStatus = dgvOrders.CurrentRow.Cells["OrderStatus"].Value?.ToString() ?? string.Empty;

                    cbClients.SelectedIndex = cbClients.FindStringExact(clientName);
                    cbOrderStatuses.SelectedIndex = cbOrderStatuses.FindStringExact(orderStatus);

                    dtpOrderDate.Value = Convert.ToDateTime(dgvOrders.CurrentRow.Cells["OrderDate"].Value);
                    dtpDeliveryDate.Value = dgvOrders.CurrentRow.Cells["DeliveryDate"].Value == DBNull.Value
                        ? DateTime.Now
                        : Convert.ToDateTime(dgvOrders.CurrentRow.Cells["DeliveryDate"].Value);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error selecting order: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                ClearOrderFields();
            }
        }

        private void ClearOrderFields()
        {
            selectedOrderId = -1;
            cbClients.SelectedIndex = -1;
            cbOrderStatuses.SelectedIndex = -1;
            dtpOrderDate.Value = DateTime.Now;
            dtpDeliveryDate.Value = DateTime.Now;
        }

        private void btnDeleteCancelledOrders_Click(object sender, EventArgs e)
        {
            try
            {
                using (OracleConnection conn = new OracleConnection(connectionString))
                {
                    conn.Open();

                    using (OracleCommand cmd = new OracleCommand("DeleteCancelledOrders", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Zrušené objednávky a jejich evidence úspěšně odstraněna.",
                                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadOrders();
                    }
                }
            }
            catch (OracleException ex)
            {
                MessageBox.Show("Chyba při spojování s databází: " + ex.Message,
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Neočekávaná chyba: " + ex.Message,
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            DataGridViewFilterHelper.FilterData(dgvOrders, txtSearch);
        }
    }
}