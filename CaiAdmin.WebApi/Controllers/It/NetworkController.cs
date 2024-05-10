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
    public class NetworkController : AutoRouteAuthorizeControllerBase
    {
        private readonly ItNetworkService _service;
        public NetworkController(ItNetworkService service)
        {
            _service = service;
        }
        //[HttpGet]
        //public ResultDto<List<ItNetworkDto>> GetList([FromQuery]ItNetworkQueryDto dto)
        //{
        //    return _service.GetList(dto);
        //}
        //[HttpPost]
        //public ResultDto<ItNetworkDto> Save([FromForm]ItNetworkSaveDto dto)
        //{
        //    return _service.SaveWithTransaction(dto);
        //}
        //[HttpDelete]
        //public ResultDto Delete([FromForm]int id)
        //{
        //    _service.Delete(id);
        //    return ResultDto.Success();
        //}
        //[HttpGet]
        //public ResultDto<List<string>> GetPropList(string prop, string keyword)
        //{
        //    return _service.GetPropList(prop, keyword);
        //}
    }
}