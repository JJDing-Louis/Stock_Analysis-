namespace Stock_Analysis
{
    internal class StockRankItem
    {
        public string StockName { get; set; }
        public string SecBrokerName { get; set; }
        public int BuyCellOver { get; set; }
        private int BuyTotal { get; set; }
        private int CellTotal { get; set; }

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="stockname">股票名稱</param>
        /// <param name="secbrokername">券商名稱</param>
        /// <param name="buycellover">買賣超</param>
        public StockRankItem(string stockname, string secbrokername, int buycellover)
        {
            StockName = stockname;
            SecBrokerName = secbrokername;
            BuyCellOver = buycellover;
        }

        public void setBuyCellOver(string buyQty, string cellQty)
        {
            BuyTotal += int.Parse(buyQty);
            CellTotal += int.Parse(cellQty);
            BuyCellOver = BuyTotal - CellTotal;
        }
    }
}