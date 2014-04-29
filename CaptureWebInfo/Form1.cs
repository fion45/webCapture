using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;
using mshtml;
using SEOHelper;
using Model;
using Controller;
using System.Configuration;
using System.Net;
using System.Net.Sockets;

namespace CaptureWebInfo
{
    public partial class Form1 : Form
    {
        private CSEOHelper mSEOHelper = new CSEOHelper();
        private Timer mTimer = new Timer();
        private int mLVIndex = 0;
        private Configuration mConfig = null;
        private FileInfo mRegexFI = null;
        private CategoryController mCatCon = new CategoryController();
        private BrandController mBraCon = new BrandController();
        private ProductController mProCon = new ProductController();
        private NetworkMonitor mNWMon = NetworkMonitor.GetInstance();

        //Test
        //private List<MemoryStream> mSList = new List<MemoryStream>();
        private List<string[]> mRgxStrList = new List<string[]>();

        public Form1()
        {
            InitializeComponent();
            LoadConfig();

            cap.Enabled = false;
            mTimer.Interval = 1000;
            mTimer.Tick += mTimer_Tick;
        }

        private void LoadConfig()
        {
#if DEBUG
            string applicationName =
                Environment.GetCommandLineArgs()[0];
            applicationName = applicationName.Replace(".vshost.", ".");
#else
             string applicationName =
            Environment.GetCommandLineArgs()[0]+ ".exe";
#endif

            string exePath = System.IO.Path.Combine(Environment.CurrentDirectory, applicationName);
            string rgxFPath = System.IO.Path.Combine(Environment.CurrentDirectory, "Content.rgx");
            mRegexFI = new FileInfo(rgxFPath);
            if(mRegexFI.Exists)
            {
                FileStream tmpFS = mRegexFI.OpenRead();
                byte[] tmpB = new byte[tmpFS.Length];
                tmpFS.Read(tmpB, 0, (int)(tmpFS.Length));
                ContentTB.Text = Encoding.Unicode.GetString(tmpB);
                tmpFS.Close();
            }
            

            mConfig = ConfigurationManager.OpenExeConfiguration(exePath);
            urlTB.Text = ConfigurationManager.AppSettings["urlAddr"];
            AETB.Text = ConfigurationManager.AppSettings["matchAddr"];

        }

        void mTimer_Tick(object sender, EventArgs e)
        {
            long threadCount;
            int tc,wc,fc;
            mSEOHelper.GetHelperStatus(out threadCount,out tc, out wc, out fc);
            threadCountLB.Text = threadCount.ToString();
            WFCLB.Text = wc.ToString();
            FCLB.Text = fc.ToString();
            TCLB.Text = tc.ToString();
            List<ListViewItem> tmpArr = new List<ListViewItem>();
            int i = 0;
            for (; i < tc - mLVIndex; i++)
            {
                Visitor tmpV = mSEOHelper.GetVisitor(mLVIndex + i);
                if(tmpV == null)
                    break;
                ListViewItem tmpLVI = new ListViewItem(tmpV.mUrl);
                tmpArr.Add(tmpLVI);
            }
            mLVIndex += i;
            urlLV.Items.AddRange(tmpArr.ToArray());
            if (wc == 0)
            {
                //已完成
                mSEOHelper.Stop();
                mTimer.Stop();
                MessageBox.Show("have completed");
            }
            //double SSpeed, RSpeed;
            //mNWMon.GetNetworkState(out SSpeed, out RSpeed);
            //SCLB.Text = SSpeed.ToString();
            //RCLB.Text = RSpeed.ToString();
        }

        private void Go_Click(object sender, EventArgs e)
        {
            //TODO:正则判断urlTB是否是网址格式
            string tmpStr = urlTB.Text;
            Regex completeHeadTag = new Regex(Analyzer.COMPLETEHEADTAG,RegexOptions.IgnoreCase);
            Match tmpMatch = completeHeadTag.Match(tmpStr);
            if (!completeHeadTag.IsMatch(tmpStr))
                tmpStr = "http://" + tmpStr;
            urlTB.Text = tmpStr;
            switch(tabView.SelectedIndex) {
                case 0:
                    cap.Enabled = false;
                    wbView.Url = new Uri(tmpStr);
                    break;
                case 1:
                    mSEOHelper.Start(tmpStr, CheckUrlCB, DealWithContentCB);
                    mTimer.Start();
                    //mSList.Clear();
                    mRgxStrList.Clear();
                    tmpStr = ContentTB.Text;
                    string[] tSArr = tmpStr.Split(new char[] { '|' });
                    foreach (string tmpS in tSArr)
                    {
                        string[] tmpSArr = tmpS.Split(new char[] { ';' });
                        mRgxStrList.Add(tmpSArr);
                        //mSList.Add(new MemoryStream());
                    }
                    break;
                case 2:
                    break;
            }
        }

        private void wbView_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            cap.Enabled = true;
            urlTB.Text = e.Url.ToString();
            StartCapture();
        }

        string oldcolor = "";
        void hevent_onmouseover(IHTMLEventObj e)
        {
            nodeTV.Nodes.Clear();
            oldcolor = e.toElement.style.border;
            e.toElement.style.border = "2px solid red";
            //this.textBox2.Text = e.toElement.parentElement.innerHTML;
            string path = "";
            IHTMLElement element = e.toElement; 
            //textBox3.Text = element.className;
            string tmpStr;
            tmpStr = string.Format("{0}{1}{2}", element.tagName, string.IsNullOrEmpty(element.id) ? "" : ",id:" + element.id, string.IsNullOrEmpty(element.className) ? "" : ",class:" + element.className);
            TreeNode tmpTN = new TreeNode(tmpStr);
            while (element.parentElement != null)
            {
                IHTMLElementCollection elementCollection = element.parentElement.children as IHTMLElementCollection;
                path = element.tagName + "(" + elementCollection.length + ")/" + path;
                element = element.parentElement;
                tmpStr = string.Format("{0}{1}{2}", element.tagName, string.IsNullOrEmpty(element.id) ? "" : ",id:" + element.id, string.IsNullOrEmpty(element.className) ? "" : ",class:" + element.className);
                TreeNode tmpTN2 = new TreeNode(tmpStr);
                tmpTN2.Nodes.Add(tmpTN);
                tmpTN = tmpTN2;
            }
            //textBox3.Text = path;
            nodeTV.Nodes.Add(tmpTN);
            nodeTV.ExpandAll();
        }

        void hevent_onmouseout(IHTMLEventObj pEvtObj)

        {
            pEvtObj.fromElement.style.border = oldcolor;
        }

        void hevent_onmousedown(IHTMLEventObj pEvtObj)
        {
            StopCapture();
            pEvtObj.toElement.style.background = oldcolor;
        }

        private void cap_Click(object sender, EventArgs e)
        {
            wbView.Focus();
            StartCapture();
        }
        
        void StartCapture()
        {
            var htmlDoc = (IHTMLDocument3)wbView.Document.DomDocument;
            HTMLDocumentEvents2_Event hevent = (HTMLDocumentEvents2_Event)htmlDoc;
            hevent.onmouseout += new HTMLDocumentEvents2_onmouseoutEventHandler(hevent_onmouseout);
            hevent.onmouseover += new HTMLDocumentEvents2_onmouseoverEventHandler(hevent_onmouseover);
            hevent.onmousedown += new HTMLDocumentEvents2_onmousedownEventHandler(hevent_onmousedown);
        }

        void StopCapture()
        {
            mshtml.HTMLDocument doc1 = (mshtml.HTMLDocument)wbView.Document.DomDocument;
            mshtml.HTMLDocumentEvents2_Event hevent = (mshtml.HTMLDocumentEvents2_Event)doc1;
            hevent.onmouseover -= new mshtml.HTMLDocumentEvents2_onmouseoverEventHandler(this.hevent_onmouseover);
            hevent.onmouseout -= new HTMLDocumentEvents2_onmouseoutEventHandler(this.hevent_onmouseout);
            hevent.onmousedown -= new HTMLDocumentEvents2_onmousedownEventHandler(hevent_onmousedown);
        }

        private void wbView_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if(e.KeyValue == 27)
            {
                StopCapture();
            }
        }

        public bool CheckUrlCB(string url)
        {
            if(SHCB.Checked)
            {
                string parStr = urlTB.Text;
                Regex comEndTag = new Regex(Analyzer.COMENDTAG, RegexOptions.IgnoreCase);
                parStr = comEndTag.Replace(parStr, "");
                if (string.Compare(url.Substring(0, parStr.Length), parStr) != 0)
                {
                    return false;
                }
            }
            string tmpStr = AETB.Text;
            string[] tmpSArr = tmpStr.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            foreach(string tmpS in tmpSArr)
            {
                bool tag = true;
                string[] tmpSArr2 = tmpS.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                foreach(string tmpS2 in tmpSArr2)
                {
                    Regex tmpRgx = new Regex(tmpS2, RegexOptions.IgnoreCase);
                    if (!tmpRgx.IsMatch(url))
                    {
                        tag = false;
                        break;
                    }
                }
                if(tag)
                    return true;
            }
            return false;
        }

        public void DealWithContentCB(Visitor visitor,string response)
        {
            try
            {
                bool urlTag = false;
                //获得Brand图片
                string[] tmpStrArr = mRgxStrList[0];
                Regex mRgx = new Regex(tmpStrArr[0]);
                if (mRgx.IsMatch(visitor.mUrl))
                {
                    urlTag = true;
                    Regex cRgx = new Regex(tmpStrArr[1], RegexOptions.IgnoreCase | RegexOptions.Singleline);
                    MatchCollection tmpMC = cRgx.Matches(response);
                    foreach (Match match in tmpMC)
                    {
                        Group tmpG = match.Groups["Brand_Img"];
                        Group tmpG1 = match.Groups["Brand_Tag"];
                        Group tmpG2 = match.Groups["Brand_Name1"];
                        Group tmpG3 = match.Groups["Brand_Name2"];
                        if (!string.IsNullOrEmpty(match.Value))
                        {
                            for(int i=0;i<tmpG.Captures.Count;i++)
                            {
                                //获得牌子图片
                                string srcStr = tmpG.Captures[i].Value;
                                Analyzer.FillUrlString(ref srcStr, visitor.mUrl);
                                string srcLoc = string.Format("\brand\\{0}.{1}", tmpG1.Captures[i], srcStr.Substring(srcStr.LastIndexOf('.') + 1));
                                if (!File.Exists(Environment.CurrentDirectory + srcLoc))
                                    mSEOHelper.GetImg(srcStr, Environment.CurrentDirectory + srcLoc);
                                //加载牌子
                                Brand brand = new Brand();
                                brand.NameStr = tmpG2.Captures[i].Value.Trim();
                                brand.Name2 = tmpG3.Captures[i].Value.Trim();
                                brand.Tag = int.Parse(tmpG1.Value);
                                if (mBraCon.AddToMemory(brand))
                                {
                                    mBraCon.RefreshToDB();
                                }
                                else
                                {
                                    mBraCon.UpdateDBAndMemory(brand);
                                }
                            }
                        }
                    }
                }
                int CID = -1, BID = -1;
                Category tmpC = new Category();
                tmpC.Tag = 0;
                tmpC.NameStr = "首页";
                tmpC.ParCID = 1;
                int tag = tmpC.Tag;
                if (mCatCon.AddToMemory(tmpC))
                {
                    mCatCon.RefreshToDB();
                    CID = mCatCon.GetID(tag);
                    tmpC.ParCID = CID;
                    mCatCon.Set(tmpC);
                }
                CID = mCatCon.GetID(tag);
                //获得Category
                tmpStrArr = mRgxStrList[1];
                mRgx = new Regex(tmpStrArr[0]);
                if (mRgx.IsMatch(visitor.mUrl))
                {
                    urlTag = true;
                    Regex cRgx = new Regex(tmpStrArr[1], RegexOptions.IgnoreCase | RegexOptions.Singleline);
                    Match match = cRgx.Match(response);
                    Group tmpG = match.Groups["Category_Name"];
                    Group tmpG1 = match.Groups["Category_Tag"];
                    for (int i = 0; i < tmpG.Captures.Count; i++)
                    {
                        tmpC = new Category();
                        if (tag != -1)
                            tmpC.ParCID = mCatCon.GetID(tag);
                        tmpC.Tag = int.Parse(tmpG1.Captures[i].Value);
                        tmpC.NameStr = tmpG.Captures[i].Value;
                        if (mCatCon.AddToMemory(tmpC))
                        {
                            mCatCon.RefreshToDB();
                        }
                        tag = tmpC.Tag;
                    }
                    CID = mCatCon.GetID(tag);
                }
                //获得Brand
                tmpStrArr = mRgxStrList[2];
                mRgx = new Regex(tmpStrArr[0]);
                if (mRgx.IsMatch(visitor.mUrl))
                {
                    urlTag = true;
                    Regex cRgx = new Regex(tmpStrArr[1], RegexOptions.IgnoreCase | RegexOptions.Singleline);
                    Match match = cRgx.Match(response);
                    Group tmpG = match.Groups["Brand_Name"];
                    Group tmpG1 = match.Groups["Brand_Tag"];
                    if (!string.IsNullOrEmpty(match.Value))
                    {
                        Brand brand = new Brand();
                        brand.NameStr = tmpG.Value.Trim();
                        brand.Tag = int.Parse(tmpG1.Value);
                        brand.Name2 = brand.NameStr;
                        if (mBraCon.AddToMemory(brand))
                        {
                            mBraCon.RefreshToDB();
                        }
                        BID = mBraCon.GetID(brand.Tag);
                    }
                }
                Regex PTagRgx = new Regex("\\d+$");
                int ProductTag;
                tmpStrArr = mRgxStrList[3];
                mRgx = new Regex(tmpStrArr[0]);
                if (mRgx.IsMatch(visitor.mUrl) && int.TryParse(PTagRgx.Match(visitor.mUrl).Value, out ProductTag))
                {
                    urlTag = true;
                    if (mProCon.GetID(ProductTag) == -1)
                    {
                        //获得产品图片
                        Product product = new Product();
                        tmpStrArr = mRgxStrList[3];
                        mRgx = new Regex(tmpStrArr[0]);
                        if (mRgx.IsMatch(visitor.mUrl))
                        {
                            Regex cRgx = new Regex(tmpStrArr[1], RegexOptions.IgnoreCase | RegexOptions.Singleline);
                            Match match = cRgx.Match(response);
                            string tmpStr = "";
                            string tmpDStr = DateTime.Now.ToString("yyyy-MM-dd");
                            Group tmpG = match.Groups["Product_ImgPath"];
                            Regex dRgx = new Regex("/(?<year>\\d+)/(?<month>\\d+)/(?<day>\\d+)/(?<name>.+)$");
                            for (int i = 0; i < tmpG.Captures.Count; i++)
                            {
                                Match m1 = dRgx.Match(tmpG.Captures[i].Value);
                                if (i == 0 && !string.IsNullOrEmpty(m1.Value))
                                {
                                    string tmpMStr = "0" + m1.Groups["month"].Value;
                                    tmpMStr = tmpMStr.Substring(tmpMStr.Length - 2);
                                    string tmpDayStr = "0" + m1.Groups["day"].Value;
                                    tmpDayStr = tmpDayStr.Substring(tmpDayStr.Length - 2);
                                    tmpDStr = "20" + m1.Groups["year"].Value + "-" + tmpMStr + "-" + tmpDayStr;
                                }
                                string imgPath = Environment.CurrentDirectory + "/picture" + m1.Value;
                                imgPath = imgPath.Replace('/', '\\');
                                //获得产品的图片
                                if (!File.Exists(imgPath))
                                    mSEOHelper.GetImg(tmpG.Captures[i].Value, imgPath);
                                tmpStr += imgPath.Substring(Environment.CurrentDirectory.Length) + ";";
                            }
                            product.Date = tmpDStr;
                            product.ImgPath = tmpStr;
                        }
                        //获得产品参数
                        tmpStrArr = mRgxStrList[4];
                        mRgx = new Regex(tmpStrArr[0]);
                        if (mRgx.IsMatch(visitor.mUrl))
                        {
                            Regex cRgx = new Regex(tmpStrArr[1], RegexOptions.IgnoreCase | RegexOptions.Singleline);
                            Match match = cRgx.Match(response);
                            Group tmpG = match.Groups["Product_Title"];
                            product.Title = tmpG.Value.Trim();
                        }
                        tmpStrArr = mRgxStrList[5];
                        mRgx = new Regex(tmpStrArr[0]);
                        if (mRgx.IsMatch(visitor.mUrl))
                        {
                            Regex cRgx = new Regex(tmpStrArr[1], RegexOptions.IgnoreCase | RegexOptions.Singleline);
                            Match match = cRgx.Match(response);
                            Group tmpG2 = match.Groups["Product_Chose"];
                            product.Chose = tmpG2.Value.Trim();
                        }
                        //获得产品参数
                        tmpStrArr = mRgxStrList[6];
                        mRgx = new Regex(tmpStrArr[0]);
                        if (mRgx.IsMatch(visitor.mUrl))
                        {
                            Regex cRgx = new Regex(tmpStrArr[1], RegexOptions.IgnoreCase | RegexOptions.Singleline);
                            Match match = cRgx.Match(response);
                            if(!string.IsNullOrEmpty(match.Value))
                            {
                                Group tmpG = match.Groups["Product_Sale"];
                                Group tmpG2 = match.Groups["Product_Price"];
                                Group tmpG3 = match.Groups["Product_MarketPrice"];
                                decimal tmpF;
                                if (!decimal.TryParse(tmpG2.Value, out tmpF))
                                    Console.WriteLine("Error:Price Parse," + visitor.mUrl);
                                product.Price = tmpF;
                                if (!decimal.TryParse(tmpG3.Value, out tmpF))
                                    Console.WriteLine("Error:MarketPrice Parse," + visitor.mUrl);
                                product.MarketPrice = tmpF;
                                int tmpInt;
                                if(!int.TryParse(tmpG.Value, out tmpInt))
                                    Console.WriteLine("Error:Sale Count Parse," + visitor.mUrl);
                                product.Sale = tmpInt;
                            }
                            product.Tag = ProductTag;
                        }
                        //获得产品描述
                        tmpStrArr = mRgxStrList[7];
                        mRgx = new Regex(tmpStrArr[0]);
                        if (mRgx.IsMatch(visitor.mUrl))
                        {
                            Regex cRgx = new Regex(tmpStrArr[1], RegexOptions.IgnoreCase | RegexOptions.Singleline);
                            Regex hrefRgx = new Regex("\\shref\\s*?=\\s*?\"(?<href>[^\"]+?)\"", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                            Regex srcRgx = new Regex("\\ssrc\\s*?=\\s*?\"(?<src>[^\"]+?)\"", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                            Match match = cRgx.Match(response);
                            Group tmpG = match.Groups["Product_Descript"];
                            string DesStr = tmpG.Value.Trim();
                            //获得其中的图片
                            int HIndex = 0;
                            int tmpAC = 0;
                            MatchCollection tmpSrcMC = srcRgx.Matches(DesStr);
                            foreach (Match tmpSrcMA in tmpSrcMC)
                            {
                                string hrefStr = tmpSrcMA.Groups["src"].Value;
                                Analyzer.FillUrlString(ref hrefStr, visitor.mUrl);
                                string srcLoc = string.Format("\\product\\{0}\\{1}.{2}", ProductTag, HIndex, hrefStr.Substring(hrefStr.LastIndexOf('.') + 1));
                                if (!File.Exists(Environment.CurrentDirectory + srcLoc))
                                    mSEOHelper.GetImg(hrefStr, Environment.CurrentDirectory + srcLoc);
                                srcLoc = srcLoc.Replace('\\', '/');
                                srcLoc = " src=\"" + srcLoc + "\"";
                                DesStr = DesStr.Substring(0, tmpSrcMA.Index - tmpAC) + srcLoc + DesStr.Substring(tmpSrcMA.Index + tmpSrcMA.Value.Length - tmpAC);
                                tmpAC += tmpSrcMA.Value.Length - srcLoc.Length;
                                ++HIndex;
                            }
                            //过滤掉所有外链
                            product.Descript = DesStr;
                            tmpAC = 0;
                            string errLink = " href=\"errorLink.html\"";
                            MatchCollection tmpHrefMC = hrefRgx.Matches(DesStr);
                            foreach (Match tmpHrefMA in tmpHrefMC)
                            {
                                DesStr = DesStr.Substring(0, tmpHrefMA.Index - tmpAC) + errLink + DesStr.Substring(tmpHrefMA.Index + tmpHrefMA.Value.Length - tmpAC);
                                tmpAC += tmpHrefMA.Value.Length - errLink.Length;
                            }
                        }
                        product.CID = CID;
                        product.BrandID = BID;
                        if (mProCon.AddToMemory(product))
                        {
                            mProCon.RefreshToDB();
                        }
                    }
                    else
                    {
                        tmpStrArr = mRgxStrList[7];
                        //重新获得产品图片
                        mRgx = new Regex(tmpStrArr[0]);
                        if (mRgx.IsMatch(visitor.mUrl))
                        {
                            Regex cRgx = new Regex(tmpStrArr[1], RegexOptions.IgnoreCase | RegexOptions.Singleline);
                            Regex srcRgx = new Regex("\\ssrc\\s*?=\\s*?\"(?<src>[^\"]+?)\"", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                            Match match = cRgx.Match(response);
                            Group tmpG = match.Groups["Product_Descript"];
                            string DesStr = tmpG.Value.Trim();
                            //获得Descript其中的图片
                            int HIndex = 0;
                            MatchCollection tmpSrcMC = srcRgx.Matches(DesStr);
                            foreach (Match tmpSrcMA in tmpSrcMC)
                            {
                                string hrefStr = tmpSrcMA.Groups["src"].Value;
                                Analyzer.FillUrlString(ref hrefStr, visitor.mUrl);
                                string hrefLoc = string.Format("\\product\\{0}\\{1}.{2}", ProductTag, HIndex, hrefStr.Substring(hrefStr.LastIndexOf('.') + 1));
                                if (!File.Exists(Environment.CurrentDirectory + hrefLoc))
                                    mSEOHelper.GetImg(hrefStr, Environment.CurrentDirectory + hrefLoc);
                                ++HIndex;
                            }
                        }
                    }
                }
                if(urlTag)
                {
                    Url tmpUrl = new Url();
                    tmpUrl.Address = visitor.mUrl;
                    UrlController tmpUrlCon = new UrlController();
                    tmpUrlCon.Add(tmpUrl);
                }
            }
            catch (Exception ex)
            {
                string EStr = string.Format("Error:{0} [{1}\t{2}]", visitor.mUrl, ex.StackTrace, ex.Message);
                Console.WriteLine(EStr);
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (mConfig.AppSettings.Settings.AllKeys.Contains("urlAddr"))
                mConfig.AppSettings.Settings["urlAddr"].Value = urlTB.Text;
            else
                mConfig.AppSettings.Settings.Add("urlAddr", urlTB.Text);
            if (mConfig.AppSettings.Settings.AllKeys.Contains("matchAddr"))
                mConfig.AppSettings.Settings["matchAddr"].Value = AETB.Text;
            else
                mConfig.AppSettings.Settings.Add("matchAddr", AETB.Text);
            mConfig.Save(ConfigurationSaveMode.Full);
            if (mRegexFI.Exists)
            {
                mRegexFI.Delete();
            }
            FileStream tmpFS = mRegexFI.Create();
            byte[] tmpBuf = Encoding.Unicode.GetBytes(ContentTB.Text);
            tmpFS.Write(tmpBuf, 0, tmpBuf.Length);
            tmpFS.Close();
        }
    }
}
