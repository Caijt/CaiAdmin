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
    [OpenApiTag("账号管理")]
    [ApiGroup(ApiGroupNames.Fam)]
    [Authorize(AuthorizationPolicyNames.ApiPermission)]
    public class AccountController : AutoRouteAuthorizeControllerBase
    {
        private readonly ItAccountService _service;
        public AccountController(ItAccountService service)
        {
            _service = service;
        }
        [HttpPost]
        public async Task<ApiResultDto<PageListDto<ItAccountDto>>> GetPageList([FromQuery]ItAccountQueryDto dto)
        {
            return await _service.GetPageListAsync(dto);
        }
        //[HttpPost]
        //public ResultDto<ItAccountDto> Save([FromForm]ItAccountSaveDto dto)
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