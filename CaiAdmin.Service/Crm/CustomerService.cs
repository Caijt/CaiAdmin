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
    public class CustomerService
    {
        private readonly CityOceanRepository _cityOceanRepository;
        private readonly CityOceanApiService _cityOceanApiService;
        public CustomerService(CityOceanApiService coIcpApiService, CityOceanRepository cityOceanRepository)
        {
            _cityOceanRepository = cityOceanRepository;
            _cityOceanApiService = coIcpApiService;
        }

        /// <summary>
        /// 获取客户分页数据
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>        
        public async Task<PageListDto<CustomerDto>> GetPageListAsync(CustomerQueryDto queryDto)
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
            queryDto.NameUniqueValue = GetNameUniqueValue(queryDto.NameUniqueValue);
            queryDto.NameLocalizationUniqueValue = GetNameUniqueValue(queryDto.NameLocalizationUniqueValue);
            var result = new PageListDto<CustomerDto>();
            RefAsync<int> total = 0;
            var data = await _cityOceanRepository.Context
                .SqlQueryable<CustomerDto>(@"Select 
                    c.Id,
                    c.Code,
                    c.Name,
                    c.EnName,
                    c.ZhName,
                    c.Email,
                    c.Address,
                    c.EnAddress,
                    c.ZhAddress,
                    c.NameUniqueValue,
                    c.NameLocalizationUniqueValue,
                    c.LocalizationText,
                    c.Tel,
                    c.Fax,
                    c.ExamineState,
                    c.MergerId,
                    c.IsDeleted,
                    c.CreationTime,
                    c.LastModificationTime
                 From CO_CRM.dbo.Customers c ")
                 .WhereIF(!string.IsNullOrWhiteSpace(queryDto.Name), o => o.Name.Contains(queryDto.Name))
                 .WhereIF(!string.IsNullOrWhiteSpace(queryDto.Code), o => o.Code.Contains(queryDto.Code))
                 .WhereIF(!string.IsNullOrWhiteSpace(queryDto.EnName), o => o.EnName.Contains(queryDto.EnName))
                 .WhereIF(!string.IsNullOrWhiteSpace(queryDto.ZhName), o => o.ZhName.Contains(queryDto.ZhName))
                 .WhereIF(!string.IsNullOrWhiteSpace(queryDto.LocalizationText), o => o.LocalizationText.Contains(queryDto.LocalizationText))
                 .WhereIF(queryDto.Ids != null && queryDto.Ids.Any(), o => queryDto.Ids.Contains(o.Id))
                 .WhereIF(!string.IsNullOrWhiteSpace(queryDto.NameUniqueValue), o => o.NameUniqueValue == queryDto.NameUniqueValue)
                 .WhereIF(!string.IsNullOrWhiteSpace(queryDto.NameLocalizationUniqueValue), o => o.NameLocalizationUniqueValue == queryDto.NameLocalizationUniqueValue)
                 .WhereIF(!string.IsNullOrWhiteSpace(queryDto.Address), o => o.Address.Contains(queryDto.Address))
                 .OrderByIF(string.IsNullOrWhiteSpace(queryDto.OrderField), o => o.CreationTime, OrderByType.Desc)
                 .OrderByIF(!string.IsNullOrWhiteSpace(queryDto.OrderField), $"{queryDto.OrderField} {(queryDto.OrderDesc == true ? "desc" : "asc")}")
                 .OrderBy(o => o.Id)
                 .ToPageListAsync(queryDto.PageIndex, queryDto.PageSize, total);
            result.List = data;
            result.Total = total.Value;
            return result;
        }
        /// <summary>
        /// 获取客户详情
        /// </summary>
        /// <returns></returns>
        public async Task<CustomerDetailDto> GetDetail(Guid id)
        {
            var customer = await _cityOceanRepository.Context.SqlQueryable<CustomerDetailDto>(@"Select c.Id,
                c.Name,
                c.Code,
                c.EnName,
                c.ZhName,
                c.Tel,
                c.Email,
                c.Fax,
                c.Phone,
                c.Address,
                c.EnAddress,
                c.ZhAddress,
                c.ExamineState,
                c.ShortName,c.MergerId,
                c.CreationTime,
                c.LastModificationTime,
                c.IsDeleted,
                c.CustomerType,
                l.CreationTime as LastTraceLogTime,
                c.ClaimTime,
                c.LastQuotationTime,
                c.LastTradeTime,
                c.CooperationState,
                c.NameLocalizationUniqueValue,c.NameUniqueValue,
                (Select top 1 u.CName From CO_CRM.dbo.CustomerAccessAllows caa inner join CO_SSO.dbo.Users u on u.Id=caa.AllowUserId where caa.IsOwner=1 and caa.customerId=c.Id) as OwnerUserName,
                isnull((Select top 1 1 From ICP3.pub.Customers c2 where c2.Id=c.Id),0) as SyncOldIcp
                From CO_CRM.dbo.Customers c
                    OUTER APPLY (SELECT TOP 1 * FROM CO_CRM.dbo.TraceLogs tl WHERE tl.CustomerId = c.Id ORDER BY tl.CreationTime desc) l").FirstAsync(o => o.Id == id);

            customer.OwnUsers = await _cityOceanRepository.Context.Ado.SqlQueryAsync<CustomerOwnUserDto>(@$"
                Select 
                    u.Id,u.CName as Name,caa.CreationTime
                    From CO_CRM.dbo.CustomerAccessAllows caa left join CO_SSO.dbo.Users u on u.Id=caa.AllowUserId 
                Where caa.IsOwner=1 and caa.customerId=@CustomerId Order By caa.CreationTime
                ", new
            {
                CustomerId = id
            });
            customer.OperationEvents = await _cityOceanRepository.Context.Ado.SqlQueryAsync<CustomerOperationEventDto>(@$"
                SELECT 
                    (Select top 1 CName From CO_SSO.dbo.Users u where u.Id = oe.CreatorUserId) CreatorUserName,
                    oe.CreationTime,
                    JSON_VALUE(oe.LocalizationText,'$.Content.zh') Content
                FROM co_crm.dbo.CustomerOperationEvents oe 
                WHERE OperationCustomerId=@CustomerId ORDER BY oe.CreationTime
                ", new
            {
                CustomerId = id
            });
            customer.Examines = await _cityOceanRepository.Context.Ado.SqlQueryAsync<CustomerExamineDto>(@$"
                SELECT 
                    cx.Id,
                    cx.ExamineType,
                    cx.ExamineState,
                    cx.ExamineTime,
                    cx.ExamineUserId,
                    cx.RefuseReason,
                    cx.CreationTime,
                    cx.ApplyRemark,
                    cx.ApprovalContent,
                    (Select top 1 CName From CO_SSO.dbo.Users u where u.Id = cx.CreatorUserId) CreatorUserName,
                    (SELECT TOP 1 CName FROM CO_SSO.dbo.Users u WHERE u.Id = cx.ExamineUserId) ExamineUserName
                FROM co_crm.dbo.CustomerExamines cx 
                WHERE CustomerId=@CustomerId
                ORDER BY cx.CreationTime 
                ", new
            {
                CustomerId = id
            });
            return customer;
        }
        /// <summary>
        /// 获取旧ICP客户详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<CustomerOldIcpDetailDto> GetOldIcpDetail(Guid id)
        {
            var customer = await _cityOceanRepository.Context.SqlQueryable<CustomerOldIcpDetailDto>(@"Select 
                c.Id,
                c.Code,
                c.EName,
                c.CName,
                c.Tel1,
                c.Tel2,
                c.EAddress,
                c.CAddress,
                c.MergerId,
                (select top 1 cc.State from icp3.pub.CustomerConfirms cc where cc.CustomerID=c.ID order by cc.ApplyDate desc) as ExamineState,
                c.CreateDate,
                c.UpdateDate
                From ICP3.pub.Customers c").FirstAsync(o => o.Id == id);

            return customer;
        }
        /// <summary>
        /// 同步客户到ES
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        public async Task SyncCustomersToEs(CustomerQueryDto queryDto)
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
                    await _cityOceanApiService.SyncCustomerToEs(item.Id);
                }
                queryDto.PageIndex++;
            } while ((queryDto.PageIndex - 1) * queryDto.PageSize < total);
        }

        /// <summary>
        /// 同步客户到ES
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        public async Task SyncCustomerExamineToEs(Guid customerExamineId)
        {
            await _cityOceanApiService.SyncCustomerExamineToEs(customerExamineId);

        }

        /// <summary>
        /// 同步客户到ICP
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        public async Task SyncCustomersToIcp(CustomerQueryDto queryDto)
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
                    await _cityOceanApiService.SyncCustomerToIcp(item.Id);
                }
                queryDto.PageIndex++;
            } while ((queryDto.PageIndex - 1) * queryDto.PageSize < total);
        }

        /// <summary>
        /// 解除合并
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task RelieveMergeAsync(Guid id)
        {
            await _cityOceanRepository.Context.Ado.ExecuteCommandAsync("Update CO_CRM.dbo.Customers set MergerId=Id Where Id =@Id ", new { Id = id });
            await _cityOceanApiService.SyncCustomerToEs(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task ResetNameUniqueValue(CustomerQueryDto queryDto)
        {
            queryDto.PageIndex = 1;
            queryDto.PageSize = 2000;
            var total = 0;
            do
            {
                var result = await this.GetPageListAsync(queryDto);
                total = result.Total;
                foreach (var item in result.List)
                {
                    var name = item.Name;
                    var zhName = System.Web.HttpUtility.HtmlDecode(item.ZhName);

                    var nameUniqueValue = GetNameUniqueValue(name);
                    var zhNameUniqueValue = GetNameUniqueValue(zhName);
                    await _cityOceanRepository.Context.Ado.ExecuteCommandAsync("Update CO_CRM.dbo.Customers set NameUniqueValue=@NameUniqueValue,NameLocalizationUniqueValue=@NameLocalizationUniqueValue Where Id =@Id ", new
                    {
                        Id = item.Id,
                        NameUniqueValue = nameUniqueValue,
                        NameLocalizationUniqueValue = zhNameUniqueValue
                    });
                }
                queryDto.PageIndex++;
            } while ((queryDto.PageIndex - 1) * queryDto.PageSize < total);
        }

        public async Task DecodeCustomerLocalizationText(CustomerQueryDto queryDto)
        {
            queryDto.PageIndex = 1;
            queryDto.PageSize = 2000;
            var total = 0;
            do
            {
                var result = await this.GetPageListAsync(queryDto);
                total = result.Total;
                foreach (var item in result.List)
                {
                    var uncodeValue = HttpUtility.HtmlDecode(item.LocalizationText);
                    await _cityOceanRepository.Context.Ado.ExecuteCommandAsync("Update CO_CRM.dbo.Customers set LocalizationText=@LocalizationText Where Id =@Id ", new
                    {
                        Id = item.Id,
                        LocalizationText = uncodeValue
                    });
                }
                queryDto.PageIndex++;
            } while ((queryDto.PageIndex - 1) * queryDto.PageSize < total);

        }

        public string GetNameUniqueValue(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return string.Empty;
            }
            var invalidWordRegex = new Regex("[^a-zA-Z0-9\u4e00-\u9fa5]");
            name = invalidWordRegex.Replace(name, " ").Trim();

            StringBuilder sb = new StringBuilder(name);
            var invalidWords = new Dictionary<string, int>
            {
                {   "[^a-zA-Z](C[O0][^a-zA-Z0-9\u4e00-\u9fa5]+limited)$",1},
                {   "[^a-zA-Z](C[O0][^a-zA-Z0-9\u4e00-\u9fa5]+LTD)$",1},
                {  "[^a-zA-Z](limited)$",1},
                { "[^a-zA-Z](LTD)$",1},
                {   "[^a-zA-Z](C[O0])$",1},
                {  "[^a-zA-Z](INC)$",1},
                {  "[^a-zA-Z](Company)$",1},
                { "[^a-zA-Z](CompanyLimited)$",1},
                {  "有限公司",0},
            };

            //清除掉无效的字符
            foreach (var item in invalidWords)
            {
                var regex = new Regex(item.Key, RegexOptions.IgnoreCase);
                var m2 = regex.Matches(name);
                for (int i = 0; i < m2.Count; i++)
                {
                    if (m2[i].Success && m2[i].Groups.Count >= item.Value + 1)
                    {
                        var g = m2[i].Groups[item.Value];
                        sb.Replace(g.Value, string.Empty.PadLeft(g.Value.Length, ' '), g.Index, g.Length);
                    }
                }
            }
            //提取所有的中英文数字
            var validWordRegex = new Regex("[a-zA-Z0-9\u4e00-\u9fa5]");
            var validWords = validWordRegex.Matches(sb.ToString());
            return string.Join("", validWords.Select(o => o.Value));
        }

        /// <summary>
        /// 查询审批状态异常的客户列表
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        public async Task<PageListDto<ExamineStatusExceptionCustomerDto>> GetExamineStatusExceptionListAsync(CustomerQueryDto queryDto)
        {
            RefAsync<int> total = 0;
            var list = await _cityOceanRepository.Context
                .SqlQueryable<ExamineStatusExceptionCustomerDto>(@"Select 
                    cus.Id,
                    cus.code,
                    cus.Name,
                    cus.ExamineState, 
                    cus2.Code as OldIcpCode,
                    isNull(t1.State,0) as OldIcpExamineState,
                   (select top 1 ExamineTime from co_crm.dbo.CustomerExamines ce where ce.CustomerId = cus.Id and ce.ExamineType=2 and ce.ExamineState=2 order by CreationTime desc) ExamineTime
                from CO_CRM.dbo.customers cus 
                inner join icp3.pub.customers cus2 on cus.id=cus2.id 
                outer apply (select top 1 * from icp3.pub.CustomerConfirms cc where cc.CustomerID=cus2.ID order by cc.ApplyDate desc) t1  
                where cus.ExamineState=2 and isnull(t1.State,0) <> 3")
                 .WhereIF(!string.IsNullOrWhiteSpace(queryDto.Name), o => o.Name.Contains(queryDto.Name))
                 .WhereIF(!string.IsNullOrWhiteSpace(queryDto.Code), o => o.Code.Contains(queryDto.Code))
                 .OrderByIF(string.IsNullOrWhiteSpace(queryDto.OrderField), o => o.ExamineTime, OrderByType.Desc)
                 .OrderByIF(!string.IsNullOrWhiteSpace(queryDto.OrderField), $"{queryDto.OrderField} {(queryDto.OrderDesc == true ? "desc" : "asc")}")
                .ToPageListAsync(queryDto.PageIndex, queryDto.PageSize, total);
            return new PageListDto<ExamineStatusExceptionCustomerDto>
            {
                List = list,
                Total = total
            };
        }

        /// <summary>
        /// 修复客户审批状态
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task RepairExamineStatus(CustomerQueryDto queryDto)
        {
            var whereStr = string.Empty;
            if (queryDto.Ids != null && queryDto.Ids.Any())
            {
                whereStr = $" and cus.Id in ({string.Join(",", queryDto.Ids.Select(o => "'" + o + "'"))})";
            }
            if (!string.IsNullOrWhiteSpace(queryDto.Name))
            {
                whereStr = $" and cus.Name like '%'+{queryDto.Name.ToSqlValue()}+'%'";
            }
            if (!string.IsNullOrWhiteSpace(queryDto.Code))
            {
                whereStr = $" and cus.Code like '%+{queryDto.Code.ToSqlValue()}+'%'";
            }
            await _cityOceanRepository.Context.Ado.ExecuteCommandAsync($@"Update cus2 
                Set cus2.Code = cus.Code, cus2.Tel1 = iif(cus.Tel is not null and cus.Tel <> '',cus.Tel,cus2.Tel1), UpdateDate = GetDate()
                From CO_CRM.dbo.customers cus 
                inner join icp3.pub.customers cus2 on cus.id=cus2.id 
                outer apply (select top 1 * from icp3.pub.CustomerConfirms cc where cc.CustomerID=cus2.ID order by cc.ApplyDate desc) t1 
                Where cus.ExamineState=2 and isnull(t1.State,0) <> 3 and cus.Code <> cus2.Code
                {whereStr}
            ");

            await _cityOceanRepository.Context.Ado.ExecuteCommandAsync($@"INSERT INTO 
                icp3.[pub].[CustomerConfirms](
                    [ID], 
                    [CustomerID], 
                    [ApplicantID], 
                    [ApplyDate], 
                    [ConfirmorID], 
                    [ConfirmDate], 
                    [ApplicantRemark], 
                    [ConfirmorRemark], 
                    [State]) 
                Select 
                    NEWID(),
                    cus.Id,
                    '4047CFAD-ECC8-E111-9D0D-0026551CA87B',
                    GETDATE(),
                    '4047CFAD-ECC8-E111-9D0D-0026551CA87B',
                    GETDATE(),
                    N'系统自动生成',
                    N'系统自动生成',
                    3 
                From CO_CRM.dbo.customers cus 
                inner join icp3.pub.customers cus2 on cus.id=cus2.id 
                outer apply (select top 1 * from icp3.pub.CustomerConfirms cc where cc.CustomerID=cus2.ID order by cc.ApplyDate desc) t1 
                Where cus.ExamineState=2 and isnull(t1.State,0) <> 3 
                {whereStr}
            ");


        }

        public async Task<PageListDto<NameUniqueValueRepeatDto>> GetNameUniqueValueRepeatList(CustomerRepeatQueryDto queryDto)
        {
            var nameUniqueValues = new List<string>();
            if (!string.IsNullOrWhiteSpace(queryDto.NameUniqueValue))
            {
                var values = queryDto.NameUniqueValue.Trim().Split(',', ' ');
                foreach (var item in values)
                {
                    nameUniqueValues.Add(GetNameUniqueValue(item));
                }
            }
            var whereList = new List<string>();
            if (queryDto.ExcludeDelete)
            {
                whereList.Add(" IsDeleted = 0 ");
            }
            if (queryDto.ExcludeMerge)
            {
                whereList.Add(" Id = MergerId ");
            }
            var whereStr = whereList.Count > 0 ? ("Where" + string.Join(" and ", whereList)) : string.Empty;
            var result = new PageListDto<NameUniqueValueRepeatDto>();
            RefAsync<int> total = 0;
            var data = await _cityOceanRepository.Context
                .SqlQueryable<NameUniqueValueRepeatDto>(@$"select 
                        NameUniqueValue,
                        count(1) as Quantity,
                        max(CreationTime) LastCreationTime 
                    From co_crm.dbo.customers {whereStr}
                    Group by NameUniqueValue having count(1)>1")
                 .WhereIF(!string.IsNullOrWhiteSpace(queryDto.NameUniqueValue), o => nameUniqueValues.Contains(o.NameUniqueValue))
                 .OrderByIF(string.IsNullOrWhiteSpace(queryDto.OrderField), o => o.LastCreationTime, OrderByType.Desc)
                 .OrderByIF(!string.IsNullOrWhiteSpace(queryDto.OrderField), $"{queryDto.OrderField} {(queryDto.OrderDesc == true ? "desc" : "asc")}")
                 .ToPageListAsync(queryDto.PageIndex, queryDto.PageSize, total);
            result.List = data;
            result.Total = total.Value;
            return result;
        }

        public async Task<PageListDto<NameLocalizationUniqueValueRepeatDto>> GetNameLocalizationUniqueValueRepeatList(CustomerRepeatQueryDto queryDto)
        {
            var nameUniqueValues = new List<string>();
            if (!string.IsNullOrWhiteSpace(queryDto.NameUniqueValue))
            {
                var values = queryDto.NameUniqueValue.Trim().Split(',', ' ');
                foreach (var item in values)
                {
                    nameUniqueValues.Add(GetNameUniqueValue(item));
                }
            }
            var whereList = new List<string>();
            if (queryDto.ExcludeDelete)
            {
                whereList.Add(" IsDeleted = 0 ");
            }
            if (queryDto.ExcludeMerge)
            {
                whereList.Add(" Id = MergerId ");
            }
            var whereStr = whereList.Count > 0 ? ("Where" + string.Join(" and ", whereList)) : string.Empty;
            var result = new PageListDto<NameLocalizationUniqueValueRepeatDto>();
            RefAsync<int> total = 0;
            var data = await _cityOceanRepository.Context
                .SqlQueryable<NameLocalizationUniqueValueRepeatDto>(@$"Select 
                        NameLocalizationUniqueValue,
                        count(1) as Quantity,
                        max(CreationTime) LastCreationTime 
                    From co_crm.dbo.customers {whereStr}
                    Group by NameLocalizationUniqueValue having count(1)>1")
                 .WhereIF(!string.IsNullOrWhiteSpace(queryDto.NameUniqueValue), o => nameUniqueValues.Contains(o.NameLocalizationUniqueValue))
                 .OrderByIF(string.IsNullOrWhiteSpace(queryDto.OrderField), o => o.LastCreationTime, OrderByType.Desc)
                 .OrderByIF(!string.IsNullOrWhiteSpace(queryDto.OrderField), $"{queryDto.OrderField} {(queryDto.OrderDesc == true ? "desc" : "asc")}")
                 .ToPageListAsync(queryDto.PageIndex, queryDto.PageSize, total);
            result.List = data;
            result.Total = total.Value;
            return result;
        }

        /// <summary>
        /// 合并客户并返回excel结果文件
        /// </summary>
        /// <param name="nameUniqueValues"></param>
        /// <returns></returns>
        public async Task<byte[]> MergeCustomersExportExcelAsync(List<string> nameUniqueValues, bool isLocalization = false)
        {
            var results = new List<MergeCustomerResultDto>();
            foreach (var item in nameUniqueValues)
            {
                results.Add(await MergeCustomers(item, isLocalization));
            }

            var customerIds = results.Where(i => i.KeepCustomerId.HasValue).Select(i => i.KeepCustomerId.Value).ToList();

            customerIds.AddRange(results.SelectMany(i => i.CustomerIds));
            List<dynamic> customers = new List<dynamic>();
            if (customerIds.Any())
            {
                customers = _cityOceanRepository.Context.Ado.SqlQuery<dynamic>(@"Select 
                c.Id,
                c.EnName,
                c.ZhName,
                c.Code,
                (Select top 1 u.CName From CO_CRM.dbo.CustomerAccessAllows caa inner join CO_SSO.dbo.Users u on u.Id=caa.AllowUserId where caa.IsOwner=1 and caa.customerId=c.Id) as OwnerUserName 
                From CO_CRM.dbo.Customers c Where c.Id in (@CustomerIds)", new { CustomerIds = customerIds });
            }

            var titleList = new Dictionary<string, Func<MergeCustomerResultDto, object>>();
            titleList.Add("名称唯一值", e => e.NameUniqueValue);
            titleList.Add("合并结果", e => e.IsSuccess ? "成功" : "失败");
            titleList.Add("合并消息", e => e.Message);
            titleList.Add("保留客户信息", e =>
            {
                if (!e.KeepCustomerId.HasValue)
                {
                    return string.Empty;
                }
                var cus = customers.FirstOrDefault(i => i.Id == e.KeepCustomerId.Value);
                if (cus == null)
                {
                    return string.Empty;
                }
                return $"ID：{cus.Id}\r\n代码：{cus.Code}\r\n英文名：{cus.EnName}\r\n中文名：{cus.ZhName}\r\n拥有人：{cus.OwnerUserName}";
            });

            titleList.Add("被合并客户信息", e =>
            {
                if (e.CustomerIds == null || !e.CustomerIds.Any())
                {
                    return string.Empty;
                }
                var cuses = customers.Where(i => e.CustomerIds.Contains(i.Id));
                if (!cuses.Any())
                {
                    return string.Empty;
                }
                List<string> content = new List<string>();
                foreach (var cus in cuses)
                {
                    content.Add($"ID：{cus.Id}\r\n代码：{cus.Code}\r\n英文名：{cus.EnName}\r\n中文名：{cus.ZhName}\r\n拥有人：{cus.OwnerUserName}");
                }
                return string.Join("\r\n\r\n", content);

            });
            return ExcelHelper.ListToExcel(results, titleList, "合并结果");
        }



        public async Task<List<MergeCustomerResultDto>> BatchMergeCustomersAsync(List<string> nameUniqueValues)
        {
            var results = new List<MergeCustomerResultDto>();
            foreach (var item in nameUniqueValues)
            {
                results.Add(await MergeCustomers(item));
            }
            return results;
        }


        /// <summary>
        /// 合并客户
        /// 以客户名称唯一值（去掉特殊符号）进行查找重复客户
        /// 合并时
        /// 重复客户里只有一个有代码的客户时，以有代码客户做为保留客户
        /// 重复客户里有多个有代码的客户时，报错返回
        /// 重复客户里均无代码时，
        /// 以【有邮件】、【有电话】、【有中文地址】、【有英文地址】、【有中文名称】、【有英文名称】五个条件进行循环取更新时间最新的客户
        /// 先取五个条件都有的客户，无就取后四个条件都有的客户，无就取后三个条件都有的客户，以此累推取到保留客户
        /// 当联系人、抬头、地址有重复时，取保留客户的数据
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<MergeCustomerResultDto> MergeCustomers(string nameUniqueValue, bool isLocalization = false)
        {
            var result = new MergeCustomerResultDto()
            {
                NameUniqueValue = nameUniqueValue,
                CustomerIds = new List<Guid>()
            };
            try
            {
                if (string.IsNullOrWhiteSpace(nameUniqueValue))
                {
                    throw new Exception("名称唯一值不能为空");
                }
                var customers = await this._cityOceanRepository.Context.Ado.SqlQueryAsync<dynamic>($@"
                Select c.Id,
                    c.Code,
                    c.Name,
                    c.EnName,
                    c.ZhName,
                    c.Address,
                    c.EnAddress,
                    c.ZhAddress,
                    c.NameUniqueValue,
                    c.NameLocalizationUniqueValue,
                    c.LocalizationText,
                    c.Tel,
                    c.Fax,
                    c.Email,
                    c.ExamineState,
                    c.ExamineTime,
                    c.MergerId,
                    c.IsDeleted,
                    c.CreationTime,
                    c.CustomerType,
                    c.LastModificationTime,
                    isnull((Select Top 1 1 From CO_PUB.dbo.Configures conf Where conf.CustomerId=c.Id),0) as IsCompanyCustomer,
                    (Select top 1 u.CName From CO_CRM.dbo.CustomerAccessAllows caa inner join CO_SSO.dbo.Users u on u.Id=caa.AllowUserId where u.IsValid=1 and u.IsDeleted=0 and caa.IsOwner=1 and caa.customerId=c.Id) as OwnerUserName 
                    From CO_CRM.dbo.Customers c Where c.IsDeleted=0 and c.MergerId = c.Id And c.{(isLocalization ? "NameLocalizationUniqueValue" : "NameUniqueValue")} = @NameUniqueValue", new { NameUniqueValue = nameUniqueValue });
                List<Guid> mergedCustomerIds = customers.Select(i => (Guid)i.Id).ToList();
                result.CustomerIds = mergedCustomerIds;
                if (customers.Any(i => i.IsCompanyCustomer == 1))
                {
                    throw new Exception("涉及到公司客户");
                }
                if (customers.Any(i => i.CustomerType == 1))
                {
                    throw new Exception("涉及到船东类型的客户");
                }
                if (customers.Any(i => i.CustomerType == 3))
                {
                    throw new Exception("涉及到货代（同行）类型的客户");
                }
                if (customers.Select(i => i.CustomerType).Distinct().Count() > 1)
                {
                    throw new Exception("客户类型不一致");
                }
                var hasCodeAmount = customers.Count(i => !string.IsNullOrWhiteSpace(i.Code));

                if (customers.Count == 1)
                {
                    throw new Exception("只有一个客户，无需合并");
                }
                Guid keepCustomerId = Guid.Empty;

                if (hasCodeAmount == 1)
                {
                    var hasCodeCustomer = customers.FirstOrDefault(i => !string.IsNullOrWhiteSpace(i.Code));
                    keepCustomerId = hasCodeCustomer.Id;
                }
                else if (hasCodeAmount > 1)
                {
                    var query = customers.Where(i => !string.IsNullOrWhiteSpace(i.Code) && i.ExamineState == 2);
                    if (!query.Any())
                    {
                        throw new Exception("无审批通过的客户");
                    }
                    var hasOwnerCustomers = query.Where(i => !string.IsNullOrWhiteSpace(i.OwnerUserName)).ToList();

                    if (hasOwnerCustomers.Count == 1)
                    {
                        keepCustomerId = hasOwnerCustomers.FirstOrDefault().Id;
                    }
                    else if (hasOwnerCustomers.Count > 1)
                    {
                        throw new Exception("存在多个客户所有人");
                    }
                    else
                    {
                        var keepCus = query.OrderByDescending(i => i.ExamineTime).FirstOrDefault();
                        if (keepCus == null)
                        {
                            throw new Exception("找不到保留客户");
                        }
                        keepCustomerId = keepCus.Id;
                    }
                }
                else
                {
                    var query = customers.AsEnumerable();
                    if (customers.Any(i => !string.IsNullOrWhiteSpace(i.OwnerUserName)))
                    {
                        query = customers.Where(i => !string.IsNullOrWhiteSpace(i.OwnerUserName));
                    }
                    List<Func<dynamic, bool>> funcs = new List<Func<dynamic, bool>>
                    {
                        i => !string.IsNullOrWhiteSpace(i.Email)  || !string.IsNullOrWhiteSpace(i.Tel),
                        i => !string.IsNullOrWhiteSpace(i.ZhAddress),
                        i => !string.IsNullOrWhiteSpace(i.EnAddress),
                        i => !string.IsNullOrWhiteSpace(i.ZhName),
                        i => !string.IsNullOrWhiteSpace(i.EnName)
                    };
                    while (true)
                    {
                        var tempQuery = query;
                        foreach (var item in funcs)
                        {
                            tempQuery = query.Where(item);
                        }
                        var fullCustomer = tempQuery.OrderByDescending(i => i.LastModificationTime).FirstOrDefault();
                        if (fullCustomer != null)
                        {
                            keepCustomerId = fullCustomer.Id;
                            break;
                        }
                        if (funcs.Count == 0)
                        {
                            break;
                        }
                        funcs.RemoveAt(0);
                    }
                    if (keepCustomerId == null)
                    {
                        throw new Exception("找不到匹配的保留客户");
                    }
                }
                result.KeepCustomerId = keepCustomerId;
                mergedCustomerIds.Remove(keepCustomerId);
                var mergeResult = await _cityOceanApiService.MergeCustomer(new ApiService.CityOcean.Dto.MergeCustomerDto
                {
                    KeepCustomerId = keepCustomerId,
                    CustomerIds = mergedCustomerIds
                });
                if (mergeResult != null)
                {
                    foreach (var item in mergeResult.Data)
                    {
                        var keepCustomerRepeatItem = item.Items.FirstOrDefault(i => i.CustomerId == keepCustomerId);
                        if (keepCustomerRepeatItem != null)
                        {
                            keepCustomerRepeatItem.IsKeep = true;
                        }
                    }
                    mergeResult = await _cityOceanApiService.MergeCustomer(new ApiService.CityOcean.Dto.MergeCustomerDto
                    {
                        KeepCustomerId = keepCustomerId,
                        CustomerIds = mergedCustomerIds,
                        Data = mergeResult.Data
                    });
                    if (mergeResult != null)
                    {
                        throw new Exception("有合并重复项导致合并失败");
                    }
                }
                result.IsSuccess = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;

        }


        public async Task<int> MoveAddressEmail()
        {
            var queryDto = new CustomerQueryDto();
            queryDto.Address = "@";
            queryDto.PageIndex = 1;
            queryDto.PageSize = 200000;
            var total = 0;
            var resultTotal = 0;
            var regex = new Regex(@"[a-zA-Z0-9_\-\.]+[ ]?@[a-zA-Z0-9_-]+(\.[a-zA-Z0-9_-]+)+");

            do
            {
                var result = await this.GetPageListAsync(queryDto);
                total = result.Total;

                foreach (var item in result.List)
                {
                    var matchResults = regex.Matches(item.Address);
                    if (matchResults.Count == 0)
                    {
                        continue;
                    }
                    var address = regex.Replace(item.Address, string.Empty);
                    var localizationText = regex.Replace(item.LocalizationText, string.Empty);
                    List<string> emails = new List<string>();
                    foreach (Match matchResult in matchResults)
                    {
                        var value = matchResult.Value;
                        if (item.Email == null || !item.Email.Contains(value))
                        {
                            emails.Add(value.Trim().Replace(" ", ""));
                        }
                    }
                    var emailString = item.Email;
                    if (emails.Any())
                    {
                        emailString = (!string.IsNullOrWhiteSpace(emailString) ? (emailString + ",") : "") + string.Join(",", emails);

                    }
                    await _cityOceanRepository.Context.Ado.ExecuteCommandAsync($@"Update c Set c.Email = @Email,c.Address=@Address,c.LocalizationText=@LocalizationText FROM CO_CRM.dbo.Customers c Where c.Id=@Id", new
                    {
                        Id = item.Id,
                        Address = address,
                        LocalizationText = localizationText,
                        Email = emailString
                    });
                    resultTotal++;

                }
                queryDto.PageIndex++;
            } while ((queryDto.PageIndex - 1) * queryDto.PageSize < total);
            return resultTotal;
        }

    }
}
