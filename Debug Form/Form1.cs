using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using ConstantContactBO;
using ConstantContactUtility;


namespace Debug_Form
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //DEBUG TESTING
            AuthenticationData authdata = new AuthenticationData();
            authdata.Username = "";
            authdata.Password = "";
            authdata.ApiKey = "";
        }
    }
}
