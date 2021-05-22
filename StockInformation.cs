namespace Stock_Analysis
{
    internal class StockInformation
    {
        public string StockID { get; }
        public string StockName { get; }
        public int BuyTotal { get; }
        public int CellTotal { get; }
        public double AvgPrice { get; }
        public int BuyCellOver { get; }
        public int SecBrokerCnt { get; }

        public StockInformation(string stockID, string stockName, int buyTotal, int cellTotal, double avgPrice, int buyCellOver, int secBrokerCnt)
        {
            StockID = stockID;
            StockName = stockName;
            BuyTotal = buyTotal;
            CellTotal = cellTotal;
            AvgPrice = avgPrice;
            BuyCellOver = buyCellOver;
            SecBrokerCnt = secBrokerCnt;
        }
    }
}