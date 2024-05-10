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
    public class AssetUseRecordItemController : AutoRouteAuthorizeControllerBase
    {
        private readonly ItAssetUseRecordItemService _service;
        public AssetUseRecordItemController(ItAssetUseRecordItemService service)
        {
            _service = service;
        }
        //[HttpGet]
        //public ResultDto<PageListDto<ItAssetUseRecordItemDto>> GetPageList([FromQuery]ItAssetUseRecordItemQueryDto dto)
        //{
        //    return _service.GetPageList(dto);
        //}
        //[HttpGet]
        //public ResultDto<List<ItAssetUseRecordItemDto>> GetList([FromQuery]ItAssetUseRecordItemQueryDto dto)
        //{
        //    return _service.GetList(dto);
        //}
    }
}