using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaiAdmin.Entity.Sys
{
    /// <summary>
    /// 菜单权限接口
    /// </summary>
    public class MenuPermissionApi : IEntity
    {
        [SugarColumn(IsPrimaryKey = true)]
        public int MenuId { get; set; }
        [SugarColumn(IsPrimaryKey = true)]
        public string PermissionCode { get; set; }
        [SugarColumn(IsPrimaryKey = true)]
        public int ApiId { get; set; }
    }
}
