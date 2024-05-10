using System;
using System.Collections.Generic;
using System.Text;

namespace CaiAdmin.ApiService.CityOcean.Dto
{
    public class ReleaseBillOfLadingEmailTemplateResult
    {
        /// <summary>
        /// 提单ID
        /// </summary>
        public Guid BillOfLadingId { get; set; }

        public string BillOfLadingNo { get; set; }
        /// <summary>
        /// 提单对应的邮件模板
        /// </summary>
        public EmailTemplateType? EmailTemplateType { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Subject { get; set; }
    }

}
