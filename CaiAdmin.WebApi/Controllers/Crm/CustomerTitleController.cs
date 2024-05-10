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
    [OpenApiTag("客户抬头管理")]
    [ApiGroup(ApiGroupNames.Crm)]
    [Authorize(AuthorizationPolicyNames.ApiPermission)]
    public class CustomerTitleController : AutoRouteAuthorizeControllerBase
    {
        private readonly CustomerTitleService _customerTitleService;
        public CustomerTitleController(CustomerTitleService customerTitleService)
        {
            _customerTitleService = customerTitleService;
        }
        /// <summary>
        /// 获取旧ICP客户抬头分页列表数据
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResultDto<PageListDto<CustomerTitleDto>>> GetPageList(CustomerTitleQueryDto queryDto)
        {
            return await _customerTitleService.GetPageListAsync(queryDto);
        }
        /// <summary>
        /// 获取旧ICP客户抬头分页列表数据
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResultDto<PageListDto<OldIcpCustomerTitleDto>>> GetOldIcpPageList(OldIcpCustomerTitleQueryDto queryDto)
        {
            return await _customerTitleService.GetOldIcpPageListAsync(queryDto);
        }

        [HttpPost]
        public async Task<ApiResultDto> ImportOldIcpCustomerTitleToNewIcp(OldIcpCustomerTitleQueryDto queryDto)
        {
            await _customerTitleService.ImportOldIcpCustomerTitleToNewIcp(queryDto);
            return ApiResultDto.Success();
        }


    }
}
