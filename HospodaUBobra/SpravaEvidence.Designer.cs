namespace HospodaUBobra
{
    partial class SpravaEvidence
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
            txtQuantity = new TextBox();
            txtOrderPrice = new TextBox();
            txtOrderId = new TextBox();
            cbUnit = new ComboBox();
            cbBeer = new ComboBox();
            dtpOrderDate = new DateTimePicker();
            dgvEvidence = new DataGridView();
            btnAdd = new Button();
            btnUpdate = new Button();
            btnDelete = new Button();
            btnBack = new Button();
            btnClear = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvEvidence).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 38);
            label1.Name = "label1";
            label1.Size = new Size(58, 15);
            label1.TabIndex = 0;
            label1.Text = "Množství:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 68);
            label2.Name = "label2";
            label2.Size = new Size(110, 15);
            label2.TabIndex = 1;
            label2.Text = "Datum objednávky:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(16, 96);
            label3.Name = "label3";
            label3.Size = new Size(33, 15);
            label3.TabIndex = 2;
            label3.Text = "Pivo:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(16, 129);
            label4.Name = "label4";
            label4.Size = new Size(57, 15);
            label4.TabIndex = 3;
            label4.Text = "Jednotka:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(16, 159);
            label5.Name = "label5";
            label5.Size = new Size(37, 15);
            label5.TabIndex = 4;
            label5.Text = "Cena:";
            // 
            // txtQuantity
            // 
            txtQuantity.Location = new Point(131, 35);
            txtQuantity.Name = "txtQuantity";
            txtQuantity.Size = new Size(119, 23);
            txtQuantity.TabIndex = 6;
            txtQuantity.TextChanged += txtQuantity_TextChanged_1;
            // 
            // txtOrderPrice
            // 
            txtOrderPrice.Location = new Point(131, 156);
            txtOrderPrice.Name = "txtOrderPrice";
            txtOrderPrice.ReadOnly = true;
            txtOrderPrice.Size = new Size(119, 23);
            txtOrderPrice.TabIndex = 7;
            // 
            // txtOrderId
            // 
            txtOrderId.Location = new Point(131, 191);
            txtOrderId.Name = "txtOrderId";
            txtOrderId.Size = new Size(119, 23);
            txtOrderId.TabIndex = 8;
            txtOrderId.Visible = false;
            // 
            // cbUnit
            // 
            cbUnit.FormattingEnabled = true;
            cbUnit.Location = new Point(131, 126);
            cbUnit.Name = "cbUnit";
            cbUnit.Size = new Size(119, 23);
            cbUnit.TabIndex = 9;
            // 
            // cbBeer
            // 
            cbBeer.FormattingEnabled = true;
            cbBeer.Location = new Point(131, 93);
            cbBeer.Name = "cbBeer";
            cbBeer.Size = new Size(119, 23);
            cbBeer.TabIndex = 10;
            cbBeer.SelectedIndexChanged += cbBeer_SelectedIndexChanged;
            // 
            // dtpOrderDate
            // 
            dtpOrderDate.Location = new Point(131, 64);
            dtpOrderDate.Name = "dtpOrderDate";
            dtpOrderDate.Size = new Size(119, 23);
            dtpOrderDate.TabIndex = 11;
            // 
            // dgvEvidence
            // 
            dgvEvidence.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvEvidence.Location = new Point(16, 185);
            dgvEvidence.Name = "dgvEvidence";
            dgvEvidence.Size = new Size(458, 310);
            dgvEvidence.TabIndex = 12;
            dgvEvidence.SelectionChanged += dgvEvidence_SelectionChanged;
            // 
            // btnAdd
            // 
            btnAdd.Location = new Point(14, 501);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(94, 23);
            btnAdd.TabIndex = 13;
            btnAdd.Text = "Přidat Evidenci";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += btnAdd_Click;
            // 
            // btnUpdate
            // 
            btnUpdate.Location = new Point(114, 501);
            btnUpdate.Name = "btnUpdate";
            btnUpdate.Size = new Size(128, 23);
            btnUpdate.TabIndex = 14;
            btnUpdate.Text = "Aktualizovat Evidenci";
            btnUpdate.UseVisualStyleBackColor = true;
            btnUpdate.Click += btnUpdate_Click;
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(248, 501);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(114, 23);
            btnDelete.TabIndex = 15;
            btnDelete.Text = "Odstranit Evidenci";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;
            // 
            // btnBack
            // 
            btnBack.Location = new Point(399, 501);
            btnBack.Name = "btnBack";
            btnBack.Size = new Size(75, 23);
            btnBack.TabIndex = 16;
            btnBack.Text = "Zpět";
            btnBack.UseVisualStyleBackColor = true;
            btnBack.Click += btnBack_Click;
            // 
            // btnClear
            // 
            btnClear.Location = new Point(399, 12);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(75, 41);
            btnClear.TabIndex = 17;
            btnClear.Text = "Vyprázdnit pole";
            btnClear.UseVisualStyleBackColor = true;
            btnClear.Click += btnClear_Click;
            // 
            // SpravaEvidence
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(486, 531);
            Controls.Add(btnClear);
            Controls.Add(btnBack);
            Controls.Add(btnDelete);
            Controls.Add(btnUpdate);
            Controls.Add(btnAdd);
            Controls.Add(dgvEvidence);
            Controls.Add(dtpOrderDate);
            Controls.Add(cbBeer);
            Controls.Add(cbUnit);
            Controls.Add(txtOrderId);
            Controls.Add(txtOrderPrice);
            Controls.Add(txtQuantity);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "SpravaEvidence";
            Text = "SpravaEvidence";
            ((System.ComponentModel.ISupportInitialize)dgvEvidence).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private TextBox txtQuantity;
        private TextBox txtOrderPrice;
        private TextBox txtOrderId;
        private ComboBox cbUnit;
        private ComboBox cbBeer;
        private DateTimePicker dtpOrderDate;
        private DataGridView dgvEvidence;
        private Button btnAdd;
        private Button btnUpdate;
        private Button btnDelete;
        private Button btnBack;
        private Button btnClear;
    }
}