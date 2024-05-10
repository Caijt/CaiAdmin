using System;
using System.Collections.Generic;
using System.Text;

namespace CaiAdmin.Dto.Sys
{
    public class ApiQueryDto : QueryDto
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public int[] NotIds { get; set; }
    }
}
