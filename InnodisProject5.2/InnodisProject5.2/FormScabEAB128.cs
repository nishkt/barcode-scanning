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
    public partial class FormScabEAB128 : Form
    {
        OracleConnection myConn = new OracleConnection();

        DataTable clDtGriData = null;

        //call the object that disables the windows start button
        InnodisProject5._2.hwndutils _hwndutils = new InnodisProject5._2.hwndutils();
        private bool _bInitializing = true;

        class Pallet
        {
            public int ean128;
            public int quantity;
            public string netweight;
            public string clientname;
            public string datescanned;
            public int ean128id2;
            public string expdat;
            public string FroChill;
        }

        public FormScabEAB128()
        {
            InitializeComponent();
            myConn.ConnectionString = "User Id=xxean;Password=ean123;Server=innebnidm02.innodisgroup.com;Port = 1541;Sid = prd1;";

            //disable the windows start button. when set to true, the button will be disabled
            this._hwndutils.StartButtonDisabled = _bInitializing;
        }

        //function to save the information for the EAN128 scanned on the device
        private Pallet SaveEan128ScanInfo()
        {
            Pallet pallet = new Pallet();

            myConn.Open();

            try
            {
                string q = "select netweight, cussur, ean128id, scndat, ean128id2, expdat, category from xxean.innodis_test where EAN128ID = :ean128id";

                using (OracleCommand cmd = new OracleCommand())
                {
                    cmd.Connection = myConn;
                    cmd.CommandText = q;

                    cmd.Parameters.Add(new OracleParameter("ean128id", OracleDbType.Number));

                    cmd.Parameters[0].Value = Convert.ToInt32(textBoxEAN128.Text.Substring(36, 5));

                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        int rows = 0;
                        decimal nweight = 0;

                        while (reader.Read())
                        {
                            rows++;
                            nweight += System.Convert.ToDecimal(reader[0]);
                            pallet.clientname = reader[1].ToString();
                            pallet.ean128 = Convert.ToInt32(reader[2]);
                            pallet.datescanned = reader[3].ToString();
                            if (reader[4] == DBNull.Value)
                            {
                                pallet.ean128id2 = 0;
                            }
                            else
                            {
                                pallet.ean128id2 = Convert.ToInt32(reader[4]);
                            }

                            pallet.expdat = reader[5].ToString();
                            pallet.FroChill = reader[6].ToString();
                        }

                        pallet.netweight = nweight.ToString();
                        pallet.quantity = rows;
                    }

                    return pallet;
                }
                myConn.Close();
            }
            catch (OracleException ex)
            {
                if (myConn.State == ConnectionState.Open)
                {
                    myConn.Close();
                }

                MessageBox.Show("EAN 128 not scanned succesfully!");

                return pallet;
            }
            
        }

        private void viewScannedInfo(Pallet p)
        {
            DataRow dr = clDtGriData.NewRow();

            //check if datagrid is empty
            if (clDtGriData.Rows.Count == 0)
            {
                dr["EAN128ID"] = p.ean128;
                dr["Quantity"] = p.quantity;
                dr["NetWeight"] = p.netweight;
                dr["Client"] = p.clientname;
                dr["DateScanned"] = p.datescanned;
                dr["ExpiryDate"] = p.expdat;
                dr["Category"] = p.FroChill;

                clDtGriData.Rows.Add(dr);
                clDtGriData.AcceptChanges();

                dataGridView.DataSource = clDtGriData;
            }
            //if not empty, then check if the barcode scanned has the same or different client as the previously scanned client
            else
            {
                string cname = clDtGriData.Rows[0]["Client"].ToString();
                
                if (cname == p.clientname)
                {
                    dr["EAN128ID"] = p.ean128;
                    dr["Quantity"] = p.quantity;
                    dr["NetWeight"] = p.netweight;
                    dr["Client"] = p.clientname;
                    dr["DateScanned"] = p.datescanned;
                    dr["ExpiryDate"] = p.expdat;
                    dr["Category"] = p.FroChill;

                    clDtGriData.Rows.Add(dr);
                    clDtGriData.AcceptChanges();

                    dataGridView.DataSource = clDtGriData;
                }
                else
                {
                    MessageBox.Show("The client for this item does not match. Please scan an item with matching clients");
                }
            }
        }

        private int calcEAN128ID2()
        {
            int ean128id2=0;
            myConn.Open();
            try
            {
                string w = "select ean128id2 from xxean.innodis_test where ean128id2 in (select max(ean128id2) from xxean.innodis_test)";
                OracleCommand cmd2 = new OracleCommand(w, myConn);

                ean128id2 = Convert.ToInt32(cmd2.ExecuteScalar());
                ean128id2 += 1;

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

            return ean128id2;
        }

        private void insertEAN128ID2table(int ean128id2)
        {
            myConn.Open();

            DataTable temptable = new DataTable();
            temptable = clDtGriData.DefaultView.ToTable(true, "EAN128ID");//will find distinct values of the datatable

            foreach (DataRow dr in temptable.Rows)
            {
                try
                {
                    string q = "update xxean.innodis_test set ean128id2 = '" + ean128id2 + "' where ean128id = '" + Convert.ToInt32(dr["EAN128ID"]) + "'";

                    using (OracleCommand cmd = new OracleCommand())
                    {
                        cmd.Connection = myConn;
                        cmd.CommandText = q;

                        //cmd.Parameters.Add(new OracleParameter("ean128id2", OracleDbType.Number));
                        //cmd.Parameters.Add(new OracleParameter("ean128id", OracleDbType.Number));

                        //cmd.Parameters[0].Value = ean128id2;
                        //cmd.Parameters[1].Value = Convert.ToInt32(dr["EAN128ID"]);

                        cmd.ExecuteNonQuery();

                        foreach (DataRow datrow in clDtGriData.Rows)
                        {
                            datrow["EAN128id2"] = ean128id2;
                        }
                    }
                }
                catch (OracleException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void SendPrinterData(string value)
        {
            byte[] data = Encoding.ASCII.GetBytes(value);
            string printerLoc = InnodisLogin.printerIP;//retrieves IP address of printer that was chosen on login

            try
            {
                if (clDtGriData.Rows.Count != 0)
                {
                    using (TcpClient client = new TcpClient(printerLoc, 9100))
                    {
                        NetworkStream stream = client.GetStream();
                        try
                        {
                            stream.Write(data, 0, data.Length);
                            MessageBox.Show("Pallet Barcode Printed succesfully!");
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
            int ean128id2 = 0;
            int prodquant = 0;
            decimal netweight = 0;
            string clientName = "";
            string cat = clDtGriData.Rows[0]["Category"].ToString();
            foreach (DataRow dr in clDtGriData.Rows)
            {
                prodquant += Convert.ToInt32(dr["Quantity"]);
                clientName = dr["Client"].ToString();
                ean128id2 = Convert.ToInt32(dr["EAN128id2"]);
                netweight += Convert.ToDecimal(dr["NetWeight"]);
            }

            DateTime proddate = Convert.ToDateTime(clDtGriData.Rows[0]["DateScanned"]);
            DateTime expdate = Convert.ToDateTime(clDtGriData.Rows[0]["ExpiryDate"]);

            decimal netweightb = Math.Round(netweight * 1000);
            int rows = clDtGriData.Rows.Count;//will give the amount of items for the ean128

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
            strSend += "PP 250," + y.ToString() + ":";
            strSend += "PT \"ProdDate: " + proddate.ToString("dd/MM/yyyy") + "\":";
            //strSend += "PT \"ProductionDate: \";DATE$:";
            strSend += "PP 550," + y.ToString() + ":";
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

            //01 - GTIN trade identification number
            //3103 - net weight with 3 decimal points
            //(10) batch number
            //(15) best before date
            //(400) customer's purchase order number
            //(15)120617(400)04GS112
        }

        private void textBoxEAN128_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                e.Handled = true;
                Pallet pallet = new Pallet();
                int count = 0;

                //need to put if statements to make sure user doesnt double scan and also so the user scanns ean 128 id and not ean13
                if (textBoxEAN128.Text.Length<16)
                {
                    MessageBox.Show("That item cannot be scanned on this page");
                }
                else if (textBoxEAN128.Text.Substring(0, 16) == "0198765432109876")
                {
                    if (clDtGriData.Rows.Count == 0)
                    {
                        pallet = SaveEan128ScanInfo();
                        viewScannedInfo(pallet);
                    }
                    else
                    {
                        //if the scanned barcode matches any value in the datagrid, then the count will be greaeter than 0
                        foreach (DataRow dr in clDtGriData.Rows)
                        {
                            //check to see if scanned ean128 matches one on the datagrid. if it matches, count will be greater than 0. if no match, count will be 0
                            string batch = textBoxEAN128.Text.Substring(36, 5);

                            if (batch.TrimStart('0') == dr[0].ToString())
                            {
                                count++;
                                break;
                            }
                        }

                        //count is 0 so there is no repeat in scanned crates or boxes
                        if (count == 0)
                        {
                            pallet = SaveEan128ScanInfo();
                            viewScannedInfo(pallet);
                        }
                        //count is greater than 0, a box/crate ean128 barcode has already been scanned before
                        else
                        {
                            MessageBox.Show("The item has already been scanned");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("That item cannot be scanned on this page");
                }
                
                textBoxEAN128.Text = "";
                textBoxEAN128.Focus();

            }
        }

        private void FormScabEAB128_Load(object sender, EventArgs e)
        {
            //this.Menu = null;//completely removes the main menu from the bottom

            clDtGriData = new DataTable();

            clDtGriData.Columns.Add("EAN128ID", typeof(int));
            clDtGriData.Columns.Add("Quantity", typeof(int));
            clDtGriData.Columns.Add("NetWeight", typeof(string));
            clDtGriData.Columns.Add("Client", typeof(string));
            clDtGriData.Columns.Add("DateScanned", typeof(string));
            clDtGriData.Columns.Add("ExpiryDate", typeof(string));
            clDtGriData.Columns.Add("Category", typeof(string));
            clDtGriData.Columns.Add("EAN128id2", typeof(int));

            clDtGriData.AcceptChanges();
        }

        private void buttonPalletPrint_Click(object sender, EventArgs e)
        {
            
                
         }

        private void menuItem1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PrintEAN128_Click(object sender, EventArgs e)
        {
            string message = "Are you sure you want to save these items into a pallet?";
            string caption = "";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, caption, buttons, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

            if (clDtGriData.Rows.Count > 0)
            {
                if (result == DialogResult.Yes)
                {
                    int i = calcEAN128ID2();
                    insertEAN128ID2table(i);
                    string send = insertPrintString();
                    SendPrinterData(send);

                    //MessageBox.Show("EAN128 has been printed!");
                    this.Close();
                }
            }
            else 
            {
                MessageBox.Show("Please scan some items before printing a new barcode!");
            }
        }
    }
}