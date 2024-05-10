using System;
using System.Collections.Generic;
using System.Text;

namespace CaiAdmin.Service.Crm
{
    public class MergeCustomerDto
    {
        public List<string> NameUniqueValues { get; set; }
        public bool IsLocalization { get; set; }
    }
}
