using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class Brand : IDBObject
    {
        public int BID
        {
            get;
            set;
        }

        public string NameStr
        {
            get;
            set;
        }

        public int Tag
        {
            get;
            set;
        }

        public Brand()
        {
        }

        public int GetID()
        {
            return BID;
        }

        public void SetID(int ID)
        {
            BID = ID;
        }
    }
}
