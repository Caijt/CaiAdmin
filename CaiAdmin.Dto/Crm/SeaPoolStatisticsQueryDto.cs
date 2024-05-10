using System;
using System.Collections.Generic;
using System.Text;

namespace CaiAdmin.Dto.Crm
{
    public class SeaPoolStatisticsQueryDto : QueryDto
    {
        public DateTime? AllocationTimeBegin { get; set; }
        public DateTime? AllocationTimeEnd { get; set; }

        public Guid[] CompanyIds { get; set; }

        public SeaPoolStatisticsType[] Types { get; set; }
    }

    public enum SeaPoolStatisticsType
    {
        /// <summary>
        /// 普通公海池
        /// </summary>
        Sea = 0,
        /// <summary>
        /// 优质客户
        /// </summary>
        HighValue = 1
    }
}
