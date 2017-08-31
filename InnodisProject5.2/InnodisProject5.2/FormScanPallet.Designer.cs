namespace InnodisProject5._2
{
    partial class FormScanPallet
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.MainMenu mainMenu1;

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
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.btnLorryEAN128 = new System.Windows.Forms.MenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxPallet = new System.Windows.Forms.TextBox();
            this.dataGridPallet = new System.Windows.Forms.DataGrid();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this.menuItem1);
            this.mainMenu1.MenuItems.Add(this.btnLorryEAN128);
            // 
            // menuItem1
            // 
            this.menuItem1.Text = "Main Menu";
            this.menuItem1.Click += new System.EventHandler(this.menuItem1_Click);
            // 
            // btnLorryEAN128
            // 
            this.btnLorryEAN128.Text = "Print Lorry EAN128";
            this.btnLorryEAN128.Click += new System.EventHandler(this.btnLorryEAN128_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(7, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(233, 20);
            this.label1.Text = "Click on textbox below to scan Pallets";
            // 
            // textBoxPallet
            // 
            this.textBoxPallet.Location = new System.Drawing.Point(3, 14);
            this.textBoxPallet.Name = "textBoxPallet";
            this.textBoxPallet.Size = new System.Drawing.Size(234, 21);
            this.textBoxPallet.TabIndex = 2;
            this.textBoxPallet.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxPallet_KeyPress);
            // 
            // dataGridPallet
            // 
            this.dataGridPallet.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.dataGridPallet.Location = new System.Drawing.Point(3, 42);
            this.dataGridPallet.Name = "dataGridPallet";
            this.dataGridPallet.Size = new System.Drawing.Size(234, 223);
            this.dataGridPallet.TabIndex = 3;
            // 
            // FormScanPallet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.Controls.Add(this.dataGridPallet);
            this.Controls.Add(this.textBoxPallet);
            this.Controls.Add(this.label1);
            this.Menu = this.mainMenu1;
            this.Name = "FormScanPallet";
            this.Text = "FormScanPallet";
            this.Load += new System.EventHandler(this.FormScanPallet_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxPallet;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.DataGrid dataGridPallet;
        private System.Windows.Forms.MenuItem btnLorryEAN128;
    }
}