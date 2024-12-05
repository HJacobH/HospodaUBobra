using Microsoft.VisualBasic.ApplicationServices;
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

        public ManageReviewsForm()
        {
            InitializeComponent();
            LoadBeers();
            LoadBreweries();
            LoadUsers();
            PopulateRatings();

            dataGridViewReviews.ReadOnly = true;
            cbHodnoceni.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxBeers.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxBreweries.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxUsers.DropDownStyle = ComboBoxStyle.DropDownList;


            if (UserSession.Role == "Anonymous")
            {
                btnAddReview.Enabled = false;
                btnUpdateReview.Enabled = false;
                btnDeleteReview.Enabled = false;
            }

            pocetRecenziLabel.Text = GetReviewCountByEmail(0);

            txtReviewDetails.WordWrap = true;
            txtReviewDetails.ReadOnly = true;
            LoadReviews();
        }

        private void PopulateRatings()
        {
            cbHodnoceni.Items.Clear();
            for (int i = 1; i <= 5; i++)
            {
                cbHodnoceni.Items.Add(i);
            }
            cbHodnoceni.SelectedIndex = 0;
        }

        private void LoadUsers()
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM A_CB_UZIVATELE_RECENZE";

                using (OracleCommand cmd = new OracleCommand(query, conn))
                {
                    using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        comboBoxUsers.DataSource = dt;
                        comboBoxUsers.DisplayMember = "UZIVATELSKE_JMENO";
                        comboBoxUsers.ValueMember = "ID_UZIVATELE";

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
                string query = "SELECT * FROM A_CB_PIVA_RECENZE";

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
                string query = "SELECT * FROM A_CB_NAZEV_PIVOVARU";

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

                string query = @"SELECT * FROM A_DGV_RECENZE";

                using (OracleCommand cmd = new OracleCommand(query, conn))
                {
                    using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        DataGridViewFilterHelper.BindData(dataGridViewReviews, dt);
                        dataGridViewReviews.DataSource = dt;

                        if (dataGridViewReviews.Columns.Contains("id_recenze"))
                        {
                            dataGridViewReviews.Columns["id_recenze"].Visible = false;
                        }
                        if (dataGridViewReviews.Columns.Contains("id_uzivatele"))
                        {
                            dataGridViewReviews.Columns["id_uzivatele"].Visible = false;
                        }
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
            int breweryId = comboBoxBreweries.SelectedValue != null ? Convert.ToInt32(comboBoxBreweries.SelectedValue) : -1;
            int beerId = comboBoxBeers.SelectedValue != null ? Convert.ToInt32(comboBoxBeers.SelectedValue) : -1;
            int rating = cbHodnoceni.SelectedItem != null ? Convert.ToInt32(cbHodnoceni.SelectedItem) : -1;

            if (!ValidateInputs(title, reviewText, breweryId, beerId, rating))
            {
                return;
            }

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();

                // Add review via stored procedure
                using (OracleCommand cmd = new OracleCommand("sprava_recenze", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Parameters for inserting a new review
                    cmd.Parameters.Add("p_identifikator", OracleDbType.Int32).Value = DBNull.Value; // Insert
                    cmd.Parameters.Add("p_id_recenze", OracleDbType.Int32).Direction = ParameterDirection.InputOutput; // New ID will be returned
                    cmd.Parameters["p_id_recenze"].Value = DBNull.Value;

                    cmd.Parameters.Add("p_titulek", OracleDbType.Varchar2).Value = title;
                    cmd.Parameters.Add("p_text_recenze", OracleDbType.Clob).Value = reviewText;
                    cmd.Parameters.Add("p_pivovar_id_pivovaru", OracleDbType.Int32).Value = breweryId;
                    cmd.Parameters.Add("p_pivo_id_piva", OracleDbType.Int32).Value = beerId;
                    cmd.Parameters.Add("p_pocet_hvezdicek", OracleDbType.Int32).Value = rating;
                    cmd.Parameters.Add("p_id_uzivatele", OracleDbType.Int32).Value = UserSession.UserID;

                    cmd.ExecuteNonQuery();                    
                }
            }

            LoadReviews();
            MessageBox.Show("Recenze přidána úspěšně!");
        }

        private void btnUpdateReview_Click(object sender, EventArgs e)
        {
            if (UserSession.Role == "Anonymous")
            {
                MessageBox.Show("Nemáte oprávnění upravit tuto recenzi.", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (selectedReviewId <= 0)
            {
                MessageBox.Show("Vyberte recenzi k aktualizaci.", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int userId = UserSession.UserID;
            int reviewOwnerId = -1;

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();

                string query = "SELECT ID_UZIVATELE FROM RECENZE WHERE ID_RECENZE = :id_recenze";
                using (OracleCommand cmd = new OracleCommand(query, conn))
                {
                    cmd.Parameters.Add("id_recenze", OracleDbType.Int32).Value = selectedReviewId;

                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        reviewOwnerId = Convert.ToInt32(result);
                    }
                }


                if (reviewOwnerId != userId)
                {
                    MessageBox.Show("Nemáte oprávnění upravit tuto recenzi.", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            string title = txtTitle.Text.Trim();
            string reviewText = txtReviewText.Text.Trim();
            int breweryId = comboBoxBreweries.SelectedValue != null ? Convert.ToInt32(comboBoxBreweries.SelectedValue) : -1;
            int beerId = comboBoxBeers.SelectedValue != null ? Convert.ToInt32(comboBoxBeers.SelectedValue) : -1;
            int rating = cbHodnoceni.SelectedItem != null ? Convert.ToInt32(cbHodnoceni.SelectedItem) : -1;

            MessageBox.Show($"DEBUG: Title: {title}, Brewery ID: {breweryId}, Beer ID: {beerId}, Rating: {rating}", "Debug Info");

            if (!ValidateInputs(title, reviewText, breweryId, beerId, rating))
            {
                return;
            }

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();

                using (OracleCommand cmd = new OracleCommand("sprava_recenze", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("p_identifikator", OracleDbType.Int32).Value = 1;
                    cmd.Parameters.Add("p_id_recenze", OracleDbType.Int32).Value = selectedReviewId;
                    cmd.Parameters.Add("p_titulek", OracleDbType.Varchar2).Value = title;
                    cmd.Parameters.Add("p_text_recenze", OracleDbType.Clob).Value = reviewText;
                    cmd.Parameters.Add("p_pivovar_id_pivovaru", OracleDbType.Int32).Value = breweryId;
                    cmd.Parameters.Add("p_pivo_id_piva", OracleDbType.Int32).Value = beerId;
                    cmd.Parameters.Add("p_pocet_hvezdicek", OracleDbType.Int32).Value = rating;
                    cmd.Parameters.Add("p_id_uzivatele", OracleDbType.Int32).Value = userId;

                    cmd.ExecuteNonQuery();
                }
            }

            LoadReviews();

            MessageBox.Show("Recenze byla úspěšně aktualizována!");
        }

        private void btnDeleteReview_Click(object sender, EventArgs e)
        {
            if (UserSession.Role == "Anonymous")
            {
                MessageBox.Show("Nemáte oprávnění odstranit tuto recenzi.", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (selectedReviewId <= 0)
            {
                MessageBox.Show("Vyberte recenzi ke smazání.", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int userId = UserSession.UserID;
            int reviewOwnerId = -1;

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();

                string query = "SELECT ID_UZIVATELE FROM RECENZE WHERE ID_RECENZE = :id_recenze";
                using (OracleCommand cmd = new OracleCommand(query, conn))
                {
                    cmd.Parameters.Add("id_recenze", OracleDbType.Int32).Value = selectedReviewId;

                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        reviewOwnerId = Convert.ToInt32(result);
                    }
                }

                if (reviewOwnerId != userId)
                {
                    MessageBox.Show("Nemáte oprávnění odstranit tuto recenzi.", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            DialogResult confirmResult = MessageBox.Show("Opravdu chcete odstranit tuto recenzi?", "Potvrdit smazání", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirmResult != DialogResult.Yes)
            {
                return;
            }

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string deleteQuery = "DELETE FROM RECENZE WHERE ID_RECENZE = :reviewId";
                    using (OracleCommand cmd = new OracleCommand(deleteQuery, conn))
                    {
                        cmd.Parameters.Add(new OracleParameter("reviewId", selectedReviewId));

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
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
            if (dataGridViewReviews.CurrentRow == null || dataGridViewReviews.CurrentRow.Cells["id_recenze"].Value == DBNull.Value)
            {
                selectedReviewId = -1;
                txtTitle.Text = string.Empty;
                txtReviewText.Text = string.Empty;
                comboBoxBreweries.SelectedIndex = -1;
                comboBoxBeers.SelectedIndex = -1;
                cbHodnoceni.SelectedIndex = -1;

                return;
            }

            try
            {
                selectedReviewId = Convert.ToInt32(dataGridViewReviews.CurrentRow.Cells["id_recenze"].Value);
                txtTitle.Text = dataGridViewReviews.CurrentRow.Cells["titulek"].Value?.ToString() ?? string.Empty;
                txtReviewText.Text = dataGridViewReviews.CurrentRow.Cells["text_recenze"].Value?.ToString() ?? string.Empty;

                string pivovarName = dataGridViewReviews.CurrentRow.Cells["pivovar_name"].Value?.ToString();
                string pivoName = dataGridViewReviews.CurrentRow.Cells["pivo_name"].Value?.ToString();

                comboBoxBreweries.SelectedIndex = !string.IsNullOrEmpty(pivovarName)
                    ? comboBoxBreweries.FindStringExact(pivovarName)
                    : -1;

                comboBoxBeers.SelectedIndex = !string.IsNullOrEmpty(pivoName)
                    ? comboBoxBeers.FindStringExact(pivoName)
                    : -1;

                cbHodnoceni.SelectedItem = dataGridViewReviews.CurrentRow.Cells["pocet_hvezdicek"].Value != DBNull.Value
                    ? Convert.ToInt32(dataGridViewReviews.CurrentRow.Cells["pocet_hvezdicek"].Value)
                    : 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error processing selection: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        public string GetReviewCountByEmail(int id)
        {
            string reviewInfo = String.Empty;

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();

                string query = "SELECT GetReviewCountById(:userId) FROM DUAL";

                using (OracleCommand cmd = new OracleCommand(query, conn))
                {
                    cmd.Parameters.Add(new OracleParameter("userId", id));

                    object result = cmd.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        reviewInfo = result.ToString();
                    }
                }
            }

            return reviewInfo;
        }

        public void DisplayUserReviews(int userId)
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();
                string query = @"
            SELECT 
                r.id_recenze,
                r.titulek,
                r.text_recenze,
                pivo.nazev AS pivo_name,
                pivovar.nazev AS pivovar_name,
                r.pocet_hvezdicek,
                r.id_uzivatele,
                u.uzivatelske_jmeno AS uzivatelske_jmeno
            FROM 
                RECENZE r
            LEFT JOIN 
                PIVOVARY pivovar ON r.pivovar_id_pivovaru = pivovar.id_pivovaru
            LEFT JOIN 
                PIVA pivo ON r.pivo_id_piva = pivo.id_piva
            LEFT JOIN 
                UZIVATELE u ON r.id_uzivatele = u.id_uzivatele
            WHERE 
                r.id_uzivatele = :userId";

                using (OracleCommand cmd = new OracleCommand(query, conn))
                {
                    cmd.Parameters.Add("userId", OracleDbType.Int32).Value = userId;

                    using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        DataGridViewFilterHelper.BindData(dataGridViewReviews, dt);
                        dataGridViewReviews.DataSource = dt;
                    }
                }
            }
        }
        private void comboBoxUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxUsers.SelectedValue != null && int.TryParse(comboBoxUsers.SelectedValue.ToString(), out int userId))
            {
                DisplayUserReviews(userId);

                pocetRecenziLabel.Text = GetReviewCountByEmail(userId);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadReviews();
        }
        private bool ValidateInputs(string title, string reviewText, int breweryId, int beerId, int rating)
        {
            if (string.IsNullOrEmpty(title))
            {
                MessageBox.Show("Název recenze nesmí být prázdný.", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrEmpty(reviewText))
            {
                MessageBox.Show("Obsah recenze nesmí být prázdný.", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (breweryId <= 0)
            {
                MessageBox.Show($"Vyberte platný pivovar. Pivovar ID: {breweryId}", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (beerId <= 0)
            {
                MessageBox.Show("Vyberte platné pivo.", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (rating < 1 || rating > 5)
            {
                MessageBox.Show("Hodnocení musí být mezi 1 a 5 hvězdami.", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            DataGridViewFilterHelper.FilterData(dataGridViewReviews, txtSearch);
        }
    }
}
