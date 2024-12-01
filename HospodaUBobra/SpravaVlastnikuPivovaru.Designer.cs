namespace HospodaUBobra
{
    partial class SpravaVlastnikuPivovaru
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
            txtJmenoNazev = new TextBox();
            txtPrijmeni = new TextBox();
            txtUlice = new TextBox();
            txtCisloPopisne = new TextBox();
            txtICO = new TextBox();
            txtDIC = new TextBox();
            cbMestoVesnice = new ComboBox();
            cbDruhVlastnika = new ComboBox();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            label7 = new Label();
            label8 = new Label();
            dgvOwners = new DataGridView();
            btnBack = new Button();
            btnDelete = new Button();
            btnUpdate = new Button();
            btnAdd = new Button();
            btnClear = new Button();
            button1 = new Button();
            cbMestaAudit = new ComboBox();
            cbDruhVlastnikaAudit = new ComboBox();
            cbPivovarAudit = new ComboBox();
            ((System.ComponentModel.ISupportInitialize)dgvOwners).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 24);
            label1.Name = "label1";
            label1.Size = new Size(45, 15);
            label1.TabIndex = 0;
            label1.Text = "Jméno:";
            // 
            // txtJmenoNazev
            // 
            txtJmenoNazev.Location = new Point(108, 21);
            txtJmenoNazev.Name = "txtJmenoNazev";
            txtJmenoNazev.Size = new Size(100, 23);
            txtJmenoNazev.TabIndex = 1;
            // 
            // txtPrijmeni
            // 
            txtPrijmeni.Location = new Point(108, 50);
            txtPrijmeni.Name = "txtPrijmeni";
            txtPrijmeni.Size = new Size(100, 23);
            txtPrijmeni.TabIndex = 2;
            // 
            // txtUlice
            // 
            txtUlice.Location = new Point(108, 79);
            txtUlice.Name = "txtUlice";
            txtUlice.Size = new Size(100, 23);
            txtUlice.TabIndex = 3;
            // 
            // txtCisloPopisne
            // 
            txtCisloPopisne.Location = new Point(108, 108);
            txtCisloPopisne.Name = "txtCisloPopisne";
            txtCisloPopisne.Size = new Size(100, 23);
            txtCisloPopisne.TabIndex = 4;
            // 
            // txtICO
            // 
            txtICO.Location = new Point(108, 137);
            txtICO.Name = "txtICO";
            txtICO.Size = new Size(100, 23);
            txtICO.TabIndex = 5;
            // 
            // txtDIC
            // 
            txtDIC.Location = new Point(108, 166);
            txtDIC.Name = "txtDIC";
            txtDIC.Size = new Size(100, 23);
            txtDIC.TabIndex = 6;
            // 
            // cbMestoVesnice
            // 
            cbMestoVesnice.FormattingEnabled = true;
            cbMestoVesnice.Location = new Point(108, 195);
            cbMestoVesnice.Name = "cbMestoVesnice";
            cbMestoVesnice.Size = new Size(100, 23);
            cbMestoVesnice.TabIndex = 7;
            // 
            // cbDruhVlastnika
            // 
            cbDruhVlastnika.FormattingEnabled = true;
            cbDruhVlastnika.Location = new Point(108, 224);
            cbDruhVlastnika.Name = "cbDruhVlastnika";
            cbDruhVlastnika.Size = new Size(100, 23);
            cbDruhVlastnika.TabIndex = 8;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 53);
            label2.Name = "label2";
            label2.Size = new Size(54, 15);
            label2.TabIndex = 9;
            label2.Text = "Příjmení:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 82);
            label3.Name = "label3";
            label3.Size = new Size(36, 15);
            label3.TabIndex = 10;
            label3.Text = "Ulice:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(12, 111);
            label4.Name = "label4";
            label4.Size = new Size(81, 15);
            label4.TabIndex = 11;
            label4.Text = "Číslo popisné:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(12, 140);
            label5.Name = "label5";
            label5.Size = new Size(30, 15);
            label5.TabIndex = 12;
            label5.Text = "ICO:";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(12, 169);
            label6.Name = "label6";
            label6.Size = new Size(29, 15);
            label6.TabIndex = 13;
            label6.Text = "DIC:";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(12, 198);
            label7.Name = "label7";
            label7.Size = new Size(87, 15);
            label7.TabIndex = 14;
            label7.Text = "Město/Vesnice:";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(12, 227);
            label8.Name = "label8";
            label8.Size = new Size(85, 15);
            label8.TabIndex = 15;
            label8.Text = "Druh vlastníka:";
            // 
            // dgvOwners
            // 
            dgvOwners.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvOwners.Location = new Point(12, 253);
            dgvOwners.Name = "dgvOwners";
            dgvOwners.Size = new Size(600, 311);
            dgvOwners.TabIndex = 16;
            dgvOwners.SelectionChanged += dgvOwners_SelectionChanged;
            // 
            // btnBack
            // 
            btnBack.Location = new Point(540, 570);
            btnBack.Name = "btnBack";
            btnBack.Size = new Size(72, 23);
            btnBack.TabIndex = 32;
            btnBack.Text = "Zpět";
            btnBack.UseVisualStyleBackColor = true;
            btnBack.Click += btnBack_Click;
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(186, 570);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(79, 23);
            btnDelete.TabIndex = 31;
            btnDelete.Text = "Odstranit";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;
            // 
            // btnUpdate
            // 
            btnUpdate.Location = new Point(86, 570);
            btnUpdate.Name = "btnUpdate";
            btnUpdate.Size = new Size(94, 23);
            btnUpdate.TabIndex = 30;
            btnUpdate.Text = "Aktualizovat";
            btnUpdate.UseVisualStyleBackColor = true;
            btnUpdate.Click += btnUpdate_Click;
            // 
            // btnAdd
            // 
            btnAdd.Location = new Point(12, 570);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(68, 23);
            btnAdd.TabIndex = 29;
            btnAdd.Text = "Přidat";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += btnAdd_Click;
            // 
            // btnClear
            // 
            btnClear.Location = new Point(537, 10);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(75, 43);
            btnClear.TabIndex = 33;
            btnClear.Text = "Vyprázdnit Pole";
            btnClear.UseVisualStyleBackColor = true;
            btnClear.Click += btnClear_Click;
            // 
            // button1
            // 
            button1.Location = new Point(537, 74);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 34;
            button1.Text = "button1";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // cbMestaAudit
            // 
            cbMestaAudit.FormattingEnabled = true;
            cbMestaAudit.Location = new Point(363, 50);
            cbMestaAudit.Name = "cbMestaAudit";
            cbMestaAudit.Size = new Size(100, 23);
            cbMestaAudit.TabIndex = 35;
            // 
            // cbDruhVlastnikaAudit
            // 
            cbDruhVlastnikaAudit.FormattingEnabled = true;
            cbDruhVlastnikaAudit.Location = new Point(363, 21);
            cbDruhVlastnikaAudit.Name = "cbDruhVlastnikaAudit";
            cbDruhVlastnikaAudit.Size = new Size(100, 23);
            cbDruhVlastnikaAudit.TabIndex = 36;
            // 
            // cbPivovarAudit
            // 
            cbPivovarAudit.FormattingEnabled = true;
            cbPivovarAudit.Location = new Point(363, 79);
            cbPivovarAudit.Name = "cbPivovarAudit";
            cbPivovarAudit.Size = new Size(100, 23);
            cbPivovarAudit.TabIndex = 37;
            // 
            // SpravaVlastnikuPivovaru
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(626, 601);
            Controls.Add(cbPivovarAudit);
            Controls.Add(cbDruhVlastnikaAudit);
            Controls.Add(cbMestaAudit);
            Controls.Add(button1);
            Controls.Add(btnClear);
            Controls.Add(btnBack);
            Controls.Add(btnDelete);
            Controls.Add(btnUpdate);
            Controls.Add(btnAdd);
            Controls.Add(dgvOwners);
            Controls.Add(label8);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(cbDruhVlastnika);
            Controls.Add(cbMestoVesnice);
            Controls.Add(txtDIC);
            Controls.Add(txtICO);
            Controls.Add(txtCisloPopisne);
            Controls.Add(txtUlice);
            Controls.Add(txtPrijmeni);
            Controls.Add(txtJmenoNazev);
            Controls.Add(label1);
            Name = "SpravaVlastnikuPivovaru";
            Text = "SpravaVlastnikuPivovaru";
            ((System.ComponentModel.ISupportInitialize)dgvOwners).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox txtJmenoNazev;
        private TextBox txtPrijmeni;
        private TextBox txtUlice;
        private TextBox txtCisloPopisne;
        private TextBox txtICO;
        private TextBox txtDIC;
        private ComboBox cbMestoVesnice;
        private ComboBox cbDruhVlastnika;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private DataGridView dgvOwners;
        private Button btnBack;
        private Button btnDelete;
        private Button btnUpdate;
        private Button btnAdd;
        private Button btnClear;
        private Button button1;
        private ComboBox cbMestaAudit;
        private ComboBox cbDruhVlastnikaAudit;
        private ComboBox cbPivovarAudit;
    }
}