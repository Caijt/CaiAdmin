using CaiAdmin.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaiAdmin.Dto.Pub
{
    public class GLCodeDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public string Code { get; set; }

        public Guid? ParentId { get; set; }

        public string LevelCode { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public GLCodeType Type { get; set; }

        public string TypeName
        {
            get
            {
                return CommonHelper.GetDescription(Type); ;
            }
        }
    }
}
