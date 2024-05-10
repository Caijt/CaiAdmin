using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CaiAdmin.Service;
using CaiAdmin.Service.Sys;
using CaiAdmin.WebApi.ApiGroup;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using CaiAdmin.WebApi.AuthorizationPolicies;
using CaiAdmin.Dto.Sys;
using CaiAdmin.Dto;

namespace CaiAdmin.WebApi.Controllers.Sys
{
    [OpenApiTag("用户管理")]
    [ApiGroup(ApiGroupNames.Sys)]
    [Authorize(AuthorizationPolicies.AuthorizationPolicyNames.ApiPermission)]
    public class UserController : AutoRouteAuthorizeControllerBase
    {
        private readonly UserService _userService;
        public UserController(UserService sysUserService)
        {
            _userService = sysUserService;
        }
        /// <summary>
        /// 获取用户分页列表
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        public async Task<ApiResultDto<PageListDto<UserDto>>> GetPageList([FromQuery]UserQueryDto queryDto)
        {
            return await _userService.GetPageListAsync(queryDto);
        }

        /// <summary>
        /// 保存用户
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResultDto<int>> Save(UserSaveDto dto)
        {
            return await _userService.SaveAsync(dto);
        }
        /// <summary>
        /// 根据Id值进行删除用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResultDto> DeleteById([FromBody]int id)
        {
            await _userService.DeleteByIdAsync(id);
            return ApiResultDto.Success();
        }

        /// <summary>
        /// 检查是否存在重复的登录名
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResultDto<bool>> CheckExistByLoginName(string loginName, int id = 0)
        {
            return await _userService.CheckExistByLoginNameAsync(loginName, id);
        }
    }
}