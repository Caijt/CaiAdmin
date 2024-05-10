using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CaiAdmin.Dto.Fam
{
    /// <summary>
    /// 提单放单状态
    /// </summary>
    public enum ReleaseOrderState : byte
    {
        /// <summary>
        /// 待放
        /// </summary>
        [Description("待放")]
        Pending = 1,

        /// <summary>
        /// 已放
        /// </summary>
        [Description("已放")]
        Released = 2
    }
}
