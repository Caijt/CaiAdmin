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
    [OpenApiTag("资产领用记录管理")]
    [ApiGroup(ApiGroupNames.It)]
    [Authorize(AuthorizationPolicyNames.ApiPermission)]
    public class AssetUseRecordController : AutoRouteAuthorizeControllerBase
    {
        private readonly ItAssetUseRecordService _service;
        public AssetUseRecordController(ItAssetUseRecordService service)
        {
            _service = service;
        }

        //[HttpGet]
        //public ResultDto<PageListDto<ItAssetUseRecordDto>> GetPageList([FromQuery]ItAssetUseRecordQueryDto dto)
        //{
        //    return _service.GetPageList(dto);
        //}

        //[HttpPost]
        //public ResultDto<ItAssetUseRecordDto> Save([FromForm]ItAssetUseRecordSaveDto dto)
        //{
        //    return _service.SaveWithTransaction(dto);
        //}

        //[HttpGet]
        //public ResultDto<ItAssetUseRecordDto> GetDetails(int id)
        //{
        //    return _service.Get(id);
        //}

        //[HttpGet]
        //public ResultDto<List<ItAssetUseRecordPrintDto>> GetPrintList([FromQuery]ItAssetUseRecordQueryDto dto)
        //{
        //    return _service.GetPrintList(dto);
        //}
        
    }
}