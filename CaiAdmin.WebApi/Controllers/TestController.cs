﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using CaiAdmin.ApiService.QQMap;
using CaiAdmin.ApiService.Qywx;
using CaiAdmin.Common;
using CaiAdmin.Entity.Sys;
using Microsoft.Extensions.Caching.Distributed;
using NSwag.Annotations;
using Namotion.Reflection;
using AspectCore.DependencyInjection;
using CaiAdmin.Service.Sys;
using CaiAdmin.Common.CacheHelper;
using System.Runtime.CompilerServices;
using CaiAdmin.Service.Interceptors;
using CaiAdmin.Service.Common;
using Microsoft.AspNetCore.Authentication;
using CaiAdmin.Service;
using AutoMapper.QueryableExtensions.Impl;
using log4net.Core;
using Microsoft.Extensions.Logging;
using System.Runtime.InteropServices.WindowsRuntime;
using CaiAdmin.Database;

namespace CaiAdmin.WebApi.Controllers
{
    [OpenApiTag("测试", Description = "测试用的")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [AllowAnonymous]
    public class TestController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _appDbContext;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly QywxApiService _qywxApiService;
        private IDistributedCache _Cache;
        private QQMapApiService _qqMapApiService;
        private readonly UserService _userService;
        private readonly ITest2 _test2;
        private readonly ICacheHelper _cacheHelper;
        private readonly ConfigContext _configContext;
        private readonly AuthService _authService;
        private readonly ILogger<TestController> _logger;
        private readonly TestEsService _testEsService;
        public TestController(IConfiguration configuration,
            AppDbContext appDbContext, IHttpClientFactory httpClientFactory,
            UserService userService, ICacheHelper cacheHelper,
            ConfigContext configContext,
            AuthService authService,
            ILogger<TestController> logger,
            TestEsService testEsService
            )
        {
            _appDbContext = appDbContext;
            _configuration = configuration;
            //ActionNameAttribute
            _httpClientFactory = httpClientFactory;
            _userService = userService;
            _cacheHelper = cacheHelper;
            _configContext = configContext;
            _authService = authService;
            _logger = logger;
            _testEsService = testEsService;
        }
        /// <summary>
        /// 测试
        /// </summary>
        /// <remarks>test</remarks>
        /// <returns></returns>
        public string Test()
        {
            return "MyName:";
        }
        public string Test2()
        {
            return "MyName2:" + _configuration["MyName2"];
        }
        public IActionResult Test3()
        {
            var apinew = new Api
            {
                Id = 2,
                Name = "dsfsdf",
                //Url = "adfsdf"
            };
            var api = _appDbContext.Set<Api>().Find(2);

            _appDbContext.Entry(api).CurrentValues.SetValues(apinew);
            var entry = _appDbContext.Entry(api);
            //entry.State = Microsoft.EntityFrameworkCore.EntityState.;
            _appDbContext.Entry(apinew).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            //_appDbContext.Entry(apinew).Property(e => e.Url).IsModified = false;
            _appDbContext.Set<Api>().Remove(api);
            var roles = _appDbContext.Set<Role>().Include(e => e.Users).ToList();

            return new JsonResult(roles);
        }

        public async Task<IActionResult> Test4()
        {
            var client = _httpClientFactory.CreateClient("qywx");
            string corpId = "wx496d8a63d89e6755";
            string secret = "fnxYMnK-zjjwPS0pvA-j0f5j3buj8dCU7sjpC-ck1Jnn2Cu00tCyl1FTXIRWeWVn";
            var res = await client.GetAsync($"gettoken?corpid={corpId}&corpsecret={secret}");
            //string content = await res.Content.ReadAsStringAsync();
            var content = await res.Content.ReadAsAsync<Res>();
            return new JsonResult(content);
            //return Content(content);
        }

        public async Task<IActionResult> Test5()
        {
            return new JsonResult(await _qywxApiService.GetAccessToken());
        }
        [HttpGet]
        public IActionResult Test6()
        {
            _Cache.SetString("testValue", "123");
            return Content("ok");
        }
        public IActionResult Test7()
        {
            string a = _Cache.GetString("testValue");
            return Content(a);
        }
        public IActionResult Test8()
        {
            return new JsonResult(_qqMapApiService.LocationToAddress(23.365250M, 116.705400M).Result);
        }
        public IActionResult Test9()
        {
            return Content(CommonHelper.GetDistance(23.365250, 116.705400, 23.413210, 116.635040).ToString());
        }

        public IActionResult Test10()
        {
            _Cache.Remove("Config:*");
            return Content("OK");
        }
        public virtual IActionResult Test11()
        {
            //_Cache.SetString("Config:LOGIN", "", new DistributedCacheEntryOptions()
            //{
            //    //SlidingExpiration = TimeSpan.FromSeconds(5),
            //    //AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(5),
            //    //AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(10)
            //});
            //_cacheHelper._cache.SetAdd("aa", "bbaaa");
            //var isExists = _cacheHelper.
            return Content("");
        }
        //[CacheDeleteInterceptor(Types = new[] { typeof(TestSer2) }, Methods = new[] { nameof(TestSer2.Test13) })]
        public virtual IActionResult Test12()
        {
            //nameof
            //string name = a.Method.DeclaringType.FullName + "." + a.Method.Name;
            //var id = _userService.Save(new Dto.Sys.UserSaveDto
            //{
            //    LoginName = "用户名",
            //    RoleId = -1
            //});
            ;
            //return Content(_test2.Test13(2).ToString());
            //var a = _Cache.Get("Config:LOGIN");
            //return Content("ok");
            _test2.Test14();
            return Content(_test2.Test13(1).Result.ToString());
        }

        public IActionResult Test13()
        {
            var context = this.HttpContext;
            var token = context.GetTokenAsync("Bearer").Result;
            return Content(token);
        }

        public IActionResult Test14()
        {
            var title = _configContext.GetValue(ConfigKey.SYSTEM_TITLE);
            return Content(title);
        }

        public string Test15()
        {
            _logger.LogError(new Exception("haha"), "test");
            return "hehe";
        }
        public string Test16(string a)
        {
            throw new Exception("haha");
            return "hehe";
        }

        //public LoginResultDto Test15(string accessToken, string refreshToken)
        //{
        //    //var res = _authService.RefreshToken(accessToken, refreshToken);
        //    return res;
        //}

        public string TestEsCreateIndex(string indexName)
        {
            var res = _testEsService.CreatIndex(indexName);
            return CommonHelper.ObjectToJsonString(res);
        }



        public string TestEsDeleteIndex(string indexName)
        {
            var res = _testEsService.DeleteIndex(indexName);
            return CommonHelper.ObjectToJsonString(res);
        }

        public string TestEsCreateDoc()
        {
            var res = _testEsService.CreateDoc();
            return CommonHelper.ObjectToJsonString(res);
        }
        public string TestEsCreateDoc2()
        {
            var res = _testEsService.CreateDoc2();
            return CommonHelper.ObjectToJsonString(res);
        }

        public string TestEsSaveDoc()
        {
            var res = _testEsService.SaveDoc();
            return CommonHelper.ObjectToJsonString(res);
        }

        public string TestEsUpdateDoc()
        {
            var res = _testEsService.UpdateDoc();
            return CommonHelper.ObjectToJsonString(res);
        }

        public string DeleteDoc()
        {
            var res = _testEsService.DeleteDoc();
            return CommonHelper.ObjectToJsonString(res);
        }

        public string BulkAll()
        {
            var res = _testEsService.BulkAll();
            return CommonHelper.ObjectToJsonString(res);
        }
        public string Bulk()
        {
            var res = _testEsService.Bulk();
            return CommonHelper.ObjectToJsonString(res);
        }

        public string Test17()
        {
            string a = "a\r\nb\"\t";
            var b = System.Web.HttpUtility.UrlEncode(a);
            var c = System.Web.HttpUtility.UrlDecode(a);
            var d = System.Text.RegularExpressions.Regex.Escape(a);
            return c;
        }

        public string Test18(string input)
        {

            return System.Web.HttpUtility.HtmlDecode(input);
        }
        public string Test19(string v)
        {
            return System.Web.HttpUtility.HtmlDecode(v);
        }

        public string Test20()
        {
            try
            {
                //throw new Exception("try");
            }
            catch (Exception ex)
            {
                throw new Exception("catch");
            }
            finally
            {
                throw new Exception("Finally");
            }
        }
    }
    [CacheInterceptor]
    public interface ITest2
    {
        ValueTask<int> Test13(int a);
        Task Test14();
    }

    public class TestSer2 : ITest2
    {

        public async ValueTask<int> Test13(int a)
        {
            return a + 1;
        }

        public async Task Test14()
        {

        }
    }
    public class Res
    {
        public int Errcode { get; set; }
        public string Errmsg { get; set; }
        public string Access_token { get; set; }
        public int ExpiresIn { get; set; }
    }
}