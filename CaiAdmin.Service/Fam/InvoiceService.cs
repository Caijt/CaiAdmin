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
    public class InvoiceService
    {
        private readonly CityOceanRepository _cityOceanRepository;
        private readonly CityOceanApiService _cityOceanApiService;
        public InvoiceService(CityOceanRepository cityOceanRepository, CityOceanApiService cityOceanApiService)
        {
            _cityOceanRepository = cityOceanRepository;
            _cityOceanApiService = cityOceanApiService;
        }

        /// <summary>
        /// 获取发票分页数据
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>        
        public async Task<PageListDto<InvoiceDto>> GetPageListAsync(InvoiceQueryDto queryDto)
        {
            var result = new PageListDto<InvoiceDto>();
            RefAsync<int> total = 0;
            var data = await _cityOceanRepository.Context
                .SqlQueryable<InvoiceDto>(@"Select 
                    ii.Id,
                    ii.InvoiceNo,ii.InvoiceCode,ii.InvoiceAmount,i.SerialNo,ii.NuoNuoStatusText,ii.Status,ii.NuoNuoSyncFinished,ii.CreationTime,
                    (select top 1 Name from CO_FAM.dbo.InvoiceTitles it where it.InvoiceItemId = ii.Id and it.TitleType=1) As BuyerTitleName,
                    (select top 1 Name from CO_FAM.dbo.InvoiceTitles it where it.InvoiceItemId = ii.Id and it.TitleType=2) As SellerTitleName,
                    ii.NuoNuoMessage,
                    ii.NuoNuoUrl,
                    ii.Remark,
                    (Select top 1 CName From CO_SSO.dbo.Users u where u.Id = ii.CreatorUserId) CreatorUserName
                 From CO_FAM.dbo.InvoiceItems ii left join CO_FAM.dbo.Invoices i on ii.InvoiceId = i.Id")
                 .WhereIF(!string.IsNullOrWhiteSpace(queryDto.InvoiceNo), o => o.InvoiceNo.Contains(queryDto.InvoiceNo))
                 .WhereIF(!string.IsNullOrWhiteSpace(queryDto.InvoiceCode), o => o.InvoiceCode.Contains(queryDto.InvoiceCode))
                 .WhereIF(!string.IsNullOrWhiteSpace(queryDto.SerialNo), o => o.SerialNo.Contains(queryDto.SerialNo))
                 .WhereIF(!string.IsNullOrWhiteSpace(queryDto.BuyerTitleName), o => o.BuyerTitleName.Contains(queryDto.BuyerTitleName))
                 .WhereIF(!string.IsNullOrWhiteSpace(queryDto.SellerTitleName), o => o.SellerTitleName.Contains(queryDto.SellerTitleName))
                 .WhereIF(queryDto.Statuses != null && queryDto.Statuses.Any(), o => queryDto.Statuses.Contains(o.Status))
                 .OrderByIF(string.IsNullOrWhiteSpace(queryDto.OrderField), o => o.CreationTime, OrderByType.Desc)
                 .OrderByIF(!string.IsNullOrWhiteSpace(queryDto.OrderField), $"{queryDto.OrderField} {(queryDto.OrderDesc == true ? "desc" : "asc")}")
                 .ToPageListAsync(queryDto.PageIndex, queryDto.PageSize, total);
            result.List = data;
            result.Total = total.Value;
            return result;
        }


        /// <summary>
        /// 获取发票详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<InvoiceDetailDto> GetDetailAsync(Guid Id)
        {
            var invoice = await this._cityOceanRepository.Context.SqlQueryable<InvoiceDetailDto>(@"Select 
                ii.Id,
                ii.InvoiceNo,
                ii.InvoiceCode,
                ii.InvoiceAmount,
                ii.NuoNuoUrl,
                ii.CreationTime,
                ii.CreatorUserId,
                ii.LastModificationTime,
                ii.Remark,
                ii.Status,
                ii.Way,
                ii.NuoNuoRequestSerialNo,
                ii.NuoNuoMessage,
                ii.NuoNuoStatus,
                ii.NuoNuoStatusText,
                ii.NuoNuoSyncFinished,
                ii.NuoNuoRequestMessage,
                ii.NuoNuoRequestStatus,
                ii.InvoiceType,
                ii.OriginalCurrency
                From CO_FAM.dbo.InvoiceItems ii 
            ").Where(i => i.Id == Id).FirstAsync();
            if (invoice == null)
            {
                throw new ApiFailException(ApiFailCode.OPERATION_FAIL, "发票不存在");
            }
            return invoice;

        }

        /// <summary>
        /// 从诺诺同步状态
        /// </summary>
        /// <param name="invoiceIds"></param>
        /// <returns></returns>
        public async Task UpdateInvoiceFromNuoNuoAsync(List<Guid> invoiceIds)
        {
            await _cityOceanRepository.Context.Ado.ExecuteCommandAsync("Update CO_FAM.dbo.InvoiceItems set NuoNuoSyncFinished=0 Where Id in (@InvoiceIds)", new { InvoiceIds = invoiceIds });
            await _cityOceanApiService.SyncInvoiceStatusFromNuoNuo(invoiceIds);
        }


        /// <summary>
        /// 请求诺诺开票
        /// </summary>
        /// <param name="invoiceIds"></param>
        /// <returns></returns>
        public async Task RequestInvoiceToNuonuoByJob(List<Guid> invoiceIds)
        {
            await _cityOceanApiService.RequestInvoiceToNuonuoByJob(invoiceIds);
        }

    }
}
