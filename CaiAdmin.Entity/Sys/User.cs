using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaiAdmin.Entity.Sys
{
    public class User : IEntity<int>, ICreateTime, IUpdateTime
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }
        public string LoginName { get; set; }
        public string LoginPassword { get; set; }
        public int RoleId { get; set; }
        [Navigate(NavigateType.ManyToOne, nameof(RoleId))]
        public Role Role { get; set; }
        public bool IsDisabled { get; set; }
        [SugarColumn(IsOnlyIgnoreUpdate = true)]
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }

    }
}
