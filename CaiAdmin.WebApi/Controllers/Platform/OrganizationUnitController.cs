using CaiAdmin.ApiService.CityOcean;
using CaiAdmin.Dto;
using CaiAdmin.Dto.Fam;
using CaiAdmin.Dto.Platform;
using CaiAdmin.Service;
using CaiAdmin.Service.Platform;
using CaiAdmin.WebApi.ApiGroup;
using CaiAdmin.WebApi.AuthorizationPolicies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.PlatformAbstractions;
using NSwag.Annotations;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaiAdmin.WebApi.Controllers.Platform
{
    [OpenApiTag("平台管理")]
    [ApiGroup(ApiGroupNames.Fam)]
    [Authorize(AuthorizationPolicyNames.ApiPermission)]
    public class OrganizationUnitController : AutoRouteAuthorizeControllerBase
    {
        private readonly OrganizationUnitService _billService;
        public OrganizationUnitController(OrganizationUnitService billService)
        {
            _billService = billService;
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResultDto<List<OrganizationUnitDto>>> GetList(OrganizationUnitQueryDto queryDto)
        {
            return await _billService.GetListAsync(queryDto);
        }

        /// <summary>
        /// 获取账单详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        //[HttpGet]
        //public async Task<ApiResultDto<BillDetailDto>> GetDetailAsync(Guid Id)
        //{
        //    return await _billService.GetDetailAsync(Id);
        //}
    }
}
