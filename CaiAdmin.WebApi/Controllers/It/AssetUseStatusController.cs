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
    public class AssetUseStatusController : AutoRouteAuthorizeControllerBase
    {
        private readonly ItAssetUseStatusService _service;
        public AssetUseStatusController(ItAssetUseStatusService service)
        {
            _service = service;
        }
        //[HttpGet]
        //public IActionResult test()
        //{
        //    return Ok(_service.test());
        //}
        //[HttpGet]
        //public ResultDto<PageListDto<ItAssetUseStatusDto>> GetPageList([FromQuery]ItAssetUseStatusQueryDto dto)
        //{
        //    return _service.GetPageList(dto);
        //}
        //[HttpGet]
        //public ResultDto<List<ItAssetUseStatusDto>> GetList([FromQuery]ItAssetUseStatusQueryDto dto)
        //{
        //    return _service.GetList(dto);
        //}
    }
}