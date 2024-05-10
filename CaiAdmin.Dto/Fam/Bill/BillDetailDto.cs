using CaiAdmin.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CaiAdmin.Service.Fam
{
    public class BillDetailDto
    {
        public Guid Id { get; set; }
        public string No { get; set; }

        public Guid RefId { get; set; }
        public DateTime AccountDate { get; set; }
        public DateTime DueDate { get; set; }
        /// <summary>
        /// 费用明细
        /// </summary>
        public List<BillChargesDto> Charges { get; set; }
        /// <summary>
        /// 工作流
        /// </summary>
        public List<BillWorkflowDto> Workflows { get; set; }

        public DateTime CreationTime { get; set; }

        public string CreatorUserName { get; set; }
        public DateTime? LastModificationTime { get; set; }
    }

    public class BillChargesDto
    {
        public Guid Id { get; set; }

        public Guid BillId { get; set; }
        public BillChargeWay Way { get; set; }
        public string WayName
        {
            get
            {
                return CommonHelper.GetDescription(Way);
            }
        }
        public string Currency { get; set; }
        public decimal Amount { get; set; }

        public decimal Qty { get; set; }
        public decimal PayAmount { get; set; }

        public decimal UnitPrice { get; set; }
        public string ChargeCodeName { get; set; }
        public DateTime CreationTime { get; set; }

        public string CreatorUserName { get; set; }
        public DateTime? LastModificationTime { get; set; }
    }

    public class BillWorkflowDto
    {
        public Guid WorkflowId { get; set; }
        public string WorkflowNo { get; set; }

        public Guid BillId { get; set; }

        public bool IsCommission { get; set; }

        public string Currency { get; set; }
        public decimal Amount { get; set; }
        public decimal CurrencyAmount { get; set; }
        public decimal ApplyAmount { get; set; }
        public DateTime CreationTime { get; set; }

        public string CreatorUserName { get; set; }
        public DateTime? LastModificationTime { get; set; }
    }

    public enum BillChargeWay
    {
        [Description("应收")]
        AR = 0,
        [Description("应付")]
        AP = 1
    }
}
