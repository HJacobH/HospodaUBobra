namespace HospodaUBobra
{
    partial class Hierarchie
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
            treeView = new TreeView();
            btnback = new Button();
            SuspendLayout();
            // 
            // treeView
            // 
            treeView.Location = new Point(12, 12);
            treeView.Name = "treeView";
            treeView.Size = new Size(365, 426);
            treeView.TabIndex = 0;
            // 
            // btnback
            // 
            btnback.Location = new Point(383, 415);
            btnback.Name = "btnback";
            btnback.Size = new Size(75, 23);
            btnback.TabIndex = 1;
            btnback.Text = "Zpět";
            btnback.UseVisualStyleBackColor = true;
            btnback.Click += btnback_Click;
            // 
            // Hierarchie
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(470, 450);
            Controls.Add(btnback);
            Controls.Add(treeView);
            Name = "Hierarchie";
            Text = "Hierarchie";
            ResumeLayout(false);
        }

        #endregion

        private TreeView treeView;
        private Button btnback;
    }
}