using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Model;

namespace Controller
{
    public class ProductDAL : DALBase<Product>
    {

    }

    public class ProductController : ControllerBase<Product, ProductDAL>
    {
        
    }
}
