using DotNetCore.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace DotNetCore.Domain.Models
{
    public class UserPermission
    {
        public int UserID { get; set; }

        public Permession PermessionId { get; set; }
    }
}
