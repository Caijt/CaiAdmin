using System;
using System.Collections.Generic;
using System.Text;

namespace CaiAdmin.Dto.Pub
{
    public class ConfigureQueryDto : QueryDto
    {
        public string CompanyName { get; set; }
        public string CustomerName { get; set; }
    }
}
