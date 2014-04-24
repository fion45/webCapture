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
            RefreshFromDB();
        }

        private bool NotExist(Category obj)
        {
            bool tag = mTagDic.Keys.Contains(obj.Tag);
            if (tag)
                return false;
            else
                return true;
        }

        private void AfterAddToMemoryFun(Category obj)
        {
            if (mTagDic.Keys.Contains(obj.Tag))
            {
                Console.WriteLine("Error:Same Tag[" + obj.Tag + "], class:Category");
            }
            else
            {
                mTagDic.Add(obj.Tag, index);
                ++index;
            }
        }

        public int GetID(int tag)
        {
            Monitor.Enter(mTagDic);
            bool exsis = mTagDic.Keys.Contains(tag);
            int result = -1;
            if (exsis)
            {
                result = mArr[mTagDic[tag]].GetID();
            }
            Monitor.Exit(mTagDic);
            return result;
        }
    }
}
