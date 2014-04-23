using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Model;

namespace Controller
{
    public class CategoryDAL : DALBase<Category>
    {

    }

    public class CategoryController : ControllerBase<Category, CategoryDAL>
    {
        private Dictionary<int, int> mTagDic = new Dictionary<int, int>();
        private int index = 0;

        public CategoryController()
        {
            mBAMCB = NotExist;
            mAAMCB = AfterAddToMemoryFun;
        }

        public bool NotExist(Category obj)
        {
            if (!mSyncTag)
                RefreshFromDB();
            Monitor.Enter(mTagDic);
            bool tag = mTagDic.Keys.Contains(obj.Tag);
            Monitor.Exit(mTagDic);
            if (tag)
                return false;
            else
                return true;
        }

        public void AfterAddToMemoryFun(Category obj)
        {
            Monitor.Enter(mTagDic);
            mTagDic.Add(obj.Tag, index);
            ++index;
            Monitor.Exit(mTagDic);
        }

        public int GetID(int tag)
        {
            Monitor.Enter(mTagDic);
            bool exsis = mTagDic.Keys.Contains(tag);
            Monitor.Exit(mTagDic);
            int result = -1;
            if (exsis)
            {
                Monitor.Enter(mTagDic);
                result = mArr[mTagDic[tag]].GetID();
                Monitor.Exit(mTagDic);
            }
            return result;
        }
    }
}
