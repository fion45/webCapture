﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class Product : IDBObject
    {
        public int PID
        {
            get;
            set;
        }

        public int CID
        {
            get;
            set;
        }

        public string Title
        {
            get;
            set;
        }

        public int BrandID
        {
            get;
            set;
        }

        public string Chose
        {
            get;
            set;
        }

        public decimal Price
        {
            get;
            set;
        }

        public decimal MarketPrice
        {
            get;
            set;
        }

        public int Discount
        {
            get;
            set;
        }

        public int Stock
        {
            get;
            set;
        }

        public int Sale
        {
            get;
            set;
        }

        public string ImgPath
        {
            get;
            set;
        }

        public string Descript
        {
            get;
            set;
        }

        public int Tag
        {
            get;
            set;
        }

        public string Date
        {
            get;
            set;
        }

        public Product()
        {
        }

        public int GetID()
        {
            return PID;
        }

        public void SetID(int ID)
        {
            PID = ID;
        }
    }
}
