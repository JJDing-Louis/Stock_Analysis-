namespace Stock_Analysis
{
    internal class StockRankItem
    {
        /// <summary>
        ///
        /// </summary>
        public string StockName { get; }

        /// <summary>
        ///
        /// </summary>
        public string SecBrokerName { get; }

        public int BuyCellOver { get; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="stockName"></param>
        /// <param name="secBrokerName"></param>
        /// <param name="buyCellOver"></param>
        public StockRankItem(string stockName, string secBrokerName, int buyCellOver)
        {
            StockName = stockName;
            SecBrokerName = secBrokerName;
            BuyCellOver = buyCellOver;
        }
    }
}