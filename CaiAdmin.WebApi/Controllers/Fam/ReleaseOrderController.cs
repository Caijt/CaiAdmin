using CaiAdmin.ApiService.CityOcean;
using CaiAdmin.ApiService.CityOcean.Dto;
using CaiAdmin.Dto;
using CaiAdmin.Dto.Fam;
using CaiAdmin.Service;
using CaiAdmin.Service.Fam;
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

namespace CaiAdmin.WebApi.Controllers.Fam
{
    [OpenApiTag("放单管理")]
    [ApiGroup(ApiGroupNames.Fam)]
    [Authorize(AuthorizationPolicyNames.ApiPermission)]
    public class ReleaseOrderController : AutoRouteAuthorizeControllerBase
    {
        private readonly ReleaseOrderService _releaseOrderService;
        public ReleaseOrderController(ReleaseOrderService releaseOrderService)
        {
            _releaseOrderService = releaseOrderService;
        }



        /// <summary>
        /// 同步放单到ES
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResultDto> SyncReleaseOrderToEs(ReleaseOrderQueryDto queryDto)
        {
            await _releaseOrderService.SyncReleaseOrderToEs(queryDto);
            return ApiResultDto.Success();
        }

        /// <summary>
        /// 获取放单分页列表
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResultDto<PageListDto<ReleaseOrderDto>>> GetPageListAsync(ReleaseOrderQueryDto queryDto)
        {
            return await _releaseOrderService.GetPageListAsync(queryDto);
        }

        /// <summary>
        /// 发送放单邮件
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResultDto<List<ReleaseBillOfLadingEmailTemplateResult>>> SendReleaseOrderEmails(List<ReleaseBillOfLadingWithEmailTemplateDto> inputDto)
        {
            return await _releaseOrderService.SendReleaseOrderEmails(inputDto);
        }

    }
}
