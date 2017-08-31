namespace InnodisProject5._2
{
    partial class FormScanEAN13
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
            this.dataGridView = new System.Windows.Forms.DataGrid();
            this.textBoxEAN13 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxClient = new System.Windows.Forms.ComboBox();
            this.comboBoxDlvDates = new System.Windows.Forms.ComboBox();
            this.dataGridPreOrd = new System.Windows.Forms.DataGrid();
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
            // dataGridView
            // 
            this.dataGridView.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.dataGridView.Location = new System.Drawing.Point(6, 111);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.Size = new System.Drawing.Size(227, 83);
            this.dataGridView.TabIndex = 11;
            // 
            // textBoxEAN13
            // 
            this.textBoxEAN13.Font = new System.Drawing.Font("Tahoma", 5F, System.Drawing.FontStyle.Regular);
            this.textBoxEAN13.Location = new System.Drawing.Point(5, 90);
            this.textBoxEAN13.Name = "textBoxEAN13";
            this.textBoxEAN13.Size = new System.Drawing.Size(228, 15);
            this.textBoxEAN13.TabIndex = 10;
            this.textBoxEAN13.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxEAN13_KeyPress);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(5, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(189, 15);
            this.label2.Text = "Click on the box below and scan";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(9, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(208, 13);
            this.label1.Text = "Choose the Client and Delivery Date";
            // 
            // comboBoxClient
            // 
            this.comboBoxClient.Location = new System.Drawing.Point(5, 21);
            this.comboBoxClient.Name = "comboBoxClient";
            this.comboBoxClient.Size = new System.Drawing.Size(325, 22);
            this.comboBoxClient.TabIndex = 9;
            this.comboBoxClient.SelectedValueChanged += new System.EventHandler(this.comboBoxClient_SelectedValueChanged);
            // 
            // comboBoxDlvDates
            // 
            this.comboBoxDlvDates.Location = new System.Drawing.Point(5, 46);
            this.comboBoxDlvDates.Name = "comboBoxDlvDates";
            this.comboBoxDlvDates.Size = new System.Drawing.Size(228, 22);
            this.comboBoxDlvDates.TabIndex = 19;
            this.comboBoxDlvDates.SelectedValueChanged += new System.EventHandler(this.comboBoxDlvDates_SelectedValueChanged);
            // 
            // dataGridPreOrd
            // 
            this.dataGridPreOrd.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.dataGridPreOrd.Location = new System.Drawing.Point(6, 200);
            this.dataGridPreOrd.Name = "dataGridPreOrd";
            this.dataGridPreOrd.Size = new System.Drawing.Size(227, 52);
            this.dataGridPreOrd.TabIndex = 22;
            // 
            // FormScanEAN13
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.ControlBox = false;
            this.Controls.Add(this.dataGridPreOrd);
            this.Controls.Add(this.comboBoxDlvDates);
            this.Controls.Add(this.dataGridView);
            this.Controls.Add(this.textBoxEAN13);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoxClient);
            this.Menu = this.mainMenu1;
            this.Name = "FormScanEAN13";
            this.Text = "FormScanEAN13";
            this.Load += new System.EventHandler(this.FormScanEAN13_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGrid dataGridView;
        private System.Windows.Forms.TextBox textBoxEAN13;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxClient;
        private System.Windows.Forms.ComboBox comboBoxDlvDates;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.DataGrid dataGridPreOrd;
        private System.Windows.Forms.MenuItem PrintEAN128;
    }
}