using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Model;

namespace Controller
{
    public class BrandDAL : DALBase<Brand>
    {

    }

    public class BrandController : ControllerBase<Brand, BrandDAL>
    {
        private Dictionary<int, int> mTagDic = new Dictionary<int, int>();
        private int index = 0;

        public BrandController()
        {
            mBAMCB = BeforeAddToMemoryFun;
            mAAMCB = AfterAddToMemoryFun;
            RefreshFromDB();
        }

        public bool BeforeAddToMemoryFun(Brand obj)
        {
            return !mTagDic.Keys.Contains(obj.Tag);
        }

        public void AfterAddToMemoryFun(Brand obj)
        {
            if (mTagDic.Keys.Contains(obj.Tag))
            {
                Console.WriteLine("Error:Same Tag[" + obj.Tag + "], class:Brand");
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
