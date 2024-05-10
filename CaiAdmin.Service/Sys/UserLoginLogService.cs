using AutoMapper;
using CaiAdmin.Common;
using CaiAdmin.Database;
using CaiAdmin.Service;
using CaiAdmin.Service.Sys;
using CaiAdmin.Entity.Sys;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using CaiAdmin.Dto.Sys;
using CaiAdmin.Dto;

namespace CaiAdmin.Service.Sys
{
    public class UserLoginLogService : BaseService<UserLoginLog>
    {
        private readonly AuthContext _authContext;
        public UserLoginLogService(Repository<UserLoginLog> repository, IMapper mapper, AuthContext authContext) : base(repository)
        {
            _authContext = authContext;
        }
        private void BuildWhere(ISugarQueryable<UserLoginLog> query, UserLoginLogQueryDto queryDto)
        {
            if (queryDto.IsLoginUser.HasValue && queryDto.IsLoginUser.Value)
            {
                query.Where(e => e.UserId == _authContext.UserId);
            }
        }

        public async Task<PageListDto<UserLoginLogDto>> GetPageListAsync(UserLoginLogQueryDto queryDto)
        {
            var result = new PageListDto<UserLoginLogDto>();
            var query = this.Repository.AsQueryable();
            this.BuildWhere(query, queryDto);
            result.Total = await query.Clone().CountAsync();
            result.List = await query.Select(ul => new UserLoginLogDto
            {
                Id = ul.Id,
                CreateTime = ul.CreateTime,
                IpAddress = ul.IpAddress,
                UserId = ul.UserId,
                UserLoginName = ul.User.LoginName
            }).BuildByQuery(queryDto, i => i.CreateTime, OrderByType.Desc).ToListAsync();
            return result;
        }

        public async Task CreateAsync()
        {
            var userLoginLog = new UserLoginLog
            {
                CreateTime = DateTime.Now,
                IpAddress = _authContext.UserIP.ToString(),
                UserId = _authContext.UserId
            };
            await this.Repository.InsertAsync(userLoginLog);
        }
    }
}
