using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaiAdmin.Entity.Sys
{
    public class MenuApi : IEntity
    {
        [SugarColumn(IsPrimaryKey = true)]
        public int ApiId { get; set; }
        //public Api Api { get; set; }
        [SugarColumn(IsPrimaryKey = true)]
        public int MenuId { get; set; }
        //public Menu Menu { get; set; }
    }
}
