using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaiAdmin.Database.SqlSugar
{
    public class SqlSugarConnectionConfig
    {
        /// <summary>
        /// 数据库类型
        /// </summary>
        public DbType DbType { get; set; }

        public SqlSugarConfigId ConfigId { get; set; }
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public string ConnectionString { get; set; }
        /// <summary>
        /// 是否自动关闭事务
        /// </summary>
        public bool IsAutoCloseConnection { get; set; } = true;
        /// <summary>
        /// 启用表名命名空间命名规则
        /// </summary>
        public bool EnableNamespaceNamePrefix { get; set; }
        /// <summary>
        /// 是否开启表名、列名小写加下划线命名方式
        /// </summary>
        public bool EnableLowerUnderscoreName { get; set; }
    }
}
