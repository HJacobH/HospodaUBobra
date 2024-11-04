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
            ((System.ComponentModel.ISupportInitialize)dataGridViewReviews).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(82, 15);
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
            comboBoxBeers.Location = new Point(80, 39);
            comboBoxBeers.Name = "comboBoxBeers";
            comboBoxBeers.Size = new Size(121, 23);
            comboBoxBeers.TabIndex = 2;
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
            comboBoxBreweries.Location = new Point(80, 68);
            comboBoxBreweries.Name = "comboBoxBreweries";
            comboBoxBreweries.Size = new Size(121, 23);
            comboBoxBreweries.TabIndex = 4;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 113);
            label3.Name = "label3";
            label3.Size = new Size(42, 15);
            label3.TabIndex = 5;
            label3.Text = "Název:";
            // 
            // txtTitle
            // 
            txtTitle.Location = new Point(80, 110);
            txtTitle.Name = "txtTitle";
            txtTitle.Size = new Size(100, 23);
            txtTitle.TabIndex = 6;
            // 
            // txtReviewText
            // 
            txtReviewText.Location = new Point(80, 139);
            txtReviewText.Multiline = true;
            txtReviewText.Name = "txtReviewText";
            txtReviewText.Size = new Size(474, 106);
            txtReviewText.TabIndex = 7;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(12, 142);
            label4.Name = "label4";
            label4.Size = new Size(31, 15);
            label4.TabIndex = 8;
            label4.Text = "Text:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(12, 256);
            label5.Name = "label5";
            label5.Size = new Size(53, 15);
            label5.TabIndex = 9;
            label5.Text = "Recenze:";
            // 
            // dataGridViewReviews
            // 
            dataGridViewReviews.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewReviews.Location = new Point(80, 256);
            dataGridViewReviews.Name = "dataGridViewReviews";
            dataGridViewReviews.Size = new Size(474, 210);
            dataGridViewReviews.TabIndex = 10;
            dataGridViewReviews.SelectionChanged += dataGridViewReviews_SelectionChanged;
            // 
            // btnAddReview
            // 
            btnAddReview.Location = new Point(80, 472);
            btnAddReview.Name = "btnAddReview";
            btnAddReview.Size = new Size(100, 23);
            btnAddReview.TabIndex = 11;
            btnAddReview.Text = "Přídat recenzi";
            btnAddReview.UseVisualStyleBackColor = true;
            btnAddReview.Click += btnAddReview_Click;
            // 
            // btnUpdateReview
            // 
            btnUpdateReview.Location = new Point(186, 472);
            btnUpdateReview.Name = "btnUpdateReview";
            btnUpdateReview.Size = new Size(125, 23);
            btnUpdateReview.TabIndex = 12;
            btnUpdateReview.Text = "Aktualizovat recenzi";
            btnUpdateReview.UseVisualStyleBackColor = true;
            btnUpdateReview.Click += btnUpdateReview_Click;
            // 
            // btnDeleteReview
            // 
            btnDeleteReview.Location = new Point(317, 472);
            btnDeleteReview.Name = "btnDeleteReview";
            btnDeleteReview.Size = new Size(106, 23);
            btnDeleteReview.TabIndex = 13;
            btnDeleteReview.Text = "Odstranit recenzi";
            btnDeleteReview.UseVisualStyleBackColor = true;
            btnDeleteReview.Click += btnDeleteReview_Click;
            // 
            // btnBack
            // 
            btnBack.Location = new Point(479, 472);
            btnBack.Name = "btnBack";
            btnBack.Size = new Size(75, 23);
            btnBack.TabIndex = 14;
            btnBack.Text = "Zpět";
            btnBack.UseVisualStyleBackColor = true;
            btnBack.Click += btnBack_Click;
            // 
            // ManageReviewsForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(566, 505);
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
    }
}