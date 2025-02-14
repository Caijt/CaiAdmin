﻿using Microsoft.AspNetCore.Hosting;
using CaiAdmin.ApiService.Qywx;
using Microsoft.Extensions.DependencyInjection;
using CaiAdmin.ApiService.QQMap;

[assembly: HostingStartup(typeof(CaiAdmin.ApiService.Startup))]
namespace CaiAdmin.ApiService
{
    public class Startup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddHttpClient<QywxApiService>();
                services.AddHttpClient<QQMapApiService>();
            });
        }
    }
}
