using System;
using System.Collections.Generic;
using System.Text;

namespace CaiAdmin.Service.Crm
{
    public class NameUniqueValueRepeatDto
    {
        public string NameUniqueValue { get; set; }

        public int Quantity { get; set; }

        public DateTime LastCreationTime { get; set; }
    }
}
