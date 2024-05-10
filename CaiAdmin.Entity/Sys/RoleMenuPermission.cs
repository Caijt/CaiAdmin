using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaiAdmin.Entity.Sys
{
    public class RoleMenuPermission : IEntity
    {
        [SugarColumn(IsPrimaryKey = true)]
        public int RoleId { get; set; }
        [SugarColumn(IsPrimaryKey = true)]
        public int MenuId { get; set; }

        [SugarColumn(IsPrimaryKey = true, Length = 100)]
        public string PermissionCode { get; set; }
    }
}
