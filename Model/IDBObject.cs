﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public interface IDBObject
    {
        int GetID();
        void SetID(int ID);
    }
}
