using CaiAdmin.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CaiAdmin.Service.Crm
{
    public class CustomerDetailDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string EnName { get; set; }
        public string ZhName { get; set; }

        public string Code { get; set; }

        public string Tel { get; set; }
        public string Fax { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public string NameLocalizationUniqueValue { get; set; }

        public string NameUniqueValue { get; set; }
        public ExamineState ExamineState { get; set; }
        public string ExamineStateName
        {
            get
            {
                return CommonHelper.GetDescription(ExamineState);
            }
        }
        public string Address { get; set; }
        public string EnAddress { get; set; }
        public string ZhAddress { get; set; }

        public string OwnerUserName { get; set; }

        public DateTime? ClaimTime { get; set; }

        /// <summary>
        /// 上次交易时间
        /// </summary>
        public DateTime? LastTradeTime { get; set; }
        /// <summary>
        /// 上次跟进时间
        /// </summary>
        public DateTime? LastTraceLogTime { get; set; }

        /// <summary>
        /// 上次报价时间
        /// </summary>
        public DateTime? LastQuotationTime { get; set; }

        /// <summary>
        /// 成交状态
        /// </summary>
        public CooperationState CooperationState { get; set; }

        /// <summary>
        /// 成交状态名称
        /// </summary>
        public string CooperationStateName
        {
            get
            {
                return CommonHelper.GetDescription(CooperationState);
            }
        }


        public List<CustomerOwnUserDto> OwnUsers { get; set; }
        /// <summary>
        /// 操作事件
        /// </summary>
        public List<CustomerOperationEventDto> OperationEvents { get; set; }
        /// <summary>
        /// 审批记录
        /// </summary>
        public List<CustomerExamineDto> Examines { get; set; }

        public Guid MergerId
        { get; set; }
        public DateTime CreationTime { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public bool SyncOldIcp { get; set; }

        public bool IsDeleted { get; set; }

        /// <summary>
        /// 客户类型
        /// </summary>
        public CustomerType CustomerType { get; set; }

        public string CustomerTypeName
        {
            get
            {
                return CommonHelper.GetDescription(CustomerType);
            }
        }
    }

    public class CustomerOwnUserDto
    {
        public long Id { get; set; }
        /// <summary>
        /// 所有人姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 认领时间
        /// </summary>
        public DateTime CreationTime { get; set; }
    }

    public class CustomerOperationEventDto
    {
        public DateTime CreationTime { get; set; }
        public string CreatorUserName { get; set; }
        public string Content { get; set; }
    }

    public class CustomerExamineDto
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }

        public ExamineType ExamineType { get; set; }

        public string ExamineTypeName
        {
            get
            {
                return CommonHelper.GetDescription(ExamineType);
            }
        }
        public ExamineState ExamineState { get; set; }
        public string ExamineStateName
        {
            get
            {
                return CommonHelper.GetDescription(ExamineState);
            }
        }
        public DateTime? ExamineTime { get; set; }
        public long? ExamineUserId { get; set; }
        public string ExamineUserName { get; set; }
        public string RefuseReason { get; set; }
        public string CreatorUserName { get; set; }
        public DateTime CreationTime { get; set; }

        public string ApplyRemark { get; set; }

        public string ApprovalContent { get; set; }
        public string BeforeContent { get; set; }
    }

    public enum ExamineType
    {
        NoteSet = 0,

        /// <summary>
        /// 变更名称
        /// </summary>
        [Description("变更名称")]
        UpdateName = 1,

        /// <summary>
        /// 申请代码
        /// </summary>
        [Description("申请代码")]
        PostCode = 2,
        /// <summary>
        /// 变更客户电话
        /// </summary>
        [Description("变更客户电话")]
        UpdatePhone = 3,
        /// <summary>
        /// 变更客户地址
        /// </summary>
        [Description("变更客户地址")]
        UpdateAddress = 4
    }

    public enum CooperationState
    {
        [Description("无")]
        NotSet = 0,

        /// <summary>
        /// 未合作
        /// </summary>
        [Description("未合作")]
        NoneCooperation = 1,

        /// <summary>
        /// 合作过
        /// </summary>
        [Description("合作过")]
        HaveCooperation = 2
    }
}
