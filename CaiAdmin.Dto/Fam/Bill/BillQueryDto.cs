using CaiAdmin.Service.Fam;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaiAdmin.Dto.Fam
{
    public class BillQueryDto : QueryDto
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
        /// <summary>
        /// 业务类型
        /// </summary>
        public ShipmentServiceType[] ShipmentServiceTypes { get; set; }

        public string ShipmentServiceNo { get; set; }
        /// <summary>
        /// 客户名称
        /// </summary>
        public string CustomerName { get; set; }
    }
}
