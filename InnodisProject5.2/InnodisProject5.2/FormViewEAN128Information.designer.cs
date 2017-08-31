namespace InnodisProject5._2
{
    partial class FormViewEAN128Information
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
            System.Windows.Forms.MainMenu mainMenu1;
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.textBoxEAN128 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dataGridView = new System.Windows.Forms.DataGrid();
            this.dataGridViewNwAndUnits = new System.Windows.Forms.DataGrid();
            this.RePrintEAN128 = new System.Windows.Forms.MenuItem();
            mainMenu1 = new System.Windows.Forms.MainMenu();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            mainMenu1.MenuItems.Add(this.menuItem1);
            mainMenu1.MenuItems.Add(this.RePrintEAN128);
            // 
            // menuItem1
            // 
            this.menuItem1.Text = "Main Menu";
            this.menuItem1.Click += new System.EventHandler(this.menuItem1_Click);
            // 
            // textBoxEAN128
            // 
            this.textBoxEAN128.Location = new System.Drawing.Point(4, 27);
            this.textBoxEAN128.Name = "textBoxEAN128";
            this.textBoxEAN128.Size = new System.Drawing.Size(233, 21);
            this.textBoxEAN128.TabIndex = 16;
            this.textBoxEAN128.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            this.textBoxEAN128.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(4, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(233, 20);
            this.label1.Text = "Click on textbox below to scan EAN128";
            // 
            // dataGridView
            // 
            this.dataGridView.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.dataGridView.Location = new System.Drawing.Point(4, 54);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.Size = new System.Drawing.Size(233, 117);
            this.dataGridView.TabIndex = 18;
            // 
            // dataGridViewNwAndUnits
            // 
            this.dataGridViewNwAndUnits.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.dataGridViewNwAndUnits.Location = new System.Drawing.Point(3, 177);
            this.dataGridViewNwAndUnits.Name = "dataGridViewNwAndUnits";
            this.dataGridViewNwAndUnits.Size = new System.Drawing.Size(233, 88);
            this.dataGridViewNwAndUnits.TabIndex = 22;
            // 
            // RePrintEAN128
            // 
            this.RePrintEAN128.Text = "Re-Print EAN128";
            this.RePrintEAN128.Click += new System.EventHandler(this.RePrintEAN128_Click);
            // 
            // FormViewEAN128Information
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.ControlBox = false;
            this.Controls.Add(this.dataGridViewNwAndUnits);
            this.Controls.Add(this.dataGridView);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxEAN128);
            this.Menu = mainMenu1;
            this.Name = "FormViewEAN128Information";
            this.Text = "FormViewEAN128Information";
            this.Load += new System.EventHandler(this.FormViewEAN128Information_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxEAN128;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGrid dataGridView;
        private System.Windows.Forms.DataGrid dataGridViewNwAndUnits;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem RePrintEAN128;
    }
}