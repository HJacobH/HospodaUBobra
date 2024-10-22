namespace HospodaUBobra
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            dataGridView1 = new DataGridView();
            comboBoxTables = new ComboBox();
            btnPrihlasit = new Button();
            roleLabel = new Label();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(14, 16);
            dataGridView1.Margin = new Padding(3, 4, 3, 4);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new Size(934, 568);
            dataGridView1.TabIndex = 0;
            // 
            // comboBoxTables
            // 
            comboBoxTables.FormattingEnabled = true;
            comboBoxTables.Location = new Point(954, 16);
            comboBoxTables.Margin = new Padding(3, 4, 3, 4);
            comboBoxTables.Name = "comboBoxTables";
            comboBoxTables.Size = new Size(138, 28);
            comboBoxTables.TabIndex = 1;
            // 
            // btnPrihlasit
            // 
            btnPrihlasit.Location = new Point(965, 85);
            btnPrihlasit.Name = "btnPrihlasit";
            btnPrihlasit.Size = new Size(94, 29);
            btnPrihlasit.TabIndex = 2;
            btnPrihlasit.Text = "Login";
            btnPrihlasit.UseVisualStyleBackColor = true;
            btnPrihlasit.Click += btnPrihlasit_Click;
            // 
            // roleLabel
            // 
            roleLabel.AutoSize = true;
            roleLabel.Location = new Point(988, 57);
            roleLabel.Name = "roleLabel";
            roleLabel.Size = new Size(0, 20);
            roleLabel.TabIndex = 3;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1106, 600);
            Controls.Add(roleLabel);
            Controls.Add(btnPrihlasit);
            Controls.Add(comboBoxTables);
            Controls.Add(dataGridView1);
            Margin = new Padding(3, 4, 3, 4);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dataGridView1;
        private ComboBox comboBoxTables;
        private Button btnPrihlasit;
        private Label roleLabel;
    }
}
