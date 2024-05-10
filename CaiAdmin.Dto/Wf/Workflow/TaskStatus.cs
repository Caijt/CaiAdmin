using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CaiAdmin.Dto.Wf
{
    public enum TaskStatus
    {
        /// <summary>
        /// 待办
        /// </summary>
        [Description("待办")]
        Wait = 0,

        /// <summary>
        /// 处理中
        /// </summary>
        [Description("处理中")]
        Processing = 1,

        /// <summary>
        /// 已处理
        /// </summary>
        [Description("已处理")]
        Processed = 2,

        /// <summary>
        /// 打回
        /// </summary>
        [Description("打回")]
        Returned = 3
    }
}
