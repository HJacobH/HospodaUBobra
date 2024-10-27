namespace HospodaUBobra
{
    partial class PiwoMangement
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
            txtBeerName = new TextBox();
            txtAlcoholContent = new TextBox();
            txtVolume = new TextBox();
            txtPrice = new TextBox();
            txtStockQuantity = new TextBox();
            comboBoxPackaging = new ComboBox();
            comboBoxUnit = new ComboBox();
            btnAddPiwo = new Button();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            label7 = new Label();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 39);
            label1.Name = "label1";
            label1.Size = new Size(42, 15);
            label1.TabIndex = 0;
            label1.Text = "Nazev:";
            // 
            // txtBeerName
            // 
            txtBeerName.Location = new Point(105, 36);
            txtBeerName.Name = "txtBeerName";
            txtBeerName.Size = new Size(100, 23);
            txtBeerName.TabIndex = 1;
            // 
            // txtAlcoholContent
            // 
            txtAlcoholContent.Location = new Point(105, 68);
            txtAlcoholContent.Name = "txtAlcoholContent";
            txtAlcoholContent.Size = new Size(100, 23);
            txtAlcoholContent.TabIndex = 2;
            // 
            // txtVolume
            // 
            txtVolume.Location = new Point(105, 94);
            txtVolume.Name = "txtVolume";
            txtVolume.Size = new Size(100, 23);
            txtVolume.TabIndex = 3;
            // 
            // txtPrice
            // 
            txtPrice.Location = new Point(105, 123);
            txtPrice.Name = "txtPrice";
            txtPrice.Size = new Size(100, 23);
            txtPrice.TabIndex = 4;
            // 
            // txtStockQuantity
            // 
            txtStockQuantity.Location = new Point(105, 152);
            txtStockQuantity.Name = "txtStockQuantity";
            txtStockQuantity.Size = new Size(100, 23);
            txtStockQuantity.TabIndex = 5;
            // 
            // comboBoxPackaging
            // 
            comboBoxPackaging.FormattingEnabled = true;
            comboBoxPackaging.Location = new Point(105, 181);
            comboBoxPackaging.Name = "comboBoxPackaging";
            comboBoxPackaging.Size = new Size(100, 23);
            comboBoxPackaging.TabIndex = 6;
            // 
            // comboBoxUnit
            // 
            comboBoxUnit.FormattingEnabled = true;
            comboBoxUnit.Location = new Point(105, 210);
            comboBoxUnit.Name = "comboBoxUnit";
            comboBoxUnit.Size = new Size(100, 23);
            comboBoxUnit.TabIndex = 7;
            // 
            // btnAddPiwo
            // 
            btnAddPiwo.Location = new Point(12, 256);
            btnAddPiwo.Name = "btnAddPiwo";
            btnAddPiwo.Size = new Size(179, 23);
            btnAddPiwo.TabIndex = 8;
            btnAddPiwo.Text = "Add Piwo";
            btnAddPiwo.UseVisualStyleBackColor = true;
            btnAddPiwo.Click += btnAddPiwo_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 71);
            label2.Name = "label2";
            label2.Size = new Size(93, 15);
            label2.TabIndex = 9;
            label2.Text = "Obsah alkoholu:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 97);
            label3.Name = "label3";
            label3.Size = new Size(46, 15);
            label3.TabIndex = 10;
            label3.Text = "Objem:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(12, 126);
            label4.Name = "label4";
            label4.Size = new Size(37, 15);
            label4.TabIndex = 11;
            label4.Text = "Cena:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(12, 155);
            label5.Name = "label5";
            label5.Size = new Size(87, 15);
            label5.TabIndex = 12;
            label5.Text = "Pocet skladem:";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(12, 184);
            label6.Name = "label6";
            label6.Size = new Size(42, 15);
            label6.TabIndex = 13;
            label6.Text = "Baleni:";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(12, 213);
            label7.Name = "label7";
            label7.Size = new Size(57, 15);
            label7.TabIndex = 14;
            label7.Text = "Jednotka:";
            // 
            // PiwoMangement
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(217, 334);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(btnAddPiwo);
            Controls.Add(comboBoxUnit);
            Controls.Add(comboBoxPackaging);
            Controls.Add(txtStockQuantity);
            Controls.Add(txtPrice);
            Controls.Add(txtVolume);
            Controls.Add(txtAlcoholContent);
            Controls.Add(txtBeerName);
            Controls.Add(label1);
            Name = "PiwoMangement";
            Text = "PiwoMangement";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox txtBeerName;
        private TextBox txtAlcoholContent;
        private TextBox txtVolume;
        private TextBox txtPrice;
        private TextBox txtStockQuantity;
        private ComboBox comboBoxPackaging;
        private ComboBox comboBoxUnit;
        private Button btnAddPiwo;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
    }
}