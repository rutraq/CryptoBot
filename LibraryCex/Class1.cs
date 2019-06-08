using System;

namespace LibraryCex
{
    public class Cex
    {
        public decimal Balance_USD(string user_id, string key, string secret_key)
        {
            decimal usdBalance = 0;
            ApiCredentials credentials = new ApiCredentials(user_id, key, secret_key);
            CexClient client = new CexClient(credentials);
            try
            {
                Balance balance = client.Account.GetBalanceAsync().Result;
                usdBalance = balance.USD.Available;
            }
            catch (AggregateException)
            {
                throw;
            }
            return usdBalance;
        }
        public decimal Balance_XRP(string user_id, string key, string secret_key)
        {
            decimal xrpBalance = 0;
            ApiCredentials credentials = new ApiCredentials(user_id, key, secret_key);
            CexClient client = new CexClient(credentials);
            try
            {
                Balance balance = client.Account.GetBalanceAsync().Result;
                xrpBalance = balance.XRP.Available;
            }
            catch (AggregateException)
            {
                throw;
            }
            return xrpBalance;
        }
        public OrderBook Order()
        {
            CexClient client = new CexClient();
            var book = client.GetOrderBookAsync(SymbolPairs.XRP_USD);
            return book.Result;
        }
    }
}
