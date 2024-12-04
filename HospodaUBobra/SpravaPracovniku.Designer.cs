namespace HospodaUBobra
{
    partial class SpravaPracovniku
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
            label3 = new Label();
            cbZamestnanci = new ComboBox();
            cbPivovary = new ComboBox();
            dataGridViewPracovnici = new DataGridView();
            btnBack = new Button();
            btnDelete = new Button();
            btnUpdate = new Button();
            btnAdd = new Button();
            txtSearch = new TextBox();
            ((System.ComponentModel.ISupportInitialize)dataGridViewPracovnici).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(12, 4);
            label1.Name = "label1";
            label1.Size = new Size(160, 21);
            label1.TabIndex = 0;
            label1.Text = "Správce pracovníků";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 45);
            label2.Name = "label2";
            label2.Size = new Size(81, 15);
            label2.TabIndex = 1;
            label2.Text = "Zaměstnanec:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 73);
            label3.Name = "label3";
            label3.Size = new Size(58, 15);
            label3.TabIndex = 2;
            label3.Text = "Pracuje v:";
            // 
            // cbZamestnanci
            // 
            cbZamestnanci.FormattingEnabled = true;
            cbZamestnanci.Location = new Point(99, 42);
            cbZamestnanci.Name = "cbZamestnanci";
            cbZamestnanci.Size = new Size(121, 23);
            cbZamestnanci.TabIndex = 3;
            // 
            // cbPivovary
            // 
            cbPivovary.FormattingEnabled = true;
            cbPivovary.Location = new Point(76, 70);
            cbPivovary.Name = "cbPivovary";
            cbPivovary.Size = new Size(121, 23);
            cbPivovary.TabIndex = 4;
            // 
            // dataGridViewPracovnici
            // 
            dataGridViewPracovnici.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewPracovnici.Location = new Point(12, 133);
            dataGridViewPracovnici.Name = "dataGridViewPracovnici";
            dataGridViewPracovnici.Size = new Size(346, 282);
            dataGridViewPracovnici.TabIndex = 5;
            dataGridViewPracovnici.SelectionChanged += dataGridViewPracovnici_SelectionChanged;
            // 
            // btnBack
            // 
            btnBack.Location = new Point(283, 421);
            btnBack.Name = "btnBack";
            btnBack.Size = new Size(75, 23);
            btnBack.TabIndex = 13;
            btnBack.Text = "Zpět";
            btnBack.UseVisualStyleBackColor = true;
            btnBack.Click += btnBack_Click;
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(189, 421);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(75, 23);
            btnDelete.TabIndex = 12;
            btnDelete.Text = "Odstranit";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;
            // 
            // btnUpdate
            // 
            btnUpdate.Location = new Point(93, 421);
            btnUpdate.Name = "btnUpdate";
            btnUpdate.Size = new Size(90, 23);
            btnUpdate.TabIndex = 11;
            btnUpdate.Text = "Aktualizovat";
            btnUpdate.UseVisualStyleBackColor = true;
            btnUpdate.Click += btnUpdate_Click;
            // 
            // btnAdd
            // 
            btnAdd.Location = new Point(12, 421);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(75, 23);
            btnAdd.TabIndex = 10;
            btnAdd.Text = "Přidat";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += btnAdd_Click;
            // 
            // txtSearch
            // 
            txtSearch.Location = new Point(12, 104);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(346, 23);
            txtSearch.TabIndex = 14;
            txtSearch.TextChanged += txtSearch_TextChanged;
            // 
            // SpravaPracovniku
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(370, 450);
            Controls.Add(txtSearch);
            Controls.Add(btnBack);
            Controls.Add(btnDelete);
            Controls.Add(btnUpdate);
            Controls.Add(btnAdd);
            Controls.Add(dataGridViewPracovnici);
            Controls.Add(cbPivovary);
            Controls.Add(cbZamestnanci);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "SpravaPracovniku";
            Text = "Správce Pracovníků";
            ((System.ComponentModel.ISupportInitialize)dataGridViewPracovnici).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private ComboBox cbZamestnanci;
        private ComboBox cbPivovary;
        private DataGridView dataGridViewPracovnici;
        private Button btnBack;
        private Button btnDelete;
        private Button btnUpdate;
        private Button btnAdd;
        private TextBox txtSearch;
    }
}