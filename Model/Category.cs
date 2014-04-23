using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class Category : IDBObject
    {
        public int CID
        {
            get;
            set;
        }

        public int ParCID
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

        public Category()
        {

        }

        public int GetID()
        {
            return CID;
        }

        public void SetID(int ID)
        {
            CID = ID;
        }
    }
}
