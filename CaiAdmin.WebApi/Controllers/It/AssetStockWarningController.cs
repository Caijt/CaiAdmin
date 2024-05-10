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
    [OpenApiTag("库存预警管理")]
    [ApiGroup(ApiGroupNames.It)]
    [Authorize(AuthorizationPolicyNames.ApiPermission)]
    public class AssetStockWarningController : AutoRouteAuthorizeControllerBase
    {
        private readonly ItAssetStockWarningService _service;
        public AssetStockWarningController(ItAssetStockWarningService service)
        {
            _service = service;
        }

        //[HttpGet]
        //public ResultDto<PageListDto<ItAssetStockWarningDto>> GetPageList([FromQuery]ItAssetStockWarningQueryDto dto)
        //{
        //    return _service.GetPageList(dto);
        //}
        //[HttpGet]
        //public ResultDto<List<ItAssetStockWarningDto>> GetList([FromQuery]ItAssetStockWarningQueryDto dto)
        //{
        //    return _service.GetList(dto);
        //}
        //[HttpPost]
        //public ResultDto<ItAssetStockWarningDto> Save([FromForm]ItAssetStockWarningSaveDto dto)
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