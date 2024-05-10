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

namespace CaiAdmin.Service.Fam
{
    public class BillService
    {
        private readonly CityOceanRepository _cityOceanRepository;
        private readonly CityOceanApiService _cityOceanApiService;
        public BillService(CityOceanRepository cityOceanRepository, CityOceanApiService cityOceanApiService)
        {
            _cityOceanRepository = cityOceanRepository;
            _cityOceanApiService = cityOceanApiService;
        }

        /// <summary>
        /// 获取发票分页数据
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>        
        public async Task<PageListDto<BillDto>> GetPageListAsync(BillQueryDto queryDto)
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
            var result = new PageListDto<BillDto>();
            RefAsync<int> total = 0;
            var data = await _cityOceanRepository.Context
                .SqlQueryable<BillDto>(@"Select 
                    b.Id,
                    b.No,
                    b.AccountDate,
                    b.DueDate,
                    b.Type,
                    b.Status,
                    (SELECT TOP 1  isnull(JSON_VALUE(ou.LocalizationText,'$.DisplayName.zh'),ou.DisplayName) FROM CO_Platform.dbo.OrganizationUnits ou WHERE ou.Id=b.companyId) CompanyName,
                    (SELECT TOP 1 Isnull(c.ZhName,c.Name)  FROM CO_Crm.dbo.Customers c WHERE c.Id=b.CustomerId) CustomerName,
                    b.CreationTime,                    
                    (Select top 1 CName From CO_SSO.dbo.Users u where u.Id = b.CreatorUserId) as CreatorUserName,
                    bs.ShipmentServiceType,
                    ss.ServiceNo as ShipmentServiceNo
                 From CO_FAM.dbo.Bills b
                    LEFT JOIN CO_FAM.dbo.BillShipments bs ON bs.BillId = b.Id
                    LEFT JOIN CO_FCM.dbo.ShipmentServices ss ON ss.Id = bs.ShipmentServiceId")
                 .WhereIF(!string.IsNullOrWhiteSpace(queryDto.No), o => o.No.Contains(queryDto.No))
                 .WhereIF(!string.IsNullOrWhiteSpace(queryDto.ShipmentServiceNo), o => o.ShipmentServiceNo.Contains(queryDto.ShipmentServiceNo))
                 .WhereIF(!string.IsNullOrWhiteSpace(queryDto.CustomerName), o => o.CustomerName.Contains(queryDto.CustomerName))
                 .WhereIF(queryDto.Ids != null && queryDto.Ids.Any(), o => queryDto.Ids.Contains(o.Id))
                 .WhereIF(queryDto.Types != null && queryDto.Types.Any(), o => queryDto.Types.Contains(o.Type))
                 .WhereIF(queryDto.ShipmentServiceTypes != null && queryDto.ShipmentServiceTypes.Any(), o => queryDto.ShipmentServiceTypes.Contains(o.ShipmentServiceType))
                 .WhereIF(queryDto.Statuses != null && queryDto.Statuses.Any(), o => queryDto.Statuses.Contains(o.Status))
                 .OrderByIF(string.IsNullOrWhiteSpace(queryDto.OrderField), o => o.CreationTime, OrderByType.Desc)
                 .OrderByIF(!string.IsNullOrWhiteSpace(queryDto.OrderField), $"{queryDto.OrderField} {(queryDto.OrderDesc == true ? "desc" : "asc")}")
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
            bill.Charges = await this._cityOceanRepository.Context.SqlQueryable<BillChargesDto>(@"Select 
                bc.Id,
                bc.BillId,
                bc.Currency,
                bc.Amount,
                bc.PayAmount,
                bc.UnitPrice,
                bc.Qty,
                bc.ChargeCodeName,
                bc.CreationTime,                    
                (Select top 1 isnull(CName,name) From CO_SSO.dbo.Users u where u.Id = bc.CreatorUserId) as CreatorUserName,
                bc.LastModificationTime
                From CO_FAM.dbo.BillCharges bc 
            ").Where(i => i.BillId == Id).ToListAsync();
            bill.Workflows = await this._cityOceanRepository.Context.SqlQueryable<BillWorkflowDto>(@"Select 
                bw.WorkflowId,
                bw.Currency,
                bw.Amount,
                bw.BillId,
                w.No as WorkflowNo,
                bw.CurrencyAmount,
                bw.ApplyAmount,
                bw.CreationTime,                    
                bw.LastModificationTime
                From CO_FAM.dbo.BillWorkflows bw LEFT JOIN CO_WF.dbo.Workflows w ON w.Id=bw.WorkflowId
            ").Where(i => i.BillId == Id).ToListAsync();
            return bill;
        }


        public async Task<PageListDto<BillCdcDto>> GetCdcPageListAsync(BillQueryDto queryDto)
        {
            var result = new PageListDto<BillCdcDto>();
            RefAsync<int> total = 0;
            var data = await _cityOceanRepository.Context
                .SqlQueryable<BillCdcDto>(@"SELECT 
                    co_fam.sys.fn_cdc_map_lsn_to_time(__$start_lsn) AS 'CdcTime', 
                    CreationTime, 
                    LastModificationTime,Id, 
                    CreatorUserId,LastModifierUserId, 
                    __$operation as CdcOperationType,
                    Amount,Currency 
                 FROM co_fam.[cdc].[dbo_Bills_CT] ORDER BY [__$start_lsn] DESC , [__$seqval] desc")
                 .WhereIF(!string.IsNullOrWhiteSpace(queryDto.No), o => o.No.Contains(queryDto.No))
                 .OrderByIF(string.IsNullOrWhiteSpace(queryDto.OrderField), o => o.CreationTime, OrderByType.Desc)
                 .OrderByIF(!string.IsNullOrWhiteSpace(queryDto.OrderField), $"{queryDto.OrderField} {(queryDto.OrderDesc == true ? "desc" : "asc")}")
                 .ToPageListAsync(queryDto.PageIndex, queryDto.PageSize, total);
            result.List = data;
            result.Total = total.Value;
            return result;
        }

        public async Task<PageListDto<BillChargeCdcDto>> GetChargeCdcPageListAsync(BillChargeQueryDto queryDto)
        {
            var result = new PageListDto<BillChargeCdcDto>();
            RefAsync<int> total = 0;
            var data = await _cityOceanRepository.Context
                .SqlQueryable<BillChargeCdcDto>(@"SELECT 
                    co_fam.sys.fn_cdc_map_lsn_to_time(cdc.__$start_lsn) AS 'CdcTime', 
                    cdc.[__$start_lsn],
                    cdc.[__$seqval],
                    cdc.CreationTime, 
                    cdc.LastModificationTime,
                    cdc.Id, 
                    cdc.BillId,
                    cdc.__$operation as CdcOperationType,
                    cdc.ChargeCodeName,
                    cdc.Amount,
                    cdc.Currency ,
                    u.CName as CreatorUserName,
                    u2.CName as LastModifierUserName   
                 FROM 
                CO_FAM.[cdc].[dbo_BillCharges_CT] cdc
                    Left Join CO_SSO.dbo.Users u on cdc.CreatorUserId = u.Id
                    Left Join CO_SSO.dbo.Users u2 on cdc.LastModifierUserId = u2.Id
                ")
                .WhereIF(queryDto.BillIds != null && queryDto.BillIds.Any(), i => queryDto.BillIds.Contains(i.BillId))
                .WhereIF(queryDto.CdcOperationTypes != null && queryDto.CdcOperationTypes.Any(), i => queryDto.CdcOperationTypes.Contains(i.CdcOperationType))
                 .OrderByIF(!string.IsNullOrWhiteSpace(queryDto.OrderField), $"{queryDto.OrderField} {(queryDto.OrderDesc == true ? "desc" : "asc")}")
                 .OrderBy($"[__$start_lsn] { (queryDto.OrderDesc == false ? "asc" : "desc") }, [__$seqval] { (queryDto.OrderDesc == false ? "asc" : "desc") }")
                 .ToPageListAsync(queryDto.PageIndex, queryDto.PageSize, total);
            result.List = data;
            result.Total = total.Value;
            return result;
        }



        public async Task SyncCurrencyBillToEs(BillQueryDto queryDto)
        {
            queryDto.PageIndex = 1;
            queryDto.PageSize = 1000;
            var total = 0;
            do
            {
                var result = await this.GetPageListAsync(queryDto);
                total = result.Total;
                var nos = result.List.Select(i => i.No).ToList();
                await _cityOceanApiService.SyncCurrencyBillToEs(nos);
                queryDto.PageIndex++;
            } while ((queryDto.PageIndex - 1) * queryDto.PageSize < total);
        }

        public async Task SyncChargeBillToEs(BillQueryDto queryDto)
        {
            queryDto.PageIndex = 1;
            queryDto.PageSize = 1000;
            var total = 0;
            do
            {
                var result = await this.GetPageListAsync(queryDto);
                total = result.Total;
                var nos = result.List.Select(i => i.No).ToList();
                await _cityOceanApiService.SyncChargeBillToEs(nos);
                queryDto.PageIndex++;
            } while ((queryDto.PageIndex - 1) * queryDto.PageSize < total);
        }
    }
}
