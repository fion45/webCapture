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
            Monitor.Enter(mArr);
            bool exsis = mTagDic.Keys.Contains(tag);
            int result = -1;
            if (exsis)
            {
                result = mArr[mTagDic[tag]].GetID();
            }
            Monitor.Exit(mArr);
            return result;
        }

        public bool UpdateDBAndMemory(Brand brand)
        {
            Monitor.Enter(mArr);
            bool result = false;
            bool exsis = mTagDic.Keys.Contains(brand.Tag);
            if (exsis)
            {
                int index = mTagDic[brand.Tag];
                if (string.Compare(mArr[index].NameStr, brand.NameStr) != 0 ||
                    string.Compare(mArr[index].Name2, brand.Name2) != 0)
                {
                    mArr[index].NameStr = brand.NameStr;
                    mArr[index].Name2 = brand.Name2;
                    result = Set(mArr[index]);
                }
            }
            Monitor.Exit(mArr);
            return result;
        }
    }
}
