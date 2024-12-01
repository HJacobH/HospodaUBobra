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

        private string currentRole;
        private string currentUsername;
        private Dictionary<string, List<string>> roleTables;

        public Form1(string roleName)
        {
            InitializeComponent();
            connectionString = $"User Id={st};Password={heslo};Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=fei-sql3.upceucebny.cz)(PORT=1521)))(CONNECT_DATA=(SID=BDAS)));";

            SetButtonVisibility();

            currentRole = roleName;

            roleTables = new Dictionary<string, List<string>>
            {
                { "Admin", null },
                { "User", new List<string> { "PIVA", "PIVOVARY", "RECENZE", "MESSAGES" } },
                { "Anonymous", new List<string> { "PIVA", "PIVOVARY", "RECENZE" } }
            };
            comboBoxTables.SelectedIndexChanged += new EventHandler(comboBoxTables_SelectedIndexChanged);

            ApplyRolePermissions();
            PopulateTableList();
            btnLogout.Enabled = false;
            currentUserLabel.Text = "Anonymous";
        }


        private void SetButtonVisibility()
        {
            if (currentRole != "Admin")
            {
                zamestnanciToolStripMenuItem.Visible = false;
                pridatPivoToolStripMenuItem.Visible = false;
                SpravaCiselnikuToolStrip.Visible = false;
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

                                if (CanAccessTable(tableName))
                                {
                                    comboBoxTables.Items.Add(tableName);
                                }
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

        private bool CanAccessTable(string tableName)
        {

            if (UserSession.Role == "Admin")
            {
                return true;
            }

            if (roleTables.ContainsKey(UserSession.Role) && roleTables[UserSession.Role] != null)
            {
                return roleTables[UserSession.Role].Contains(tableName);
            }

            return false;
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
            zamestnanciToolStripMenuItem.Visible = UserSession.Role == "Admin";
            pridatPivoToolStripMenuItem.Visible = UserSession.Role == "Admin";
            vytvoritUzivateleToolStripMenuItem.Visible = UserSession.Role == "Admin";
            SpravaCiselnikuToolStrip.Visible = UserSession.Role == "Admin";
            vyrobkyToolStripMenuItem.Visible = UserSession.Role == "Admin";
            objednavkyToolStripMenuItem1.Visible = UserSession.Role == "Admin";
            nizkyPocetPivToolStripMenuItem.Visible = UserSession.Role == "Admin";
            nesplneneObjednavkyToolStripMenuItem.Visible = UserSession.Role == "Admin";
            evidenceToolStripMenuItem.Visible = UserSession.Role == "Admin";

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

                // Check if the user is from KLIENTI or UZIVATELE and set the currentUserLabel accordingly
                if (UserSession.Role == "Klient")
                {
                    // Get klient's name or business name
                    string klientName = GetKlientNameOrBusinessName(UserSession.UserID);
                    currentUserLabel.Text = klientName;
                }
                else
                {
                    // Get username for UZIVATELE
                    currentUserLabel.Text = GetUserUsername(UserSession.UserID);
                }

                UpdateProfilePictureAsync(UserSession.UserID);
            }
            else
            {
                // Optionally handle login failure
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

                // Step 1: Retrieve the PROFILE_OBRAZKY_ID from the appropriate table
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
            //pivovar_mesto_kraj UKOL3 view 2
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
            //UKOL5 procedura 3
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    using (OracleCommand cmd = new OracleCommand("GetIncompleteOrders", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("p_incomplete_orders", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                        using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
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

        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (UserSession.Role != "Anonymous")
            {
                UserSession.ClearSession();
                ApplyRolePermissions();
                MessageBox.Show("Odhlášení úspěšné!");
                btnLogout.Enabled = false;
                currentUserLabel.Text = "Anonymous";
                profilePictureBox.Image = null;
            }
        }

        private void SetLoadingMessage()
        {
            profilePictureBox.Image = null;
            profilePictureBox.Invalidate();
            Graphics g = profilePictureBox.CreateGraphics();
            g.DrawString("Loading...", new Font("Arial", 12), Brushes.Gray, new PointF(10, profilePictureBox.Height / 2 - 10));
            g.Dispose();
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
            try
            {
                using (OracleConnection conn = new OracleConnection(connectionString))
                {
                    conn.Open();

                    string query = "SELECT * FROM hierarchie";

                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    {
                        using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                        {
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);

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
    }
}