namespace HospodaUBobra
{
    partial class SpravaSpojeni
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
            btnAdd = new Button();
            txtAktualni = new Label();
            buttonBack = new Button();
            dgvTableData = new DataGridView();
            label2 = new Label();
            cbTables = new ComboBox();
            textBox1 = new TextBox();
            dateTimePicker1 = new DateTimePicker();
            comboBox1 = new ComboBox();
            comboBox2 = new ComboBox();
            ((System.ComponentModel.ISupportInitialize)dgvTableData).BeginInit();
            SuspendLayout();
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(452, 278);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(100, 32);
            btnDelete.TabIndex = 18;
            btnDelete.Text = "Odstranit";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;
            // 
            // btnUpdate
            // 
            btnUpdate.Location = new Point(452, 240);
            btnUpdate.Name = "btnUpdate";
            btnUpdate.Size = new Size(100, 32);
            btnUpdate.TabIndex = 17;
            btnUpdate.Text = "Aktualizovat";
            btnUpdate.UseVisualStyleBackColor = true;
            btnUpdate.Click += btnUpdate_Click;
            // 
            // btnAdd
            // 
            btnAdd.Location = new Point(452, 202);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(100, 32);
            btnAdd.TabIndex = 16;
            btnAdd.Text = "Přidat";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += btnAdd_Click;
            // 
            // txtAktualni
            // 
            txtAktualni.AutoSize = true;
            txtAktualni.Location = new Point(263, 62);
            txtAktualni.Name = "txtAktualni";
            txtAktualni.Size = new Size(95, 15);
            txtAktualni.TabIndex = 15;
            txtAktualni.Text = "Aktuální spojení:";
            // 
            // buttonBack
            // 
            buttonBack.Location = new Point(451, 332);
            buttonBack.Margin = new Padding(2);
            buttonBack.Name = "buttonBack";
            buttonBack.Size = new Size(101, 30);
            buttonBack.TabIndex = 14;
            buttonBack.Text = "Zpět";
            buttonBack.UseVisualStyleBackColor = true;
            buttonBack.Click += buttonBack_Click;
            // 
            // dgvTableData
            // 
            dgvTableData.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvTableData.Location = new Point(11, 92);
            dgvTableData.Margin = new Padding(2);
            dgvTableData.Name = "dgvTableData";
            dgvTableData.RowHeadersWidth = 62;
            dgvTableData.Size = new Size(436, 270);
            dgvTableData.TabIndex = 13;
            dgvTableData.SelectionChanged += dgvTableData_SelectionChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(14, 62);
            label2.Margin = new Padding(2, 0, 2, 0);
            label2.Name = "label2";
            label2.Size = new Size(85, 15);
            label2.TabIndex = 12;
            label2.Text = "Vybrat spojení:";
            // 
            // cbTables
            // 
            cbTables.FormattingEnabled = true;
            cbTables.Location = new Point(101, 59);
            cbTables.Margin = new Padding(2);
            cbTables.Name = "cbTables";
            cbTables.Size = new Size(129, 23);
            cbTables.TabIndex = 11;
            cbTables.SelectedIndexChanged += cbTables_SelectedIndexChanged;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(452, 173);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(100, 23);
            textBox1.TabIndex = 21;
            // 
            // dateTimePicker1
            // 
            dateTimePicker1.Location = new Point(451, 87);
            dateTimePicker1.Name = "dateTimePicker1";
            dateTimePicker1.Size = new Size(101, 23);
            dateTimePicker1.TabIndex = 22;
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(451, 116);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(100, 23);
            comboBox1.TabIndex = 23;
            // 
            // comboBox2
            // 
            comboBox2.FormattingEnabled = true;
            comboBox2.Location = new Point(451, 145);
            comboBox2.Name = "comboBox2";
            comboBox2.Size = new Size(100, 23);
            comboBox2.TabIndex = 24;
            // 
            // SpravaSpojeni
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(563, 379);
            Controls.Add(comboBox2);
            Controls.Add(comboBox1);
            Controls.Add(dateTimePicker1);
            Controls.Add(textBox1);
            Controls.Add(btnDelete);
            Controls.Add(btnUpdate);
            Controls.Add(btnAdd);
            Controls.Add(txtAktualni);
            Controls.Add(buttonBack);
            Controls.Add(dgvTableData);
            Controls.Add(label2);
            Controls.Add(cbTables);
            Name = "SpravaSpojeni";
            Text = "SpravaSpojeni";
            ((System.ComponentModel.ISupportInitialize)dgvTableData).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnDelete;
        private Button btnUpdate;
        private Button btnAdd;
        private Label txtAktualni;
        private Button buttonBack;
        private DataGridView dgvTableData;
        private Label label2;
        private ComboBox cbTables;
        private TextBox textBox1;
        private DateTimePicker dateTimePicker1;
        private ComboBox comboBox1;
        private ComboBox comboBox2;
    }
}