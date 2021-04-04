
namespace Stock_Analysis
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.txtfile_address = new System.Windows.Forms.TextBox();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnOpenFile = new System.Windows.Forms.Button();
            this.btnStockSearch = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.btnMarketingRank = new System.Windows.Forms.Button();
            this.rtxt_ProcessStatus = new System.Windows.Forms.RichTextBox();
            this.dGV_List = new System.Windows.Forms.DataGridView();
            this.dGV_Items = new System.Windows.Forms.DataGridView();
            this.dGV_StockRank = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dGV_List)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dGV_Items)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dGV_StockRank)).BeginInit();
            this.SuspendLayout();
            // 
            // txtfile_address
            // 
            this.txtfile_address.Location = new System.Drawing.Point(12, 12);
            this.txtfile_address.Name = "txtfile_address";
            this.txtfile_address.Size = new System.Drawing.Size(661, 22);
            this.txtfile_address.TabIndex = 0;
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(12, 40);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(661, 22);
            this.txtSearch.TabIndex = 1;
            // 
            // btnOpenFile
            // 
            this.btnOpenFile.Location = new System.Drawing.Point(680, 12);
            this.btnOpenFile.Name = "btnOpenFile";
            this.btnOpenFile.Size = new System.Drawing.Size(75, 23);
            this.btnOpenFile.TabIndex = 2;
            this.btnOpenFile.Text = "讀取檔案";
            this.btnOpenFile.UseVisualStyleBackColor = true;
            this.btnOpenFile.Click += new System.EventHandler(this.btnOpenFile_Click);
            // 
            // btnStockSearch
            // 
            this.btnStockSearch.Location = new System.Drawing.Point(680, 40);
            this.btnStockSearch.Name = "btnStockSearch";
            this.btnStockSearch.Size = new System.Drawing.Size(75, 23);
            this.btnStockSearch.TabIndex = 3;
            this.btnStockSearch.Text = "股票查詢";
            this.btnStockSearch.UseVisualStyleBackColor = true;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(772, 17);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(53, 12);
            this.lblStatus.TabIndex = 4;
            this.lblStatus.Text = "讀取狀態";
            // 
            // btnMarketingRank
            // 
            this.btnMarketingRank.Location = new System.Drawing.Point(764, 40);
            this.btnMarketingRank.Name = "btnMarketingRank";
            this.btnMarketingRank.Size = new System.Drawing.Size(75, 23);
            this.btnMarketingRank.TabIndex = 5;
            this.btnMarketingRank.Text = "買賣超Top50";
            this.btnMarketingRank.UseVisualStyleBackColor = true;
            // 
            // rtxt_ProcessStatus
            // 
            this.rtxt_ProcessStatus.Location = new System.Drawing.Point(850, 12);
            this.rtxt_ProcessStatus.Name = "rtxt_ProcessStatus";
            this.rtxt_ProcessStatus.Size = new System.Drawing.Size(247, 51);
            this.rtxt_ProcessStatus.TabIndex = 6;
            this.rtxt_ProcessStatus.Text = "";
            // 
            // dGV_List
            // 
            this.dGV_List.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dGV_List.Location = new System.Drawing.Point(12, 78);
            this.dGV_List.Name = "dGV_List";
            this.dGV_List.RowTemplate.Height = 24;
            this.dGV_List.Size = new System.Drawing.Size(827, 267);
            this.dGV_List.TabIndex = 7;
            // 
            // dGV_Items
            // 
            this.dGV_Items.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dGV_Items.Location = new System.Drawing.Point(12, 351);
            this.dGV_Items.Name = "dGV_Items";
            this.dGV_Items.RowTemplate.Height = 24;
            this.dGV_Items.Size = new System.Drawing.Size(827, 221);
            this.dGV_Items.TabIndex = 8;
            // 
            // dGV_StockRank
            // 
            this.dGV_StockRank.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dGV_StockRank.Location = new System.Drawing.Point(854, 78);
            this.dGV_StockRank.Name = "dGV_StockRank";
            this.dGV_StockRank.RowTemplate.Height = 24;
            this.dGV_StockRank.Size = new System.Drawing.Size(242, 493);
            this.dGV_StockRank.TabIndex = 9;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1109, 584);
            this.Controls.Add(this.dGV_StockRank);
            this.Controls.Add(this.dGV_Items);
            this.Controls.Add(this.dGV_List);
            this.Controls.Add(this.rtxt_ProcessStatus);
            this.Controls.Add(this.btnMarketingRank);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.btnStockSearch);
            this.Controls.Add(this.btnOpenFile);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.txtfile_address);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.dGV_List)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dGV_Items)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dGV_StockRank)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtfile_address;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnOpenFile;
        private System.Windows.Forms.Button btnStockSearch;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Button btnMarketingRank;
        private System.Windows.Forms.RichTextBox rtxt_ProcessStatus;
        private System.Windows.Forms.DataGridView dGV_List;
        private System.Windows.Forms.DataGridView dGV_Items;
        private System.Windows.Forms.DataGridView dGV_StockRank;
    }
}

