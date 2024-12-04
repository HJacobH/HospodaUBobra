namespace HospodaUBobra
{
    partial class SpravceVyroby
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
            cbPivovary = new ComboBox();
            label2 = new Label();
            label3 = new Label();
            cbPiva = new ComboBox();
            dataGridView1 = new DataGridView();
            btnBack = new Button();
            btnDelete = new Button();
            btnUpdate = new Button();
            btnAdd = new Button();
            txtLitryZaDen = new TextBox();
            label4 = new Label();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(12, 4);
            label1.Name = "label1";
            label1.Size = new Size(127, 21);
            label1.TabIndex = 0;
            label1.Text = "Správce výroby";
            // 
            // cbPivovary
            // 
            cbPivovary.FormattingEnabled = true;
            cbPivovary.Location = new Point(93, 45);
            cbPivovary.Name = "cbPivovary";
            cbPivovary.Size = new Size(121, 23);
            cbPivovary.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(16, 48);
            label2.Name = "label2";
            label2.Size = new Size(49, 15);
            label2.TabIndex = 2;
            label2.Text = "Pivovar:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(16, 79);
            label3.Name = "label3";
            label3.Size = new Size(33, 15);
            label3.TabIndex = 3;
            label3.Text = "Pivo:";
            // 
            // cbPiva
            // 
            cbPiva.FormattingEnabled = true;
            cbPiva.Location = new Point(93, 76);
            cbPiva.Name = "cbPiva";
            cbPiva.Size = new Size(121, 23);
            cbPiva.TabIndex = 4;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(12, 135);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(346, 278);
            dataGridView1.TabIndex = 5;
            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
            // 
            // btnBack
            // 
            btnBack.Location = new Point(283, 419);
            btnBack.Name = "btnBack";
            btnBack.Size = new Size(75, 23);
            btnBack.TabIndex = 17;
            btnBack.Text = "Zpět";
            btnBack.UseVisualStyleBackColor = true;
            btnBack.Click += btnBack_Click;
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(189, 419);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(75, 23);
            btnDelete.TabIndex = 16;
            btnDelete.Text = "Odstranit";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;
            // 
            // btnUpdate
            // 
            btnUpdate.Location = new Point(93, 419);
            btnUpdate.Name = "btnUpdate";
            btnUpdate.Size = new Size(90, 23);
            btnUpdate.TabIndex = 15;
            btnUpdate.Text = "Aktualizovat";
            btnUpdate.UseVisualStyleBackColor = true;
            btnUpdate.Click += btnUpdate_Click;
            // 
            // btnAdd
            // 
            btnAdd.Location = new Point(12, 419);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(75, 23);
            btnAdd.TabIndex = 14;
            btnAdd.Text = "Přidat";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += btnAdd_Click;
            // 
            // txtLitryZaDen
            // 
            txtLitryZaDen.Location = new Point(93, 105);
            txtLitryZaDen.Name = "txtLitryZaDen";
            txtLitryZaDen.Size = new Size(121, 23);
            txtLitryZaDen.TabIndex = 18;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(16, 108);
            label4.Name = "label4";
            label4.Size = new Size(70, 15);
            label4.TabIndex = 19;
            label4.Text = "Litry za den:";
            // 
            // SpravceVyroby
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(372, 450);
            Controls.Add(label4);
            Controls.Add(txtLitryZaDen);
            Controls.Add(btnBack);
            Controls.Add(btnDelete);
            Controls.Add(btnUpdate);
            Controls.Add(btnAdd);
            Controls.Add(dataGridView1);
            Controls.Add(cbPiva);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(cbPivovary);
            Controls.Add(label1);
            Name = "SpravceVyroby";
            Text = "Správce Výroby";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private ComboBox cbPivovary;
        private Label label2;
        private Label label3;
        private ComboBox cbPiva;
        private DataGridView dataGridView1;
        private Button btnBack;
        private Button btnDelete;
        private Button btnUpdate;
        private Button btnAdd;
        private TextBox txtLitryZaDen;
        private Label label4;
    }
}