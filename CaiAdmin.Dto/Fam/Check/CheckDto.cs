using CaiAdmin.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CaiAdmin.Dto.Fam
{
    public class CheckDto
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 账单号
        /// </summary>
        public string No { get; set; }

        /// <summary>
        /// 核销单类型
        /// </summary>
        public CheckType Type { get; set; }


        public bool IsDeleted { get; set; }

        /// <summary>
        /// 核销单类型名称
        /// </summary>
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
        public Guid CompanyId { get; set; }
        /// <summary>
        /// 公司名称
        /// </summary>
        public string CompanyName { get; set; }

        public Guid CustomerId { get; set; }
        /// <summary>
        /// 客户名称
        /// </summary>
        public string CustomerName { get; set; }

        public bool IsMultCurrency { get; set; }

        /// <summary>
        /// 账单日期
        /// </summary>
        public DateTime AccountDate { get; set; }
        public DateTime DueDate { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public string CreatorUserName { get; set; }
    }

    public enum CheckType 
    {

        /// <summary>
        /// 收款核销单
        /// </summary>
        [Description("收款核销单")]
        AR = 0,
        /// <summary>
        /// 付款核销单
        /// </summary>
        [Description("付款核销单")]
        AP = 1
    }
}
