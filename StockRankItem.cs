namespace Stock_Analysis
{
    internal class StockRankItem
    {
        public string StockName { get; }
        public string SecBrokerName { get; }
        public int BuyCellOver { get; }

        public StockRankItem(string stockName, string secBrokerName, int buyCellOver)
        {
            StockName = stockName;
            SecBrokerName = secBrokerName;
            BuyCellOver = buyCellOver;
        }

        public string test()
        {
            return string.Empty;
        }
    }
}