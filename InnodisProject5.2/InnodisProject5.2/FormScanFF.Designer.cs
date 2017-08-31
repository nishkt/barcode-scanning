namespace InnodisProject5._2
{
    partial class FormScanFF
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
            this.PrintEAN128 = new System.Windows.Forms.MenuItem();
            this.comboBoxColdRoom = new System.Windows.Forms.ComboBox();
            this.textBoxScanFrozen = new System.Windows.Forms.TextBox();
            this.dataGrid1 = new System.Windows.Forms.DataGrid();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this.menuItem1);
            this.mainMenu1.MenuItems.Add(this.PrintEAN128);
            // 
            // menuItem1
            // 
            this.menuItem1.Text = "Main Menu";
            this.menuItem1.Click += new System.EventHandler(this.menuItem1_Click);
            // 
            // PrintEAN128
            // 
            this.PrintEAN128.Text = "Print EAN128";
            this.PrintEAN128.Click += new System.EventHandler(this.PrintEAN128_Click);
            // 
            // comboBoxColdRoom
            // 
            this.comboBoxColdRoom.Location = new System.Drawing.Point(4, 4);
            this.comboBoxColdRoom.Name = "comboBoxColdRoom";
            this.comboBoxColdRoom.Size = new System.Drawing.Size(233, 22);
            this.comboBoxColdRoom.TabIndex = 0;
            // 
            // textBoxScanFrozen
            // 
            this.textBoxScanFrozen.Location = new System.Drawing.Point(4, 33);
            this.textBoxScanFrozen.Name = "textBoxScanFrozen";
            this.textBoxScanFrozen.Size = new System.Drawing.Size(233, 21);
            this.textBoxScanFrozen.TabIndex = 1;
            this.textBoxScanFrozen.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxScanFrozen_KeyPress);
            // 
            // dataGrid1
            // 
            this.dataGrid1.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.dataGrid1.Location = new System.Drawing.Point(4, 61);
            this.dataGrid1.Name = "dataGrid1";
            this.dataGrid1.Size = new System.Drawing.Size(233, 197);
            this.dataGrid1.TabIndex = 2;
            // 
            // FormScanFF
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.Controls.Add(this.dataGrid1);
            this.Controls.Add(this.textBoxScanFrozen);
            this.Controls.Add(this.comboBoxColdRoom);
            this.Menu = this.mainMenu1;
            this.Name = "FormScanFF";
            this.Text = "FormScanFF";
            this.Load += new System.EventHandler(this.FormScanFF_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem PrintEAN128;
        private System.Windows.Forms.ComboBox comboBoxColdRoom;
        private System.Windows.Forms.TextBox textBoxScanFrozen;
        private System.Windows.Forms.DataGrid dataGrid1;

    }
}