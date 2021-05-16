using System.Collections;
using System.Collections.Generic;

namespace Stock_Analysis
{
    internal class StockItem
    {
        private string DealDate { get;}
        private string StockID { get; }
        private string StockName { get; }
        private string SecBrokerID { get; }
        private string SecBrokerName { get; }
        private string Price { get; }
        private string BuyQty { get; }
        private string CellQty { get; }

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
        public StockItem(string data)
        {
            //List<string> Data = new List<string>(data.Split(','));//改array測試
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
            string[] data = new string[] {DealDate, StockID, StockName, SecBrokerID, SecBrokerName, Price, BuyQty, CellQty};
            return data;
         }

        public void getBuyTotal()
        {
        

            
        }

        public void getCellTotal()
        {

        }

        public void getAvgPrice()
        {

        }

        public void getBuyCellOver()
        {

        }

        public void getSecBrokerCnt()
        {

        }

    }
}