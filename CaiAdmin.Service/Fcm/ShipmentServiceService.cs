using CaiAdmin.ApiService.CityOcean;
using CaiAdmin.Common;
using CaiAdmin.Dto;
using CaiAdmin.Dto.Fcm;
using CaiAdmin.Database;
using CaiAdmin.Service;
using CaiAdmin.Service.Fcm;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaiAdmin.Service.Fcm
{
    public class ShipmentServiceService
    {
        private readonly CityOceanRepository _cityOceanRepository;
        private readonly CityOceanApiService _cityOceanApiService;
        public ShipmentServiceService(CityOceanRepository cityOceanRepository, CityOceanApiService cityOceanApiService)
        {
            _cityOceanRepository = cityOceanRepository;
            _cityOceanApiService = cityOceanApiService;
        }

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>        
        public async Task<PageListDto<ShipmentServiceDto>> GetPageListAsync(ShipmentServiceQueryDto queryDto)
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
            var result = new PageListDto<ShipmentServiceDto>();
            RefAsync<int> total = 0;
            var data = await _cityOceanRepository.Context
                .SqlQueryable<ShipmentServiceDto>(@"Select 
                    ss.Id,
                    ss.ServiceNo,
                    ss.CreationTime,                    
                    (Select top 1 CName From CO_SSO.dbo.Users u where u.Id = ss.CreatorUserId) as CreatorUserName
                 From CO_FCM.dbo.ShipmentServices ss")
                 .WhereIF(queryDto.Ids != null && queryDto.Ids.Any(), o => queryDto.Ids.Contains(o.Id))
                 .OrderByIF(string.IsNullOrWhiteSpace(queryDto.OrderField), o => o.CreationTime, OrderByType.Desc)
                 .OrderByIF(!string.IsNullOrWhiteSpace(queryDto.OrderField), $"{queryDto.OrderField} {(queryDto.OrderDesc == true ? "desc" : "asc")}")
                 .ToPageListAsync(queryDto.PageIndex, queryDto.PageSize, total);
            result.List = data;
            result.Total = total.Value;
            return result;
        }


        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        //public async Task<BillDetailDto> GetDetailAsync(Guid Id)
        //{
        //    var bill = await this._cityOceanRepository.Context.SqlQueryable<BillDetailDto>(@"Select 
        //        b.Id,
        //        b.No,
        //        b.RefId,
        //        b.AccountDate,
        //        b.DueDate,
        //        b.CreationTime,                    
        //        (Select top 1 CName From CO_SSO.dbo.Users u where u.Id = b.CreatorUserId) as CreatorUserName,
        //        b.LastModificationTime
        //        From CO_FAM.dbo.Bills b 
        //    ").Where(i => i.Id == Id).FirstAsync();
        //    if (bill == null)
        //    {
        //        throw new ApiFailException(ApiFailCode.OPERATION_FAIL, "账单不存在");
        //    }
        //    bill.Charges = await this._cityOceanRepository.Context.SqlQueryable<BillChargesDto>(@"Select 
        //        bc.Id,
        //        bc.BillId,
        //        bc.Currency,
        //        bc.Amount,
        //        bc.ChargeCodeName,
        //        bc.CreationTime,                    
        //        (Select top 1 isnull(CName,name) From CO_SSO.dbo.Users u where u.Id = bc.CreatorUserId) as CreatorUserName,
        //        bc.LastModificationTime
        //        From CO_FAM.dbo.BillCharges bc 
        //    ").Where(i => i.BillId == Id).ToListAsync();
        //    bill.Workflows = await this._cityOceanRepository.Context.SqlQueryable<BillWorkflowDto>(@"Select 
        //        bw.WorkflowId,
        //        bw.Currency,
        //        bw.Amount,
        //        bw.BillId,
        //        w.No as WorkflowNo,
        //        bw.CurrencyAmount,
        //        bw.ApplyAmount,
        //        bw.CreationTime,                    
        //        bw.LastModificationTime
        //        From CO_FAM.dbo.BillWorkflows bw LEFT JOIN CO_WF.dbo.Workflows w ON w.Id=bw.WorkflowId
        //    ").Where(i => i.BillId == Id).ToListAsync();
        //    return bill;
        //}


        //public async Task<PageListDto<BillCdcDto>> GetCdcPageListAsync(BillQueryDto queryDto)
        //{
        //    var result = new PageListDto<BillCdcDto>();
        //    RefAsync<int> total = 0;
        //    var data = await _cityOceanRepository.Context
        //        .SqlQueryable<BillCdcDto>(@"SELECT 
        //            co_fam.sys.fn_cdc_map_lsn_to_time(__$start_lsn) AS 'CdcTime', 
        //            CreationTime, 
        //            LastModificationTime,Id, 
        //            CreatorUserId,LastModifierUserId, 
        //            __$operation as CdcOperationType,
        //            Amount,Currency 
        //         FROM co_fam.[cdc].[dbo_Bills_CT] ORDER BY [__$start_lsn] DESC , [__$seqval] desc")
        //         .WhereIF(!string.IsNullOrWhiteSpace(queryDto.No), o => o.No.Contains(queryDto.No))
        //         .OrderByIF(string.IsNullOrWhiteSpace(queryDto.OrderField), o => o.CreationTime, OrderByType.Desc)
        //         .OrderByIF(!string.IsNullOrWhiteSpace(queryDto.OrderField), $"{queryDto.OrderField} {(queryDto.OrderDesc == true ? "desc" : "asc")}")
        //         .ToPageListAsync(queryDto.PageIndex, queryDto.PageSize, total);
        //    result.List = data;
        //    result.Total = total.Value;
        //    return result;
        //}

        //public async Task<PageListDto<BillChargeCdcDto>> GetChargeCdcPageListAsync(BillChargeQueryDto queryDto)
        //{
        //    var result = new PageListDto<BillChargeCdcDto>();
        //    RefAsync<int> total = 0;
        //    var data = await _cityOceanRepository.Context
        //        .SqlQueryable<BillChargeCdcDto>(@"SELECT 
        //            co_fam.sys.fn_cdc_map_lsn_to_time(cdc.__$start_lsn) AS 'CdcTime', 
        //            cdc.[__$start_lsn],
        //            cdc.[__$seqval],
        //            cdc.CreationTime, 
        //            cdc.LastModificationTime,
        //            cdc.Id, 
        //            cdc.BillId,
        //            cdc.__$operation as CdcOperationType,
        //            cdc.ChargeCodeName,
        //            cdc.Amount,
        //            cdc.Currency ,
        //            u.CName as CreatorUserName,
        //            u2.CName as LastModifierUserName   
        //         FROM 
        //        CO_FAM.[cdc].[dbo_BillCharges_CT] cdc
        //            Left Join CO_SSO.dbo.Users u on cdc.CreatorUserId = u.Id
        //            Left Join CO_SSO.dbo.Users u2 on cdc.LastModifierUserId = u2.Id
        //        ")
        //        .WhereIF(queryDto.BillIds != null && queryDto.BillIds.Any(), i => queryDto.BillIds.Contains(i.BillId))
        //        .WhereIF(queryDto.CdcOperationTypes != null && queryDto.CdcOperationTypes.Any(), i => queryDto.CdcOperationTypes.Contains(i.CdcOperationType))
        //         .OrderByIF(!string.IsNullOrWhiteSpace(queryDto.OrderField), $"{queryDto.OrderField} {(queryDto.OrderDesc == true ? "desc" : "asc")}")
        //         .OrderBy($"[__$start_lsn] { (queryDto.OrderDesc == false ? "asc" : "desc") }, [__$seqval] { (queryDto.OrderDesc == false ? "asc" : "desc") }")
        //         .ToPageListAsync(queryDto.PageIndex, queryDto.PageSize, total);
        //    result.List = data;
        //    result.Total = total.Value;
        //    return result;
        //}
    }
}
