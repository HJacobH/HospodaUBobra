namespace HospodaUBobra
{
    partial class SpravaObjednavek
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
            cbClients = new ComboBox();
            cbOrderStatuses = new ComboBox();
            dtpOrderDate = new DateTimePicker();
            dtpDeliveryDate = new DateTimePicker();
            dgvOrders = new DataGridView();
            btnBack = new Button();
            btnDelete = new Button();
            btnUpdate = new Button();
            btnAdd = new Button();
            label5 = new Label();
            btnDeleteCancelledOrders = new Button();
            txtSearch = new TextBox();
            ((System.ComponentModel.ISupportInitialize)dgvOrders).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(14, 49);
            label1.Name = "label1";
            label1.Size = new Size(40, 15);
            label1.TabIndex = 0;
            label1.Text = "Klient:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(14, 79);
            label2.Name = "label2";
            label2.Size = new Size(42, 15);
            label2.TabIndex = 1;
            label2.Text = "Status:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(14, 111);
            label3.Name = "label3";
            label3.Size = new Size(109, 15);
            label3.TabIndex = 2;
            label3.Text = "Datum objedvávky:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(14, 140);
            label4.Name = "label4";
            label4.Size = new Size(96, 15);
            label4.TabIndex = 3;
            label4.Text = "Datum doručení:";
            // 
            // cbClients
            // 
            cbClients.FormattingEnabled = true;
            cbClients.Location = new Point(135, 46);
            cbClients.Name = "cbClients";
            cbClients.Size = new Size(121, 23);
            cbClients.TabIndex = 4;
            // 
            // cbOrderStatuses
            // 
            cbOrderStatuses.FormattingEnabled = true;
            cbOrderStatuses.Location = new Point(135, 76);
            cbOrderStatuses.Name = "cbOrderStatuses";
            cbOrderStatuses.Size = new Size(121, 23);
            cbOrderStatuses.TabIndex = 5;
            // 
            // dtpOrderDate
            // 
            dtpOrderDate.Location = new Point(135, 105);
            dtpOrderDate.Name = "dtpOrderDate";
            dtpOrderDate.Size = new Size(200, 23);
            dtpOrderDate.TabIndex = 6;
            // 
            // dtpDeliveryDate
            // 
            dtpDeliveryDate.Location = new Point(135, 134);
            dtpDeliveryDate.Name = "dtpDeliveryDate";
            dtpDeliveryDate.Size = new Size(200, 23);
            dtpDeliveryDate.TabIndex = 7;
            // 
            // dgvOrders
            // 
            dgvOrders.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvOrders.Location = new Point(14, 192);
            dgvOrders.Name = "dgvOrders";
            dgvOrders.Size = new Size(444, 228);
            dgvOrders.TabIndex = 8;
            dgvOrders.SelectionChanged += dgvOrders_SelectionChanged;
            // 
            // btnBack
            // 
            btnBack.Location = new Point(386, 429);
            btnBack.Name = "btnBack";
            btnBack.Size = new Size(72, 23);
            btnBack.TabIndex = 24;
            btnBack.Text = "Zpět";
            btnBack.UseVisualStyleBackColor = true;
            btnBack.Click += btnBack_Click;
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(186, 429);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(79, 23);
            btnDelete.TabIndex = 23;
            btnDelete.Text = "Odstranit";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;
            // 
            // btnUpdate
            // 
            btnUpdate.Location = new Point(86, 429);
            btnUpdate.Name = "btnUpdate";
            btnUpdate.Size = new Size(94, 23);
            btnUpdate.TabIndex = 22;
            btnUpdate.Text = "Aktualizovat";
            btnUpdate.UseVisualStyleBackColor = true;
            btnUpdate.Click += btnUpdate_Click;
            // 
            // btnAdd
            // 
            btnAdd.Location = new Point(12, 429);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(68, 23);
            btnAdd.TabIndex = 21;
            btnAdd.Text = "Přidat";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += btnAdd_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.Location = new Point(12, 5);
            label5.Name = "label5";
            label5.Size = new Size(166, 21);
            label5.TabIndex = 25;
            label5.Text = "Správce Objednávek";
            // 
            // btnDeleteCancelledOrders
            // 
            btnDeleteCancelledOrders.Location = new Point(365, 12);
            btnDeleteCancelledOrders.Name = "btnDeleteCancelledOrders";
            btnDeleteCancelledOrders.Size = new Size(100, 38);
            btnDeleteCancelledOrders.TabIndex = 26;
            btnDeleteCancelledOrders.Text = "Smazat zrušené objednávky";
            btnDeleteCancelledOrders.UseVisualStyleBackColor = true;
            btnDeleteCancelledOrders.Click += btnDeleteCancelledOrders_Click;
            // 
            // txtSearch
            // 
            txtSearch.Location = new Point(14, 163);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(444, 23);
            txtSearch.TabIndex = 27;
            txtSearch.TextChanged += txtSearch_TextChanged;
            // 
            // SpravaObjednavek
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(477, 464);
            Controls.Add(txtSearch);
            Controls.Add(btnDeleteCancelledOrders);
            Controls.Add(label5);
            Controls.Add(btnBack);
            Controls.Add(btnDelete);
            Controls.Add(btnUpdate);
            Controls.Add(btnAdd);
            Controls.Add(dgvOrders);
            Controls.Add(dtpDeliveryDate);
            Controls.Add(dtpOrderDate);
            Controls.Add(cbOrderStatuses);
            Controls.Add(cbClients);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "SpravaObjednavek";
            Text = "Správce Objednávek";
            ((System.ComponentModel.ISupportInitialize)dgvOrders).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private ComboBox cbClients;
        private ComboBox cbOrderStatuses;
        private DateTimePicker dtpOrderDate;
        private DateTimePicker dtpDeliveryDate;
        private DataGridView dgvOrders;
        private Button btnBack;
        private Button btnDelete;
        private Button btnUpdate;
        private Button btnAdd;
        private Label label5;
        private Button btnDeleteCancelledOrders;
        private TextBox txtSearch;
    }
}