using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CaiAdmin.Dto.Wf
{
    public enum TaskProcessStatus
    {
        /// <summary>
        /// 未处理
        /// </summary>
        [Description("未处理")]
        Untreated = 0,
        /// <summary>
        /// 未通过
        /// </summary>
        [Description("未通过")]
        Unpassed = 1,
        /// <summary>
        /// 通过
        /// </summary>
        [Description("通过")]
        Passed = 2,
    }
}
