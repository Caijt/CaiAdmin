using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CaiAdmin.Dto.Platform
{
    public enum OrganizationUnitType
    {
        /// <summary>
        /// 集团总部
        /// </summary>
        [Description("集团总部")]
        Head = 32,

        /// <summary>
        /// 业务版块
        /// </summary>
        [Description("业务版块")]
        Section = 16,

        /// <summary>
        /// 区域
        /// </summary>
        [Description("区域")]
        Region = 8,

        /// <summary>
        /// 公司
        /// </summary>
        [Description("公司")]
        Company = 4,

        /// <summary>
        /// 部门
        /// </summary>
        [Description("部门")]
        Department = 2,

        /// <summary>
        /// 组
        /// </summary>
        [Description("组")]
        Group = 1
    }
}
