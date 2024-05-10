using CaiAdmin.Dto;
using CaiAdmin.Dto.Crm;
using CaiAdmin.Service;
using CaiAdmin.Service.Crm;
using CaiAdmin.WebApi.ApiGroup;
using CaiAdmin.WebApi.AuthorizationPolicies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaiAdmin.WebApi.Controllers.Crm
{
    [OpenApiTag("公海池客户统计")]
    [ApiGroup(ApiGroupNames.Crm)]
    [Authorize(AuthorizationPolicyNames.ApiPermission)]
    public class SeaPoolStatisticsController : AutoRouteAuthorizeControllerBase
    {
        private readonly SeaPoolStatisticsService _seaPoolStatisticsService;
        public SeaPoolStatisticsController(SeaPoolStatisticsService seaPoolStatisticsService)
        {
            _seaPoolStatisticsService = seaPoolStatisticsService;
        }
        /// <summary>
        /// 获取客户分页列表数据
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResultDto<PageListSummaryDto<SeaPoolStatisticsDto>>> GetPageList(SeaPoolStatisticsQueryDto queryDto)
        {
            return await _seaPoolStatisticsService.GetPageListAsync(queryDto);
        }

        [HttpPost]
        public async Task<ApiResultDto<PageListDto<A>>> GetAllocationUsers(BQuery queryDto)
        {
            return await _seaPoolStatisticsService.GetAllocationUsers(queryDto);
        }

        [HttpPost]
        public async Task<ApiResultDto<PageListDto<C>>> GetAllocationRecords(CQuery queryDto)
        {
            return await _seaPoolStatisticsService.GetAllocationRecords(queryDto);
        }
    }
}
