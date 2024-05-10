using CaiAdmin.Dto.Fcm;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaiAdmin.Dto.Fam
{
    public class ReleaseOrderQueryDto : QueryDto
    {
        /// <summary>
        /// 运单号
        /// </summary>
        public string ShipmentNo { get; set; }
        /// <summary>
        /// 提单号
        /// </summary>
        public string No { get; set; }
        /// <summary>
        /// 放单类型
        /// </summary>
        public ReleaseType[] ReleaseTypes { get; set; }
        public BillOfLadingType[] Types { get; set; }
        public ReleaseOrderState[] States { get; set; }
        public Guid[] NotIds { get; set; }
        public Guid[] Ids { get; set; }
        public string IdString { get; set; }
    }
}
