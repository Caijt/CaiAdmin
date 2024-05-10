using System;
using System.Collections.Generic;
using System.Text;

namespace CaiAdmin.Dto.Crm
{
    public class OldIcpCustomerTitleQueryDto : QueryDto
    {
        public string IdString { get; set; }
        public string Name { get; set; }

        public string TaxNo { get; set; }
        

        public string CustomerName { get; set; }

        public string AddressTel { get; set; }

        public Guid[] Ids { get; set; }
        public DateTime? CreateDateBegin { get; set; }
        public DateTime? CreateDateEnd { get; set; }

    }
}
