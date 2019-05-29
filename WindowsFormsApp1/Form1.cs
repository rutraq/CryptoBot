using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        GetCurrency currency = new GetCurrency();
        public Form1()
        {
            InitializeComponent();
            Dictionary<string, string> dict = currency.ParseJSON();
            label1.Text = dict["lprice"];
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            Dictionary<string, string> dict = currency.ParseJSON();
            label1.Text = dict["lprice"];
        }
    }
}
