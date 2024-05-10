using CaiAdmin.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaiAdmin.Service.Crm
{
    public class SeaPoolStatisticsDto
    {
        /// <summary>
        /// 分配时间
        /// </summary>
        public DateTime AllocationTime { get; set; }
        /// <summary>
        /// 公司ID
        /// </summary>
        public Guid CompanyId { get; set; }
        /// <summary>
        /// 公司名称
        /// </summary>
        public string CompanyName { get; set; }
        /// <summary>
        /// 当天分配数量
        /// </summary>
        public int Total { get; set; }
        /// <summary>
        /// 认领数量
        /// </summary>
        public int ClaimTotal { get; set; }
        /// <summary>
        /// 退回数量
        /// </summary>
        public int ReturnTotal { get; set; }
        /// <summary>
        /// 未认领数量
        /// </summary>
        public int UnclaimTotal { get; set; }
        /// <summary>
        /// 分发业务员数量
        /// </summary>
        public int SalesmanTotal { get; set; }

    }
}
