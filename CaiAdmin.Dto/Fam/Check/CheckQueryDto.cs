using CaiAdmin.Service.Fam;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaiAdmin.Dto.Fam
{
    public class CheckQueryDto : QueryDto
    {
        /// <summary>
        /// 账单号
        /// </summary>
        public string No { get; set; }
        
        public BillType[] Types { get; set; }
        public BillStatus[] Statuses { get; set; }

        public Guid[] NotIds { get; set; }
        public Guid[] Ids { get; set; }

        public string IdString { get; set; }
    }
}
