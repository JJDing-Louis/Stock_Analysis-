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
            return this;
        }
    }
}