﻿namespace HospodaUBobra
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
            roleLabel = new Label();
            currentUserLabel = new Label();
            menuStrip1 = new MenuStrip();
            uživatelToolStripMenuItem = new ToolStripMenuItem();
            loginStipItem = new ToolStripMenuItem();
            registerStipItem = new ToolStripMenuItem();
            uploadPfpToolStripMenuItem = new ToolStripMenuItem();
            vytvoritUzivateleToolStripMenuItem = new ToolStripMenuItem();
            recenzeToolStripMenuItem = new ToolStripMenuItem();
            správaRecenzíToolStripMenuItem = new ToolStripMenuItem();
            pivaToolStripMenuItem = new ToolStripMenuItem();
            pridatPivoToolStripMenuItem = new ToolStripMenuItem();
            pivovaryToolStripMenuItem = new ToolStripMenuItem();
            lokaceToolStripMenuItem = new ToolStripMenuItem();
            zamestnanciToolStripMenuItem = new ToolStripMenuItem();
            zobrazitToolStripMenuItem = new ToolStripMenuItem();
            spravovatToolStripMenuItem = new ToolStripMenuItem();
            objednavkyToolStripMenuItem = new ToolStripMenuItem();
            explicidCursorToolStripMenuItem = new ToolStripMenuItem();
            SpravaCiselnikuToolStrip = new ToolStripMenuItem();
            btnLogout = new Button();
            profilePictureBox = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)profilePictureBox).BeginInit();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(12, 24);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new Size(817, 414);
            dataGridView1.TabIndex = 0;
            // 
            // comboBoxTables
            // 
            comboBoxTables.FormattingEnabled = true;
            comboBoxTables.Location = new Point(835, 110);
            comboBoxTables.Name = "comboBoxTables";
            comboBoxTables.Size = new Size(121, 23);
            comboBoxTables.TabIndex = 1;
            // 
            // roleLabel
            // 
            roleLabel.AutoSize = true;
            roleLabel.Location = new Point(864, 43);
            roleLabel.Name = "roleLabel";
            roleLabel.Size = new Size(0, 15);
            roleLabel.TabIndex = 3;
            // 
            // currentUserLabel
            // 
            currentUserLabel.AutoSize = true;
            currentUserLabel.Location = new Point(835, 92);
            currentUserLabel.Name = "currentUserLabel";
            currentUserLabel.Size = new Size(38, 15);
            currentUserLabel.TabIndex = 5;
            currentUserLabel.Text = "label1";
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { uživatelToolStripMenuItem, recenzeToolStripMenuItem, pivaToolStripMenuItem, pivovaryToolStripMenuItem, objednavkyToolStripMenuItem, SpravaCiselnikuToolStrip });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Padding = new Padding(5, 2, 0, 2);
            menuStrip1.Size = new Size(968, 24);
            menuStrip1.TabIndex = 9;
            menuStrip1.Text = "menuStrip1";
            // 
            // uživatelToolStripMenuItem
            // 
            uživatelToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { loginStipItem, registerStipItem, uploadPfpToolStripMenuItem, vytvoritUzivateleToolStripMenuItem });
            uživatelToolStripMenuItem.Name = "uživatelToolStripMenuItem";
            uživatelToolStripMenuItem.Size = new Size(60, 20);
            uživatelToolStripMenuItem.Text = "Uživatel";
            // 
            // loginStipItem
            // 
            loginStipItem.Name = "loginStipItem";
            loginStipItem.Size = new Size(204, 22);
            loginStipItem.Text = "Přihlásit se";
            loginStipItem.Click += loginStipItem_Click;
            // 
            // registerStipItem
            // 
            registerStipItem.Name = "registerStipItem";
            registerStipItem.Size = new Size(204, 22);
            registerStipItem.Text = "Registrace";
            registerStipItem.Click += registerStipItem_Click;
            // 
            // uploadPfpToolStripMenuItem
            // 
            uploadPfpToolStripMenuItem.Name = "uploadPfpToolStripMenuItem";
            uploadPfpToolStripMenuItem.Size = new Size(204, 22);
            uploadPfpToolStripMenuItem.Text = "Nahrát profilový obrázek";
            uploadPfpToolStripMenuItem.Click += uploadPfpToolStripMenuItem_Click;
            // 
            // vytvoritUzivateleToolStripMenuItem
            // 
            vytvoritUzivateleToolStripMenuItem.Name = "vytvoritUzivateleToolStripMenuItem";
            vytvoritUzivateleToolStripMenuItem.Size = new Size(204, 22);
            vytvoritUzivateleToolStripMenuItem.Text = "Správa uživatelů";
            vytvoritUzivateleToolStripMenuItem.Click += vytvoritUzivateleToolStripMenuItem_Click;
            // 
            // recenzeToolStripMenuItem
            // 
            recenzeToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { správaRecenzíToolStripMenuItem });
            recenzeToolStripMenuItem.Name = "recenzeToolStripMenuItem";
            recenzeToolStripMenuItem.Size = new Size(62, 20);
            recenzeToolStripMenuItem.Text = "Recenze";
            // 
            // správaRecenzíToolStripMenuItem
            // 
            správaRecenzíToolStripMenuItem.Name = "správaRecenzíToolStripMenuItem";
            správaRecenzíToolStripMenuItem.Size = new Size(149, 22);
            správaRecenzíToolStripMenuItem.Text = "Správa recenzí";
            správaRecenzíToolStripMenuItem.Click += spravaRecenziToolStripMenuItem_Click;
            // 
            // pivaToolStripMenuItem
            // 
            pivaToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { pridatPivoToolStripMenuItem });
            pivaToolStripMenuItem.Name = "pivaToolStripMenuItem";
            pivaToolStripMenuItem.Size = new Size(41, 20);
            pivaToolStripMenuItem.Text = "Piva";
            // 
            // pridatPivoToolStripMenuItem
            // 
            pridatPivoToolStripMenuItem.Name = "pridatPivoToolStripMenuItem";
            pridatPivoToolStripMenuItem.Size = new Size(105, 22);
            pridatPivoToolStripMenuItem.Text = "Přidat";
            pridatPivoToolStripMenuItem.Click += pridatPivoToolStripMenuItem_Click;
            // 
            // pivovaryToolStripMenuItem
            // 
            pivovaryToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { lokaceToolStripMenuItem, zamestnanciToolStripMenuItem });
            pivovaryToolStripMenuItem.Name = "pivovaryToolStripMenuItem";
            pivovaryToolStripMenuItem.Size = new Size(64, 20);
            pivovaryToolStripMenuItem.Text = "Pivovary";
            // 
            // lokaceToolStripMenuItem
            // 
            lokaceToolStripMenuItem.Name = "lokaceToolStripMenuItem";
            lokaceToolStripMenuItem.Size = new Size(142, 22);
            lokaceToolStripMenuItem.Text = "Lokace";
            lokaceToolStripMenuItem.Click += lokaceToolStripMenuItem_Click;
            // 
            // zamestnanciToolStripMenuItem
            // 
            zamestnanciToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { zobrazitToolStripMenuItem, spravovatToolStripMenuItem });
            zamestnanciToolStripMenuItem.Name = "zamestnanciToolStripMenuItem";
            zamestnanciToolStripMenuItem.Size = new Size(142, 22);
            zamestnanciToolStripMenuItem.Text = "Zaměstnanci";
            zamestnanciToolStripMenuItem.Click += zamestnanciToolStripMenuItem_Click;
            // 
            // zobrazitToolStripMenuItem
            // 
            zobrazitToolStripMenuItem.Name = "zobrazitToolStripMenuItem";
            zobrazitToolStripMenuItem.Size = new Size(126, 22);
            zobrazitToolStripMenuItem.Text = "Zobrazit";
            zobrazitToolStripMenuItem.Click += zobrazitToolStripMenuItem_Click;
            // 
            // spravovatToolStripMenuItem
            // 
            spravovatToolStripMenuItem.Name = "spravovatToolStripMenuItem";
            spravovatToolStripMenuItem.Size = new Size(126, 22);
            spravovatToolStripMenuItem.Text = "Spravovat";
            spravovatToolStripMenuItem.Click += spravovatToolStripMenuItem_Click;
            // 
            // objednavkyToolStripMenuItem
            // 
            objednavkyToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { explicidCursorToolStripMenuItem });
            objednavkyToolStripMenuItem.Name = "objednavkyToolStripMenuItem";
            objednavkyToolStripMenuItem.Size = new Size(82, 20);
            objednavkyToolStripMenuItem.Text = "Objednávky";
            // 
            // explicidCursorToolStripMenuItem
            // 
            explicidCursorToolStripMenuItem.Name = "explicidCursorToolStripMenuItem";
            explicidCursorToolStripMenuItem.Size = new Size(211, 22);
            explicidCursorToolStripMenuItem.Text = "Klienti a jejich objednávky";
            explicidCursorToolStripMenuItem.Click += explicidCursorToolStripMenuItem_Click;
            // 
            // SpravaCiselnikuToolStrip
            // 
            SpravaCiselnikuToolStrip.Name = "SpravaCiselnikuToolStrip";
            SpravaCiselnikuToolStrip.Size = new Size(103, 20);
            SpravaCiselnikuToolStrip.Text = "Správa číselníků";
            SpravaCiselnikuToolStrip.Click += SpravaCiselnikuToolStrip_Click;
            // 
            // btnLogout
            // 
            btnLogout.Location = new Point(835, 401);
            btnLogout.Name = "btnLogout";
            btnLogout.Size = new Size(121, 37);
            btnLogout.TabIndex = 10;
            btnLogout.Text = "Odhlásit se";
            btnLogout.UseVisualStyleBackColor = true;
            btnLogout.Click += btnLogout_Click;
            // 
            // profilePictureBox
            // 
            profilePictureBox.Location = new Point(835, 27);
            profilePictureBox.Name = "profilePictureBox";
            profilePictureBox.Size = new Size(64, 62);
            profilePictureBox.TabIndex = 11;
            profilePictureBox.TabStop = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(968, 450);
            Controls.Add(profilePictureBox);
            Controls.Add(btnLogout);
            Controls.Add(currentUserLabel);
            Controls.Add(roleLabel);
            Controls.Add(comboBoxTables);
            Controls.Add(dataGridView1);
            Controls.Add(menuStrip1);
            Name = "Form1";
            Text = "Hospoda U Bobra";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)profilePictureBox).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dataGridView1;
        private ComboBox comboBoxTables;
        private Label roleLabel;
        private Label currentUserLabel;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem uživatelToolStripMenuItem;
        private ToolStripMenuItem loginStipItem;
        private ToolStripMenuItem registerStipItem;
        private ToolStripMenuItem recenzeToolStripMenuItem;
        private ToolStripMenuItem správaRecenzíToolStripMenuItem;
        private ToolStripMenuItem pivaToolStripMenuItem;
        private ToolStripMenuItem pridatPivoToolStripMenuItem;
        private ToolStripMenuItem pivovaryToolStripMenuItem;
        private ToolStripMenuItem lokaceToolStripMenuItem;
        private ToolStripMenuItem zamestnanciToolStripMenuItem;
        private ToolStripMenuItem objednavkyToolStripMenuItem;
        private ToolStripMenuItem explicidCursorToolStripMenuItem;
        private Button btnLogout;
        private ToolStripMenuItem uploadPfpToolStripMenuItem;
        private PictureBox profilePictureBox;
        private ToolStripMenuItem vytvoritUzivateleToolStripMenuItem;
        private ToolStripMenuItem zobrazitToolStripMenuItem;
        private ToolStripMenuItem spravovatToolStripMenuItem;
        private ToolStripMenuItem SpravaCiselnikuToolStrip;
    }
}
