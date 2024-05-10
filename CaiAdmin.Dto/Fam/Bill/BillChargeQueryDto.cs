using System;
using System.Collections.Generic;
using System.Text;

namespace CaiAdmin.Dto.Fam
{
    public class BillChargeQueryDto : QueryDto
    {
        public Guid[] Ids { get; set; }
        public Guid[] BillIds { get; set; }

        public CdcOperationType[] CdcOperationTypes { get; set; }
    }
}
