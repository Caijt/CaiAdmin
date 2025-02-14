﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CaiAdmin.Dto;
using CaiAdmin.Service;
using CaiAdmin.WebApi.ApiGroup;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace CaiAdmin.WebApi.Controllers
{
    [OpenApiTag("企业微信认证授权")]
    [ApiGroup(ApiGroupNames.Common)]
    public class QywxAuthController : AutoRouteAuthorizeControllerBase
    {
        private readonly QywxAuthService _qywxAuthService;
        public QywxAuthController(QywxAuthService qywxAuthService)
        {
            _qywxAuthService = qywxAuthService;
        }
        /// <summary>
        /// 根据微信code获取身份Token
        /// </summary>
        /// <param name="loginDto"></param>
        /// <returns></returns>
        //[HttpGet]
        //[AllowAnonymous]
        //public async Task<ApiResultDto<LoginResultDto>> GetTokenByCode(string code)
        //{
        //    return await _qywxAuthService.GetTokenByCode(code);
        //}

        /// <summary>
        /// 获取授权链接
        /// </summary>
        /// <param name="redirectUrl"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public ApiResultDto<string> GetQywxAuthUrl(string redirectUrl, string state = null)
        {
            return _qywxAuthService.GetAuthUrl(redirectUrl, state);
        }

        [HttpGet]
        [AllowAnonymous]
        public ApiResultDto<WeixinJssdkConfigDto> GetJssdkConfig(string url)
        {
            return _qywxAuthService.GetJssdkConfig(url);
        }
    }
}