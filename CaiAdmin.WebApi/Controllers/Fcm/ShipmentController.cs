using CaiAdmin.ApiService.CityOcean;
using CaiAdmin.Service;
using CaiAdmin.Service.Fam;
using CaiAdmin.Service.Fcm;
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
using CaiAdmin.Dto;
using CaiAdmin.Dto.Fcm;

namespace CaiAdmin.WebApi.Controllers.Fcm
{
    [OpenApiTag("运单管理")]
    [ApiGroup(ApiGroupNames.Fcm)]
    [Authorize(AuthorizationPolicyNames.ApiPermission)]
    public class ShipmentController : AutoRouteAuthorizeControllerBase
    {
        private readonly ShipmentService _billService;
        public ShipmentController(ShipmentService billService)
        {
            _billService = billService;
        }
        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResultDto<PageListDto<ShipmentDto>>> GetPageList(ShipmentQueryDto queryDto)
        {
            return await _billService.GetPageListAsync(queryDto);
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

        /// <summary>
        /// 获取CDC数据
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        //[HttpPost]
        //public async Task<ApiResultDto<PageListDto<BillCdcDto>>> GetCdcPageList(BillQueryDto queryDto)
        //{
        //    return await _billService.GetCdcPageListAsync(queryDto);
        //}

        /// <summary>
        /// 获取费用CDC数据
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        //[HttpPost]
        //public async Task<ApiResultDto<PageListDto<BillChargeCdcDto>>> GetChargeCdcPageList(BillChargeQueryDto queryDto)
        //{
        //    return await _billService.GetChargeCdcPageListAsync(queryDto);
        //}
    }
}
