using DotNetCore.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetCore.Application.Validators
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple =false)]
    public class CheckPermissionAttribute:Attribute
    {
        public Permession Permession { get; } 
        public CheckPermissionAttribute(Permession permession)
        {
            Permession = permession;
        }
    }
}
