namespace CaptureWebInfo
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.tabView = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.eleLV = new System.Windows.Forms.ListView();
            this.regTB = new System.Windows.Forms.TextBox();
            this.cap = new System.Windows.Forms.Button();
            this.nodeTV = new System.Windows.Forms.TreeView();
            this.wbView = new System.Windows.Forms.WebBrowser();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label8 = new System.Windows.Forms.Label();
            this.ContentTB = new System.Windows.Forms.TextBox();
            this.SHCB = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.AETB = new System.Windows.Forms.TextBox();
            this.TCLB = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.FCLB = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.WFCLB = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.threadCountLB = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.urlLV = new System.Windows.Forms.ListView();
            this.resultTP = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.urlTB = new System.Windows.Forms.TextBox();
            this.Go = new System.Windows.Forms.Button();
            this.tabView.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabView
            // 
            this.tabView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabView.Controls.Add(this.tabPage1);
            this.tabView.Controls.Add(this.tabPage2);
            this.tabView.Controls.Add(this.resultTP);
            this.tabView.Location = new System.Drawing.Point(12, 37);
            this.tabView.Name = "tabView";
            this.tabView.SelectedIndex = 0;
            this.tabView.Size = new System.Drawing.Size(868, 572);
            this.tabView.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.button1);
            this.tabPage1.Controls.Add(this.eleLV);
            this.tabPage1.Controls.Add(this.regTB);
            this.tabPage1.Controls.Add(this.cap);
            this.tabPage1.Controls.Add(this.nodeTV);
            this.tabPage1.Controls.Add(this.wbView);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(860, 546);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "网页";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(223, 464);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 9;
            this.label2.Text = "正则：";
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(672, 459);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // eleLV
            // 
            this.eleLV.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.eleLV.Location = new System.Drawing.Point(223, 486);
            this.eleLV.Name = "eleLV";
            this.eleLV.Size = new System.Drawing.Size(524, 54);
            this.eleLV.TabIndex = 7;
            this.eleLV.UseCompatibleStateImageBehavior = false;
            // 
            // regTB
            // 
            this.regTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.regTB.Location = new System.Drawing.Point(262, 459);
            this.regTB.Name = "regTB";
            this.regTB.Size = new System.Drawing.Size(404, 21);
            this.regTB.TabIndex = 6;
            // 
            // cap
            // 
            this.cap.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cap.Location = new System.Drawing.Point(753, 459);
            this.cap.Name = "cap";
            this.cap.Size = new System.Drawing.Size(104, 84);
            this.cap.TabIndex = 5;
            this.cap.Text = "Capture";
            this.cap.UseVisualStyleBackColor = true;
            this.cap.Click += new System.EventHandler(this.cap_Click);
            // 
            // nodeTV
            // 
            this.nodeTV.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.nodeTV.Indent = 5;
            this.nodeTV.Location = new System.Drawing.Point(0, 0);
            this.nodeTV.Name = "nodeTV";
            this.nodeTV.Size = new System.Drawing.Size(217, 540);
            this.nodeTV.TabIndex = 1;
            // 
            // wbView
            // 
            this.wbView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.wbView.Location = new System.Drawing.Point(223, 0);
            this.wbView.MinimumSize = new System.Drawing.Size(20, 20);
            this.wbView.Name = "wbView";
            this.wbView.Size = new System.Drawing.Size(637, 453);
            this.wbView.TabIndex = 0;
            this.wbView.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.wbView_DocumentCompleted);
            this.wbView.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.wbView_PreviewKeyDown);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label8);
            this.tabPage2.Controls.Add(this.ContentTB);
            this.tabPage2.Controls.Add(this.SHCB);
            this.tabPage2.Controls.Add(this.label7);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.AETB);
            this.tabPage2.Controls.Add(this.TCLB);
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Controls.Add(this.FCLB);
            this.tabPage2.Controls.Add(this.label9);
            this.tabPage2.Controls.Add(this.WFCLB);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.threadCountLB);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.urlLV);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(860, 546);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "操作";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(541, 197);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(305, 12);
            this.label8.TabIndex = 17;
            this.label8.Text = "获取内容规则（[url正则;内容1正则;内容2正则...]）：";
            // 
            // ContentTB
            // 
            this.ContentTB.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ContentTB.Location = new System.Drawing.Point(540, 212);
            this.ContentTB.Multiline = true;
            this.ContentTB.Name = "ContentTB";
            this.ContentTB.Size = new System.Drawing.Size(314, 338);
            this.ContentTB.TabIndex = 15;
            this.ContentTB.Text = "/Product/List/\\?category=\\d+;<div[^<]*?\\sid\\s*=\\s*\\\"*currentPlace\\\"*[^<>]+?>[^<>]" +
    "*?(<a\\s[^<>]+?>([^<>]+?)</a>[^<>]*)+?</div>|/Product/Details/\\d+;";
            // 
            // SHCB
            // 
            this.SHCB.AutoSize = true;
            this.SHCB.Location = new System.Drawing.Point(540, 36);
            this.SHCB.Name = "SHCB";
            this.SHCB.Size = new System.Drawing.Size(84, 16);
            this.SHCB.TabIndex = 14;
            this.SHCB.Text = "同一个域名";
            this.SHCB.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(538, 64);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(131, 12);
            this.label7.TabIndex = 13;
            this.label7.Text = "URL满足的正则表达式：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(536, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 12;
            this.label4.Text = "地址规则";
            // 
            // AETB
            // 
            this.AETB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AETB.Location = new System.Drawing.Point(538, 79);
            this.AETB.Multiline = true;
            this.AETB.Name = "AETB";
            this.AETB.Size = new System.Drawing.Size(316, 108);
            this.AETB.TabIndex = 11;
            this.AETB.Text = "product";
            // 
            // TCLB
            // 
            this.TCLB.AutoSize = true;
            this.TCLB.Location = new System.Drawing.Point(439, 9);
            this.TCLB.Name = "TCLB";
            this.TCLB.Size = new System.Drawing.Size(11, 12);
            this.TCLB.TabIndex = 10;
            this.TCLB.Text = "0";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(392, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 12);
            this.label6.TabIndex = 9;
            this.label6.Text = "总数：";
            // 
            // FCLB
            // 
            this.FCLB.AutoSize = true;
            this.FCLB.Location = new System.Drawing.Point(323, 9);
            this.FCLB.Name = "FCLB";
            this.FCLB.Size = new System.Drawing.Size(11, 12);
            this.FCLB.TabIndex = 8;
            this.FCLB.Text = "0";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(264, 9);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 12);
            this.label9.TabIndex = 7;
            this.label9.Text = "已完成：";
            // 
            // WFCLB
            // 
            this.WFCLB.AutoSize = true;
            this.WFCLB.Location = new System.Drawing.Point(199, 9);
            this.WFCLB.Name = "WFCLB";
            this.WFCLB.Size = new System.Drawing.Size(11, 12);
            this.WFCLB.TabIndex = 4;
            this.WFCLB.Text = "0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(152, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 3;
            this.label5.Text = "待读：";
            // 
            // threadCountLB
            // 
            this.threadCountLB.AutoSize = true;
            this.threadCountLB.Location = new System.Drawing.Point(97, 9);
            this.threadCountLB.Name = "threadCountLB";
            this.threadCountLB.Size = new System.Drawing.Size(11, 12);
            this.threadCountLB.TabIndex = 2;
            this.threadCountLB.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(2, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 12);
            this.label3.TabIndex = 1;
            this.label3.Text = "线程池线程数：";
            // 
            // urlLV
            // 
            this.urlLV.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.urlLV.Location = new System.Drawing.Point(-4, 28);
            this.urlLV.Name = "urlLV";
            this.urlLV.Size = new System.Drawing.Size(536, 522);
            this.urlLV.TabIndex = 0;
            this.urlLV.TileSize = new System.Drawing.Size(536, 28);
            this.urlLV.UseCompatibleStateImageBehavior = false;
            this.urlLV.View = System.Windows.Forms.View.Tile;
            // 
            // resultTP
            // 
            this.resultTP.Location = new System.Drawing.Point(4, 22);
            this.resultTP.Name = "resultTP";
            this.resultTP.Size = new System.Drawing.Size(860, 546);
            this.resultTP.TabIndex = 2;
            this.resultTP.Text = "结果";
            this.resultTP.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "URL地址:";
            // 
            // urlTB
            // 
            this.urlTB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.urlTB.Location = new System.Drawing.Point(75, 10);
            this.urlTB.Name = "urlTB";
            this.urlTB.Size = new System.Drawing.Size(724, 21);
            this.urlTB.TabIndex = 2;
            this.urlTB.Text = "http://www.usashopcn.com";
            // 
            // Go
            // 
            this.Go.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Go.Location = new System.Drawing.Point(805, 8);
            this.Go.Name = "Go";
            this.Go.Size = new System.Drawing.Size(75, 23);
            this.Go.TabIndex = 3;
            this.Go.Text = "Go";
            this.Go.UseVisualStyleBackColor = true;
            this.Go.Click += new System.EventHandler(this.Go_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(892, 621);
            this.Controls.Add(this.Go);
            this.Controls.Add(this.urlTB);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tabView);
            this.Name = "Form1";
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.tabView.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabView;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox urlTB;
        private System.Windows.Forms.Button Go;
        private System.Windows.Forms.WebBrowser wbView;
        private System.Windows.Forms.TreeView nodeTV;
        private System.Windows.Forms.Button cap;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListView eleLV;
        private System.Windows.Forms.TextBox regTB;
        private System.Windows.Forms.TabPage resultTP;
        private System.Windows.Forms.ListView urlLV;
        private System.Windows.Forms.Label threadCountLB;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label FCLB;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label WFCLB;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label TCLB;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox AETB;
        private System.Windows.Forms.CheckBox SHCB;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox ContentTB;
    }
}

