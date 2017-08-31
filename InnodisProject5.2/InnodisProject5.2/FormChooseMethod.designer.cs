namespace InnodisProject5._2
{
    partial class FormChooseMethod
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
            this.btnLogout = new System.Windows.Forms.Button();
            this.btnScanEAN13 = new System.Windows.Forms.Button();
            this.btnScanEAN128 = new System.Windows.Forms.Button();
            this.btnPallet = new System.Windows.Forms.Button();
            this.btnLorry = new System.Windows.Forms.Button();
            this.btnScanFF = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnLogout
            // 
            this.btnLogout.Location = new System.Drawing.Point(67, 213);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(102, 52);
            this.btnLogout.TabIndex = 0;
            this.btnLogout.Text = "Logout";
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // btnScanEAN13
            // 
            this.btnScanEAN13.Location = new System.Drawing.Point(3, 3);
            this.btnScanEAN13.Name = "btnScanEAN13";
            this.btnScanEAN13.Size = new System.Drawing.Size(234, 36);
            this.btnScanEAN13.TabIndex = 1;
            this.btnScanEAN13.Text = "Scan CH Items and Print EAN128";
            this.btnScanEAN13.Click += new System.EventHandler(this.btnScanEAN13_Click);
            // 
            // btnScanEAN128
            // 
            this.btnScanEAN128.Location = new System.Drawing.Point(3, 171);
            this.btnScanEAN128.Name = "btnScanEAN128";
            this.btnScanEAN128.Size = new System.Drawing.Size(234, 36);
            this.btnScanEAN128.TabIndex = 2;
            this.btnScanEAN128.Text = "Scan EAN128 and View Details";
            this.btnScanEAN128.Click += new System.EventHandler(this.btnScanEAN128_Click);
            // 
            // btnPallet
            // 
            this.btnPallet.Location = new System.Drawing.Point(3, 87);
            this.btnPallet.Name = "btnPallet";
            this.btnPallet.Size = new System.Drawing.Size(234, 36);
            this.btnPallet.TabIndex = 3;
            this.btnPallet.Text = "Print Pallet Barcode";
            this.btnPallet.Click += new System.EventHandler(this.btnPallet_Click);
            // 
            // btnLorry
            // 
            this.btnLorry.Location = new System.Drawing.Point(3, 129);
            this.btnLorry.Name = "btnLorry";
            this.btnLorry.Size = new System.Drawing.Size(234, 36);
            this.btnLorry.TabIndex = 4;
            this.btnLorry.Text = "Print Lorry Barcode";
            this.btnLorry.Click += new System.EventHandler(this.btnLorry_Click);
            // 
            // btnScanFF
            // 
            this.btnScanFF.Location = new System.Drawing.Point(3, 45);
            this.btnScanFF.Name = "btnScanFF";
            this.btnScanFF.Size = new System.Drawing.Size(234, 36);
            this.btnScanFF.TabIndex = 5;
            this.btnScanFF.Text = "Scan FF Items and Print EAN128";
            this.btnScanFF.Click += new System.EventHandler(this.btnScanFF_Click);
            // 
            // FormChooseMethod
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.ControlBox = false;
            this.Controls.Add(this.btnScanFF);
            this.Controls.Add(this.btnLorry);
            this.Controls.Add(this.btnPallet);
            this.Controls.Add(this.btnScanEAN128);
            this.Controls.Add(this.btnScanEAN13);
            this.Controls.Add(this.btnLogout);
            this.Menu = this.mainMenu1;
            this.Name = "FormChooseMethod";
            this.Text = "FormChooseMethod";
            this.Load += new System.EventHandler(this.FormChooseMethod_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Button btnScanEAN13;
        private System.Windows.Forms.Button btnScanEAN128;
        private System.Windows.Forms.Button btnPallet;
        private System.Windows.Forms.Button btnLorry;
        private System.Windows.Forms.Button btnScanFF;
    }
}