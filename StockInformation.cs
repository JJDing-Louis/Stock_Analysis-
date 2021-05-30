using System.Collections.Generic;

namespace Stock_Analysis
{
    public class StockInformation
    {
        public string StockID { get; set; }
        public string StockName { get; set; }
        public int BuyTotal { get; set; }
        public int CellTotal { get; set; }
        public double AvgPrice { get; set; }
        public int BuyCellOver { get; set; }
        public int SecBrokerCnt { get; set; }

        /* public StockInformation()
         {
             StockID = string.Empty;
             StockName = string.Empty;
             BuyTotal = 0;
             CellTotal = 0;
             AvgPrice = 0;
             BuyCellOver = 0;
             SecBrokerCnt = 0;
         }

         public StockInformation(string stockID, string stockName, int buyTotal, int cellTotal, double avgPrice, int buyCellOver, int secBrokerCnt)
         {
             StockID = stockID;
             StockName = stockName;
             BuyTotal = buyTotal;
             CellTotal = cellTotal;
             AvgPrice = avgPrice;
             BuyCellOver = buyCellOver;
             SecBrokerCnt = secBrokerCnt;
         }*/

        public StockInformation getInformation(List<StockItem> stockID)
        {
            StockID = stockID[0].StockID;
            StockName = stockID[0].StockName;
            double sum = 0;

            List<string> secBrokerID_List = new List<string>();
            foreach (StockItem stock in stockID)
            {
                BuyTotal += int.Parse(stock.BuyQty);
                CellTotal += int.Parse(stock.CellQty);
                sum = double.Parse(stock.Price) * (int.Parse(stock.BuyQty) + int.Parse(stock.CellQty));
                if (!secBrokerID_List.Contains(stock.SecBrokerID))
                {
                    secBrokerID_List.Add(stock.SecBrokerID);
                }
            }
            AvgPrice = sum / (BuyTotal + CellTotal);
            BuyCellOver = BuyTotal - CellTotal;
            SecBrokerCnt = secBrokerID_List.Count;

            //StockInformation stockInformation = new StockInformation(StockID, StockName, BuyTotal, CellTotal, AvgPrice, BuyCellOver, SecBrokerCnt);
            return this;
        }
    }
}