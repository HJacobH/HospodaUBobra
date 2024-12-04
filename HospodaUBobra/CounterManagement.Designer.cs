namespace HospodaUBobra
{
    partial class CounterManagement
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
            cbTables = new ComboBox();
            label2 = new Label();
            dgvTableData = new DataGridView();
            buttonBack = new Button();
            lblStatus = new Label();
            txtAktualni = new TextBox();
            label3 = new Label();
            btnAdd = new Button();
            btnUpdate = new Button();
            btnDelete = new Button();
            txtSearch = new TextBox();
            ((System.ComponentModel.ISupportInitialize)dgvTableData).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            label1.Location = new Point(205, 5);
            label1.Margin = new Padding(2, 0, 2, 0);
            label1.Name = "label1";
            label1.Size = new Size(142, 21);
            label1.TabIndex = 0;
            label1.Text = "Správce číselníků";
            // 
            // cbTables
            // 
            cbTables.FormattingEnabled = true;
            cbTables.Location = new Point(98, 32);
            cbTables.Margin = new Padding(2);
            cbTables.Name = "cbTables";
            cbTables.Size = new Size(129, 23);
            cbTables.TabIndex = 1;
            cbTables.SelectedIndexChanged += cbTables_SelectedIndexChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(11, 35);
            label2.Margin = new Padding(2, 0, 2, 0);
            label2.Name = "label2";
            label2.Size = new Size(83, 15);
            label2.TabIndex = 2;
            label2.Text = "Vybrat číselník";
            // 
            // dgvTableData
            // 
            dgvTableData.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvTableData.Location = new Point(8, 86);
            dgvTableData.Margin = new Padding(2);
            dgvTableData.Name = "dgvTableData";
            dgvTableData.RowHeadersWidth = 62;
            dgvTableData.Size = new Size(436, 249);
            dgvTableData.TabIndex = 3;
            dgvTableData.SelectionChanged += dgvTableData_SelectionChanged;
            // 
            // buttonBack
            // 
            buttonBack.Location = new Point(448, 305);
            buttonBack.Margin = new Padding(2);
            buttonBack.Name = "buttonBack";
            buttonBack.Size = new Size(101, 30);
            buttonBack.TabIndex = 4;
            buttonBack.Text = "Zpět";
            buttonBack.UseVisualStyleBackColor = true;
            buttonBack.Click += buttonBack_Click;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(260, 35);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(96, 15);
            lblStatus.TabIndex = 5;
            lblStatus.Text = "Aktuální čísleník:";
            // 
            // txtAktualni
            // 
            txtAktualni.Location = new Point(449, 86);
            txtAktualni.Name = "txtAktualni";
            txtAktualni.Size = new Size(100, 23);
            txtAktualni.TabIndex = 6;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(449, 68);
            label3.Name = "label3";
            label3.Size = new Size(97, 15);
            label3.TabIndex = 7;
            label3.Text = "Vybraná položka:";
            // 
            // btnAdd
            // 
            btnAdd.Location = new Point(449, 124);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(100, 32);
            btnAdd.TabIndex = 8;
            btnAdd.Text = "Přidat";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += btnAdd_Click;
            // 
            // btnUpdate
            // 
            btnUpdate.Location = new Point(449, 162);
            btnUpdate.Name = "btnUpdate";
            btnUpdate.Size = new Size(100, 32);
            btnUpdate.TabIndex = 9;
            btnUpdate.Text = "Aktualizovat";
            btnUpdate.UseVisualStyleBackColor = true;
            btnUpdate.Click += btnUpdate_Click;
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(449, 200);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(100, 32);
            btnDelete.TabIndex = 10;
            btnDelete.Text = "Odstranit";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;
            // 
            // txtSearch
            // 
            txtSearch.Location = new Point(12, 58);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(432, 23);
            txtSearch.TabIndex = 11;
            txtSearch.TextChanged += txtSearch_TextChanged;
            // 
            // CounterManagement
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(560, 346);
            Controls.Add(txtSearch);
            Controls.Add(btnDelete);
            Controls.Add(btnUpdate);
            Controls.Add(btnAdd);
            Controls.Add(label3);
            Controls.Add(txtAktualni);
            Controls.Add(lblStatus);
            Controls.Add(buttonBack);
            Controls.Add(dgvTableData);
            Controls.Add(label2);
            Controls.Add(cbTables);
            Controls.Add(label1);
            Margin = new Padding(2);
            Name = "CounterManagement";
            Text = "Správce Číselníků";
            ((System.ComponentModel.ISupportInitialize)dgvTableData).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private ComboBox cbTables;
        private Label label2;
        private DataGridView dgvTableData;
        private Button buttonBack;
        private Label lblStatus;
        private TextBox txtAktualni;
        private Label label3;
        private Button btnAdd;
        private Button btnUpdate;
        private Button btnDelete;
        private TextBox txtSearch;
    }
}