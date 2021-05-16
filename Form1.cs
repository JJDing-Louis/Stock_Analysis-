using System;
using System.ComponentModel;
using System.Data;
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
        private DataTable dt = new DataTable("StockTable"); //建立ＤataTable

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

            dGV_List.DataSource = dt;
            sw.Stop();
            TimeSpan ts_dGV_List = sw.Elapsed;

            //combobox
            sw.Restart();
            cbm_stocklist.DisplayMember = $"{column_Name[1]}";
            cbm_stocklist.ValueMember = $"{column_Name[2]}";

            cbm_stocklist.DataSource = dt;
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

        }
    }
}