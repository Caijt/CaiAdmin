using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaiAdmin.WebApi.ApiGroup
{
    /// <summary>
    /// 系统分组枚举值
    /// </summary>
    public enum ApiGroupNames
    {
        [ApiGroupInfo(Title = "公共模块", Description = "系统公共接口", Version = "1.0")]
        Common,
        //[ApiGroupInfo(Title = "登录认证", Description = "登录认证相关接口", Version = "1.0")]
        //Auth,
        [ApiGroupInfo(Title = "系统管理", Description = "系统后台管理接口", Version = "1.0")]
        Sys,
        [ApiGroupInfo(Title = "客户管理", Description = "客户相关接口", Version = "1.0")]
        Crm,
        [ApiGroupInfo(Title = "财务管理", Description = "财务相关接口", Version = "1.0")]
        Fam,
        [ApiGroupInfo(Title = "业务管理", Description = "业务相关接口", Version = "1.0")]
        Fcm,
        [ApiGroupInfo(Title = "用户管理", Description = "用户相关接口", Version = "1.0")]
        Sso,
        [ApiGroupInfo(Title = "工作流管理", Description = "工作流相关接口", Version = "1.0")]
        Wf,
        [ApiGroupInfo(Title = "平台管理", Description = "平台相关接口", Version = "1.0")]
        Platform,
        [ApiGroupInfo(Title = "IT管理", Description = "IT相关接口", Version = "1.0")]
        It,
		[ApiGroupInfo(Title = "公共管理", Description = "公共相关接口", Version = "1.0")]
        Pub
    }
}
