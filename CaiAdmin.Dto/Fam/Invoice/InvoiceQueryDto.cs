using System;
using System.Collections.Generic;
using System.Text;

namespace CaiAdmin.Dto.Fam
{
    public class InvoiceQueryDto : QueryDto
    {
        /// <summary>
        /// 发票号
        /// </summary>
        public string InvoiceNo { get; set; }
        /// <summary>
        /// 发票代码
        /// </summary>
        public string InvoiceCode { get; set; }
        /// <summary>
        /// 流水号
        /// </summary>
        public string SerialNo { get; set; }
        public string BuyerTitleName { get; set; }
        public string SellerTitleName { get; set; }
        public int[] NotIds { get; set; }
        public int[] Ids { get; set; }

        public InvoiceStatus[] Statuses { get; set; }
    }
}
