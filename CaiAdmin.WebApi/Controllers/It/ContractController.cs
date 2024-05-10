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
    public class ContractController : AutoRouteAuthorizeControllerBase
    {
        private readonly ItContractService _service;
        public ContractController(ItContractService service)
        {
            _service = service;
        }
        //[HttpGet]
        //public ResultDto<PageListDto<ItContractDto>> GetPageList([FromQuery]ItContractQueryDto dto)
        //{
        //    return _service.GetPageList(dto);
        //}
        //[HttpGet]
        //public ResultDto<PageListSummaryDto<ItContractDto>> GetPageListWithSummary([FromQuery]ItContractQueryDto dto)
        //{
        //    return _service.GetPageListWithSummary(dto);
        //}
        //[HttpGet]
        //public ResultDto<List<ItContractDto>> GetList([FromQuery]ItContractQueryDto dto)
        //{
        //    return _service.GetList(dto);
        //}
        //[HttpPost]
        //public ResultDto<ItContractDto> Save([FromForm]ItContractSaveDto dto)
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
        //    [FromQuery]TimeStatisticQueryDto timeStatisticQueryDto, [FromQuery]ItContractQueryDto queryDto)
        //{
        //    return _service.GetTimeStatistic(timeStatisticQueryDto, queryDto);
        //}
    }
}