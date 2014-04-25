using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text.RegularExpressions;
using System.Drawing;

namespace SEOHelper
{
    public class CSEOHelper
    {
        const int MAXWORKTHREAD = 16;
        public delegate bool CheckUrlCB(string url);
        public delegate void DealWithContentCB(Visitor visitor, string responseStr);

        private CheckUrlCB mCUCB;
        private DealWithContentCB mDWCCB;
        NetworkMonitor mNWMon = NetworkMonitor.GetInstance();

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
            ThreadPool.SetMaxThreads(MAXWORKTHREAD, MAXWORKTHREAD);
        }

        //public ~CSEOHelper()
        //{
        //    Stop();
        //}

        public void Start(string url,CheckUrlCB cuCB = null,DealWithContentCB dwcCB = null)
        {
            mCUCB = cuCB;
            mDWCCB = dwcCB;
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
                ////网络检测
                //mNWMon.Increase(0, streamResponse.Length);
                //调用回调函数处理内容
                if (mDWCCB != null)
                    mDWCCB(tmpVisitor, responseString);

                //找出<a>
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
                            tmpUrlStr = tmpUrlStr.Trim();
                            if (!string.IsNullOrEmpty(tmpUrlStr))
                            {
                                //去除前后缀
                                tmpUrlStr = StartTagRgx.Replace(tmpUrlStr, "");
                                tmpUrlStr = EndTagRgx.Replace(tmpUrlStr, "");
                                //补全网址
                                if (!Analyzer.FillUrlString(ref tmpUrlStr, tmpVisitor.mUrl))
                                    break;
                                //Console.WriteLine(tmpUrlStr);
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
                string EStr = string.Format("Error:{0} [{1}\t{2}]", tmpVisitor.mUrl, ex.StackTrace, ex.Message);
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

        public void GetImg(string url, string localPath,int TimeOut = 5000)
        {
            HttpWebRequest tmpRequest = (HttpWebRequest)HttpWebRequest.Create(url);
            tmpRequest.Timeout = TimeOut;
            HttpWebResponse response = null;
            Stream streamResponse = null;
            try
            {
                response = (HttpWebResponse)tmpRequest.GetResponse();
                streamResponse = response.GetResponseStream();
                ////网络检测
                //mNWMon.Increase(0, streamResponse.Length);
                Image simg = Image.FromStream(streamResponse);
                string tmpDirectoryPath = localPath.Substring(0, localPath.LastIndexOf('\\'));
                if (!Directory.Exists(tmpDirectoryPath))
                    Directory.CreateDirectory(tmpDirectoryPath);
                string tmpTag = localPath.Substring(localPath.LastIndexOf('.') + 1);
                System.Drawing.Imaging.ImageFormat tmpIFormat = System.Drawing.Imaging.ImageFormat.Bmp;
                switch(tmpTag.ToLower())
                {
                    case "jpg":
                        {
                            tmpIFormat = System.Drawing.Imaging.ImageFormat.Jpeg;
                            break;
                        }
                    case "png":
                        {
                            tmpIFormat = System.Drawing.Imaging.ImageFormat.Png;
                            break;
                        }
                    case "gif":
                        {
                            tmpIFormat = System.Drawing.Imaging.ImageFormat.Gif;
                            break;
                        }
                    default:
                        {
                            tmpIFormat = System.Drawing.Imaging.ImageFormat.Bmp;
                            break;
                        }
                }
                simg.Save(localPath, tmpIFormat);
            }
            catch(Exception ex)
            {
                string EStr = string.Format("Error:{0} [{1}\t{2}]", url, ex.StackTrace, ex.Message);
                Console.WriteLine(EStr);
            }
            if (streamResponse != null)
                streamResponse.Close();
            if (response != null)
                response.Close();
        }
    }
}
