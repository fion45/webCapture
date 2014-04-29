using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class Url : IDBObject
    {
        public int UID
        {
            get;
            set;
        }

        public string Address
        {
            get;
            set;
        }

        public int GetID()
        {
            return UID;
        }

        public void SetID(int ID)
        {
            UID = ID;
        }
    }
}
