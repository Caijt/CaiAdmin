using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CaiAdmin.Service.Crm
{
    public enum OldIcpCustomerTitleType
    {
        /// <summary>
        /// 专用发票
        /// </summary>
        [Description("专用发票")]
        Special = 1,
        [Description("普通发票")]
        Ordinary = 2
    }
}
