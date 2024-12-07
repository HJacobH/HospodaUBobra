namespace HospodaUBobra
{
    partial class SpravaVlastnictvi
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
            cmbVlastnici = new ComboBox();
            label2 = new Label();
            label3 = new Label();
            cmbPivovary = new ComboBox();
            dataGridViewVlastnictvi = new DataGridView();
            txtSearch = new TextBox();
            dtpDatumPocatku = new DateTimePicker();
            label4 = new Label();
            btnBack = new Button();
            btnDelete = new Button();
            btnUpdate = new Button();
            btnAdd = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridViewVlastnictvi).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(12, 4);
            label1.Name = "label1";
            label1.Size = new Size(155, 21);
            label1.TabIndex = 0;
            label1.Text = "Správce Vlastnictví";
            // 
            // cmbVlastnici
            // 
            cmbVlastnici.FormattingEnabled = true;
            cmbVlastnici.Location = new Point(68, 41);
            cmbVlastnici.Name = "cmbVlastnici";
            cmbVlastnici.Size = new Size(121, 23);
            cmbVlastnici.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 44);
            label2.Name = "label2";
            label2.Size = new Size(50, 15);
            label2.TabIndex = 2;
            label2.Text = "Vlastvík:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 73);
            label3.Name = "label3";
            label3.Size = new Size(45, 15);
            label3.TabIndex = 3;
            label3.Text = "Vlastní:";
            // 
            // cmbPivovary
            // 
            cmbPivovary.FormattingEnabled = true;
            cmbPivovary.Location = new Point(68, 70);
            cmbPivovary.Name = "cmbPivovary";
            cmbPivovary.Size = new Size(121, 23);
            cmbPivovary.TabIndex = 4;
            // 
            // dataGridViewVlastnictvi
            // 
            dataGridViewVlastnictvi.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewVlastnictvi.Location = new Point(12, 161);
            dataGridViewVlastnictvi.Name = "dataGridViewVlastnictvi";
            dataGridViewVlastnictvi.Size = new Size(353, 250);
            dataGridViewVlastnictvi.TabIndex = 5;
            dataGridViewVlastnictvi.SelectionChanged += dataGridViewVlastnictvi_SelectionChanged;
            // 
            // txtSearch
            // 
            txtSearch.Location = new Point(12, 132);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(353, 23);
            txtSearch.TabIndex = 6;
            txtSearch.TextChanged += txtSearch_TextChanged;
            // 
            // dtpDatumPocatku
            // 
            dtpDatumPocatku.Location = new Point(126, 99);
            dtpDatumPocatku.Name = "dtpDatumPocatku";
            dtpDatumPocatku.Size = new Size(200, 23);
            dtpDatumPocatku.TabIndex = 7;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(12, 105);
            label4.Name = "label4";
            label4.Size = new Size(108, 15);
            label4.TabIndex = 8;
            label4.Text = "Počátek vlsatnictví:";
            // 
            // btnBack
            // 
            btnBack.Location = new Point(293, 417);
            btnBack.Name = "btnBack";
            btnBack.Size = new Size(72, 23);
            btnBack.TabIndex = 28;
            btnBack.Text = "Zpět";
            btnBack.UseVisualStyleBackColor = true;
            btnBack.Click += btnBack_Click;
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(186, 417);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(79, 23);
            btnDelete.TabIndex = 27;
            btnDelete.Text = "Odstranit";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;
            // 
            // btnUpdate
            // 
            btnUpdate.Location = new Point(86, 417);
            btnUpdate.Name = "btnUpdate";
            btnUpdate.Size = new Size(94, 23);
            btnUpdate.TabIndex = 26;
            btnUpdate.Text = "Aktualizovat";
            btnUpdate.UseVisualStyleBackColor = true;
            btnUpdate.Click += btnUpdate_Click;
            // 
            // btnAdd
            // 
            btnAdd.Location = new Point(12, 417);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(68, 23);
            btnAdd.TabIndex = 25;
            btnAdd.Text = "Přidat";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += btnAdd_Click;
            // 
            // SpravaVlastnictvi
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(382, 450);
            Controls.Add(btnBack);
            Controls.Add(btnDelete);
            Controls.Add(btnUpdate);
            Controls.Add(btnAdd);
            Controls.Add(label4);
            Controls.Add(dtpDatumPocatku);
            Controls.Add(txtSearch);
            Controls.Add(dataGridViewVlastnictvi);
            Controls.Add(cmbPivovary);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(cmbVlastnici);
            Controls.Add(label1);
            Name = "SpravaVlastnictvi";
            Text = "Správce Vlastvnictví";
            ((System.ComponentModel.ISupportInitialize)dataGridViewVlastnictvi).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private ComboBox cmbVlastnici;
        private Label label2;
        private Label label3;
        private ComboBox cmbPivovary;
        private DataGridView dataGridViewVlastnictvi;
        private TextBox txtSearch;
        private DateTimePicker dtpDatumPocatku;
        private Label label4;
        private Button btnBack;
        private Button btnDelete;
        private Button btnUpdate;
        private Button btnAdd;
    }
}