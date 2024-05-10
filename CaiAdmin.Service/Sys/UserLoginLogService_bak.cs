using AutoMapper;
using CaiAdmin.Common;
using CaiAdmin.Database;
using CaiAdmin.Service.Sys;
using CaiAdmin.Entity.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using CaiAdmin.Dto.Sys;

namespace CaiAdmin.Service.Sys
{
    public class UserLoginLogService_bak : EntityService<UserLoginLogDto, UserLoginLogDto, UserLoginLogDto, UserLoginLogQueryDto, UserLoginLog, int>
    {
        private readonly AuthContext _authContext;
        public UserLoginLogService_bak(AppDbContext appDbContext, IMapper mapper, AuthContext authContext, IEnumerable<IEntityHandler<UserLoginLog>> entityHandlers, IEnumerable<IQueryFilter<UserLoginLog>> queryFilters) : base(appDbContext, mapper, entityHandlers, queryFilters)
        {
            _authContext = authContext;
        }
        protected override IQueryable<UserLoginLog> OnFilter(IQueryable<UserLoginLog> query, UserLoginLogQueryDto queryDto)
        {
            query = base.OnFilter(query, queryDto);
            if (queryDto.IsLoginUser.HasValue && queryDto.IsLoginUser.Value)
            {
                query = query.Where(e => e.UserId == _authContext.UserId);
            }
            return query;
        }

        protected override void OnCreating(UserLoginLog entity, UserLoginLogDto createDto)
        {
            entity.IpAddress = _authContext.UserIP.ToString();
        }
    }
}
