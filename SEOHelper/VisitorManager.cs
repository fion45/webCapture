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
            Visited
        }
        public string mUrl;
        public Visitor mParent;
        public EVisitStatus mStatus;
        public byte[] mContent;
        public int mContentLen;
    }

    public class VisitorManager
    {
        private Queue<Visitor> mVisitorArr;
        private Queue<Visitor> mHasVisitedArr;
        public ManualResetEvent mEvent;

        public VisitorManager()
        {
            mEvent = new ManualResetEvent(false);
            mVisitorArr = new Queue<Visitor>();
            mHasVisitedArr = new Queue<Visitor>();
        }

        public void AddVisitor(string url,Visitor parVisitor)
        {
            Visitor visitor = new Visitor() {
                mUrl = url,
                mParent = parVisitor,
                mStatus = Visitor.EVisitStatus.Init,
                mContent = null,
                mContentLen = 0
            };
            Monitor.Enter(mVisitorArr);
            mVisitorArr.Enqueue(visitor);
            mEvent.Set();
            Monitor.Exit(mVisitorArr);
        }

        public bool HasFinished()
        {
            return mVisitorArr.Count == 0;
        }

        public Visitor GetInitVisitor()
        {
            Monitor.Enter(mVisitorArr);
            Visitor result = mVisitorArr.Dequeue();
            if (mVisitorArr.Count == 0)
                mEvent.Reset();
            Monitor.Exit(mVisitorArr);
            return result;
        }

        public void FinishVisit(Visitor visitor)
        {
            Monitor.Enter(mHasVisitedArr);
            visitor.mStatus = Visitor.EVisitStatus.Visited;
            mHasVisitedArr.Enqueue(visitor);
            Monitor.Exit(mHasVisitedArr);
        }
    }
}
