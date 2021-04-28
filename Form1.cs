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
        string content;
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
            //以下為讀取資料
            content = sr.ReadToEnd();//讀取整個csv檔
            //MessageBox.Show(content);顯示內容
            //MessageBox.Show(content.Split('\n')[0]);顯示第一行
            //以下建立欄位
            List<string> column_Name = new List<string>(content.Split('\n')[0].Split(','));
            

            for (int i = 0; i < column_Name.Count; i++)
            {
                dGV_List.Columns.Add(string.Empty,column_Name[i]);
            }

            int row = content.Split('\n').Length;//計算列數
            int column = content.Split('\n')[0].Split(',').Length;//計算欄數
            string[] row_content = content.Split('\n');
            string[] detail= new string[column];
            string[,] content_matrix = new string[row, column];
            string[] content_detail ;
            for (int i = 0; i < row; i++)
            {
                content_detail = row_content[i].Split(',');
                for (int j = 0; j < column; j++)
                {
                    content_matrix[i,j] = content_detail[j];
                }

            }





            for (int i = 1; i < row; i++)
            {
                detail = content.Split('\n')[i].Split(',');
                dGV_List.Rows.Add(detail);
            }


        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            odf.ShowDialog();
        }
    }
}
