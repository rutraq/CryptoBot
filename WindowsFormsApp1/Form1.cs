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
        MyAccount account = new MyAccount();
        Telegram telegram = new Telegram();
        public Form1()
        {
            InitializeComponent();
            Dictionary<string, string> dict = currency.ParseJSON();
            label1.Text = dict["lprice"];
            telegram.Bot();
            telegram.Text_for_client = "Курс XRP/USD: " + dict["lprice"];
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            Dictionary<string, string> dict = currency.ParseJSON();
            label1.Text = dict["lprice"];
            telegram.Text_for_client = "Курс XRP/USD: " + dict["lprice"];
        }

        private void TelegramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f = new Form();
            f.Width = 500;
            f.Height = 500;
            f.Show();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Cex cex = new Cex();
            decimal s = new decimal();
            Thread thread = new Thread(delegate () { s = cex.Balance_USD(); });
            thread.Start();
            thread.Join();
            MessageBox.Show(Convert.ToString(s));
        }
    }
}
