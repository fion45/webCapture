using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SEOHelper
{
    public class NetworkMonitor
    {
        private static NetworkMonitor gNWMonitor = null;
        private static readonly object lockHelper = new object();

        private long mSC = 0;
        private long mRC = 0;

        private readonly object ILockHelper = new object();
        private DateTime mDT = new DateTime();

        private NetworkMonitor()
        {
            mDT = DateTime.MinValue;
        }

        public static NetworkMonitor GetInstance()
        {
            if(gNWMonitor == null)
            {
                Monitor.Enter(lockHelper);
                if(gNWMonitor == null)
                {
                    gNWMonitor = new NetworkMonitor();
                }
                Monitor.Exit(lockHelper);
            }
            return gNWMonitor;
        }

        public void GetNetworkState(out double sendSpeed, out double receiveSpeed)
        {
            sendSpeed = 0;
            receiveSpeed = 0;
            Monitor.Enter(ILockHelper);
            if(mDT != DateTime.MinValue)
            {
                TimeSpan tmpTS = DateTime.Now - mDT;
                sendSpeed = (double)mSC / 1024 / tmpTS.TotalSeconds;
                receiveSpeed = (double)mRC / 1024 / tmpTS.TotalSeconds;
                mDT = DateTime.Now;
            }
            Monitor.Exit(ILockHelper);
        }

        public void Increase(long sBCount, long rBCount)
        {
            Monitor.Enter(ILockHelper);
            mSC += sBCount;
            mRC += rBCount;
            Monitor.Exit(ILockHelper);
        }

        
    }
}
