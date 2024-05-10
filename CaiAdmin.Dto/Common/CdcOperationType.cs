using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CaiAdmin.Dto
{
    public enum CdcOperationType
    {
        /// <summary>
        /// 删除
        /// </summary>
        [Description("删除")]
        Delete = 1,
        [Description("创建")]
        Create = 2,
        [Description("更新[旧值]")]
        UpdateOld = 3,
        [Description("更新[新值]")]
        UpdateNew = 4
    }
}
