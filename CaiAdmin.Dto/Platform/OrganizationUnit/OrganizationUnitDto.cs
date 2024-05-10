using CaiAdmin.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaiAdmin.Dto.Platform
{
    public class OrganizationUnitDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public Guid ParentId { get; set; }

        public string LevelCode { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public OrganizationUnitType Type { get; set; }

        public bool IsDeleted { get; set; }

        public string TypeName
        {
            get
            {
                return CommonHelper.GetDescription(Type); ;
            }
        }
    }
}
