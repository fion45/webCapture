using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class Product : IDBObject
    {
        public int PID;
        public int CID;
        public string Title;
        public int BrandID;
        public float Price;
        public int Discount;
        public int Stock;
        public int Sale;
        public string Descript;
        public int Tag;

        public Product()
        {
        }

        public int GetID()
        {
            return PID;
        }
    }
}
