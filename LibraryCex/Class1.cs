using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryCex
{
    public class Cex
    {
        public decimal Balance_USD(string user_id, string key, string secret_key)
        {
            ApiCredentials credentials = new ApiCredentials(user_id, key, secret_key);
            CexClient client = new CexClient(credentials);
            Balance balance = client.Account.GetBalanceAsync().Result;
            decimal usdBalance = balance.USD.Available;
            return usdBalance;
        }
        public decimal Balance_XRP(string user_id, string key, string secret_key)
        {
            ApiCredentials credentials = new ApiCredentials(user_id, key, secret_key);
            CexClient client = new CexClient(credentials);
            Balance balance = client.Account.GetBalanceAsync().Result;
            decimal xrpBalance = balance.XRP.Available;
            return xrpBalance;
        }

        public decimal LastPrice(string user_id, string key, string secret_key)
        {
            ApiCredentials credentials = new ApiCredentials(user_id, key, secret_key);
            CexClient client = new CexClient(credentials);
            var lastPrice = client.GetLastPriceAsync(SymbolPairs.BTC_USD);
            return lastPrice.Result;
        }
    }
}
