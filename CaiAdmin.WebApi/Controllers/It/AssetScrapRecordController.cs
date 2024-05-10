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
    [OpenApiTag("资产报刻记录管理")]
    [ApiGroup(ApiGroupNames.It)]
    [Authorize(AuthorizationPolicyNames.ApiPermission)]
    public class AssetScrapRecordController : AutoRouteAuthorizeControllerBase
    {
        private readonly ItAssetScrapRecordService _service;
        public AssetScrapRecordController(ItAssetScrapRecordService service)
        {
            _service = service;
        }
        //[HttpGet]
        //public ResultDto<PageListDto<ItAssetScrapRecordDto>> GetPageList([FromQuery]ItAssetScrapRecordQueryDto dto)
        //{
        //    return _service.GetPageList(dto);
        //}
        //[HttpGet]
        //public ResultDto<List<ItAssetScrapRecordDto>> GetList([FromQuery]ItAssetScrapRecordQueryDto dto)
        //{
        //    return _service.GetList(dto);
        //}
        //[HttpPost]
        //public ResultDto<ItAssetScrapRecordDto> Save([FromForm]ItAssetScrapRecordSaveDto dto)
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