using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Devart.Data.Oracle;
using System.Net;
using System.Net.Sockets;

namespace InnodisProject5._2
{
    public partial class FormScanFF : Form
    {
        OracleConnection myConn = new OracleConnection();

        //call the object that disables the windows start button
        InnodisProject5._2.hwndutils _hwndutils = new InnodisProject5._2.hwndutils();
        private bool _bInitializing = true;

        class coldroom
        {
            public string code;
            public string name;
        }

        coldroom MFD = new coldroom();
        coldroom BClimat = new coldroom();

        public FormScanFF()
        {
            InitializeComponent();
            myConn.ConnectionString = "User Id=xxean;Password=ean123;Server=innebnidm02.innodisgroup.com;Port = 1541;Sid = prd1;";

            //disable the windows start button. when set to true, the button will be disabled
            this._hwndutils.StartButtonDisabled = _bInitializing;
        }

        private void FormScanFF_Load(object sender, EventArgs e)
        {
            loadComboBoxColdRoom();

            MFD.code = "INN002";
            MFD.name = "MFD COLDROOM";
            BClimat.code = "XXXXXX";
            BClimat.name = "B.CLIMAT COLDROOM";
        }

        private void loadComboBoxColdRoom()
        {
            DataTable ColdRoomData = new DataTable();
            ColdRoomData.Columns.Add("code", typeof(string));
            ColdRoomData.Columns.Add("name", typeof(string));
            ColdRoomData.AcceptChanges();

            List<coldroom> mylist = new List<coldroom>();

            mylist.Add(MFD);
            mylist.Add(BClimat);

            comboBoxColdRoom.DataSource = new BindingSource(ColdRoomData, null);
            //foreach(var client in mylist)
            //{
            //    DataRow dr = ColdRoomData.NewRow();
                
            //    dr["code"] = client.code;
            //    dr["name"] = client.name;

            //    ColdRoomData.Rows.Add(dr);
            //}
            
            

            comboBoxColdRoom.DisplayMember = "name";
            comboBoxColdRoom.ValueMember = "code";
        }

        private coldroom saveColdRoomInformation(string coldroomcode)
        {
            coldroom client = new coldroom();

            client.code = coldroomcode;
            if(coldroomcode == MFD.code)
            {
                client.name = MFD.name;
            }
            else if (coldroomcode == BClimat.code)
            {
                client.name = BClimat.name;
            }

            return client;
            
        }

        private void menuItem1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PrintEAN128_Click(object sender, EventArgs e)
        {

        }

        private void textBoxScanFrozen_KeyPress(object sender, KeyPressEventArgs e)
        {
            //when enter key is pressed //when item is scanned
            if (e.KeyChar == (char)Keys.Return)
            {
                e.Handled = true;
                coldroom client = new coldroom();
                client = saveColdRoomInformation(comboBoxColdRoom.SelectedValue.ToString());

            }
        }
    }
}