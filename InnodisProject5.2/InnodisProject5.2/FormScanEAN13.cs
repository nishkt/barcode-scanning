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
    public partial class FormScanEAN13 : Form
    {
        OracleConnection myConn = new OracleConnection();

        //call the object that disables the windows start button
        InnodisProject5._2.hwndutils _hwndutils = new InnodisProject5._2.hwndutils();
        private bool _bInitializing = true;

        //creating client class to be able to return multiple values on client information
        class Client
        {
            public string name;
            public string nadcod;
            public string dlvdat;
            public string stcodt;
            public string chacod;
            public string slscod; 
        }

        //Similarily with client class, creating product class (EAN13 barcodes) to be able to return multiple values 
        //on product information
        class Product
        {
            public string barcode;
            public string stkcod;
            public string stkdes;
            public string netweight;
            public int EAN128ID;
            public string cpycod;
            public string thisDay;
            public string FroChill;
            public string expdat;
        }

        //calls out oracle database connection information
        public FormScanEAN13()
        {
            InitializeComponent();
            myConn.ConnectionString = "User Id=xxean;Password=ean123;Server=innebnidm02.innodisgroup.com;Port = 1541;Sid = prd1;";
            
            //disable the windows start button. when set to true, the button will be disabled
            this._hwndutils.StartButtonDisabled = _bInitializing;
        }

        //initialize datatable that will display the information of the product that will be scanned
        public DataTable clDtGriData = null;

        //initialize datatable that will display the preorder information based on the client and delivery date chosen by the user
        public DataTable PreOrd = new DataTable();

        private void FormScanEAN13_Load(object sender, EventArgs e)
        {
            //starts loading cursor and shows on middle of screen
            Cursor.Current = Cursors.WaitCursor;

            loadComboBoxClient();

            //ends the loading cursor
            Cursor.Current = Cursors.Default;

            clDtGriData = new DataTable();

            clDtGriData.Columns.Add("#", typeof(int));
            clDtGriData.Columns.Add("barcode", typeof(String));
            clDtGriData.Columns.Add("name", typeof(String));//client name
            clDtGriData.Columns.Add("nadcod", typeof(String));//client code
            clDtGriData.Columns.Add("cpycod", typeof(String));//location code(Beau Climat Innodis Code)
            clDtGriData.Columns.Add("dlvdat", typeof(String));//delivery date
            clDtGriData.Columns.Add("netweight", typeof(String));
            clDtGriData.Columns.Add("EAN128ID", typeof(int));
            clDtGriData.Columns.Add("stkcod", typeof(String));//product code
            clDtGriData.Columns.Add("stkdes", typeof(String));//product description
            clDtGriData.Columns.Add("DateScanned", typeof(String));
            clDtGriData.Columns.Add("Category", typeof(String));//frozen or chilled
            clDtGriData.Columns.Add("ExpiryDate", typeof(String));
            clDtGriData.Columns.Add("STCODT", typeof(String));
            clDtGriData.Columns.Add("CHACOD", typeof(String));
            clDtGriData.Columns.Add("SLSCOD", typeof(String));

            clDtGriData.AcceptChanges();
        }

        //loads the combobox with the names of the clients. gets called in the FormScanEAN13_Load
        //NEW and UPGRADED. USES DATATABLES TO VIEW COMBOBOX CLIENT INFORMATION
        private void loadComboBoxClient()
        {
            try
            {
                string q = "select distinct nadcod, ordctn from innodis_sa_preorh order by nadcod";//nadcod = client code, ordctn = client name
                myConn.Open();
                OracleCommand cmd = new OracleCommand(q, myConn);

                //create datatable of query results
                DataTable ClientData = new DataTable();
                ClientData.Columns.Add("nadcod", typeof(string));
                ClientData.Columns.Add("ordctn", typeof(string));
                ClientData.Columns.Add("ConcatenatedField", typeof(string), "nadcod + ' | ' +ordctn");//this line allows both the client code and client name to be displayed in one row of the combobox
                OracleDataAdapter sda = new OracleDataAdapter();
                sda.SelectCommand = cmd;
                sda.Fill(ClientData);
                BindingSource bSource = new BindingSource();

                bSource.DataSource = ClientData;
                comboBoxClient.DataSource = bSource;
                sda.Update(ClientData);

                comboBoxClient.DisplayMember = "ConcatenatedField";
                comboBoxClient.ValueMember = "nadcod";//this line lets the program know that the chosen row on the combobox by the user will give a value of the client code (not client name)
                myConn.Close();
            }
            catch(OracleException ex)
            {
                if (myConn.State == ConnectionState.Open)
                {
                    myConn.Close();
                }

                MessageBox.Show("There has been a problem in the network. Please try again");

                this.Close();
            }
            
        }

        private void viewPreOrderInfo()
        {
            try
            {
                if (comboBoxClient.SelectedValue != null)
                {
                    myConn.Open();

                    //the below query q gets the preorder transaction information for the selected client and dlvdate
                    //strcod = product code, allcod = product name, trdqtx = quantity ordered, dlvdat =  delivery date, nadcod = client code, trdvch trhvch = transaction code
                    string q = "select STRCOD, ALLCOD, TRDQTX from INNODIS_SA_PREORD, INNODIS_SA_PREORH where DLVDAT=TO_DATE(:dlvdat,'dd/MM/yyyy') and NADCOD = :clientcode and TRDVCH = TRHVCH and TRDQTY != '0'";
                    
                    using (OracleCommand cmd = new OracleCommand())
                    {
                        cmd.Connection = myConn;
                        cmd.CommandText = q;

                        //parametrizing query values
                        cmd.Parameters.Add(new OracleParameter("dlvdat", OracleDbType.VarChar));
                        cmd.Parameters.Add(new OracleParameter("clientcode", OracleDbType.VarChar));

                        cmd.Parameters[0].Value = comboBoxDlvDates.Text.ToString();//use value from the combobox that displays delivery dates
                        cmd.Parameters[1].Value = comboBoxClient.SelectedValue.ToString();//use value from the combobox that displays client information

                        //put query results into datatable and display on the bottom datagrid
                        OracleDataAdapter sda = new OracleDataAdapter();
                        sda.SelectCommand = cmd;
                        sda.Fill(PreOrd);
                        BindingSource bSource = new BindingSource();

                        bSource.DataSource = PreOrd;
                        dataGridPreOrd.DataSource = bSource;//view the query datatable information on the bottom datagrid
                        sda.Update(PreOrd);
                    }
                }
            }
            catch (OracleException ex)
            {
                if (myConn.State == ConnectionState.Open)
                {
                    myConn.Close();
                }
            }

        }

        //loads the combobox with the delivery dates. gets called in the comboBoxClient_SelectedValueChanged
        //USES DATA READER
        private void loadDeliveryDates()
        {
            comboBoxDlvDates.Items.Clear();//delivery dates combobox clear
            myConn.Open();

            //dlvdat = delivery date, nadcod = client code, 
            string q = "select TO_CHAR(dlvdat, 'dd/MM/yyyy') from innodis_sa_preorh where nadcod = '" + comboBoxClient.SelectedValue.ToString() + "' AND STATUS = '0'";

            OracleCommand cmd = new OracleCommand(q, myConn);
            OracleDataReader DR = cmd.ExecuteReader();

            //run reader to load the delivery dates into combobox
            while (DR.Read())
            {
                comboBoxDlvDates.Items.Add(DR[0]);
            }

            DR.Close();
            myConn.Close();
        }

        //takes the client that the user selected and saves into a class called client with the client's information
        private Client saveClientInfo(string clientcode)
        {
            //create new client class
            Client client = new Client();

            try
            {
                //ordctn = client name, stcodt = 
                string q = "select ordctn, stcodt, chacod, slscod FROM innodis_sa_preorh WHERE nadcod = '" + clientcode + "' ";
                myConn.Open();
                OracleCommand cmd = new OracleCommand(q, myConn);

                OracleDataReader reader = cmd.ExecuteReader();

                //read the values returned from the query executed above
                while (reader.Read())
                {
                    client.name = reader[0].ToString();
                    client.stcodt = reader[1].ToString();
                    client.chacod = reader[2].ToString();
                    client.slscod = reader[3].ToString();
                }

                client.nadcod = clientcode;
                client.dlvdat = comboBoxDlvDates.Text.ToString();
                reader.Close();
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

            return client;
        }

        private Product saveProductInfo(string barcode, Client client)
        {
            Product product = new Product();

            try
            {
                myConn.Open();

                //this is the barcode value that gets passed in the oracle query (because unfixed products are only recorded with first 7 digits, fixed products use all 13 digits)
                string parambarcode;

                //if barcode is less than 13 digits -> use empty string in query
                //if barcode starts with 6 or 3 -> use all 13 digits of barcode
                //if barcode starts with 02 -> use the first 7 digits
                // else use empty string in query
                if(string.IsNullOrEmpty(barcode) || barcode.Length < 13)
                {
                    parambarcode = "";
                }
                else if (barcode.Substring(0, 1) == "6" || barcode.Substring(0, 1) == "3")
                {
                    parambarcode = barcode;
                }
                else if (barcode.Substring(0, 2) == "02")
                {
                    parambarcode = barcode.Substring(0, 7);
                }
                else
                {
                    parambarcode = "";
                }

                string q = "select item, description, type, fixed_weight, category from innodis_ean_barcode where barcode = :barcode";

         
                using (OracleCommand cmd = new OracleCommand())
                {
                    cmd.Connection = myConn;
                    cmd.CommandText = q;
                    
                    cmd.Parameters.Add(new OracleParameter("barcode", OracleDbType.VarChar));

                    cmd.Parameters[0].Value = parambarcode;//will use parambarcode in oracle query

                    OracleDataReader reader = cmd.ExecuteReader();

                    //when saving the barcode into the product class, we use the original full barcode
                    product.barcode = barcode;
                    product.cpycod = "04";//cpycod of beau climat innodis location is 04
                    product.thisDay = DateTime.Today.ToString("dd/MM/yyyy");

                    //read the values returned from the query executed above
                    while (reader.Read())
                    {
                        product.stkcod = reader[0].ToString();//class product item code
                        product.stkdes = reader[1].ToString();//class product item description
                        
                        string type = reader[2].ToString();
                        product.FroChill = reader[4].ToString();

                        if (type == "Fixed")
                        {
                            product.netweight = reader[3].ToString();
                        }
                        else
                        {
                            decimal price = System.Convert.ToDecimal(barcode.Substring(7, 5)) / 100;

                            //assign unit price to the recorded unit price from web interface
                            decimal unitprice=1;
                            int ptype = 6;//pricing type (look up z pricing on sheet)
                            
                            try
                            {
                                //retrieve all rows of pricing promotions on the specific date of delivery
                                string w = "select itemprice, pricingtype from innodis_webinterface where (nadcode = :nadcode OR nadcode is null) AND stkcod = :stkcod AND TO_DATE(:tdate, 'dd/MM/yyyy') BETWEEN promotiondatestart AND promotiondateend";
                                

                                using (OracleCommand cmd2 = new OracleCommand())
                                {
                                    cmd2.Connection = myConn;
                                    cmd2.CommandText = w;

                                    cmd2.Parameters.Add(new OracleParameter("nadcode", OracleDbType.VarChar));
                                    cmd2.Parameters.Add(new OracleParameter("stkcod", OracleDbType.VarChar));
                                    cmd2.Parameters.Add(new OracleParameter("tdate", OracleDbType.VarChar));

                                    cmd2.Parameters[0].Value = client.nadcod;//client code
                                    cmd2.Parameters[1].Value = product.stkcod;//product code
                                    cmd2.Parameters[2].Value = client.dlvdat;//delivery  date

                                    //unitprice = Convert.ToDecimal(cmd2.ExecuteScalar());

                                    OracleDataReader rdr = cmd2.ExecuteReader();

                                    while (rdr.Read())
                                    {
                                        //the if statement below will get the lowest pricingtype available from the query w
                                        if(rdr.GetInt32(1) <= ptype)
                                        {
                                            ptype = Convert.ToInt32(rdr[1]);
                                            unitprice = Convert.ToDecimal(rdr[0]);
                                        }
                                        
                                    }
                                }
                            }
                            catch(OracleException ex)
                            {
                                if (myConn.State == ConnectionState.Open)
                                {
                                    myConn.Close();
                                }

                                MessageBox.Show(ex.Message);
                            }
                            
                            //calculate netweight with the price from the barcode and unitprice retrieved from the pricing database table
                            decimal netweightdecimal = price / unitprice;

                            product.netweight = System.Convert.ToString(Math.Round(netweightdecimal, 3));
                        }
                    }

                    string prodCod = product.stkcod;

                    //compare the scanned product codes to the product codes in the preorder datagrid. if the codes match, the user can scan the items. if not, product stkcod (item code) will be save as 'NOT'
                    foreach (DataRow dr in PreOrd.Rows)
                    {
                        if (prodCod != dr["STRCOD"].ToString())
                        {
                            product.stkcod = "NOT";
                        }
                        else
                        {
                            product.stkcod = prodCod;
                            break;
                        }
                    }
                    
                    //calculate expiry date of product scanned. if frozen, 2 years shelf life. if chilled, 5 days. if neither, 3 days. 3 days shelf life will be changed. keeping for testing purposes

                    DateTime expdate;
                    if (product.FroChill == "Frozen")
                    {
                        expdate = DateTime.ParseExact(product.thisDay, "dd/MM/yyyy", null).AddYears(2);
                        product.expdat = expdate.ToString("dd/MM/yyyy");
                    }
                    else if (product.FroChill == "Chilled")
                    {
                        TimeSpan duration = new TimeSpan(5, 0, 0, 0);
                        expdate = DateTime.ParseExact(product.thisDay, "dd/MM/yyyy", null).Add(duration);
                        product.expdat = expdate.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        TimeSpan duration = new TimeSpan(3, 0, 0, 0);
                        expdate = DateTime.ParseExact(product.thisDay, "dd/MM/yyyy", null).Add(duration);
                        product.expdat = expdate.ToString("dd/MM/yyyy");
                    }
                    
                    reader.Close();
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

            return product;
        }

        //saveFixedProductInfo and saveUnFixedProductInfo are not being used. can ignore
        //function to save fixed product info into a product class. if statement in the textbox_keypress to differentiate between fixed and unfixed products
        private Product saveFixedProductInfo(string barcode)
        {
            Product product = new Product();

            try
            {
                myConn.Open();

                string q = "SELECT innodis_ic_barcode.barcod, innodis_ic_stkmst_bc.stkcod, innodis_ic_stkmst_bc.stkdes, innodis_ic_stkmst_bc.cpycod ";
                q += "FROM innodis_ic_stkmst_bc ";
                q += "INNER JOIN innodis_ic_barcode ";
                q += "ON innodis_ic_stkmst_bc.stkcod=innodis_ic_barcode.stkcod WHERE barcod='" + barcode + "'";

                OracleCommand cmd = new OracleCommand(q, myConn);

                OracleDataReader reader = cmd.ExecuteReader();

                //read the values returned from the query executed above
                while (reader.Read())
                {

                    product.barcode = reader[0].ToString();
                    product.stkcod = reader[1].ToString();
                    product.stkdes = reader[2].ToString();
                    product.cpycod = reader[3].ToString();
                    product.netweight = "0.400";
                }

                product.thisDay = DateTime.Today.ToString("dd/MM/yyyy");

                reader.Close();
            }
            catch (OracleException ex)
            {
                if (myConn.State == ConnectionState.Open)
                {
                    myConn.Close();
                }

                MessageBox.Show(ex.Message);
            }

            return product;
        }

        //function to save unfixed product info into a product class. if statement in the textbox_keypress to differentiate between fixed and unfixed products
        private Product saveUnFixedProductInfo(string barcode)
        {
            Product product = new Product();

            try
            {
                //connection to database may have to take place after the unfixed weight products data has been inputte

                decimal price = System.Convert.ToDecimal(textBoxEAN13.Text.Substring(7, 5)) / 100;
                //will eventually have to assign unitprice to the value assigned in the webinterface table. with the clientcode, date, and product code(stkcod)
                decimal unitprice = 155;
                decimal netweightdecimal = price / unitprice;

                product.barcode = barcode;
                product.stkcod = "";
                product.stkdes = "";
                product.cpycod = "80";

                product.netweight = System.Convert.ToString(Math.Round(netweightdecimal, 3));

                product.thisDay = DateTime.Today.ToString("dd/MM/yyyy");

            }
            catch (OracleException ex)
            {
                if (myConn.State == ConnectionState.Open)
                {
                    myConn.Close();
                }

                MessageBox.Show(ex.Message);
            }

            return product;

        }

        //This function displays the information of the product scanned on the datagrid that is available on the form
        private void displayEAN13tableOnDatagrid(Product product, Client client)
        {
            //if delivery date not selected, scanned products would not be displayed on the datagrid
            if (client.dlvdat != "")
            {
                DataRow dr = clDtGriData.NewRow();
                dr["barcode"] = product.barcode;
                dr["name"] = client.name;
                dr["nadcod"] = client.nadcod;
                dr["cpycod"] = product.cpycod;
                dr["dlvdat"] = client.dlvdat;
                dr["netweight"] = product.netweight;
                dr["stkcod"] = product.stkcod;
                dr["stkdes"] = product.stkdes;
                dr["DateScanned"] = product.thisDay;
                dr["Category"] = product.FroChill;
                dr["ExpiryDate"] = product.expdat;
                dr["STCODT"] = client.stcodt;
                dr["CHACOD"] = client.chacod;
                dr["SLSCOD"] = client.slscod;

                //adding row number into clDtGriData to show how many products have been scanned
                int rownum = 0;
                if (clDtGriData.Rows.Count == 0)
                {
                    rownum = 1;
                }
                else
                {
                    for (int i = 1; i <= clDtGriData.Rows.Count; i++)
                    {
                        rownum = i+1;
                    }
                }
                

                dr["#"] = rownum; 

                clDtGriData.Rows.Add(dr);
                clDtGriData.AcceptChanges();

                dataGridView.DataSource = clDtGriData;
            }
            else
            {
                MessageBox.Show("Please choose a delivery date");
            }
        }

        //This table will insert all the values that are being displayed on the datagrid into the oracle database table
        private void insertIntoEAN13tableParam()
        {
            try
            {
                myConn.Open();

                string q = "INSERT INTO XXEAN.INNODIS_TEST(barcod, cussur, nadcode, cpycod, netweight, ean128id, stkcod, stkdes, dlvdat, scndat, category, expdat, stcodt, chacod, slscod) VALUES (:barcode,:name,:nadcod,:cpycod,:netweight,:ean128id,:stkcod,:stkdes,TO_DATE(:dlvdat,'dd/MM/yyyy'),TO_DATE(:thisday, 'dd/MM/yyyy'), :cat, TO_DATE(:expdat,'dd/MM/yyyy'), :stcodt, :chacod, :slscod)";

                //if no items are scanned
                if (clDtGriData.Rows.Count == 0)
                {
                    MessageBox.Show("Cannot print new barcode without scanned items. Please try again");
                }
                else//if items have been scanned are being viewed on the datagrid
                {
                    foreach (DataRow dr in clDtGriData.Rows)
                    {
                        String barcode = dr["barcode"].ToString();
                        String name = dr["name"].ToString();
                        String nadcod = dr["nadcod"].ToString();
                        String cpycod = dr["cpycod"].ToString();
                        String dlvdat = dr["dlvdat"].ToString();
                        String netweight = dr["netweight"].ToString();
                        int EAN128ID = Convert.ToInt32(dr["EAN128ID"]);
                        String stkcod = dr["stkcod"].ToString();
                        String stkdes = dr["stkdes"].ToString();
                        String thisday = dr["DateScanned"].ToString();
                        String cat = dr["Category"].ToString();
                        String expdat = dr["ExpiryDate"].ToString();
                        String stcodt = dr["STCODT"].ToString();
                        String chacod = dr["CHACOD"].ToString();
                        String slscod = dr["SLSCOD"].ToString();

                        using (OracleCommand cmd = new OracleCommand())
                        {
                            cmd.Connection = myConn;
                            cmd.CommandText = q;

                            cmd.Parameters.Add(new OracleParameter("barcode", OracleDbType.VarChar));
                            cmd.Parameters.Add(new OracleParameter("name", OracleDbType.VarChar));
                            cmd.Parameters.Add(new OracleParameter("nadcod", OracleDbType.VarChar));
                            cmd.Parameters.Add(new OracleParameter("cpycod", OracleDbType.VarChar));
                            cmd.Parameters.Add(new OracleParameter("netweight", OracleDbType.VarChar));
                            cmd.Parameters.Add(new OracleParameter("ean128id", OracleDbType.Number));
                            cmd.Parameters.Add(new OracleParameter("stkcod", OracleDbType.VarChar));
                            cmd.Parameters.Add(new OracleParameter("stkdes", OracleDbType.VarChar));
                            cmd.Parameters.Add(new OracleParameter("dlvdat", OracleDbType.VarChar));
                            cmd.Parameters.Add(new OracleParameter("thisday", OracleDbType.VarChar));
                            cmd.Parameters.Add(new OracleParameter("cat", OracleDbType.VarChar));
                            cmd.Parameters.Add(new OracleParameter("expdat", OracleDbType.VarChar));
                            cmd.Parameters.Add(new OracleParameter("stcodt", OracleDbType.VarChar));
                            cmd.Parameters.Add(new OracleParameter("chacod", OracleDbType.VarChar));
                            cmd.Parameters.Add(new OracleParameter("slscod", OracleDbType.VarChar));

                            cmd.Parameters[0].Value = barcode;
                            cmd.Parameters[1].Value = name;
                            cmd.Parameters[2].Value = nadcod;
                            cmd.Parameters[3].Value = cpycod;
                            cmd.Parameters[4].Value = netweight;
                            cmd.Parameters[5].Value = EAN128ID;
                            cmd.Parameters[6].Value = stkcod;
                            cmd.Parameters[7].Value = stkdes;
                            cmd.Parameters[8].Value = dlvdat;
                            cmd.Parameters[9].Value = thisday;
                            cmd.Parameters[10].Value = cat;
                            cmd.Parameters[11].Value = expdat;
                            cmd.Parameters[12].Value = stcodt;
                            cmd.Parameters[13].Value = chacod;
                            cmd.Parameters[14].Value = slscod;

                            cmd.ExecuteNonQuery();
                        }
                    }
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

        //The SendPrinterData function allows communication with the printer through the software. The string value will be sent as input and get printed accordingly
        private void SendPrinterData(string value)
        {
            byte[] data = Encoding.ASCII.GetBytes(value);
            string printerLoc = InnodisLogin.printerIP;//retrieves IP address of printer that was chosen on login

            try
            {
                insertIntoEAN13tableParam();//function to insert the values from datagrid into the database table innodis_test
                if (clDtGriData.Rows.Count != 0)
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

                            clDtGriData.Rows.Clear();
                            textBoxEAN13.Focus();
                        }
                    }
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.ToString());
            }
        }

        //This function assigns a unique EAN128 id number to the products scanned
        private void insertEAN128IDtoDatagrid()
        {
            myConn.Open();

            string w = "select ean128id from xxean.innodis_test where ean128id in (select max(ean128id) from xxean.innodis_test)";
            OracleCommand cmd2 = new OracleCommand(w, myConn);

            OracleDataReader reader = cmd2.ExecuteReader();

            int ean128id = 0;
            while (reader.Read())
            {
                ean128id = Convert.ToInt32(reader[0]);
                ean128id += 1;
            }

            if (ean128id == 0)
            {
                ean128id = 1;
            }

            foreach (DataRow dr in clDtGriData.Rows)
            {
                dr["EAN128ID"] = ean128id;
            }

            reader.Close();
            myConn.Close();
        }

        //this function creates the string that will be sent to the printer to print ean128id
        private string insertPrintString()
        {
            int ean128id = 0;
            decimal netweight = 0;
            string clientName = "";
            string proddate = clDtGriData.Rows[0]["DateScanned"].ToString();
            DateTime expdate = DateTime.ParseExact(clDtGriData.Rows[0]["ExpiryDate"].ToString(), "dd/MM/yyyy", null);
            string cat = clDtGriData.Rows[0]["Category"].ToString();
            string itemDescription = clDtGriData.Rows[0]["stkdes"].ToString();

            foreach (DataRow dr in clDtGriData.Rows)
            {
                clientName = dr["name"].ToString();
                ean128id = Convert.ToInt32(dr["EAN128ID"]);
                netweight += Convert.ToDecimal(dr["netweight"]);
                
            }
            
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
            strSend += "PP 250," + y.ToString() + ":";
            strSend += "PT \"ProdDate: " + proddate + "\":";
            //strSend += "PT \"ProductionDate: \";DATE$:";
            strSend += "PP 550," + y.ToString() + ":";
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

        private void btnPrintEAN128_Click(object sender, EventArgs e)
        {
            
        }

        private void textBoxEAN13_KeyPress(object sender, KeyPressEventArgs e)
        {
            //when enter key is pressed //when item is scanned
            if (e.KeyChar == (char)Keys.Return)
            {
                e.Handled = true;
                Client client = new Client();
                client = saveClientInfo(comboBoxClient.SelectedValue.ToString());
                Product product = new Product();
                bool sameitem = false;//state to check if same item has been scanned
                bool sameprod = true; // state to check if the same product has been scanned or not

                product = saveProductInfo(textBoxEAN13.Text.ToString(), client);


                if (textBoxEAN13.Text.Length != 13)
                {
                    MessageBox.Show("That item cannot be scanned on this page");
                }
                else if (product.stkcod == "NOT")//this occurs when a scanned item does not match the items in the preorder list
                {
                    MessageBox.Show("The scanned product is not included in the Pre-Order. Please scan another item");
                }
                else if (product.stkcod == null)//this occurs when there is no item code retrieved from database
                {
                    MessageBox.Show("The scanned product does not exist in the database");
                }
                else if (textBoxEAN13.Text.Substring(0, 2) == "02")//item is variable weight product
                {
                    //check to see if the scanned barcode has already been scanned or if the scanned barcode is the same item code as the previously scanned items
                    foreach (DataRow datrow in clDtGriData.Rows)
                    {
                        if (textBoxEAN13.Text == datrow["barcode"].ToString())
                        {
                            sameitem = true;
                        }
                    }
                    
                    if (sameitem == true)
                    {
                        //warning message that same barcode was scanned
                        MessageBox.Show("The same specific item has been scanned already");
                    }
                    
                    displayEAN13tableOnDatagrid(product, client);//display scanned information on the top datagrid
                    
                }
                else if (textBoxEAN13.Text.Substring(0, 1) == "6" || textBoxEAN13.Text.Substring(0, 1) == "3")
                {
                    displayEAN13tableOnDatagrid(product, client);//display scanned information on the top datagrid
                }
                else
                {
                    MessageBox.Show("There has been an error in scanning. Please try again");
                }

                textBoxEAN13.Text = "";
                textBoxEAN13.Focus();
            }
        }

        private void comboBoxClient_SelectedValueChanged(object sender, EventArgs e)
        {
            if (clDtGriData != null && PreOrd != null)
            {
                clDtGriData.Rows.Clear();
                PreOrd.Rows.Clear();
            }
            
            loadDeliveryDates();
        }

        private void menuItem1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBoxDlvDates_SelectedValueChanged(object sender, EventArgs e)
        {
            clDtGriData.Rows.Clear();//clear top datagrid datatable
            PreOrd.Rows.Clear();//clear preord datatable
            viewPreOrderInfo();//view new preorder information of new selected delivery date
            textBoxEAN13.Focus();
        }

        private void PrintEAN128_Click(object sender, EventArgs e)
        {
            //display message to user
            string message = "Are you sure you want to save these items into an EAN128?";
            string caption = "";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, caption, buttons, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

            if (result == DialogResult.Yes)
            {
                if (clDtGriData.Rows.Count > 0)//if items have been scanned
                {
                    try
                    {
                        insertEAN128IDtoDatagrid();//insert the unique ean128 id number to the datagrid for each item scanned
                        String strSend = insertPrintString();//return the string that will be sent to printer
                        SendPrinterData(strSend);//send the string to printer through TCP/IP
                    }
                    catch (OracleException ex)
                    {
                        MessageBox.Show("EAN 128 not Printed succesfully!");
                    }
                }
                else
                {
                    MessageBox.Show("Please scan some items before trying to print");
                }
                
            }
        }
    }
}