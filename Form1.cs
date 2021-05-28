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
        private string filePath = string.Empty; //建立讀檔路徑
        private Stopwatch stopwatch = new Stopwatch(); //待建立全域變數

        private StockRankItem stockRank;

        private bool datatype = false; //紀錄輸入框的狀態

        private string selected_Stock; //Combox的文字內容
        private List<StockItem> dt = new List<StockItem>(); //建立股票的所有資料，以List建立

        private List<StockInformation> stockInformation_dt = new List<StockInformation>();//建立搜尋資料陣列
        private List<StockItem> stockItems_dt = new List<StockItem>(); //建立搜尋資料用的列表陣列

        //以下BuyCellOver排序用
        private List<StockRankItem> stockRanklist_dt = new List<StockRankItem>(); //建立排名資料用的列表陣列

        private List<StockItem> stockRbufferList = new List<StockItem>();//RankBuffer

        /// <summary>
        /// group為整理好的資料
        /// </summary>

        private Dictionary<string, List<StockItem>> group = new Dictionary<string, List<StockItem>>();
        private StockInformation stockInformation; //建立用在搜尋欄位的物件

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
            stopwatch.Start();
            TimeSpan ts_dGV_List;//Datagridview 讀取計算時間
            using (StreamReader sr = new StreamReader(txtfile_address.Text, Encoding.GetEncoding("Big5")))
            {
                //建立欄位名稱
                sr.ReadLine(); //(思考其他寫法)//以下建立欄位
                string stockContent = sr.ReadLine();
                List<StockItem> allStock = new List<StockItem>();
                group.Add("All", allStock);
                while (!string.IsNullOrWhiteSpace(stockContent))
                {
                    StockItem stock = new StockItem(stockContent);

                    List<StockItem> groupId;
                    if (!group.TryGetValue(stock.StockID, out groupId))
                    {
                        groupId = new List<StockItem>();
                        group.Add(stock.StockID, groupId);
                    }
                    groupId.Add(stock);
                    //allStock.Add(stock);
                    stockContent = sr.ReadLine();
                }
                foreach (List<StockItem> item in group.Values)
                {
                    allStock.AddRange(item);
                }
                //dt.Sort((x, y) => x.StockID.CompareTo(y.StockID)); //新增ID排序 (記憶體占比沒差多少)
                dGV_List.DataSource = group["All"];
            }
            stopwatch.Stop();
            ts_dGV_List = stopwatch.Elapsed; //計算讀取完成的時間
            lb_Status.Text = "讀取完成";
        }

        /// <summary>
        /// 建立Combobox
        /// </summary>
        private void setupCombobox()
        {
            stopwatch.Restart();

            cbm_stocklist.DataSource = group.Select(data => $"{data.Key} - {data.Value[0].StockName}").ToList();

            stopwatch.Stop();
            TimeSpan ts_cbm_stocklist = stopwatch.Elapsed;

            //txt修改
            lb_Status.Text = "讀檔完成";

            //richbox修改
            //rtxt_ProcessStatus.Text = $"讀取時間: {ts_dGV_List}\nComboBox產生時間: {ts_cbm_stocklist}";
        }

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
                    filePath = openFileDialog.FileName;
                    txtfile_address.Text = openFileDialog.FileName;
                    //MessageBox.Show(filePath); //=>檢測用程式碼
                    readFile();
                    setupCombobox();
                }
            }

            //odf.ShowDialog();
        }

        /// <summary>
        /// 發現選單有更換，所觸發的事件
        /// </summary>
        /// <param name="sender">讀取盪案按鍵</param>
        /// <param name="e">讀取檔案室建</param>
        private void cbm_stocklist_SelectedIndexChanged(object sender, EventArgs e)
        {
            //MessageBox.Show($"{selected_Stock.ToString()}"); => 偵測用程式碼
            selected_Stock = cbm_stocklist.Text;
            datatype = false;
        }

        /// <summary>
        /// 發現選單有輸入文字，所觸發的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbm_stocklist_TextUpdate(object sender, EventArgs e)
        {
            selected_Stock = cbm_stocklist.Text;
            datatype = true;
        }

        /// <summary>
        /// 按下按鈕開始搜尋，判定搜尋條件，並建立清單
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStockSearch_Click(object sender, EventArgs e)
        {
            //Regex rergex = new Regex("^([A-z0-9]{4,},){0,}$");
            //以下是整理DataGridview
            if (datatype)
            {
                MessageBox.Show("Key in Word"); //=> 偵測用程式碼

                //先清空一次dGV_List資料源
                stockItems_dt.Clear();
                dGV_List.DataSource = null; //UI先清空
                //先清空一次dGV_Items資料源
                stockInformation_dt.Clear();
                dGV_Items.DataSource = null;
                string[] stockID_list = selected_Stock.Split(','); //分析內容
                foreach (string stockID in stockID_list)
                {
                    getstockInformation_Search(stockID);
                    stockInformation_dt.Add(stockInformation);
                    foreach (var item in group[stockID])
                    {
                        stockItems_dt.Add(item);
                    }
                }
                dGV_List.DataSource = stockItems_dt;
                dGV_Items.DataSource = stockInformation_dt;
            }
            else
            {
                MessageBox.Show("List Item Seach"); //=> 偵測用程式碼

                //先清空一次UI的資料源
                dGV_List.DataSource = null;
                dGV_Items.DataSource = null;

                //開始查詢資料
                string stockID = selected_Stock.Split(' ')[0];
                dGV_List.DataSource = group[stockID];

                //股票查詢資料更新更新
                //先清空一次資料源
                stockInformation_dt.Clear();
                getstockInformation_Search(stockID);
                stockInformation_dt.Add(stockInformation);

                //讀取新資料

                dGV_Items.DataSource = stockInformation_dt;
            }
        }

        /// <summary>
        /// 查詢股票的價位細節
        /// 以ID搜尋
        /// </summary>
        /// <param name="stockID"></param>
        /// <returns></returns>
        public void getstockInformation_Search(string stockID) //在想一下如何簡化
        {
            //以下為搜尋方法
            List<StockItem> stockItems = group[stockID];
            int buyTotal = 0; ;
            int cellTotal = 0;
            int buyCellOver = 0;
            int secBrokerCnt = 0;
            double avgPrice = 0;
            double sum = 0;
            List<string> secBrokerID_List = new List<string>();
            foreach (StockItem stock in stockItems)
            {
                buyTotal += int.Parse(stock.BuyQty);
                cellTotal += int.Parse(stock.CellQty);
                sum = double.Parse(stock.Price) * (int.Parse(stock.BuyQty) + int.Parse(stock.CellQty));
                if (!secBrokerID_List.Contains(stock.SecBrokerID))
                {
                    secBrokerID_List.Add(stock.SecBrokerID);
                }
            }
            avgPrice = sum / (buyTotal + cellTotal);
            buyCellOver = buyTotal - cellTotal;
            secBrokerCnt = secBrokerID_List.Count;
            stockInformation = new StockInformation(stockID, stockItems[0].StockName, buyTotal, cellTotal, avgPrice, buyCellOver, secBrokerCnt);
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

            MessageBox.Show($"{stockRanklist_dt.Count}");
            dGV_StockRank.DataSource = stockRanklist_dt;
        }

        private void mergesecBroker(string stockID)
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
                stockRank = new StockRankItem(stockName, secBroker, buyCellOver);
                stockRanklist_dt.Add(stockRank);
            }
        }

        private void btnMarketingRank_Click(object sender, EventArgs e)
        {
            ;
            if (datatype)
            {
                //先清空一次資料源
                stockRanklist_dt.Clear();
                dGV_StockRank.DataSource = null;

                //MessageBox.Show("Key in Word"); => 偵測用程式碼
                string[] stockID_List = selected_Stock.Split(',');
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
                string stockID = selected_Stock.Split(' ')[0];
                mergesecBroker(stockID);
                //排序與顯示前50筆資料
                sortRank();
            }
        }
    }
}