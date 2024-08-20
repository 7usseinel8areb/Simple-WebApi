using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetCore.Domain.Enums
{
    public enum Permession
    {
        None,
        //Permissions for end point of product controller 
        ReadProduct,
        AddProduct,
        EditProduct,
        DeleteProduct

    }
}
