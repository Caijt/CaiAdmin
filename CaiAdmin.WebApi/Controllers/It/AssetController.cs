using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ItSys.Dto;
using ItSys.Service;
using OfficeOpenXml;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using CaiAdmin.WebApi.ApiGroup;
using CaiAdmin.WebApi.AuthorizationPolicies;
using NSwag.Annotations;
using CaiAdmin.WebApi.Controllers;
using CaiAdmin.Dto;

namespace CaiAdmin.Controllers.It
{
    [OpenApiTag("资产管理")]
    [ApiGroup(ApiGroupNames.It)]
    [Authorize(AuthorizationPolicyNames.ApiPermission)]
    public class AssetController : AutoRouteAuthorizeControllerBase
    {
        private ItAssetService _service;
        public AssetController(ItAssetService service)
        {
            _service = service;


        }
        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResultDto<PageListDto<ItAssetDto>>> GetPageList(ItAssetQueryDto dto)
        {
            return await _service.GetPageListAsync(dto);
        }
        //[HttpGet]
        //public ResultDto<PageListSummaryDto<ItAssetDto>> GetPageListWithSummary([FromQuery]ItAssetQueryDto dto)
        //{
        //    return _service.GetPageListWithSummary(dto);
        //}
        //[HttpGet]
        //public Task<ApiResultDto<List<ItAssetDto>>> GetList([FromQuery]ItAssetQueryDto dto)
        //{
        //    return _service.GetList(dto);
        //}
        [HttpGet]
        public async Task<ApiResultDto<ItAssetDto>> GetDetails(int id)
        {
            //return _service.Get(id);
            return null;
        }
        //[HttpGet]
        //public ResultDto<List<dynamic>> GetPropList(string prop, string keyword)
        //{
        //    return _service.GetPropList(prop, keyword);
        //}
        //[HttpPost]
        //public ResultDto<ItAssetDto> Save([FromForm]ItAssetSaveDto dto)
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
        //public ResultDto<List<Dictionary<string, object>>> GetTimeStatistic(
        //    [FromQuery]TimeStatisticQueryDto timeStatisticQueryDto, [FromQuery]ItAssetQueryDto queryDto)
        //{
        //    return _service.GetTimeStatistic(timeStatisticQueryDto, queryDto);
        //}
        //[AllowAnonymous]
        //[HttpGet("{id}")]
        //public IActionResult GetPrintQrcode(int id)
        //{
        //    string url = Request.Scheme + "://" + Request.Host.Value + "/mobile/#/it/asset/details/" + id;
        //    return File(QrcodeHelper.CreateQrcode(url, 3), "image/png");
        //}
        //[HttpGet("{id}")]
        //public IActionResult getDetailsQrcode(int id)
        //{
        //    string url = Request.Scheme + "://" + Request.Host.Value + "/mobile/#/it/asset/detailsNoAuth/" + id;
        //    return File(QrcodeHelper.CreateQrcode(url), "image/png");
        //}

        [HttpPost]
        public async Task<ApiResultDto<int>> ImportExcel([FromForm]IFormFile file)
        {
            return _service.ImportExcel(file.OpenReadStream());
        }
        [HttpPost]
        public async Task<IActionResult> ExportExcel([FromQuery]ItAssetQueryDto dto)
        {
            return File(
                await _service.ExportExcel(dto),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "IT资产列表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx");
        }
    }
}