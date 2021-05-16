using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlClient;

namespace Stock_Analysis
{
    public partial class Form1 : Form
    {
        OpenFileDialog odf = new OpenFileDialog();
        string fileAdress;
        //string stockItem;
        //string content;
        string columnName;
        //StockItem stock;


        BindingList<string[]> stockData = new BindingList<string[]>(); //建立List
        DataTable dt = new DataTable("StockTable"); //建立ＤataTable


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
            //MessageBox.Show(odf.FileName);
            txtfile_address.Text = odf.FileName;
            lblStatus.Text = "讀取中";
            FileStream file = new FileStream(txtfile_address.Text, FileMode.Open, FileAccess.Read,FileShare.None);
            StreamReader sr = new StreamReader(file, System.Text.Encoding.GetEncoding("Big5"));

            //建立欄位名稱
            columnName = sr.ReadLine();
            //以下建立欄位
            //List<string> column_Name = new List<string>(columnName.Split(','));
            String[] column_Name = columnName.Split(',');
            for (int i = 0; i < column_Name.Length; i++)
            {
                //dGV_List.Columns.Add(string.Empty, column_Name[i]);
                dt.Columns.Add(column_Name[i]);
            }

            //改用array測試
            /*columnName.Split(',')
                      .Select(data => data.Trim())
                      .ToList()
                      .ForEach(data => dGV_List.Columns.Add(string.Empty, data));*/




            /*for (int i = 0; i < 10; i++)
            {
                string stockContent = sr.ReadLine();
                StockItem stock = new StockItem(stockContent);
                DataRow row = dt.NewRow();
                for (int j = 0; j < column_Name.Length; j++)
                {
                    row[column_Name[j]] = stock.getStockItem()[j];
                }
                dt.Rows.Add(row);

            }*/

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
            }

            /*for (int i = 0; i < 1426428; i++)
            {
                string stockContent = sr.ReadLine();
                if (stockContent == null)
                {
                    break;
                }
                stock = new StockItem(stockContent);
                stockData.Add(stock.getStockItem());
                //dGV_List.Rows.Add(stock.getStockItem());              
            }*/

            dGV_List.DataSource = dt;



        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            odf.ShowDialog();
        }
    }
}
