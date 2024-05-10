using CaiAdmin.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaiAdmin.Dto.Fam
{
    public class InvoiceDto
    {
        public Guid Id { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceCode { get; set; }
        public string SerialNo { get; set; }
        /// <summary>
        /// 购方抬头
        /// </summary>
        public string BuyerTitleName { get; set; }
        /// <summary>
        /// 销方抬头
        /// </summary>
        public string SellerTitleName { get; set; }
        public string NuoNuoMessage { get; set; }
        public string NuoNuoUrl { get; set; }
        public string Remark { get; set; }

        public InvoiceStatus Status { get; set; }
        public string StatusText
        {
            get
            {
                return CommonHelper.GetDescription(Status);
            }
        }

        /// <summary>
        /// 诺诺开票结果
        /// </summary>
        public string NuoNuoStatusText { get; set; }

        /// <summary>
        /// 是否在诺诺同步完成
        /// </summary>
        public bool NuoNuoSyncFinished { get; set; }

        public decimal InvoiceAmount { get; set; }
        public DateTime CreationTime { get; set; }

        public string CreatorUserName { get; set; }
    }
}
