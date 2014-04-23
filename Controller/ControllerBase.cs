using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;
using System.Threading;

namespace Controller
{
    public class ControllerBase<T, K> : BLLBase<T, K>
        where T : IDBObject, new()
        where K : DALBase<T>, new()
    {
        public delegate bool BeforeAddToMemoryCB(T obj);
        public delegate void AfterAddToMemoryCB(T obj);

        protected bool mSyncTag = false;
        private List<int> mNotInDB = new List<int>();
        public List<T> mArr = new List<T>();
        protected BeforeAddToMemoryCB mBAMCB = null;
        protected AfterAddToMemoryCB mAAMCB = null;
        private object tmpLock = new object();

        //把缓存的数据保存到数据库中
        public void RefreshToDB()
        {
            Monitor.Enter(mArr);
            //写进数据库
            foreach (int index in mNotInDB)
            {
                mArr[index].SetID(Add(mArr[index]));
            }
            mNotInDB.Clear();
            Monitor.Exit(mArr);
        }

        public bool AddToMemory(T obj)
        {
            if (mBAMCB == null | mBAMCB(obj))
            {
                Monitor.Enter(mArr);
                mNotInDB.Add(mArr.Count);
                mArr.Add(obj);
                Monitor.Exit(mArr);
                if (mAAMCB != null)
                    mAAMCB(obj);
                return true;
            }
            return false;
        }

        //从数据库中刷新数据到缓存中
        public void RefreshFromDB()
        {
            Monitor.Enter(tmpLock);
            if (!mSyncTag)
            {
                mSyncTag = true;
                Monitor.Exit(tmpLock);
                //读取数据库
                List<T> OList = GetAll();
                Monitor.Enter(mArr);
                for (int i = 0; i < mNotInDB.Count;i++ )
                {
                    OList.Add(mArr[i]);
                }
                List<T> tmpOArr = mArr;
                mArr = OList;
                Monitor.Exit(tmpOArr);
                if (mAAMCB != null)
                {
                    foreach (T obj in OList)
                        mAAMCB(obj);
                }
                return;
            }
            Monitor.Exit(tmpLock);
        }
    }
}
