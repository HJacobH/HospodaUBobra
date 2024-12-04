namespace HospodaUBobra
{
    partial class SpravaMestVesnic
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
            txtNazev = new TextBox();
            txtPocetObyvatel = new TextBox();
            txtPSC = new TextBox();
            cbOkresy = new ComboBox();
            cbKraje = new ComboBox();
            dgvMestaVesnice = new DataGridView();
            btnBack = new Button();
            btnDelete = new Button();
            btnUpdate = new Button();
            btnAdd = new Button();
            btnClear = new Button();
            label6 = new Label();
            txtSearch = new TextBox();
            ((System.ComponentModel.ISupportInitialize)dgvMestaVesnice).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(11, 35);
            label1.Name = "label1";
            label1.Size = new Size(42, 15);
            label1.TabIndex = 0;
            label1.Text = "Nazev:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(11, 62);
            label2.Name = "label2";
            label2.Size = new Size(88, 15);
            label2.TabIndex = 1;
            label2.Text = "Počet obyvatel:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(11, 94);
            label3.Name = "label3";
            label3.Size = new Size(31, 15);
            label3.TabIndex = 2;
            label3.Text = "PSČ:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(12, 123);
            label4.Name = "label4";
            label4.Size = new Size(40, 15);
            label4.TabIndex = 3;
            label4.Text = "Okres:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(12, 152);
            label5.Name = "label5";
            label5.Size = new Size(30, 15);
            label5.TabIndex = 4;
            label5.Text = "Kraj:";
            // 
            // txtNazev
            // 
            txtNazev.Location = new Point(108, 32);
            txtNazev.Name = "txtNazev";
            txtNazev.Size = new Size(100, 23);
            txtNazev.TabIndex = 5;
            // 
            // txtPocetObyvatel
            // 
            txtPocetObyvatel.Location = new Point(108, 59);
            txtPocetObyvatel.Name = "txtPocetObyvatel";
            txtPocetObyvatel.Size = new Size(100, 23);
            txtPocetObyvatel.TabIndex = 6;
            // 
            // txtPSC
            // 
            txtPSC.Location = new Point(108, 91);
            txtPSC.Name = "txtPSC";
            txtPSC.Size = new Size(100, 23);
            txtPSC.TabIndex = 7;
            // 
            // cbOkresy
            // 
            cbOkresy.FormattingEnabled = true;
            cbOkresy.Location = new Point(108, 120);
            cbOkresy.Name = "cbOkresy";
            cbOkresy.Size = new Size(100, 23);
            cbOkresy.TabIndex = 8;
            // 
            // cbKraje
            // 
            cbKraje.FormattingEnabled = true;
            cbKraje.Location = new Point(108, 149);
            cbKraje.Name = "cbKraje";
            cbKraje.Size = new Size(100, 23);
            cbKraje.TabIndex = 9;
            // 
            // dgvMestaVesnice
            // 
            dgvMestaVesnice.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvMestaVesnice.Location = new Point(11, 208);
            dgvMestaVesnice.Name = "dgvMestaVesnice";
            dgvMestaVesnice.Size = new Size(449, 229);
            dgvMestaVesnice.TabIndex = 10;
            dgvMestaVesnice.SelectionChanged += dgvMestaVesnice_SelectionChanged;
            // 
            // btnBack
            // 
            btnBack.Location = new Point(385, 443);
            btnBack.Name = "btnBack";
            btnBack.Size = new Size(75, 23);
            btnBack.TabIndex = 20;
            btnBack.Text = "Zpět";
            btnBack.UseVisualStyleBackColor = true;
            btnBack.Click += btnBack_Click;
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(185, 443);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(79, 23);
            btnDelete.TabIndex = 19;
            btnDelete.Text = "Odstranit";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;
            // 
            // btnUpdate
            // 
            btnUpdate.Location = new Point(85, 443);
            btnUpdate.Name = "btnUpdate";
            btnUpdate.Size = new Size(94, 23);
            btnUpdate.TabIndex = 18;
            btnUpdate.Text = "Aktualizovat";
            btnUpdate.UseVisualStyleBackColor = true;
            btnUpdate.Click += btnUpdate_Click;
            // 
            // btnAdd
            // 
            btnAdd.Location = new Point(11, 443);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(68, 23);
            btnAdd.TabIndex = 17;
            btnAdd.Text = "Přidat";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += btnAdd_Click;
            // 
            // btnClear
            // 
            btnClear.Location = new Point(385, 12);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(75, 43);
            btnClear.TabIndex = 21;
            btnClear.Text = "Vyprázdnit Pole";
            btnClear.UseVisualStyleBackColor = true;
            btnClear.Click += btnClear_Click;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label6.Location = new Point(12, 4);
            label6.Name = "label6";
            label6.Size = new Size(177, 21);
            label6.TabIndex = 22;
            label6.Text = "Správce Měst a Vesnic";
            // 
            // txtSearch
            // 
            txtSearch.Location = new Point(12, 179);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(448, 23);
            txtSearch.TabIndex = 23;
            txtSearch.TextChanged += txtSearch_TextChanged;
            // 
            // SpravaMestVesnic
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(472, 474);
            Controls.Add(txtSearch);
            Controls.Add(label6);
            Controls.Add(btnClear);
            Controls.Add(btnBack);
            Controls.Add(btnDelete);
            Controls.Add(btnUpdate);
            Controls.Add(btnAdd);
            Controls.Add(dgvMestaVesnice);
            Controls.Add(cbKraje);
            Controls.Add(cbOkresy);
            Controls.Add(txtPSC);
            Controls.Add(txtPocetObyvatel);
            Controls.Add(txtNazev);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "SpravaMestVesnic";
            Text = "Správce Měst a Vesnic";
            ((System.ComponentModel.ISupportInitialize)dgvMestaVesnice).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private TextBox txtNazev;
        private TextBox txtPocetObyvatel;
        private TextBox txtPSC;
        private ComboBox cbOkresy;
        private ComboBox cbKraje;
        private DataGridView dgvMestaVesnice;
        private Button btnBack;
        private Button btnDelete;
        private Button btnUpdate;
        private Button btnAdd;
        private Button btnClear;
        private Label label6;
        private TextBox txtSearch;
    }
}