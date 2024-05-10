using System;
using System.Collections.Generic;
using System.Text;

namespace CaiAdmin.ApiService.CityOcean.Dto
{
    public class ElasticSearchDataChangedEventBase
    {
        /// <summary>
        /// 运单id
        /// </summary>
        public Guid? ShipmentId { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime? LastModificationTime { get; set; }
        /// <summary>
        /// 标识id
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// 页索引
        /// </summary>
        public int PageIndex { get; set; } = 0;
        /// <summary>
        /// 每页记录数
        /// </summary>
        public int PageSize { get; set; } = 1000;
    }
}
