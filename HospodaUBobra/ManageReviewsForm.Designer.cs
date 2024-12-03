namespace HospodaUBobra
{
    partial class ManageReviewsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            label2 = new Label();
            comboBoxBeers = new ComboBox();
            Breweries = new Label();
            comboBoxBreweries = new ComboBox();
            label3 = new Label();
            txtTitle = new TextBox();
            txtReviewText = new TextBox();
            label4 = new Label();
            label5 = new Label();
            dataGridViewReviews = new DataGridView();
            btnAddReview = new Button();
            btnUpdateReview = new Button();
            btnDeleteReview = new Button();
            btnBack = new Button();
            label6 = new Label();
            txtReviewDetails = new TextBox();
            comboBoxUsers = new ComboBox();
            label8 = new Label();
            pocetRecenziLabel = new Label();
            button1 = new Button();
            cbHodnoceni = new ComboBox();
            label9 = new Label();
            ((System.ComponentModel.ISupportInitialize)dataGridViewReviews).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            label1.Location = new Point(12, 4);
            label1.Name = "label1";
            label1.Size = new Size(121, 21);
            label1.TabIndex = 0;
            label1.Text = "Správa recenzí";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 42);
            label2.Name = "label2";
            label2.Size = new Size(32, 15);
            label2.TabIndex = 1;
            label2.Text = "Piva:";
            // 
            // comboBoxBeers
            // 
            comboBoxBeers.FormattingEnabled = true;
            comboBoxBeers.Location = new Point(112, 39);
            comboBoxBeers.Name = "comboBoxBeers";
            comboBoxBeers.Size = new Size(121, 23);
            comboBoxBeers.TabIndex = 2;
            comboBoxBeers.SelectedIndexChanged += comboBoxBeers_SelectedIndexChanged;
            // 
            // Breweries
            // 
            Breweries.AutoSize = true;
            Breweries.Location = new Point(12, 71);
            Breweries.Name = "Breweries";
            Breweries.Size = new Size(55, 15);
            Breweries.TabIndex = 3;
            Breweries.Text = "Pivovary:";
            // 
            // comboBoxBreweries
            // 
            comboBoxBreweries.FormattingEnabled = true;
            comboBoxBreweries.Location = new Point(112, 68);
            comboBoxBreweries.Name = "comboBoxBreweries";
            comboBoxBreweries.Size = new Size(121, 23);
            comboBoxBreweries.TabIndex = 4;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 102);
            label3.Name = "label3";
            label3.Size = new Size(42, 15);
            label3.TabIndex = 5;
            label3.Text = "Název:";
            // 
            // txtTitle
            // 
            txtTitle.Location = new Point(112, 99);
            txtTitle.Name = "txtTitle";
            txtTitle.Size = new Size(121, 23);
            txtTitle.TabIndex = 6;
            // 
            // txtReviewText
            // 
            txtReviewText.Location = new Point(112, 155);
            txtReviewText.Multiline = true;
            txtReviewText.Name = "txtReviewText";
            txtReviewText.Size = new Size(442, 90);
            txtReviewText.TabIndex = 7;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(12, 158);
            label4.Name = "label4";
            label4.Size = new Size(31, 15);
            label4.TabIndex = 8;
            label4.Text = "Text:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(12, 254);
            label5.Name = "label5";
            label5.Size = new Size(53, 15);
            label5.TabIndex = 9;
            label5.Text = "Recenze:";
            // 
            // dataGridViewReviews
            // 
            dataGridViewReviews.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewReviews.Location = new Point(112, 254);
            dataGridViewReviews.Name = "dataGridViewReviews";
            dataGridViewReviews.RowHeadersWidth = 62;
            dataGridViewReviews.Size = new Size(442, 183);
            dataGridViewReviews.TabIndex = 10;
            dataGridViewReviews.SelectionChanged += dataGridViewReviews_SelectionChanged;
            // 
            // btnAddReview
            // 
            btnAddReview.Location = new Point(72, 502);
            btnAddReview.Name = "btnAddReview";
            btnAddReview.Size = new Size(100, 23);
            btnAddReview.TabIndex = 11;
            btnAddReview.Text = "Přídat recenzi";
            btnAddReview.UseVisualStyleBackColor = true;
            btnAddReview.Click += btnAddReview_Click;
            // 
            // btnUpdateReview
            // 
            btnUpdateReview.Location = new Point(178, 502);
            btnUpdateReview.Name = "btnUpdateReview";
            btnUpdateReview.Size = new Size(125, 23);
            btnUpdateReview.TabIndex = 12;
            btnUpdateReview.Text = "Aktualizovat recenzi";
            btnUpdateReview.UseVisualStyleBackColor = true;
            btnUpdateReview.Click += btnUpdateReview_Click;
            // 
            // btnDeleteReview
            // 
            btnDeleteReview.Location = new Point(309, 502);
            btnDeleteReview.Name = "btnDeleteReview";
            btnDeleteReview.Size = new Size(106, 23);
            btnDeleteReview.TabIndex = 13;
            btnDeleteReview.Text = "Odstranit recenzi";
            btnDeleteReview.UseVisualStyleBackColor = true;
            btnDeleteReview.Click += btnDeleteReview_Click;
            // 
            // btnBack
            // 
            btnBack.Location = new Point(471, 502);
            btnBack.Name = "btnBack";
            btnBack.Size = new Size(75, 23);
            btnBack.TabIndex = 14;
            btnBack.Text = "Zpět";
            btnBack.UseVisualStyleBackColor = true;
            btnBack.Click += btnBack_Click;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(252, 9);
            label6.Name = "label6";
            label6.Size = new Size(163, 15);
            label6.TabIndex = 15;
            label6.Text = "Nejlepší recenze daného piva:";
            // 
            // txtReviewDetails
            // 
            txtReviewDetails.Location = new Point(252, 39);
            txtReviewDetails.Multiline = true;
            txtReviewDetails.Name = "txtReviewDetails";
            txtReviewDetails.Size = new Size(302, 83);
            txtReviewDetails.TabIndex = 17;
            // 
            // comboBoxUsers
            // 
            comboBoxUsers.FormattingEnabled = true;
            comboBoxUsers.Location = new Point(252, 443);
            comboBoxUsers.Name = "comboBoxUsers";
            comboBoxUsers.Size = new Size(99, 23);
            comboBoxUsers.TabIndex = 18;
            comboBoxUsers.SelectedIndexChanged += comboBoxUsers_SelectedIndexChanged;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(80, 446);
            label8.Name = "label8";
            label8.Size = new Size(166, 15);
            label8.TabIndex = 19;
            label8.Text = "Recenze od určitého uživatele:";
            // 
            // pocetRecenziLabel
            // 
            pocetRecenziLabel.AutoSize = true;
            pocetRecenziLabel.Location = new Point(90, 474);
            pocetRecenziLabel.Name = "pocetRecenziLabel";
            pocetRecenziLabel.Size = new Size(10, 15);
            pocetRecenziLabel.TabIndex = 20;
            pocetRecenziLabel.Text = ".";
            // 
            // button1
            // 
            button1.Location = new Point(452, 5);
            button1.Name = "button1";
            button1.Size = new Size(102, 23);
            button1.TabIndex = 21;
            button1.Text = "Všechny recenze";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // cbHodnoceni
            // 
            cbHodnoceni.FormattingEnabled = true;
            cbHodnoceni.Location = new Point(112, 128);
            cbHodnoceni.Name = "cbHodnoceni";
            cbHodnoceni.Size = new Size(121, 23);
            cbHodnoceni.TabIndex = 22;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(12, 131);
            label9.Name = "label9";
            label9.Size = new Size(97, 15);
            label9.TabIndex = 23;
            label9.Text = "Počet Hvězdiček:";
            // 
            // ManageReviewsForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(566, 537);
            Controls.Add(label9);
            Controls.Add(cbHodnoceni);
            Controls.Add(button1);
            Controls.Add(pocetRecenziLabel);
            Controls.Add(label8);
            Controls.Add(comboBoxUsers);
            Controls.Add(txtReviewDetails);
            Controls.Add(label6);
            Controls.Add(btnBack);
            Controls.Add(btnDeleteReview);
            Controls.Add(btnUpdateReview);
            Controls.Add(btnAddReview);
            Controls.Add(dataGridViewReviews);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(txtReviewText);
            Controls.Add(txtTitle);
            Controls.Add(label3);
            Controls.Add(comboBoxBreweries);
            Controls.Add(Breweries);
            Controls.Add(comboBoxBeers);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "ManageReviewsForm";
            Text = "Recenze";
            ((System.ComponentModel.ISupportInitialize)dataGridViewReviews).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private ComboBox comboBoxBeers;
        private Label Breweries;
        private ComboBox comboBoxBreweries;
        private Label label3;
        private TextBox txtTitle;
        private TextBox txtReviewText;
        private Label label4;
        private Label label5;
        private DataGridView dataGridViewReviews;
        private Button btnAddReview;
        private Button btnUpdateReview;
        private Button btnDeleteReview;
        private Button btnBack;
        private Label label6;
        private TextBox txtReviewDetails;
        private ComboBox comboBoxUsers;
        private Label label8;
        private Label pocetRecenziLabel;
        private Button button1;
        private ComboBox cbHodnoceni;
        private Label label9;
    }
}