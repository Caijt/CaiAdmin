using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaiAdmin.Entity.Sys
{
    public class RoleMenu : IEntity
    {
        [SugarColumn(IsPrimaryKey = true)]
        public int RoleId { get; set; }
        //[SugarColumn(IsIgnore = true)]
        //public Role Role { get; set; }
        [SugarColumn(IsPrimaryKey = true)]
        public int MenuId { get; set; }
        //[SugarColumn(IsIgnore = true)]
        //public Menu Menu { get; set; }

        //public bool CanRead { get; set; }
        //public bool CanWrite { get; set; }
        //public bool CanReview { get; set; }

    }
}
