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
using CaiAdmin.Dto;
using CaiAdmin.Dto.Sys;

namespace CaiAdmin.Service.Sys
{
    public class RoleService : BaseService<Role>
    {
        private readonly AuthContext _authContext;
        private readonly Repository<RoleMenu> _roleMenuRepository;
        private readonly Repository<Menu> _menuRepository;
        private readonly Repository<MenuPermission> _menuPermissionRepository;
        private readonly Repository<RoleMenuPermission> _roleMenuPermissionRepository;
        private readonly MenuService _menuService;
        private readonly IMapper _mapper;

        public RoleService(
            Repository<Role> repository,
            Repository<RoleMenu> roleMenuRepository,
            Repository<Menu> menuRepository,
            IMapper mapper,
            Repository<MenuPermission> menuPermissionRepository,
             Repository<RoleMenuPermission> roleMenuPermissionRepository,
            AuthContext authContext,
            MenuService menuService
            ) : base(repository)
        {
            _authContext = authContext;
            _roleMenuRepository = roleMenuRepository;
            _menuRepository = menuRepository;
            _menuPermissionRepository = menuPermissionRepository;
            _roleMenuPermissionRepository = roleMenuPermissionRepository;
            _menuService = menuService;
            _mapper = mapper;
        }

        /// <summary>
        /// 构建查询条件
        /// </summary>
        /// <param name="query"></param>
        /// <param name="queryDto"></param>
        private void BuildWhere(ISugarQueryable<Role> query, RoleQueryDto queryDto)
        {
            query
                .WhereIF(!string.IsNullOrWhiteSpace(queryDto.Name), r => r.Name.Contains(queryDto.Name));
        }

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        public async Task<PageListDto<RoleDto>> GetPageListAsync(RoleQueryDto queryDto)
        {
            var result = new PageListDto<RoleDto>();
            var query = this.Repository.AsQueryable();
            this.BuildWhere(query, queryDto);
            result.Total = await query.Clone().CountAsync();
            result.List = await query.Select(r => new RoleDto
            {
                Id = r.Id,
                Name = r.Name,
                Remarks = r.Remarks,
                CreateTime = r.CreateTime,
                UpdateTime = r.UpdateTime
            }).BuildByQuery(queryDto, i => i.UpdateTime, OrderByType.Desc).ToListAsync();
            return result;
        }
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<RoleSaveDto> GetEditById(int id)
        {
            var role = await this.Repository.GetByIdAsync(id);
            if (role == null)
            {
                throw new ApiFailException(ApiFailCode.OPERATION_FAIL, "角色不存在");
            }
            var menus = await _roleMenuRepository.AsQueryable().Where(i => i.RoleId == id).ToListAsync();
            var menuPermissions = await _roleMenuPermissionRepository.AsQueryable().Where(i => i.RoleId == id).ToListAsync();
            return new RoleSaveDto
            {
                Id = role.Id,
                Name = role.Name,
                Remarks = role.Remarks,
                MenuPermissions = menus.Select(i => new RoleMenuDto
                {
                    MenuId = i.MenuId,
                    PermissionCodes = menuPermissions.Where(i2 => i2.MenuId == i.MenuId).Select(i2 => i2.PermissionCode).ToList()
                }).ToList()
            };
        }
        /// <summary>
        /// 保存角色
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>

        [TransactionInterceptor]
        public virtual async Task<int> SaveAsync(RoleSaveDto inputDto)
        {
            if (await this.Repository.IsAnyAsync(i => i.Name == inputDto.Name && i.Id != inputDto.Id))
            {
                throw new ApiFailException(ApiFailCode.OPERATION_FAIL, "角色名已存在");
            }
            var roleMenus = new List<RoleMenu>();
            var roleMenuPermissions = new List<RoleMenuPermission>();
            foreach (var item in inputDto.MenuPermissions)
            {
                roleMenus.Add(new RoleMenu
                {
                    MenuId = item.MenuId,
                    RoleId = inputDto.Id
                });
                roleMenuPermissions.AddRange(item.PermissionCodes.Select(code => new RoleMenuPermission
                {
                    MenuId = item.MenuId,
                    RoleId = inputDto.Id,
                    PermissionCode = code
                }));
            }
            //创建
            if (inputDto.Id == default)
            {
                var role = new Role
                {
                    Name = inputDto.Name,
                    Remarks = inputDto.Name,
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now
                };
                inputDto.Id = await this.Repository.InsertReturnIdentityAsync(role);
                if (roleMenus.Any())
                {
                    roleMenus.ForEach(i => i.RoleId = inputDto.Id);
                    await _roleMenuRepository.InsertRangeAsync(roleMenus);
                }
                if (roleMenuPermissions.Any())
                {
                    roleMenuPermissions.ForEach(i => i.RoleId = inputDto.Id);
                    await _roleMenuPermissionRepository.InsertRangeAsync(roleMenuPermissions);
                }
            }
            //更新
            else
            {
                if (inputDto.Id < 0)
                {
                    throw new ApiFailException(ApiFailCode.OPERATION_FAIL, "系统内置角色禁止修改");
                }
                var role = await this.Repository.GetByIdAsync(inputDto.Id);
                if (role == null)
                {
                    throw new ApiFailException(ApiFailCode.OPERATION_FAIL, "角色不存在");
                }
                role.Name = inputDto.Name;
                role.Remarks = inputDto.Remarks;
                role.UpdateTime = DateTime.Now;
                var updateable = this.Repository.AsUpdateable(role);
                await updateable.ExecuteCommandAsync();
                var oldRoleMenus = await _roleMenuRepository.AsQueryable().Where(i => i.RoleId == role.Id).ToListAsync();
                CommonHelper.CollectionChange(roleMenus, oldRoleMenus, (r1, r2) => r1.MenuId == r2.MenuId, data =>
                {
                    _roleMenuRepository.InsertRange(data);
                },
                data =>
                {
                    //_roleMenuRepository.UpdateRange(data);
                },
                data =>
                {
                    var menuIds = data.Select(i => i.MenuId);
                    _roleMenuRepository.Delete(i => i.RoleId == role.Id && menuIds.Contains(i.MenuId));
                });
                var oldRoleMenuPermissions = await _roleMenuPermissionRepository.AsQueryable().Where(i => i.RoleId == role.Id).ToListAsync();
                CommonHelper.CollectionChange(roleMenuPermissions, oldRoleMenuPermissions, (r1, r2) => r1.MenuId == r2.MenuId && r1.PermissionCode == r2.PermissionCode, data =>
                {
                    _roleMenuPermissionRepository.InsertRange(data);
                },
                data =>
                {
                    //_roleMenuPermissionRepository.UpdateRange(data);
                },
                data =>
                {
                    var menuIds = data.Select(i => i.MenuId);
                    _roleMenuPermissionRepository.Delete(i => i.RoleId == role.Id && menuIds.Contains(i.MenuId));
                });

            }
            return inputDto.Id;
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [TransactionInterceptor]
        public async Task DeleteByIdAsync(int id)
        {
            if (id < 0)
            {
                throw new ApiFailException(ApiFailCode.OPERATION_FAIL, "系统内置用户禁止删除");
            }
            await this._roleMenuPermissionRepository.DeleteAsync(i => i.RoleId == id);
            await this._roleMenuRepository.DeleteAsync(i => i.RoleId == id);
            await this.Repository.DeleteByIdAsync(id);
        }

        /// <summary>
        /// 检查是否存在重复的角色名
        /// </summary>
        /// <param name="path"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> CheckExistByNameAsync(string name, int id = 0)
        {
            return await this.Repository.IsAnyAsync(i => i.Name == name && i.Id != id);
        }

        /// <summary>
        /// 根据角色Id获取角色全部菜单，包含上级菜单
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public async Task<List<MenuIncludePermissionDto>> GetRoleMenuPermissionListAsync(int roleId)
        {
            List<MenuIncludePermissionDto> menuPermissions = default;
            if (roleId > 0)
            {
                var menus = await _roleMenuRepository.AsQueryable().Where(i => i.RoleId == roleId).ToListAsync();
                var menuIds = menus.Select(e => e.MenuId).ToList();
                if (menuIds.Any())
                {
                    var parentStrIds = await _menuRepository.AsQueryable().Where(i => menuIds.Contains(i.Id)).Select(i => i.ChainIds).ToListAsync();
                    var parentIds = parentStrIds.Where(i => !string.IsNullOrWhiteSpace(i)).SelectMany(i => i.Split(",")).Where(i => !string.IsNullOrWhiteSpace(i)).Select(i => int.Parse(i)).ToList();
                    var menuAllIds = parentIds.Distinct().ToList();
                    menuPermissions = await _menuRepository.AsQueryable().Where(e => menuAllIds.Contains(e.Id)).OrderBy(e => e.Order).Select(e => new MenuIncludePermissionDto
                    {
                        Id = e.Id,
                        Name = e.Name,
                        ParentId = e.ParentId
                    }).ToListAsync();
                    var permissions = await _roleMenuPermissionRepository.AsQueryable()
                        .InnerJoin<MenuPermission>((rmp, mp) => rmp.PermissionCode == mp.Code && rmp.MenuId == mp.MenuId)
                        .Where(rmp => rmp.RoleId == roleId)
                        .OrderBy((rmp, mp) => mp.Order)
                        .Select((rmp, mp) => new
                        {
                            rmp.MenuId,
                            rmp.PermissionCode,
                            mp.Name
                        }).ToListAsync();
                    menuPermissions.ForEach(item => item.Permissions = permissions.Where(i => i.MenuId == item.Id).Select(i => new MenuPermissionDto
                    {
                        Code = i.PermissionCode,
                        Name = i.Name
                    }).ToList());
                }
            }
            else
            {
                menuPermissions = await _menuService.GetMenuPermissionListAsync();
            }
            return menuPermissions ?? new List<MenuIncludePermissionDto>();
        }


        /// <summary>
        /// 获取角色选项列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<OptionDto>> GetOptionListAsync(RoleQueryDto inputDto)
        {
            var query = Repository.AsQueryable();
            this.BuildWhere(query, inputDto);
            inputDto.PageSize = 0;
            return await query.Select(i => new OptionDto
            {
                Id = i.Id,
                Name = i.Name
            }).BuildByQuery(inputDto).ToListAsync();
        }
    }
}
