using CaiAdmin.Service;
using CaiAdmin.Service.Common;
using NSwag.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CaiAdmin.Dto;

namespace CaiAdmin.WebApi.Controllers
{
    public class HomeController : AutoRouteAuthorizeControllerBase
    {
        private readonly HomeService _homeService;
        public HomeController(HomeService homeService)
        {
            _homeService = homeService;
        }

        public async Task<ApiResultDto<TodaySummaryDto>> GetTodaySummary()
        {
            return await _homeService.GetTodaySummaryAsync();
        }
    }
}
