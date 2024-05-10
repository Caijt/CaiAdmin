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
    [OpenApiTag("供应商管理")]
    [ApiGroup(ApiGroupNames.Fam)]
    [Authorize(AuthorizationPolicyNames.ApiPermission)]
    public class SupplierController : AutoRouteAuthorizeControllerBase
    {
        private readonly ItSupplierService _service;
        public SupplierController(ItSupplierService service)
        {
            _service = service;
        }
        //[HttpGet]
        //public ResultDto<PageListDto<ItSupplierDto>> GetPageList([FromQuery]ItSupplierQueryDto dto)
        //{

        //    return _service.GetPageList(dto);
        //}
        //[HttpPost]
        //public ResultDto<ItSupplierDto> Save([FromForm]ItSupplierSaveDto dto)
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
        //public ResultDto<ItSupplierDto> GetDetails(int id)
        //{
        //    return _service.Get(id);
        //}
    }
}