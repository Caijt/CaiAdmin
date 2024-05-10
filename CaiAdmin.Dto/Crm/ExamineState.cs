using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CaiAdmin.Service.Crm
{
    public enum ExamineState
    {
        /// <summary>
        /// 占位
        /// </summary>
        [Description("无")]
        NotSet = 0,

        /// <summary>
        /// 等待审批
        /// </summary>
        [Description("等待审批")]
        WaitingExamine = 1,

        /// <summary>
        /// 审批通过
        /// </summary>
        [Description("审批通过")]
        PassExamine = 2,

        /// <summary>
        /// 驳回审批
        /// </summary>
        [Description("驳回审批")]
        RefuseExamine = 3

    }

    public enum OldIcpExamineState
    {
        /// <summary>
        /// 占位
        /// </summary>
        [Description("无")]
        NotSet = 0,

        /// <summary>
        /// 
        /// </summary>
        [Description("正在处理")]
        WaitingExamine = 1,

        /// <summary>
        /// 
        /// </summary>
        [Description("已经通过")]
        PassExamine = 3,

        /// <summary>
        /// 
        /// </summary>
        [Description("未通过")]
        RefuseExamine = 4

    }
}
