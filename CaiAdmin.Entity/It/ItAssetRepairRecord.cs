using ItSys.Entity;
using CaiAdmin.Entity.It;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CaiAdmin.Entity.It
{
    public class AssetRepairRecord : AuditEntity
    {
        public int asset_id { get; set; }
        [ForeignKey("asset_id")]
        public Asset Asset { get; set; }
        public int? supplier_id { get; set; }
        [ForeignKey("supplier_id")]
        public Supplier Supplier { get; set; }
        public string reason { get; set; }
        public string content { get; set; }
        public DateTime repair_date { get; set; }
        public DateTime? finish_date { get; set; }
        public bool is_finish { get; set; }
        public decimal price { get; set; }

    }
}
