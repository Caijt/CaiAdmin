using CaiAdmin.Common;
using CaiAdmin.Entity;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace CaiAdmin.Database.SqlSugar
{
    public static class SqlSugarExtensions
    {
        public static IServiceCollection AddSqlSugar(this IServiceCollection serviceCollection, List<SqlSugarConnectionConfig> configs)
        {
            var connectionConfigs = new List<ConnectionConfig>();
            //serviceCollection.BuildServiceProvider().
            foreach (var item in configs)
            {
                connectionConfigs.Add(new ConnectionConfig()
                {
                    DbType = item.DbType,
                    ConnectionString = item.ConnectionString,
                    IsAutoCloseConnection = item.IsAutoCloseConnection,
                    ConfigId = item.ConfigId,
                    //MoreSettings = new ConnMoreSettings
                    //{
                    //    TableEnumIsString = true
                    //},
                    ConfigureExternalServices = new ConfigureExternalServices
                    {
                        EntityNameService = (type, entity) =>
                        {
                            bool isEntity = typeof(IEntity).IsAssignableFrom(type);
                            bool isQuery = typeof(IQuery).IsAssignableFrom(type);
                            if (!isEntity && !isQuery)
                            {
                                return;
                            }
                            if (!item.EnableLowerUnderscoreName && !item.EnableNamespaceNamePrefix)
                            {
                                return;
                            }
                            var tableAttr = type.GetCustomAttribute<SugarTable>();
                            if (tableAttr != null && !string.IsNullOrWhiteSpace(tableAttr.TableName))
                            {
                                return;
                            }

                            var assembly = Assembly.GetAssembly(type);
                            string assemblyName = assembly.GetName().Name + ".";
                            var tableName = type.Name;
                            if (item.EnableNamespaceNamePrefix)
                            {
                                int index = type.Namespace.IndexOf(assemblyName);
                                if (index != -1 && type.Namespace.Length > assemblyName.Length)
                                {
                                    string prefix = type.Namespace.Substring(index + assemblyName.Length);
                                    tableName = prefix + tableName;
                                }
                            }
                            if (isQuery)
                            {
                                tableName = "V_" + tableName;
                            }
                            if (item.EnableLowerUnderscoreName)
                            {
                                tableName = CommonHelper.CamelCaseToLowerUnderScore(tableName);
                            }
                            entity.DbTableName = tableName;
                        },
                        EntityService = (prop, column) =>
                        {
                            bool isEntity = typeof(IEntity).IsAssignableFrom(prop.DeclaringType);
                            bool isQuery = typeof(IQuery).IsAssignableFrom(prop.DeclaringType);
                            if (!isEntity && !isQuery)
                            {
                                return;
                            }
                            if (item.EnableLowerUnderscoreName)
                            {
                                string columnName = CommonHelper.CamelCaseToLowerUnderScore(prop.Name);
                                column.DbColumnName = columnName;
                            }
                        }
                    }
                });
            }

            var sqlSugar = new SqlSugarScope(connectionConfigs,
            db =>
            {

                foreach (var item in configs)
                {
                    var currentDb = db.GetConnection(item.ConfigId);
                    currentDb.Aop.OnError = (ex) =>
                    {
                        Console.WriteLine($"OnError:SQL错误输出打印开始：{DateTime.Now.ToString()}");
                        Console.WriteLine(ex.Message);
                        Console.WriteLine(ex.Sql);//输出sql
                        var sqlParams = ex.Parametres as SugarParameter[];
                        foreach (var item in sqlParams)
                        {
                            Console.WriteLine(item.ParameterName + ":" + CommonHelper.ObjectToJsonString(item.Value));
                        }
                        Console.WriteLine($"OnError:SQL错误输出打印结束：{DateTime.Now.ToString()}");
                    };
#if DEBUG

                    //currentDb.Aop.OnLogExecuting = (sql, pars) =>
                    //{
                    //    Console.WriteLine($"OnLogExecuting:SQL输出打印开始：{DateTime.Now.ToString()}");
                    //    Console.WriteLine(sql);//输出sql
                    //    foreach (var item in pars)
                    //    {
                    //        Console.WriteLine(item.ParameterName + ":" + CommonHelper.ObjectToJsonString(item.Value));
                    //    }
                    //    Console.WriteLine($"OnLogExecuting:执行时间：{db.Ado.SqlExecutionTime.ToString()}");
                    //    Console.WriteLine($"OnLogExecuting:SQL输出打印结束：{DateTime.Now.ToString()}");
                    //};
                    //SQL执行完
                    currentDb.Aop.OnLogExecuted = (sql, pars) =>
                    {

                        Console.WriteLine($"OnLogExecutedSQL输出打印开始：{DateTime.Now.ToString()}");
                        Console.WriteLine(sql);//输出sql
                        foreach (var item in pars)
                        {
                            Console.WriteLine(item.ParameterName + ":" + CommonHelper.ObjectToJsonString(item.Value));
                        }
                        Console.WriteLine($"OnLogExecuted执行时间：{db.Ado.SqlExecutionTime.ToString()}");
                        ////代码CS文件名
                        //var fileName = db.Ado.SqlStackTrace.FirstFileName;
                        ////代码行数
                        //var fileLine = db.Ado.SqlStackTrace.FirstLine;
                        ////db.Ado.SqlStackTrace.MyStackTraceList[1]
                        ////方法名
                        //var FirstMethodName = db.Ado.SqlStackTrace.FirstMethodName;
                        //Console.WriteLine($"代码位置：{fileName}/{FirstMethodName}[{fileLine}行]");
                        Console.WriteLine($"OnLogExecutedSQL输出打印结束：{DateTime.Now.ToString()}");
                    };
#endif
                    //数据新增更新过滤
                    currentDb.Aop.DataExecuting = (value, entityInfo) =>
                    {
                        //if (entityInfo.PropertyName == nameof(ICreateTime.CreateTime) && entityInfo.OperationType == DataFilterType.InsertByObject)
                        //{
                        //    entityInfo.SetValue(DateTime.Now);
                        //}
                        //if (entityInfo.PropertyName == nameof(IUpdateTime.UpdateTime))
                        //{
                        //    entityInfo.SetValue(DateTime.Now);
                        //}
                    };
                }

            });
            serviceCollection.AddSingleton<ISqlSugarClient>(sqlSugar);//这边是SqlSugarScope用AddSingleton
            return serviceCollection;
        }


    }
}
