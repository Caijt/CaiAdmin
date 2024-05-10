using AutoMapper;
using ItSys.Dto;
using ItSys.Entity;
using CaiAdmin.Database;
using CaiAdmin.Dto;
using CaiAdmin.Service;
using Microsoft.EntityFrameworkCore;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItSys.Service
{
    public class ItAccountService : BaseService<ItAccount>
    {
        public ItAccountService(Repository<ItAccount> repository, IMapper mapper, AuthContext authContext)
            : base(repository)
        {
            //onInclude = query => query.Include(e => e.Company);
            //onWhere = (query, queryParams) =>
            //{
            //    if (!string.IsNullOrWhiteSpace(queryParams.name))
            //    {
            //        query = query.Where(e => e.name.Contains(queryParams.name));
            //    }
            //    if (!string.IsNullOrWhiteSpace(queryParams.login_address))
            //    {
            //        query = query.Where(e => e.login_address.Contains(queryParams.login_address));
            //    }
            //    if (!string.IsNullOrWhiteSpace(queryParams.account))
            //    {
            //        query = query.Where(e => e.account.Contains(queryParams.account));
            //    }
            //    if (!string.IsNullOrWhiteSpace(queryParams.remarks))
            //    {
            //        query = query.Where(e => e.remarks.Contains(queryParams.remarks));
            //    }
            //    return query;
            //};
            //orderProp = prop =>
            //{
            //    return null;
            //};
        }

        private ISugarQueryable<ItAccount> BuildWhere(ISugarQueryable<ItAccount> query, ItAccountQueryDto queryDto) {
            if (!string.IsNullOrWhiteSpace(queryDto.name))
            {
                query = query.Where(e => e.name.Contains(queryDto.name));
            }
            if (!string.IsNullOrWhiteSpace(queryDto.login_address))
            {
                query = query.Where(e => e.login_address.Contains(queryDto.login_address));
            }
            if (!string.IsNullOrWhiteSpace(queryDto.account))
            {
                query = query.Where(e => e.account.Contains(queryDto.account));
            }
            if (!string.IsNullOrWhiteSpace(queryDto.remarks))
            {
                query = query.Where(e => e.remarks.Contains(queryDto.remarks));
            }
            return query;
        }

        public async Task<PageListDto<ItAccountDto>> GetPageListAsync(ItAccountQueryDto queryDto)
        {
            var query = this.Repository.AsQueryable();
            query = BuildWhere(query, queryDto);
            var result = new PageListDto<ItAccountDto>();
            result.Total = await query.Clone().CountAsync();            
            result.List = await query.BuildPageByQuery(queryDto).Select(e => new ItAccountDto
            {
                 id  = e.Id,
                account = e.account,
                attach_guid = e.attach_guid,
                company_id = e.company_id,
                company_name = e.Company.Name,
                create_time = e.create_time,
                create_user_name = e.CreateUser.LoginName,
                login_address = e.login_address,
                name = e.name,
                password = e.password,
                remarks = e.remarks,
                update_time = e.update_time
            }).BuildOrderByQuery(queryDto).ToListAsync();
            return result;
        }
    }
}
