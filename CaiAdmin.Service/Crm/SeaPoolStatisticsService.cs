using CaiAdmin.ApiService.CityOcean;
using CaiAdmin.Common;
using CaiAdmin.Dto;
using CaiAdmin.Dto.Crm;
using CaiAdmin.Database;
using CaiAdmin.Database.SqlSugar;
using CaiAdmin.Service;
using CaiAdmin.Service.Crm;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace CaiAdmin.Service.Crm
{
    public class SeaPoolStatisticsService
    {
        private readonly CityOceanRepository _cityOceanRepository;
        private readonly CityOceanApiService _cityOceanApiService;
        public SeaPoolStatisticsService(CityOceanApiService coIcpApiService, CityOceanRepository cityOceanRepository)
        {
            _cityOceanRepository = cityOceanRepository;
            _cityOceanApiService = coIcpApiService;
        }

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>        
        public async Task<PageListSummaryDto<SeaPoolStatisticsDto>> GetPageListAsync(SeaPoolStatisticsQueryDto queryDto)
        {
            var result = new PageListSummaryDto<SeaPoolStatisticsDto>();
            RefAsync<int> total = 0;

            string orderString = "Order by AllocationTime Desc";
            List<string> whereString = new List<string>();
            List<SugarParameter> whereParams = new List<SugarParameter>();
            var whereParams2 = new Dictionary<string, object>();
            if (queryDto.AllocationTimeBegin.HasValue)
            {
                whereString.Add($"chs.AllocationTime >= @AllocationTimeBegin");
                //whereParams.Add(new SugarParameter("AllocationTimeBegin", queryDto.AllocationTimeBegin));
                whereParams2.Add("AllocationTimeBegin", queryDto.AllocationTimeBegin);
            }
            if (queryDto.AllocationTimeEnd.HasValue)
            {
                whereString.Add($"chs.AllocationTime < @AllocationTimeEnd");
                //whereParams.Add(new SugarParameter("AllocationTimeEnd", queryDto.AllocationTimeEnd));
                whereParams2.Add("AllocationTimeEnd", queryDto.AllocationTimeEnd.Value.AddDays(1).Date);
            }
            if (queryDto.CompanyIds != null && queryDto.CompanyIds.Any())
            {
                whereString.Add($"c.CompanyId In (@CompanyIds)");
                //whereParams.Add(new SugarParameter("@CompanyIds", queryDto.CompanyIds) { DbType = System.Data.DbType.Guid });
                whereParams2.Add("CompanyIds", queryDto.CompanyIds);
            }
            if (queryDto.Types != null && queryDto.Types.Length == 1)
            {
                if (queryDto.Types.First() == SeaPoolStatisticsType.Sea)
                {
                    whereString.Add($"chs.CustomerValueType IS NULL");
                }
                else if (queryDto.Types.First() == SeaPoolStatisticsType.HighValue)
                {
                    whereString.Add($"chs.CustomerValueType = 2");
                }
            }
            if (!string.IsNullOrWhiteSpace(queryDto.Order))
            {
                orderString = "Order by " + queryDto.Order;
            }

            string sqlPage = $"OFFSET {(queryDto.PageIndex - 1) * queryDto.PageSize} ROWS FETCH NEXT {queryDto.PageSize} ROWS ONLY";

            //string sqlField = @"Select 
            //           DATEADD(DAY, DATEDIFF(DAY, 0, chs.AllocationTime), 0) AS AllocationTime,
            //           c.CompanyId,
            //           (
            //               SELECT TOP 1
            //                      JSON_VALUE(ou.LocalizationText, '$.DisplayName.zh')
            //               FROM CO_Platform.dbo.OrganizationUnits ou
            //               WHERE ou.Id = c.CompanyId
            //           ) AS CompanyName,
            //           COUNT(1) AS Total,
            //           COUNT(IIF(chs.ClaimStatus = 1, 1, NULL)) AS ClaimTotal,
            //           COUNT(IIF(chs.ReturnType IS NOT NULL, 1, NULL)) AS ReturnTotal,
            //           COUNT(IIF(chs.ReturnType IS NULL AND chs.ClaimStatus = 0, 1, NULL)) AS UnclaimTotal,
            //           COUNT(DISTINCT chs.AllocationUserId) AS SalesmanTotal";
            string sqlBody = $@"
                Select 
                       DATEADD(DAY, DATEDIFF(DAY, 0, chs.AllocationTime), 0) AS AllocationTime,
                       c.CompanyId,
                       (
                           SELECT TOP 1
                                  JSON_VALUE(ou.LocalizationText, '$.DisplayName.zh')
                           FROM CO_Platform.dbo.OrganizationUnits ou
                           WHERE ou.Id = c.CompanyId
                       ) AS CompanyName,
                       COUNT(1) AS Total,
                       COUNT(IIF(chs.ClaimStatus = 1, 1, NULL)) AS ClaimTotal,
                       COUNT(IIF(chs.ReturnType IS NOT NULL, 1, NULL)) AS ReturnTotal,
                       COUNT(IIF(chs.ReturnType IS NULL AND chs.ClaimStatus = 0, 1, NULL)) AS UnclaimTotal,
                       COUNT(DISTINCT chs.AllocationUserId) AS SalesmanTotal
                FROM CO_CRM.dbo.CustomerHighSeass chs
                    INNER JOIN CO_SSO.dbo.Users u
                        ON u.Id = chs.AllocationUserId
                    INNER JOIN CO_PUB.dbo.Configures c
                        ON c.CustomerId = u.CustomerId
                WHERE chs.AllocationStatus = 1
                      AND c.IsDeleted = 0
                      AND chs.IsDeleted = 0
                      {(whereString.Any() ? "AND " + string.Join(" AND ", whereString) : string.Empty)}
                GROUP BY DATEDIFF(DAY, 0, chs.AllocationTime),
                         c.CompanyId";

            result.List = await _cityOceanRepository.Context.Ado.SqlQueryAsync<SeaPoolStatisticsDto>($"{sqlBody} {orderString} {sqlPage}", whereParams2);
            result.Total = await _cityOceanRepository.Context.Ado.SqlQuerySingleAsync<int>($"Select Count(1) From ({sqlBody}) t1", whereParams2);
            result.Summary = await _cityOceanRepository.Context.Ado.SqlQuerySingleAsync<object>(@$"
                Select 
                    Sum(Total) As Total, 
                    Sum(ClaimTotal) As ClaimTotal, 
                    Sum(ReturnTotal) As ReturnTotal, 
                    Sum(UnclaimTotal) As UnclaimTotal, 
                    Sum(SalesmanTotal) As SalesmanTotal
                From ({sqlBody}) t1", whereParams2);
            return result;
        }


        public async Task<PageListDto<A>> GetAllocationUsers(BQuery queryDto)
        {
            var result = new PageListDto<A>();
            RefAsync<int> total = 0;

            string orderString = "Order by AllocationTime Desc";
            List<string> whereString = new List<string>();
            List<SugarParameter> whereParams = new List<SugarParameter>();
            var whereParams2 = new Dictionary<string, object>();
            if (queryDto.AllocationTimeBegin.HasValue)
            {
                whereString.Add($"chs.AllocationTime >= @AllocationTimeBegin");
                //whereParams.Add(new SugarParameter("AllocationTimeBegin", queryDto.AllocationTimeBegin));
                whereParams2.Add("AllocationTimeBegin", queryDto.AllocationTimeBegin);
            }
            if (queryDto.AllocationTimeEnd.HasValue)
            {
                whereString.Add($"chs.AllocationTime < @AllocationTimeEnd");
                //whereParams.Add(new SugarParameter("AllocationTimeEnd", queryDto.AllocationTimeEnd));
                whereParams2.Add("AllocationTimeEnd", queryDto.AllocationTimeEnd.Value.AddDays(1).Date);
            }
            if (queryDto.CompanyIds != null && queryDto.CompanyIds.Any())
            {
                whereString.Add($"c.CompanyId In (@CompanyIds)");
                //whereParams.Add(new SugarParameter("@CompanyIds", queryDto.CompanyIds) { DbType = System.Data.DbType.Guid });
                whereParams2.Add("CompanyIds", queryDto.CompanyIds);
            }
            if (!string.IsNullOrWhiteSpace(queryDto.UserName))
            {
                whereString.Add($"u.CName like @UserName");
                //whereParams.Add(new SugarParameter("@CompanyIds", queryDto.CompanyIds) { DbType = System.Data.DbType.Guid });
                whereParams2.Add("UserName", "%" + queryDto.UserName + "%");
            }
            if (queryDto.Types != null && queryDto.Types.Length == 1)
            {
                if (queryDto.Types.First() == SeaPoolStatisticsType.Sea)
                {
                    whereString.Add($"chs.CustomerValueType IS NULL");
                }
                else if (queryDto.Types.First() == SeaPoolStatisticsType.HighValue)
                {
                    whereString.Add($"chs.CustomerValueType = 2");
                }
            }
            if (!string.IsNullOrWhiteSpace(queryDto.Order))
            {
                orderString = "Order by " + queryDto.Order;
            }

            string sqlPage = $"OFFSET {(queryDto.PageIndex - 1) * queryDto.PageSize} ROWS FETCH NEXT {queryDto.PageSize} ROWS ONLY";

            //string sqlField = @"Select 
            //           DATEADD(DAY, DATEDIFF(DAY, 0, chs.AllocationTime), 0) AS AllocationTime,
            //           c.CompanyId,
            //           (
            //               SELECT TOP 1
            //                      JSON_VALUE(ou.LocalizationText, '$.DisplayName.zh')
            //               FROM CO_Platform.dbo.OrganizationUnits ou
            //               WHERE ou.Id = c.CompanyId
            //           ) AS CompanyName,
            //           COUNT(1) AS Total,
            //           COUNT(IIF(chs.ClaimStatus = 1, 1, NULL)) AS ClaimTotal,
            //           COUNT(IIF(chs.ReturnType IS NOT NULL, 1, NULL)) AS ReturnTotal,
            //           COUNT(IIF(chs.ReturnType IS NULL AND chs.ClaimStatus = 0, 1, NULL)) AS UnclaimTotal,
            //           COUNT(DISTINCT chs.AllocationUserId) AS SalesmanTotal";
            string sqlBody = $@"
                Select 
                       DATEADD(DAY, DATEDIFF(DAY, 0, chs.AllocationTime), 0) AS AllocationTime,
                       c.CompanyId,
                       (
                           SELECT TOP 1
                                  JSON_VALUE(ou.LocalizationText, '$.DisplayName.zh')
                           FROM CO_Platform.dbo.OrganizationUnits ou
                           WHERE ou.Id = c.CompanyId
                       ) AS CompanyName,
                       COUNT(1) AS Total,
                       chs.AllocationUserId,
                       u.CName as AllocationUserName
                FROM CO_CRM.dbo.CustomerHighSeass chs
                    INNER JOIN CO_SSO.dbo.Users u
                        ON u.Id = chs.AllocationUserId
                    INNER JOIN CO_PUB.dbo.Configures c
                        ON c.CustomerId = u.CustomerId
                WHERE chs.AllocationStatus = 1
                      AND c.IsDeleted = 0
                      AND chs.IsDeleted = 0
                      {(whereString.Any() ? "AND " + string.Join(" AND ", whereString) : string.Empty)}
                GROUP BY DATEDIFF(DAY, 0, chs.AllocationTime),
                         c.CompanyId,chs.AllocationUserId,u.CName";

            result.List = await _cityOceanRepository.Context.Ado.SqlQueryAsync<A>($"{sqlBody} {orderString} {sqlPage}", whereParams2);
            result.Total = await _cityOceanRepository.Context.Ado.SqlQuerySingleAsync<int>($"Select Count(1) From ({sqlBody}) t1", whereParams2);
            return result;
        }

        public async Task<PageListDto<C>> GetAllocationRecords(CQuery queryDto)
        {
            var result = new PageListDto<C>();
            RefAsync<int> total = 0;

            string orderString = "Order by AllocationTime Desc";
            List<string> whereString = new List<string>();
            List<SugarParameter> whereParams = new List<SugarParameter>();
            var whereParams2 = new Dictionary<string, object>();
            if (queryDto.AllocationTimeBegin.HasValue)
            {
                whereString.Add($"chs.AllocationTime >= @AllocationTimeBegin");
                //whereParams.Add(new SugarParameter("AllocationTimeBegin", queryDto.AllocationTimeBegin));
                whereParams2.Add("AllocationTimeBegin", queryDto.AllocationTimeBegin);
            }
            if (queryDto.AllocationTimeEnd.HasValue)
            {
                whereString.Add($"chs.AllocationTime < @AllocationTimeEnd");
                //whereParams.Add(new SugarParameter("AllocationTimeEnd", queryDto.AllocationTimeEnd));
                whereParams2.Add("AllocationTimeEnd", queryDto.AllocationTimeEnd.Value.AddDays(1).Date);
            }
            if (queryDto.CompanyIds != null && queryDto.CompanyIds.Any())
            {
                whereString.Add($"c.CompanyId In (@CompanyIds)");
                //whereParams.Add(new SugarParameter("@CompanyIds", queryDto.CompanyIds) { DbType = System.Data.DbType.Guid });
                whereParams2.Add("CompanyIds", queryDto.CompanyIds);
            }
            if (queryDto.Types != null && queryDto.Types.Length == 1)
            {
                if (queryDto.Types.First() == SeaPoolStatisticsType.Sea)
                {
                    whereString.Add($"chs.CustomerValueType IS NULL");
                }
                else if (queryDto.Types.First() == SeaPoolStatisticsType.HighValue)
                {
                    whereString.Add($"chs.CustomerValueType = 2");
                }
            }
            if (queryDto.CustomerTypes != null && queryDto.CustomerTypes.Any())
            {
                whereString.Add($"cus.CustomerType In (@CustomerTypes)");
                whereParams2.Add("CustomerTypes", queryDto.CustomerTypes.Cast<byte>().ToList());
            }
            if (!string.IsNullOrWhiteSpace(queryDto.Order))
            {
                orderString = "Order by " + queryDto.Order;
            }

            string sqlPage = $"OFFSET {(queryDto.PageIndex - 1) * queryDto.PageSize} ROWS FETCH NEXT {queryDto.PageSize} ROWS ONLY";

            string sqlBody = $@"
                Select 
                       chs.AllocationTime,
                       chs.ReturnType,
                       chs.OperationRemark,
                       chs.ClaimTime,
                       chs.ClaimStatus,
                       chs.IsValid,
                       c.CompanyId,
                       (
                           SELECT TOP 1
                                  JSON_VALUE(ou.LocalizationText, '$.DisplayName.zh')
                           FROM CO_Platform.dbo.OrganizationUnits ou
                           WHERE ou.Id = c.CompanyId
                       ) AS CompanyName,
                       chs.AllocationUserId,
                       IIF(chs.CustomerValueType =2,1,0) Type,
                       u.CName as AllocationUserName,
                       cus.ZhName as CustomerName,
                       cus.CustomerType,
                       chs.CustomerId
                FROM CO_CRM.dbo.CustomerHighSeass chs
                    INNER JOIN CO_SSO.dbo.Users u
                        ON u.Id = chs.AllocationUserId
                    INNER JOIN CO_CRM.Dbo.Customers cus on cus.Id = chs.CustomerId
                    INNER JOIN CO_PUB.dbo.Configures c
                        ON c.CustomerId = u.CustomerId
                WHERE chs.AllocationStatus = 1
                      AND c.IsDeleted = 0
                      AND chs.IsDeleted = 0
                      {(whereString.Any() ? "AND " + string.Join(" AND ", whereString) : string.Empty)}";

            result.List = await _cityOceanRepository.Context.Ado.SqlQueryAsync<C>($"{sqlBody} {orderString} {sqlPage}", whereParams2);
            result.Total = await _cityOceanRepository.Context.Ado.SqlQuerySingleAsync<int>($"Select Count(1) From ({sqlBody}) t1", whereParams2);
            return result;
        }
    }
}
