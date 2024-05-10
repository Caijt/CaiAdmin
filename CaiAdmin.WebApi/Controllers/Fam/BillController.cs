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
    [OpenApiTag("账单管理")]
    [ApiGroup(ApiGroupNames.Fam)]
    [Authorize(AuthorizationPolicyNames.ApiPermission)]
    public class BillController : AutoRouteAuthorizeControllerBase
    {
        private readonly BillService _billService;
        public BillController(BillService billService)
        {
            _billService = billService;
        }
        /// <summary>
        /// 获取账单分页数据
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResultDto<PageListDto<BillDto>>> GetPageList(BillQueryDto queryDto)
        {
            return await _billService.GetPageListAsync(queryDto);
        }

        /// <summary>
        /// 获取账单详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResultDto<BillDetailDto>> GetDetailAsync(Guid Id)
        {
            return await _billService.GetDetailAsync(Id);
        }

        /// <summary>
        /// 同步币种账单到ES
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResultDto> SyncCurrencyBillToEs(BillQueryDto queryDto)
        {
            await _billService.SyncCurrencyBillToEs(queryDto);
            return ApiResultDto.Success();
        }

        /// <summary>
        /// 同步费用账单到ES
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResultDto> SyncChargeBillToEs(BillQueryDto queryDto)
        {
            await _billService.SyncChargeBillToEs(queryDto);
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
            return await _billService.GetCdcPageListAsync(queryDto);
        }

        /// <summary>
        /// 获取费用CDC数据
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResultDto<PageListDto<BillChargeCdcDto>>> GetChargeCdcPageList(BillChargeQueryDto queryDto)
        {
            return await _billService.GetChargeCdcPageListAsync(queryDto);
        }
    }
}
