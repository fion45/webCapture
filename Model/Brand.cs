using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class Brand : IDBObject
    {
        int BID;
        string NameStr;

        public Brand()
        {
        }

        public int GetID()
        {
            return BID;
        }
    }
}
