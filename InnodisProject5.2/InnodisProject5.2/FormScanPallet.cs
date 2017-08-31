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
    public partial class FormScanPallet : Form
    {
        OracleConnection myConn = new OracleConnection();

        DataTable clDtGriData = null;

        //call the object that disables the windows start button
        InnodisProject5._2.hwndutils _hwndutils = new InnodisProject5._2.hwndutils();
        private bool _bInitializing = true;

        class Lorry
        {
            //public int ean128;
            public int prodquantity;
            public int boxquantity;
            public string netweight;
            public string clientname;
            public string datescanned;
            public int ean128id2;
            public int ean128id3;
        }

        public FormScanPallet()
        {
            InitializeComponent();
            myConn.ConnectionString = "User Id=xxean;Password=ean123;Server=innebnidm02.innodisgroup.com;Port = 1541;Sid = prd1;";

            //disable the windows start button. when set to true, the button will be disabled
            this._hwndutils.StartButtonDisabled = _bInitializing;

        }

        private Lorry SavePalletScanInfo()
        {
            Lorry lorry = new Lorry();

            myConn.Open();

            try
            {
                string q = "select netweight, cussur, scndat, ean128id2, ean128id3 from xxean.innodis_test where EAN128ID2 = :ean128id2";

                using (OracleCommand cmd = new OracleCommand())
                {
                    cmd.Connection = myConn;
                    cmd.CommandText = q;

                    cmd.Parameters.Add(new OracleParameter("ean128id2", OracleDbType.Number));

                    cmd.Parameters[0].Value = Convert.ToInt32(textBoxPallet.Text.Substring(36, 5));

                    OracleDataReader reader = cmd.ExecuteReader();

                    int rows = 0;
                    decimal nweight = 0;

                    while (reader.Read())
                    {
                        rows++;
                        nweight += System.Convert.ToDecimal(reader[0]);
                        lorry.clientname = reader[1].ToString();
                        //lorry.ean128 = Convert.ToInt32(reader[2]);
                        lorry.datescanned = reader[2].ToString();
                        lorry.ean128id2 = Convert.ToInt32(reader[3]);

                        if (reader[4] == DBNull.Value)
                        {

                            lorry.ean128id3 = 0;
                        }
                        else
                        {
                            lorry.ean128id3 = Convert.ToInt32(reader[4]);
                        }
                    }

                    //find number of boxes in the pallet
                    string w = "SELECT COUNT(distinct ean128id) FROM innodis_test where ean128id2 = :id2";

                    using (OracleCommand cmd2 = new OracleCommand())
                    {
                        cmd2.Connection = myConn;
                        cmd2.CommandText = w;

                        cmd2.Parameters.Add(new OracleParameter("id2", OracleDbType.Number));

                        cmd2.Parameters[0].Value = lorry.ean128id2;

                        var bq = cmd2.ExecuteScalar();

                        if (bq != null)
                        {
                            lorry.boxquantity = Convert.ToInt32(bq);
                        }
                        else
                        {
                            lorry.boxquantity = 0;
                        }
                    }

                    lorry.netweight = nweight.ToString();
                    lorry.prodquantity = rows;

                    return lorry;    
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

                return lorry;
            }
        }

        private void viewScannedInfo(Lorry l)
        {
            if (l.ean128id2 != 0)
            {
                DataRow dr = clDtGriData.NewRow();

                //dr["EAN128ID"] = l.ean128;
                dr["EAN128ID2"] = l.ean128id2;
                dr["Product Quantity"] = l.prodquantity;
                dr["Box Quantity"] = l.boxquantity;
                dr["NetWeight"] = l.netweight;
                dr["Client"] = l.clientname;
                dr["DateScanned"] = l.datescanned;

                clDtGriData.Rows.Add(dr);
                clDtGriData.AcceptChanges();

                dataGridPallet.DataSource = clDtGriData;
            }
            else
            {
                MessageBox.Show("The scanned pallet does not exist. Please scan again");
            }

        }

        private int calcEAN128ID3()
        {
            int ean128id3 = 0;
            myConn.Open();
            try
            {
                string q = "select ean128id3 from xxean.innodis_test where ean128id3 in (select max(ean128id3) from xxean.innodis_test)";
                OracleCommand cmd = new OracleCommand(q, myConn);

                ean128id3 = Convert.ToInt32(cmd.ExecuteScalar());
                ean128id3 += 1;                

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

            return ean128id3;
        }

        private void insertEAN128ID3table(int ean128id3)
        {
            myConn.Open();

            DataTable temptable = new DataTable();
            temptable = clDtGriData.DefaultView.ToTable(true, "EAN128ID2");//will find distinct values of the datatable

            foreach (DataRow dr in temptable.Rows)
            {
                try
                {
                    string q = "update xxean.innodis_test set ean128id3 = '" + ean128id3 + "' where ean128id2 = '" + Convert.ToInt32(dr["EAN128ID2"]) + "'";

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
                            datrow["EAN128ID3"] = ean128id3;
                        }
                    }
                }
                catch (OracleException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private string insertPrintString()
        {
            int ean128id3 = 0;
            int prodquant = 0;//number of individual products
            int boxquant = 0;//number of boxes/crates
            decimal netweight = 0;
            foreach (DataRow dr in clDtGriData.Rows)
            {
                prodquant += Convert.ToInt32(dr["Product Quantity"]);
                boxquant += Convert.ToInt32(dr["Box Quantity"]);
                ean128id3 = Convert.ToInt32(dr["EAN128ID3"]);
                netweight += Convert.ToDecimal(dr["NetWeight"]);
            }

            DateTime proddate = Convert.ToDateTime(clDtGriData.Rows[0]["DateScanned"]);

            TimeSpan duration = new TimeSpan(7, 0, 0, 0);
            DateTime expdate = Convert.ToDateTime(proddate).Add(duration);

            decimal netweightb = Math.Round(netweight * 1000);
            int rows = clDtGriData.Rows.Count;//will give the amount of pallets for the ean128

            return "";
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
                            MessageBox.Show("Lorry Barcode Printed succesfully!");
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

        private void textBoxPallet_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                e.Handled = true;

                Lorry lorry = new Lorry();

                int count = 0;

                if (textBoxPallet.Text.Length > 16 && textBoxPallet.Text.Substring(0, 16) == "0198765432101234")
                {
                    if (clDtGriData.Rows.Count == 0)
                    {
                        lorry = SavePalletScanInfo();//Correct EAN128 barcode scanned. do something
                        viewScannedInfo(lorry);
                    }
                    else
                    {
                        //if the scanned barcode matches any value in the datagrid, then the count will be greaeter than 0
                        foreach (DataRow dr in clDtGriData.Rows)
                        {
                            //check to see if scanned ean128 matches one on the datagrid. if it matches, count will be greater than 0. if no match, count will be 0
                            string batch = textBoxPallet.Text.Substring(36, 5);

                            if (batch.TrimStart('0') == dr[5].ToString())
                            {
                                count++;
                                break;
                            }
                        }

                        //count is 0 so there is no repeat in scanned crates or boxes
                        if (count == 0)
                        {
                            lorry = SavePalletScanInfo();//Correct EAN128 barcode scanned. do something
                            viewScannedInfo(lorry);
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

                textBoxPallet.Text = "";
                textBoxPallet.Focus();
            }
        }

        private void FormScanPallet_Load(object sender, EventArgs e)
        {
            clDtGriData = new DataTable();

            //clDtGriData.Columns.Add("EAN128ID", typeof(int));
            clDtGriData.Columns.Add("EAN128ID2", typeof(int));
            clDtGriData.Columns.Add("Product Quantity", typeof(int));
            clDtGriData.Columns.Add("Box Quantity", typeof(int));
            clDtGriData.Columns.Add("NetWeight", typeof(string));
            clDtGriData.Columns.Add("Client", typeof(string));
            clDtGriData.Columns.Add("DateScanned", typeof(string));
            clDtGriData.Columns.Add("EAN128ID3", typeof(int));

            clDtGriData.AcceptChanges();
        }

        private void btnLorryEAN128_Click(object sender, EventArgs e)
        {
            string message = "Are you sure you want to save these pallets into a Lorry?";
            string caption = "";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, caption, buttons, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

            if (clDtGriData.Rows.Count > 0)
            {
                if (result == DialogResult.Yes)
                {
                    //datagridview is not empty. User clicked yes to messagebox. do something

                    int i = calcEAN128ID3();
                    insertEAN128ID3table(i);//will add the new EAN128id3 (lorry ean) to the database table (innodis_test)                    
                    string send = insertPrintString();
                    //SendPrinterData(send);

                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Please scan some items before printing a new barcode!");
            }
        }

        //go back to formchoosemethod
        private void menuItem1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}