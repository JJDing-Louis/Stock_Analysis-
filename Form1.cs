using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Stock_Analysis
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// group為整理好的資料
        /// </summary>
        private Dictionary<string, List<StockItem>> group = new Dictionary<string, List<StockItem>>(); //讀取並整好的資料

        private List<StockRankItem> stockRanklist_dt = new List<StockRankItem>(); //建立排名資料RankTop50用的資料
        private StockInformation stockInformation; //建立用在搜尋欄位的物件
        private bool datatype = false; //紀錄輸入框的狀態
        private Stopwatch stopwatch = new Stopwatch();

        private Dictionary<string, List<string>> ColumnName = new Dictionary<string, List<string>>();//建立comboboxlist用的東西

        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 建立開啟檔案的方式
        /// </summary>
        private void readFile()
        {
            lb_Status.Text = "讀取中";

            using (StreamReader sr = new StreamReader(txtfile_address.Text, Encoding.GetEncoding("Big5")))
            {
                //建立欄位名稱
                sr.ReadLine(); //(思考其他寫法)//以下建立欄位
                string stockContent = sr.ReadLine();

                ColumnName.Add("All", new List<string>()); //建立一組All的key,value

                while (!string.IsNullOrWhiteSpace(stockContent))
                {
                    StockItem stock = new StockItem(stockContent);

                    if (!group.TryGetValue(stock.StockID, out List<StockItem> groupId)) //利用Dic去查詢對應的Value，有則把List取出並加入stockitem，沒有則建立List
                    {
                        groupId = new List<StockItem>(); //建立Value(List<StockItem>)
                        group.Add(stock.StockID, groupId);//加入一組Key,Value進入

                        ColumnName.Add($"{stock.StockID} - {stock.StockName}", new List<string>() { stock.StockID }); //建立一組對照表
                    }
                    groupId.Add(stock);
                    stockContent = sr.ReadLine();
                }

                ColumnName["All"] = group.Select(data => data.Key).ToList(); //指定一組值到All的Value裡面


                dGV_List.DataSource = group.SelectMany(data => data.Value).ToList(); //資料與dGV_List繫結
                cbm_stocklist.DataSource = ColumnName.Select(data => data.Key).ToList(); //資料與combobox繫結
            }
            lb_Status.Text = "讀取完成";
        }


        /// <summary>
        /// 按下讀取檔案的觸發事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                //OpenFileDialog讀檔設定
                openFileDialog.InitialDirectory = @"C:\"; //預設檔案地址
                openFileDialog.Filter = "csv檔|*.csv|純文字檔|*.txt|所有檔案|*.*"; //預設檔案分類
                openFileDialog.FilterIndex = 0; //預設檔案的過濾項目
                openFileDialog.RestoreDirectory = true; //取得或設定值，指出對話方塊是否在關閉前將目錄還原至先前選取的目錄。
                openFileDialog.FileName = string.Empty; //取得或設定含有檔案對話方塊中所選取檔名的字串。
                openFileDialog.Multiselect = false; //不允許多選
                openFileDialog.ShowReadOnly = true; //設定唯獨
                openFileDialog.Title = "請選取股票資料"; //讀檔標題

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtfile_address.Text = openFileDialog.FileName;
                    stopwatch.Start();
                    readFile();
                    log_time("讀取");
                    stopwatch.Restart();
                    log_time("ComboBox產生");
                }
            }
        }

        /// <summary>
        /// 按下按鈕開始搜尋，判定搜尋條件，並建立清單
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void btnStockSearch_Click(object sender, EventArgs e)
        {
            List<StockItem> stockItems_dt = new List<StockItem>(); //建立搜尋資料用的列表陣列
            List<StockInformation> stockInformation_dt = new List<StockInformation>();//建立搜尋資股票欄位的資料

            if (!ColumnName.TryGetValue(cbm_stocklist.Text, out List<string> allStockId))
            {
                //單個.多個
                allStockId = cbm_stocklist.Text.Split(',').ToList();
            }



            stopwatch.Restart();
            if (datatype)
            {
                //先清空一次dGV_List資料源
                stockItems_dt.Clear();
                dGV_List.DataSource = null; //UI先清空
                //先清空一次dGV_Items資料源
                stockInformation_dt.Clear();
                dGV_Items.DataSource = null;
                // string[] stockID_list = cbm_stocklist.Text.Split(','); //取得Combox的文字內容

                List<StockItem> first= new List<StockItem>();
                List< StockInformation > second= new List<StockInformation>();

                foreach (string stockID in allStockId)
                {
                    List<StockItem> eee = group[stockID];

                    //StockInformation test =;
                    // 1
                    first.AddRange(eee);
                    // 2
                    second.Add(new StockInformation().Count(eee));
                    //Search(stockID);

                    //stockInformation_dt.Add(stockInformation);
                    //foreach (var item in group[stockID])
                    //{
                    //    stockItems_dt.Add(item);
                    //}
                }
                dGV_List.DataSource = stockItems_dt;

                dGV_Items.DataSource = second;
            }
            else
            {
                //先清空一次UI的資料源
                dGV_List.DataSource = null;
                dGV_Items.DataSource = null;

                //開始查詢資料
                string stockID = cbm_stocklist.Text.Split(' ')[0]; //取得Combox的文字內容
                dGV_List.DataSource = group[stockID];

                //股票查詢資料更新更新
                //先清空一次資料源
                stockInformation_dt.Clear();
                var eee = Search(stockID);
                stockInformation_dt.Add(stockInformation);

                //讀取新資料
                dGV_Items.DataSource = stockInformation_dt;
            }

            log_time("查詢");
        }

        /// <summary>
        /// 發現選單有更換，所觸發的事件
        /// </summary>
        /// <param name="sender">讀取盪案按鍵</param>
        /// <param name="e">讀取檔案室建</param>
        private void cbm_stocklist_SelectedIndexChanged(object sender, EventArgs e)
        {
            string a = "1234";
            List<string> ee = a.Split(',').ToList();

            datatype = false;
        }

        /// <summary>
        /// 發現選單有輸入文字，所觸發的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbm_stocklist_TextUpdate(object sender, EventArgs e)
        {
            datatype = true;
        }

        /// <summary>
        /// 查詢股票的價位細節
        /// 以ID搜尋
        /// </summary>
        /// <param name="stockID"></param>
        /// <returns></returns>
        public StockInformation Search(string stockID) //在想一下如何簡化
        {
            //以下為搜尋方法
            //List<StockItem> stockItems = group[stockID];

            StockInformation test = new StockInformation();
            StockInformation a = test.Count(group[stockID]);
            //int buyTotal = 0; ;
            //int cellTotal = 0;
            //int buyCellOver = 0;
            //int secBrokerCnt = 0;
            //double avgPrice = 0;
            double sum = 0;

            //List<string> secBrokerID_List = new List<string>();

            //foreach (StockItem stock in stockItems)
            //{
            //    buyTotal += int.Parse(stock.BuyQty);
            //    cellTotal += int.Parse(stock.CellQty);
            //    sum = double.Parse(stock.Price) * (int.Parse(stock.BuyQty) + int.Parse(stock.CellQty));
            //    if (!secBrokerID_List.Contains(stock.SecBrokerID))
            //    {
            //        secBrokerID_List.Add(stock.SecBrokerID);
            //    }
            //}
            //avgPrice = sum / (buyTotal + cellTotal);
            //buyCellOver = buyTotal - cellTotal;
            //secBrokerCnt = secBrokerID_List.Count;
            //stockInformation = new StockInformation(stockID, stockItems[0].StockName, buyTotal, cellTotal, avgPrice, buyCellOver, secBrokerCnt);
            return test;
        }

        /// <summary>
        /// 按下Top50 Rank
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMarketingRank_Click(object sender, EventArgs e)
        {
            stopwatch.Restart();
            if (datatype)
            {
                //先清空一次資料源
                stockRanklist_dt.Clear();
                dGV_StockRank.DataSource = null;
                string[] stockID_List = cbm_stocklist.Text.Split(','); //取得Combox的文字內容
                foreach (string stockID in stockID_List)
                {
                    mergesecBroker(stockID);
                }
                //排序與顯示前50筆資料
                sortRank();
            }
            else
            {
                //先清空一次資料源
                stockRanklist_dt.Clear();
                dGV_StockRank.DataSource = null;
                //開始查詢資料
                string stockID = cbm_stocklist.Text.Split(' ')[0]; //取得Combox的文字內容
                mergesecBroker(stockID);
                //排序與顯示前50筆資料
                sortRank();
            }

            log_time("買賣超Top50");
        }

        private void log_time(string message) //筆記!!!
        {
            rtxt_ProcessStatus.Text = $"{rtxt_ProcessStatus.Text} {message}時間:{stopwatch.Elapsed.ToString(@"hh\:mm\:ss\:fff")}{Environment.NewLine}";
            stopwatch.Stop();
        }

        /// <summary>
        /// 券商名稱整併與稀出
        /// </summary>
        /// <param name="stockID">輸入股票ID</param>
        private void mergesecBroker(string stockID) //再想一下如何簡化
        {
            //合併相同的SecBrokerID
            List<string> secBroker_List = new List<string>();
            foreach (StockItem stock in group[stockID])
            {
                if (!secBroker_List.Contains(stock.SecBrokerName))
                {
                    secBroker_List.Add(stock.SecBrokerName);
                }
            }

            foreach (string secBroker in secBroker_List) //先取secBroker
            {
                string stockName = string.Empty;
                int buyTotal = 0;
                int cellTotal = 0;
                int buyCellOver = 0;
                foreach (StockItem stock in group[stockID])//比對各項
                {
                    stockName = stock.StockName;
                    if (stock.SecBrokerName.Equals(secBroker))
                    {
                        buyTotal += int.Parse(stock.BuyQty);
                        cellTotal += int.Parse(stock.CellQty);
                    }
                }
                buyCellOver = buyTotal - cellTotal;
                StockRankItem stockRank = new StockRankItem(stockName, secBroker, buyCellOver);
                stockRanklist_dt.Add(stockRank);
            }
        }

        /// <summary>
        /// Top50資料排序
        /// </summary>
        private void sortRank()
        {
            stockRanklist_dt.Sort((x, y) => -x.BuyCellOver.CompareTo(y.BuyCellOver));
            if (stockRanklist_dt.Count > 50)
            {
                stockRanklist_dt.RemoveRange(50, (stockRanklist_dt.Count - 50));
            }
            dGV_StockRank.DataSource = stockRanklist_dt;
        }
    }
}