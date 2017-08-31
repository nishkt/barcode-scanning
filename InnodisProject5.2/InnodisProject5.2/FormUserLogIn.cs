using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Devart.Data.Oracle;

namespace InnodisProject5._2
{
    public partial class FormUserLogIn : Form
    {
        //call the object that disables the windows start button
        InnodisProject5._2.hwndutils _hwndutils = new InnodisProject5._2.hwndutils();
        private bool _bInitializing = true;

        //create printer information class. the user will choose the printer to be used and the IP address will be accessible across all forms through GlobalClass.cs
        public class PrinterInfo
        {
            public string Name { get; set; }
            public string ip { get; set; }
        }

        OracleConnection myConn = new OracleConnection();

        public FormUserLogIn()
        {
            InitializeComponent();

            //disable the windows start button. when set to true, the button will be disabled
            this._hwndutils.StartButtonDisabled = _bInitializing;
        }

        private void validateLogin()
        {
            string validate;
            try
            {
                myConn.Open();
                string q = "select APPS.FND_WEB_SEC.validate_login(:username,:password) from dual";//PL/SQL function to verify username and password. returns Y for correct credentials. N for incorrect

                using (OracleCommand cmd = new OracleCommand())
                {
                    cmd.Connection = myConn;
                    cmd.CommandText = q;

                    cmd.Parameters.Add(new OracleParameter("username", OracleDbType.VarChar));
                    cmd.Parameters.Add(new OracleParameter("password", OracleDbType.VarChar));

                    cmd.Parameters[0].Value = textBoxUsername.Text.ToString();
                    cmd.Parameters[1].Value = textBoxPassword.Text.ToString();

                    //validate = cmd.ExecuteScalar().ToString();//used for live. needs to run PL/SQL function string to verify username and password
                    validate = "Y";//used for testing purposes. inputting username and password gets tedious. 

                    if (validate == "Y")
                    {
                        FormChooseMethod myformchoosemethod = new FormChooseMethod();
                        myformchoosemethod.ShowDialog();
                    }
                    
                    else if (validate == "N")
                    {
                        MessageBox.Show(" The username and/or password is incorrect. Please try again");
                    }
                }

                myConn.Close();
            }
            catch (OracleException ex)
            {
                if (myConn.State == ConnectionState.Open)
                    myConn.Close();

                MessageBox.Show("You are not connected. Please check your network settings");
            }

        }

        private void btnLogIn_Click(object sender, EventArgs e)
        {
            InnodisLogin.uname = textBoxUsername.Text.ToString();
            InnodisLogin.printerIP = PrinterInfoComboBox.SelectedValue.ToString();

            try
            {
                //myConn.ConnectionString = "User Id=" + textBoxUsername.Text + ";Password=" + textBoxPassword.Text + ";Server=innebnidm02.innodisgroup.com;Port = 1541;Sid = prd1;";
                myConn.ConnectionString = "User Id=xxean;Password=ean123;Server=innebnidm02.innodisgroup.com;Port = 1541;Sid = prd1;";
                myConn.Open();

                if (myConn.State == System.Data.ConnectionState.Open)
                {
                    //if (textBoxUsername.Text.ToString() == "" || textBoxPassword.Text.ToString() == "")
                    //{
                    //    MessageBox.Show("Please enter a username or password");
                    //}
                    //else 
                    //{
                        validateLogin();
                    //}

                    textBoxUsername.Text = "";
                    textBoxPassword.Text = "";
                    textBoxUsername.Focus();                   

                }

                myConn.Close();
                OracleConnection.ClearPool(myConn);
            }
            catch (OracleException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormUserLogIn_Load(object sender, EventArgs e)
        {
            //Build a list for user to select which printer. each printer will be assigned a different IP address
            var dataSource = new List<PrinterInfo>();
            dataSource.Add(new PrinterInfo() { Name = "Printer 1", ip = "172.17.45.245" });
            dataSource.Add(new PrinterInfo() { Name = "Printer 2", ip = "172.17.45.245" });
            dataSource.Add(new PrinterInfo() { Name = "Printer 3", ip = "172.17.45.245" });

            //Setup data binding
            this.PrinterInfoComboBox.DataSource = dataSource;
            this.PrinterInfoComboBox.DisplayMember = "Name";
            this.PrinterInfoComboBox.ValueMember = "ip";

            // make it readonly
            this.PrinterInfoComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void textBoxUsername_GotFocus(object sender, EventArgs e)
        {
            
        }

        private void textBoxUsername_LostFocus(object sender, EventArgs e)
        {
            
        }
    }
}