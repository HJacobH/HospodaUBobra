using Oracle.ManagedDataAccess.Client;
using System.Data;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Windows.Forms;

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
                currentUserLabel.Text = GetUserUsername(UserSession.UserID);
                UpdateProfilePictureAsync(UserSession.UserID);
            }
            else
            {
                //MessageBox.Show("Přihlášení selhalo!");
            }
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
            Image profilePicture = GetUserProfilePicture(UserSession.UserID);

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

        public Image GetUserProfilePicture(int id)
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT PROFILE_PICTURE FROM UZIVATELE WHERE id_uzivatele = :id";

                using (OracleCommand cmd = new OracleCommand(query, conn))
                {
                    cmd.Parameters.Add(new OracleParameter("id_uzivatele", OracleDbType.Varchar2)).Value = id;

                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read() && !reader.IsDBNull(0))
                        {
                            byte[] imageBytes = (byte[])reader["PROFILE_PICTURE"];
                            using (MemoryStream ms = new MemoryStream(imageBytes))
                            {
                                return Image.FromStream(ms);
                            }
                        }
                    }
                }
            }

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

        public void UpdateUserProfilePicture(string userEmail, Image profilePicture)
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();

                // Fetch the current user details to pass them to the procedure
                string fetchQuery = @"
            SELECT ID_UZIVATELE, UZIVATELSKE_JMENO, EMAIL, TELEFON, DATUM_REGISTRACE, PASSWORD, SALT, ROLE_ID
            FROM UZIVATELE
            WHERE email = :userEmail";

                using (OracleCommand fetchCmd = new OracleCommand(fetchQuery, conn))
                {
                    fetchCmd.Parameters.Add(new OracleParameter("email", OracleDbType.Varchar2)).Value = userEmail;

                    using (OracleDataReader reader = fetchCmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Retrieve existing user data
                            int userId = reader.GetInt32(0);
                            string userName = reader.GetString(1);
                            string email = reader.GetString(2);
                            string phone = reader.GetString(3);
                            DateTime? registrationDate = reader.IsDBNull(4) ? null : reader.GetDateTime(4);
                            string password = reader.GetString(5);
                            string salt = reader.GetString(6);
                            int roleId = reader.GetInt32(7);

                            // Convert profile picture to byte array
                            byte[] profilePictureBytes = null;
                            if (profilePicture != null)
                            {
                                using (MemoryStream ms = new MemoryStream())
                                {
                                    profilePicture.Save(ms, profilePicture.RawFormat);
                                    profilePictureBytes = ms.ToArray();
                                }
                            }

                            // Call the sprava_uzivatele procedure to update the profile picture
                            using (OracleCommand cmd = new OracleCommand("sprava_uzivatele", conn))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.Add("p_identifikator", OracleDbType.Int32).Value = userId; // Update existing user
                                cmd.Parameters.Add("p_id_uzivatele", OracleDbType.Int32).Value = userId;
                                cmd.Parameters.Add("p_uzivatelske_jmeno", OracleDbType.Varchar2).Value = userName;
                                cmd.Parameters.Add("p_email", OracleDbType.Varchar2).Value = email;
                                cmd.Parameters.Add("p_telefon", OracleDbType.Varchar2).Value = phone;
                                cmd.Parameters.Add("p_datum_registrace", OracleDbType.Date).Value = registrationDate;
                                cmd.Parameters.Add("p_password", OracleDbType.Varchar2).Value = password;
                                cmd.Parameters.Add("p_salt", OracleDbType.Varchar2).Value = salt;
                                cmd.Parameters.Add("p_role_id", OracleDbType.Int32).Value = roleId;
                                cmd.Parameters.Add("p_profile_picture", OracleDbType.Blob).Value = profilePictureBytes;

                                cmd.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            throw new Exception("User not found.");
                        }
                    }
                }
            }
        }

        private void uploadPfpToolStripMenuItem_Click(object sender, EventArgs e)
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
                        profilePictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                        Image profileImage = Image.FromFile(filePath);
                        profilePictureBox.Image = profileImage;

                        //UpdateUserProfilePicture(UserSession.Username, profileImage);

                        MessageBox.Show("Profile picture uploaded successfully.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Musíte být přihlášeni!");
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

        private void SpravaCiselnikuToolStrip_Click(object sender, EventArgs e)
        {
            CounterManagement counterManagement = new CounterManagement();
            counterManagement.ShowDialog();
        }
    }
}
