using AutoMapper;
using CaiAdmin.Common;
using CaiAdmin.Database;
using CaiAdmin.Service;
using CaiAdmin.Service.Sys;
using CaiAdmin.Entity.Sys;
using CaiAdmin.Service.Interceptors;
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
    public class ApiService : BaseService<Api>
    {
        private readonly Repository<MenuApi> _menuApiRepository;
        private readonly Repository<MenuPermissionApi> _menuPermissionApiRepository;

        public ApiService(Repository<Api> repository, Repository<MenuApi> menuApiRepository, Repository<MenuPermissionApi> menuPermissionApiRepository) : base(repository)
        {
            _menuApiRepository = menuApiRepository;
            _menuPermissionApiRepository = menuPermissionApiRepository;
        }

        /// <summary>
        /// 构建查询条件
        /// </summary>
        /// <param name="query"></param>
        /// <param name="queryDto"></param>
        private void BuildWhere(ISugarQueryable<Api> query, ApiQueryDto queryDto)
        {
            if (queryDto.NotIds != null && queryDto.NotIds.Length > 0)
            {
                query.Where(e => !queryDto.NotIds.Contains(e.Id));
            }
            if (!string.IsNullOrWhiteSpace(queryDto.Name))
            {
                query.Where(e => e.Name.Contains(queryDto.Name));
            }
            if (!string.IsNullOrWhiteSpace(queryDto.Path))
            {
                query.Where(e => e.Path.Contains(queryDto.Path));
            }
        }

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        public async Task<PageListDto<ApiDto>> GetPageListAsync(ApiQueryDto queryDto)
        {
            var result = new PageListDto<ApiDto>();
            var query = this.Repository.AsQueryable();
            this.BuildWhere(query, queryDto);
            result.Total = await query.Clone().CountAsync();
            result.List = await query.Select(a => new ApiDto
            {
                Id = a.Id,
                //IsCommon = a.IsCommon,
                Name = a.Name,
                Path = a.Path,
                //PermissionType = a.PermissionType.ToString()
            }).BuildByQuery(queryDto).ToListAsync();
            return result;
        }

        public async Task<int> SaveAsync(ApiDto dto)
        {
            //创建
            if (dto.Id == default)
            {
                var api = new Api
                {
                    //IsCommon = dto.IsCommon,
                    Name = dto.Name,
                    Path = dto.Path
                };
                dto.Id = await this.Repository.InsertReturnIdentityAsync(api);
            }
            //更新
            else
            {
                var role = await this.Repository.GetByIdAsync(dto.Id);
                if (role == null)
                {
                    throw new ApiFailException(ApiFailCode.OPERATION_FAIL, "接口不存在");
                }
                role.Name = dto.Name;
                role.Path = dto.Path;
                //role.IsCommon = dto.IsCommon;
                //role.PermissionType = Enum.Parse<ApiPermissionType>(dto.PermissionType);
                var updateable = this.Repository.AsUpdateable(role);
                await updateable.ExecuteCommandAsync();
            }
            return dto.Id;
        }
        /// <summary>
        /// 根据ID删除接口
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [TransactionInterceptor]
        public async Task DeleteByIdAsync(int id)
        {
            await this._menuApiRepository.DeleteAsync(i => i.ApiId == id);
            await this._menuPermissionApiRepository.DeleteAsync(i => i.ApiId == id);
            await this.Repository.DeleteByIdAsync(id);

        }

        /// <summary>
        /// 检查是否存在重复名称
        /// </summary>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> CheckExistByNameAsync(string name, int id = 0)
        {
            return await this.Repository.IsAnyAsync(e => e.Name == name && e.Id != id);
        }
        /// <summary>
        /// 检查是否存在重复路径
        /// </summary>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> CheckExistByPathAsync(string path, int id = 0)
        {
            return await this.Repository.IsAnyAsync(e => e.Path == path && e.Id != id);
        }
    }
}
