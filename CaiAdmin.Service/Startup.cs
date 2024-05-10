using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using System.Reflection;
using System.Linq;
using Microsoft.Extensions.DependencyInjection.Extensions;
using CaiAdmin.Common;
using CaiAdmin.Service.Common;
using SqlSugar;

[assembly: HostingStartup(typeof(CaiAdmin.Service.Startup))]
[assembly: DependOn(typeof(CaiAdmin.Database.Startup))]
[assembly: DependOn(typeof(CaiAdmin.ApiService.Startup))]

namespace CaiAdmin.Service
{
    public class Startup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                services.AddScoped<AuthContext>();
                services.AddScoped<ConfigContext>();
                foreach (var t in Assembly.GetExecutingAssembly().GetTypes())
                {
                    if (t.IsAbstract || t.IsInterface || !t.IsVisible)
                    {
                        continue;
                    }
                    if (t.Name.EndsWith("Service"))
                    {
                        services.AddTransient(t);
                    }
                    else if (t.IsGenericType)
                    {
                        if (t.GetInterfaces().Any(e => e.GetGenericTypeDefinition() == typeof(IQueryFilter<>)))
                        {
                            services.AddScoped(typeof(IQueryFilter<>), t);
                        }
                        else if (t.GetInterfaces().Any(e => e.GetGenericTypeDefinition() == typeof(IEntityHandler<>)))
                        {
                            services.AddScoped(typeof(IEntityHandler<>), t);
                        }
                    }
                }
                services.AddAutoMapper(typeof(MapperConfiguration.Configuration));                
            });
        }
    }
}
