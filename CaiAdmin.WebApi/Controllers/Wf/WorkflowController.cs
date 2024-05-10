using CaiAdmin.Service;
using CaiAdmin.Service.Crm;
using CaiAdmin.Service.Wf;
using CaiAdmin.WebApi.ApiGroup;
using CaiAdmin.WebApi.AuthorizationPolicies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CaiAdmin.Dto.Wf;
using CaiAdmin.Dto;

namespace CaiAdmin.WebApi.Controllers.Wf
{
    [OpenApiTag("工作流管理")]
    [ApiGroup(ApiGroupNames.Crm)]
    [Authorize(AuthorizationPolicyNames.ApiPermission)]
    public class WorkflowController : AutoRouteAuthorizeControllerBase
    {
        private readonly WorkflowService _workflowService;
        public WorkflowController(WorkflowService customerService)
        {
            _workflowService = customerService;
        }
        /// <summary>
        /// 获取工作流分页列表数据
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResultDto<PageListDto<WorkflowDto>>> GetPageList(WorkflowQueryDto queryDto)
        {
            return await _workflowService.GetPageListAsync(queryDto);
        }

        /// <summary>
        /// 获取工作流详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ApiResultDto<WorkflowDetailDto>> GetDetail(Guid id)
        {
            return await _workflowService.GetDetail(id);
        }

        /// <summary>
        /// 同步工作流到ES
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResultDto> SyncToEs(WorkflowQueryDto queryDto)
        {
            await _workflowService.SyncToEs(queryDto);
            return ApiResultDto.Success();
        }

        /// <summary>
        /// 退回节点
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResultDto> ReturnTask(ReturnTaskDto dto)
        {
            await _workflowService.ReturnTask(dto);
            return ApiResultDto.Success();
        }

    }
}
