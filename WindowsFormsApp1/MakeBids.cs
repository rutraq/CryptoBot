using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LibraryCex;
using Telegram.Bot.Args;

namespace WindowsFormsApp1
{
    class MakeBids
    {
        bool start;
        int username;

        public bool Start { get => start; set => start = value; }
        public int Username { get => username; set => username = value; }

        public string Analize()
        {
            Cex cex = new Cex();
            var tradeHistory = cex.GetTradeHistory();
            decimal sell = 0;
            decimal buy = 0;
            int countSell = 0;
            int countBuy = 0;
            for (int i = 0; i < 6; i++)
            {
                if (tradeHistory[i].Type == "sell")
                {
                    countSell++;
                    sell += Convert.ToDecimal(tradeHistory[i].Amount.Replace(".", ","));
                }
                else
                {
                    countBuy++;
                    buy += Convert.ToDecimal(tradeHistory[i].Amount.Replace(".", ","));
                }
            }
            if (buy > sell)
            {
                return "higher";
            }
            else if (buy < sell)
            {
                return "lower";
            }
            else
            {
                return "nothing";
            }
        }
        public void Bid()
        {
            for (; ; )
            {
                string firstCheck = Analize();
                Thread.Sleep(2000);
                string secondCheck = Analize();
                if (firstCheck == secondCheck)
                {
                    GetCurrency currency = new GetCurrency();
                    decimal course = Convert.ToDecimal(currency.ParseJSON()["lprice"].Replace(".", ","));
                    if (firstCheck == "higher")
                    {
                        double newCourse = Convert.ToDouble(course) + 0.01;
                        DataBase data = new DataBase();
                        List<string> info = data.Getinfo(username);
                        PlaceOrder order = new PlaceOrder();
                        int amount = data.GetSum(username);
                        order.MakeOrder(info[0], info[1], info[2], Convert.ToString(amount), Convert.ToString(newCourse), "buy");
                        break;
                    }
                    else if (firstCheck == "lower")
                    {
                        double newCourse = Convert.ToDouble(course) - 0.01;
                        DataBase data = new DataBase();
                        List<string> info = data.Getinfo(username);
                        PlaceOrder order = new PlaceOrder();
                        int amount = data.GetSum(username);
                        order.MakeOrder(info[0], info[1], info[2], Convert.ToString(amount), Convert.ToString(newCourse), "sell");
                        break;
                    }
                } 
            }
        }
    }
}
