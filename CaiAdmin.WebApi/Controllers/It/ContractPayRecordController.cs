using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ItSys.Dto;
using ItSys.Service;
using CaiAdmin.WebApi.ApiGroup;
using CaiAdmin.WebApi.AuthorizationPolicies;
using CaiAdmin.WebApi.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace ItSys.Controllers.It
{
    [OpenApiTag("合同付款管理")]
    [ApiGroup(ApiGroupNames.It)]
    [Authorize(AuthorizationPolicyNames.ApiPermission)]
    public class ContractPayRecordController : AutoRouteAuthorizeControllerBase
    {
        private readonly ItContractPayRecordService _service;
        public ContractPayRecordController(ItContractPayRecordService service)
        {
            _service = service;
        }
        //[HttpGet]
        //public ResultDto<PageListDto<ItContractPayRecordDto>> GetPageList([FromQuery]ItContractPayRecordQueryDto dto)
        //{
        //    return _service.GetPageList(dto);
        //}
        //[HttpPost]
        //public ResultDto<ItContractPayRecordDto> Save([FromForm]ItContractPayRecordSaveDto dto)
        //{
        //    return _service.Save(dto);
        //}
        //[HttpDelete]
        //public ResultDto Delete([FromForm]int id)
        //{
        //    _service.Delete(id);
        //    return ResultDto.Success();
        //}
        //[HttpGet]
        //public ResultDto<List<Dictionary<string, object>>> GetTimeStatistic(
        //    [FromQuery]TimeStatisticQueryDto timeStatisticQueryDto, [FromQuery]ItContractPayRecordQueryDto queryDto)
        //{
        //    return _service.GetTimeStatistic(timeStatisticQueryDto, queryDto);
        //}
    }
}