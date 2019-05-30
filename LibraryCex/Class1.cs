using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryCex
{
    public class Cex
    {
        static ApiCredentials credentials = new ApiCredentials("up112448722", "74FTgNUB0xDlXKB17us6EOPqQ", "9gZbn9aNyvpJZZRv2zu1lgchbE");
        static CexClient client = new CexClient(credentials);
        public decimal Balance_USD()
        {
            Balance balance = client.Account.GetBalanceAsync().Result;
            decimal usdBalance = balance.USD.Available;
            return usdBalance;
        }

        public decimal LastPrice()
        {
            var lastPrice = client.GetLastPriceAsync(SymbolPairs.BTC_USD);
            return lastPrice.Result;
        }
    }
}
