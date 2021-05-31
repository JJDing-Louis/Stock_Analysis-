using System.Collections.Generic;

namespace Stock_Analysis
{
    internal class StockRankItem
    {
        public string StockName { get; set; }
        public string SecBrokerName { get; set; }
        public int BuyCellOver { get; set; }
        private int BuyTotal { get; set; }
        private int CellTotal { get; set; }

        //public StockRankItem()
        //{
        //    StockName = string.Empty;
        //    SecBrokerName = string.Empty;
        //    BuyCellOver = 0;
        //}

        public StockRankItem(string stockname, string secbrokername, int buycellover) //建構子多載
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

        //public List<StockRankItem> mergeSecBroker(List<StockItem> stockItems)
        //{
        //    Dictionary<string, List<string>> group = new Dictionary<string, List<string>>();

            


        //    //List<StockRankItem> secBrokerList = new List<StockRankItem>();
        //    //List<string> secBroker_List = new List<string>();

        //    ////建立劵商名稱的對照表
        //    //foreach (StockItem stock in stockItems)
        //    //{
        //    //    if (!secBroker_List.Contains(stock.SecBrokerName))
        //    //    {
        //    //        secBroker_List.Add(stock.SecBrokerName);
        //    //    }
        //    //}

        //    ////先取secBroker
        //    //foreach (string secBroker in secBroker_List)
        //    //{
        //    //    SecBrokerName = secBroker;
        //    //    int BuyTotal = 0; //再在想一下
        //    //    int CellTotal = 0; //再在想一下
        //    //    foreach (StockItem stock in stockItems)//比對各項
        //    //    {
        //    //        StockName = stock.StockName;
        //    //        if (stock.SecBrokerName.Equals(secBroker))
        //    //        {
        //    //            BuyTotal += int.Parse(stock.BuyQty);
        //    //            CellTotal += int.Parse(stock.CellQty);
        //    //        }
        //    //    }
        //    //    BuyCellOver = BuyTotal - CellTotal;
        //    //    StockRankItem stockRankItem = new StockRankItem(StockName, SecBrokerName, BuyCellOver);
        //    //    secBrokerList.Add(stockRankItem);
        //    //}
        //    //return secBrokerList;
        //}
    }
}