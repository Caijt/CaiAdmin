using System;
using System.Collections.Generic;
using System.Text;

namespace CaiAdmin.Service.Fcm
{
    public class ShipmentDto
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 运单号
        /// </summary>
        public string ShipmentNo { get; set; }
        public DateTime CreationTime { get; set; }
        public string CreatorUserName { get; set; }
    }
}
