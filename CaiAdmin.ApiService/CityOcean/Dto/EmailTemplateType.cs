using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CaiAdmin.ApiService.CityOcean.Dto
{
    /// <summary>
    /// 模板类型
    /// </summary>
    public enum EmailTemplateType
    {
        /// <summary>
        /// 送货进仓通知书
        /// </summary>
        [Description("送货进仓通知书")]
        InWarehouseNotice,
        /// <summary>
        /// 海运出口订舱委托书  TODO
        /// </summary>
        [Description("海运出口订舱委托书")]
        BookingNote,
        /// <summary>
        /// 订舱确认书 
        /// </summary>
        [Description("订舱确认书")]
        BookingReceiptNotice,
        /// <summary>
        /// 提单补料通知  TODO
        /// </summary> 
        [Description("提单补料通知")]
        ShippingInstruction,
        /// <summary>
        /// 提单确认通知书  TODO
        /// </summary>
        [Description("提单确认通知书")]
        ConfirmationOfBillOfLading,
        /// <summary>
        /// 分提单  TODO  =>  BillOfLading,
        /// </summary>
        [Description("海运分提单")]
        HouseBillOfLading,

        /// <summary>
        /// 到港通知书 A/N（Arrival Notice） CO发给客户的到港通知书，在收到船东的A/N之后
        /// </summary>
        [Description("到港通知书 A/N")]
        ArrivalNotice,

        /// <summary>
        /// 提货通知书 D/O(Delivery Order)
        /// </summary>
        [Description("提货通知书 D/O")]
        [Obsolete("FCM已废弃")]
        DeliveryOrder,

        /// <summary>
        /// 操作联系单 TODO
        /// </summary>
        [Description("操作联系单")]
        WorksheetOfOceanExport,

        /// <summary>
        /// 取消订舱委托
        /// </summary>
        [Description("取消订舱委托")]
        CancelBooking,

        /// <summary>
        /// 主提单
        /// </summary>
        [Description("海运主提单")]
        MasterBillOfLading,
        /// <summary>
        /// 侧唛
        /// </summary>
        [Description("侧唛")]
        SideMarks,

        /// <summary>
        /// 提货通知书
        /// </summary>
        [Description("提货通知书")]

        DeliverNotice,

        /// <summary>
        /// 空运分提单
        /// </summary>
        [Description("空运分提单")]
        HAWB,

        /// <summary>
        /// 空运主提单
        /// </summary>
        [Description("空运主提单")]
        MAWB,

        /// <summary>
        /// 出口拖车单
        /// </summary>
        [Description("出口拖车单")]
        ExportPickupOrder = 15,


        /// <summary>
        /// 出口已放单通知（美加线电放） 出口已放单通知（美加线电放）
        /// </summary>
        [Description("出口已放单通知（美加线电放）")]
        USLineTelexRelease = 16,
        /// <summary>
        /// 出口已放单通知（欧杂先电放SWB） 欧杂线放单（电放  SWB）邮件通知
        /// </summary>
        [Description("出口已放单通知（欧杂先电放SWB）")]
        EURLineTelexRelease = 17,
        /// <summary>
        /// 出口已放单通知（海外部指定货MBL） 海外部指定货MBL放单
        /// </summary>
        [Description("出口已放单通知（海外部指定货MBL）")]
        OverseasMBLReleaseOrder = 18,
        /// <summary>
        /// 出口已放单通知（正本） 放单（正本提单）邮件通知
        /// </summary>
        [Description("出口已放单通知（正本）")]
        BLReleaseOrder = 19,
        /// <summary>
        /// 催客户放单邮件
        /// </summary>
        [Description("催客户放单邮件通知")]
        PressReleaseOrder = 20,
        /// <summary>
        /// 变更放单类型通知
        /// </summary>
        [Description("放单类型变更通知")]
        ChangeReleaseOrderType = 21,
        /// <summary>
        /// 取消放单邮件通知
        /// </summary>
        [Description("放单已取消通知")]
        CancelReleaseOrder = 22
    }
}
