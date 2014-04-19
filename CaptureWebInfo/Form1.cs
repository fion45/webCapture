using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using mshtml;
using SEOHelper;

namespace CaptureWebInfo
{
    public partial class Form1 : Form
    {
        private CSEOHelper mSEOHelper = new CSEOHelper();

        public Form1()
        {
            InitializeComponent();
            cap.Enabled = false;
        }

        private void Go_Click(object sender, EventArgs e)
        {
            //TODO:正则判断urlTB是否是网址格式
            switch(tabView.SelectedIndex) {
                case 0:
                    cap.Enabled = false;
                    wbView.Url = new Uri("http://" + urlTB.Text);
                    break;
                case 1:
                    mSEOHelper.Start("http://" + urlTB.Text);
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
    }
}
