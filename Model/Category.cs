using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class Category : IDBObject
    {
        int CID;
        int ParCID;
        string NameStr;

        public Category()
        {

        }

        public int GetID()
        {
            return CID;
        }
    }
}
