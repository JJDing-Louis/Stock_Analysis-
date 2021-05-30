using System.Collections.Generic;

namespace Stock_Analysis
{
    internal class StockRankItem
    {

        public string StockName { get; set; }
        public string SecBrokerName { get; set; }
        public int BuyCellOver { get; set; }



        public StockRankItem()
        {
            StockName = string.Empty;
            SecBrokerName = string.Empty;
            BuyCellOver = 0;
        }

        public StockRankItem(string stockName, string secBrokerName, int buyCellOver) //建構子多載
        {
            StockName = stockName;
            SecBrokerName = secBrokerName;
            BuyCellOver = buyCellOver;
        }

        public List<StockRankItem> mergeSecBroker(List<StockItem> stockItems)
        {
            List<StockRankItem> secBrokerList = new List<StockRankItem>();
            List<string> secBroker_List = new List<string>();

            foreach (StockItem stock in stockItems)
            {
                if (!secBroker_List.Contains(stock.SecBrokerName))
                {
                    secBroker_List.Add(stock.SecBrokerName);
                }
            }

            foreach (string secBroker in secBroker_List) //先取secBroker
            {
                SecBrokerName = secBroker;
                int BuyTotal = 0; //再在想一下
                int CellTotal = 0; //再在想一下
                foreach (StockItem stock in stockItems)//比對各項
                {
                    StockName = stock.StockName;
                    if (stock.SecBrokerName.Equals(secBroker))
                    {
                        BuyTotal += int.Parse(stock.BuyQty);
                        CellTotal += int.Parse(stock.CellQty);
                    }
                }
                BuyCellOver = BuyTotal - CellTotal;
                StockRankItem stockRankItem = new StockRankItem(StockName, SecBrokerName, BuyCellOver);
                secBrokerList.Add(stockRankItem);
            }
            return secBrokerList;

        }
    }
}