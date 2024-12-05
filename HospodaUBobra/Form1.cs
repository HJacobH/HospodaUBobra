using Oracle.ManagedDataAccess.Client;
using System.Data;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Windows.Forms;
using Microsoft.VisualBasic.ApplicationServices;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace HospodaUBobra
{
    public partial class Form1 : Form
    {
        string st = "st69639";
        string heslo = "Server2022";
        string connectionString;

        public Form1(string roleName)
        {
            InitializeComponent();
            connectionString = $"User Id={st};Password={heslo};Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=fei-sql3.upceucebny.cz)(PORT=1521)))(CONNECT_DATA=(SID=BDAS)));";

            comboBoxTables.SelectedIndexChanged += new EventHandler(comboBoxTables_SelectedIndexChanged);

            comboBoxTables.Visible = false;

            ApplyRolePermissions();
            PopulateTableList();
            btnLogout.Enabled = false;
            currentUserLabel.Text = "Anonymous";

            UserSession.EmulatedRole = null;

            PopulateRoleEmulationDropdown();

            dataGridView1.ReadOnly = true;

            LoadInitialData();
        }

        private void LoadInitialData()
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    using (OracleCommand cmd = new OracleCommand("PivaStats", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("p_beer_stats", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                        using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            DataGridViewFilterHelper.BindData(dataGridView1, dt);

                            dataGridView1.DataSource = dt;

                            if (dataGridView1.Columns.Contains("BEER_ID"))
                            {
                                dataGridView1.Columns["BEER_ID"].Visible = false;
                            }

                        }
                    }
                }
                catch (OracleException ex)
                {
                    MessageBox.Show("Oracle error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void PopulateTableList()
        {
            try
            {
                using (OracleConnection conn = new OracleConnection(connectionString))
                {
                    conn.Open();

                    string query = $"SELECT table_name FROM all_tables WHERE owner = UPPER('{st}')";

                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    {
                        using (OracleDataReader reader = cmd.ExecuteReader())
                        {
                            comboBoxTables.Items.Clear();

                            while (reader.Read())
                            {
                                string tableName = reader.GetString(0);

                                comboBoxTables.Items.Add(tableName);

                            }
                        }
                    }

                    conn.Close();
                }
            }
            catch (OracleException ex)
            {
                Log("Oracle error while fetching table names: " + ex.Message);
                MessageBox.Show("Oracle error: " + ex.Message);
            }
            catch (Exception ex)
            {
                Log("General error: " + ex.Message);
                MessageBox.Show("General error: " + ex.Message);
            }
        }

        private void comboBoxTables_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedTable = comboBoxTables.SelectedItem.ToString();
            LoadTableData(selectedTable);
        }

        private void LoadTableData(string tableName)
        {
            try
            {
                using (OracleConnection conn = new OracleConnection(connectionString))
                {
                    conn.Open();

                    // 1. Generování dynamického seznamu sloupců
                    string columnQuery = $@"
                        SELECT LISTAGG(column_name, ', ') WITHIN GROUP (ORDER BY column_id)
                        FROM user_tab_columns 
                        WHERE table_name = UPPER('{tableName}') 
                        AND column_name NOT LIKE 'ID_%'";

                    string columns = null;

                    using (OracleCommand cmd = new OracleCommand(columnQuery, conn))
                    {
                        using (OracleDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                columns = reader.GetString(0); // Načte dynamicky vytvořený seznam sloupců
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(columns))
                    {
                        // 2. Vytvoření a vykonání dotazu s dynamickými sloupci
                        string dataQuery = $"SELECT {columns} FROM {tableName}";

                        using (OracleCommand cmd = new OracleCommand(dataQuery, conn))
                        {
                            using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                            {
                                DataTable dataTable = new DataTable();
                                adapter.Fill(dataTable);

                                dataGridView1.DataSource = dataTable;
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("No columns found to query.");
                    }

                    conn.Close();
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
            MessageBox.Show(DateTime.Now + ": " + message);
        }

        private void ApplyRolePermissions()
        {
            string currentRole = UserSession.EmulatedRole ?? UserSession.Role;

            if (currentRole == "Anonymous")
            {
                // Uzivatel
                uploadPfpToolStripMenuItem.Visible = false;
                vytvoritUzivateleToolStripMenuItem.Visible = false;
                spravaKlientuToolStripMenuItem.Visible = false;

                // Recenze
                smazaneRecenzeToolStripMenuItem.Visible = false;

                // Piva
                pridatPivoToolStripMenuItem.Visible = false;
                nizkyPocetPivToolStripMenuItem.Visible = false;

                // Pivovary
                zamestnanciToolStripMenuItem.Visible = false;
                vyrobkyToolStripMenuItem.Visible = false;
                objednavkyToolStripMenuItem1.Visible = false;
                spravaPivovaruToolStripMenuItem.Visible = false;
                vlastniciToolStripMenuItem.Visible = false;
                hierarchiePracovnikuToolStripMenuItem.Visible = false;
                spravaVyrobyToolStripMenuItem.Visible = false;
                topMestaToolStripMenuItem.Visible = false;

                SpravaCiselnikuToolStrip.Visible = false;

                objednavkyToolStripMenuItem.Visible = false;
                SpravaCiselnikuToolStrip.Visible = false;

                cbEmulace.Visible = false;
                btnLogs.Visible = false;
                btnSystemKatalog.Visible = false;

                if (UserSession.Role == "Admin")
                {
                    cbEmulace.Visible = true;
                }
            }
            else if (currentRole == "User")
            {
                vytvoritUzivateleToolStripMenuItem.Visible = false;
                spravaKlientuToolStripMenuItem.Visible = false;

                // Recenze
                smazaneRecenzeToolStripMenuItem.Visible = false;

                // Piva
                pridatPivoToolStripMenuItem.Visible = false;
                nizkyPocetPivToolStripMenuItem.Visible = false;

                // Pivovary
                zamestnanciToolStripMenuItem.Visible = false;
                vyrobkyToolStripMenuItem.Visible = false;
                objednavkyToolStripMenuItem1.Visible = false;
                spravaPivovaruToolStripMenuItem.Visible = false;
                vlastniciToolStripMenuItem.Visible = false;
                hierarchiePracovnikuToolStripMenuItem.Visible = false;
                spravaVyrobyToolStripMenuItem.Visible = false;

                uploadPfpToolStripMenuItem.Visible = true;
                topMestaToolStripMenuItem.Visible = true;

                objednavkyToolStripMenuItem.Visible = false;


                SpravaCiselnikuToolStrip.Visible = false;

                btnSystemKatalog.Visible = false;
                cbEmulace.Visible = false;
                if (UserSession.Role == "Admin")
                {
                    cbEmulace.Visible = true;
                }
                btnLogs.Visible = false;
            }
            else if (currentRole == "Klient")
            {
                uploadPfpToolStripMenuItem.Visible = true;
                vytvoritUzivateleToolStripMenuItem.Visible = false;
                spravaKlientuToolStripMenuItem.Visible = false;

                smazaneRecenzeToolStripMenuItem.Visible = false;

                pridatPivoToolStripMenuItem.Visible = false;

                topMestaToolStripMenuItem.Visible = true;
                nizkyPocetPivToolStripMenuItem.Visible = true;

                objednavkyToolStripMenuItem.Visible = true;
                nesplneneObjednavkyToolStripMenuItem.Visible = true;
                evidenceToolStripMenuItem.Visible = true;
                spravaObjednavekToolStripMenuItem.Visible = true;
                splneneObjednavkyToolStripMenuItem.Visible = true;

                SpravaCiselnikuToolStrip.Visible = false;
                cbEmulace.Visible = false;
                if (UserSession.Role == "Admin")
                {
                    cbEmulace.Visible = true;
                }
                btnLogs.Visible = false;
                btnSystemKatalog.Visible = false;
            }
            else if (currentRole == "Admin")
            {
                // Uzivatel
                uploadPfpToolStripMenuItem.Visible = true;
                vytvoritUzivateleToolStripMenuItem.Visible = true;
                spravaKlientuToolStripMenuItem.Visible = true;

                // Recenze
                smazaneRecenzeToolStripMenuItem.Visible = true;

                // Piva
                pridatPivoToolStripMenuItem.Visible = true;
                nizkyPocetPivToolStripMenuItem.Visible = true;

                // Pivovary
                zamestnanciToolStripMenuItem.Visible = true;
                vyrobkyToolStripMenuItem.Visible = true;
                objednavkyToolStripMenuItem1.Visible = true;
                spravaPivovaruToolStripMenuItem.Visible = true;
                vlastniciToolStripMenuItem.Visible = true;
                hierarchiePracovnikuToolStripMenuItem.Visible = true;
                spravaVyrobyToolStripMenuItem.Visible = true;
                topMestaToolStripMenuItem.Visible = true;

                objednavkyToolStripMenuItem.Visible = true;
                SpravaCiselnikuToolStrip.Visible = true;

                cbEmulace.Visible = true;
                btnSystemKatalog.Visible = true;
                btnLogs.Visible = true;
            }

            PopulateTableList();
        }



        private void btnShowKlientObj_Click(object sender, EventArgs e)
        {
            try
            {
                using (OracleConnection conn = new OracleConnection(connectionString))
                {
                    conn.Open();
                    using (OracleCommand cmd = new OracleCommand("GetClientOrders", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("client_orders", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                        using (OracleDataReader reader = cmd.ExecuteReader())
                        {
                            DataTable dataTable = new DataTable();
                            dataTable.Load(reader);

                            dataGridView1.DataSource = dataTable;
                        }
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

        private void loginStipItem_Click(object sender, EventArgs e)
        {
            if (UserSession.Role != "Anonymous")
            {
                MessageBox.Show("Již jste přihlášeni. Nejdříve se odhlašte, než se pokusíte znovu přihlásit.");
                return;
            }

            LoginForm loginForm = new LoginForm();
            loginForm.ShowDialog();

            if (UserSession.Role != "Anonymous")
            {
                MessageBox.Show("Přihlášení úspěšné!");
                btnLogout.Enabled = true;
                ApplyRolePermissions();

                if (UserSession.Role == "Klient")
                {
                    string klientName = GetKlientNameOrBusinessName(UserSession.UserID);
                    currentUserLabel.Text = klientName;
                }
                else
                {
                    currentUserLabel.Text = GetUserUsername(UserSession.UserID);
                }

                UpdateProfilePictureAsync(UserSession.UserID);
            }
            else
            {

            }
        }

        private string GetKlientNameOrBusinessName(int userId)
        {
            string klientName = "Klient";

            try
            {
                using (OracleConnection conn = new OracleConnection(connectionString))
                {
                    conn.Open();

                    string query = @"
                SELECT NVL(TRIM(JMENO || ' ' || PRIJMENI), NAZEV) AS FULL_NAME
                FROM KLIENTI
                WHERE ID_KLIENTA = :userId";

                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    {
                        cmd.Parameters.Add(new OracleParameter("userId", OracleDbType.Int32)).Value = userId;

                        object result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            klientName = result.ToString();
                        }
                    }
                }
            }
            catch (OracleException ex)
            {
                MessageBox.Show($"Oracle error: {ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"General error: {ex.Message}");
            }

            return klientName;
        }


        private string GetUserUsername(int userId)
        {
            string username = null;

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT uzivatelske_jmeno FROM UZIVATELE WHERE ID_UZIVATELE = :userId";

                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    {
                        cmd.Parameters.Add(new OracleParameter("userId", OracleDbType.Int32)).Value = userId;

                        object result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            username = result.ToString();
                        }
                    }
                }
                catch (OracleException ex)
                {
                    MessageBox.Show("Oracle error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            return username;
        }


        private void UpdateProfilePictureAsync(int id)
        {
            Image profilePicture;
            if (UserSession.Role == "Klient")
            {
                profilePicture = GetUserProfilePicture(UserSession.UserID, true);
            }
            else
            {
                profilePicture = GetUserProfilePicture(UserSession.UserID, false);
            }

            if (profilePicture != null)
            {
                profilePictureBox.Image = profilePicture;
                profilePictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            else
            {
                profilePictureBox.Image = null;
            }
        }

        public Image GetUserProfilePicture(int id, bool isClient)
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();

                int? profilePictureId = null;
                string tableName = isClient ? "KLIENTI" : "UZIVATELE";
                string idColumn = isClient ? "ID_KLIENTA" : "ID_UZIVATELE";

                string selectQuery = $"SELECT PROFILE_OBRAZKY_ID FROM {tableName} WHERE {idColumn} = :id";
                using (OracleCommand cmdFetchProfile = new OracleCommand(selectQuery, conn))
                {
                    cmdFetchProfile.CommandType = CommandType.Text;
                    cmdFetchProfile.Parameters.Add(new OracleParameter("id", OracleDbType.Int32)).Value = id;

                    using (OracleDataReader reader = cmdFetchProfile.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            profilePictureId = reader["PROFILE_OBRAZKY_ID"] == DBNull.Value
                                ? (int?)null
                                : Convert.ToInt32(reader["PROFILE_OBRAZKY_ID"]);
                        }
                    }
                }

                // Step 2: Check if the entity has an associated profile picture
                if (profilePictureId == null)
                {
                    // Return null if no profile picture is associated
                    return null;
                }

                // Step 3: Fetch the profile picture from the PROFILOVE_OBRAZKY table
                string pictureQuery = "SELECT PICTURE FROM PROFILOVE_OBRAZKY WHERE ID_PICTURE = :profilePictureId";
                using (OracleCommand cmdFetchPicture = new OracleCommand(pictureQuery, conn))
                {
                    cmdFetchPicture.CommandType = CommandType.Text;
                    cmdFetchPicture.Parameters.Add(new OracleParameter("profilePictureId", OracleDbType.Int32)).Value = profilePictureId.Value;

                    using (OracleDataReader reader = cmdFetchPicture.ExecuteReader())
                    {
                        if (reader.Read() && !reader.IsDBNull(0))
                        {
                            byte[] imageBytes = (byte[])reader["PICTURE"];
                            using (MemoryStream ms = new MemoryStream(imageBytes))
                            {
                                return Image.FromStream(ms);
                            }
                        }
                    }
                }
            }

            // Return null if no profile picture is found
            return null;
        }




        private void registerStipItem_Click(object sender, EventArgs e)
        {
            RegisterForm registerForm = new RegisterForm();
            if (registerForm.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show("Registrace úspěšná!");
            }
        }

        private void spravaRecenziToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ManageReviewsForm manageReviewsForm = new ManageReviewsForm();
            manageReviewsForm.ShowDialog();
        }

        private void pridatPivoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PiwoMangement piwoMangement = new PiwoMangement();
            piwoMangement.ShowDialog();
        }

        private void lokaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //pivovar_mesto_kraj UKOL3 view 2  pivovar_mesto_kraj
            try
            {
                using (OracleConnection conn = new OracleConnection(connectionString))
                {
                    conn.Open();

                    string query = "SELECT * FROM pivovar_mesto_kraj";

                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    {
                        using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                        {
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);

                            DataGridViewFilterHelper.BindData(dataGridView1, dataTable);

                            dataGridView1.DataSource = dataTable;
                        }
                    }

                    conn.Close();
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

        private void zamestnanciToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void explicidCursorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // UKOL5 procedura 3
            bool isAdmin = UserSession.Role == "Admin";
            bool isEmulatingKlient = UserSession.EmulatedRole == "Klient";

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    using (OracleCommand cmd = new OracleCommand("GetIncompleteOrders", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("p_user_id", OracleDbType.Int32).Value = UserSession.UserID;
                        cmd.Parameters.Add("p_is_admin", OracleDbType.Boolean).Value = isAdmin;
                        cmd.Parameters.Add("p_nedokoncene_objednavky", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                        using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            if (isEmulatingKlient && dt.Columns.Contains("ZAKAZNIK"))
                            {
                                foreach (DataRow row in dt.Rows)
                                {
                                    if (row["ZAKAZNIK"] != DBNull.Value)
                                    {
                                        string originalName = row["ZAKAZNIK"].ToString();
                                        string[] nameParts = originalName.Split(' ');

                                        if (nameParts.Length > 1)
                                        {
                                            string firstNameInitial = nameParts[0].Length > 0 ? nameParts[0][0] + "." : string.Empty;
                                            string censoredLastName = new string('*', nameParts[1].Length);
                                            row["ZAKAZNIK"] = $"{firstNameInitial} {censoredLastName}";
                                        }
                                        else
                                        {
                                            row["ZAKAZNIK"] = new string('*', originalName.Length);
                                        }
                                    }
                                }
                            }
                            DataGridViewFilterHelper.BindData(dataGridView1, dt);
                            dataGridView1.DataSource = dt;

                            if (dt.Columns.Contains("ZAKAZNIK"))
                            {
                                dataGridView1.Columns["ZAKAZNIK"].HeaderText = "Zákazník";
                            }
                            if (dt.Columns.Contains("STAV_OBJEDNAVKY"))
                            {
                                dataGridView1.Columns["STAV_OBJEDNAVKY"].HeaderText = "Stav objednávky";
                            }
                            if (dt.Columns.Contains("DATUM_OBJEDNAVKY"))
                            {
                                dataGridView1.Columns["DATUM_OBJEDNAVKY"].HeaderText = "Datum objednávky";
                            }
                            if (dt.Columns.Contains("DATUM_DODANI"))
                            {
                                dataGridView1.Columns["DATUM_DODANI"].HeaderText = "Datum dodání";
                            }
                            if (dt.Columns.Contains("STAV_DODANI"))
                            {
                                dataGridView1.Columns["STAV_DODANI"].HeaderText = "Stav dodání";
                            }
                            if (dt.Columns.Contains("DNY_ZPOZDENI"))
                            {
                                dataGridView1.Columns["DNY_ZPOZDENI"].HeaderText = "Dny zpoždění";
                            }
                        }
                    }
                }
                catch (OracleException ex)
                {
                    MessageBox.Show("Oracle error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (UserSession.Role != "Anonymous")
            {
                UserSession.ClearSession();
                MessageBox.Show("Odhlášení úspěšné!");
                ApplyRolePermissions();
                btnLogout.Enabled = false;
                currentUserLabel.Text = "Anonymous";
                profilePictureBox.Image = null;
                dataGridView1.DataSource = null;
            }
            else
            {
                ApplyRolePermissions();
            }
        }

        private int GetNextPictureId()
        {
            int nextId = 0;

            try
            {
                using (OracleConnection conn = new OracleConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT NVL(MAX(ID_PICTURE), 0) + 1 FROM PROFILOVE_OBRAZKY";

                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    {
                        object result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            nextId = Convert.ToInt32(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chyba při získávání ID obrazku: " + ex.Message);
            }

            return nextId;
        }

        public void UploadProfilePicture(int id, Image profilePicture, string fileName, bool isClient)
        {
            string fileType = "image/jpeg";
            string fileExtension = Path.GetExtension(fileName);
            byte[] profilePictureData;

            // Convert Image to byte array
            using (var ms = new MemoryStream())
            {
                profilePicture.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                profilePictureData = ms.ToArray();
            }

            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();

                int? profilePictureId = null;
                string tableName = isClient ? "KLIENTI" : "UZIVATELE";
                string idColumn = isClient ? "ID_KLIENTA" : "ID_UZIVATELE";

                // Step 1: Retrieve the PROFILE_OBRAZKY_ID from the appropriate table
                string selectQuery = $"SELECT PROFILE_OBRAZKY_ID FROM {tableName} WHERE {idColumn} = :id";
                using (OracleCommand cmdFetchProfile = new OracleCommand(selectQuery, connection))
                {
                    cmdFetchProfile.CommandType = CommandType.Text;
                    cmdFetchProfile.Parameters.Add(new OracleParameter("id", OracleDbType.Int32)).Value = id;

                    using (OracleDataReader reader = cmdFetchProfile.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            profilePictureId = reader["PROFILE_OBRAZKY_ID"] == DBNull.Value
                                ? (int?)null
                                : Convert.ToInt32(reader["PROFILE_OBRAZKY_ID"]);
                        }
                    }
                }

                // Step 2: If PROFILE_OBRAZKY_ID exists, update the existing picture
                if (profilePictureId != null)
                {
                    using (OracleCommand cmdUpdatePicture = new OracleCommand("sprava_profilove_obrazky", connection))
                    {
                        cmdUpdatePicture.CommandType = CommandType.StoredProcedure;

                        cmdUpdatePicture.Parameters.Add("p_identifikator", OracleDbType.Int32).Value = profilePictureId.Value; // Update existing picture
                        cmdUpdatePicture.Parameters.Add("p_id_picture", OracleDbType.Int32).Value = profilePictureId.Value;
                        cmdUpdatePicture.Parameters.Add("p_picture", OracleDbType.Blob).Value = profilePictureData;
                        cmdUpdatePicture.Parameters.Add("p_file_name", OracleDbType.Varchar2).Value = fileName;
                        cmdUpdatePicture.Parameters.Add("p_file_type", OracleDbType.Varchar2).Value = fileType;
                        cmdUpdatePicture.Parameters.Add("p_file_extension", OracleDbType.Varchar2).Value = fileExtension;
                        cmdUpdatePicture.Parameters.Add("p_upload_date", OracleDbType.Date).Value = DateTime.Now;

                        cmdUpdatePicture.ExecuteNonQuery();
                    }
                }
                else
                {
                    // Step 3: If no PROFILE_OBRAZKY_ID exists, insert a new picture
                    int newPictureId = GetNextPictureId();

                    using (OracleCommand cmdInsertPicture = new OracleCommand("sprava_profilove_obrazky", connection))
                    {
                        cmdInsertPicture.CommandType = CommandType.StoredProcedure;

                        cmdInsertPicture.Parameters.Add("p_identifikator", OracleDbType.Int32).Value = DBNull.Value; // New picture
                        cmdInsertPicture.Parameters.Add("p_id_picture", OracleDbType.Int32).Value = newPictureId;
                        cmdInsertPicture.Parameters.Add("p_picture", OracleDbType.Blob).Value = profilePictureData;
                        cmdInsertPicture.Parameters.Add("p_file_name", OracleDbType.Varchar2).Value = fileName;
                        cmdInsertPicture.Parameters.Add("p_file_type", OracleDbType.Varchar2).Value = fileType;
                        cmdInsertPicture.Parameters.Add("p_file_extension", OracleDbType.Varchar2).Value = fileExtension;
                        cmdInsertPicture.Parameters.Add("p_upload_date", OracleDbType.Date).Value = DateTime.Now;

                        cmdInsertPicture.ExecuteNonQuery();
                    }

                    // Step 4: Update the table with the new PROFILE_OBRAZKY_ID
                    string updateQuery = $"UPDATE {tableName} SET PROFILE_OBRAZKY_ID = :profilePictureId WHERE {idColumn} = :id";
                    using (OracleCommand cmdUpdate = new OracleCommand(updateQuery, connection))
                    {
                        cmdUpdate.CommandType = CommandType.Text;

                        cmdUpdate.Parameters.Add("profilePictureId", OracleDbType.Int32).Value = newPictureId;
                        cmdUpdate.Parameters.Add("id", OracleDbType.Int32).Value = id;

                        cmdUpdate.ExecuteNonQuery();
                    }
                }

                MessageBox.Show($"Profile picture successfully {(profilePictureId != null ? "updated" : "uploaded")} for {(isClient ? "KLIENTI" : "UZIVATELE")} ID {id}.");
            }
        }



        private void vytvoritUzivateleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateUser addUserForm = new CreateUser(connectionString);

            addUserForm.ShowDialog();
        }

        private void zobrazitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SpravaPozicePracovnika spravaPozicePracovnika = new SpravaPozicePracovnika();
            spravaPozicePracovnika.ShowDialog();
        }

        private void spravovatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EmplyeeManagement emplyeeManagement = new EmplyeeManagement(connectionString);
            emplyeeManagement.ShowDialog();
        }

        private void vyrobkyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //UKOL3 view VYROBY_VIEW view 1
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    string query = "SELECT * FROM VYROBY_VIEW";

                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    {
                        using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            DataGridViewFilterHelper.BindData(dataGridView1, dt);

                            dataGridView1.DataSource = dt;
                        }
                    }
                }
                catch (OracleException ex)
                {
                    MessageBox.Show("Oracle error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void objednavkyToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //UKOL3 view OBJEDNAVKY_VIEW view 3
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    string query = "SELECT * FROM OBJEDNAVKY_VIEW";

                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    {
                        using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            DataGridViewFilterHelper.BindData(dataGridView1, dt);

                            dataGridView1.DataSource = dt;
                        }
                    }
                }
                catch (OracleException ex)
                {
                    MessageBox.Show("Oracle error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void nizkyPocetPivToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //UKOL5 funkce 1
            string input = Microsoft.VisualBasic.Interaction.InputBox("Zadejte spotní hranu:", "Zobrazení málo piv", "100");

            if (int.TryParse(input, out int threshold))
            {
                using (OracleConnection conn = new OracleConnection(connectionString))
                {
                    try
                    {
                        conn.Open();

                        using (OracleCommand cmd = new OracleCommand("GetLowStockBeers", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add("p_threshold", OracleDbType.Int32).Value = threshold;

                            cmd.Parameters.Add("p_low_stock_beers", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                            using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                            {
                                DataTable dt = new DataTable();
                                adapter.Fill(dt);
                                DataGridViewFilterHelper.BindData(dataGridView1, dt);
                                dataGridView1.DataSource = dt;
                            }
                        }
                    }
                    catch (OracleException ex)
                    {
                        MessageBox.Show("Oracle error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Zadejte validní číslo prosím.", "Neplatný vstup", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void statistikyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //UKOL5 procedura 2
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    using (OracleCommand cmd = new OracleCommand("PivaStats", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("p_beer_stats", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                        using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            DataGridViewFilterHelper.BindData(dataGridView1, dt);

                            dataGridView1.DataSource = dt;

                            if (dataGridView1.Columns.Contains("BEER_ID"))
                            {
                                dataGridView1.Columns["BEER_ID"].Visible = false;
                            }
                        }
                    }
                }
                catch (OracleException ex)
                {
                    MessageBox.Show("Oracle error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void evidenceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SpravaEvidence spravaEvidence = new SpravaEvidence();

            spravaEvidence.ShowDialog();
        }

        private void zobrazitSpravuCiselnikuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CounterManagement counterManagement = new CounterManagement();
            counterManagement.ShowDialog();
        }

        private void spravaMestVesnicToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SpravaMestVesnic spravaMestVesnic = new SpravaMestVesnic();

            spravaMestVesnic.ShowDialog();
        }

        private void spravaObjednavekToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SpravaObjednavek spravaObjednavek = new SpravaObjednavek();
            spravaObjednavek.ShowDialog();
        }

        private void spravaPivovaruToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SpravaPivovaru spravaPivovaru = new SpravaPivovaru();
            spravaPivovaru.ShowDialog();
        }


        private void spravaKlientuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SpravaKlientu spravaKlientu = new SpravaKlientu();
            spravaKlientu.ShowDialog();
        }

        private void nahratToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (UserSession.Role != "Anonymous")
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                    openFileDialog.Title = "Select Profile Picture";

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string filePath = openFileDialog.FileName;
                        string fileName = Path.GetFileName(filePath);
                        profilePictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                        Image profileImage = Image.FromFile(filePath);
                        profilePictureBox.Image = profileImage;

                        if (UserSession.Role == "Klient")
                        {
                            UploadProfilePicture(UserSession.UserID, profileImage, fileName, true);
                        }
                        else
                        {
                            UploadProfilePicture(UserSession.UserID, profileImage, fileName, false);
                        }

                        MessageBox.Show("Profile picture uploaded successfully.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Musíte být přihlášeni!");
            }
        }

        private void odstranitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (UserSession.Role != "Anonymous")
            {
                bool isClient = UserSession.Role == "Klient";

                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    connection.Open();

                    // Begin a transaction
                    using (OracleTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            int? profilePictureId = null;
                            string tableName = isClient ? "KLIENTI" : "UZIVATELE";
                            string idColumn = isClient ? "ID_KLIENTA" : "ID_UZIVATELE";

                            // Step 1: Retrieve the PROFILE_OBRAZEK_ID
                            string selectQuery = $"SELECT PROFILE_OBRAZKY_ID FROM {tableName} WHERE {idColumn} = :id";
                            using (OracleCommand cmdFetchProfile = new OracleCommand(selectQuery, connection))
                            {
                                cmdFetchProfile.Transaction = transaction; // Set the transaction explicitly
                                cmdFetchProfile.CommandType = CommandType.Text;
                                cmdFetchProfile.Parameters.Add(new OracleParameter("id", OracleDbType.Int32)).Value = UserSession.UserID;

                                using (OracleDataReader reader = cmdFetchProfile.ExecuteReader())
                                {
                                    if (reader.Read())
                                    {
                                        profilePictureId = reader["PROFILE_OBRAZKY_ID"] == DBNull.Value
                                            ? (int?)null
                                            : Convert.ToInt32(reader["PROFILE_OBRAZKY_ID"]);
                                    }
                                    else
                                    {
                                        throw new Exception($"{tableName} entry not found.");
                                    }
                                }
                            }


                            // Step 2: Check if there is an associated profile picture
                            if (profilePictureId == null)
                            {
                                MessageBox.Show($"{tableName} entity does not have a profile picture to delete.");
                                transaction.Rollback();
                                return;
                            }

                            // Step 3: Set the PROFILE_OBRAZKY_ID to NULL
                            string updateQuery = $"UPDATE {tableName} SET PROFILE_OBRAZKY_ID = NULL WHERE {idColumn} = :id";
                            using (OracleCommand cmdUpdateProfile = new OracleCommand(updateQuery, connection))
                            {
                                cmdUpdateProfile.CommandType = CommandType.Text;
                                cmdUpdateProfile.Parameters.Add(new OracleParameter("id", OracleDbType.Int32)).Value = UserSession.Role; //OVER HERE

                                cmdUpdateProfile.ExecuteNonQuery();
                                MessageBox.Show($"{tableName} profile picture ID set to NULL.");
                            }

                            // Step 4: Delete the profile picture from the PROFILOVE_OBRAZKY table
                            string deleteQuery = "DELETE FROM PROFILOVE_OBRAZKY WHERE ID_PICTURE = :profilePictureId";
                            using (OracleCommand cmdDeletePicture = new OracleCommand(deleteQuery, connection))
                            {
                                cmdDeletePicture.CommandType = CommandType.Text;
                                cmdDeletePicture.Parameters.Add(new OracleParameter("profilePictureId", OracleDbType.Int32)).Value = profilePictureId.Value;

                                int rowsAffected = cmdDeletePicture.ExecuteNonQuery();
                                if (rowsAffected > 0)
                                {
                                    MessageBox.Show("Profile picture deleted successfully.");
                                }
                                else
                                {
                                    throw new Exception("No profile picture found with the given ID.");
                                }
                            }



                            // Commit the transaction
                            transaction.Commit();
                            UpdateProfilePictureAsync(UserSession.UserID); // This should be outside the transaction
                            MessageBox.Show($"{tableName} profile picture deleted successfully.");
                        }
                        catch (Exception ex)
                        {
                            // Rollback the transaction on error
                            transaction.Rollback();
                            MessageBox.Show($"An error occurred: {ex.Message}");
                        }
                    }
                }
            }
        }

        private void hierarchiePracovnikuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Hierarchie hierarchie = new Hierarchie();
            hierarchie.ShowDialog();
        }

        private void spravaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SpravaVlastnikuPivovaru spravaVlastnikuPivovaru = new SpravaVlastnikuPivovaru();
            spravaVlastnikuPivovaru.ShowDialog();
        }

        private void vlastniciSVicePivovaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    // Create a command to call the stored procedure
                    using (OracleCommand cmd = new OracleCommand("GetMultipleBreweryOwnersWithBreweries", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Define the OUT parameter for the cursor
                        cmd.Parameters.Add("result_cursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                        // Execute the command and load the cursor data into a DataTable
                        using (OracleDataReader reader = cmd.ExecuteReader())
                        {
                            DataTable dataTable = new DataTable();
                            dataTable.Load(reader);

                            DataGridViewFilterHelper.BindData(dataGridView1, dataTable);
                            // Bind the DataTable to the DataGridView
                            dataGridView1.DataSource = dataTable;
                        }
                    }
                }
                catch (OracleException ex)
                {
                    MessageBox.Show($"Oracle error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"General error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void topMestaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    using (OracleCommand cmd = new OracleCommand("GetTopCitiesWithMostBreweries", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Add the output parameter for the cursor
                        OracleParameter cursorOutParam = new OracleParameter("cursor_out", OracleDbType.RefCursor)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(cursorOutParam);

                        // Execute the procedure and retrieve the cursor
                        using (OracleDataReader reader = cmd.ExecuteReader())
                        {
                            // Load data into a DataTable
                            DataTable dataTable = new DataTable();
                            dataTable.Load(reader);

                            DataGridViewFilterHelper.BindData(dataGridView1, dataTable);
                            // Bind the DataTable to the DataGridView
                            dataGridView1.DataSource = dataTable;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void spravaFyzickychOsobToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SpravaFyzickzchOsob sp = new SpravaFyzickzchOsob();
            sp.ShowDialog();
        }

        private void prideleniToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SpravaPracovniku spravaPracovniku = new SpravaPracovniku();
            spravaPracovniku.ShowDialog();
        }

        private void smazaneRecenzeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    string query = "SELECT * FROM SMAZANE_RECENZE_VIEW";
                    using (OracleDataAdapter adapter = new OracleDataAdapter(query, conn))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        DataGridViewFilterHelper.BindData(dataGridView1, dt);

                        dataGridView1.DataSource = dt;

                        dataGridView1.Columns["Title"].HeaderText = "Název";
                        dataGridView1.Columns["Review_Text"].HeaderText = "Text Recenze";
                        dataGridView1.Columns["Brewery_Name"].HeaderText = "Pivovar";
                        dataGridView1.Columns["Beer_Name"].HeaderText = "Pivo";
                        dataGridView1.Columns["Deleted_On"].HeaderText = "Smazáno Dne";

                        if (dataGridView1.Columns["Deleted_On"] != null)
                        {
                            dataGridView1.Columns["Deleted_On"].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm:ss";
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading reviews: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void splneneObjednavkyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    string query = "SELECT * FROM SPLNENE_OBJEDNAVKY_VIEW";

                    if (UserSession.Role != "Admin")
                    {
                        query += " WHERE KLIENT_ID = :loggedInClientId";
                    }

                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    {
                        if (UserSession.Role != "Admin")
                        {
                            cmd.Parameters.Add(new OracleParameter(":loggedInClientId", OracleDbType.Int32)).Value = UserSession.UserID;
                        }

                        using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            // Censor the client name when emulating as a client
                            if (UserSession.EmulatedRole == "Klient")
                            {
                                foreach (DataRow row in dt.Rows)
                                {
                                    if (!row.IsNull("Klient_Name"))
                                    {
                                        string originalName = row["Klient_Name"].ToString();
                                        row["Klient_Name"] = new string('*', originalName.Length);
                                    }
                                }
                            }

                            DataGridViewFilterHelper.BindData(dataGridView1, dt);
                            dataGridView1.DataSource = dt;

                            dataGridView1.Columns["Klient_Name"].HeaderText = "Klient";
                            dataGridView1.Columns["Stav_Objednavky"].HeaderText = "Stav Objednávky";
                            dataGridView1.Columns["Datum_Objednavky"].HeaderText = "Datum Objednávky";
                            dataGridView1.Columns["Datum_Dodani"].HeaderText = "Datum Dodání";

                            dataGridView1.Columns["KLIENT_ID"].Visible = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading completed orders: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        private void spravaVyrobyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SpravceVyroby spravceVyroby = new SpravceVyroby();
            spravceVyroby.ShowDialog();
        }

        private void btnLogs_Click(object sender, EventArgs e)
        {
            string query = "SELECT * FROM LOG_TABLE_VIEW";

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    using (OracleDataAdapter adapter = new OracleDataAdapter(query, conn))
                    {
                        DataTable logTable = new DataTable();
                        adapter.Fill(logTable);

                        logTable.Columns["Log ID"].ColumnName = "ID Záznamu";
                        logTable.Columns["Table"].ColumnName = "Tabulka";
                        logTable.Columns["Operation"].ColumnName = "Operace";
                        logTable.Columns["Timestamp"].ColumnName = "Čas";
                        logTable.Columns["User"].ColumnName = "Uživatel";
                        logTable.Columns["Details"].ColumnName = "Podrobnosti";

                        DataGridViewFilterHelper.BindData(dataGridView1, logTable);
                        dataGridView1.DataSource = logTable;

                        if (dataGridView1.Columns["ID Záznamu"] != null)
                        {
                            dataGridView1.Columns["ID Záznamu"].Visible = false;
                        }

                        if (dataGridView1.Columns["Uživatel"] != null)
                        {
                            dataGridView1.Columns["Uživatel"].Visible = false;
                        }

                        if (dataGridView1.Columns["Podrobnosti"] != null)
                        {
                            dataGridView1.Columns["Podrobnosti"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading log data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            DataGridViewFilterHelper.FilterData(dataGridView1, txtSearch);
        }

        private void PopulateRoleEmulationDropdown()
        {
            cbEmulace.Items.Clear();

            cbEmulace.Items.Add("Admin");
            cbEmulace.Items.Add("Klient");
            cbEmulace.Items.Add("User");
            cbEmulace.Items.Add("Anonymous");

            cbEmulace.SelectedItem = UserSession.Role;
        }


        private void cbEmulace_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbEmulace.SelectedItem != null)
            {
                string selectedRole = cbEmulace.SelectedItem.ToString();

                if (selectedRole == UserSession.Role)
                {
                    UserSession.EmulatedRole = null;
                }
                else
                {
                    UserSession.EmulatedRole = selectedRole;
                    MessageBox.Show($"Nyní emulujete roli: {selectedRole}", "Emulace zapnuta", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadInitialData();
                }

                ApplyRolePermissions();
            }
        }

        private void btnSystemKatalog_Click(object sender, EventArgs e)
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    string query = "SELECT * FROM A_SYSTEMOVY_KATALOG";

                    using (OracleDataAdapter adapter = new OracleDataAdapter(query, conn))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        dataGridView1.DataSource = dataTable;

                        dataGridView1.Columns["ObjectType"].HeaderText = "Typ objektu";
                        dataGridView1.Columns["ObjectName"].HeaderText = "Název";
                        dataGridView1.Columns["ObjectStatus"].HeaderText = "Status";
                        dataGridView1.Columns["CreationDate"].HeaderText = "Datum vytvoření";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading system catalog: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}