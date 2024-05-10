using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CaiAdmin.Dto;
using CaiAdmin.Dto.Sys;
using CaiAdmin.Service;
using CaiAdmin.Service.Sys;
using CaiAdmin.WebApi.ApiGroup;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using static CaiAdmin.Service.Sys.RoleService;

namespace CaiAdmin.WebApi.Controllers.Sys
{
    [ApiGroup(ApiGroupNames.Sys)]
    [OpenApiTag("角色管理")]
    [Authorize(AuthorizationPolicies.AuthorizationPolicyNames.ApiPermission)]
    public class RoleController : AutoRouteAuthorizeControllerBase
    {
        private readonly RoleService _roleService;
        public RoleController(RoleService roleService)
        {
            _roleService = roleService;
        }
        /// <summary>
        /// 获取角色分页列表
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResultDto<PageListDto<RoleDto>>> GetPageList([FromQuery] RoleQueryDto queryDto)
        {
            return await _roleService.GetPageListAsync(queryDto);
        }

        [HttpGet]
        public async Task<ApiResultDto<List<OptionDto>>> GetCommonOptionList([FromQuery] RoleQueryDto queryDto)
        {
            return await _roleService.GetOptionListAsync(queryDto);
        }

        /// <summary>
        /// 根据Id获取修改实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResultDto<RoleSaveDto>> GetEditById(int id)
        {
            return await _roleService.GetEditById(id);
        }

        /// <summary>
        /// 保存角色信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResultDto<int>> Save(RoleSaveDto dto)
        {
            return await _roleService.SaveAsync(dto);
        }

        /// <summary>
        /// 根据Id值进行删除角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResultDto> DeleteById([FromBody] int id)
        {
            await _roleService.DeleteByIdAsync(id);
            return ApiResultDto.Success();
        }


        [HttpGet]
        public async Task<ApiResultDto<bool>> CheckExistByName(string name, int id = 0)
        {
            return await _roleService.CheckExistByNameAsync(name, id);
        }

        /// <summary>
        /// 根据角色Id获取角色全部菜单及权限，包含上级菜单
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public async Task<ApiResultDto<List<MenuIncludePermissionDto>>> GetRoleMenuPermissionList(int roleId)
        {
            return await _roleService.GetRoleMenuPermissionListAsync(roleId);
        }
    }
}