using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryCex;

namespace WindowsFormsApp1
{
    class MakeBids
    { 
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
            if (countBuy > countSell && buy > sell)
            {
                return "higher";
            }
            else if (countBuy < countSell && buy < sell)
            {
                return "lower";
            }
            else
            {
                return "nothing";
            }
        }
    }
}
