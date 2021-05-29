namespace Stock_Analysis
{
    public class StockItem
    {
        public string DealDate { get; }
        public string StockID { get; }
        public string StockName { get; }
        public string SecBrokerID { get; }
        public string SecBrokerName { get; }
        public string Price { get; }
        public string BuyQty { get; }
        public string CellQty { get; }

        public StockItem(string data)
        {
            string[] Data = data.Split(',');
            DealDate = Data[0];
            StockID = Data[1];
            StockName = Data[2];
            SecBrokerID = Data[3];
            SecBrokerName = Data[4];
            Price = Data[5];
            BuyQty = Data[6];
            CellQty = Data[7];
        }

        public string[] getStockItem()
        {
            string[] data = new string[] { DealDate, StockID, StockName, SecBrokerID, SecBrokerName, Price, BuyQty, CellQty };
            return data;
        }
    }
}