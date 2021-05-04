using System.Collections;

namespace Stock_Analysis
{
    internal class StockItem
    {
        private int DealDate;
        private string StockID;
        private string StockName;
        private int SecBrokerID;
        private string SecBrokerName;
        private int Price;
        private int BuyQty;
        private int CellQty;

        /*public StockItem(int DealDate, string StockID, string StockName, int SecBrokerID, string SecBrokerName, int Price, int BuyQty, int CellQty)
        {
            this.DealDate = DealDate;
            this.StockID = StockID;
            this.StockName = StockName;
            this.SecBrokerID = SecBrokerID;
            this.SecBrokerName = SecBrokerName;
            this.Price = Price;
            this.BuyQty = BuyQty;
            this.CellQty = CellQty;
        }*/
        public StockItem(ArrayList Data)
        {
            this.DealDate = (int)Data[0];
            this.StockID = (string)Data[1];
            this.StockName = (string)Data[2];
            this.SecBrokerID = (int)Data[3];
            this.SecBrokerName = (string)Data[4];
            this.Price = (int)Data[5];
            this.BuyQty = (int)Data[6];
            this.CellQty = (int)Data[7];
        }

    }
}