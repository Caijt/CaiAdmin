using System;
using System.Collections.Generic;
using System.Text;

namespace CaiAdmin.Dto.Platform
{
    public class OrganizationUnitQueryDto : QueryDto
    {
        public string Name { get; set; }

        public OrganizationUnitType[] Types { get; set; }

        public string IdString { get; set; }

        public Guid[] Ids { get; set; }

        /// <summary>
        /// 是否包含父级
        /// </summary>
        public bool IncludeParent { get; set; }
        /// <summary>
        /// 是否包含子级
        /// </summary>
        public bool IncludeChildren { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool? IsDeleted { get; set; }
    }
}
