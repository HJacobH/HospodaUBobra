namespace HospodaUBobra
{
    partial class SpravaKlientu
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
            btnDelete = new Button();
            btnUpdate = new Button();
            dgvKlienti = new DataGridView();
            label5 = new Label();
            label4 = new Label();
            txtTelefon = new TextBox();
            txtEmail = new TextBox();
            btnBack = new Button();
            btnCreate = new Button();
            txtPassword = new TextBox();
            label2 = new Label();
            txtUsername = new TextBox();
            label1 = new Label();
            label3 = new Label();
            label6 = new Label();
            txtPrijmeni = new TextBox();
            cbDruhPodniku = new ComboBox();
            label7 = new Label();
            txtNazev = new TextBox();
            dtpDatumRegistrace = new DateTimePicker();
            label8 = new Label();
            btnClear = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvKlienti).BeginInit();
            SuspendLayout();
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(273, 550);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(118, 28);
            btnDelete.TabIndex = 29;
            btnDelete.Text = "Odstranit klienta";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;
            // 
            // btnUpdate
            // 
            btnUpdate.Location = new Point(130, 550);
            btnUpdate.Name = "btnUpdate";
            btnUpdate.Size = new Size(137, 28);
            btnUpdate.TabIndex = 28;
            btnUpdate.Text = "Aktualizovat klienta";
            btnUpdate.UseVisualStyleBackColor = true;
            btnUpdate.Click += btnUpdate_Click;
            // 
            // dgvKlienti
            // 
            dgvKlienti.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvKlienti.Location = new Point(10, 270);
            dgvKlienti.Name = "dgvKlienti";
            dgvKlienti.Size = new Size(549, 274);
            dgvKlienti.TabIndex = 27;
            dgvKlienti.SelectionChanged += dataGridViewUsers_SelectionChanged;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(10, 186);
            label5.Name = "label5";
            label5.Size = new Size(48, 15);
            label5.TabIndex = 26;
            label5.Text = "Telefon:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(12, 157);
            label4.Name = "label4";
            label4.Size = new Size(39, 15);
            label4.TabIndex = 25;
            label4.Text = "Email:";
            // 
            // txtTelefon
            // 
            txtTelefon.Location = new Point(114, 183);
            txtTelefon.Name = "txtTelefon";
            txtTelefon.Size = new Size(122, 23);
            txtTelefon.TabIndex = 24;
            // 
            // txtEmail
            // 
            txtEmail.Location = new Point(114, 154);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(122, 23);
            txtEmail.TabIndex = 23;
            // 
            // btnBack
            // 
            btnBack.Location = new Point(469, 550);
            btnBack.Name = "btnBack";
            btnBack.Size = new Size(90, 28);
            btnBack.TabIndex = 22;
            btnBack.Text = "Zpět";
            btnBack.UseVisualStyleBackColor = true;
            btnBack.Click += btnBack_Click;
            // 
            // btnCreate
            // 
            btnCreate.Location = new Point(10, 550);
            btnCreate.Name = "btnCreate";
            btnCreate.Size = new Size(114, 28);
            btnCreate.TabIndex = 21;
            btnCreate.Text = "Vytvořit klienta";
            btnCreate.UseVisualStyleBackColor = true;
            btnCreate.Click += btnCreate_Click;
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(114, 241);
            txtPassword.Name = "txtPassword";
            txtPassword.PasswordChar = '*';
            txtPassword.Size = new Size(122, 23);
            txtPassword.TabIndex = 18;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 244);
            label2.Name = "label2";
            label2.Size = new Size(40, 15);
            label2.TabIndex = 17;
            label2.Text = "Heslo:";
            // 
            // txtUsername
            // 
            txtUsername.Location = new Point(114, 36);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(122, 23);
            txtUsername.TabIndex = 16;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 39);
            label1.Name = "label1";
            label1.Size = new Size(45, 15);
            label1.TabIndex = 15;
            label1.Text = "Jméno:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 69);
            label3.Name = "label3";
            label3.Size = new Size(54, 15);
            label3.TabIndex = 30;
            label3.Text = "Příjmení:";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(12, 128);
            label6.Name = "label6";
            label6.Size = new Size(83, 15);
            label6.TabIndex = 31;
            label6.Text = "Druh podniku:";
            // 
            // txtPrijmeni
            // 
            txtPrijmeni.Location = new Point(114, 66);
            txtPrijmeni.Name = "txtPrijmeni";
            txtPrijmeni.Size = new Size(122, 23);
            txtPrijmeni.TabIndex = 32;
            // 
            // cbDruhPodniku
            // 
            cbDruhPodniku.FormattingEnabled = true;
            cbDruhPodniku.Location = new Point(114, 125);
            cbDruhPodniku.Name = "cbDruhPodniku";
            cbDruhPodniku.Size = new Size(122, 23);
            cbDruhPodniku.TabIndex = 33;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(12, 99);
            label7.Name = "label7";
            label7.Size = new Size(42, 15);
            label7.TabIndex = 34;
            label7.Text = "Název:";
            // 
            // txtNazev
            // 
            txtNazev.Location = new Point(114, 96);
            txtNazev.Name = "txtNazev";
            txtNazev.Size = new Size(122, 23);
            txtNazev.TabIndex = 35;
            // 
            // dtpDatumRegistrace
            // 
            dtpDatumRegistrace.Location = new Point(114, 212);
            dtpDatumRegistrace.Name = "dtpDatumRegistrace";
            dtpDatumRegistrace.Size = new Size(122, 23);
            dtpDatumRegistrace.TabIndex = 36;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(12, 218);
            label8.Name = "label8";
            label8.Size = new Size(100, 15);
            label8.TabIndex = 37;
            label8.Text = "Datum registrace:";
            // 
            // btnClear
            // 
            btnClear.Location = new Point(484, 11);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(75, 43);
            btnClear.TabIndex = 38;
            btnClear.Text = "Vyprázdnit Pole";
            btnClear.UseVisualStyleBackColor = true;
            btnClear.Click += btnClear_Click;
            // 
            // SpravaKlientu
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(571, 590);
            Controls.Add(btnClear);
            Controls.Add(label8);
            Controls.Add(dtpDatumRegistrace);
            Controls.Add(txtNazev);
            Controls.Add(label7);
            Controls.Add(cbDruhPodniku);
            Controls.Add(txtPrijmeni);
            Controls.Add(label6);
            Controls.Add(label3);
            Controls.Add(btnDelete);
            Controls.Add(btnUpdate);
            Controls.Add(dgvKlienti);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(txtTelefon);
            Controls.Add(txtEmail);
            Controls.Add(btnBack);
            Controls.Add(btnCreate);
            Controls.Add(txtPassword);
            Controls.Add(label2);
            Controls.Add(txtUsername);
            Controls.Add(label1);
            Name = "SpravaKlientu";
            Text = "SpravaKlientu";
            ((System.ComponentModel.ISupportInitialize)dgvKlienti).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnDelete;
        private Button btnUpdate;
        private DataGridView dgvKlienti;
        private Label label5;
        private Label label4;
        private TextBox txtTelefon;
        private TextBox txtEmail;
        private Button btnBack;
        private Button btnCreate;
        private TextBox txtPassword;
        private Label label2;
        private TextBox txtUsername;
        private Label label1;
        private Label label3;
        private Label label6;
        private TextBox txtPrijmeni;
        private ComboBox cbDruhPodniku;
        private Label label7;
        private TextBox txtNazev;
        private DateTimePicker dtpDatumRegistrace;
        private Label label8;
        private Button btnClear;
    }
}