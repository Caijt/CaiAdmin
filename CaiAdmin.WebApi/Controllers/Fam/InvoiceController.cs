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
    [OpenApiTag("发票管理")]
    [ApiGroup(ApiGroupNames.Fam)]
    [Authorize(AuthorizationPolicyNames.ApiPermission)]
    public class InvoiceController : AutoRouteAuthorizeControllerBase
    {
        private readonly InvoiceService _invoiceService;
        public InvoiceController(InvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }
        /// <summary>
        /// 获取发票分页数据
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResultDto<PageListDto<InvoiceDto>>> GetPageList(InvoiceQueryDto queryDto)
        {
            return await _invoiceService.GetPageListAsync(queryDto);
        }

        /// <summary>
        /// 获取发票详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResultDto<InvoiceDetailDto>> GetDetailAsync(Guid Id)
        {
            return await _invoiceService.GetDetailAsync(Id);
        }


        /// <summary>
        /// 从诺诺同步状态
        /// </summary>
        /// <param name="invoiceIds"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResultDto> UpdateInvoiceFromNuoNuo(List<Guid> invoiceIds)
        {
            await _invoiceService.UpdateInvoiceFromNuoNuoAsync(invoiceIds);
            return ApiResultDto.Success();
        }

        /// <summary>
        /// 请求诺诺开票
        /// </summary>
        /// <param name="invoiceIds"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResultDto> RequestInvoiceToNuonuoByJob(List<Guid> invoiceIds)
        {
            await _invoiceService.RequestInvoiceToNuonuoByJob(invoiceIds);
            return ApiResultDto.Success();
        }
    }
}
