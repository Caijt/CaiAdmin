using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaiAdmin.Database
{

    public class Repository : ISugarRepository
    {
        public ISqlSugarClient Context { get; set; }
        public Repository(ISqlSugarClient context)
        {
            this.Context = context;
        }
    }

    public class Repository<T> : SimpleClient<T> where T : class, new()
    {
        public Repository(ISqlSugarClient context = null) : base(context)
        {
            
        }
    }
}
