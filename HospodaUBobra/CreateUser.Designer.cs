namespace HospodaUBobra
{
    partial class CreateUser
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
            txtUsername = new TextBox();
            label2 = new Label();
            txtPassword = new TextBox();
            label3 = new Label();
            comboBoxRole = new ComboBox();
            btnCreate = new Button();
            btnBack = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 52);
            label1.Name = "label1";
            label1.Size = new Size(105, 15);
            label1.TabIndex = 0;
            label1.Text = "Uživatelské jméno:";
            // 
            // txtUsername
            // 
            txtUsername.Location = new Point(123, 49);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(100, 23);
            txtUsername.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 81);
            label2.Name = "label2";
            label2.Size = new Size(40, 15);
            label2.TabIndex = 2;
            label2.Text = "Heslo:";
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(123, 78);
            txtPassword.Name = "txtPassword";
            txtPassword.PasswordChar = '*';
            txtPassword.Size = new Size(100, 23);
            txtPassword.TabIndex = 3;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 109);
            label3.Name = "label3";
            label3.Size = new Size(33, 15);
            label3.TabIndex = 4;
            label3.Text = "Role:";
            // 
            // comboBoxRole
            // 
            comboBoxRole.FormattingEnabled = true;
            comboBoxRole.Location = new Point(123, 109);
            comboBoxRole.Name = "comboBoxRole";
            comboBoxRole.Size = new Size(100, 23);
            comboBoxRole.TabIndex = 5;
            // 
            // btnCreate
            // 
            btnCreate.Location = new Point(12, 144);
            btnCreate.Name = "btnCreate";
            btnCreate.Size = new Size(88, 43);
            btnCreate.TabIndex = 6;
            btnCreate.Text = "Vytvořit";
            btnCreate.UseVisualStyleBackColor = true;
            btnCreate.Click += btnCreate_Click;
            // 
            // btnBack
            // 
            btnBack.Location = new Point(136, 144);
            btnBack.Name = "btnBack";
            btnBack.Size = new Size(90, 43);
            btnBack.TabIndex = 7;
            btnBack.Text = "Zpět";
            btnBack.UseVisualStyleBackColor = true;
            btnBack.Click += btnBack_Click;
            // 
            // CreateUser
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(238, 199);
            Controls.Add(btnBack);
            Controls.Add(btnCreate);
            Controls.Add(comboBoxRole);
            Controls.Add(label3);
            Controls.Add(txtPassword);
            Controls.Add(label2);
            Controls.Add(txtUsername);
            Controls.Add(label1);
            Name = "CreateUser";
            Text = "CreateUser";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox txtUsername;
        private Label label2;
        private TextBox txtPassword;
        private Label label3;
        private ComboBox comboBoxRole;
        private Button btnCreate;
        private Button btnBack;
    }
}