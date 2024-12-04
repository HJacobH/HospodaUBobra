namespace HospodaUBobra
{
    partial class SpravaPozicePracovnika
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
            comboBoxNazevPozice = new ComboBox();
            label3 = new Label();
            comboBoxParentPozice = new ComboBox();
            dataGridViewPozice = new DataGridView();
            btnAdd = new Button();
            btnUpdate = new Button();
            btnDelete = new Button();
            btnBack = new Button();
            textBoxNazevPozice = new TextBox();
            label4 = new Label();
            txtSearch = new TextBox();
            ((System.ComponentModel.ISupportInitialize)dataGridViewPozice).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(12, 4);
            label1.Name = "label1";
            label1.Size = new Size(205, 21);
            label1.TabIndex = 0;
            label1.Text = "Správce pozic pracovníků";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 46);
            label2.Name = "label2";
            label2.Size = new Size(44, 15);
            label2.TabIndex = 1;
            label2.Text = "Pozice:";
            // 
            // comboBoxNazevPozice
            // 
            comboBoxNazevPozice.FormattingEnabled = true;
            comboBoxNazevPozice.Location = new Point(62, 43);
            comboBoxNazevPozice.Name = "comboBoxNazevPozice";
            comboBoxNazevPozice.Size = new Size(121, 23);
            comboBoxNazevPozice.TabIndex = 2;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 80);
            label3.Name = "label3";
            label3.Size = new Size(70, 15);
            label3.TabIndex = 3;
            label3.Text = "Pracuje pro:";
            // 
            // comboBoxParentPozice
            // 
            comboBoxParentPozice.FormattingEnabled = true;
            comboBoxParentPozice.Location = new Point(88, 77);
            comboBoxParentPozice.Name = "comboBoxParentPozice";
            comboBoxParentPozice.Size = new Size(121, 23);
            comboBoxParentPozice.TabIndex = 4;
            // 
            // dataGridViewPozice
            // 
            dataGridViewPozice.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewPozice.Location = new Point(12, 141);
            dataGridViewPozice.Name = "dataGridViewPozice";
            dataGridViewPozice.Size = new Size(357, 276);
            dataGridViewPozice.TabIndex = 5;
            dataGridViewPozice.SelectionChanged += dataGridViewPozice_SelectionChanged;
            // 
            // btnAdd
            // 
            btnAdd.Location = new Point(12, 423);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(75, 23);
            btnAdd.TabIndex = 6;
            btnAdd.Text = "Přidat";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += btnAdd_Click;
            // 
            // btnUpdate
            // 
            btnUpdate.Location = new Point(93, 423);
            btnUpdate.Name = "btnUpdate";
            btnUpdate.Size = new Size(90, 23);
            btnUpdate.TabIndex = 7;
            btnUpdate.Text = "Aktualizovat";
            btnUpdate.UseVisualStyleBackColor = true;
            btnUpdate.Click += btnUpdate_Click;
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(189, 423);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(75, 23);
            btnDelete.TabIndex = 8;
            btnDelete.Text = "Odstranit";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;
            // 
            // btnBack
            // 
            btnBack.Location = new Point(294, 423);
            btnBack.Name = "btnBack";
            btnBack.Size = new Size(75, 23);
            btnBack.TabIndex = 9;
            btnBack.Text = "Zpět";
            btnBack.UseVisualStyleBackColor = true;
            btnBack.Click += btnBack_Click;
            // 
            // textBoxNazevPozice
            // 
            textBoxNazevPozice.Location = new Point(261, 61);
            textBoxNazevPozice.Name = "textBoxNazevPozice";
            textBoxNazevPozice.Size = new Size(108, 23);
            textBoxNazevPozice.TabIndex = 10;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(261, 43);
            label4.Name = "label4";
            label4.Size = new Size(90, 15);
            label4.TabIndex = 11;
            label4.Text = "Vybraná pozice:";
            // 
            // txtSearch
            // 
            txtSearch.Location = new Point(12, 112);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(357, 23);
            txtSearch.TabIndex = 12;
            txtSearch.TextChanged += txtSearch_TextChanged;
            // 
            // SpravaPozicePracovnika
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(381, 450);
            Controls.Add(txtSearch);
            Controls.Add(label4);
            Controls.Add(textBoxNazevPozice);
            Controls.Add(btnBack);
            Controls.Add(btnDelete);
            Controls.Add(btnUpdate);
            Controls.Add(btnAdd);
            Controls.Add(dataGridViewPozice);
            Controls.Add(comboBoxParentPozice);
            Controls.Add(label3);
            Controls.Add(comboBoxNazevPozice);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "SpravaPozicePracovnika";
            Text = "Správce Pozic Pracovníka";
            ((System.ComponentModel.ISupportInitialize)dataGridViewPozice).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private ComboBox comboBoxNazevPozice;
        private Label label3;
        private ComboBox comboBoxParentPozice;
        private DataGridView dataGridViewPozice;
        private Button btnAdd;
        private Button btnUpdate;
        private Button btnDelete;
        private Button btnBack;
        private TextBox textBoxNazevPozice;
        private Label label4;
        private TextBox txtSearch;
    }
}