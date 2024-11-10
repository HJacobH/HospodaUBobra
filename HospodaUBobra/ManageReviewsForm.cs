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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace HospodaUBobra
{
    public partial class ManageReviewsForm : Form
    {
        string connectionString = $"User Id=st69639;Password=Server2022;Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=fei-sql3.upceucebny.cz)(PORT=1521)))(CONNECT_DATA=(SID=BDAS)));";
        private int selectedReviewId;
        private string selectedReviewUser;

        public ManageReviewsForm()
        {
            InitializeComponent();
            LoadBeers();
            LoadBreweries();
            LoadUsers();

            dataGridViewReviews.ReadOnly = true;

            if (UserSession.Role == "Anonymous")
            {
                btnAddReview.Enabled = false;
                btnUpdateReview.Enabled = false;
                btnDeleteReview.Enabled = false;
            }

            pocetRecenziLabel.Text = $"Počet recenzí uživatele  je {GetReviewCountByUsername("")}.";

            txtReviewDetails.WordWrap = true;
            txtReviewDetails.ReadOnly = true;
            LoadReviews();
        }

        private void LoadUsers()
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT USERNAME FROM USERS";

                using (OracleCommand cmd = new OracleCommand(query, conn))
                {
                    using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        comboBoxUsers.DataSource = dt;
                        comboBoxUsers.DisplayMember = "USERNAME";
                        comboBoxUsers.ValueMember = "USERNAME";

                        comboBoxUsers.SelectedIndex = -1;
                    }
                }
            }
        }


        private void LoadBeers()
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT id_piva, nazev FROM PIVA";

                using (OracleCommand cmd = new OracleCommand(query, conn))
                {
                    using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        comboBoxBeers.DataSource = dt;
                        comboBoxBeers.DisplayMember = "NAZEV";
                        comboBoxBeers.ValueMember = "ID_PIVA";
                    }
                }
            }
        }

        private void LoadBreweries()
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT id_pivovaru, nazev FROM PIVOVARY";

                using (OracleCommand cmd = new OracleCommand(query, conn))
                {
                    using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        comboBoxBreweries.DataSource = dt;
                        comboBoxBreweries.DisplayMember = "nazev";
                        comboBoxBreweries.ValueMember = "id_pivovaru";
                    }
                }
            }
        }

        private void LoadReviews()
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT id_recenze, titulek, text_recenze, pivovar_id_pivovaru, pivo_id_piva, pocet_hvezdicek, user_id FROM RECENZE";

                using (OracleCommand cmd = new OracleCommand(query, conn))
                {
                    using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dataGridViewReviews.DataSource = dt;

                        dataGridViewReviews.Columns["user_id"].Visible = false;
                    }
                }
            }
        }

        private void btnAddReview_Click(object sender, EventArgs e)
        {

            if (UserSession.Role == "Anonymous")
            {
                MessageBox.Show("Musíte být přihlášeni, abyste mohli zanechat recenzi.");
                return;
            }

            string title = txtTitle.Text.Trim();
            string reviewText = txtReviewText.Text.Trim();
            int breweryId = Convert.ToInt32(comboBoxBreweries.SelectedValue);
            int beerId = Convert.ToInt32(comboBoxBeers.SelectedValue);

            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(reviewText))
            {
                MessageBox.Show("Název a obsah recenze nesmí být prázdý.");
                return;
            }

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();
                string insertQuery = "INSERT INTO RECENZE (TITULEK, text_recenze, pivovar_id_pivovaru, pivo_id_piva) VALUES (:title, :text, :breweryId, :beerId)";
                using (OracleCommand cmd = new OracleCommand(insertQuery, conn))
                {
                    cmd.Parameters.Add(new OracleParameter("TITULEK", title));
                    cmd.Parameters.Add(new OracleParameter("text_recenze", reviewText));
                    cmd.Parameters.Add(new OracleParameter("pivovar_id_pivovaru", breweryId));
                    cmd.Parameters.Add(new OracleParameter("pivo_id_piva", beerId));

                    cmd.ExecuteNonQuery();
                }
            }

            LogUserAction("Přidat recenzi", $"Přidaná recenze: {title} od {UserSession.Username} pro pivovar s ID {breweryId} a pivo s ID {beerId}");

            LoadReviews();

            MessageBox.Show("Recenze přidána úspěšně!");
        }

        private void btnUpdateReview_Click(object sender, EventArgs e)
        {
            if (UserSession.Role == "Anonymous" || selectedReviewUser != UserSession.Username)
            {
                MessageBox.Show("Můžete aktualizovat pouze své recenze.");
                return;
            }

            if (selectedReviewId <= 0) return;

            string title = txtTitle.Text.Trim();
            string reviewText = txtReviewText.Text.Trim();

            if (comboBoxBreweries.SelectedValue == null || comboBoxBeers.SelectedValue == null)
            {
                MessageBox.Show("Vyberte pivovar a pivo.");
                return;
            }

            int breweryId = Convert.ToInt32(comboBoxBreweries.SelectedValue);
            int beerId = Convert.ToInt32(comboBoxBeers.SelectedValue);

            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(reviewText))
            {
                MessageBox.Show("Název a obsah recenze nesmí být prázdý.");
                return;
            }

            try
            {
                using (OracleConnection conn = new OracleConnection(connectionString))
                {
                    conn.Open();
                    string updateQuery = "UPDATE Recenze SET Titulek = :title, text_recenze = :text, pivovar_id_pivovaru = :breweryId, pivo_id_piva = :beerId WHERE id_recenze = :reviewId";
                    using (OracleCommand cmd = new OracleCommand(updateQuery, conn))
                    {
                        cmd.Parameters.Add(new OracleParameter("Titulek", title));
                        cmd.Parameters.Add(new OracleParameter("text_recenze", reviewText));
                        cmd.Parameters.Add(new OracleParameter("pivovar_id_pivovaru", breweryId));
                        cmd.Parameters.Add(new OracleParameter("pivo_id_piva", beerId));
                        cmd.Parameters.Add(new OracleParameter("id_recenze", selectedReviewId));
                        cmd.ExecuteNonQuery();
                        int rowsAffected = cmd.ExecuteNonQuery();
                    }
                }

                LogUserAction("Aktualizování recenze", $"Aktualizace recenze s ID: {selectedReviewId} od {UserSession.Username}");

                LoadReviews();

                MessageBox.Show("Review updated succesfully!");
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

        private void btnDeleteReview_Click(object sender, EventArgs e)
        {
            if (UserSession.Role == "Anonymous" || selectedReviewUser != UserSession.Username)
            {
                MessageBox.Show("Můžete odstranit pouze Vaše recenze.");
                return;
            }

            if (selectedReviewId <= 0)
            {
                MessageBox.Show("Vyberte recenzi ke smazání.");
                return;
            }

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string deleteQuery = "DELETE FROM RECENZE WHERE id_recenze = :reviewId";
                    using (OracleCommand cmd = new OracleCommand(deleteQuery, conn))
                    {
                        cmd.Parameters.Add(new OracleParameter("reviewId", selectedReviewId));

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            LogUserAction("Recenze odstraněna", $"ID odstraněné recenze: {selectedReviewId} od {UserSession.Username}");

                            LoadReviews();

                            MessageBox.Show("Recenze odstraněna úspěšně!");
                        }
                        else
                        {
                            MessageBox.Show("Recenze s daným ID nenalezena.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Chyba při mazání recenze: " + ex.Message);
                }
            }
        }

        private void dataGridViewReviews_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewReviews.CurrentRow != null)
            {
                selectedReviewId = Convert.ToInt32(dataGridViewReviews.CurrentRow.Cells["id_recenze"].Value);
                txtTitle.Text = dataGridViewReviews.CurrentRow.Cells["titulek"].Value.ToString();
                txtReviewText.Text = dataGridViewReviews.CurrentRow.Cells["text_recenze"].Value.ToString();
                comboBoxBreweries.SelectedValue = dataGridViewReviews.CurrentRow.Cells["pivovar_id_pivovaru"].Value;
                comboBoxBeers.SelectedValue = dataGridViewReviews.CurrentRow.Cells["pivo_id_piva"].Value;

                if (UserSession.Role != "Anonymous")
                {
                    selectedReviewUser = dataGridViewReviews.CurrentRow.Cells["USER_ID"].Value.ToString();
                }
            }
        }

        private void LogUserAction(string actionType, string actionDesc)
        {
            try
            {
                using (OracleConnection conn = new OracleConnection(connectionString))
                {
                    conn.Open();
                    string insertLogQuery = "INSERT INTO User_logs (ACTION_TYPE, ACTION_DESC, ACTION_DATE, USER_ID, ROLE) VALUES (:actionType, :actionDesc, SYSDATE, :userId, :role)";
                    using (OracleCommand cmd = new OracleCommand(insertLogQuery, conn))
                    {
                        cmd.Parameters.Add(new OracleParameter("actionType", actionType));
                        cmd.Parameters.Add(new OracleParameter("actionDesc", actionDesc));
                        cmd.Parameters.Add(new OracleParameter("userId", UserSession.Username));
                        cmd.Parameters.Add(new OracleParameter("role", UserSession.Role.ToString()));

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error logging action: " + ex.Message);
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private string GetHighestRatedReview(int productId)
        {
            string reviewDetails = string.Empty;

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();

                string query = "SELECT GetHighestRatedReview(:productId) AS ReviewDetails FROM DUAL";

                using (OracleCommand cmd = new OracleCommand(query, conn))
                {
                    cmd.Parameters.Add(new OracleParameter("productId", productId));

                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        reviewDetails = result.ToString();
                    }
                }
            }

            return reviewDetails;
        }

        private void comboBoxBeers_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int productId = Convert.ToInt32(comboBoxBeers.SelectedValue);

                string reviewDetails = GetHighestRatedReview(productId);

                if (!string.IsNullOrEmpty(reviewDetails))
                {
                    txtReviewDetails.Text = reviewDetails;
                }
                else
                {
                    txtReviewDetails.Text = "Žádná recenze pro toto pivo nenalezena.";
                }

            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }

        public int GetReviewCountByUsername(string username)
        {
            int reviewCount = 0;

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT GetReviewCountByUsername(:username) FROM DUAL";

                using (OracleCommand cmd = new OracleCommand(query, conn))
                {
                    cmd.Parameters.Add(new OracleParameter("username", username));

                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        reviewCount = Convert.ToInt32(result);
                    }
                }
            }

            return reviewCount;
        }

        public void DisplayUserReviews(string username)
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();
                using (OracleCommand cmd = new OracleCommand("GetUserReviewsProcedure", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("p_username", OracleDbType.Varchar2).Value = username;

                    cmd.Parameters.Add("p_reviews", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                    using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dataGridViewReviews.DataSource = dt;
                    }
                }
            }
        }

        private void comboBoxUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxUsers.SelectedValue != null)
            {
                string selectedUsername = comboBoxUsers.SelectedValue.ToString();
                DisplayUserReviews(selectedUsername);
                pocetRecenziLabel.Text = $"Počet recenzí uživatele {selectedUsername} je {GetReviewCountByUsername(selectedUsername)}.";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadReviews();
        }
    }
}
