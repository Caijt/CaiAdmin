using System;
using System.Collections.Generic;
using System.Text;

namespace CaiAdmin.Dto.Crm
{

    public class A
    {
        public long AllocationUserId { get; set; }
        public string AllocationUserName { get; set; }

        public int Total { get; set; }
    }

    public class BQuery : QueryDto
    {
        public DateTime? AllocationTimeBegin { get; set; }
        public DateTime? AllocationTimeEnd { get; set; }

        public Guid[] CompanyIds { get; set; }

        public SeaPoolStatisticsType[] Types { get; set; }

        public string UserName { get; set; }
    }
}
