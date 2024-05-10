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
    [OpenApiTag("资产管理")]
    [ApiGroup(ApiGroupNames.Fam)]
    [Authorize(AuthorizationPolicyNames.ApiPermission)]
    public class AssetReturnRecordController : AutoRouteAuthorizeControllerBase
    {
        private readonly ItAssetReturnRecordService _service;
        public AssetReturnRecordController(ItAssetReturnRecordService service)
        {
            _service = service;
        }
        //[HttpGet]
        //public ResultDto<PageListDto<ItAssetReturnRecordDto>> GetPageList([FromQuery]ItAssetReturnRecordQueryDto dto)
        //{
        //    return _service.GetPageList(dto);
        //}
        //[HttpPost]
        //public ResultDto<ItAssetReturnRecordDto> Save([FromForm]ItAssetReturnRecordSaveDto dto)
        //{
        //    return _service.Save(dto);
        //}
        //[HttpGet]
        //public ResultDto<ItAssetReturnRecordDto> GetDetails(int id)
        //{
        //    return _service.Get(id);
        //}
        //[HttpGet]
        //public ResultDto<List<ItAssetReturnRecordPrintDto>> GetPrintList([FromQuery]ItAssetReturnRecordQueryDto dto)
        //{
        //    return _service.GetPrintList(dto);
        //}
    }
}