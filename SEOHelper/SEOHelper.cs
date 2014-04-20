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

        private VisitorManager mVManager;
        private EHelperStatus mRunStatus;
        public ManualResetEvent mContinue;
        private int mThreadCount;

        public CSEOHelper()
        {
            mContinue = new ManualResetEvent(true);
            mVManager = new VisitorManager();
            mThreadCount = 0;
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
            Interlocked.Increment(ref mThreadCount);
            while(mRunStatus != EHelperStatus.Stoped)
            {
                if(mContinue.WaitOne(3000) && mVManager.mEvent.WaitOne(3000))
                {
                    Visitor tmpV = mVManager.GetInitVisitor();
                    ThreadPool.QueueUserWorkItem(WaitCallback,tmpV);
                }
            }
            Interlocked.Decrement(ref mThreadCount);
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
            Interlocked.Increment(ref mThreadCount);
            Visitor tmpVisitor = (Visitor)state;
            bool successTag = false;
            try
            {
                HttpWebRequest tmpRequest = (HttpWebRequest)HttpWebRequest.Create(tmpVisitor.mUrl);
                tmpRequest.Timeout = 5000;
                tmpVisitor.mStatus = Visitor.EVisitStatus.Visiting;
                HttpWebResponse response = (HttpWebResponse)tmpRequest.GetResponse();
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
                                if (!Analyzer.FillUrlString(ref tmpUrlStr, tmpVisitor.mUrl))
                                    break;
                                Console.WriteLine(tmpUrlStr);
                                if (CheckUrl(tmpUrlStr))
                                    mVManager.AddVisitor(tmpUrlStr, tmpVisitor);
                                break;
                            }
                        }
                    }
                }
                successTag = true;

                // Close the stream object

                streamResponse.Close();
                streamRead.Close();
  
                // Release the HttpWebResponse
  
                response.Close();
            }
            catch(Exception ex)
            {
                successTag = false;
                string EStr = string.Format("{0} [{1}]", tmpVisitor.mUrl, ex.Message);
                Console.WriteLine(EStr);

            }
            finally
            {
                mVManager.FinishVisit(tmpVisitor,successTag);
                Interlocked.Decrement(ref mThreadCount);
            }
        }

        //控制其Url是否需要读取
        private bool CheckUrl(string urlStr)
        {
            return mCUCB == null ? true : mCUCB(urlStr);
        }

        public void GetHelperStatus(out long threadCount,out int totalCount,out int waitCount, out int finishCount)
        {
            threadCount = mThreadCount;
            mVManager.GetArrCount(out totalCount,out waitCount, out finishCount);
        }

        public Visitor GetVisitor(int index)
        {
            return mVManager.mVisitorArr[index];
        }

        public bool AlreadyHave(string url)
        {
            return mVManager.mUrlSet.Contains(url);
        }
    }
}
