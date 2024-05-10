using CaiAdmin.ApiService.CityOcean;
using CaiAdmin.ApiService.CityOcean.Dto;
using CaiAdmin.Dto;
using CaiAdmin.Dto.Crm;
using CaiAdmin.Database;
using CaiAdmin.Database.SqlSugar;
using CaiAdmin.Service;
using CaiAdmin.Service.Crm;
using Microsoft.Extensions.Logging;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace CaiAdmin.Service.Crm
{
    public class CustomerTitleService
    {
        private readonly CityOceanRepository _cityOceanRepository;
        private readonly CityOceanApiService _coIcpApiService;
        private readonly ILogger<CustomerTitleService> _logger;
        public CustomerTitleService(CityOceanRepository cityOceanRepository, CityOceanApiService coIcpApiService, ILogger<CustomerTitleService> logger)
        {
            _cityOceanRepository = cityOceanRepository;
            _coIcpApiService = coIcpApiService;
            _logger = logger;
        }

        /// <summary>
        /// 获取客户抬头分页数据
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>        
        public async Task<PageListDto<OldIcpCustomerTitleDto>> GetOldIcpPageListAsync(OldIcpCustomerTitleQueryDto queryDto)
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
            var result = new PageListDto<OldIcpCustomerTitleDto>();
            RefAsync<int> total = 0;
            var data = await _cityOceanRepository.Context
                .SqlQueryable<OldIcpCustomerTitleDto>(@"Select 
                    t.Id,
                    t.Code,
                    t.Name,
                    t.TaxNo,
                    t.AddressTel,
                    t.BankAccountNo,
                    t.CustomerId,
                    t.Type,
                    t.IsValid,
                    t.CreateBy,
                    t.CreateDate,
                    t.UpdateDate,
                    t.CompanyID,
                    (Select top 1 CName From Icp3.pub.Customers c where c.Id = t.CustomerId) CustomerName
                 From ICP3.pub.CustomerInvoiceTitles t ")
                 .WhereIF(!string.IsNullOrWhiteSpace(queryDto.Name), i => i.Name.Contains(queryDto.Name))
                 .WhereIF(!string.IsNullOrWhiteSpace(queryDto.TaxNo), i => i.TaxNo.Contains(queryDto.TaxNo))
                 .WhereIF(!string.IsNullOrWhiteSpace(queryDto.AddressTel), i => i.AddressTel.Contains(queryDto.AddressTel))
                 .WhereIF(!string.IsNullOrWhiteSpace(queryDto.CustomerName), i => i.CustomerName.Contains(queryDto.CustomerName))
                 .WhereIF(queryDto.Ids != null && queryDto.Ids.Any(), i => queryDto.Ids.Contains(i.Id))
                 .WhereIF(queryDto.CreateDateBegin.HasValue, i => i.CreateDate >= queryDto.CreateDateBegin.Value)
                 .WhereIF(queryDto.CreateDateEnd.HasValue, i => i.CreateDate < queryDto.CreateDateEnd.Value.AddDays(1))
                 .WhereIF(queryDto.Ids != null && queryDto.Ids.Any(), o => queryDto.Ids.Contains(o.Id))
                 .OrderByIF(string.IsNullOrWhiteSpace(queryDto.OrderField), o => o.CreateDate, OrderByType.Desc)
                 .OrderByIF(!string.IsNullOrWhiteSpace(queryDto.OrderField), $"{queryDto.OrderField} {(queryDto.OrderDesc == true ? "desc" : "asc")}")
                 .OrderBy(i => i.Id)
                 .ToPageListAsync(queryDto.PageIndex, queryDto.PageSize, total);
            result.List = data;
            result.Total = total.Value;
            return result;
        }


        /// <summary>
        /// 获取客户抬头分页数据
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>        
        public async Task<PageListDto<CustomerTitleDto>> GetPageListAsync(CustomerTitleQueryDto queryDto)
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
            var result = new PageListDto<CustomerTitleDto>();
            RefAsync<int> total = 0;
            var data = await _cityOceanRepository.Context
                .SqlQueryable<CustomerTitleDto>(@"Select 
                    t.Id,
                    t.Code,
                    t.Name,
                    t.TFN,
                    t.Address,
                    t.Tel,
                    t.Bank1,
                    t.BankAccount1,
                    t.Currency1,
                    t.Bank2,
                    t.BankAccount2,
                    t.Currency2,
                    t.CustomerId,
                    t.Type,
                    t.IsValid,
                    t.CreatorUserId,
                    t.CreationTime,
                    t.LastModificationTime,
                    t.LastModifierUserId,
                    t.IsDeleted,
                    (Select top 1 ZhName From CO_CRM.dbo.Customers c where c.Id = t.CustomerId) CustomerName
                 From CO_CRM.dbo.CustomerTitles t ")
                 .WhereIF(!string.IsNullOrWhiteSpace(queryDto.Name), o => o.Name.Contains(queryDto.Name))
                 .WhereIF(!string.IsNullOrWhiteSpace(queryDto.TaxNo), o => o.TFN.Contains(queryDto.TaxNo))
                 .WhereIF(!string.IsNullOrWhiteSpace(queryDto.Address), o => o.Address.Contains(queryDto.Address))
                 .WhereIF(!string.IsNullOrWhiteSpace(queryDto.Tel), o => o.Address.Contains(queryDto.Tel))
                 .WhereIF(!string.IsNullOrWhiteSpace(queryDto.CustomerName), o => o.CustomerName.Contains(queryDto.CustomerName))
                 .WhereIF(queryDto.Ids != null && queryDto.Ids.Any(), o => queryDto.Ids.Contains(o.Id))
                 .WhereIF(queryDto.CreateDateBegin.HasValue, o => o.CreationTime >= queryDto.CreateDateBegin.Value)
                 .WhereIF(queryDto.CreateDateEnd.HasValue, o => o.CreationTime < queryDto.CreateDateEnd.Value.AddDays(1))
                 .WhereIF(queryDto.Ids != null && queryDto.Ids.Any(), o => queryDto.Ids.Contains(o.Id))
                 .OrderByIF(string.IsNullOrWhiteSpace(queryDto.OrderField), o => o.CreationTime, OrderByType.Desc)
                 .OrderByIF(!string.IsNullOrWhiteSpace(queryDto.OrderField), $"{queryDto.OrderField} {(queryDto.OrderDesc == true ? "desc" : "asc")}")
                 .ToPageListAsync(queryDto.PageIndex, queryDto.PageSize, total);
            result.List = data;
            result.Total = total.Value;
            return result;
        }
        /// <summary>
        /// 获取客户抬头详情
        /// </summary>
        /// <returns></returns>
        public async Task<CustomerDetailDto> GetDetail(Guid id)
        {
            var customer = await _cityOceanRepository.Context.SqlQueryable<CustomerDetailDto>(@"Select Id,
                c.Name,
                c.Code,
                c.EnName,
                c.ZhName,
                c.Tel,
                c.Address,
                c.EnAddress,
                c.ZhAddress,
                c.ShortName,c.MergerId,
                c.CreationTime,
                c.LastModificationTime,
                c.IsDeleted,
                isnull((Select top 1 1 From ICP3.pub.Customers c2 where c2.Id=c.Id),0) as SyncOldIcp
                From CO_CRM.dbo.Customers c").FirstAsync(o => o.Id == id);

            return customer;
        }
        /// <summary>
        /// 获取旧ICP客户抬头详情
        /// </summary>
        /// <returns></returns>
        public async Task<CustomerDetailDto> GetOldIcpDetail(Guid id)
        {
            var customer = await _cityOceanRepository.Context.SqlQueryable<CustomerDetailDto>(@"Select Id,
                c.Name,
                c.Code,
                c.EnName,
                c.ZhName,
                c.Tel,
                c.Address,
                c.EnAddress,
                c.ZhAddress,
                c.ShortName,c.MergerId,
                c.CreationTime,
                c.LastModificationTime,
                c.IsDeleted,
                isnull((Select top 1 1 From ICP3.pub.Customers c2 where c2.Id=c.Id),0) as SyncOldIcp
                From CO_CRM.dbo.Customers c").FirstAsync(o => o.Id == id);

            return customer;
        }


        /// <summary>
        /// 同步旧ICP发票抬头到新系统
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        public async Task ImportOldIcpCustomerTitleToNewIcp(OldIcpCustomerTitleQueryDto queryDto)
        {
            queryDto.PageIndex = 1;
            queryDto.PageSize = 1000;
            var total = 0;
            Dictionary<string, long> user = new Dictionary<string, long>();
            IEnumerable<dynamic> currencyList;
            string defaultCurrency = string.Empty;
            currencyList = _cityOceanRepository.Context.SqlQueryable<dynamic>("Select * from CO_PUB.dbo.Currencies").ToList();
            if (currencyList.Count() > 0)
            {
                defaultCurrency = currencyList.FirstOrDefault(o => o.Name == "RMB").Id.ToString();
            }

            do
            {
                var result = await this.GetOldIcpPageListAsync(queryDto);
                total = result.Total;

                foreach (var item in result.List)
                {
                    _cityOceanRepository.Context.Ado.BeginTran();
                    try
                    {
                        var isExist = _cityOceanRepository.Context.Ado.GetScalar("Select 1 From CO_Crm.dbo.CustomerTitles where id = @id", new { id = item.Id });
                        if (isExist != null)
                        {
                            continue;
                        }
                        var hasCustomer = _cityOceanRepository.Context.Ado.GetScalar("Select 1 From CO_Crm.dbo.Customers where id = @id", new { id = item.CustomerId });
                        if (hasCustomer == null)
                        {
                            continue;
                        }

                        List<string> errors = new List<string>();
                        Regex taxNoRegex = new Regex(@"[a-zA-Z\d]{4,}");
                        var match = taxNoRegex.Match(item.TaxNo);
                        var taxNo = match.Value;
                        if (taxNo?.Length > 100)
                        {
                            taxNo.Substring(0, 100);
                            errors.Add("税号过长");
                        }
                        if (item.Code?.Length > 100)
                        {
                            item.Code.Substring(0, 100);
                            errors.Add("代码过长");
                        }
                        if (item.Name?.Length > 200)
                        {
                            item.Name.Substring(0, 200);
                            errors.Add("名称过长");
                        }
                        long createUserId = 2931; //admin账号
                        if (user.ContainsKey(item.CreateBy.ToString()))
                        {
                            createUserId = user.GetValueOrDefault(item.CreateBy.ToString());
                        }
                        else
                        {
                            var tempUserId = _cityOceanRepository.Context.Ado.GetScalar("Select top 1 Id from CO_SSO.dbo.Users where ICPUserId=@userId", new { userId = item.CreateBy });
                            if (tempUserId != null && long.TryParse(tempUserId.ToString(), out createUserId))
                            {
                                user.Add(item.CreateBy.ToString(), createUserId);
                            }
                        }
                        //long? updateUserId = null;
                        //if (item.UpdateBy != null)
                        //{
                        //    if (user.ContainsKey(item.UpdateBy.ToString()))
                        //    {
                        //        updateUserId = user.GetValueOrDefault(item.UpdateBy.ToString());
                        //    }
                        //    else
                        //    {
                        //        var tempUserId = _dbClient.Ado.GetScalar("Select top 1 Id from CO_SSO.dbo.Users where ICPUserId=@userId", new { userId = item.UpdateBy });
                        //        if (long.TryParse(tempUserId.ToString(), out long userId))
                        //        {
                        //            updateUserId = userId;
                        //            user.Add(item.UpdateBy.ToString(), updateUserId.Value);
                        //        }
                        //    }
                        //}
                        int isValid = item.IsValid ? 1 : 0;
                        int type = (int)item.Type;
                        string address = string.Empty;
                        string tel = string.Empty;
                        item.AddressTel = item.AddressTel ?? string.Empty;
                        item.BankAccountNo = item.BankAccountNo ?? string.Empty;
                        Regex telRegex = new Regex(@"([\d]+[-—])?[\d]{7,11}([\*/][\d]{1,5})*"); //匹配电话
                        var matchResults = telRegex.Matches(item.AddressTel);
                        if (matchResults.Count > 0)
                        {
                            tel = matchResults.FirstOrDefault().Value;
                        }
                        //tel = string.Join(",", matchResults.Select(o => o.Value));
                        if (tel?.Length > 50)
                        {
                            tel.Substring(0, 50);
                            errors.Add("电话过长");
                        }
                        address = telRegex.Replace(item.AddressTel, string.Empty).Trim();
                        if (address?.Length > 500)
                        {
                            address.Substring(0, 500);
                            errors.Add("地址过长");
                        }
                        Regex bankAccountRegex = new Regex(@"[\d -]{7,}");
                        matchResults = bankAccountRegex.Matches(item.BankAccountNo);
                        string bankAccount1 = string.Empty;
                        string bankAccount2 = string.Empty;
                        if (matchResults.Count > 0)
                        {
                            bankAccount1 = matchResults[0].Value.Trim();
                            if (bankAccount1?.Length > 100)
                            {
                                bankAccount1.Substring(0, 100);
                                errors.Add("银行账号1过长");
                            }
                            if (matchResults.Count > 1)
                            {
                                bankAccount2 = matchResults[1].Value.Trim();
                                if (bankAccount2?.Length > 100)
                                {
                                    bankAccount2.Substring(0, 100);
                                    errors.Add("银行账号2过长");
                                }
                            }
                        }

                        Regex currencyRegex = new Regex(@"[a-zA-Z]{1,5}"); //币种
                        matchResults = currencyRegex.Matches(item.BankAccountNo);
                        string currency1 = defaultCurrency;
                        string currency2 = string.Empty;
                        if (matchResults.Count > 0)
                        {
                            var currencyCode = matchResults[0].Value.Trim();
                            var c = currencyList.FirstOrDefault(o => o.Code == currencyCode || o.Name == currencyCode);
                            if (c != null)
                            {
                                currency1 = c.Id.ToString();
                            }
                            if (matchResults.Count > 1)
                            {
                                currencyCode = matchResults[1].Value.Trim();
                                c = currencyList.FirstOrDefault(o => o.Code == currencyCode || o.Name == currencyCode);
                                if (c != null)
                                {
                                    currency2 = c.Id.ToString();
                                }
                            }
                        }
                        else
                        {
                            var indexRmb = item.BankAccountNo.IndexOf("人民币");
                            var indexUsd = item.BankAccountNo.IndexOf("美金");
                            if (indexUsd < 0)
                            {
                                indexUsd = item.BankAccountNo.IndexOf("美元");
                            }
                            if (indexRmb >= 0 && indexRmb < indexUsd)
                            {
                                currency1 = defaultCurrency;
                                if (indexUsd >= 0)
                                {
                                    currency2 = currencyList.FirstOrDefault(o => o.Name == "USD").Id.ToString();
                                }
                            }
                            if (indexUsd >= 0 && indexUsd < indexRmb)
                            {
                                currency1 = currencyList.FirstOrDefault(o => o.Name == "USD").Id.ToString();
                                if (indexRmb >= 0)
                                {
                                    currency1 = defaultCurrency;
                                }
                            }
                        }
                        Regex bankRegex = new Regex(@"[\u4e00-\u9fa5]{5,}");
                        matchResults = bankRegex.Matches(item.BankAccountNo);
                        string bank1 = string.Empty;
                        string bank2 = string.Empty;
                        if (matchResults.Count > 0)
                        {
                            bank1 = matchResults[0].Value.Trim();
                            if (bank1?.Length > 200)
                            {
                                bank1.Substring(0, 200);
                                errors.Add("银行名称1过长");
                            }
                            if (matchResults.Count > 1)
                            {
                                bank2 = matchResults[1].Value.Trim();
                                if (bank2?.Length > 200)
                                {
                                    bank2.Substring(0, 200);
                                    errors.Add("银行名称2过长");
                                }
                                if (string.IsNullOrEmpty(currency2))
                                {
                                    currency2 = defaultCurrency;
                                }
                            }
                        }
                        //导入抬头
                        string sql = @"Insert into CO_CRM.dbo.CustomerTitles(
                                        Id,IsDeleted,IsDefault,CreationTime,CreatorUserId,LastModificationTime,LastModifierUserId,CustomerId,Name,
                                        Code,TFN,Address,Tel,Bank1,BankAccount1,Currency1,Bank2,BankAccount2,Currency2,Type,IsValid,TenantId,RawAddressTel,RawBankAccountNo,RawTaxNo,FromICP,Remark) 
                                        Values(@Id,@IsDeleted,@IsDefault,@CreationTime,@CreatorUserId,Null,Null,@CustomerId,@Name,
                                        @Code,@TFN,@Address,@Tel,@Bank1,@BankAccount1,@Currency1,@Bank2,@BankAccount2,@Currency2,@Type,@IsValid,@TenantId,@RawAddressTel,@RawBankAccountNo,@RawTaxNo,@FromICP,@Remark)";
                        _cityOceanRepository.Context.Ado.ExecuteCommand(sql, new
                        {
                            Id = item.Id,
                            IsDeleted = 0,
                            IsDefault = 0,
                            CreationTime = DateTime.Now,
                            CreatorUserId = createUserId,
                            CustomerId = item.CustomerId,
                            Name = item.Name,
                            Code = item.Code,
                            TFN = taxNo,
                            Address = address,
                            Tel = tel,
                            Bank1 = bank1,
                            BankAccount1 = bankAccount1,
                            Currency1 = currency1,
                            Bank2 = bank2,
                            BankAccount2 = bankAccount2,
                            Currency2 = currency2,
                            Type = type,
                            IsValid = isValid,
                            TenantId = 1,
                            RawAddressTel = item.AddressTel,
                            RawBankAccountNo = item.BankAccountNo,
                            RawTaxNo = item.TaxNo,
                            FromICP = 1,
                            Remark = string.Join(",", errors)
                        });
                        //导入客户关系
                        _cityOceanRepository.Context.Ado.ExecuteCommand(@"insert into CO_Crm.dbo.CustomerRelations(
                        Id,IsDeleted,CreationTime,CreatorUserId,LastModificationTime,LastModifierUserId,CustomerId,OwnerId,IsMerge,RefId,Type,TenantId)
                        Values(@Id,0,GETDATE(),@CreatorUserId,Null,Null,@CustomerId,@CreatorUserId,0,@Id,4,1)", new
                        {
                            Id = item.Id,
                            CreationTime = DateTime.Now,
                            CreatorUserId = createUserId,
                            CustomerId = item.CustomerId,
                            TenantId = 1,
                            FromICP = 1,
                        });

                        //获取ICP开票合同
                        var invoiceContracts = _cityOceanRepository.Context.Ado.SqlQuery<dynamic>(@"select o.id as id, 
                            o.CreateDate as CreationTime,
                            o.StreamId,
                            (select Id from co_sso.dbo.users where ICPUserId = o.CreateBy) CreatorUserId,
                            o.name FileName,
                            (select top 1 c.companyId from co_sso.dbo.users u inner join CO_Platform.dbo.CompanyConfigures c on u.customerId = c.customerId where c.isactive = 1 and u.ICPUserId = o.CreateBy) CompanyId,
                            o.id as OpertaionId
                            from icp3.fcm.OperationFiles o inner join icp3.fam.Invoice i on o.OperationID = i.id  where i.CustomerId = @CustomerId and i.CustomerTaxNo = @TaxNo", new
                        {
                            CustomerId = item.CustomerId,
                            TaxNo = item.TaxNo
                        });

                        foreach (var file in invoiceContracts)
                        {
                            //获取
                            var stream = _cityOceanRepository.Context.Ado.SqlQuerySingle<dynamic>("select file_stream from ICPDoc.dbo.FCMDoc where stream_id =@StreamId", new { StreamId = file.StreamId });
                            if (stream == null || stream.file_stream == null)
                            {
                                continue;
                            }
                            var byteStream = stream.file_stream as byte[];
                            if (byteStream == null)
                            {
                                continue;
                            }
                            UploadResultDto uploadResult;
                            using (MemoryStream ms = new MemoryStream(byteStream))
                            {
                                try
                                {
                                    uploadResult = await _coIcpApiService.UploadFileAsync(new UploadFileDto
                                    {
                                        FileName = file.FileName,
                                        File = ms
                                    });
                                }
                                catch (Exception ex)
                                {
                                    _logger.LogWarning($"客户抬头上传附件失败[ExceptionMessage:{ex.Message},CustomerTitleId:{item.Id},FileName:{file.FileName},StreamId:{file.StreamId}]");
                                    continue;
                                }
                            }
                            var id = Guid.NewGuid();
                            //导入抬头合同
                            sql = @"Insert into CO_Crm.dbo.FreeTaxContracts(
                                    id,
	                                CreationTime,
	                                CreatorUserId,
	                                Name,
	                                ExtensionName,
	                                FromDate,
	                                ToDate,
	                                Status,
	                                CustomerTitleId,
	                                FileId,
	                                TenantId,
	                                CompanyId,
                                    FromICP)
                                Values(@Id,@CreationTime,@CreatorUserId,@FileName,@FileExtensionName,@FromDate,@ToDate,1,@CustomerTitleId,@FileId,1,@CompanyId,1)";
                            _cityOceanRepository.Context.Ado.ExecuteCommand(sql, new
                            {
                                Id = id,
                                CreationTime = file.CreationTime,
                                CreatorUserId = file.CreatorUserId,
                                FileName = Path.GetFileNameWithoutExtension(file.FileName),
                                FileExtensionName = Path.GetExtension(file.FileName as string).TrimStart('.'),
                                FromDate = file.CreationTime,
                                ToDate = (file.CreationTime as DateTime?).Value.AddYears(1),
                                CustomerTitleId = item.Id,
                                CompanyId = file.CompanyId,
                                FileId = uploadResult.FileId
                            });
                            _cityOceanRepository.Context.Ado.CommitTran();

                        }
                    }
                    catch (Exception ex)
                    {
                        _cityOceanRepository.Context.Ado.RollbackTran();
                        throw;
                    }
                }
                queryDto.PageIndex++;
            } while ((queryDto.PageIndex - 1) * queryDto.PageSize < total);
        }

    }



}
