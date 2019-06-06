using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LibraryCex;
using System.Threading;
using Telegram.Bot.Args;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        GetCurrency currency = new GetCurrency();
        Telegram telegram = new Telegram();
        private string userMessage = "";
        public Form1()
        {
            InitializeComponent();
            Dictionary<string, string> dict = currency.ParseJSON();
            telegram.Bot();
            telegram.Text_for_client = "Курс XRP/USD: " + dict["lprice"];
            telegram.Probe(Listen);
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            Dictionary<string, string> dict = currency.ParseJSON();
            telegram.Text_for_client = "Курс XRP/USD: " + dict["lprice"];
        }
        private void Listen(object sender, MessageEventArgs e)
        {
            userMessage += e.Message.Date + ": ";
            userMessage += e.Message.Chat.FirstName + " ";
            if (e.Message.Chat.Username != null)
            {
                userMessage += $"\"{e.Message.Chat.Username}\" ";

            }
            userMessage += $"text: {e.Message.Text}";
            userMessage += "\n";
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            richTextBox1.Text += userMessage;
        }
    }
}
