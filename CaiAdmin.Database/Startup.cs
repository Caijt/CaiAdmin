using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Pomelo.EntityFrameworkCore.MySql.Storage;
using SqlSugar;
using CaiAdmin.Database.SqlSugar;
using System.Reflection;
using CaiAdmin.Entity;

[assembly: HostingStartup(typeof(CaiAdmin.Database.Startup))]
namespace CaiAdmin.Database
{
    public class Startup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                services.AddScoped(typeof(Repository<>));
                foreach (var t in Assembly.GetExecutingAssembly().GetTypes())
                {
                    if (t.IsAbstract || t.IsInterface || !t.IsVisible)
                    {
                        continue;
                    }
                    if (t.Name.EndsWith("Repository"))
                    {
                        services.AddScoped(t);
                    }
                }
                services.AddDbContext<AppDbContext>(o =>
                {
                    this.DbContextConfig(o, context.Configuration);
                });
                services.AddSqlSugar(new List<SqlSugarConnectionConfig> {
                    new SqlSugarConnectionConfig{
                         ConnectionString = context.Configuration.GetConnectionString("MySql"),
                         DbType = DbType.MySql,
                         IsAutoCloseConnection = true,
                         ConfigId = SqlSugarConfigId.Default,
                         EnableLowerUnderscoreName = true,
                         EnableNamespaceNamePrefix = true
                     },
                     new SqlSugarConnectionConfig{
                         ConnectionString = context.Configuration["Cityocean:Database"],
                         DbType = DbType.SqlServer,
                         IsAutoCloseConnection = true,
                         ConfigId = SqlSugarConfigId.CityOcean
                     }
                });
            });
        }

        public Action<DbContextOptionsBuilder, IConfiguration> DbContextConfig = (builder, configuration) =>
         {
             //builder.UseMySql(configuration.GetConnectionString("Mysql"), b =>
             //{
             //    b.ServerVersion(ServerVersion.NullableGeneratedColumnsMySqlSupportVersionString);
             //    //b.CharSet(new CharSet("utf8mb4", 250));
             //    var charset = CharSet.Utf8Mb4;
             //    b.CharSet(new CharSet(charset.Name, 2));
             //});
             builder.UseSqlServer(configuration.GetConnectionString("SqlServer"), b =>
             {
                 //2008数据库
                 //b.UseRowNumberForPaging();
             });
         };
    }
}
