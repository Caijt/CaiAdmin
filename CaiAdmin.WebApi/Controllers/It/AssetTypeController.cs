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
    [OpenApiTag("资产类型管理")]
    [ApiGroup(ApiGroupNames.It)]
    [Authorize(AuthorizationPolicyNames.ApiPermission)]
    public class AssetTypeController : AutoRouteAuthorizeControllerBase
    {
        private readonly ItAssetTypeService _service;
        public AssetTypeController(ItAssetTypeService service)
        {
            _service = service;
        }
        //[HttpGet]
        //public ResultDto<List<ItAssetTypeDto>> GetList([FromQuery]ItAssetTypeQueryDto dto)
        //{
        //    return _service.GetList(dto);
        //}
        //[HttpPost]
        //public ResultDto<ItAssetTypeDto> Save([FromForm]ItAssetTypeDto dto)
        //{
        //    return _service.SaveWithTransaction(dto);
        //}
        //[HttpDelete]
        //public ResultDto Delete([FromForm]int id)
        //{
        //     _service.Delete(id);
        //    return ResultDto.Success();
        //}
    }
}