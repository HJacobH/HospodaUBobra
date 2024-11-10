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
            currentUserLabel.Text = UserSession.Username;
        }


        private void SetButtonVisibility()
        {
            if (currentRole != "Admin")
            {
                zamestnanciToolStripMenuItem.Visible = false;
                pridatPivoToolStripMenuItem.Visible = false;
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

                    string query = $"SELECT * FROM {tableName}";

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

            if (UserSession.Username != "Anonymous" && UserSession.Role != "Anonymous")
            {
                MessageBox.Show("Přihlášení úspěšné!");
                btnLogout.Enabled = true;
                currentUserLabel.Text = UserSession.Username;
                UpdateProfilePictureAsync(UserSession.Username);
                ApplyRolePermissions();
            }
            else
            {
                //MessageBox.Show("Přihlášení selhalo!");
            }
        }

        private async Task UpdateProfilePictureAsync(string username)
        {
            SetLoadingMessage();

            Image profilePicture = await Task.Run(() => GetUserProfilePicture(username));

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

        public Image GetUserProfilePicture(string username)
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT PROFILE_PICTURE FROM USERS WHERE USERNAME = :username";

                using (OracleCommand cmd = new OracleCommand(query, conn))
                {
                    cmd.Parameters.Add(new OracleParameter("username", OracleDbType.Varchar2)).Value = username;

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

        public void UpdateUserProfilePicture(string username, Image profilePicture)
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();
                string query = "UPDATE USERS SET PROFILE_PICTURE = :profilePicture WHERE USERNAME = :username";

                using (OracleCommand cmd = new OracleCommand(query, conn))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        profilePicture.Save(ms, profilePicture.RawFormat);
                        byte[] imageBytes = ms.ToArray();

                        OracleParameter blobParameter = new OracleParameter("profilePicture", OracleDbType.Blob);
                        blobParameter.Value = imageBytes;
                        cmd.Parameters.Add(blobParameter);
                    }

                    OracleParameter usernameParameter = new OracleParameter("username", OracleDbType.Varchar2);
                    usernameParameter.Value = username;
                    cmd.Parameters.Add(usernameParameter);

                    cmd.ExecuteNonQuery();
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

                        UpdateUserProfilePicture(UserSession.Username, profileImage);

                        MessageBox.Show("Profile picture uploaded successfully.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Musíte být přihlášeni!");
            }
        }
    }
}
