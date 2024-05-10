using System;
using System.Collections.Generic;
using System.Text;

namespace CaiAdmin.ApiService.CityOcean.Dto
{
    public enum CustomerRelationType : byte
    {
        /// <summary>
        /// 联系人
        /// </summary>
        Contact = 1,
        /// <summary>
        /// 位置
        /// </summary>
        Location = 2,
        /// <summary>
        /// 合作伙伴
        /// </summary>
        Partner = 3,
        /// <summary>
        /// 抬头
        /// </summary>
        CustomerTitle = 4,
        /// <summary>
        /// 文档
        /// </summary>
        Document = 5
    }
}
