using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;

namespace Controller
{
    public class ControllerBase<T, K> : BLLBase<T, K>
        where T : IDBObject, new()
        where K : DALBase<T>, new()
    {
        private bool mSyncTag = false;
        private List<int> mNotInDB = new List<int>();
        public List<T> mArr = new List<T>();

        //把缓存的数据保存到数据库中
        public void RefreshDB()
        {
            //写进数据库
            foreach (int index in mNotInDB)
            {
                Add(mArr[index]);
            }
            mNotInDB.Clear();
        }

        public void Add(T obj)
        {
            if(!HasExist(obj))
            {
                mNotInDB.Add(mArr.Count);
                mArr.Add(obj);
            }
        }

        //从数据库中刷新数据到缓存中
        public void RefreshMemory()
        {
            if (!mSyncTag)
            {
                //读取数据库
                List<T> OList = GetAll();
                foreach (T obj in OList)
                {
                    mArr.Add(obj);
                }
                mSyncTag = true;
            }
        }

        public bool HasExist(T obj)
        {
            return false;
        }
    }
}
