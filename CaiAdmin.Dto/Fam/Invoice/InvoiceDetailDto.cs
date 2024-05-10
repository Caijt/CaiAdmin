using System;
using System.Collections.Generic;
using System.Text;

namespace CaiAdmin.Dto.Fam
{
    /// <summary>
    /// 发票详情
    /// </summary>
    public class InvoiceDetailDto
    {
        public Guid Id { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceCode { get; set; }

        public decimal InvoiceAmount { get; set; }

        public string NuoNuoUrl { get; set; }
        public DateTime CreationTime { get; set; }
        public long CreatorUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public string Remark { get; set; }
        public int Status { get; set; }
        public int Way { get; set; }
        public string NuoNuoRequestSerialNo { get; set; }
        public string NuoNuoMessage { get; set; }
        public string NuoNuoStatus { get; set; }
        public string NuoNuoStatusText { get; set; }
        public bool NuoNuoSyncFinished { get; set; }
        public string NuoNuoRequestMessage { get; set; }
        /// <summary>
        /// 诺诺开票状态
        /// </summary>
        public string NuoNuoRequestStatus { get; set; }
        public int InvoiceType { get; set; }
        public string OriginalCurrency { get; set; }
    }

    public class InvoiceBillDto
    {
        public string BillNo { get; set; }
        public decimal InvoiceCurrency { get; set; }
        public decimal InvoiceAmount { get; set; }
        public decimal OriginalCurrency { get; set; }
        public decimal OriginalAmount { get; set; }
    }
}
