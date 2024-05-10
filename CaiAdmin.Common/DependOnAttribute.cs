using System;
using System.Collections.Generic;
using System.Text;

namespace CaiAdmin.Common
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public class DependOnAttribute : Attribute
    {
        public Type[] DependTypes { get; }
        public DependOnAttribute(params Type[] types)
        {
            DependTypes = types;
        }
    }
}
