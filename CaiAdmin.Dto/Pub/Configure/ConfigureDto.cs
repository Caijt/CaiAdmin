using CaiAdmin.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaiAdmin.Dto.Pub
{
    public class ConfigureDto
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public string CompanyName { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; }

        public bool IsDeleted { get; set; }

        public string StandardCurrency { get; set; }
        public string DefaultCurrency { get; set; }
        public string ShortCode { get; set; }
        /// <summary>
        /// 会计关账日期
        /// </summary>
        public DateTime AccountingClosingDate { get; set; }
        /// <summary>
        /// 商务关账日期
        /// </summary>
        public DateTime BusinessClosingDate { get; set; }
        /// <summary>
        /// 计费关账日期
        /// </summary>
        public DateTime ChargingClosingDate { get; set; }

        public DateTime CreationTime { get; set; }
        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime? LastModificationTime { get; set; }
    }
}
