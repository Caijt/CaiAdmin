using CaiAdmin.Entity.Sys;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ItSys.Entity
{
    public abstract class AuditEntity : IdEntity
    {
        [Column("create_time")]
        public DateTime create_time { get; set; }

        [Column("create_user_id")]
        public int create_user_id { get; set; }
        [Navigate( NavigateType.OneToOne,nameof(create_user_id))]
        public User CreateUser { get; set; }
        [Column("update_time")]
        public DateTime update_time { get; set; }

        [Column("update_user_id")]
        public int update_user_id { get; set; }
    }
}
