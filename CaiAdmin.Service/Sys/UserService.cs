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
    public class UserService : BaseService<User>
    {
        private readonly AuthContext _authContext;
        private readonly TokenService _tokenService;
        public UserService(Repository<User> repository, IMapper mapper, AuthContext authContext, TokenService tokenService) : base(repository)
        {
            _authContext = authContext;
            _tokenService = tokenService;
        }

        /// <summary>
        /// 构建查询条件
        /// </summary>
        /// <param name="query"></param>
        /// <param name="queryDto"></param>
        private void BuildWhere(ISugarQueryable<User> query, UserQueryDto queryDto)
        {
            query
                .WhereIF(!string.IsNullOrWhiteSpace(queryDto.LoginName), u => u.LoginName.Contains(queryDto.LoginName))
                .WhereIF(!string.IsNullOrWhiteSpace(queryDto.RoleName), u => u.Role.Name.Contains(queryDto.RoleName))
                .WhereIF(queryDto.IsDisabled.HasValue, u => u.IsDisabled == queryDto.IsDisabled.Value);
        }

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        public async Task<PageListDto<UserDto>> GetPageListAsync(UserQueryDto queryDto)
        {
            var result = new PageListDto<UserDto>();
            var query = this.Repository.AsQueryable();
            this.BuildWhere(query, queryDto);
            result.Total = await query.Clone().CountAsync();
            result.List = await query.BuildByQuery(queryDto, i => i.UpdateTime, OrderByType.Desc).Select(u => new UserDto
            {
                Id = u.Id,
                CreateTime = u.CreateTime,
                IsDisabled = u.IsDisabled,
                LoginName = u.LoginName,
                RoleId = u.RoleId,
                RoleName = u.Role.Name,
                UpdateTime = u.UpdateTime
            }).ToListAsync();
            return result;
        }

        public async Task<int> SaveAsync(UserSaveDto dto)
        {
            if (await this.Repository.IsAnyAsync(i => i.LoginName == dto.LoginName && i.Id != dto.Id))
            {
                throw new ApiFailException(ApiFailCode.OPERATION_FAIL, "登录名已存在");
            }
            if (dto.Id == default)
            {
                var user = new User
                {
                    IsDisabled = dto.IsDisabled,
                    LoginName = dto.LoginName,
                    LoginPassword = dto.LoginPassword,
                    RoleId = dto.RoleId,
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now
                };
                dto.Id = await this.Repository.InsertReturnIdentityAsync(user);
            }
            else
            {
                if (dto.Id < 0)
                {
                    throw new ApiFailException(ApiFailCode.OPERATION_FAIL, "系统内置用户禁止修改");
                }
                var user = await this.Repository.GetByIdAsync(dto.Id);
                if (user == null)
                {
                    throw new ApiFailException(ApiFailCode.OPERATION_FAIL, "用户不存在");
                }
                user.LoginName = dto.LoginName;
                user.IsDisabled = dto.IsDisabled;
                user.RoleId = dto.RoleId;
                user.LoginPassword = dto.LoginPassword;
                user.UpdateTime = DateTime.Now;
                var updateable = this.Repository.AsUpdateable(user);
                if (!dto.ChangePassword)
                {
                    updateable.IgnoreColumns(i => i.LoginPassword);
                }
                await updateable.ExecuteCommandAsync();
                //禁用用户或修改密码的话，就得把之前登录的Token都禁用掉
                if (dto.IsDisabled || dto.ChangePassword)
                {
                    await _tokenService.DisableTokenByUserIdAsync(dto.Id);
                }
            }
            return dto.Id;
        }

        public async Task DeleteByIdAsync(int id)
        {
            if (id < 0)
            {
                throw new ApiFailException(ApiFailCode.OPERATION_FAIL, "系统内置用户禁止删除");
            }
            await this.Repository.DeleteByIdAsync(id);
        }

        /// <summary>
        /// 检查是否存在重复的登录名
        /// </summary>
        /// <param name="path"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> CheckExistByLoginNameAsync(string loginName, int id = 0)
        {
            return await this.Repository.IsAnyAsync(i => i.LoginName == loginName && i.Id != id);
        }
    }
}
