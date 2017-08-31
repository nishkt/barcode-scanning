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
    public partial class FormViewEAN128Information : Form
    {
        OracleConnection myConn = new OracleConnection();
        DataTable dbdataset = new DataTable();//=null
        DataTable dbdataset2 = new DataTable();

        //call the object that disables the windows start button
        InnodisProject5._2.hwndutils _hwndutils = new InnodisProject5._2.hwndutils();
        private bool _bInitializing = true;

        public class Stage
        {
            public string Name { get; set; }
            public string Value { get; set; }
        }

        public FormViewEAN128Information()
        {
            InitializeComponent();
            myConn.ConnectionString = "User Id=xxean;Password=ean123;Server=innebnidm02.innodisgroup.com;Port = 1541;Sid = prd1;";

            //disable the windows start button. when set to true, the button will be disabled
            this._hwndutils.StartButtonDisabled = _bInitializing;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                if (dbdataset != null && dbdataset2 != null)
                {
                    dbdataset.Rows.Clear();
                    dbdataset2.Rows.Clear();
                }
                
                dbdataset = new DataTable();
                //dbdataset2 = new DataTable();
                e.Handled = true;
                viewEAN128Information();
                textBoxEAN128.Text = "";
                textBoxEAN128.Focus();
                
            }
        }

        private void FormViewEAN128Information_Load(object sender, EventArgs e)
        {
            //this.Menu = null;//completely removes the main menu from the bottom
            
        }

        private void viewEAN128Information()
        {
            try
            {
                myConn.Open();

                //if its container/box/etc
                if (textBoxEAN128.Text.Length < 16)
                {
                    MessageBox.Show("That item cannot be scanned on this page");
                }
                else if (textBoxEAN128.Text.Substring(0, 16) == "0198765432109876")
                {
                    int ean128id = Convert.ToInt32(textBoxEAN128.Text.Substring(36, 5));
                    string q = "select * from xxean.innodis_test where ean128id = :ean128id";

                    using (OracleCommand cmd = new OracleCommand())
                    {
                        cmd.Connection = myConn;
                        cmd.CommandText = q;

                        cmd.Parameters.Add(new OracleParameter("ean128id", OracleDbType.Number));

                        cmd.Parameters[0].Value = Convert.ToInt32(textBoxEAN128.Text.Substring(36, 5));

                        OracleDataAdapter sda = new OracleDataAdapter();
                        sda.SelectCommand = cmd;
                        sda.Fill(dbdataset);
                        BindingSource bSource = new BindingSource();

                        bSource.DataSource = dbdataset;
                        dataGridView.DataSource = bSource;
                        sda.Update(dbdataset);

                        int rows = dbdataset.Rows.Count;


                        //**********************************ADDING DATA TO THE SECOND DATAGRIDVIEW********************************************
                        dbdataset2 = new DataTable();

                        dbdataset2.Columns.Add("EAN128ID", typeof(int));
                        dbdataset2.Columns.Add("NetWeight", typeof(String));
                        dbdataset2.Columns.Add("Units", typeof(int));
                        dbdataset2.Columns.Add("Layer", typeof(String));

                        dbdataset2.AcceptChanges();

                        string layer = "Primary";
                        decimal netweight = 0;
                        int i = 0;//this will be used to display the ean128id in the second datagridview
                        foreach (DataRow dr in dbdataset.Rows)
                        {
                            i = Convert.ToInt32(dr["EAN128ID"]);
                            netweight += Convert.ToDecimal(dr["netweight"]);
                        }

                        DataRow datrow = dbdataset2.NewRow();
                        datrow["EAN128ID"] = i;
                        datrow["NetWeight"] = netweight.ToString();
                        datrow["Units"] = rows;
                        datrow["Layer"] = layer;

                        dbdataset2.Rows.Add(datrow);
                        dbdataset2.AcceptChanges();

                        dataGridViewNwAndUnits.DataSource = dbdataset2;
                        //**********************************ADDING DATA TO THE SECOND DATAGRIDVIEW********************************************

                        if (rows == 0)
                        {
                            MessageBox.Show("No such EAN128 barcode exists in the system");
                        }
                        else
                        {
                            MessageBox.Show("There are a total of " + rows + " amount of products under this barcode");
                        }
                        //MessageBox.Show("There are a total of " + rows + " amount of products under this barcode"
                    }
                }
                //if scanned Item is pallet
                else if (textBoxEAN128.Text.Substring(0, 16) == "0198765432101234")
                {
                    int ean128id2 = Convert.ToInt32(textBoxEAN128.Text.Substring(36, 5));
                    string q = "select * from xxean.innodis_test where ean128id2 = :ean128id2";

                    using (OracleCommand cmd = new OracleCommand())
                    {
                        cmd.Connection = myConn;
                        cmd.CommandText = q;

                        cmd.Parameters.Add(new OracleParameter("ean128id2", OracleDbType.Number));

                        cmd.Parameters[0].Value = ean128id2;

                        OracleDataAdapter sda = new OracleDataAdapter();
                        sda.SelectCommand = cmd;
                        
                        sda.Fill(dbdataset);
                        BindingSource bSource = new BindingSource();

                        bSource.DataSource = dbdataset;
                        dataGridView.DataSource = bSource;
                        sda.Update(dbdataset);

                        int rows = dbdataset.Rows.Count;

                        //**********************************ADDING DATA TO THE SECOND DATAGRIDVIEW********************************************
                        dbdataset2 = new DataTable();

                        dbdataset2.Columns.Add("EAN128ID2", typeof(int));
                        dbdataset2.Columns.Add("NetWeight", typeof(String));
                        dbdataset2.Columns.Add("Product Units", typeof(int));
                        dbdataset2.Columns.Add("Container Units", typeof(int));
                        dbdataset2.Columns.Add("Layer", typeof(String));

                        dbdataset2.AcceptChanges();

                        string layer = "Secondary";
                        decimal netweight = 0;
                        int contquan = 0;
                        int i = 0;//this will be used to display the ean128id in the second datagridview
                        foreach (DataRow dr in dbdataset.Rows)
                        {
                            i = Convert.ToInt32(dr["EAN128ID2"]);
                            netweight += Convert.ToDecimal(dr["netweight"]);
                        }

                        //finding the number of containers on the pallet
                        DataTable temptable = new DataTable();
                        temptable = dbdataset.DefaultView.ToTable(true, "EAN128ID");

                        if (dbdataset.Rows.Count != 0)
                        {
                            foreach (DataRow dr in temptable.Rows)
                            {
                                contquan += 1;
                            }
                        }

                        DataRow datrow = dbdataset2.NewRow();
                        datrow["EAN128ID2"] = i;
                        datrow["NetWeight"] = netweight.ToString();
                        datrow["Product Units"] = rows;
                        datrow["Container Units"] = contquan;
                        datrow["Layer"] = layer;

                        dbdataset2.Rows.Add(datrow);
                        dbdataset2.AcceptChanges();

                        dataGridViewNwAndUnits.DataSource = dbdataset2;
                        //**********************************ADDING DATA TO THE SECOND DATAGRIDVIEW********************************************

                        if (rows == 0)
                        {
                            MessageBox.Show("No such EAN128 barcode exists in the system");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please Enter a valid EAN128 barcode");
                    myConn.Close();
                    this.Close();
                }
                myConn.Close();
            }
            catch (OracleException ex)
            {
                if (myConn.State == ConnectionState.Open)
                {
                    myConn.Close();
                }

                MessageBox.Show(ex.Message);
            }
        }

        //The SendPrinterData function allows communication with the printer through the software. The string value will
        //the string that will pass through the printer to print the neccessary barcode
        private void SendPrinterData(string value)
        {
            byte[] data = Encoding.ASCII.GetBytes(value);
            string printerLoc = InnodisLogin.printerIP;//retrieves IP address of printer that was chosen on login

            try
            {
                if (dbdataset.Rows.Count != 0)
                {
                    using (TcpClient client = new TcpClient(printerLoc, 9100))
                    {
                        NetworkStream stream = client.GetStream();
                        try
                        {
                            stream.Write(data, 0, data.Length);
                            MessageBox.Show("EAN 128 Printed succesfully!");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }
                        finally
                        {
                            stream.Close();
                            client.Close();
                        }
                    }
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.ToString());
            }
        }


        private string insertPrintString()
        {
            
            string clientName = dbdataset.Rows[0]["CUSSUR"].ToString();
            int ean128id = Convert.ToInt32(dbdataset.Rows[0]["EAN128ID"]);
            decimal netweight = Convert.ToDecimal(dbdataset2.Rows[0]["netweight"]);
            string cat = dbdataset.Rows[0]["CATEGORY"].ToString();
            DateTime proddate = Convert.ToDateTime(dbdataset.Rows[0]["SCNDAT"]);
            DateTime expdate = Convert.ToDateTime(dbdataset.Rows[0]["EXPDAT"]);
            string itemDescription = dbdataset.Rows[0]["STKDES"].ToString();
           
            decimal netweightb = Math.Round(netweight * 1000);
            int rows = dbdataset.Rows.Count;//will give the amount of items for the ean128

            //determines the x,y position of all the components
            int x = 750;
            int y = 85;
            int fontsize = 6;

            String strSend = "CLIP ON:";
            strSend += "AN 6:";
            strSend += "PP " + x.ToString() + "," + y.ToString() + ":";
            strSend += "FT \"Swiss 721 BT\"," + fontsize.ToString() + ":";
            strSend += "PT \"(01)98765432109876(3103)" + netweightb.ToString("000000") + "(15)" + expdate.ToString("yyMMdd") + "(10)" + ean128id.ToString("00000") + "\":";
            strSend += "BT \"EAN128\":";
            y += 105;
            x -= 50;
            strSend += "PP " + x.ToString() + "," + y.ToString() + ":";
            strSend += "BH 180:";
            strSend += "BM 1:";
            strSend += "PB \"01987654321098763103" + netweightb.ToString("000000") + "15" + expdate.ToString("yyMMdd") + "10" + ean128id.ToString("00000") + "\":";
            y += 110;
            x -= 300;
            strSend += "AN 5:";
            strSend += "PP " + x.ToString() + "," + y.ToString() + ":";
            strSend += "PL 700,5:";
            fontsize += 6;
            strSend += "FT \"Swiss 721 Bold Condensed BT\"," + fontsize.ToString() + ":";
            y += 10;
            strSend += "PP " + x.ToString() + "," + y.ToString() + ":";
            strSend += "PX 100, 700, 0, \"Client Name: " + clientName + "\":";
            //strSend += "PT \"Client Name: " + clientName + "\":";
            y += 100;
            strSend += "PP 230," + y.ToString() + ":";
            strSend += "PT \"ProdDate: " + proddate.ToString("dd/MM/yyyy") + "\":";
            //strSend += "PT \"ProductionDate: \";DATE$:";
            strSend += "PP 570," + y.ToString() + ":";
            strSend += "PT \"ExpDate: " + expdate.ToString("dd/MM/yyyy") + "\":";
            //strSend += "PT \"ExpiryDate: \";DATEADD$ (7):";
            y += 50;
            strSend += "PP " + x.ToString() + "," + y.ToString() + ":";
            strSend += "PT \"LOT#: " + ean128id.ToString("00000") + "\":";
            y += 50;
            strSend += "PP " + x.ToString() + "," + y.ToString() + ":";
            strSend += "PT \"Quantity: " + rows.ToString() + "\":";
            y += 50;
            strSend += "PP " + x.ToString() + "," + y.ToString() + ":";
            strSend += "PT \"NetWeight: " + netweight.ToString() + " KG\":";
            y += 35;
            strSend += "PP " + x.ToString() + "," + y.ToString() + ":";
            strSend += "PL 700,5:";
            y += 15;
            strSend += "PP " + x.ToString() + "," + y.ToString() + ":";
            strSend += "PX 100, 700, 0, \"" + itemDescription + "\":";
            //strSend += "PT \"INNODIS BARCODE SYSTEM\":";
            strSend += "PF\r\n";
            return strSend;

            //01 - GTIN trade identification number
            //3103 - net weight with 3 decimal points
            //(10) batch number
            //(15) best before date
            //(400) customer's purchase order number
            //(15)120617(400)04GS112
        }

        private string insertPrintStringSecondary()
        {
            int ean128id2 = 0;
            int prodquant = 0;
            decimal netweight = 0;
            string clientName = "";

            prodquant = Convert.ToInt32(dbdataset2.Rows[0]["Product Units"]);
            ean128id2 = Convert.ToInt32(dbdataset2.Rows[0]["EAN128ID2"]);
            netweight = Convert.ToDecimal(dbdataset2.Rows[0]["NetWeight"]);
            DateTime proddate = Convert.ToDateTime(dbdataset.Rows[0]["SCNDAT"]);
            DateTime expdate = Convert.ToDateTime(dbdataset.Rows[0]["EXPDAT"]);
            
            clientName = dbdataset.Rows[0]["CUSSUR"].ToString();


            decimal netweightb = Math.Round(netweight * 1000);
            int rows = Convert.ToInt32(dbdataset2.Rows[0]["Container Units"]);//will give the amount of items for the ean128

            //determines the x,y position of all the components
            int x = 750;
            int y = 85;
            int fontsize = 6;

            String strSend = "CLIP ON:";
            strSend += "AN 6:";
            strSend += "PP " + x.ToString() + "," + y.ToString() + ":";
            strSend += "FT \"Swiss 721 BT\"," + fontsize.ToString() + ":";
            strSend += "PT \"(01)98765432101234(3103)" + netweightb.ToString("000000") + "(15)" + expdate.ToString("yyMMdd") + "(10)" + ean128id2.ToString("00000") + "\":";
            strSend += "BT \"EAN128\":";
            y += 105;
            x -= 50;
            strSend += "PP " + x.ToString() + "," + y.ToString() + ":";
            strSend += "BH 180:";
            strSend += "BM 1:";
            strSend += "PB \"01987654321012343103" + netweightb.ToString("000000") + "15" + expdate.ToString("yyMMdd") + "10" + ean128id2.ToString("00000") + "\":";
            y += 110;
            x -= 300;
            strSend += "AN 5:";
            strSend += "PP " + x.ToString() + "," + y.ToString() + ":";
            strSend += "PL 700,5:";
            fontsize += 6;
            strSend += "FT \"Swiss 721 Bold Condensed BT\"," + fontsize.ToString() + ":";
            y += 10;
            strSend += "PP " + x.ToString() + "," + y.ToString() + ":";
            strSend += "PX 100, 700, 0, \"Client Name: " + clientName + "\":";
            //strSend += "PT \"Client Name: " + clientName + "\":";
            y += 100;
            strSend += "PP 230," + y.ToString() + ":";
            strSend += "PT \"ProdDate: " + proddate.ToString("dd/MM/yyyy") + "\":";
            //strSend += "PT \"ProductionDate: \";DATE$:";
            strSend += "PP 570," + y.ToString() + ":";
            strSend += "PT \"ExpDate: " + expdate.ToString("dd/MM/yyyy") + "\":";
            //strSend += "PT \"ExpiryDate: \";DATEADD$ (7):";
            y += 50;
            strSend += "PP " + x.ToString() + "," + y.ToString() + ":";
            strSend += "PT \"Pallet#: " + ean128id2.ToString("00000") + "\":";
            y += 50;
            strSend += "PP 250," + y.ToString() + ":";
            strSend += "PT \"Cont. Quantity: " + rows.ToString() + "\":";
            strSend += "PP 550," + y.ToString() + ":";
            strSend += "PT \"Prod. Quantity: " + prodquant.ToString() + "\":";
            y += 50;
            strSend += "PP " + x.ToString() + "," + y.ToString() + ":";
            strSend += "PT \"NetWeight: " + netweight.ToString() + " KG\":";
            y += 35;
            strSend += "PP " + x.ToString() + "," + y.ToString() + ":";
            strSend += "PL 700,5:";
            y += 50;
            strSend += "PP " + x.ToString() + "," + y.ToString() + ":";
            strSend += "PT \"INNODIS BARCODE SYSTEM\":";
            strSend += "PF\r\n";
            return strSend;
        }

        private void btnMainMenu_Click(object sender, EventArgs e)
        {

        }

        private void buttonReprint_Click(object sender, EventArgs e)
        {

        }

        private void menuItem1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RePrintEAN128_Click(object sender, EventArgs e)
        {
            string message = "Are you sure you want to save these items into a pallet or EAN128?";
            string caption = "";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, caption, buttons, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

            if (result == DialogResult.Yes)
            {
                try
                {
                    if (dbdataset2.Rows.Count != 0)
                    {
                        string strSend = "";
                        string layer = "";

                        layer = dbdataset2.Rows[0]["Layer"].ToString();

                        if (layer == "Primary")
                        {
                            strSend = insertPrintString();
                        }
                        else if (layer == "Secondary")
                        {
                            strSend = insertPrintStringSecondary();
                        }

                        SendPrinterData(strSend);
                        //MessageBox.Show("EAN 128 Printed succesfully!");
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Please scan an EAN128 Barcode before Re-Printing!");
                    }
                }
                catch (OracleException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

        }
    }
}