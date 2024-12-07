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
            roleLabel = new Label();
            currentUserLabel = new Label();
            menuStrip1 = new MenuStrip();
            uživatelToolStripMenuItem = new ToolStripMenuItem();
            loginStipItem = new ToolStripMenuItem();
            registerStipItem = new ToolStripMenuItem();
            uploadPfpToolStripMenuItem = new ToolStripMenuItem();
            nahratToolStripMenuItem = new ToolStripMenuItem();
            odstranitToolStripMenuItem = new ToolStripMenuItem();
            vytvoritUzivateleToolStripMenuItem = new ToolStripMenuItem();
            spravaKlientuToolStripMenuItem = new ToolStripMenuItem();
            recenzeToolStripMenuItem = new ToolStripMenuItem();
            správaRecenzíToolStripMenuItem = new ToolStripMenuItem();
            smazaneRecenzeToolStripMenuItem = new ToolStripMenuItem();
            pivaToolStripMenuItem = new ToolStripMenuItem();
            pridatPivoToolStripMenuItem = new ToolStripMenuItem();
            nizkyPocetPivToolStripMenuItem = new ToolStripMenuItem();
            statistikyToolStripMenuItem = new ToolStripMenuItem();
            pivovaryToolStripMenuItem = new ToolStripMenuItem();
            lokaceToolStripMenuItem = new ToolStripMenuItem();
            zamestnanciToolStripMenuItem = new ToolStripMenuItem();
            zobrazitToolStripMenuItem = new ToolStripMenuItem();
            spravovatToolStripMenuItem = new ToolStripMenuItem();
            prideleniToolStripMenuItem = new ToolStripMenuItem();
            vyrobkyToolStripMenuItem = new ToolStripMenuItem();
            objednavkyToolStripMenuItem1 = new ToolStripMenuItem();
            spravaPivovaruToolStripMenuItem = new ToolStripMenuItem();
            vlastniciToolStripMenuItem = new ToolStripMenuItem();
            spravaToolStripMenuItem = new ToolStripMenuItem();
            vlastniciSVicePivovaryToolStripMenuItem = new ToolStripMenuItem();
            vlastvnictviToolStripMenuItem = new ToolStripMenuItem();
            hierarchiePracovnikuToolStripMenuItem = new ToolStripMenuItem();
            topMestaToolStripMenuItem = new ToolStripMenuItem();
            spravaVyrobyToolStripMenuItem = new ToolStripMenuItem();
            objednavkyToolStripMenuItem = new ToolStripMenuItem();
            nesplneneObjednavkyToolStripMenuItem = new ToolStripMenuItem();
            evidenceToolStripMenuItem = new ToolStripMenuItem();
            spravaObjednavekToolStripMenuItem = new ToolStripMenuItem();
            splneneObjednavkyToolStripMenuItem = new ToolStripMenuItem();
            SpravaCiselnikuToolStrip = new ToolStripMenuItem();
            zobrazitSpravuCiselnikuToolStripMenuItem = new ToolStripMenuItem();
            spravaMestVesnicToolStripMenuItem = new ToolStripMenuItem();
            spravaFyzickychOsobToolStripMenuItem = new ToolStripMenuItem();
            btnLogout = new Button();
            profilePictureBox = new PictureBox();
            btnLogs = new Button();
            txtSearch = new TextBox();
            cbEmulace = new ComboBox();
            btnSystemKatalog = new Button();
            lblEmulace = new Label();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)profilePictureBox).BeginInit();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(12, 60);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new Size(817, 378);
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
            uživatelToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { loginStipItem, registerStipItem, uploadPfpToolStripMenuItem, vytvoritUzivateleToolStripMenuItem, spravaKlientuToolStripMenuItem });
            uživatelToolStripMenuItem.Name = "uživatelToolStripMenuItem";
            uživatelToolStripMenuItem.Size = new Size(60, 20);
            uživatelToolStripMenuItem.Text = "Uživatel";
            // 
            // loginStipItem
            // 
            loginStipItem.Name = "loginStipItem";
            loginStipItem.Size = new Size(165, 22);
            loginStipItem.Text = "Přihlásit se";
            loginStipItem.Click += loginStipItem_Click;
            // 
            // registerStipItem
            // 
            registerStipItem.Name = "registerStipItem";
            registerStipItem.Size = new Size(165, 22);
            registerStipItem.Text = "Registrace";
            registerStipItem.Click += registerStipItem_Click;
            // 
            // uploadPfpToolStripMenuItem
            // 
            uploadPfpToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { nahratToolStripMenuItem, odstranitToolStripMenuItem });
            uploadPfpToolStripMenuItem.Name = "uploadPfpToolStripMenuItem";
            uploadPfpToolStripMenuItem.Size = new Size(165, 22);
            uploadPfpToolStripMenuItem.Text = "Profilový obrázek";
            // 
            // nahratToolStripMenuItem
            // 
            nahratToolStripMenuItem.Name = "nahratToolStripMenuItem";
            nahratToolStripMenuItem.Size = new Size(123, 22);
            nahratToolStripMenuItem.Text = "Nahrát";
            nahratToolStripMenuItem.Click += nahratToolStripMenuItem_Click;
            // 
            // odstranitToolStripMenuItem
            // 
            odstranitToolStripMenuItem.Name = "odstranitToolStripMenuItem";
            odstranitToolStripMenuItem.Size = new Size(123, 22);
            odstranitToolStripMenuItem.Text = "Odstranit";
            odstranitToolStripMenuItem.Click += odstranitToolStripMenuItem_Click;
            // 
            // vytvoritUzivateleToolStripMenuItem
            // 
            vytvoritUzivateleToolStripMenuItem.Name = "vytvoritUzivateleToolStripMenuItem";
            vytvoritUzivateleToolStripMenuItem.Size = new Size(165, 22);
            vytvoritUzivateleToolStripMenuItem.Text = "Správa uživatelů";
            vytvoritUzivateleToolStripMenuItem.Click += vytvoritUzivateleToolStripMenuItem_Click;
            // 
            // spravaKlientuToolStripMenuItem
            // 
            spravaKlientuToolStripMenuItem.Name = "spravaKlientuToolStripMenuItem";
            spravaKlientuToolStripMenuItem.Size = new Size(165, 22);
            spravaKlientuToolStripMenuItem.Text = "Správa klientů";
            spravaKlientuToolStripMenuItem.Click += spravaKlientuToolStripMenuItem_Click;
            // 
            // recenzeToolStripMenuItem
            // 
            recenzeToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { správaRecenzíToolStripMenuItem, smazaneRecenzeToolStripMenuItem });
            recenzeToolStripMenuItem.Name = "recenzeToolStripMenuItem";
            recenzeToolStripMenuItem.Size = new Size(62, 20);
            recenzeToolStripMenuItem.Text = "Recenze";
            // 
            // správaRecenzíToolStripMenuItem
            // 
            správaRecenzíToolStripMenuItem.Name = "správaRecenzíToolStripMenuItem";
            správaRecenzíToolStripMenuItem.Size = new Size(164, 22);
            správaRecenzíToolStripMenuItem.Text = "Správa recenzí";
            správaRecenzíToolStripMenuItem.Click += spravaRecenziToolStripMenuItem_Click;
            // 
            // smazaneRecenzeToolStripMenuItem
            // 
            smazaneRecenzeToolStripMenuItem.Name = "smazaneRecenzeToolStripMenuItem";
            smazaneRecenzeToolStripMenuItem.Size = new Size(164, 22);
            smazaneRecenzeToolStripMenuItem.Text = "Smazané recenze";
            smazaneRecenzeToolStripMenuItem.Click += smazaneRecenzeToolStripMenuItem_Click;
            // 
            // pivaToolStripMenuItem
            // 
            pivaToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { pridatPivoToolStripMenuItem, nizkyPocetPivToolStripMenuItem, statistikyToolStripMenuItem });
            pivaToolStripMenuItem.Name = "pivaToolStripMenuItem";
            pivaToolStripMenuItem.Size = new Size(41, 20);
            pivaToolStripMenuItem.Text = "Piva";
            // 
            // pridatPivoToolStripMenuItem
            // 
            pridatPivoToolStripMenuItem.Name = "pridatPivoToolStripMenuItem";
            pridatPivoToolStripMenuItem.Size = new Size(155, 22);
            pridatPivoToolStripMenuItem.Text = "Správa";
            pridatPivoToolStripMenuItem.Click += pridatPivoToolStripMenuItem_Click;
            // 
            // nizkyPocetPivToolStripMenuItem
            // 
            nizkyPocetPivToolStripMenuItem.Name = "nizkyPocetPivToolStripMenuItem";
            nizkyPocetPivToolStripMenuItem.Size = new Size(155, 22);
            nizkyPocetPivToolStripMenuItem.Text = "Nizky pocet piv";
            nizkyPocetPivToolStripMenuItem.Click += nizkyPocetPivToolStripMenuItem_Click;
            // 
            // statistikyToolStripMenuItem
            // 
            statistikyToolStripMenuItem.Name = "statistikyToolStripMenuItem";
            statistikyToolStripMenuItem.Size = new Size(155, 22);
            statistikyToolStripMenuItem.Text = "Statistiky";
            statistikyToolStripMenuItem.Click += statistikyToolStripMenuItem_Click;
            // 
            // pivovaryToolStripMenuItem
            // 
            pivovaryToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { lokaceToolStripMenuItem, zamestnanciToolStripMenuItem, vyrobkyToolStripMenuItem, objednavkyToolStripMenuItem1, spravaPivovaruToolStripMenuItem, vlastniciToolStripMenuItem, hierarchiePracovnikuToolStripMenuItem, topMestaToolStripMenuItem, spravaVyrobyToolStripMenuItem });
            pivovaryToolStripMenuItem.Name = "pivovaryToolStripMenuItem";
            pivovaryToolStripMenuItem.Size = new Size(64, 20);
            pivovaryToolStripMenuItem.Text = "Pivovary";
            // 
            // lokaceToolStripMenuItem
            // 
            lokaceToolStripMenuItem.Name = "lokaceToolStripMenuItem";
            lokaceToolStripMenuItem.Size = new Size(190, 22);
            lokaceToolStripMenuItem.Text = "Lokace";
            lokaceToolStripMenuItem.Click += lokaceToolStripMenuItem_Click;
            // 
            // zamestnanciToolStripMenuItem
            // 
            zamestnanciToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { zobrazitToolStripMenuItem, spravovatToolStripMenuItem, prideleniToolStripMenuItem });
            zamestnanciToolStripMenuItem.Name = "zamestnanciToolStripMenuItem";
            zamestnanciToolStripMenuItem.Size = new Size(190, 22);
            zamestnanciToolStripMenuItem.Text = "Zaměstnanci";
            zamestnanciToolStripMenuItem.Click += zamestnanciToolStripMenuItem_Click;
            // 
            // zobrazitToolStripMenuItem
            // 
            zobrazitToolStripMenuItem.Name = "zobrazitToolStripMenuItem";
            zobrazitToolStripMenuItem.Size = new Size(126, 22);
            zobrazitToolStripMenuItem.Text = "Pozice";
            zobrazitToolStripMenuItem.Click += zobrazitToolStripMenuItem_Click;
            // 
            // spravovatToolStripMenuItem
            // 
            spravovatToolStripMenuItem.Name = "spravovatToolStripMenuItem";
            spravovatToolStripMenuItem.Size = new Size(126, 22);
            spravovatToolStripMenuItem.Text = "Spravovat";
            spravovatToolStripMenuItem.Click += spravovatToolStripMenuItem_Click;
            // 
            // prideleniToolStripMenuItem
            // 
            prideleniToolStripMenuItem.Name = "prideleniToolStripMenuItem";
            prideleniToolStripMenuItem.Size = new Size(126, 22);
            prideleniToolStripMenuItem.Text = "Přidělení";
            prideleniToolStripMenuItem.Click += prideleniToolStripMenuItem_Click;
            // 
            // vyrobkyToolStripMenuItem
            // 
            vyrobkyToolStripMenuItem.Name = "vyrobkyToolStripMenuItem";
            vyrobkyToolStripMenuItem.Size = new Size(190, 22);
            vyrobkyToolStripMenuItem.Text = "Vyrobky";
            vyrobkyToolStripMenuItem.Click += vyrobkyToolStripMenuItem_Click;
            // 
            // objednavkyToolStripMenuItem1
            // 
            objednavkyToolStripMenuItem1.Name = "objednavkyToolStripMenuItem1";
            objednavkyToolStripMenuItem1.Size = new Size(190, 22);
            objednavkyToolStripMenuItem1.Text = "Objednavky";
            objednavkyToolStripMenuItem1.Click += objednavkyToolStripMenuItem1_Click;
            // 
            // spravaPivovaruToolStripMenuItem
            // 
            spravaPivovaruToolStripMenuItem.Name = "spravaPivovaruToolStripMenuItem";
            spravaPivovaruToolStripMenuItem.Size = new Size(190, 22);
            spravaPivovaruToolStripMenuItem.Text = "Správa pivovarů";
            spravaPivovaruToolStripMenuItem.Click += spravaPivovaruToolStripMenuItem_Click;
            // 
            // vlastniciToolStripMenuItem
            // 
            vlastniciToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { spravaToolStripMenuItem, vlastniciSVicePivovaryToolStripMenuItem, vlastvnictviToolStripMenuItem });
            vlastniciToolStripMenuItem.Name = "vlastniciToolStripMenuItem";
            vlastniciToolStripMenuItem.Size = new Size(190, 22);
            vlastniciToolStripMenuItem.Text = "Vlastníci";
            // 
            // spravaToolStripMenuItem
            // 
            spravaToolStripMenuItem.Name = "spravaToolStripMenuItem";
            spravaToolStripMenuItem.Size = new Size(198, 22);
            spravaToolStripMenuItem.Text = "Správa";
            spravaToolStripMenuItem.Click += spravaToolStripMenuItem_Click;
            // 
            // vlastniciSVicePivovaryToolStripMenuItem
            // 
            vlastniciSVicePivovaryToolStripMenuItem.Name = "vlastniciSVicePivovaryToolStripMenuItem";
            vlastniciSVicePivovaryToolStripMenuItem.Size = new Size(198, 22);
            vlastniciSVicePivovaryToolStripMenuItem.Text = "Vlastníci s více pivovary";
            vlastniciSVicePivovaryToolStripMenuItem.Click += vlastniciSVicePivovaryToolStripMenuItem_Click;
            // 
            // vlastvnictviToolStripMenuItem
            // 
            vlastvnictviToolStripMenuItem.Name = "vlastvnictviToolStripMenuItem";
            vlastvnictviToolStripMenuItem.Size = new Size(198, 22);
            vlastvnictviToolStripMenuItem.Text = "Vlastvnictví";
            vlastvnictviToolStripMenuItem.Click += vlastvnictviToolStripMenuItem_Click;
            // 
            // hierarchiePracovnikuToolStripMenuItem
            // 
            hierarchiePracovnikuToolStripMenuItem.Name = "hierarchiePracovnikuToolStripMenuItem";
            hierarchiePracovnikuToolStripMenuItem.Size = new Size(190, 22);
            hierarchiePracovnikuToolStripMenuItem.Text = "Hierarchie pracovníků";
            hierarchiePracovnikuToolStripMenuItem.Click += hierarchiePracovnikuToolStripMenuItem_Click;
            // 
            // topMestaToolStripMenuItem
            // 
            topMestaToolStripMenuItem.Name = "topMestaToolStripMenuItem";
            topMestaToolStripMenuItem.Size = new Size(190, 22);
            topMestaToolStripMenuItem.Text = "Top města";
            topMestaToolStripMenuItem.Click += topMestaToolStripMenuItem_Click;
            // 
            // spravaVyrobyToolStripMenuItem
            // 
            spravaVyrobyToolStripMenuItem.Name = "spravaVyrobyToolStripMenuItem";
            spravaVyrobyToolStripMenuItem.Size = new Size(190, 22);
            spravaVyrobyToolStripMenuItem.Text = "Správa výroby";
            spravaVyrobyToolStripMenuItem.Click += spravaVyrobyToolStripMenuItem_Click;
            // 
            // objednavkyToolStripMenuItem
            // 
            objednavkyToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { nesplneneObjednavkyToolStripMenuItem, evidenceToolStripMenuItem, spravaObjednavekToolStripMenuItem, splneneObjednavkyToolStripMenuItem });
            objednavkyToolStripMenuItem.Name = "objednavkyToolStripMenuItem";
            objednavkyToolStripMenuItem.Size = new Size(82, 20);
            objednavkyToolStripMenuItem.Text = "Objednávky";
            // 
            // nesplneneObjednavkyToolStripMenuItem
            // 
            nesplneneObjednavkyToolStripMenuItem.Name = "nesplneneObjednavkyToolStripMenuItem";
            nesplneneObjednavkyToolStripMenuItem.Size = new Size(238, 22);
            nesplneneObjednavkyToolStripMenuItem.Text = "Zobrazit nesplněné objednávky";
            nesplneneObjednavkyToolStripMenuItem.Click += explicidCursorToolStripMenuItem_Click;
            // 
            // evidenceToolStripMenuItem
            // 
            evidenceToolStripMenuItem.Name = "evidenceToolStripMenuItem";
            evidenceToolStripMenuItem.Size = new Size(238, 22);
            evidenceToolStripMenuItem.Text = "Evidence";
            evidenceToolStripMenuItem.Click += evidenceToolStripMenuItem_Click;
            // 
            // spravaObjednavekToolStripMenuItem
            // 
            spravaObjednavekToolStripMenuItem.Name = "spravaObjednavekToolStripMenuItem";
            spravaObjednavekToolStripMenuItem.Size = new Size(238, 22);
            spravaObjednavekToolStripMenuItem.Text = "Sprava Objednávek";
            spravaObjednavekToolStripMenuItem.Click += spravaObjednavekToolStripMenuItem_Click;
            // 
            // splneneObjednavkyToolStripMenuItem
            // 
            splneneObjednavkyToolStripMenuItem.Name = "splneneObjednavkyToolStripMenuItem";
            splneneObjednavkyToolStripMenuItem.Size = new Size(238, 22);
            splneneObjednavkyToolStripMenuItem.Text = "Splněné objednávky";
            splneneObjednavkyToolStripMenuItem.Click += splneneObjednavkyToolStripMenuItem_Click;
            // 
            // SpravaCiselnikuToolStrip
            // 
            SpravaCiselnikuToolStrip.DropDownItems.AddRange(new ToolStripItem[] { zobrazitSpravuCiselnikuToolStripMenuItem, spravaMestVesnicToolStripMenuItem, spravaFyzickychOsobToolStripMenuItem });
            SpravaCiselnikuToolStrip.Name = "SpravaCiselnikuToolStrip";
            SpravaCiselnikuToolStrip.Size = new Size(103, 20);
            SpravaCiselnikuToolStrip.Text = "Správa číselníků";
            // 
            // zobrazitSpravuCiselnikuToolStripMenuItem
            // 
            zobrazitSpravuCiselnikuToolStripMenuItem.Name = "zobrazitSpravuCiselnikuToolStripMenuItem";
            zobrazitSpravuCiselnikuToolStripMenuItem.Size = new Size(193, 22);
            zobrazitSpravuCiselnikuToolStripMenuItem.Text = "Zobrazit";
            zobrazitSpravuCiselnikuToolStripMenuItem.Click += zobrazitSpravuCiselnikuToolStripMenuItem_Click;
            // 
            // spravaMestVesnicToolStripMenuItem
            // 
            spravaMestVesnicToolStripMenuItem.Name = "spravaMestVesnicToolStripMenuItem";
            spravaMestVesnicToolStripMenuItem.Size = new Size(193, 22);
            spravaMestVesnicToolStripMenuItem.Text = "Správa měst a vesnic";
            spravaMestVesnicToolStripMenuItem.Click += spravaMestVesnicToolStripMenuItem_Click;
            // 
            // spravaFyzickychOsobToolStripMenuItem
            // 
            spravaFyzickychOsobToolStripMenuItem.Name = "spravaFyzickychOsobToolStripMenuItem";
            spravaFyzickychOsobToolStripMenuItem.Size = new Size(193, 22);
            spravaFyzickychOsobToolStripMenuItem.Text = "Správa  fyzických osob";
            spravaFyzickychOsobToolStripMenuItem.Click += spravaFyzickychOsobToolStripMenuItem_Click;
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
            // btnLogs
            // 
            btnLogs.Location = new Point(835, 358);
            btnLogs.Name = "btnLogs";
            btnLogs.Size = new Size(121, 37);
            btnLogs.TabIndex = 12;
            btnLogs.Text = "Zobrazit logs";
            btnLogs.UseVisualStyleBackColor = true;
            btnLogs.Click += btnLogs_Click;
            // 
            // txtSearch
            // 
            txtSearch.Location = new Point(12, 31);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(817, 23);
            txtSearch.TabIndex = 13;
            txtSearch.TextChanged += txtSearch_TextChanged;
            // 
            // cbEmulace
            // 
            cbEmulace.FormattingEnabled = true;
            cbEmulace.Location = new Point(835, 192);
            cbEmulace.Name = "cbEmulace";
            cbEmulace.Size = new Size(121, 23);
            cbEmulace.TabIndex = 14;
            cbEmulace.SelectedIndexChanged += cbEmulace_SelectedIndexChanged;
            // 
            // btnSystemKatalog
            // 
            btnSystemKatalog.Location = new Point(835, 319);
            btnSystemKatalog.Name = "btnSystemKatalog";
            btnSystemKatalog.Size = new Size(121, 33);
            btnSystemKatalog.TabIndex = 15;
            btnSystemKatalog.Text = "Systémový katalog";
            btnSystemKatalog.UseVisualStyleBackColor = true;
            btnSystemKatalog.Click += btnSystemKatalog_Click;
            // 
            // lblEmulace
            // 
            lblEmulace.AutoSize = true;
            lblEmulace.Location = new Point(835, 174);
            lblEmulace.Name = "lblEmulace";
            lblEmulace.Size = new Size(55, 15);
            lblEmulace.TabIndex = 16;
            lblEmulace.Text = "Emulace:";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(968, 450);
            Controls.Add(lblEmulace);
            Controls.Add(btnSystemKatalog);
            Controls.Add(cbEmulace);
            Controls.Add(txtSearch);
            Controls.Add(btnLogs);
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
        private ToolStripMenuItem nesplneneObjednavkyToolStripMenuItem;
        private Button btnLogout;
        private ToolStripMenuItem uploadPfpToolStripMenuItem;
        private PictureBox profilePictureBox;
        private ToolStripMenuItem vytvoritUzivateleToolStripMenuItem;
        private ToolStripMenuItem zobrazitToolStripMenuItem;
        private ToolStripMenuItem spravovatToolStripMenuItem;
        private ToolStripMenuItem SpravaCiselnikuToolStrip;
        private ToolStripMenuItem vyrobkyToolStripMenuItem;
        private ToolStripMenuItem objednavkyToolStripMenuItem1;
        private ToolStripMenuItem nizkyPocetPivToolStripMenuItem;
        private ToolStripMenuItem statistikyToolStripMenuItem;
        private ToolStripMenuItem evidenceToolStripMenuItem;
        private ToolStripMenuItem zobrazitSpravuCiselnikuToolStripMenuItem;
        private ToolStripMenuItem spravaMestVesnicToolStripMenuItem;
        private ToolStripMenuItem spravaObjednavekToolStripMenuItem;
        private ToolStripMenuItem spravaPivovaruToolStripMenuItem;
        private ToolStripMenuItem vlastniciToolStripMenuItem;
        private ToolStripMenuItem spravaKlientuToolStripMenuItem;
        private ToolStripMenuItem nahratToolStripMenuItem;
        private ToolStripMenuItem odstranitToolStripMenuItem;
        private ToolStripMenuItem hierarchiePracovnikuToolStripMenuItem;
        private ToolStripMenuItem spravaToolStripMenuItem;
        private ToolStripMenuItem vlastniciSVicePivovaryToolStripMenuItem;
        private ToolStripMenuItem topMestaToolStripMenuItem;
        private ToolStripMenuItem spravaFyzickychOsobToolStripMenuItem;
        private ToolStripMenuItem prideleniToolStripMenuItem;
        private ToolStripMenuItem smazaneRecenzeToolStripMenuItem;
        private ToolStripMenuItem splneneObjednavkyToolStripMenuItem;
        private ToolStripMenuItem spravaVyrobyToolStripMenuItem;
        private Button btnLogs;
        private TextBox txtSearch;
        private ComboBox cbEmulace;
        private Button btnSystemKatalog;
        private ToolStripMenuItem vlastvnictviToolStripMenuItem;
        private Label lblEmulace;
    }
}
