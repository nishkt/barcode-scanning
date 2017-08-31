using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace InnodisProject5._2
{
    public partial class FormChooseMethod : Form
    {
        //call the object that disables the windows start button
        InnodisProject5._2.hwndutils _hwndutils = new InnodisProject5._2.hwndutils();
        private bool _bInitializing = true;

        public FormChooseMethod()
        {
            InitializeComponent();

            //disable the windows start button. when set to true, the button will be disabled
            this._hwndutils.StartButtonDisabled = _bInitializing;
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnScanEAN13_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            FormScanEAN13 myformscanean13 = new FormScanEAN13();
            myformscanean13.ShowDialog();
            //
        }

        private void btnScanEAN128_Click(object sender, EventArgs e)
        {
            FormViewEAN128Information myformviewean128information = new FormViewEAN128Information();
            myformviewean128information.ShowDialog();
        }

        private void FormChooseMethod_Load(object sender, EventArgs e)
        {
        }

        private void btnPallet_Click(object sender, EventArgs e)
        {
            FormScabEAB128 myformscabeab128 = new FormScabEAB128();
            myformscabeab128.ShowDialog();
        }

        private void btnLorry_Click(object sender, EventArgs e)
        {
            FormScanPallet myformscanpallet = new FormScanPallet();
            myformscanpallet.ShowDialog();
        }

        private void btnScanFF_Click(object sender, EventArgs e)
        {
            FormScanFF myformscanff = new FormScanFF();
            myformscanff.ShowDialog();
        }

    }
}