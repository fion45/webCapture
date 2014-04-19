using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text.RegularExpressions;

namespace SEOHelper
{
    public class CSEOHelper
    {
        public delegate bool CheckUrlCB(string url);

        private CheckUrlCB mCUCB;

        public enum EHelperStatus
        {
            Init,
            Running,
            Paused,
            Stoped
        }
        struct SAsynObj
        {
            public HttpWebRequest mRequest;
            public Visitor mVisitor;
        }

        private VisitorManager mVManager;
        private EHelperStatus mRunStatus;
        public ManualResetEvent mContinue;

        public CSEOHelper()
        {
            mContinue = new ManualResetEvent(true);
            mVManager = new VisitorManager();
        }

        //public ~CSEOHelper()
        //{
        //    Stop();
        //}

        public void Start(string url,CheckUrlCB cuCB = null)
        {
            mCUCB = cuCB;
            mContinue.Set();
            mRunStatus = EHelperStatus.Running;
            ThreadPool.QueueUserWorkItem(RunCallback);
            mVManager.AddVisitor(url,null);
        }

        public void RunCallback(Object state)
        {
            while(mRunStatus != EHelperStatus.Stoped)
            {
                if(mContinue.WaitOne(3000) && mVManager.mEvent.WaitOne(3000))
                {
                    Visitor tmpV = mVManager.GetInitVisitor();
                    ThreadPool.QueueUserWorkItem(WaitCallback,tmpV);
                }
            }
        }

        public void Pause()
        {
            mRunStatus = EHelperStatus.Paused;
            mContinue.Reset();
        }

        public void Stop()
        {
            mRunStatus = EHelperStatus.Stoped;
        }

        public void WaitCallback(Object state)
        {
            Visitor tmpVisitor = (Visitor)state;
            try
            {
                HttpWebRequest tmpRequest = (HttpWebRequest)HttpWebRequest.Create(tmpVisitor.mUrl);
                SAsynObj tmpObj = new SAsynObj
                {
                    mRequest = tmpRequest,
                    mVisitor = tmpVisitor
                };
                tmpRequest.BeginGetResponse(new AsyncCallback(GetResponseCallback), tmpObj);
                tmpVisitor.mStatus = Visitor.EVisitStatus.Visiting;
            }
            catch(Exception ex)
            {
                string EStr = string.Format("{0} [{1}]", tmpVisitor.mUrl, ex.Message);
                Console.WriteLine(EStr);
            }
        }

        public void GetResponseCallback(IAsyncResult asynchronousResult)
        {
            SAsynObj tmpObj = (SAsynObj)asynchronousResult.AsyncState;
            Visitor visitor = tmpObj.mVisitor;
            HttpWebRequest request = tmpObj.mRequest;
            // End the operation
            HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(asynchronousResult);
            Stream streamResponse = response.GetResponseStream();
            StreamReader streamRead = new StreamReader(streamResponse);
            string responseString = streamRead.ReadToEnd();
            //找出<a> 和 需要的内容
            List<string> tmpList = Analyzer.GetHtmlNode(responseString, "a");
            Regex urlRgx = new Regex(Analyzer.GetPropertyRegexStr("href"),RegexOptions.IgnoreCase);
            Regex StartTagRgx = new Regex(Analyzer.STRSTARTTAG);
            Regex EndTagRgx = new Regex(Analyzer.STRENDTAG);
            foreach (string hStr in tmpList)
            {
                //Console.WriteLine(hStr);
                if(urlRgx.IsMatch(hStr))
                {
                    MatchCollection tmpMC = urlRgx.Matches(hStr);
                    for(int i=1;i<4;i++)
                    {
                        string tmpUrlStr = tmpMC[tmpMC.Count - 1].Groups[i].Value;
                        if (!string.IsNullOrEmpty(tmpUrlStr))
                        {
                            //去除前后缀
                            tmpUrlStr = StartTagRgx.Replace(tmpUrlStr, "");
                            tmpUrlStr = EndTagRgx.Replace(tmpUrlStr, "");
                            //补全网址
                            if (!Analyzer.FillUrlString(ref tmpUrlStr, visitor.mUrl))
                                break;
                            if (CheckUrl(tmpUrlStr))
                                mVManager.AddVisitor(tmpUrlStr, visitor);
                            break;
                        }
                    }
                }
            }
            mVManager.FinishVisit(visitor);


            // Close the stream object

            streamResponse.Close();
            streamRead.Close();
  
            // Release the HttpWebResponse
  
            response.Close();
        }

        //控制其Url是否需要读取
        private bool CheckUrl(string urlStr)
        {
            return mCUCB == null ? true : mCUCB(urlStr);
        }
    }
}
