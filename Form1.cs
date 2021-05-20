using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace Stock_Analysis
{
    public partial class Form1 : Form
    {
        private OpenFileDialog odf = new OpenFileDialog();
        private string fileAdress;
        private string columnName;

        //private DataTable dt = new DataTable("StockTable"); //建立ＤataTable
        private List<StockItem> dt = new List<StockItem>(); //建立股票的所有資料，以List建立

        private StockItem stock;
        private Object selected_Stock;

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
            odf.FileOk += new CancelEventHandler(odf_FileOk);
        }

        private void odf_FileOk(object sender, CancelEventArgs e)
        {
            //開始計時
            Stopwatch sw = new Stopwatch();
            sw.Start();

            //MessageBox.Show(odf.FileName);
            txtfile_address.Text = odf.FileName;
            lblStatus.Text = "讀取中";
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
            /*
            while (true)
            {
                string stockContent = sr.ReadLine();
                if (stockContent == null)
                {
                    break;
                }
                StockItem stock = new StockItem(stockContent);
                DataRow row = dt.NewRow();
                for (int j = 0; j < column_Name.Length; j++)
                {
                    row[column_Name[j]] = stock.getStockItem()[j];
                }
                dt.Rows.Add(row);
            }*/

            sw.Stop();
            TimeSpan ts_dGV_List = sw.Elapsed;

            //combobox
            sw.Restart();

            //建立combolist
            List<string> stocklist = new List<string>();
            Hashtable htb = new Hashtable();
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
            lblStatus.Text = "讀檔完成";

            //richbox修改
            rtxt_ProcessStatus.Text = $"讀取時間: {ts_dGV_List}\nComboBox產生時間: {ts_cbm_stocklist}";
        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            odf.ShowDialog();
        }

        private void cbm_stocklist_SelectedIndexChanged(object sender, EventArgs e)
        {
            selected_Stock = cbm_stocklist.SelectedItem;
            //MessageBox.Show($"{selected_Stock.ToString()}");
        }

        private void btnStockSearch_Click(object sender, EventArgs e)
        {
            int stockID = getstock_Search(selected_Stock);
            int buytotal = getBuyTotal(stockID);
            int celltotal = getCellTotal(stockID) ;
            //double avgprice = getAvgPrice(, buytotal, celltotal);
            int buycellover = (buytotal - celltotal);
            int secbrokercnt = getSecBrokerCnt(stockID);
            MessageBox.Show($"BuyTotal:{buytotal}\nCellTotal:{celltotal}\nAvgPrice:{celltotal}\nBuyCellOver:{buycellover}\nSecbrokerCnt:{secbrokercnt}");
        }

        //建立搜尋方法
        public int getstock_Search(Object stock) 
        {
            //以下為搜尋方法
            String[] stock_Detail = stock.ToString().Split(' ');
            int stockID = int.Parse(stock_Detail[0]);
            string stockName = stock_Detail[2];
            //MessageBox.Show($"股票代號:{stockID}\n股票名稱:{stockName}");

            return stockID;
        }


        public int getBuyTotal(int stockID)
        {
            int BuyTotal = 0;
            foreach (StockItem item in dt)
            {
                //MessageBox.Show($"{item.StockID}\n");
                if ((int.Parse(item.StockID)) == stockID)
                {
                    BuyTotal += int.Parse(item.BuyQty);
                }
            }

            return BuyTotal;
        }

        public int getCellTotal(int stockID)
        {
            int CellTotal = 0;
            foreach (StockItem item in dt)
            {
                if ((int.Parse(item.StockID)) == stockID)
                {
                    CellTotal += int.Parse(item.CellQty);
                }
            }

            return CellTotal;
        }

        public double getAvgPrice(int price, int buyTotal, int cellTotal) //再思考傳入的參數1
        {
            double AvgPrice = (price * buyTotal + price * cellTotal) / (buyTotal + cellTotal);
            return AvgPrice;
        }

        public int getBuyCellOver(int buyTotal, int cellTotal) //
        {
            return (buyTotal - cellTotal);
        }

        public int getSecBrokerCnt(int stockID)
        {
            int count = 0;
            foreach (StockItem item in dt)
            {
                if (int.Parse(item.StockID) == stockID)
                {
                    count += 1;
                }
            }

            return count;
        }
    }
}