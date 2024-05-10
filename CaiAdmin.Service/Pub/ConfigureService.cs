using CaiAdmin.ApiService.CityOcean;
using CaiAdmin.Common;
using CaiAdmin.Dto;
using CaiAdmin.Dto.Fam;
using CaiAdmin.Database;
using CaiAdmin.Service;
using CaiAdmin.Service.Fam;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CaiAdmin.Dto.Pub;

namespace CaiAdmin.Service.Pub
{
    public class ConfigureService
    {
        private readonly CityOceanRepository _cityOceanRepository;
        private readonly CityOceanApiService _cityOceanApiService;
        public ConfigureService(CityOceanRepository cityOceanRepository, CityOceanApiService cityOceanApiService)
        {
            _cityOceanRepository = cityOceanRepository;
            _cityOceanApiService = cityOceanApiService;
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>        
        public async Task<PageListDto<ConfigureDto>> GetPageListAsync(ConfigureQueryDto queryDto)
        {
            var result = new PageListDto<ConfigureDto>();
            RefAsync<int> total = 0;
            var data = await _cityOceanRepository.Context
                .SqlQueryable<ConfigureDto>(@" 
                Select 
                    Id,
                    AccountingClosingDate,
                    BusinessClosingDate,
                    ChargingClosingDate,
                    CompanyId,
                    (SELECT TOP 1 JSON_VALUE(LocalizationText,'$.DisplayName.zh') Name FROM CO_Platform.dbo.OrganizationUnits ou WHERE ou.Id=c.CompanyId) AS CompanyName,
                    CustomerId,
                    (SELECT TOP 1 ZhName From CO_Crm.dbo.Customers cus WHERE cus.Id=c.CustomerId) AS CustomerName,
                    IsDeleted,
                    StandardCurrency,
                    DefaultCurrency,
                    ShortCode,
                    CreationTime,
                    ISNULL(LastModificationTime,CreationTime) as LastModificationTime
                From CO_PUB.dbo.Configures c")
                 .WhereIF(!string.IsNullOrWhiteSpace(queryDto.CompanyName), o => o.CompanyName.Contains(queryDto.CompanyName))
                 .WhereIF(!string.IsNullOrWhiteSpace(queryDto.CustomerName), o => o.CustomerName.Contains(queryDto.CustomerName))
                 .OrderByIF(string.IsNullOrWhiteSpace(queryDto.Order), o => o.LastModificationTime, OrderByType.Desc)
                 .OrderByIF(!string.IsNullOrWhiteSpace(queryDto.OrderField), queryDto.Order)
                 .ToPageListAsync(queryDto.PageIndex, queryDto.PageSize, total);
            result.List = data;
            result.Total = total.Value;
            return result;
        }


        /// <summary>
        /// 获取账单详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<BillDetailDto> GetDetailAsync(Guid Id)
        {
            var bill = await this._cityOceanRepository.Context.SqlQueryable<BillDetailDto>(@"Select 
                b.Id,
                b.No,
                b.RefId,
                b.AccountDate,
                b.DueDate,
                b.CreationTime,                    
                (Select top 1 CName From CO_SSO.dbo.Users u where u.Id = b.CreatorUserId) as CreatorUserName,
                b.LastModificationTime
                From CO_FAM.dbo.Bills b 
            ").Where(i => i.Id == Id).FirstAsync();
            if (bill == null)
            {
                throw new ApiFailException(ApiFailCode.OPERATION_FAIL, "账单不存在");
            }
            return bill;
        }
    }
}
