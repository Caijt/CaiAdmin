using System;
using System.Collections.Generic;
using System.Text;

namespace CaiAdmin.Dto.Crm
{
    public class CustomerQueryDto : QueryDto
    {
        public string IdString { get; set; }
        public string Name { get; set; }
        public string EnName { get; set; }
        public string ZhName { get; set; }

        public string Code { get; set; }

        public string Address { get; set; }

        public string EnAddress { get; set; }
        public string ZhAddress { get; set; }

        public string NameUniqueValue { get; set; }

        public string NameLocalizationUniqueValue { get; set; }
        public string LocalizationText { get; set; }

        public Guid[] Ids { get; set; }

    }
}
