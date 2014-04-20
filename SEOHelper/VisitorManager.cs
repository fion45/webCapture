using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SEOHelper
{
    public class Visitor
    {
        public enum EVisitStatus
        {
            Init,
            Visiting,
            Success,
            Failure
        }
        public string mUrl;
        public Visitor mParent;
        public EVisitStatus mStatus;
        public byte[] mContent;
        public int mContentLen;
    }

    public class VisitorManager
    {
        public HashSet<string> mUrlSet;
        public List<Visitor> mVisitorArr;
        private Queue<Visitor> mInitVisitorArr;
        private Queue<Visitor> mHasVisitedArr;
        public ManualResetEvent mEvent;

        public VisitorManager()
        {
            mEvent = new ManualResetEvent(false);
            mUrlSet = new HashSet<string>();
            mVisitorArr = new List<Visitor>(1024);
            mInitVisitorArr = new Queue<Visitor>();
            mHasVisitedArr = new Queue<Visitor>();
        }

        public bool AddVisitor(string url,Visitor parVisitor)
        {
            if (mUrlSet.Contains(url))      //不添加重复的
                return false;
            Visitor visitor = new Visitor() {
                mUrl = url,
                mParent = parVisitor,
                mStatus = Visitor.EVisitStatus.Init,
                mContent = null,
                mContentLen = 0
            };
            mUrlSet.Add(url);
            mVisitorArr.Add(visitor);
            Monitor.Enter(mInitVisitorArr);
            mInitVisitorArr.Enqueue(visitor);
            mEvent.Set();
            Monitor.Exit(mInitVisitorArr);
            return true;
        }

        public bool HasFinished()
        {
            return mInitVisitorArr.Count == 0;
        }

        public Visitor GetInitVisitor()
        {
            Monitor.Enter(mInitVisitorArr);
            Visitor result = mInitVisitorArr.Dequeue();
            if (mInitVisitorArr.Count == 0)
                mEvent.Reset();
            Monitor.Exit(mInitVisitorArr);
            return result;
        }

        public void FinishVisit(Visitor visitor,bool sTag = true)
        {
            Monitor.Enter(mHasVisitedArr);
            visitor.mStatus = sTag ? Visitor.EVisitStatus.Success : Visitor.EVisitStatus.Success;
            mHasVisitedArr.Enqueue(visitor);
            Monitor.Exit(mHasVisitedArr);
        }

        public void GetArrCount(out int totalCount, out int waitCount, out int finishCount)
        {
            totalCount = mVisitorArr.Count();
            finishCount = mHasVisitedArr.Count();
            waitCount = totalCount - finishCount;
        }
    }
}
