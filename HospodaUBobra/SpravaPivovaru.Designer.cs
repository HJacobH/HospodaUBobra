namespace HospodaUBobra
{
    partial class SpravaPivovaru
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
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            label7 = new Label();
            txtNazev = new TextBox();
            txtPopisAkci = new TextBox();
            cbDruhPodniku = new ComboBox();
            cbMestoVesnice = new ComboBox();
            cbRokZalozeni = new ComboBox();
            cbProvozProhlidek = new ComboBox();
            cbProvozAkci = new ComboBox();
            dgvBreweries = new DataGridView();
            btnBack = new Button();
            btnDelete = new Button();
            btnUpdate = new Button();
            btnAdd = new Button();
            btnClear = new Button();
            label8 = new Label();
            txtSearch = new TextBox();
            ((System.ComponentModel.ISupportInitialize)dgvBreweries).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(16, 37);
            label1.Name = "label1";
            label1.Size = new Size(42, 15);
            label1.TabIndex = 0;
            label1.Text = "Název:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(16, 69);
            label2.Name = "label2";
            label2.Size = new Size(75, 15);
            label2.TabIndex = 1;
            label2.Text = "Rok založení:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(16, 99);
            label3.Name = "label3";
            label3.Size = new Size(99, 15);
            label3.TabIndex = 2;
            label3.Text = "Provoz prohlídek:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(16, 127);
            label4.Name = "label4";
            label4.Size = new Size(70, 15);
            label4.TabIndex = 3;
            label4.Text = "Provoz akcí:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(16, 157);
            label5.Name = "label5";
            label5.Size = new Size(63, 15);
            label5.TabIndex = 4;
            label5.Text = "Popis akcí:";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(16, 221);
            label6.Name = "label6";
            label6.Size = new Size(83, 15);
            label6.TabIndex = 5;
            label6.Text = "Druh podniku:";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(16, 250);
            label7.Name = "label7";
            label7.Size = new Size(87, 15);
            label7.TabIndex = 6;
            label7.Text = "Město/Vesnice:";
            // 
            // txtNazev
            // 
            txtNazev.Location = new Point(133, 34);
            txtNazev.Name = "txtNazev";
            txtNazev.Size = new Size(100, 23);
            txtNazev.TabIndex = 7;
            // 
            // txtPopisAkci
            // 
            txtPopisAkci.Location = new Point(133, 154);
            txtPopisAkci.Multiline = true;
            txtPopisAkci.Name = "txtPopisAkci";
            txtPopisAkci.Size = new Size(335, 58);
            txtPopisAkci.TabIndex = 8;
            // 
            // cbDruhPodniku
            // 
            cbDruhPodniku.FormattingEnabled = true;
            cbDruhPodniku.Location = new Point(133, 218);
            cbDruhPodniku.Name = "cbDruhPodniku";
            cbDruhPodniku.Size = new Size(121, 23);
            cbDruhPodniku.TabIndex = 9;
            // 
            // cbMestoVesnice
            // 
            cbMestoVesnice.FormattingEnabled = true;
            cbMestoVesnice.Location = new Point(133, 247);
            cbMestoVesnice.Name = "cbMestoVesnice";
            cbMestoVesnice.Size = new Size(121, 23);
            cbMestoVesnice.TabIndex = 10;
            // 
            // cbRokZalozeni
            // 
            cbRokZalozeni.FormattingEnabled = true;
            cbRokZalozeni.Location = new Point(133, 66);
            cbRokZalozeni.Name = "cbRokZalozeni";
            cbRokZalozeni.Size = new Size(121, 23);
            cbRokZalozeni.TabIndex = 11;
            // 
            // cbProvozProhlidek
            // 
            cbProvozProhlidek.FormattingEnabled = true;
            cbProvozProhlidek.Location = new Point(133, 96);
            cbProvozProhlidek.Name = "cbProvozProhlidek";
            cbProvozProhlidek.Size = new Size(121, 23);
            cbProvozProhlidek.TabIndex = 12;
            // 
            // cbProvozAkci
            // 
            cbProvozAkci.FormattingEnabled = true;
            cbProvozAkci.Location = new Point(133, 124);
            cbProvozAkci.Name = "cbProvozAkci";
            cbProvozAkci.Size = new Size(121, 23);
            cbProvozAkci.TabIndex = 13;
            // 
            // dgvBreweries
            // 
            dgvBreweries.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvBreweries.Location = new Point(14, 307);
            dgvBreweries.Name = "dgvBreweries";
            dgvBreweries.Size = new Size(499, 246);
            dgvBreweries.TabIndex = 14;
            dgvBreweries.SelectionChanged += dgvBreweries_SelectionChanged;
            // 
            // btnBack
            // 
            btnBack.Location = new Point(441, 559);
            btnBack.Name = "btnBack";
            btnBack.Size = new Size(72, 23);
            btnBack.TabIndex = 28;
            btnBack.Text = "Zpět";
            btnBack.UseVisualStyleBackColor = true;
            btnBack.Click += btnBack_Click;
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(188, 559);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(79, 23);
            btnDelete.TabIndex = 27;
            btnDelete.Text = "Odstranit";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;
            // 
            // btnUpdate
            // 
            btnUpdate.Location = new Point(88, 559);
            btnUpdate.Name = "btnUpdate";
            btnUpdate.Size = new Size(94, 23);
            btnUpdate.TabIndex = 26;
            btnUpdate.Text = "Aktualizovat";
            btnUpdate.UseVisualStyleBackColor = true;
            btnUpdate.Click += btnUpdate_Click;
            // 
            // btnAdd
            // 
            btnAdd.Location = new Point(14, 559);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(68, 23);
            btnAdd.TabIndex = 25;
            btnAdd.Text = "Přidat";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += btnAdd_Click;
            // 
            // btnClear
            // 
            btnClear.Location = new Point(438, 9);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(75, 43);
            btnClear.TabIndex = 29;
            btnClear.Text = "Vyprázdnit Pole";
            btnClear.UseVisualStyleBackColor = true;
            btnClear.Click += btnClear_Click;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label8.Location = new Point(12, 4);
            label8.Name = "label8";
            label8.Size = new Size(142, 21);
            label8.TabIndex = 30;
            label8.Text = "Správce Pivovarů";
            // 
            // txtSearch
            // 
            txtSearch.Location = new Point(16, 278);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(497, 23);
            txtSearch.TabIndex = 31;
            txtSearch.TextChanged += txtSearch_TextChanged;
            // 
            // SpravaPivovaru
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(525, 588);
            Controls.Add(txtSearch);
            Controls.Add(label8);
            Controls.Add(btnClear);
            Controls.Add(btnBack);
            Controls.Add(btnDelete);
            Controls.Add(btnUpdate);
            Controls.Add(btnAdd);
            Controls.Add(dgvBreweries);
            Controls.Add(cbProvozAkci);
            Controls.Add(cbProvozProhlidek);
            Controls.Add(cbRokZalozeni);
            Controls.Add(cbMestoVesnice);
            Controls.Add(cbDruhPodniku);
            Controls.Add(txtPopisAkci);
            Controls.Add(txtNazev);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "SpravaPivovaru";
            Text = "Správce Pivovarů";
            ((System.ComponentModel.ISupportInitialize)dgvBreweries).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private TextBox txtNazev;
        private TextBox txtPopisAkci;
        private ComboBox cbDruhPodniku;
        private ComboBox cbMestoVesnice;
        private ComboBox cbRokZalozeni;
        private ComboBox cbProvozProhlidek;
        private ComboBox cbProvozAkci;
        private DataGridView dgvBreweries;
        private Button btnBack;
        private Button btnDelete;
        private Button btnUpdate;
        private Button btnAdd;
        private Button btnClear;
        private Label label8;
        private TextBox txtSearch;
    }
}