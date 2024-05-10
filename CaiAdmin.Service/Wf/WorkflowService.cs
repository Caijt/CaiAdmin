using CaiAdmin.ApiService.CityOcean;
using CaiAdmin.Common;
using CaiAdmin.Dto;
using CaiAdmin.Dto.Wf;
using CaiAdmin.Database;
using CaiAdmin.Database.SqlSugar;
using CaiAdmin.Service;
using CaiAdmin.Service.Crm;
using CaiAdmin.Service.Wf;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace CaiAdmin.Service.Wf
{
    public class WorkflowService
    {
        private readonly CityOceanRepository _cityOceanRepository;
        private readonly CityOceanApiService _cityOceanApiService;
        public WorkflowService(CityOceanApiService coIcpApiService, CityOceanRepository cityOceanRepository)
        {
            _cityOceanRepository = cityOceanRepository;
            _cityOceanApiService = coIcpApiService;
        }

        /// <summary>
        /// 获取工作流分页数据
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>        
        public async Task<PageListDto<WorkflowDto>> GetPageListAsync(WorkflowQueryDto queryDto)
        {
            if (!string.IsNullOrWhiteSpace(queryDto.IdString))
            {
                var ids = queryDto.IdString.Trim().Split(',', ' ');
                var guidIds = new List<Guid>();
                foreach (var item in ids)
                {
                    if (Guid.TryParse(item, out Guid id))
                    {
                        guidIds.Add(id);
                    }
                }
                if (queryDto.Ids != null && queryDto.Ids.Any())
                {
                    guidIds.AddRange(queryDto.Ids);
                }
                queryDto.Ids = guidIds.ToArray();
            }
            var result = new PageListDto<WorkflowDto>();
            RefAsync<int> total = 0;
            var data = await _cityOceanRepository.Context
                .SqlQueryable<WorkflowDto>(@"Select 
                    w.Id,
                    w.No,
                    w.Name,
                    w.IsDeleted,
                    w.Status,
                    w.ApplicationUserId,
                    w.ApplicationTime,
                    (SELECT TOP 1 u.CName FROM co_sso.dbo.Users u WHERE u.Id=w.ApplicationUserId) ApplicationUserName,
                    wd.Name as WorkflowCode,
                    wd.DisplayName as WorkflowName,
                    w.CreationTime,
                    w.LastModificationTime
                 From co_wf.dbo.Workflows w 
                    INNER JOIN co_wf.dbo.WorkflowDefinitionVersions wdv ON w.WorkflowDefinitionVersionId = wdv.Id 
                    INNER JOIN co_wf.dbo.WorkflowDefinitions wd ON wd.Id = wdv.WorkflowDefinitionId ")
                 .WhereIF(!string.IsNullOrWhiteSpace(queryDto.Name), i => i.Name.Contains(queryDto.Name))
                 .WhereIF(!string.IsNullOrWhiteSpace(queryDto.No), i => i.No.Contains(queryDto.No))
                 .WhereIF(queryDto.Ids != null && queryDto.Ids.Any(), i => queryDto.Ids.Contains(i.Id))
                 .WhereIF(queryDto.Statuses != null && queryDto.Statuses.Any(), i => queryDto.Statuses.Contains(i.Status))
                 .WhereIF(queryDto.WorkflowCodes != null && queryDto.WorkflowCodes.Any(), i => queryDto.WorkflowCodes.Contains(i.WorkflowCode))
                 .OrderByIF(string.IsNullOrWhiteSpace(queryDto.OrderField), o => o.CreationTime, OrderByType.Desc)
                 .OrderByIF(!string.IsNullOrWhiteSpace(queryDto.OrderField), $"{queryDto.OrderField} {(queryDto.OrderDesc == true ? "desc" : "asc")}")
                 .OrderBy(o => o.Id)
                 .ToPageListAsync(queryDto.PageIndex, queryDto.PageSize, total);
            result.List = data;
            result.Total = total.Value;
            return result;
        }
        /// <summary>
        /// 获取客户详情
        /// </summary>
        /// <returns></returns>
        public async Task<WorkflowDetailDto> GetDetail(Guid id)
        {
            var workflow = await _cityOceanRepository.Context.SqlQueryable<WorkflowDetailDto>(@"Select 
                w.Id,
                w.No,
                w.Name,
                w.IsDeleted,
                w.Status,
                w.ApplicationUserId,
                w.ApplicationTime,
                (SELECT TOP 1 u.CName FROM co_sso.dbo.Users u WHERE u.Id=w.ApplicationUserId) ApplicationUserName,
                wd.Name as WorkflowCode,
                wd.DisplayName as WorkflowName,
                w.CreationTime,
                w.LastModificationTime
                From co_wf.dbo.Workflows w 
                    INNER JOIN co_wf.dbo.WorkflowDefinitionVersions wdv ON w.WorkflowDefinitionVersionId = wdv.Id 
                    INNER JOIN co_wf.dbo.WorkflowDefinitions wd ON wd.Id = wdv.WorkflowDefinitionId ")
                .FirstAsync(o => o.Id == id);
            if (workflow == null)
            {
                throw new ApiFailException(ApiFailCode.OPERATION_FAIL, "工作流不存在");
            }
            workflow.Tasks = await _cityOceanRepository.Context.SqlQueryable<WorkflowTaskDto>(@"Select 
                t.Id,
                t.Name,
                t.WorkflowId,
                t.Status,
                t.CreationTime,
                t.FinishedTime,
                t.ExtraProperties
                From CO_WF.dbo.Tasks t")
                .Where(i => i.WorkflowId == id)
                .OrderBy(i => i.CreationTime)
                .ToListAsync();
            var taskIds = workflow.Tasks.Select(i => i.Id).ToList();
            var processes = await _cityOceanRepository.Context.SqlQueryable<WorkflowTaskProcessDto>(@"Select 
                tp.Id,
                tp.ProcessUserId,
                (Select top 1 CName From CO_SSO.dbo.Users u Where u.Id = tp.ProcessUserId) ProcessUserName,
                tp.TaskId,
                tp.Status,
                tp.FormData,
                tp.ExtensionData,
                tp.StartedTime,
                tp.FinishedTime
                From CO_WF.dbo.TaskProcesses tp")
                .Where(i => taskIds.Contains(i.TaskId))
                .OrderBy(i => i.StartedTime)
                .ToListAsync();
            var Candidates = await _cityOceanRepository.Context.SqlQueryable<WorkflowTaskCandidateDto>(@"Select 
                tc.Id,
                tc.CandidateUserId,
                tc.TaskId,
                (Select top 1 CName From CO_SSO.dbo.Users u Where u.Id = tc.CandidateUserId) CandidateUserName
                From CO_WF.dbo.TaskCandidates tc")
                .Where(i => taskIds.Contains(i.TaskId))
                .ToListAsync();
            foreach (var task in workflow.Tasks)
            {
                task.Processes = processes.Where(i => i.TaskId == task.Id).ToList();
                task.Candidates = Candidates.Where(i => i.TaskId == task.Id).ToList();
            }
            return workflow;
        }

        /// <summary>
        /// 同步工作流到ES
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        public async Task SyncToEs(WorkflowQueryDto queryDto)
        {
            queryDto.PageIndex = 1;
            queryDto.PageSize = 1000;
            var total = 0;
            do
            {
                var result = await this.GetPageListAsync(queryDto);
                total = result.Total;
                var nos = result.List.Select(i => i.No).ToList();
                await _cityOceanApiService.SyncWorkflowToEs(nos);
                queryDto.PageIndex++;
            } while ((queryDto.PageIndex - 1) * queryDto.PageSize < total);
        }

        /// <summary>
        /// 退回节点
        /// </summary>
        /// <returns></returns>
        public async Task ReturnTask(ReturnTaskDto dto)
        {
            var workflowTask = await _cityOceanRepository.Context.Ado.SqlQuerySingleAsync<dynamic>(@"
                Select WorkflowId,CreationTime,ElsaActivityInstanceId From CO_WF.dbo.Tasks Where Id=@Id", new
            {
                Id = dto.ReturnTaskId
            });
            if (workflowTask == null)
            {
                throw new ApiFailException(ApiFailCode.OPERATION_FAIL, "工作流不存在");
            }
            _cityOceanRepository.Context.Ado.BeginTran();
            try
            {
                //更新节点的状态
                await _cityOceanRepository.Context.Ado.ExecuteCommandAsync("Update CO_WF.dbo.Tasks Set Status=0 Where Id=@Id", new
                {
                    Id = dto.ReturnTaskId
                });
                //更新节点处理人的状态
                await _cityOceanRepository.Context.Ado.ExecuteCommandAsync("Update CO_WF.dbo.TaskProcesses Set Status=0 Where TaskId=@TaskId", new
                {
                    TaskId = dto.ReturnTaskId
                });
                //更新节点的状态
                await _cityOceanRepository.Context.Ado.ExecuteCommandAsync("Update CO_WF.dbo.Workflows Set Status=2 Where Id=@Id", new
                {
                    Id = workflowTask.WorkflowId
                });
                #region 备份后面节点相关数据
                //备份后面节点候选人
                await _cityOceanRepository.Context.Ado.ExecuteCommandAsync(@"
                INSERT INTO CO_WF.dbo.TaskCandidates_bak_Auto  Select tc.* From CO_WF.dbo.Tasks t INNER JOIN CO_WF.dbo.TaskCandidates tc ON t.Id=tc.TaskId 
                Where t.WorkflowId=@WorkflowId And t.CreationTime > @CreationTime And t.Id <> @TaskId", new
                {
                    WorkflowId = workflowTask.WorkflowId,
                    TaskId = dto.ReturnTaskId,
                    CreationTime = workflowTask.CreationTime
                });
                //备份后面节点处理人
                await _cityOceanRepository.Context.Ado.ExecuteCommandAsync(@"
                INSERT INTO CO_WF.dbo.TaskProcesses_bak_Auto Select tp.* From CO_WF.dbo.Tasks t INNER JOIN CO_WF.dbo.TaskProcesses tp ON t.Id=tp.TaskId 
                Where t.WorkflowId=@WorkflowId And t.CreationTime > @CreationTime And t.Id <> @TaskId", new
                {
                    WorkflowId = workflowTask.WorkflowId,
                    TaskId = dto.ReturnTaskId,
                    CreationTime = workflowTask.CreationTime
                });
                //备份后面节点标签
                await _cityOceanRepository.Context.Ado.ExecuteCommandAsync(@"
                INSERT INTO CO_WF.dbo.TaskTags_bak_Auto Select tt.* From CO_WF.dbo.Tasks t INNER JOIN CO_WF.dbo.TaskTags tt ON t.Id=tt.TaskId 
                Where t.WorkflowId=@WorkflowId And t.CreationTime > @CreationTime And t.Id <> @TaskId", new
                {
                    WorkflowId = workflowTask.WorkflowId,
                    TaskId = dto.ReturnTaskId,
                    CreationTime = workflowTask.CreationTime
                });
                //备份后面节点催办人
                await _cityOceanRepository.Context.Ado.ExecuteCommandAsync(@"
                INSERT INTO CO_WF.dbo.TaskReminders_bak_Auto Select tr.* From CO_WF.dbo.Tasks t INNER JOIN CO_WF.dbo.TaskReminders tr ON t.Id=tr.TaskId 
                Where t.WorkflowId=@WorkflowId And t.CreationTime > @CreationTime And t.Id <> @TaskId", new
                {
                    WorkflowId = workflowTask.WorkflowId,
                    TaskId = dto.ReturnTaskId,
                    CreationTime = workflowTask.CreationTime
                });
                //备份后面节点附件
                await _cityOceanRepository.Context.Ado.ExecuteCommandAsync(@"
                INSERT INTO CO_WF.dbo.TaskAttachments_bak_Auto Select ta.* From CO_WF.dbo.Tasks t INNER JOIN CO_WF.dbo.TaskAttachments ta ON t.Id=ta.TaskId 
                Where t.WorkflowId=@WorkflowId And t.CreationTime > @CreationTime And t.Id <> @TaskId", new
                {
                    WorkflowId = workflowTask.WorkflowId,
                    TaskId = dto.ReturnTaskId,
                    CreationTime = workflowTask.CreationTime
                });
                //备份后面节点
                await _cityOceanRepository.Context.Ado.ExecuteCommandAsync(@"
                INSERT INTO CO_WF.dbo.Tasks_bak_Auto Select t.* From CO_WF.dbo.Tasks t Where t.WorkflowId=@WorkflowId And t.CreationTime > @CreationTime And t.Id <> @TaskId", new
                {
                    WorkflowId = workflowTask.WorkflowId,
                    TaskId = dto.ReturnTaskId,
                    CreationTime = workflowTask.CreationTime
                });
                #endregion


                #region 删除后面节点相关数据
                //删除后面节点候选人
                await _cityOceanRepository.Context.Ado.ExecuteCommandAsync(@"
                Delete tc From CO_WF.dbo.Tasks t INNER JOIN CO_WF.dbo.TaskCandidates tc ON t.Id=tc.TaskId 
                Where t.WorkflowId=@WorkflowId And t.CreationTime > @CreationTime And t.Id <> @TaskId", new
                {
                    WorkflowId = workflowTask.WorkflowId,
                    TaskId = dto.ReturnTaskId,
                    CreationTime = workflowTask.CreationTime
                });
                //删除后面节点处理人
                await _cityOceanRepository.Context.Ado.ExecuteCommandAsync(@"
                Delete tp From CO_WF.dbo.Tasks t INNER JOIN CO_WF.dbo.TaskProcesses tp ON t.Id=tp.TaskId 
                Where t.WorkflowId=@WorkflowId And t.CreationTime > @CreationTime And t.Id <> @TaskId", new
                {
                    WorkflowId = workflowTask.WorkflowId,
                    TaskId = dto.ReturnTaskId,
                    CreationTime = workflowTask.CreationTime
                });
                //删除后面节点标签
                await _cityOceanRepository.Context.Ado.ExecuteCommandAsync(@"
                Delete tt From CO_WF.dbo.Tasks t INNER JOIN CO_WF.dbo.TaskTags tt ON t.Id=tt.TaskId 
                Where t.WorkflowId=@WorkflowId And t.CreationTime > @CreationTime And t.Id <> @TaskId", new
                {
                    WorkflowId = workflowTask.WorkflowId,
                    TaskId = dto.ReturnTaskId,
                    CreationTime = workflowTask.CreationTime
                });
                //删除后面节点催办人
                await _cityOceanRepository.Context.Ado.ExecuteCommandAsync(@"
                Delete tr From CO_WF.dbo.Tasks t INNER JOIN CO_WF.dbo.TaskReminders tr ON t.Id=tr.TaskId 
                Where t.WorkflowId=@WorkflowId And t.CreationTime > @CreationTime And t.Id <> @TaskId", new
                {
                    WorkflowId = workflowTask.WorkflowId,
                    TaskId = dto.ReturnTaskId,
                    CreationTime = workflowTask.CreationTime
                });
                //删除后面节点附件
                await _cityOceanRepository.Context.Ado.ExecuteCommandAsync(@"
                Delete ta From CO_WF.dbo.Tasks t INNER JOIN CO_WF.dbo.TaskAttachments ta ON t.Id=ta.TaskId 
                Where t.WorkflowId=@WorkflowId And t.CreationTime > @CreationTime And t.Id <> @TaskId", new
                {
                    WorkflowId = workflowTask.WorkflowId,
                    TaskId = dto.ReturnTaskId,
                    CreationTime = workflowTask.CreationTime
                });
                //删除后面节点
                await _cityOceanRepository.Context.Ado.ExecuteCommandAsync(@"
                Delete t From CO_WF.dbo.Tasks t Where t.WorkflowId=@WorkflowId And t.CreationTime > @CreationTime And t.Id <> @TaskId", new
                {
                    WorkflowId = workflowTask.WorkflowId,
                    TaskId = dto.ReturnTaskId,
                    CreationTime = workflowTask.CreationTime
                });
                #endregion

                if (dto.UpdateTaskDefinition)
                {
                    await _cityOceanRepository.Context.Ado.ExecuteCommandAsync(@"
                Insert into CO_WF.elsa.ActivityInstances_Bak_Auto Select ac.* 
                FROM co_wf.elsa.ActivityInstances ac INNER JOIN CO_WF.elsa.WorkflowInstances wi ON ac.WorkflowInstanceId=wi.Id 
                INNER JOIN CO_WF.dbo.Workflows w ON w.ElsaWorkflowInstanceId=wi.InstanceId 
                INNER JOIN CO_WF.elsa.ActivityDefinitions ad ON ac.ActivityId = ad.ActivityId 
                WHERE ac.ActivityId=@ActivityId AND w.Id=@WorkflowId", new
                    {
                        ActivityId = workflowTask.ElsaActivityInstanceId,
                        WorkflowId = workflowTask.WorkflowId
                    });

                    await _cityOceanRepository.Context.Ado.ExecuteCommandAsync(@"
                UPDATE ac set ac.State=ad.State 
                FROM co_wf.elsa.ActivityInstances ac INNER JOIN CO_WF.elsa.WorkflowInstances wi ON ac.WorkflowInstanceId=wi.Id 
                INNER JOIN CO_WF.dbo.Workflows w ON w.ElsaWorkflowInstanceId=wi.InstanceId 
                INNER JOIN CO_WF.elsa.ActivityDefinitions ad ON ac.ActivityId = ad.ActivityId 
                WHERE ac.ActivityId=@ActivityId AND w.Id=@WorkflowId", new
                    {
                        ActivityId = workflowTask.ElsaActivityInstanceId,
                        WorkflowId = workflowTask.WorkflowId
                    });
                }

                _cityOceanRepository.Context.Ado.CommitTran();
            }
            catch (Exception ex)
            {
                _cityOceanRepository.Context.Ado.RollbackTran();
                throw;
            }
        }

    }
}
