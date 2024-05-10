using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ItSys.Dto;
using ItSys.Service;
using CaiAdmin.Dto;
using CaiAdmin.WebApi.ApiGroup;
using CaiAdmin.WebApi.AuthorizationPolicies;
using CaiAdmin.WebApi.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace ItSys.Controllers.It
{
    [OpenApiTag("资产维护记录管理")]
    [ApiGroup(ApiGroupNames.Fam)]
    [Authorize(AuthorizationPolicyNames.ApiPermission)]
    public class AssetRepairRecordController : AutoRouteAuthorizeControllerBase
    {
        private readonly ItAssetRepairRecordService _service;
        public AssetRepairRecordController(ItAssetRepairRecordService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ApiResultDto<PageListDto<ItAssetRepairRecordDto>>> GetPageList([FromQuery]ItAssetRepairRecordQueryDto dto)
        {
            //return await _service.(dto);
            return null;
        }

        //[HttpGet]
        //public ResultDto<List<ItAssetRepairRecordDto>> GetList([FromQuery]ItAssetRepairRecordQueryDto dto)
        //{
        //    return _service.GetList(dto);
        //}

        //[HttpPost]
        //public ResultDto<ItAssetRepairRecordDto> Save([FromForm]ItAssetRepairRecordSaveDto dto)
        //{
        //    return _service.Save(dto);
        //}
        //[HttpDelete]
        //public ResultDto Delete([FromForm]int id)
        //{
        //    _service.Delete(id);
        //    return ResultDto.Success();
        //}
    }
}