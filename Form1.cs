using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Stock_Analysis
{
    public partial class Form1 : Form
    {
        //private OpenFileDialog odf = new OpenFileDialog();

        private string columnName;

        private string text;
        private StockItem stock;
        private StockRankItem stockRank;

        //建立開啟檔案的物件
        private OpenFileDialog odf = new OpenFileDialog();

        private string selected_Stock; //Combox的文字內容
        private List<StockItem> dt = new List<StockItem>(); //建立股票的所有資料，以List建立

        private List<StockInformation> stockInformation_dt = new List<StockInformation>();//建立搜尋資料陣列
        private List<StockItem> stockItems_dt = new List<StockItem>(); //建立搜尋資料用的列表陣列

        //以下BuyCellOver排序用
        private List<StockRankItem> stockRanklist_dt = new List<StockRankItem>(); //建立排名資料用的列表陣列

        private List<StockItem> stockRbufferList = new List<StockItem>();//RankBuffer

        public Form1()
        {
            InitializeComponent();
            //初始化OpenFileDialog
            odf.AddExtension = true;
            odf.CheckFileExists = true;
            odf.Filter = "csv檔|*.csv|純文字檔|*.txt|所有檔案|*.*";
            odf.FilterIndex = 0;
            odf.FileName = String.Empty;
            odf.InitialDirectory = @"C:\";
            odf.Multiselect = false;
            odf.RestoreDirectory = true;

            odf.ShowReadOnly = true;
            odf.Title = "請選取股票資料";
            odf.FileOk += new CancelEventHandler(odf_FileOk); //查一下用法
        }

        private void odf_FileOk(object sender, CancelEventArgs e)
        {
            //開始計時
            Stopwatch sw = new Stopwatch();
            sw.Start();

            //MessageBox.Show(odf.FileName);
            txtfile_address.Text = odf.FileName;
            lb_Status.Text = "讀取中";
            FileStream file = new FileStream(txtfile_address.Text, FileMode.Open, FileAccess.Read, FileShare.None);
            StreamReader sr = new StreamReader(file, System.Text.Encoding.GetEncoding("Big5"));

            //建立欄位名稱
            columnName = sr.ReadLine(); //(思考其他寫法)
            //以下建立欄位
            string stockContent = sr.ReadLine();
            while (!string.IsNullOrWhiteSpace(stockContent))
            {
                stock = new StockItem(stockContent);
                dt.Add(stock);
                stockContent = sr.ReadLine();
            }
            dGV_List.DataSource = dt;

            sw.Stop();
            TimeSpan ts_dGV_List = sw.Elapsed;

            //combobox
            sw.Restart();

            //建立combolist
            List<string> stocklist = new List<string>();
            Hashtable htb = new Hashtable(); //改Dictoinary
            foreach (StockItem item in dt)
            {
                if (htb.Contains(item.StockID))
                {
                    continue;
                }
                htb.Add(item.StockID, item.StockName);
            }
            foreach (DictionaryEntry item in htb)
            {
                string name = $"{item.Key} - {item.Value}";
                stocklist.Add(name);
            }
            cbm_stocklist.DataSource = stocklist;

            //cbm_stocklist.SelectedIndex = -1;

            //cbm_stocklist.DataSource = dt; //(ComboBox的問題在想一下，運行較久)
            //cbm_stocklist.DisplayMember =

            sw.Stop();
            TimeSpan ts_cbm_stocklist = sw.Elapsed;

            //txt修改
            lb_Status.Text = "讀檔完成";

            //richbox修改
            rtxt_ProcessStatus.Text = $"讀取時間: {ts_dGV_List}\nComboBox產生時間: {ts_cbm_stocklist}";
        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            List<StockItem> a = new List<StockItem>();

            Dictionary<string, List<StockItem>> dictionary = new Dictionary<string, List<StockItem>>();
            dictionary.Add("1234", a);

            odf.ShowDialog();
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
        }

        /// <summary>
        /// 發現選單有輸入文字，所觸發的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbm_stocklist_TextUpdate(object sender, EventArgs e)
        {
            selected_Stock = cbm_stocklist.Text;
        }

        /// <summary>
        /// 按下按鈕開始搜尋，判定搜尋條件，並建立清單
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStockSearch_Click(object sender, EventArgs e)
        {
            Regex rergex = new Regex("^([A-9]{4,},){0,}$");
            if (rergex.IsMatch(selected_Stock))
            {
                MessageBox.Show("Key in Word"); //=> 偵測用程式碼
                //先清空一次資料源
                //stockItems_dt.Clear();
                //dGV_List.DataSource = null;

                //string[] items = selected_Stock.Split(',');
                //foreach (string item in items)
                //{
                //    foreach (StockItem stock in dt)
                //    {
                //        if (stock.StockID.Equals(item))
                //        {
                //            stockItems_dt.Add(stock);
                //        }
                //    }
                //}

                //dGV_List.DataSource = stockItems_dt;
            }
            else
            {
                MessageBox.Show("List Item Seach"); //=> 偵測用程式碼

                //先清空一次資料源
                //stockItems_dt.Clear();
                //dGV_List.DataSource = null;

                //開始查詢資料
                //string stockID = selected_Stock.Split(' ')[0];

                //foreach (StockItem stock in dt)
                //{
                //    if (stock.StockID.Equals(stockID))
                //    {
                //        stockItems_dt.Add(stock);
                //    }
                //}
                //dGV_List.DataSource = stockItems_dt;

                //股票查詢資料更新更新
                //先清空一次資料源
                //stockInformation_dt.Clear();
                //dGV_Items.DataSource = null;

                //讀取新資料
                //getstockInformation_Search(stockID);
                //dGV_Items.DataSource = stockInformation_dt;
            }
        }

        /// <summary>
        /// 查詢股票的價位細節
        /// 以ID搜尋
        /// </summary>
        /// <param name="stock"></param>
        /// <returns></returns>
        public void getstockInformation_Search(string stock)
        {
            //以下為搜尋方法
            string stockID = stock;
            string stockName = getstockName(stock);
            int buytotal = getBuyTotal(stockID);
            int celltotal = getCellTotal(stockID);
            double avgprice = getAvgPrice(getstockPrice(stockID), buytotal, celltotal);
            int buycellover = (buytotal - celltotal);
            int secbrokercnt = getSecBrokerCnt(stockID);
            StockInformation stockInformation = new StockInformation(stock, stockName, buytotal, celltotal, avgprice, buycellover, secbrokercnt);
            stockInformation_dt.Add(stockInformation);
        }

        /// <summary>
        /// 搜尋股票名稱
        /// </summary>
        /// <param name="stockID"></param>
        /// <returns></returns>
        public string getstockName(string stockID)
        {
            string stockName = string.Empty;
            foreach (StockItem item in dt)
            {
                if (item.StockID.Equals(stockID))
                {
                    stockName = item.StockName;
                }
            }
            return stockName;
        }

        /// <summary>
        /// 搜尋股票價格
        /// </summary>
        /// <param name="stockID"></param>
        /// <returns></returns>
        public double getstockPrice(string stockID)
        {
            double StockPrice = 0;

            foreach (StockItem item in dt)
            {
                if (item.StockID.Equals(stockID))
                {
                    StockPrice = double.Parse(item.Price);
                }
            }
            return StockPrice;
        }

        /// <summary>
        /// 搜尋股票購買總數
        /// </summary>
        /// <param name="stockID"></param>
        /// <returns></returns>
        public int getBuyTotal(string stockID)
        {
            int BuyTotal = 0;
            foreach (StockItem item in dt)
            {
                //MessageBox.Show($"{item.StockID}\n");
                if (item.StockID.Equals(stockID))
                {
                    BuyTotal += int.Parse(item.BuyQty);
                }
            }

            return BuyTotal;
        }

        /// <summary>
        /// 搜尋股票賣出總數
        /// </summary>
        /// <param name="stockID"></param>
        /// <returns></returns>
        public int getCellTotal(string stockID)
        {
            int CellTotal = 0;
            foreach (StockItem item in dt)
            {
                if (item.StockID.Equals(stockID))
                {
                    CellTotal += int.Parse(item.CellQty);
                }
            }

            return CellTotal;
        }

        /// <summary>
        /// 計算股票平均價格
        /// </summary>
        /// <param name="price"></param>
        /// <param name="buyTotal"></param>
        /// <param name="cellTotal"></param>
        /// <returns></returns>
        public double getAvgPrice(double price, int buyTotal, int cellTotal)
        {
            double AvgPrice = (price * buyTotal + price * cellTotal) / (buyTotal + cellTotal);
            return AvgPrice;
        }

        /// <summary>
        /// 計算股票賣超
        /// </summary>
        /// <param name="buyTotal"></param>
        /// <param name="cellTotal"></param>
        /// <returns></returns>
        public int getBuyCellOver(int buyTotal, int cellTotal) //
        {
            return (buyTotal - cellTotal);
        }

        /// <summary>
        /// 統計券商代號得總類數目
        /// </summary>
        /// <param name="stockID"></param>
        /// <returns></returns>
        public int getSecBrokerCnt(string stockID)
        {
            List<string> secBrokerID_list = new List<string>();
            foreach (StockItem item in dt)
            {
                if (item.StockID.Equals(stockID))
                {
                    if (!secBrokerID_list.Contains(item.SecBrokerID))
                    {
                        secBrokerID_list.Add(item.SecBrokerID);
                    }
                }
            }
            return secBrokerID_list.Count;
        }

        private void btnMarketingRank_Click(object sender, EventArgs e)
        {
            Regex rergex = new Regex("^([A-Za-z0-9]{4,},){0,}$");
            if (rergex.IsMatch(selected_Stock))
            {
                //先清空一次資料源
                stockRanklist_dt.Clear();
                dGV_StockRank.DataSource = null;

                //MessageBox.Show("Key in Word"); => 偵測用程式碼
                string[] items = selected_Stock.Split(',');
                foreach (string item in items)
                {
                    foreach (StockItem stock in dt)
                    {
                        if (stock.StockID.Equals(item))
                        {
                            stockItems_dt.Add(stock);
                        }
                    }
                }

                dGV_List.DataSource = stockItems_dt;
            }
            else
            {
                //先清空一次資料源
                stockRanklist_dt.Clear();
                dGV_StockRank.DataSource = null;

                //開始查詢資料
                string stockID = selected_Stock.Split(' ')[0];

                //先濾出股票ID
                foreach (StockItem stock in dt)
                {
                    if (stock.StockID.Equals(stockID))
                    {
                        stockRbufferList.Add(stock);
                    }
                }

                //建立SecBrokerlist
                List<string> secBrokerlist = new List<string>();
                foreach (StockItem stock in stockRbufferList)
                {
                    if (!secBrokerlist.Contains(stock.SecBrokerName))
                    {
                        secBrokerlist.Add((stock.SecBrokerName));
                    }
                }

                //合併相同的SecBrokerID

                foreach (string secBroker in secBrokerlist) //先取secBroker
                {
                    string stockName = string.Empty;
                    int buyTotal = 0;
                    int cellTotal = 0;
                    int buyCellOver = 0;
                    foreach (StockItem item in stockRbufferList)//比對各項
                    {
                        stockName = item.StockName;
                        if (item.SecBrokerName.Equals(secBroker))
                        {
                            buyTotal += int.Parse(item.BuyQty);
                            cellTotal += int.Parse(item.CellQty);
                        }
                    }
                    buyCellOver = buyTotal - cellTotal;
                    stockRank = new StockRankItem(stockName, secBroker, buyCellOver);
                    stockRanklist_dt.Add(stockRank);
                }

                //排序與顯示前50筆資料
                stockRanklist_dt.Sort((x, y) => -x.BuyCellOver.CompareTo(y.BuyCellOver));
                stockRanklist_dt.RemoveRange(50, (stockRanklist_dt.Count - 50));
                MessageBox.Show($"{stockRanklist_dt.Count}");
                dGV_StockRank.DataSource = stockRanklist_dt;
            }
        }
    }
}