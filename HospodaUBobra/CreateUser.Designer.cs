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
            txtEmail = new TextBox();
            txtTelefon = new TextBox();
            label4 = new Label();
            label5 = new Label();
            dataGridViewUsers = new DataGridView();
            btnUpdate = new Button();
            btnDelete = new Button();
            label6 = new Label();
            ((System.ComponentModel.ISupportInitialize)dataGridViewUsers).BeginInit();
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
            label2.Location = new Point(12, 139);
            label2.Name = "label2";
            label2.Size = new Size(40, 15);
            label2.TabIndex = 2;
            label2.Text = "Heslo:";
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(123, 136);
            txtPassword.Name = "txtPassword";
            txtPassword.PasswordChar = '*';
            txtPassword.Size = new Size(100, 23);
            txtPassword.TabIndex = 3;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 168);
            label3.Name = "label3";
            label3.Size = new Size(33, 15);
            label3.TabIndex = 4;
            label3.Text = "Role:";
            // 
            // comboBoxRole
            // 
            comboBoxRole.FormattingEnabled = true;
            comboBoxRole.Location = new Point(123, 165);
            comboBoxRole.Name = "comboBoxRole";
            comboBoxRole.Size = new Size(100, 23);
            comboBoxRole.TabIndex = 5;
            // 
            // btnCreate
            // 
            btnCreate.Location = new Point(12, 491);
            btnCreate.Name = "btnCreate";
            btnCreate.Size = new Size(114, 28);
            btnCreate.TabIndex = 6;
            btnCreate.Text = "Vytvořit uživatele";
            btnCreate.UseVisualStyleBackColor = true;
            btnCreate.Click += btnCreate_Click;
            // 
            // btnBack
            // 
            btnBack.Location = new Point(471, 491);
            btnBack.Name = "btnBack";
            btnBack.Size = new Size(90, 28);
            btnBack.TabIndex = 7;
            btnBack.Text = "Zpět";
            btnBack.UseVisualStyleBackColor = true;
            btnBack.Click += btnBack_Click;
            // 
            // txtEmail
            // 
            txtEmail.Location = new Point(123, 78);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(100, 23);
            txtEmail.TabIndex = 8;
            // 
            // txtTelefon
            // 
            txtTelefon.Location = new Point(123, 107);
            txtTelefon.Name = "txtTelefon";
            txtTelefon.Size = new Size(100, 23);
            txtTelefon.TabIndex = 9;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(12, 81);
            label4.Name = "label4";
            label4.Size = new Size(39, 15);
            label4.TabIndex = 10;
            label4.Text = "Email:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(12, 110);
            label5.Name = "label5";
            label5.Size = new Size(48, 15);
            label5.TabIndex = 11;
            label5.Text = "Telefon:";
            // 
            // dataGridViewUsers
            // 
            dataGridViewUsers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewUsers.Location = new Point(12, 217);
            dataGridViewUsers.Name = "dataGridViewUsers";
            dataGridViewUsers.Size = new Size(549, 268);
            dataGridViewUsers.TabIndex = 12;
            dataGridViewUsers.SelectionChanged += dataGridViewUsers_SelectionChanged;
            // 
            // btnUpdate
            // 
            btnUpdate.Location = new Point(132, 491);
            btnUpdate.Name = "btnUpdate";
            btnUpdate.Size = new Size(137, 28);
            btnUpdate.TabIndex = 13;
            btnUpdate.Text = "Aktualizovat uživatele";
            btnUpdate.UseVisualStyleBackColor = true;
            btnUpdate.Click += btnUpdate_Click;
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(275, 491);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(118, 28);
            btnDelete.TabIndex = 14;
            btnDelete.Text = "Odstranit uživatele";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label6.Location = new Point(12, 9);
            label6.Name = "label6";
            label6.Size = new Size(145, 21);
            label6.TabIndex = 15;
            label6.Text = "Správce uživatelů";
            // 
            // CreateUser
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(573, 522);
            Controls.Add(label6);
            Controls.Add(btnDelete);
            Controls.Add(btnUpdate);
            Controls.Add(dataGridViewUsers);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(txtTelefon);
            Controls.Add(txtEmail);
            Controls.Add(btnBack);
            Controls.Add(btnCreate);
            Controls.Add(comboBoxRole);
            Controls.Add(label3);
            Controls.Add(txtPassword);
            Controls.Add(label2);
            Controls.Add(txtUsername);
            Controls.Add(label1);
            Name = "CreateUser";
            Text = "Správce Uživatelů";
            ((System.ComponentModel.ISupportInitialize)dataGridViewUsers).EndInit();
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
        private TextBox txtEmail;
        private TextBox txtTelefon;
        private Label label4;
        private Label label5;
        private DataGridView dataGridViewUsers;
        private Button btnUpdate;
        private Button btnDelete;
        private Label label6;
    }
}