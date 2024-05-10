using CaiAdmin.Common;
using CaiAdmin.Service;
using CaiAdmin.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace CaiAdmin.Service
{
    /// <summary>
    /// 查询过滤器接口
    /// </summary>
    /// <typeparam name="TQuery">查询对象</typeparam>
    public interface IQueryFilter<TQuery>
    {
        IQueryable<TQuery> Filter(IQueryable<TQuery> query, object service);
    }
    
}
