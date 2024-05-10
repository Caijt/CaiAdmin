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
using CaiAdmin.Dto;
using CaiAdmin.Dto.Sys;

namespace CaiAdmin.WebApi.Controllers.Sys
{
    [OpenApiTag("接口管理")]
    [ApiGroup(ApiGroupNames.Sys)]
    [Authorize(AuthorizationPolicyNames.ApiPermission)]
    public class ApiController : AutoRouteAuthorizeControllerBase
    {
        private readonly CaiAdmin.Service.Sys.ApiService _apiService;
        public ApiController(CaiAdmin.Service.Sys.ApiService apiService)
        {
            _apiService = apiService;
        }
        /// <summary>
        /// 获取接口分页列表
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        public async Task<ApiResultDto<PageListDto<ApiDto>>> GetPageList([FromQuery]ApiQueryDto queryDto)
        {
            return await _apiService.GetPageListAsync(queryDto);
        }

        /// <summary>
        /// 获取接口公共分页列表
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        public async Task<ApiResultDto<PageListDto<ApiDto>>> GetCommonPageList([FromQuery]ApiQueryDto queryDto)
        {
            return await _apiService.GetPageListAsync(queryDto);
        }
        /// <summary>
        /// 保存接口
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResultDto<int>> Save(ApiDto dto)
        {
            return await _apiService.SaveAsync(dto);
        }

        /// <summary>
        /// 根据Id值进行删除接口
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResultDto> DeleteById([FromBody]int id)
        {
            await _apiService.DeleteByIdAsync(id);
            return ApiResultDto.Success();
        }

        /// <summary>
        /// 检查是否存在重复的名称
        /// </summary>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResultDto<bool>> CheckExistByName(string name, int id = 0)
        {
            return await _apiService.CheckExistByNameAsync(name, id);
        }

        /// <summary>
        /// 检查是否存在重复的路径值
        /// </summary>
        /// <param name="path"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResultDto<bool>> CheckExistByPath(string path, int id = 0)
        {
            return await _apiService.CheckExistByPathAsync(path, id);
        }
    }
}