using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CaiAdmin.Dto.Fam
{
    /// <summary>
    /// 发票状态
    /// </summary>
    public enum InvoiceStatus
    {
        /// <summary>
        /// 已申请
        /// </summary>
        [Description("已申请")]
        Submitted,

        /// <summary>
        /// 新增蓝票
        /// </summary>
        [Description("新增蓝票")]
        BlueInvoice,

        /// <summary>
        /// 作废票
        /// </summary>
        [Description("作废票")]
        VoidedInvoice,

        /// <summary>
        /// 冲红红票
        /// </summary>
        [Description("冲红红票")]
        RedInvoice,

        /// <summary>
        /// 重开蓝票
        /// </summary>
        [Description("重开蓝票")]
        ReBlueInvoice,

        /// <summary>
        /// 新增蓝票已冲红
        /// </summary>
        [Description("新增蓝票已冲红")]
        BlueInvoiceWithRed,
        /// <summary>
        /// 开票失败
        /// </summary>
        [Description("开票失败")]
        Fail
    }
}
