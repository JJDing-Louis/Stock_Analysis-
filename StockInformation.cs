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

        public StockInformation()
        {
            BuyTotal = 0;
            CellTotal = 0;
            BuyCellOver = 0;
            SecBrokerCnt = 0;
            AvgPrice = 0;
            //Sum = 0;
        }

        public StockInformation Count(List<StockItem> a)
        {


            return new StockInformation();

        }
    }
}