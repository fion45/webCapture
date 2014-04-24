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

        private List<int> mNotInDB = new List<int>();
        public List<T> mArr = new List<T>();
        protected BeforeAddToMemoryCB mBAMCB = null;
        protected AfterAddToMemoryCB mAAMCB = null;

        //把缓存的数据保存到数据库中
        public void RefreshToDB()
        {
            try
            {
                Monitor.Enter(mArr);
                //写进数据库
                foreach (int index in mNotInDB)
                {
                    mArr[index].SetID(Add(mArr[index]));
                }
                mNotInDB.Clear();
            }
            catch (Exception ex)
            {
                Type modeltype = typeof(T);
                string tempStr = modeltype.Name;
                string EStr = string.Format("Error:{0} {1} {2}", tempStr, ex.StackTrace, ex.Message);
                Console.WriteLine(EStr);
            }
            finally
            {
                Monitor.Exit(mArr);
            }
        }

        public bool AddToMemory(T obj)
        {
            Monitor.Enter(mArr);
            if (mBAMCB == null | mBAMCB(obj))
            {
                try
                {
                    mNotInDB.Add(mArr.Count);
                    mArr.Add(obj);
                }
                catch (Exception ex)
                {
                    Type modeltype = typeof(T);
                    string tempStr = modeltype.Name;
                    string EStr = string.Format("Error:{0} {1} {2}", tempStr, ex.StackTrace, ex.Message);
                    Console.WriteLine(EStr);
                }
                if (mAAMCB != null)
                    mAAMCB(obj);
                Monitor.Exit(mArr);
                return true;
            }
            Monitor.Exit(mArr);
            return false;
        }

        //从数据库中刷新数据到缓存中
        public void RefreshFromDB()
        {
            List<T> OList = null;
            List<T> tmpOArr = mArr;
            try
            {
                //读取数据库
                OList = GetAll();
                Monitor.Enter(tmpOArr);
                for (int i = 0; i < mNotInDB.Count; i++)
                {
                    OList.Add(mArr[i]);
                }
                mArr = OList;
            }
            catch (Exception ex)
            {
                Type modeltype = typeof(T);
                string tempStr = modeltype.Name;
                string EStr = string.Format("Error:{0} {1} {2}", tempStr, ex.StackTrace, ex.Message);
                Console.WriteLine(EStr);
            }
            finally
            {
                Monitor.Exit(tmpOArr);
            }
            if (mAAMCB != null && OList != null)
            {
                foreach (T obj in OList)
                    mAAMCB(obj);
            }
            return;
        }
    }
}
