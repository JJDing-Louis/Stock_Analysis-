namespace Stock_Analysis
{
    public class StockItem
    {
        /// <summary>
        ///DealDate: 交易日期
        ///StockID: 股票ID
        ///StockName: 股票名稱
        ///SecBrokerID:券商代號
        ///Sec
        /// </summary>
        public string DealDate { get; }

        public string StockID { get; }
        public string StockName { get; }
        public string SecBrokerID { get; }

        /// <summary>
        /// 87逆
        /// </summary>
        public string SecBrokerName { get; }

        public string Price { get; }
        public string BuyQty { get; }
        public string CellQty { get; }

        /// <summary>
        /// 傳入讀取的文字
        /// </summary>
        /// <param name="data">文字字串</param>
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
    }
}