using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CaiAdmin.Service.Crm
{
    /// <summary>
    /// 客户类型
    /// </summary>
    public enum CustomerType : byte
    {
        /// <summary>
        /// 船东
        /// </summary>
        [Description("船东")]
        Carrier = 1,
        /// <summary>
        /// 航空公司
        /// </summary>
        [Description("航空公司")]
        AirLine = 2,
        /// <summary>
        /// 货代(同行)
        /// </summary>
        [Description("货代(同行)")]
        Forwarding = 3,
        /// <summary>
        /// 直客
        /// </summary>
        [Description("直客")]
        DirectClient = 4,
        /// <summary>
        /// 拖车行
        /// </summary>
        [Description("拖车行")]
        Trucker = 5,
        /// <summary>
        /// 报关行
        /// </summary>
        [Description("报关行")]
        CustomsBroker = 6,
        /// <summary>
        /// 仓储
        /// </summary>
        [Description("仓储")]
        WareHouse = 7,
        /// <summary>
        /// 堆场
        /// </summary>
        [Description("堆场")]
        Storage = 8,
        /// <summary>
        /// 铁路
        /// </summary>
        [Description("铁路")]
        RailWay = 9,
        /// <summary>
        /// 快递
        /// </summary>
        [Description("快递")]
        Express = 10,
        /// <summary>
        /// 码头
        /// </summary>
        [Description("码头")]
        Terminal = 11,
        /// <summary>
        /// 其他
        /// </summary>
        [Description("其他")]
        Other = 12
    }
}
