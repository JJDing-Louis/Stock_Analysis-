namespace Stock_Analysis
{
    internal class StockRankItem
    {
        public string StockName { get; set; }
        public string SecBrokerName { get; set; }
        public int BuyCellOver { get; set; }
        private int BuyTotal { get; set; }
        private int CellTotal { get; set; }

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