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
    public partial class ManageReviewsForm : Form
    {
        string connectionString = $"User Id=st69639;Password=Server2022;Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=fei-sql3.upceucebny.cz)(PORT=1521)))(CONNECT_DATA=(SID=BDAS)));";
        private int selectedReviewId;

        public ManageReviewsForm()
        {
            InitializeComponent();
            LoadBeers();
            LoadBreweries();
            LoadReviews();
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
                        comboBoxBeers.DisplayMember = "nazev";
                        comboBoxBeers.ValueMember = "id_piva";
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
                string query = "SELECT id_recenze, titulek, text_recenze, pivovar_id_pivovaru, pivo_id_piva FROM RECENZE";

                using (OracleCommand cmd = new OracleCommand(query, conn))
                {
                    using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dataGridViewReviews.DataSource = dt;
                    }
                }
            }
        }

        private void btnAddReview_Click(object sender, EventArgs e)
        {
            string title = txtTitle.Text.Trim();
            string reviewText = txtReviewText.Text.Trim();
            int breweryId = Convert.ToInt32(comboBoxBreweries.SelectedValue);
            int beerId = Convert.ToInt32(comboBoxBeers.SelectedValue);

            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(reviewText))
            {
                MessageBox.Show("Title and review text cannot be empty.");
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

            LogUserAction("Add Review", $"Added review: {title} by {UserSession.Username} for brewery ID {breweryId} and beer ID {beerId}");
            
            LoadReviews();

            MessageBox.Show("Review added succesfully!");
        }

        private void btnUpdateReview_Click(object sender, EventArgs e)
        {
            if (selectedReviewId <= 0) return;
            
            string title = txtTitle.Text.Trim();
            string reviewText = txtReviewText.Text.Trim();

            if (comboBoxBreweries.SelectedValue == null || comboBoxBeers.SelectedValue == null)
            {
                MessageBox.Show("Please select a brewery and a beer.");
                return;
            }

            int breweryId = Convert.ToInt32(comboBoxBreweries.SelectedValue);
            int beerId = Convert.ToInt32(comboBoxBeers.SelectedValue);

            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(reviewText))
            {
                MessageBox.Show("Title and review text cannot be empty.");
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

                LogUserAction("Update Review", $"Updated review ID: {selectedReviewId} by {UserSession.Username}");

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
            if (selectedReviewId <= 0)
            {
                MessageBox.Show("Please select a review to delete.");
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
                            LogUserAction("Delete Review", $"Deleted review ID: {selectedReviewId} by {UserSession.Username}");

                            LoadReviews();

                            MessageBox.Show("Review deleted successfully!");
                        }
                        else
                        {
                            MessageBox.Show("No review found with the specified ID.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error deleting review: " + ex.Message);
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

    }
}
