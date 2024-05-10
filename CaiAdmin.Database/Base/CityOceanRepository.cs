using CaiAdmin.Database.SqlSugar;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace CaiAdmin.Database
{
    public class CityOceanRepository : Repository
    {
        public CityOceanRepository(ISqlSugarClient context) : base(context)
        {
            base.Context = context.AsTenant().GetConnection(SqlSugarConfigId.CityOcean);
        }
    }

    public class CityOceanRepository<T> : SimpleClient<T> where T : class, new()
    {
        public CityOceanRepository(ISqlSugarClient context = null) : base(context?.AsTenant().GetConnection(SqlSugarConfigId.CityOcean))
        {

        }
    }
}
