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
        }
        private void LoadOrders()
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    // Updated query for DataGridView
                    string query = @"
                SELECT 
                    O.ID_OBJEDNAVKY AS OrderID, 
                    CASE 
                        WHEN K.JMENO IS NOT NULL AND K.PRIJMENI IS NOT NULL THEN K.JMENO || ' ' || K.PRIJMENI
                        ELSE K.NAZEV 
                    END AS ClientName, 
                    S.STAV AS OrderStatus, 
                    O.DATUM_OBJ AS OrderDate, 
                    O.DATUM_DOD AS DeliveryDate
                FROM OBJEDNAVKY O
                LEFT JOIN KLIENTI K ON O.KLIENT_ID = K.ID_KLIENTA
                LEFT JOIN STAVY_OBJEDNAVEK S ON O.STAV_OBJEDNAVKY_ID_STAVU = S.ID_STAVU";

                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                    {
                        DataTable ordersTable = new DataTable();
                        adapter.Fill(ordersTable);

                        dgvOrders.DataSource = ordersTable;

                        dgvOrders.Columns["OrderID"].HeaderText = "Order ID";
                        dgvOrders.Columns["ClientName"].HeaderText = "Client Name";
                        dgvOrders.Columns["OrderStatus"].HeaderText = "Order Status";
                        dgvOrders.Columns["OrderDate"].HeaderText = "Order Date";
                        dgvOrders.Columns["DeliveryDate"].HeaderText = "Delivery Date";
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
                    // Updated query for client names
                    string query = @"SELECT 
                                ID_KLIENTA, 
                                CASE 
                                    WHEN JMENO IS NOT NULL AND PRIJMENI IS NOT NULL THEN JMENO || ' ' || PRIJMENI
                                    ELSE NAZEV 
                                END AS CLIENT_NAME
                             FROM KLIENTI";

                    using (OracleCommand cmd = new OracleCommand(query, conn))
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
                        cbClients.DisplayMember = "Item2"; // Display client name
                        cbClients.ValueMember = "Item1";  // Use client ID as the value
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
                    string query = "SELECT ID_STAVU, STAV FROM STAVY_OBJEDNAVEK";

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

            ManageOrder(null, clientId, orderStatusId, orderDate, deliveryDate);

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

            ManageOrder(selectedOrderId, clientId, orderStatusId, orderDate, deliveryDate);

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

        private void ManageOrder(int? orderId, int clientId, int orderStatusId, DateTime orderDate, DateTime? deliveryDate)
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();
                using (OracleCommand cmd = new OracleCommand("sprava_objednavky", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new OracleParameter("p_identifikator", OracleDbType.Int32)).Value = orderId.HasValue ? orderId : DBNull.Value;
                    cmd.Parameters.Add(new OracleParameter("p_id_objednavky", OracleDbType.Int32)).Value = orderId.HasValue ? orderId : DBNull.Value;
                    cmd.Parameters.Add(new OracleParameter("p_klient_id", OracleDbType.Int32)).Value = clientId;
                    cmd.Parameters.Add(new OracleParameter("p_stav_objednavky_id", OracleDbType.Int32)).Value = orderStatusId;
                    cmd.Parameters.Add(new OracleParameter("p_datum_obj", OracleDbType.Date)).Value = orderDate;
                    cmd.Parameters.Add(new OracleParameter("p_datum_dod", OracleDbType.Date)).Value = deliveryDate.HasValue ? (object)deliveryDate : DBNull.Value;

                    try
                    {
                        cmd.ExecuteNonQuery();
                        string message = orderId.HasValue ? "Order updated successfully!" : "Order added successfully!";
                        MessageBox.Show(message);
                        LoadOrders();
                        ClearFormFields();
                        selectedOrderId = -1;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error managing order: " + ex.Message);
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
            if (dgvOrders.CurrentRow != null)
            {
                selectedOrderId = Convert.ToInt32(dgvOrders.CurrentRow.Cells["OrderID"].Value);

                // Find the matching client and status in the ComboBoxes
                string clientName = dgvOrders.CurrentRow.Cells["ClientName"].Value.ToString();
                string orderStatus = dgvOrders.CurrentRow.Cells["OrderStatus"].Value.ToString();

                cbClients.SelectedIndex = cbClients.FindStringExact(clientName);
                cbOrderStatuses.SelectedIndex = cbOrderStatuses.FindStringExact(orderStatus);

                dtpOrderDate.Value = Convert.ToDateTime(dgvOrders.CurrentRow.Cells["OrderDate"].Value);
                dtpDeliveryDate.Value = dgvOrders.CurrentRow.Cells["DeliveryDate"].Value == DBNull.Value
                    ? DateTime.Now
                    : Convert.ToDateTime(dgvOrders.CurrentRow.Cells["DeliveryDate"].Value);
            }
        }
    }
}
