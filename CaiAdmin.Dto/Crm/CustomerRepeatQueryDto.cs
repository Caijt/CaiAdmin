using System;
using System.Collections.Generic;
using System.Text;

namespace CaiAdmin.Dto.Crm
{
    public class CustomerRepeatQueryDto : QueryDto
    {
        public string NameUniqueValue { get; set; }

        public bool ExcludeDelete { get; set; }

        public bool ExcludeMerge { get; set; }
    }
}
