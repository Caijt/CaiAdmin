using CaiAdmin.ApiService.CityOcean;
using CaiAdmin.Dto;
using CaiAdmin.Dto.Sso;
using CaiAdmin.Service;
using CaiAdmin.Service.Fam;
using CaiAdmin.Service.Sso;
using CaiAdmin.WebApi.ApiGroup;
using CaiAdmin.WebApi.AuthorizationPolicies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaiAdmin.WebApi.Controllers.Sso
{
    [OpenApiTag("用户管理")]
    [ApiGroup(ApiGroupNames.Sso)]
    [Authorize(AuthorizationPolicyNames.ApiPermission)]
    public class UserController : AutoRouteAuthorizeControllerBase
    {
        private readonly UserService _userService;
        public UserController(UserService billService)
        {
            _userService = billService;
        }
        /// <summary>
        /// 获取用户分页数据
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResultDto<PageListDto<UserDto>>> GetPageList(UserQueryDto queryDto)
        {
            return await _userService.GetPageListAsync(queryDto);
        }

        /// <summary>
        /// 获取用户详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResultDto<UserDetailDto>> GetDetail(long Id)
        {
            return await _userService.GetDetailAsync(Id);
        }
    }
}
