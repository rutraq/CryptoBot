using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LibraryCex;
using System.Threading;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        GetCurrency currency = new GetCurrency();
        Telegram telegram = new Telegram();
        public Form1()
        {
            InitializeComponent();
            Dictionary<string, string> dict = currency.ParseJSON();
            telegram.Bot();
            telegram.Text_for_client = "Курс XRP/USD: " + dict["lprice"];
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            Dictionary<string, string> dict = currency.ParseJSON();
            telegram.Text_for_client = "Курс XRP/USD: " + dict["lprice"];
        }
    }
}
