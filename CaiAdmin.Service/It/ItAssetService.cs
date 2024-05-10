using AutoMapper;
using ItSys.Dto;
using ItSys.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using System.IO;
using CaiAdmin.Dto;
using System.Threading.Tasks;
using CaiAdmin.Service;
using CaiAdmin.Database;
using SqlSugar;
using CaiAdmin.Common;
using CaiAdmin.Entity.It;

namespace ItSys.Service
{
    public class ItAssetService : BaseService<Asset>
    {
        public ItAssetService(IMapper mapper, AuthContext authContext,Repository<Asset> repository)
            : base(repository)
        {
            //onBeforeCreate = (entity, dto) =>
            //{
            //    string noPrefix = "IT" + DateTime.Now.ToString("yyMM");
            //    var dateMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            //    int i = 1;
            //    string maxNo = dbSet.Where(e => e.CreateTime >= dateMonth).Max(e => e.no);
            //    if (!string.IsNullOrWhiteSpace(maxNo))
            //    {
            //        string n = maxNo.Substring(6);
            //        if (int.TryParse(n, out int a))
            //        {
            //            i = a + 1;
            //        }
            //    }
            //    entity.no = noPrefix + i.ToString().PadLeft(4, '0');
            //};
            //onBeforeUpdate = (entity, dto, props) =>
            //{
            //    props.Add(e => e.no);
            //    props.Add(e => e.attach_guid);
            //    props.Add(e => e.scrap_amount);
            //    props.Add(e => e.is_repair);
            //    props.Add(e => e.used);
            //};
            //onWhere = (query, queryParams) =>
            //{
            //    #region 资产编号
            //    if (!string.IsNullOrWhiteSpace(queryParams.no))
            //    {
            //        query = query.Where(e => e.no.Contains(queryParams.no));
            //    }
            //    #endregion
            //    #region 资产型号
            //    if (!string.IsNullOrWhiteSpace(queryParams.model))
            //    {
            //        query = query.Where(e => e.no.Contains(queryParams.model));
            //    }
            //    #endregion
            //    #region 资产状态
            //    if (queryParams.abnormal_status != null && queryParams.abnormal_status.Length > 0)
            //    {
            //        query = query.Where(e =>
            //        queryParams.abnormal_status.Contains("FREE") && (e.amount - e.scrap_amount - e.used) > 0
            //        ||
            //        queryParams.abnormal_status.Contains("REPAIR") && e.is_repair == true
            //        ||
            //        queryParams.abnormal_status.Contains("SCRAP") && e.scrap_amount > 0
            //        );
            //    }
            //    #endregion
            //    #region 标识号
            //    if (!string.IsNullOrWhiteSpace(queryParams.diy_no))
            //    {
            //        query = query.Where(e => e.diy_no.Contains(queryParams.diy_no));
            //    }
            //    #endregion
            //    #region 资产类型
            //    if (queryParams.type_id.HasValue)
            //    {
            //        if (queryParams.hasSubDep)
            //        {
            //            query = query.Where(e => (e.Type.parent_ids + ",").Contains(queryParams.type_id.ToString()) || e.type_id == queryParams.type_id);
            //        }
            //        else
            //        {
            //            query = query.Where(e => e.type_id == queryParams.type_id);
            //        }

            //    }
            //    #endregion
            //    #region 资产备注
            //    if (!string.IsNullOrWhiteSpace(queryParams.remarks))
            //    {
            //        query = query.Where(e => e.remarks.Contains(queryParams.remarks));
            //    }
            //    #endregion
            //    #region 来源方式
            //    if (!string.IsNullOrWhiteSpace(queryParams.source))
            //    {
            //        query = query.Where(e => e.source.Contains(queryParams.source));
            //    }
            //    #endregion
            //    #region 序列号
            //    if (!string.IsNullOrWhiteSpace(queryParams.sn))
            //    {
            //        query = query.Where(e => e.sn.Contains(queryParams.sn));
            //    }
            //    #endregion
            //    #region 供应商
            //    if (!string.IsNullOrWhiteSpace(queryParams.supplier_name))
            //    {
            //        query = query.Where(e => e.Supplier.name.Contains(queryParams.supplier_name));
            //    }
            //    #endregion
            //    #region 所属公司
            //    if (queryParams.company_ids != null && queryParams.company_ids.Length > 0)
            //    {
            //        query = query.Where(e => queryParams.company_ids.Contains(e.company_id));
            //    }
            //    #endregion
            //    #region 领用部门
            //    if (queryParams.dep_id.HasValue)
            //    {
            //        query = query.Where(e => e.use_dep_id == queryParams.dep_id);
            //    }
            //    #endregion
            //    #region 领用员工
            //    if (!string.IsNullOrWhiteSpace(queryParams.employee_name))
            //    {
            //        query = query.Where(e => e.UseEmployee.name.Contains(queryParams.employee_name));
            //    }
            //    #endregion
            //    #region 入库日期
            //    if (queryParams.inbound_date_begin.HasValue)
            //    {
            //        query = query.Where(e => e.inbound_date >= queryParams.inbound_date_begin);
            //    }
            //    if (queryParams.inbound_date_end.HasValue)
            //    {
            //        query = query.Where(e => e.inbound_date < queryParams.inbound_date_end.Value.AddDays(1));
            //    }
            //    #endregion
            //    #region 资产金额
            //    if (queryParams.price_begin.HasValue)
            //    {
            //        query = query.Where(e => e.price >= queryParams.price_begin);
            //    }
            //    if (queryParams.price_end.HasValue)
            //    {
            //        query = query.Where(e => e.price <= queryParams.price_end);
            //    }
            //    #endregion

            //    if (queryParams.stock_warning_id.HasValue)
            //    {
            //        query = query.Where(e => e.stock_warning_id == queryParams.stock_warning_id);
            //    }
            //    if (queryParams.not_ids != null && queryParams.not_ids.Length > 0)
            //    {
            //        query = query.Where(e => !queryParams.not_ids.Contains(e.Id));
            //    }
            //    if (queryParams.ids != null && queryParams.ids.Length > 0)
            //    {
            //        query = query.Where(e => queryParams.ids.Contains(e.Id));
            //    }
            //    #region 近10天资产
            //    if (queryParams.isNear)
            //    {
            //        query = query.Where(e => e.CreateTime >= DateTime.Now.AddDays(-10));
            //    }
            //    #endregion
            //    return query;
            //};
            //orderProp = prop =>
            //{
            //    switch (prop)
            //    {
            //        case "no":
            //            return e => e.no;
            //        case "inbound_date":
            //            return e => e.inbound_date;
            //        case "remain":
            //            return e => e.amount - e.scrap_amount - e.used;
            //        case "use_dep_employee":
            //            return e => e.use_dep_id;
            //        case "stock_warning_name":
            //            return e => e.stock_warning_id;
            //        case "price":
            //            return e => e.price;
            //    }
            //    return null;
            //};
            //selectExpression = e => new ItAssetDto()
            //{
            //    amount = e.amount,
            //    attach_guid = e.attach_guid,
            //    avaiable_amount = e.amount - e.scrap_amount,
            //    remain = e.amount - e.scrap_amount - e.used,
            //    stock_warning_id = e.stock_warning_id,
            //    use_dep_id = e.use_dep_id,
            //    use_dep_name = e.UseDep.name,
            //    use_employee_id = e.use_employee_id,
            //    use_employee_name = e.UseEmployee.name,
            //    source = e.source,
            //    sn = e.sn,
            //    remarks = e.remarks,
            //    price = e.price,
            //    model = e.model,
            //    is_repair = e.is_repair,
            //    inbound_date = e.inbound_date,
            //    no = e.no,
            //    id = e.Id,
            //    diy_no = e.diy_no,
            //    company_id = e.company_id,
            //    company_name = e.Company.Name,
            //    create_time = e.CreateTime,
            //    update_time = e.UpdateTime,
            //    create_user_name = e.CreateUser.Name,
            //    supplier_id = e.supplier_id,
            //    supplier_name = e.Supplier.name,
            //    scrap_amount = e.scrap_amount,
            //    stock_warning_name = e.StockWarning.name,
            //    type_id = e.type_id,
            //    type_name = e.Type.name,
            //    used = e.used,
            //    input_status = e.input_status
            //};
            //summaryExpression = g => new
            //{
            //    remain = g.Sum(e => e.amount) - g.Sum(e => e.scrap_amount) - g.Sum(e => e.used),
            //    price = g.Sum(e => e.price)
            //};
        }

        private ISugarQueryable<Asset> BuildWhere(ISugarQueryable<Asset> query, ItAssetQueryDto queryDto) {

            #region 资产编号
            if (!string.IsNullOrWhiteSpace(queryDto.no))
            {
                query = query.Where(e => e.no.Contains(queryDto.no));
            }
            #endregion
            #region 资产型号
            if (!string.IsNullOrWhiteSpace(queryDto.model))
            {
                query = query.Where(e => e.no.Contains(queryDto.model));
            }
            #endregion
            #region 资产状态
            if (queryDto.abnormal_status != null && queryDto.abnormal_status.Length > 0)
            {
                query = query.Where(e =>
                queryDto.abnormal_status.Contains("FREE") && (e.amount - e.scrap_amount - e.used) > 0
                ||
                queryDto.abnormal_status.Contains("REPAIR") && e.is_repair == true
                ||
                queryDto.abnormal_status.Contains("SCRAP") && e.scrap_amount > 0
                );
            }
            #endregion
            #region 标识号
            if (!string.IsNullOrWhiteSpace(queryDto.diy_no))
            {
                query = query.Where(e => e.diy_no.Contains(queryDto.diy_no));
            }
            #endregion
            #region 资产类型
            if (queryDto.type_id.HasValue)
            {
                //if (queryDto.hasSubDep)
                //{
                //    query = query.Where(e => (e.Type.parent_ids + ",").Contains(queryDto.type_id.ToString()) || e.type_id == queryDto.type_id);
                //}
                //else
                //{
                query = query.Where(e => e.type_id == queryDto.type_id);
                //}

            }
            #endregion
            #region 资产备注
            if (!string.IsNullOrWhiteSpace(queryDto.remarks))
            {
                query = query.Where(e => e.remarks.Contains(queryDto.remarks));
            }
            #endregion
            #region 来源方式
            if (!string.IsNullOrWhiteSpace(queryDto.source))
            {
                query = query.Where(e => e.source.Contains(queryDto.source));
            }
            #endregion
            #region 序列号
            if (!string.IsNullOrWhiteSpace(queryDto.sn))
            {
                query = query.Where(e => e.sn.Contains(queryDto.sn));
            }
            #endregion
            #region 供应商
            if (!string.IsNullOrWhiteSpace(queryDto.supplier_name))
            {
                query = query.Where(e => e.Supplier.name.Contains(queryDto.supplier_name));
            }
            #endregion
            #region 所属公司
            if (queryDto.company_ids != null && queryDto.company_ids.Length > 0)
            {
                query = query.Where(e => queryDto.company_ids.Contains(e.company_id));
            }
            #endregion
            #region 领用部门
            if (queryDto.dep_id.HasValue)
            {
                query = query.Where(e => e.use_dep_id == queryDto.dep_id);
            }
            #endregion
            #region 领用员工
            if (!string.IsNullOrWhiteSpace(queryDto.employee_name))
            {
                query = query.Where(e => e.UseEmployee.name.Contains(queryDto.employee_name));
            }
            #endregion
            #region 入库日期
            if (queryDto.inbound_date_begin.HasValue)
            {
                query = query.Where(e => e.inbound_date >= queryDto.inbound_date_begin);
            }
            if (queryDto.inbound_date_end.HasValue)
            {
                query = query.Where(e => e.inbound_date < queryDto.inbound_date_end.Value.AddDays(1));
            }
            #endregion
            #region 资产金额
            if (queryDto.price_begin.HasValue)
            {
                query = query.Where(e => e.price >= queryDto.price_begin);
            }
            if (queryDto.price_end.HasValue)
            {
                query = query.Where(e => e.price <= queryDto.price_end);
            }
            #endregion
            if (queryDto.stock_warning_id.HasValue)
            {
                query = query.Where(e => e.stock_warning_id == queryDto.stock_warning_id);
            }
            if (queryDto.not_ids != null && queryDto.not_ids.Length > 0)
            {
                query = query.Where(e => !queryDto.not_ids.Contains(e.Id));
            }
            if (queryDto.ids != null && queryDto.ids.Length > 0)
            {
                query = query.Where(e => queryDto.ids.Contains(e.Id));
            }
            #region 近10天资产
            if (queryDto.isNear)
            {
                query = query.Where(e => e.create_time >= DateTime.Now.AddDays(-10));
            }
            #endregion
            return query;
        }

        public async Task<PageListDto<ItAssetDto>> GetPageListAsync(ItAssetQueryDto queryDto) 
        {
            var query = this.Repository.AsQueryable();
            query = BuildWhere(query, queryDto);
            var result = new PageListDto<ItAssetDto>();
            result.Total =await query.Clone().CountAsync();
            result.List = await query.BuildPageByQuery(queryDto).Select(e => new ItAssetDto
            {
                amount = e.amount,
                attach_guid = e.attach_guid,
                avaiable_amount = e.amount - e.scrap_amount,
                remain = e.amount - e.scrap_amount - e.used,
                stock_warning_id = e.stock_warning_id,
                use_dep_id = e.use_dep_id,
                use_dep_name = e.UseDep.name,
                use_employee_id = e.use_employee_id,
                use_employee_name = e.UseEmployee.name,
                source = e.source,
                sn = e.sn,
                remarks = e.remarks,
                price = e.price,
                model = e.model,
                is_repair = e.is_repair,
                inbound_date = e.inbound_date,
                no = e.no,
                id = e.Id,
                diy_no = e.diy_no,
                company_id = e.company_id,
                company_name = e.Company.Name,
                create_time = e.create_time,
                update_time = e.update_time,
                create_user_name = e.CreateUser.LoginName,
                supplier_id = e.supplier_id,
                supplier_name = e.Supplier.name,
                scrap_amount = e.scrap_amount,
                stock_warning_name = e.StockWarning.name,
                type_id = e.type_id,
                type_name = e.Type.name,
                used = e.used,
                input_status = e.input_status
            }).BuildOrderByQuery(queryDto).ToListAsync();           
            return result;
        }

        public List<Dictionary<string, object>> GetTimeStatistic(TimeStatisticQueryDto timeStatisticQueryDto, ItAssetQueryDto queryDto)
        {
            Expression<Func<Asset, dynamic>> groupExp = null;
            Func<DateTime, int, DateTime> dateFunc = null;
            Func<DateTime, DateTime> beginDateFunc = null;
            Func<DateTime, DateTime> endDateFunc = null;
            string format = "";
            switch (timeStatisticQueryDto.unit)
            {
                case "year":
                    format = "yyyy";
                    dateFunc = (datetime, i) => datetime.Date.AddYears(i);
                    beginDateFunc = datetime => new DateTime(datetime.Year, 1, 1);
                    endDateFunc = datetime => new DateTime(datetime.Year + 1, 1, 1);
                    groupExp = e => new
                    {
                        unit = e.inbound_date.Year.ToString()
                    };
                    break;
                case "day":
                    format = "yyyy-M-d";
                    dateFunc = (datetime, i) => datetime.AddDays(i);
                    beginDateFunc = datetime => datetime.Date;
                    endDateFunc = datetime => datetime.AddDays(1).Date;
                    groupExp = e => new
                    {
                        unit = e.inbound_date.Year.ToString() + "-" + e.inbound_date.Month + "-" + e.inbound_date.Day
                    };
                    break;
                case "month":
                default:
                    format = "yyyy-M";
                    dateFunc = (datetime, i) => datetime.AddMonths(i);
                    beginDateFunc = datetime => new DateTime(datetime.Year, datetime.Month, 1);
                    endDateFunc = datetime =>
                    {
                        datetime = datetime.AddMonths(1);
                        return new DateTime(datetime.Year, datetime.Month, 1);
                    };
                    groupExp = e => new
                    {
                        unit = e.inbound_date.Year.ToString() + "-" + e.inbound_date.Month.ToString()
                    };

                    break;
            }
            if (!timeStatisticQueryDto.time_end.HasValue)
            {
                timeStatisticQueryDto.time_end = DateTime.Today;
            }
            if (!timeStatisticQueryDto.time_begin.HasValue)
            {
                timeStatisticQueryDto.time_begin = dateFunc(timeStatisticQueryDto.time_end.Value, -8);
            }
            DateTime beginTime = beginDateFunc(timeStatisticQueryDto.time_begin.Value);
            DateTime endTime = endDateFunc(timeStatisticQueryDto.time_end.Value);
            List<Dictionary<string, object>> unitDataList = new List<Dictionary<string, object>>();
            DateTime timeTemp = beginTime;
            while (timeTemp < endTime)
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict.Add("unit", timeTemp.ToString(format));
                dict.Add("price", 0);
                dict.Add("amount", 0);
                unitDataList.Add(dict);
                timeTemp = dateFunc(timeTemp, 1);
            }
            var query = this.Repository.AsQueryable();
            query = BuildWhere(query, queryDto);

            var data = query.Where(e => e.inbound_date >= beginTime && e.inbound_date < endTime)
                .GroupBy(groupExp)
                .Select(g => new
                {
                    Key = g.inbound_date,
                    price = SqlFunc.AggregateSum(g.price),
                    amount = SqlFunc.AggregateSum(g.amount)
                }).ToList();
            unitDataList.ForEach(i =>
            {
                data.ForEach(i2 =>
                {
                    if (string.Equals(i["unit"], i2.Key))
                    {
                        i["price"] = i2.price;
                        i["amount"] = i2.amount;
                    }
                });
            });
            return unitDataList;
        }


        public int ImportExcel(Stream stream)
        {
            string[] keys = new string[] {
                "company_name",
                "model",
                "type_name",
                "diy_no",
                "inbound_date",
                "amount",
                "supplier_name","source","price","sn","remarks"
            };
            var excelList = ExcelHelper.ToList(stream, 3, keys);
            int successCount = 0;
            foreach (var item in excelList)
                    {
                        string companyName = item["company_name"].ToString();
                        int companyId = this.Repository.Context.Queryable<SysCompany>()
                            .Where(e => e.Name == companyName)
                            .Select(e => e.Id)
                            .First();
                        if (companyId == 0)
                        {

                            //throw new ResultException(
                            //    ResultDto.Error(
                            //        "第" + (excelList.IndexOf(item) + 3) + "行数据资产所属公司名称在系统不存在！"));
                        }
                        string typeName = item["type_name"].ToString();
                        int typeId = this.Repository.Context.Queryable<ItAssetType>().Where(e => e.name == typeName)
                            .Select(e => e.Id)
                            .First();
                        if (typeId == 0)
                        {
                            //throw new ResultException(
                            //    ResultDto.Error(
                            //        "第" + (excelList.IndexOf(item) + 3) + "行数据资产类型名称在系统不存在！"));
                        }
                        string supplierName = item["supplier_name"].ToString();
                        int? supplierId = this.Repository.Context.Queryable<Supplier>().Where(e => e.name == supplierName)
                            .Select(e => e.Id)
                            .First();
                        if (supplierId == 0)
                        {
                            supplierId = null;
                        }
                        ItAssetSaveDto dto = new ItAssetSaveDto()
                        {
                            company_id = companyId,
                            type_id = typeId,
                            supplier_id = supplierId,
                            amount = Convert.ToInt32(item["amount"]),
                            attach_guid = Guid.NewGuid().ToString(),
                            diy_no = item["diy_no"].ToString(),
                            inbound_date = Convert.ToDateTime(item["inbound_date"]),
                            model = item["model"].ToString(),
                            price = Convert.ToDecimal(item["price"]),
                            remarks = item["remarks"].ToString(),
                            sn = item["sn"].ToString(),
                            source = item["source"].ToString()
                        };
                        //Create(dto);
                        successCount++;
                    }
            return successCount;
        }


        public async Task<byte[]> ExportExcel(ItAssetQueryDto dto)
        {
            var titleList = new Dictionary<string, Func<ItAssetDto, object>>();
            titleList.Add("资产编号", e => e.no);
            titleList.Add("资产型号", e => e.model);
            titleList.Add("资产类型", e => e.type_name);
            titleList.Add("标识号", e => e.diy_no);
            titleList.Add("入库日期", e => e.inbound_date.ToShortDateString());
            titleList.Add("入库量", e => e.amount);
            titleList.Add("可用量", e => e.avaiable_amount);
            titleList.Add("库存量", e => e.remain);
            titleList.Add("供应商", e => e.supplier_name);
            titleList.Add("金额", e => e.price);
            titleList.Add("来源方式", e => e.source);
            titleList.Add("库存种类", e => e.stock_warning_name);
            titleList.Add("序列号", e => e.sn);
            titleList.Add("备注", e => e.remarks);
            titleList.Add("资产所属公司", e => e.company_name);
            titleList.Add("录入员", e => e.create_user_name);
            titleList.Add("创建时间", e => e.create_time.ToString());
            titleList.Add("更新时间", e => e.update_time.ToString());

            var result = await this.GetPageListAsync(dto);

            return ExcelHelper.ListToExcel(result.List, titleList);
        }
    }
}
