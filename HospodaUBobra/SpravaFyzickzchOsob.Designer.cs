﻿namespace HospodaUBobra
{
    partial class SpravaFyzickzchOsob
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
            comboBoxVlastnikPivovaru = new ComboBox();
            dateTimePickerDatumNarozeni = new DateTimePicker();
            textBoxRodneCislo = new TextBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            dgvFyzickeOsoby = new DataGridView();
            btnAdd = new Button();
            btnUpdate = new Button();
            btnDelete = new Button();
            btnBack = new Button();
            label4 = new Label();
            txtSearch = new TextBox();
            ((System.ComponentModel.ISupportInitialize)dgvFyzickeOsoby).BeginInit();
            SuspendLayout();
            // 
            // comboBoxVlastnikPivovaru
            // 
            comboBoxVlastnikPivovaru.FormattingEnabled = true;
            comboBoxVlastnikPivovaru.Location = new Point(130, 35);
            comboBoxVlastnikPivovaru.Name = "comboBoxVlastnikPivovaru";
            comboBoxVlastnikPivovaru.Size = new Size(121, 23);
            comboBoxVlastnikPivovaru.TabIndex = 0;
            // 
            // dateTimePickerDatumNarozeni
            // 
            dateTimePickerDatumNarozeni.Location = new Point(130, 64);
            dateTimePickerDatumNarozeni.Name = "dateTimePickerDatumNarozeni";
            dateTimePickerDatumNarozeni.Size = new Size(200, 23);
            dateTimePickerDatumNarozeni.TabIndex = 1;
            // 
            // textBoxRodneCislo
            // 
            textBoxRodneCislo.Location = new Point(130, 93);
            textBoxRodneCislo.Name = "textBoxRodneCislo";
            textBoxRodneCislo.Size = new Size(100, 23);
            textBoxRodneCislo.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 38);
            label1.Name = "label1";
            label1.Size = new Size(100, 15);
            label1.TabIndex = 3;
            label1.Text = "Vlastník Pivovaru:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 70);
            label2.Name = "label2";
            label2.Size = new Size(94, 15);
            label2.TabIndex = 4;
            label2.Text = "Datum narození:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 96);
            label3.Name = "label3";
            label3.Size = new Size(71, 15);
            label3.TabIndex = 5;
            label3.Text = "Rodné číslo:";
            // 
            // dgvFyzickeOsoby
            // 
            dgvFyzickeOsoby.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvFyzickeOsoby.Location = new Point(11, 158);
            dgvFyzickeOsoby.Name = "dgvFyzickeOsoby";
            dgvFyzickeOsoby.Size = new Size(318, 237);
            dgvFyzickeOsoby.TabIndex = 6;
            dgvFyzickeOsoby.SelectionChanged += dgvFyzickeOsoby_SelectionChanged;
            // 
            // btnAdd
            // 
            btnAdd.Location = new Point(11, 401);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(75, 28);
            btnAdd.TabIndex = 7;
            btnAdd.Text = "Přidat";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += btnAdd_Click;
            // 
            // btnUpdate
            // 
            btnUpdate.Location = new Point(92, 401);
            btnUpdate.Name = "btnUpdate";
            btnUpdate.Size = new Size(75, 28);
            btnUpdate.TabIndex = 8;
            btnUpdate.Text = "Atualizovat";
            btnUpdate.UseVisualStyleBackColor = true;
            btnUpdate.Click += btnUpdate_Click;
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(173, 401);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(75, 28);
            btnDelete.TabIndex = 9;
            btnDelete.Text = "Odstranit";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;
            // 
            // btnBack
            // 
            btnBack.Location = new Point(257, 401);
            btnBack.Name = "btnBack";
            btnBack.Size = new Size(75, 28);
            btnBack.TabIndex = 10;
            btnBack.Text = "Zpět";
            btnBack.UseVisualStyleBackColor = true;
            btnBack.Click += btnBack_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.Location = new Point(12, 4);
            label4.Name = "label4";
            label4.Size = new Size(191, 21);
            label4.TabIndex = 11;
            label4.Text = "Správce Fyzických Osob";
            // 
            // txtSearch
            // 
            txtSearch.Location = new Point(12, 129);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(317, 23);
            txtSearch.TabIndex = 12;
            txtSearch.TextChanged += txtSearch_TextChanged;
            // 
            // SpravaFyzickzchOsob
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(345, 441);
            Controls.Add(txtSearch);
            Controls.Add(label4);
            Controls.Add(btnBack);
            Controls.Add(btnDelete);
            Controls.Add(btnUpdate);
            Controls.Add(btnAdd);
            Controls.Add(dgvFyzickeOsoby);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(textBoxRodneCislo);
            Controls.Add(dateTimePickerDatumNarozeni);
            Controls.Add(comboBoxVlastnikPivovaru);
            Name = "SpravaFyzickzchOsob";
            Text = "Správce Fyzických Osob";
            ((System.ComponentModel.ISupportInitialize)dgvFyzickeOsoby).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox comboBoxVlastnikPivovaru;
        private DateTimePicker dateTimePickerDatumNarozeni;
        private TextBox textBoxRodneCislo;
        private Label label1;
        private Label label2;
        private Label label3;
        private DataGridView dgvFyzickeOsoby;
        private Button btnAdd;
        private Button btnUpdate;
        private Button btnDelete;
        private Button btnBack;
        private Label label4;
        private TextBox txtSearch;
    }
}