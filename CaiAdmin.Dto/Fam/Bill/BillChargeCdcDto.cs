using CaiAdmin.Common;
using CaiAdmin.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CaiAdmin.Dto.Fam
{
    public class BillChargeCdcDto
    {
        public Guid Id { get; set; }
        /// <summary>
        /// cdc记录时间
        /// </summary>
        public DateTime CdcTime { get; set; }
        /// <summary>
        /// 操作类型
        /// </summary>
        public CdcOperationType CdcOperationType { get; set; }

        /// <summary>
        /// 费用名称
        /// </summary>
        public string ChargeCodeName { get; set; }

        /// <summary>
        /// 账单ID
        /// </summary>
        public Guid BillId { get; set; }

        /// <summary>
        /// 操作类型名称
        /// </summary>
        public string CdcOperationTypeName
        {
            get
            {
                return CommonHelper.GetDescription(CdcOperationType);
            }
        }

        /// <summary>
        /// 币种
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatorUserName { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        public string LastModifierUserName { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        public decimal UnitPrice { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime? LastModificationTime { get; set; }
    }


}
