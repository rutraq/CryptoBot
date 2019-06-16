using LibraryCex;

namespace WindowsFormsApp1
{
    class InfoFromStock
    {
        public decimal GetAsks(int countOfAsk)
        {
            Cex cex = new Cex();
            OrderBook orderBook = cex.Order();
            decimal asks = 0;
            int i = 0;
            foreach (var ask in orderBook.Asks)
            {
                bool first = true;
                foreach (var a in ask)
                {
                    if (!first)
                    {
                        asks += a;
                    }
                    first = false;
                }
                i++;
                if (i == countOfAsk)
                {
                    break;
                }
            }
            return asks;
        }
        public decimal GetBids(int countOfBids)
        {
            Cex cex = new Cex();
            OrderBook orderBook = cex.Order();
            decimal bids = 0;
            int i = 0;
            foreach (var bid in orderBook.Bids)
            {
                bool first = true;
                foreach (var a in bid)
                {
                    if (!first)
                    {
                        bids += a;
                    }
                    first = false;
                }
                i++;
                if (i == countOfBids)
                {
                    break;
                }
            }
            return bids;
        }
    }
}
