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

namespace CaiAdmin.Service.Sys
{
    public class MenuService : BaseService<Menu>
    {
        private readonly AuthContext _authContext;
        private readonly TokenService _tokenService;
        private readonly Repository<MenuApi> _menuApiRepository;
        private readonly Repository<MenuPermission> _menuPermissionRepository;
        private readonly Repository<MenuPermissionApi> _menuPermissionApiRepository;
        private readonly Repository<RoleMenu> _roleMenuRepository;
        private readonly Repository<RoleMenuPermission> _roleMenuPermissionRepository;

        public MenuService(
            Repository<Menu> repository,
            Repository<MenuApi> menuApiRepository,
            Repository<MenuPermission> menuPermissionRepository,
            Repository<MenuPermissionApi> menuPermissionApiRepository,
            Repository<RoleMenu> roleMenuRepository,
            Repository<RoleMenuPermission> roleMenuPermissionRepository,
            IMapper mapper, AuthContext
            authContext,
            TokenService
            tokenService) : base(repository)
        {
            _authContext = authContext;
            _tokenService = tokenService;
            _menuApiRepository = menuApiRepository;
            _menuPermissionRepository = menuPermissionRepository;
            _menuPermissionApiRepository = menuPermissionApiRepository;
            _roleMenuRepository = roleMenuRepository;
            _roleMenuPermissionRepository = roleMenuPermissionRepository;
        }

        /// <summary>
        /// 构建查询条件
        /// </summary>
        /// <param name="query"></param>
        /// <param name="queryDto"></param>
        private ISugarQueryable<Menu> BuildWhere(ISugarQueryable<Menu> query, MenuQueryDto queryDto)
        {
            bool hasWhere = false;
            if (!string.IsNullOrEmpty(queryDto.Name))
            {
                hasWhere = true;
                query.Where(m => m.Name.Contains(queryDto.Name));
            }
            if (!string.IsNullOrEmpty(queryDto.Path))
            {
                hasWhere = true;

                query.Where(e => e.Name.Contains(queryDto.Path));
            }
            if (queryDto.NotId.HasValue && queryDto.NotId.Value != 0)
            {
                hasWhere = true;
                query.Where(e => e.Id != queryDto.NotId.Value);
            }
            if (queryDto.IsPage.HasValue)
            {
                hasWhere = true;
                query.Where(e => e.IsPage == queryDto.IsPage.Value);
            }
            if (hasWhere)
            {
                var subSql = this.Repository.AsQueryable().InnerJoin(query.Select(m2 => new
                {
                    m2.ChainIds
                }), (m, m2) => m2.ChainIds.StartsWith(m.ChainIds)).Select(m => new
                {
                    subId = m.Id
                }).ToSql();
                return this.Repository.AsQueryable()
                     .Where($"EXISTS (Select 1 From ({subSql.Key}) sub Where Id=sub.subId)", subSql.Value);
            }
            else
            {
                return query;
            }
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        public async Task<List<MenuDto>> GetListAsync(MenuQueryDto queryDto)
        {
            var query = this.Repository.AsQueryable();
            query = this.BuildWhere(query, queryDto);
            return await query.OrderBy(m => m.Order).OrderBy(m => m.Id).Select(m => new MenuDto
            {
                Id = m.Id,
                Icon = m.Icon,
                IsPage = m.IsPage,
                Name = m.Name,
                Order = m.Order,
                ParentId = m.ParentId,
                Path = m.Path
            }).ToListAsync();
        }
        /// <summary>
        /// 获取菜单编辑信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<MenuEditDto> GetEditById(int id)
        {
            var menu = await this.Repository.GetByIdAsync(id);
            if (menu == null)
            {
                throw new ApiFailException(ApiFailCode.OPERATION_FAIL, "菜单不存在");
            }
            var parentName = string.Empty;
            if (menu.ParentId.HasValue)
            {
                parentName = this.Repository.AsQueryable().Where(i => i.Id == menu.ParentId.Value).Select(i => i.Name).First();
            }

            var menuApis = await _menuApiRepository.AsQueryable().Where(i => i.MenuId == id).InnerJoin<Api>((ma, a) => ma.ApiId == a.Id)
                .Select((ma, a) => new MenuEditApiDto
                {
                    Id = ma.ApiId,
                    Name = a.Name,
                    Path = a.Path
                }).ToListAsync();
            var menuPermissions = await _menuPermissionRepository.AsQueryable().Where(i => i.MenuId == id)
                .OrderBy(i => i.Order)
                .Select(i => new MenuEditPermissionEditDto
                {
                    Code = i.Code,
                    Name = i.Name,
                    Order = i.Order
                }).ToListAsync();
            var menuPermissionApis = await _menuPermissionApiRepository.AsQueryable().Where(i => i.MenuId == id).InnerJoin<Api>((mpa, a) => mpa.ApiId == a.Id)
                .Select((mpa, a) => new
                {
                    PermissionCode = mpa.PermissionCode,
                    ApiId = mpa.ApiId,
                    ApiName = a.Name,
                    ApiPath = a.Path
                }).ToListAsync();
            foreach (var item in menuPermissions)
            {
                item.Apis = menuPermissionApis.Where(i => i.PermissionCode == item.Code).Select(i => new MenuEditApiDto
                {
                    Id = i.ApiId,
                    Name = i.ApiName,
                    Path = i.ApiPath
                }).ToList();
            }
            return new MenuEditDto
            {
                Id = menu.Id,
                Name = menu.Name,
                CanMultipleOpen = menu.CanMultipleOpen,
                Apis = menuApis,
                Icon = menu.Icon,
                IsPage = menu.IsPage,
                Order = menu.Order,
                Path = menu.Path,
                ParentId = menu.ParentId,
                ParentName = parentName,
                Permissions = menuPermissions
            };
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        [TransactionInterceptor]
        public virtual async Task<int> SaveAsync(MenuSaveDto inputDto)
        {
            if (inputDto.IsPage)
            {
                if (!inputDto.ParentId.HasValue || this.Repository.IsAny(i => i.ParentId == inputDto.ParentId))
                {
                    throw new ApiFailException(ApiFailCode.OPERATION_FAIL, "子页面只能创建在底层菜单下创建");
                }
            }

            var menuApis = inputDto.ApiIds.Select(id => new MenuApi
            {
                MenuId = inputDto.Id,
                ApiId = id
            }).ToList();
            var menuPermissions = new List<MenuPermission>();
            var menuPermissionApis = new List<MenuPermissionApi>();
            foreach (var item in inputDto.Permissions)
            {
                menuPermissions.Add(new MenuPermission
                {
                    MenuId = inputDto.Id,
                    Code = item.Code,
                    Name = item.Name,
                    Order = item.Order
                });
                menuPermissionApis.AddRange(item.ApiIds.Select(id => new MenuPermissionApi
                {
                    ApiId = id,
                    MenuId = inputDto.Id,
                    PermissionCode = item.Code
                }));
            }

            //创建
            if (inputDto.Id == default)
            {
                var menu = new Menu
                {
                    Name = inputDto.Name,
                    ParentId = inputDto.ParentId,
                    Order = inputDto.Order,
                    Path = inputDto.Path
                };
                inputDto.Id = await this.Repository.InsertReturnIdentityAsync(menu);
                string parentChainIds = string.Empty;
                if (inputDto.ParentId.HasValue)
                {
                    parentChainIds = await this.Repository.AsQueryable().Where(e => e.Id == inputDto.ParentId.Value).Select(e => e.ChainIds).FirstAsync();
                }
                var chainIds = parentChainIds + inputDto.Id.ToString() + ",";
                await this.Repository.AsUpdateable().SetColumns(m => m.ChainIds == chainIds).Where(m => m.Id == inputDto.Id).ExecuteCommandAsync();
                if (menuApis.Any())
                {
                    menuApis.ForEach(i => i.MenuId = inputDto.Id);
                    await _menuApiRepository.InsertRangeAsync(menuApis);
                }
                if (menuPermissions.Any())
                {
                    menuPermissions.ForEach(i => i.MenuId = inputDto.Id);
                    await this._menuPermissionRepository.InsertRangeAsync(menuPermissions);
                }
                if (menuPermissionApis.Any())
                {
                    menuPermissionApis.ForEach(i => i.MenuId = inputDto.Id);
                    await _menuPermissionApiRepository.InsertRangeAsync(menuPermissionApis);
                }
            }
            //更新
            else
            {
                if (inputDto.IsPage)
                {
                    if (await this.Repository.IsAnyAsync(i => i.ParentId == inputDto.Id))
                    {
                        throw new ApiFailException(ApiFailCode.OPERATION_FAIL, "作为子页面的菜单必须在底层菜单下级创建！");
                    }
                }
                var menu = await this.Repository.GetByIdAsync(inputDto.Id);
                if (menu == null)
                {
                    throw new ApiFailException(ApiFailCode.OPERATION_FAIL, "菜单不存在");
                }
                menu.Name = inputDto.Name;
                menu.Order = inputDto.Order;
                menu.IsPage = inputDto.IsPage;
                menu.CanMultipleOpen = inputDto.CanMultipleOpen;
                menu.Icon = inputDto.Icon;
                menu.Path = inputDto.Path;

                if (menu.ParentId != inputDto.ParentId)
                {
                    var parentChainIds = string.Empty;
                    var oldChainIds = menu.ChainIds;
                    if (inputDto.ParentId.HasValue)
                    {
                        parentChainIds = await this.Repository.AsQueryable().Where(e => e.Id == menu.ParentId.Value).Select(e => e.ChainIds).FirstAsync();
                    }
                    menu.ParentId = inputDto.ParentId;
                    menu.ChainIds = parentChainIds + inputDto.Id.ToString() + ",";
                    //更新子菜单的ChainIds
                    var childrenMenus = await this.Repository.AsQueryable().Where(i => i.ChainIds.StartsWith(oldChainIds)).ToListAsync();
                    if (childrenMenus.Any())
                    {
                        childrenMenus.ForEach(e =>
                        {
                            e.ChainIds = menu.ChainIds + e.ChainIds.Remove(0, oldChainIds.Length);
                        });
                        await this.Repository.AsUpdateable(childrenMenus).UpdateColumns(i => i.ChainIds).ExecuteCommandAsync();
                    }
                }
                var updateable = this.Repository.AsUpdateable(menu);
                await updateable.ExecuteCommandAsync();


                #region 菜单接口的更新
                var oldMenuApi = await _menuApiRepository.AsQueryable().Where(i => i.MenuId == menu.Id).ToListAsync();
                CommonHelper.CollectionChange(menuApis, oldMenuApi, (r1, r2) => r1.ApiId == r2.ApiId,
                data =>
                {
                    //新增
                    _menuApiRepository.InsertRange(data);
                },
                data =>
                {
                    //更新
                    //_menuApiRepository.UpdateRange(data);
                },
                data =>
                {
                    //删除
                    var apiIds = data.Select(i => i.ApiId);
                    _menuApiRepository.Delete(i => i.MenuId == menu.Id && apiIds.Contains(i.ApiId));
                });
                #endregion

                #region 菜单权限接口的更新
                //旧的菜单权限
                var oldMenuPermissions = await _menuPermissionRepository.GetListAsync(i => i.MenuId == menu.Id);

                //旧的菜单权限接口
                var oldMenuPermissionApis = await _menuPermissionApiRepository.GetListAsync(i => i.MenuId == menu.Id);

                CommonHelper.CollectionChange(menuPermissions, oldMenuPermissions, (r1, r2) => r1.Code == r2.Code,
                data =>
                {
                    //新增
                    _menuPermissionRepository.InsertRange(data);
                },
                data =>
                {
                    //更新
                    _menuPermissionRepository.UpdateRange(data);
                },
                data =>
                {
                    //删除
                    var codes = data.Select(i => i.Code);
                    _menuPermissionRepository.Delete(i => i.MenuId == menu.Id && codes.Contains(i.Code));
                    _roleMenuPermissionRepository.Delete(i => codes.Contains(i.PermissionCode) && i.MenuId == menu.Id);

                });
                CommonHelper.CollectionChange(menuPermissionApis, oldMenuPermissionApis, (r1, r2) => r1.PermissionCode == r2.PermissionCode && r1.ApiId == r2.ApiId,
                data =>
                {
                    //新增
                    _menuPermissionApiRepository.InsertRange(data);
                },
                data =>
                {
                    //更新

                },
                data =>
                {
                    //删除
                    var codes = data.Select(i => i.PermissionCode);
                    var apiIds = data.Select(i => i.ApiId);
                    _menuPermissionApiRepository.Delete(i => i.MenuId == menu.Id && codes.Contains(i.PermissionCode) && apiIds.Contains(i.ApiId));
                });
                #endregion

            }
            return inputDto.Id;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteByIdAsync(int id)
        {
            if (await this.Repository.IsAnyAsync(i => i.ParentId == id))
            {
                throw new ApiFailException(ApiFailCode.OPERATION_FAIL, "此菜单下还有子菜单，无法删除！");
            }

            await _roleMenuRepository.DeleteAsync(i => i.MenuId == id);
            await _roleMenuPermissionRepository.DeleteAsync(i => i.MenuId == id);

            await _menuApiRepository.DeleteAsync(i => i.MenuId == id);
            await _menuPermissionApiRepository.DeleteAsync(i => i.MenuId == id);
            await this.Repository.DeleteByIdAsync(id);
        }

        /// <summary>
        /// 获取菜单及权限数据
        /// </summary>
        /// <returns></returns>
        public async Task<List<MenuIncludePermissionDto>> GetMenuPermissionListAsync()
        {
            var menuPermissions = await Repository.AsQueryable().Where(i => i.IsPage == false).OrderBy(e => e.Order).Select(e => new MenuIncludePermissionDto
            {
                Id = e.Id,
                Name = e.Name,
                ParentId = e.ParentId
            }).ToListAsync();
            var permissions = await _menuPermissionRepository.AsQueryable().OrderBy(i => i.Order).ToListAsync();
            menuPermissions.ForEach(i => i.Permissions = permissions.Where(i2 => i2.MenuId == i.Id).Select(i2 => new MenuPermissionDto
            {
                Code = i2.Code,
                Name = i2.Name
            }).ToList());
            return menuPermissions;
        }
    }
}
