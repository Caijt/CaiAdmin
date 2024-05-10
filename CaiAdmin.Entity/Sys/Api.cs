using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaiAdmin.Entity.Sys
{
    public class Api : IEntity<int>
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }
        /// <summary>
        /// 接口名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 接口路径
        /// </summary>
        public string Path { get; set; }
        [SugarColumn(IsIgnore = true)]
        public bool IsCommon { get; set; }
        [SugarColumn(IsIgnore = true)]
        public ApiPermissionType PermissionType { get; set; }
        [SugarColumn(IsIgnore = true)]
        public List<MenuApi> MenuApis { get; set; }
    }
    public enum ApiPermissionType
    {
        READ,
        WRITE,
        REVIEW
    }
}
