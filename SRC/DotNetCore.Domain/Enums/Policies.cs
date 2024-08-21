using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetCore.Domain.Enums
{
    public enum Policies
    {
        None,
        SuperAdminsOnly,
        Gender,
        SuperAdminOrGender,
        Adult
    }
}
