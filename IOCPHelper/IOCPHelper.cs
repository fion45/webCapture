using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;
using System.Management;
using System.Net.Sockets;

namespace IOCPHelper
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public unsafe struct OVERLAPPED
    {
        public UInt32* ulpInternal;
        public UInt32* ulpInternalHigh;
        public Int32 lOffset;
        public Int32 lOffsetHigh;
        public UInt32 hEvent;
    }

    public class IOCPHelper
    {
        public delegate Int32 CallBack();
        public struct Internal
        {
            public TcpClient mClient;
            public CallBack mCBFun;
        }

        // Win32 Function Prototypes

        /// <summary> Win32Func: Create an IO Completion Port Thread Pool </summary>
        [DllImport("Kernel32", CharSet = CharSet.Auto)]
        private unsafe static extern UInt32 CreateIoCompletionPort(UInt32 hFile, UInt32 hExistingCompletionPort, UInt32* puiCompletionKey, UInt32 uiNumberOfConcurrentThreads);

        /// <summary> Win32Func: Closes an IO Completion Port Thread Pool </summary>
        [DllImport("Kernel32", CharSet = CharSet.Auto)]
        private unsafe static extern Boolean CloseHandle(UInt32 hObject);

        /// <summary> Win32Func: Posts a context based event into an IO Completion Port Thread Pool </summary>
        [DllImport("Kernel32", CharSet = CharSet.Auto)]
        private unsafe static extern Boolean PostQueuedCompletionStatus(UInt32 hCompletionPort, UInt32 dwNumberOfBytesTrlansferred, UInt32* dwCompletlonKey, OVERLAPPED* pOverlapped);

        /// <summary> Win32Func: Waits on a context based event from an IO Completion Port Thread Pool.
        ///           All threads in the pool wait in this Win32 Function </summary>
        [DllImport("Kernel32", CharSet = CharSet.Auto)]
        private unsafe static extern Boolean GetQueuedCompletionStatus(UInt32 hCompletionPort, UInt32* lpNumberOfBytes, UInt32* lpCompletionKey, OVERLAPPED** lpOverlapped, UInt32 uiMilliseconds);

        // Constants
        /// <summary> SimTypeConst: This represents the Win32 Invalid Handle Value Macro </summary>
        private const UInt32 INVALID_HANDLE_VALUE = 0xffffffff;

        /// <summary> SimTypeConst: This represents the Win32 INFINITE Macro </summary>
        private const UInt32 INIFINITE = 0xffffffff;

        /// <summary> SimTypeConst: This tells the IOCP Function to shutdown </summary>
        private const Int32 SHUTDOWN_IOCPTHREAD = 0x7fffffff;

        private UInt32 mCPUCount;
        private Thread[] mThreadArr;
        private UInt32 mCompletionPort;
        private bool mRunTag;
        private List<Internal> mInternalArr = new List<Internal>();

        public IOCPHelper()
        {
            mRunTag = true;

            ManagementClass c = new ManagementClass(
            new ManagementPath("Win32_Processor"));
            // Get the properties in the class
            ManagementObjectCollection moc = c.GetInstances();

            mCPUCount = (UInt32)moc.Count;
            unsafe
            {
                mCompletionPort = CreateIoCompletionPort(INVALID_HANDLE_VALUE, 0, null, mCPUCount);
            }

            mThreadArr = new Thread[mCPUCount];
            ThreadStart TmpTS = new ThreadStart(Run);
            for (int i = 0; i < mCPUCount; i++)
            {
                mThreadArr[i] = new Thread(TmpTS);
                mThreadArr[i].Start();
            }
        }

        public ~IOCPHelper()
        {
            mRunTag = false;
            //等待子线程结束
            for (int i = 0; i < mCPUCount; i++)
            {
                unsafe
                {
                    bool bret = PostQueuedCompletionStatus(mCompletionPort, 4, (UInt32*)SHUTDOWN_IOCPTHREAD, null);
                }
            }
        }

        public void Run()
        {
            while(mRunTag)
            {
                unsafe
                {
                    UInt32* NumberOfBytes = null;
                    UInt32 CompletionKey = 0;
                    OVERLAPPED* tmpOL;
                    if(!GetQueuedCompletionStatus(mCompletionPort,NumberOfBytes,(UInt32*)&CompletionKey,&tmpOL,5000))
                        continue;
                    if(CompletionKey == SHUTDOWN_IOCPTHREAD)
                        return;
                    
                    mInternalArr[(int)CompletionKey].mCBFun(NumberOfBytes,tmpOL->da);
                }
            }
        }

        public void Bind(TcpClient tcpClient,CallBack cbFun)
        {
            int tmpIndex = mInternalArr.Count;
            unsafe
            {
                CreateIoCompletionPort((uint)(tcpClient.Client.Handle), mCompletionPort, (UInt32*)&(tmpIndex), mCPUCount);
            }
            Internal tmpInternal = new Internal()
            {
                mClient = tcpClient,
                mCBFun = cbFun
            };
            mInternalArr.Add(tmpInternal);
        }
    }
}
