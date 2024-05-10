using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaiAdmin.Entity.Sys
{
    public class Role : IEntity<int>, ICreateTime, IUpdateTime
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Remarks { get; set; }
        [SugarColumn(IsIgnore = true)]
        public List<RoleMenu> RoleMenus { get; set; }
        [SugarColumn(IsIgnore = true)]
        public List<User> Users { get; set; }
        [SugarColumn(IsOnlyIgnoreUpdate = true)]
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }

    }
}
