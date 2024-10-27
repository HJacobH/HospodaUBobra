namespace HospodaUBobra
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            dataGridView1 = new DataGridView();
            comboBoxTables = new ComboBox();
            btnPrihlasit = new Button();
            roleLabel = new Label();
            btnRegister = new Button();
            currentUserLabel = new Label();
            btnReviews = new Button();
            btnAddPiwo = new Button();
            btnShowKlientObj = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(12, 43);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new Size(817, 395);
            dataGridView1.TabIndex = 0;
            // 
            // comboBoxTables
            // 
            comboBoxTables.FormattingEnabled = true;
            comboBoxTables.Location = new Point(835, 150);
            comboBoxTables.Name = "comboBoxTables";
            comboBoxTables.Size = new Size(121, 23);
            comboBoxTables.TabIndex = 1;
            // 
            // btnPrihlasit
            // 
            btnPrihlasit.Location = new Point(881, 416);
            btnPrihlasit.Margin = new Padding(3, 2, 3, 2);
            btnPrihlasit.Name = "btnPrihlasit";
            btnPrihlasit.Size = new Size(75, 23);
            btnPrihlasit.TabIndex = 2;
            btnPrihlasit.Text = "Login";
            btnPrihlasit.UseVisualStyleBackColor = true;
            btnPrihlasit.Click += btnPrihlasit_Click;
            // 
            // roleLabel
            // 
            roleLabel.AutoSize = true;
            roleLabel.Location = new Point(864, 43);
            roleLabel.Name = "roleLabel";
            roleLabel.Size = new Size(0, 15);
            roleLabel.TabIndex = 3;
            // 
            // btnRegister
            // 
            btnRegister.Location = new Point(881, 388);
            btnRegister.Name = "btnRegister";
            btnRegister.Size = new Size(75, 23);
            btnRegister.TabIndex = 4;
            btnRegister.Text = "Register";
            btnRegister.UseVisualStyleBackColor = true;
            btnRegister.Click += btnRegister_Click;
            // 
            // currentUserLabel
            // 
            currentUserLabel.AutoSize = true;
            currentUserLabel.Location = new Point(12, 9);
            currentUserLabel.Name = "currentUserLabel";
            currentUserLabel.Size = new Size(38, 15);
            currentUserLabel.TabIndex = 5;
            currentUserLabel.Text = "label1";
            // 
            // btnReviews
            // 
            btnReviews.Location = new Point(881, 359);
            btnReviews.Name = "btnReviews";
            btnReviews.Size = new Size(75, 23);
            btnReviews.TabIndex = 6;
            btnReviews.Text = "Reviews";
            btnReviews.UseVisualStyleBackColor = true;
            btnReviews.Click += btnReviews_Click;
            // 
            // btnAddPiwo
            // 
            btnAddPiwo.Location = new Point(881, 330);
            btnAddPiwo.Name = "btnAddPiwo";
            btnAddPiwo.Size = new Size(75, 23);
            btnAddPiwo.TabIndex = 7;
            btnAddPiwo.Text = "Add Piwko";
            btnAddPiwo.UseVisualStyleBackColor = true;
            btnAddPiwo.Click += btnAddPiwo_Click;
            // 
            // btnShowKlientObj
            // 
            btnShowKlientObj.Location = new Point(881, 285);
            btnShowKlientObj.Name = "btnShowKlientObj";
            btnShowKlientObj.Size = new Size(75, 39);
            btnShowKlientObj.TabIndex = 8;
            btnShowKlientObj.Text = "Explicit cursor";
            btnShowKlientObj.UseVisualStyleBackColor = true;
            btnShowKlientObj.Click += btnShowKlientObj_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(968, 450);
            Controls.Add(btnShowKlientObj);
            Controls.Add(btnAddPiwo);
            Controls.Add(btnReviews);
            Controls.Add(currentUserLabel);
            Controls.Add(btnRegister);
            Controls.Add(roleLabel);
            Controls.Add(btnPrihlasit);
            Controls.Add(comboBoxTables);
            Controls.Add(dataGridView1);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dataGridView1;
        private ComboBox comboBoxTables;
        private Button btnPrihlasit;
        private Label roleLabel;
        private Button btnRegister;
        private Label currentUserLabel;
        private Button btnReviews;
        private Button btnAddPiwo;
        private Button btnShowKlientObj;
    }
}
