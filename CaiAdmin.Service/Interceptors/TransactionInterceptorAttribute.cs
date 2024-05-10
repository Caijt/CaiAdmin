using AspectCore.DynamicProxy;
using AspectCore.DynamicProxy.Parameters;
using CaiAdmin.Common;
using CaiAdmin.Common.CacheHelper;
using CaiAdmin.Database;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;
using System;
using System.Threading.Tasks;

namespace CaiAdmin.Service.Interceptors
{
    /// <summary>
    /// 事务拦截器
    /// </summary>
    public class TransactionInterceptorAttribute : AbstractInterceptorAttribute
    {
        public async override Task Invoke(AspectContext context, AspectDelegate next)
        {
            var db = context.ServiceProvider.GetService<ISqlSugarClient>();
            //先判断是否已经启用了事务
            if (db.Ado.Transaction == null)
            {
                try
                {
                    db.Ado.BeginTran();
                    await next(context);
                    db.Ado.CommitTran();
                }
                catch(Exception ex)
                {
                    db.Ado.RollbackTran();
                    throw;
                }
            }
            else
            {
                await next(context);
            }
        }
    }




}
