using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CaiAdmin.Dto.Wf
{
    public enum WorkflowStatus
    {
        /// <summary>
        /// 草稿
        /// </summary>
        [Description("草稿")]
        Draft = 0,
        /// <summary>
        /// 待办
        /// </summary>
        [Description("待处理")]
        Waiting = 1,
        /// <summary>
        /// 处理中
        /// </summary>
        [Description("处理中")]
        Processing = 2,

        /// <summary>
        /// 驳回
        /// </summary>
        [Description("驳回")]
        Reject = 3,

        /// <summary>
        /// 办结
        /// </summary>
        [Description("办结")]
        Finished = 4,

        /// <summary>
        /// 取消/撤回
        /// </summary>
        [Description("撤回")]
        Cancel = 5,

        /// <summary>
        /// 作废
        /// </summary>
        [Description("作废")]
        Unvalid = 6,
        /// <summary>
        /// 异常
        /// </summary>
        [Description("异常")]
        Exception = 7

    }
}
