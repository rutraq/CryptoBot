﻿using System;
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
    public partial class MainForm : Form
    {
        GetCurrency currency = new GetCurrency();
        MyAccount account = new MyAccount();
        Telegram telegram = new Telegram();
        public MainForm()
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
        }
    }
}
