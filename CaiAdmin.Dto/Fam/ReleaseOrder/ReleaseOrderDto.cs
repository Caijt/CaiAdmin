using CaiAdmin.Common;
using CaiAdmin.Dto.Fcm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CaiAdmin.Dto.Fam
{
    /// <summary>
    /// 放单任务
    /// </summary>
    public class ReleaseOrderDto
    {
        public Guid Id { get; set; }
        public string No { get; set; }
        public Guid ShipmentId { get; set; }
        /// <summary>
        /// 运单号
        /// </summary>
        public string ShipmentNo { get; set; }
        public ReleaseOrderState State { get; set; }
        public string StateName
        {
            get
            {
                return CommonHelper.GetDescription(State);
            }
        }
        /// <summary>
        /// 提单类型
        /// </summary>
        public BillOfLadingType Type { get; set; }
        public string TypeName
        {
            get
            {
                return Type.ToString();
            }
        }
        /// <summary>
        /// 放单类型
        /// </summary>
        public ReleaseType ReleaseType { get; set; }

        public string ReleaseTypeName
        {
            get
            {
                return CommonHelper.GetDescription(ReleaseType);
            }
        }

        public DateTime CreationTime { get; set; }

        public string CreatorUserName { get; set; }
    }

    /// <summary>
    /// 提单类型
    /// </summary>
    public enum BillOfLadingType
    {
        MBL = 0,
        HBL = 1
    }

    

   
}
