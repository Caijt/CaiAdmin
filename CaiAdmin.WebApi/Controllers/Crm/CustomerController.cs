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
    [OpenApiTag("客户管理")]
    [ApiGroup(ApiGroupNames.Crm)]
    [Authorize(AuthorizationPolicyNames.ApiPermission)]
    public class CustomerController : AutoRouteAuthorizeControllerBase
    {
        private readonly CustomerService _customerService;
        public CustomerController(CustomerService customerService)
        {
            _customerService = customerService;
        }
        /// <summary>
        /// 获取客户分页列表数据
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResultDto<PageListDto<CustomerDto>>> GetPageList(CustomerQueryDto queryDto)
        {
            return await _customerService.GetPageListAsync(queryDto);
        }

        /// <summary>
        /// 获取客户详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ApiResultDto<CustomerDetailDto>> GetDetail(Guid id)
        {
            return await _customerService.GetDetail(id);
        }

        /// <summary>
        /// 获取旧ICP客户详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ApiResultDto<CustomerOldIcpDetailDto>> GetOldIcpDetail(Guid id)
        {
            return await _customerService.GetOldIcpDetail(id);
        }
        /// <summary>
        /// 同步客户到ES
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResultDto> SyncCustomersToEs(CustomerQueryDto queryDto)
        {
            await _customerService.SyncCustomersToEs(queryDto);
            return ApiResultDto.Success();
        }

        /// <summary>
        /// 同步客户审批到ES
        /// </summary>
        /// <param name="customerExamineId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResultDto> SyncCustomerExamineToEs(Guid customerExamineId)
        {
            await _customerService.SyncCustomerExamineToEs(customerExamineId);
            return ApiResultDto.Success();
        }

        /// <summary>
        /// 同步客户到ICP
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResultDto> SyncCustomersToIcp(CustomerQueryDto queryDto)
        {
            await _customerService.SyncCustomersToIcp(queryDto);
            return ApiResultDto.Success();
        }
        /// <summary>
        /// 解除合并
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ApiResultDto> RelieveMerge(Guid id)
        {
            await _customerService.RelieveMergeAsync(id);
            return ApiResultDto.Success();
        }

        /// <summary>
        /// 重置客户名称唯一值
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResultDto> ResetNameUniqueValue(CustomerQueryDto queryDto)
        {
            await _customerService.ResetNameUniqueValue(queryDto);
            return ApiResultDto.Success();
        }
        /// <summary>
        /// 获取审批状态异常的客户列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResultDto<PageListDto<ExamineStatusExceptionCustomerDto>>> GetExamineStatusExceptionList(CustomerQueryDto queryDto)
        {
            return await _customerService.GetExamineStatusExceptionListAsync(queryDto);
        }

        [HttpPost]
        public async Task<ApiResultDto> RepairExamineStatus(CustomerQueryDto queryDto)
        {
            await _customerService.RepairExamineStatus(queryDto);
            return ApiResultDto.Success();
        }

        [HttpPost]
        public async Task<ApiResultDto<PageListDto<NameUniqueValueRepeatDto>>> GetNameUniqueValueRepeatList(CustomerRepeatQueryDto queryDto)
        {
            return await _customerService.GetNameUniqueValueRepeatList(queryDto);
        }

        [HttpPost]
        public async Task<ApiResultDto<PageListDto<NameLocalizationUniqueValueRepeatDto>>> GetNameLocalizationUniqueValueRepeatList(CustomerRepeatQueryDto queryDto)
        {
            return await _customerService.GetNameLocalizationUniqueValueRepeatList(queryDto);
        }

        /// <summary>
        /// 反转义客户名称
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResultDto> DecodeCustomerLocalizationText(CustomerQueryDto queryDto)
        {
            await _customerService.DecodeCustomerLocalizationText(queryDto);
            return ApiResultDto.Success();
        }

        /// <summary>
        /// 合并客户
        /// </summary>
        /// <param name="nameUniqueValues"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResultDto<List<MergeCustomerResultDto>>> MergeCustomers(List<string> nameUniqueValues)
        {
            return await _customerService.BatchMergeCustomersAsync(nameUniqueValues);
        }

        /// <summary>
        /// 合并客户并导出Excel
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> MergeCustomersExportExcel(MergeCustomerDto dto)
        {
            return File(
                await _customerService.MergeCustomersExportExcelAsync(dto.NameUniqueValues, dto.IsLocalization),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "合并客户结果" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx");
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<ApiResultDto<int>> MoveAddressEmail()
        {
            return await _customerService.MoveAddressEmail();
        }

    }
}
