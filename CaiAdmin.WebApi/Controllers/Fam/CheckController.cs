using CaiAdmin.ApiService.CityOcean;
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
    [OpenApiTag("核销单管理")]
    [ApiGroup(ApiGroupNames.Fam)]
    [Authorize(AuthorizationPolicyNames.ApiPermission)]
    public class CheckController : AutoRouteAuthorizeControllerBase
    {
        private readonly CheckService _checkService;
        public CheckController(CheckService checkService)
        {
            _checkService = checkService;
        }
        /// <summary>
        /// 获取账单分页数据
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResultDto<PageListDto<CheckDto>>> GetPageList(CheckQueryDto queryDto)
        {
            return await _checkService.GetPageListAsync(queryDto);
        }

        /// <summary>
        /// 获取账单详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResultDto<CheckDetailDto>> GetDetailAsync(Guid Id)
        {
            return await _checkService.GetDetailAsync(Id);
        }

        /// <summary>
        /// 同步核销单到ES
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResultDto> SyncCheckAmountToEs(CheckQueryDto queryDto)
        {
            await _checkService.SyncCheckAmountToEs(queryDto);
            return ApiResultDto.Success();
        }

        /// <summary>
        /// 获取CDC数据
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResultDto<PageListDto<BillCdcDto>>> GetCdcPageList(BillQueryDto queryDto)
        {
            return await _checkService.GetCdcPageListAsync(queryDto);
        }

        /// <summary>
        /// 获取费用CDC数据
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResultDto<PageListDto<BillChargeCdcDto>>> GetChargeCdcPageList(BillChargeQueryDto queryDto)
        {
            return await _checkService.GetChargeCdcPageListAsync(queryDto);
        }
    }
}
