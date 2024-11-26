namespace HospodaUBobra
{
    partial class EmplyeeManagement
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
            txtJmeno = new TextBox();
            txtPrijmeni = new TextBox();
            dateTimePickerNarozeni = new DateTimePicker();
            txtPozice = new TextBox();
            txtVyplata = new TextBox();
            dateTimePickerStartWorking = new DateTimePicker();
            txtFavBeer = new TextBox();
            lablel2 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            dgvZamestnanci = new DataGridView();
            btnAdd = new Button();
            btnUpdate = new Button();
            btnDelete = new Button();
            btnBack = new Button();
            cbPozice = new ComboBox();
            ((System.ComponentModel.ISupportInitialize)dgvZamestnanci).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 35);
            label1.Name = "label1";
            label1.Size = new Size(45, 15);
            label1.TabIndex = 0;
            label1.Text = "Jméno:";
            // 
            // txtJmeno
            // 
            txtJmeno.Location = new Point(111, 32);
            txtJmeno.Name = "txtJmeno";
            txtJmeno.Size = new Size(144, 23);
            txtJmeno.TabIndex = 1;
            // 
            // txtPrijmeni
            // 
            txtPrijmeni.Location = new Point(111, 61);
            txtPrijmeni.Name = "txtPrijmeni";
            txtPrijmeni.Size = new Size(144, 23);
            txtPrijmeni.TabIndex = 2;
            // 
            // dateTimePickerNarozeni
            // 
            dateTimePickerNarozeni.Location = new Point(111, 90);
            dateTimePickerNarozeni.Name = "dateTimePickerNarozeni";
            dateTimePickerNarozeni.Size = new Size(144, 23);
            dateTimePickerNarozeni.TabIndex = 3;
            // 
            // txtPozice
            // 
            txtPozice.Location = new Point(111, 119);
            txtPozice.Name = "txtPozice";
            txtPozice.Size = new Size(100, 23);
            txtPozice.TabIndex = 4;
            // 
            // txtVyplata
            // 
            txtVyplata.Location = new Point(111, 148);
            txtVyplata.Name = "txtVyplata";
            txtVyplata.Size = new Size(100, 23);
            txtVyplata.TabIndex = 5;
            // 
            // dateTimePickerStartWorking
            // 
            dateTimePickerStartWorking.Location = new Point(111, 177);
            dateTimePickerStartWorking.Name = "dateTimePickerStartWorking";
            dateTimePickerStartWorking.Size = new Size(144, 23);
            dateTimePickerStartWorking.TabIndex = 6;
            // 
            // txtFavBeer
            // 
            txtFavBeer.Location = new Point(111, 206);
            txtFavBeer.Name = "txtFavBeer";
            txtFavBeer.Size = new Size(100, 23);
            txtFavBeer.TabIndex = 7;
            // 
            // lablel2
            // 
            lablel2.AutoSize = true;
            lablel2.Location = new Point(12, 64);
            lablel2.Name = "lablel2";
            lablel2.Size = new Size(54, 15);
            lablel2.TabIndex = 8;
            lablel2.Text = "Příjmení:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 96);
            label2.Name = "label2";
            label2.Size = new Size(94, 15);
            label2.TabIndex = 9;
            label2.Text = "Datum narození:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 122);
            label3.Name = "label3";
            label3.Size = new Size(44, 15);
            label3.TabIndex = 10;
            label3.Text = "Pozice:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(12, 151);
            label4.Name = "label4";
            label4.Size = new Size(46, 15);
            label4.TabIndex = 11;
            label4.Text = "Výplata";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(12, 183);
            label5.Name = "label5";
            label5.Size = new Size(92, 15);
            label5.TabIndex = 12;
            label5.Text = "Datum nástupu:";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(12, 209);
            label6.Name = "label6";
            label6.Size = new Size(83, 15);
            label6.TabIndex = 13;
            label6.Text = "Oblíbená piva:";
            // 
            // dgvZamestnanci
            // 
            dgvZamestnanci.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvZamestnanci.Location = new Point(15, 235);
            dgvZamestnanci.Name = "dgvZamestnanci";
            dgvZamestnanci.Size = new Size(579, 296);
            dgvZamestnanci.TabIndex = 14;
            dgvZamestnanci.SelectionChanged += dgvZamestnanci_SelectionChanged;
            // 
            // btnAdd
            // 
            btnAdd.Location = new Point(15, 540);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(75, 23);
            btnAdd.TabIndex = 15;
            btnAdd.Text = "Přidat";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += btnAdd_Click;
            // 
            // btnUpdate
            // 
            btnUpdate.Location = new Point(96, 540);
            btnUpdate.Name = "btnUpdate";
            btnUpdate.Size = new Size(88, 23);
            btnUpdate.TabIndex = 16;
            btnUpdate.Text = "Aktualizovat";
            btnUpdate.UseVisualStyleBackColor = true;
            btnUpdate.Click += btnUpdate_Click;
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(190, 540);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(75, 23);
            btnDelete.TabIndex = 17;
            btnDelete.Text = "Odstranit";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;
            // 
            // btnBack
            // 
            btnBack.Location = new Point(519, 540);
            btnBack.Name = "btnBack";
            btnBack.Size = new Size(75, 23);
            btnBack.TabIndex = 18;
            btnBack.Text = "Zpět";
            btnBack.UseVisualStyleBackColor = true;
            btnBack.Click += btnBack_Click;
            // 
            // cbPozice
            // 
            cbPozice.FormattingEnabled = true;
            cbPozice.Location = new Point(217, 119);
            cbPozice.Name = "cbPozice";
            cbPozice.Size = new Size(121, 23);
            cbPozice.TabIndex = 19;
            // 
            // EmplyeeManagement
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(606, 575);
            Controls.Add(cbPozice);
            Controls.Add(btnBack);
            Controls.Add(btnDelete);
            Controls.Add(btnUpdate);
            Controls.Add(btnAdd);
            Controls.Add(dgvZamestnanci);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(lablel2);
            Controls.Add(txtFavBeer);
            Controls.Add(dateTimePickerStartWorking);
            Controls.Add(txtVyplata);
            Controls.Add(txtPozice);
            Controls.Add(dateTimePickerNarozeni);
            Controls.Add(txtPrijmeni);
            Controls.Add(txtJmeno);
            Controls.Add(label1);
            Name = "EmplyeeManagement";
            Text = "EmplyeeManagement";
            ((System.ComponentModel.ISupportInitialize)dgvZamestnanci).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox txtJmeno;
        private TextBox txtPrijmeni;
        private DateTimePicker dateTimePickerNarozeni;
        private TextBox txtPozice;
        private TextBox txtVyplata;
        private DateTimePicker dateTimePickerStartWorking;
        private TextBox txtFavBeer;
        private Label lablel2;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private DataGridView dgvZamestnanci;
        private Button btnAdd;
        private Button btnUpdate;
        private Button btnDelete;
        private Button btnBack;
        private ComboBox cbPozice;
    }
}