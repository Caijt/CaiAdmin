using CaiAdmin.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CaiAdmin.Dto.Fam
{
    public class BillDto
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 账单号
        /// </summary>
        public string No { get; set; }

        public BillType Type { get; set; }
        public string TypeName
        {
            get
            {
                return CommonHelper.GetDescription(Type);
            }
        }

        public BillStatus Status { get; set; }

        public string StatusName
        {
            get
            {
                return CommonHelper.GetDescription(Status);
            }
        }

        /// <summary>
        /// 账单日期
        /// </summary>
        public DateTime AccountDate { get; set; }
        public DateTime DueDate { get; set; }

        public string CompanyName { get; set; }

        public string CustomerName { get; set; }

        public DateTime CreationTime { get; set; }

        public string CreatorUserName { get; set; }

        public ShipmentServiceType ShipmentServiceType { get; set; }

        public string ShipmentServiceTypeName
        {
            get
            {
                return CommonHelper.GetDescription(ShipmentServiceType);
            }
        }
        /// <summary>
        /// 业务号
        /// </summary>
        public string ShipmentServiceNo { get; set; }
    }

    public enum BillType
    {
        [Description("应收")]
        AR = 0,
        [Description("应付")]
        AP = 1,
        [Description("代理")]

        DC = 2
    }

    public enum BillStatus
    {
        [Description("已创建")]
        Init = 0,
        [Description("锁单")]
        Locked = 1,
        /// <summary>
        /// 部分核销
        /// </summary>
        [Description("部分核销")]
        PartWriteOff = 2,
        /// <summary>
        /// 完全核销
        /// </summary>
        [Description("完全核销")]
        AllWriteOff = 3,
        /// <summary>
        /// 部分到账
        /// </summary>
        [Description("部分到账")]
        PartToBank = 4,
        /// <summary>
        /// 全部到账
        /// </summary>
        [Description("全部到账")]
        AllToBank = 5,
        /// <summary>
        /// 审核(出凭证)
        /// </summary>
        [Description("审核")]
        Approvaled = 6
    }

    public enum ShipmentServiceType
    {
        /// <summary>
        /// 未知
        /// </summary>
        NotSet = 0,

        /// <summary>
        /// 出口
        /// </summary>
        [Description("出口业务")]
        Export,

        /// <summary>
        /// 进口
        /// </summary>
        [Description("进口业务")]
        Import,

        /// <summary>
        /// 尾程派送
        /// </summary>
        [Description("其它业务")]
        Other
    }
}
