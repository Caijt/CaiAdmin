using CaiAdmin.Common;
using CaiAdmin.Service.Crm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CaiAdmin.Dto.Crm
{

    public class C
    {
        public long AllocationUserId { get; set; }
        public string AllocationUserName { get; set; }

        public DateTime AllocationTime { get; set; }

        public SeaPoolStatisticsType Type { get; set; }

        public string OperationRemark { get; set; }
        public CustomerReturnType? ReturnType { get; set; }
        public string ReturnTypeName
        {
            get
            {
                return CommonHelper.GetDescription(ReturnType);
            }
        }

        public CustomerType CustomerType { get; set; }

        public string CustomerTypeName
        {
            get
            {
                return CommonHelper.GetDescription(CustomerType);
            }
        }

        /// <summary>
        /// 认领时间
        /// </summary>
        public DateTime? ClaimTime { get; set; }
        public bool ClaimStatus { get; set; }
        public bool IsValid { get; set; }
        public string CustomerName { get; set; }
        public Guid CustomerId { get; set; }
    }

    public class CQuery : QueryDto
    {
        public DateTime? AllocationTimeBegin { get; set; }
        public DateTime? AllocationTimeEnd { get; set; }

        public Guid[] CompanyIds { get; set; }

        public SeaPoolStatisticsType[] Types { get; set; }

        public CustomerType[] CustomerTypes { get; set; }
    }

    public enum CustomerReturnType : byte
    {
        /// <summary>
        /// 缺省
        /// </summary>
        [Description("缺省")]
        NoSet = 0,

        /// <summary>
        /// 低价值
        /// </summary>
        [Description("低价值")]
        Low = 1,

        /// <summary>
        /// 黑名单
        /// </summary>
        [Description("黑名单")]
        Blacklist = 2,

        /// <summary>
        /// 其他原因
        /// </summary>
        [Description("其他原因")]
        Other = 3,
    }
}
