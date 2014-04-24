using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading;
using Model;

namespace Controller
{
    public class ProductDAL : DALBase<Product>
    {

    }

    public class ProductController : ControllerBase<Product, ProductDAL>
    {
        private Dictionary<int, int> mTagDic = new Dictionary<int, int>();
        private int index = 0;

        public ProductController()
        {
            mBAMCB = NotExist;
            mAAMCB = AfterAddToMemoryFun;
            RefreshFromDB();
        }

        public bool NotExist(Product obj)
        {
            bool tag = mTagDic.Keys.Contains(obj.Tag);
            if (tag)
                return false;
            else
                return true;
        }

        public void AfterAddToMemoryFun(Product obj)
        {
            if (mTagDic.Keys.Contains(obj.Tag))
            {
                Console.WriteLine("Error:Same Tag[" + obj.Tag + "], class:Product");
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
