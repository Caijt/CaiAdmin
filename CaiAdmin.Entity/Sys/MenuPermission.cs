using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaiAdmin.Entity.Sys
{
    public class MenuPermission : IEntity
    {
        /// <summary>
        /// 权限值
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, Length = 100)]
        public string Code { get; set; }
        /// <summary>
        /// 菜单ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public int MenuId { get; set; }
        public string Name { get; set; }

        public int Order { get; set; }

    }
}
