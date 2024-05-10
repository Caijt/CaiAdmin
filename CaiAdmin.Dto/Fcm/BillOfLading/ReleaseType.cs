using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CaiAdmin.Dto.Fcm
{
    /// <summary>
    /// 放单放货类型
    /// </summary>
    public enum ReleaseType : byte
    {
        /// <summary>
        /// 
        /// </summary>
        [Description("不确定")] 
        NotSet = 0,

        /// <summary>
        /// 正本提单
        /// </summary>
        [Description("正本提单")] 
        Original = 1,

        /// <summary>
        /// 电放提单
        /// </summary>
        [Description("电放提单")] 
        Telex = 2,

        /// <summary>
        /// 海运提单
        /// </summary>
        [Description("海运提单")] 
        SWB = 3
    }
}
