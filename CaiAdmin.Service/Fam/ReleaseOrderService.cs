using CaiAdmin.ApiService.CityOcean;
using CaiAdmin.ApiService.CityOcean.Dto;
using CaiAdmin.Common;
using CaiAdmin.Database;
using CaiAdmin.Dto;
using CaiAdmin.Dto.Fam;
using CaiAdmin.Service;
using CaiAdmin.Service.Fam;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaiAdmin.Service.Fam
{
    public class ReleaseOrderService
    {
        private readonly CityOceanRepository _cityOceanRepository;
        private readonly CityOceanApiService _cityOceanApiService;
        public ReleaseOrderService(CityOceanRepository cityOceanRepository, CityOceanApiService cityOceanApiService)
        {
            _cityOceanRepository = cityOceanRepository;
            _cityOceanApiService = cityOceanApiService;
        }

        /// <summary>
        /// 同步客户到ES
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        public async Task SyncReleaseOrderToEs(ReleaseOrderQueryDto queryDto)
        {
            queryDto.PageIndex = 1;
            queryDto.PageSize = 1000;
            var total = 0;
            do
            {
                var result = await this.GetPageListAsync(queryDto);
                total = result.Total;
                foreach (var item in result.List)
                {
                    await _cityOceanApiService.SyncReleaseBillOfLadingsToEs(new ElasticSearchDataChangedEventBase
                    {
                        Id = item.Id
                    });
                }
                queryDto.PageIndex++;
            } while ((queryDto.PageIndex - 1) * queryDto.PageSize < total);
        }

        /// <summary>
        /// 获取发票分页数据
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>        
        public async Task<PageListDto<ReleaseOrderDto>> GetPageListAsync(ReleaseOrderQueryDto queryDto)
        {
            if (!string.IsNullOrWhiteSpace(queryDto.IdString))
            {
                var ids = queryDto.IdString.Trim().Split(',', ' ');
                var guidIds = new List<Guid>();
                foreach (var item in ids)
                {
                    if (Guid.TryParse(item, out Guid id))
                    {
                        guidIds.Add(id);
                    }
                }
                if (queryDto.Ids != null && queryDto.Ids.Any())
                {
                    guidIds.AddRange(queryDto.Ids);
                }
                queryDto.Ids = guidIds.ToArray();
            }
            var result = new PageListDto<ReleaseOrderDto>();
            RefAsync<int> total = 0;
            var data = await _cityOceanRepository.Context
                .SqlQueryable<ReleaseOrderDto>(@"
                SELECT 
                    bl.Id,
                    bl.No,
                    bl.Type,
                    bl.ReleaseType,
                    bl.State,
                    rs.Id AS ShipmentId,
                    rs.ServiceNo,
                    s.ShipmentNo,
                    bl.CreationTime,
                    (Select top 1 CName From CO_SSO.dbo.Users u where u.Id = bl.CreatorUserId) CreatorUserName
                FROM 
                co_fam.dbo.ReleaseBillOfLadings bl INNER JOIN co_fam.dbo.ReleaseBillOfShipments rs ON rs.Id=bl.ReleaseBillOfShipmentId
                    INNER JOIN co_fcm.dbo.Shipments s ON s.Id=rs.Id")
                 .WhereIF(queryDto.Ids != null && queryDto.Ids.Any(), o => queryDto.Ids.Contains(o.Id))
                 .WhereIF(!string.IsNullOrWhiteSpace(queryDto.ShipmentNo), o => o.ShipmentNo.Contains(queryDto.ShipmentNo))
                 .WhereIF(!string.IsNullOrWhiteSpace(queryDto.No), o => o.No.Contains(queryDto.No))
                 .WhereIF(queryDto.States != null && queryDto.States.Any(), o => queryDto.States.Contains(o.State))
                 .WhereIF(queryDto.ReleaseTypes != null && queryDto.ReleaseTypes.Any(), o => queryDto.ReleaseTypes.Contains(o.ReleaseType))
                 .WhereIF(queryDto.Types != null && queryDto.Types.Any(), o => queryDto.Types.Contains(o.Type))
                 .WhereIF(queryDto.Ids != null && queryDto.Ids.Any(), o => queryDto.Ids.Contains(o.Id))
                 .OrderByIF(string.IsNullOrWhiteSpace(queryDto.OrderField), o => o.CreationTime, OrderByType.Desc)
                 .OrderByIF(!string.IsNullOrWhiteSpace(queryDto.OrderField), $"{queryDto.OrderField} {(queryDto.OrderDesc == true ? "desc" : "asc")}")
                 .ToPageListAsync(queryDto.PageIndex, queryDto.PageSize, total);
            result.List = data;
            result.Total = total.Value;
            return result;
        }

        /// <summary>
        /// 发送放单邮件
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>        
        public async Task<List<ReleaseBillOfLadingEmailTemplateResult>> SendReleaseOrderEmails(List<ReleaseBillOfLadingWithEmailTemplateDto> inputDto)
        {
            return await _cityOceanApiService.SendReleaseOrderEmails(inputDto);
        }

    }
}
