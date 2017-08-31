namespace InnodisProject5._2
{
    partial class FormScabEAB128
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
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxEAN128 = new System.Windows.Forms.TextBox();
            this.dataGridView = new System.Windows.Forms.DataGrid();
            this.PrintEAN128 = new System.Windows.Forms.MenuItem();
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
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(233, 20);
            this.label1.Text = "Click on textbox below to scan EAN128";
            // 
            // textBoxEAN128
            // 
            this.textBoxEAN128.Location = new System.Drawing.Point(3, 14);
            this.textBoxEAN128.Name = "textBoxEAN128";
            this.textBoxEAN128.Size = new System.Drawing.Size(233, 21);
            this.textBoxEAN128.TabIndex = 17;
            this.textBoxEAN128.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxEAN128_KeyPress);
            // 
            // dataGridView
            // 
            this.dataGridView.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.dataGridView.Location = new System.Drawing.Point(3, 41);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.Size = new System.Drawing.Size(233, 224);
            this.dataGridView.TabIndex = 19;
            // 
            // PrintEAN128
            // 
            this.PrintEAN128.Text = "Print Pallet";
            this.PrintEAN128.Click += new System.EventHandler(this.PrintEAN128_Click);
            // 
            // FormScabEAB128
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.Controls.Add(this.dataGridView);
            this.Controls.Add(this.textBoxEAN128);
            this.Controls.Add(this.label1);
            this.Menu = this.mainMenu1;
            this.Name = "FormScabEAB128";
            this.Text = "FormScabEAB128";
            this.Load += new System.EventHandler(this.FormScabEAB128_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxEAN128;
        private System.Windows.Forms.DataGrid dataGridView;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem PrintEAN128;
    }
}