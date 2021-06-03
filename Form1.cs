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
        /// group為csv檔讀取完成後，並整理好的資料空間
        /// </summary>
        private Dictionary<string, List<StockItem>> group = new Dictionary<string, List<StockItem>>();

        /// <summary>
        ///  stockRanklist_dt為secBroker讀取完後的資料空間，放置此區域做排序
        /// </summary>
        private List<StockRankItem> stockRanklist_dt = new List<StockRankItem>();

        /// <summary>
        /// stopwatch為碼錶
        /// </summary>
        private Stopwatch stopwatch = new Stopwatch();

        /// <summary>
        /// ColumnName為建立好建立comboboxlist用的列表 (股票ID對照表)
        /// </summary>
        private Dictionary<string, List<string>> ColumnName = new Dictionary<string, List<string>>();

        /// <summary>
        /// groupSecBroker是讀取好的，另一個csv檔的查詢用的，券商代號的資料空間
        /// </summary>
        /// 用ID去對證券名稱
        private Dictionary<string, Dictionary<string, StockRankItem>> groupSecBroker = new Dictionary<string, Dictionary<string, StockRankItem>>();

        /// <summary>
        /// 初始化設定
        /// 目前沒東西
        /// </summary>
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
                //只讀取csv檔的開頭標題
                sr.ReadLine();
                //開始讀取資料
                string stockContent = sr.ReadLine();

                //建立一組All的key,value
                ColumnName.Add("All", new List<string>());

                while (!string.IsNullOrWhiteSpace(stockContent))
                {
                    StockItem stock = new StockItem(stockContent);
                    //利用Dic去查詢對應的Value，有則把List取出並加入stockitem，沒有則建立List
                    if (!group.TryGetValue(stock.StockID, out List<StockItem> groupId))
                    {
                        //建立Value(List<StockItem>)
                        groupId = new List<StockItem>();
                        //加入一組Key,Value進入
                        group.Add(stock.StockID, groupId);
                        //建立一組對照表
                        ColumnName.Add($"{stock.StockID} - {stock.StockName}", new List<string>() { stock.StockID });
                    }

                    //Rank表單建立
                    if (groupSecBroker.TryGetValue(stock.StockID, out Dictionary<string, StockRankItem> secBroker))
                    {
                        if (!secBroker.TryGetValue(stock.SecBrokerName, out StockRankItem stockRankItem))
                        {
                            stockRankItem = new StockRankItem(stock.StockName, stock.SecBrokerName, 0);
                            stockRankItem.setBuyCellOver(stock.BuyQty, stock.CellQty);
                            secBroker.Add(stock.SecBrokerName, stockRankItem);
                        }
                        stockRankItem.setBuyCellOver(stock.BuyQty, stock.CellQty);
                    }
                    else
                    {
                        secBroker = new Dictionary<string, StockRankItem>();
                        StockRankItem stockRankItem = new StockRankItem(stock.StockName, stock.SecBrokerName, 0);
                        stockRankItem.setBuyCellOver(stock.BuyQty, stock.CellQty);
                        secBroker.Add(stock.SecBrokerName, stockRankItem);
                        groupSecBroker.Add(stock.StockID, secBroker);
                    }
                    groupId.Add(stock);
                    stockContent = sr.ReadLine();
                }
                //指定一組值到All的Value裡面
                ColumnName["All"] = group.Select(data => data.Key).ToList();
                //資料與dGV_List繫結
                dGV_List.DataSource = group.SelectMany(data => data.Value).ToList();
                //資料與combobox繫結
                cbm_stocklist.DataSource = ColumnName.Select(data => data.Key).ToList();
            }
            lb_Status.Text = "讀取完成";
        }

        /// <summary>
        /// 觸發讀取檔案的觸發事件
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
            stopwatch.Restart();//開始計時
            List<StockItem> stockItems_dt = new List<StockItem>(); //建立搜尋資料用的列表陣列
            List<StockInformation> stockInformation_dt = new List<StockInformation>();//建立搜尋資股票欄位的資料
            //StockInformation stockInformation = new StockInformation(); //建立StockInformation物件(功能二使用)

            if (!ColumnName.TryGetValue(cbm_stocklist.Text, out List<string> allStockId))
            {
                //單筆.多筆股票查詢
                allStockId = cbm_stocklist.Text.Split(',').ToList();
            }

            //開始搜尋搜尋
            foreach (string stockID in allStockId)
            {
                //處裡dGV_List
                List<StockItem> stockItems = group[stockID];

                //
                stockItems_dt.AddRange(stockItems);

                //處裡dGV_Item
                stockInformation_dt.Add(new StockInformation().getInformation(group[stockID]));
            }
            dGV_List.DataSource = stockItems_dt; //dGV_List的繫結
            dGV_Items.DataSource = stockInformation_dt; //dGV_Item的繫結
            log_time("查詢");
        }

        /// <summary>
        /// 按下Top50 Rank
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMarketingRank_Click(object sender, EventArgs e)
        {
            stopwatch.Restart();//開始計時

            if (!ColumnName.TryGetValue(cbm_stocklist.Text, out List<string> allStockId))
            {
                //單筆.多筆股票查詢
                allStockId = cbm_stocklist.Text.Split(',').ToList();
            }

            foreach (string stock in allStockId)
            {
                stockRanklist_dt.AddRange(groupSecBroker[stock].Select(data => data.Value).ToList());
            }

            sortRank();
            log_time("買賣超Top50");
        }

        /// <summary>
        /// Top50資料排序
        /// </summary>
        private void sortRank()
        {   //減少排序方式，待修改
            List<StockRankItem> ranklist = new List<StockRankItem>();
            stockRanklist_dt.Sort((x, y) => -x.BuyCellOver.CompareTo(y.BuyCellOver));
            int index_p = stockRanklist_dt.FindIndex(data => data.BuyCellOver < 0); //開始轉負數的index
            int index_n = stockRanklist_dt.FindLastIndex(data => data.BuyCellOver >= 0); //最後一個大於零的index
            if (index_n > 50)
            {
                ranklist.AddRange(stockRanklist_dt.GetRange(0, 50));
            }
            else { ranklist.AddRange(stockRanklist_dt.GetRange(0, index_n)); }
            List<StockRankItem> buffer = stockRanklist_dt.GetRange(index_p, (stockRanklist_dt.Count - index_p));
            buffer.Reverse();
            if (buffer.Count > 0)
            {
                ranklist.AddRange(buffer.GetRange(0, 50));
            }
            else
            {
                ranklist.AddRange(buffer);
            }
            int num3 = ranklist.Count;

            dGV_StockRank.DataSource = ranklist;
        }

        /// <summary>
        /// 紀錄Log資訊
        /// </summary>
        /// <param name="message">輸入要建立階段的文字</param>
        private void log_time(string message) //筆記!!!
        {
            stopwatch.Stop();
            rtxt_ProcessStatus.Text = $"{rtxt_ProcessStatus.Text} {message}時間:{stopwatch.Elapsed.ToString(@"hh\:mm\:ss\:fff")}{Environment.NewLine}";
        }
    }
}