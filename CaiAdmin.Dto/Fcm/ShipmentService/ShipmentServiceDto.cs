using System;
using System.Collections.Generic;
using System.Text;

namespace CaiAdmin.Service.Fcm
{
    public class ShipmentServiceDto
    {
        public Guid Id { get; set; }
        public string ServiceNo { get; set; }
        public DateTime CreationTime { get; set; }

        public string CreatorUserName { get; set; }
    }
}
