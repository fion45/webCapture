using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using mshtml;
using SEOHelper;

namespace CaptureWebInfo
{
    public partial class Form1 : Form
    {
        private CSEOHelper mSEOHelper = new CSEOHelper();
        private Timer mTimer = new Timer();
        private int mLVIndex = 0;

        public Form1()
        {
            InitializeComponent();
            cap.Enabled = false;
            mTimer.Interval = 1000;
            mTimer.Tick += mTimer_Tick;
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
                    mSEOHelper.Start(tmpStr, CheckUrlCB);
                    mTimer.Start();
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
            string[] tmpSArr = tmpStr.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach(string tmpS in tmpSArr)
            {
                Regex tmpRgx = new Regex(tmpS);
                if (!tmpRgx.IsMatch(url))
                    return false;
            }
            return true;
        }
    }
}
