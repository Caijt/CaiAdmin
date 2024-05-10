using System;
using System.Collections.Generic;
using System.Text;

namespace CaiAdmin.Dto.Sys
{
    public class BaseDataQueryDto : QueryDto
    {
        public string Name { get; set; }
        public bool? IsDisabled { get; set; }
    }

}
