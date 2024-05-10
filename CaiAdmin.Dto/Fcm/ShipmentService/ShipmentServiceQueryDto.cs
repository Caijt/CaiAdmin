using System;
using System.Collections.Generic;
using System.Text;

namespace CaiAdmin.Dto.Fcm
{
    public class ShipmentServiceQueryDto : QueryDto
    {
        /// <summary>
        /// 账单号
        /// </summary>
        public string No { get; set; }

        public Guid[] NotIds { get; set; }
        public Guid[] Ids { get; set; }

        public string IdString { get; set; }
    }
}
