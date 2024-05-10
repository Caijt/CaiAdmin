using CaiAdmin.Common;
using CaiAdmin.Database;
using CaiAdmin.Service;
using CaiAdmin.Entity.Sys;
using CaiAdmin.Option;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using CaiAdmin.Service.Common;
using CaiAdmin.Service.Interceptors;
using CaiAdmin.Service.Sys;
using System.Threading.Tasks;
using CaiAdmin.Dto;

namespace CaiAdmin.Service
{
    /// <summary>
    /// 认证服务
    /// </summary>
    public class AuthService
    {
        private readonly JwtOption _jwtOption;
        private readonly AuthContext _authContext;
        private readonly TokenService _tokenService;
        private readonly RoleService _roleService;
        private readonly UserLoginLogService _userLoginLogService;
        private readonly Repository<User> _userRepository;
        private readonly Repository<UserLoginLog> _userLoginLogRepository;
        private readonly Repository<Api> _apiRepository;
        private readonly Repository<RoleMenu> _roleMenuRepository;
        private readonly Repository<Token> _tokenRepository;
        private readonly Repository<RoleMenuPermission> _roleMenuPermissionRepository;
        private readonly Repository<Menu> _menuRepository;
        private readonly Repository<MenuPermission> _menuPermissionRepository;

        public AuthService(
            //AppDbContext dbContext,
            AuthContext authContext,
            Repository<User> userRepository,
            Repository<RoleMenu> roleMenuRepository,
            Repository<UserLoginLog> userLoginLogRepository,
            Repository<Api> apiRepository,
            RoleService roleService,
            Repository<Token> tokenRepository,
            TokenService tokenService,
            Repository<RoleMenuPermission> roleMenuPermissionRepository,
            Repository<MenuPermission> menuPermissionRepository,
            Repository<Menu> menuRepository,
            UserLoginLogService userLoginLogService,
            IOptionsSnapshot<JwtOption> option)
        {
            _jwtOption = option.Value;
            _authContext = authContext;
            _tokenService = tokenService;
            _roleService = roleService;
            _userLoginLogService = userLoginLogService;
            _userRepository = userRepository;
            _userLoginLogRepository = userLoginLogRepository;
            _apiRepository = apiRepository;
            _roleMenuRepository = roleMenuRepository;
            _tokenRepository = tokenRepository;
            _roleMenuPermissionRepository = roleMenuPermissionRepository;
            _menuRepository = menuRepository;
            _menuPermissionRepository = menuPermissionRepository;
        }
        /// <summary>
        /// 根据登录名及密码获取身份token
        /// </summary>
        /// <param name="loginDto"></param>
        /// <returns></returns>
        [TransactionInterceptor]
        public async Task<LoginResultDto> GetTokenByLoginAsync(LoginDto loginDto)
        {
            var user = await _userRepository.AsQueryable().Where(i => i.IsDisabled == false && i.LoginName == loginDto.LoginName).FirstAsync();
            var result = new LoginResultDto();
            if (user == null)
            {
                result.Status = LoginResultDto.USER_FAIL;
                return result;
            }
            if (user.LoginPassword != loginDto.LoginPassword)
            {
                result.Status = LoginResultDto.PASSWORD_FAIL;
                return result;
            }
            await _userLoginLogRepository.InsertAsync(new UserLoginLog
            {
                UserId = user.Id,
                CreateTime = DateTime.Now,
                IpAddress = _authContext.UserIP.ToString()
            });
            result.Status = LoginResultDto.SUCCESS;
            result.AccessToken = CreateAccessToken(user.Id);
            result.AccessExpiresIn = _jwtOption.AccessExpiresIn;
            result.RefreshToken = CreateRefreshToken(user.Id, loginDto.IsRemember);
            result.RefreshExpiresIn = loginDto.IsRemember ? _jwtOption.RememberRefreshExpiresIn : _jwtOption.RefreshExpiresIn;
            await SaveTokenRecordAsync(result, user.Id);
            return result;
        }

        /// <summary>
        /// 注销登录
        /// </summary>
        /// <returns></returns>
        public async Task LogoutAsync()
        {
            await _tokenService.DisableTokenAsync(_authContext.AccessToken);
        }

        /// <summary>
        /// 保存Token记录
        /// </summary>
        /// <param name="loginResult"></param>
        /// <param name="userId"></param>
        private async Task SaveTokenRecordAsync(LoginResultDto loginResult, int userId)
        {
            //var token = await _tokenService.GetByAccessTokenAsync(loginResult.AccessToken);
            await _tokenService.CreateAsync(new Service.Sys.TokenDto
            {
                //Id = token == null ? 0 : token.Id,
                AccessToken = loginResult.AccessToken,
                RefreshToken = loginResult.RefreshToken,
                UserId = userId,
                AccessExpire = DateTime.Now.AddSeconds(loginResult.AccessExpiresIn),
                RefreshExpire = DateTime.Now.AddSeconds(loginResult.RefreshExpiresIn)
            });
        }

        /// <summary>
        /// 刷新token，为什么使用缓存，因为考虑到可能同时并发多个接口
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        [CacheInterceptor(ExpireSeconds = 5)]
        [TransactionInterceptor]
        public virtual async Task<LoginResultDto> RefreshTokenAsync(RefreshTokenDto refreshTokenDto)
        {
            var result = new LoginResultDto();
            try
            {
                if (!await _tokenRepository.IsAnyAsync(e => e.AccessToken == refreshTokenDto.AccessToken && e.RefreshToken == refreshTokenDto.RefreshToken && e.RefreshExpire >= DateTime.Now && e.IsDisabled == false))
                {
                    throw new SecurityTokenValidationException();
                }
                var tokenValidation = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _jwtOption.Issuer,
                    ValidAudience = _jwtOption.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOption.AccessSecretKey)),
                    ClockSkew = TimeSpan.Zero
                };
                var handler = new JwtSecurityTokenHandler();
                var token2 = handler.ValidateToken(refreshTokenDto.AccessToken, tokenValidation, out SecurityToken _);
                tokenValidation.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOption.RefreshSecretKey));
                tokenValidation.ValidateLifetime = true;

                var token3 = new JwtSecurityTokenHandler().ValidateToken(refreshTokenDto.RefreshToken, tokenValidation, out SecurityToken _);

                if (token2.FindFirst("uid").Value != token3.FindFirst("uid").Value)
                {
                    throw new SecurityTokenValidationException();
                }
                var userId = int.Parse(token3.FindFirst("uid").Value);
                var user = await _userRepository.AsQueryable().Where(e => e.IsDisabled == false && e.Id == userId).FirstAsync();
                if (user == null)
                {
                    result.Status = LoginResultDto.USER_FAIL;
                    return result;
                }
                //await _userLoginLogService.CreateAsync();
                result.Status = LoginResultDto.SUCCESS;
                result.AccessToken = CreateAccessToken(user.Id);
                result.AccessExpiresIn = _jwtOption.AccessExpiresIn;
                result.RefreshToken = CreateRefreshToken(user.Id, refreshTokenDto.IsRemember);
                result.RefreshExpiresIn = refreshTokenDto.IsRemember ? _jwtOption.RememberRefreshExpiresIn : _jwtOption.RefreshExpiresIn;
                await SaveTokenRecordAsync(result, user.Id);
                await _tokenService.DisableTokenAsync(refreshTokenDto.AccessToken);
            }
            catch (SecurityTokenValidationException)
            {
                result.Status = LoginResultDto.TOKEN_FAIL;
            }
            return result;
        }

        /// <summary>
        /// 创建AccessToken
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string CreateAccessToken(int userId)
        {
            var claims = new Claim[] {
                new Claim("uid",userId.ToString())
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOption.AccessSecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(
                claims: claims,
                issuer: _jwtOption.Issuer,
                audience: _jwtOption.Audience,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddSeconds(_jwtOption.AccessExpiresIn),
                signingCredentials: creds
                );
            var token = new JwtSecurityTokenHandler().WriteToken(jwt);
            return token;
        }

        /// <summary>
        /// 创建RefreshToken
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string CreateRefreshToken(int userId, bool isRemember)
        {
            var claims = new Claim[] {
                new Claim("uid",userId.ToString()),
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOption.RefreshSecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var jwt = new JwtSecurityToken(
                claims: claims,
                issuer: _jwtOption.Issuer,
                audience: _jwtOption.Audience,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddSeconds(isRemember ? _jwtOption.RememberRefreshExpiresIn : _jwtOption.RefreshExpiresIn),
                signingCredentials: creds
                );
            var token = new JwtSecurityTokenHandler().WriteToken(jwt);
            return token;
        }

        /// <summary>
        /// 获取登录信息
        /// </summary>
        public async Task<AuthInfoDto> GetAuthInfoAsync()
        {
            var result = new AuthInfoDto();
            var user = await _userRepository.GetSingleAsync(e => e.Id == _authContext.UserId);
            result.MenuPermissions = await this.GetMenuPermissionsByRoleIdAsync(user.RoleId);
            result.UserId = user.Id;
            result.UserName = user.LoginName;
            return result;
        }

        /// <summary>
        /// 根据角色Id获取角色全部菜单，包含上级菜单
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public async Task<List<AuthMenuPermissionDto>> GetMenuPermissionsByRoleIdAsync(int roleId)
        {
            List<AuthMenuPermissionDto> menuPermissions = default;
            if (roleId > 0)
            {
                var menus = await _roleMenuRepository.AsQueryable().Where(i => i.RoleId == roleId).ToListAsync();
                var menuIds = menus.Select(e => e.MenuId).ToList();
                if (menuIds.Any())
                {
                    var parentStrIds = await _menuRepository.AsQueryable().Where(i => menuIds.Contains(i.Id)).Select(i => i.ChainIds).ToListAsync();
                    var parentIds = parentStrIds.Where(i => !string.IsNullOrWhiteSpace(i)).SelectMany(i => i.Split(",")).Where(i => !string.IsNullOrWhiteSpace(i)).Select(i => int.Parse(i)).ToList();
                    var menuAllIds = parentIds.Distinct().ToList();
                    menuPermissions = await _menuRepository.AsQueryable().Where(e => menuAllIds.Contains(e.Id)).OrderBy(e => e.Order).Select(e => new AuthMenuPermissionDto
                    {
                        Id = e.Id,
                        Name = e.Name,
                        ParentId = e.ParentId,
                        Path = e.Path,
                        Icon = e.Icon,
                        CanMultipleOpen = e.CanMultipleOpen,
                        IsPage = e.IsPage
                    }).ToListAsync();
                    var permissionCodes = await _roleMenuPermissionRepository.AsQueryable().Where(i => i.RoleId == roleId).Select(i => new
                    {
                        i.MenuId,
                        i.PermissionCode
                    }).ToListAsync();
                    menuPermissions.ForEach(item => item.PermissionCodes = permissionCodes.Where(i => i.MenuId == item.Id).Select(i => i.PermissionCode).ToList());
                }
            }
            else
            {
                menuPermissions = await _menuRepository.AsQueryable().OrderBy(e => e.Order).Select(e => new AuthMenuPermissionDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    ParentId = e.ParentId,
                    Path = e.Path,
                    Icon = e.Icon,
                    CanMultipleOpen = e.CanMultipleOpen,
                    IsPage = e.IsPage
                }).ToListAsync();
                var permissions = await _menuPermissionRepository.GetListAsync();
                menuPermissions.ForEach(i => i.PermissionCodes = permissions.Where(i2 => i2.MenuId == i.Id).Select(i2 => i2.Code).ToList());
            }
            return menuPermissions ?? new List<AuthMenuPermissionDto>();
        }

        /// <summary>
        /// 通过访问路径验证权限
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public async Task<ApiPermissionCheckResultDto> CheckApiPermissionByPathAsync(string path)
        {
            var result = new ApiPermissionCheckResultDto();
            var user = await _userRepository.AsQueryable().Where(i => i.Id == _authContext.UserId).FirstAsync();
            if (user.RoleId < 0)
            {
                result.IsSuccess = true;
                return result;
            }
            var api = await _apiRepository.AsQueryable().Where(i => i.Path == path).FirstAsync();
            if (api == null)
            {
                result.IsSuccess = false;
                return result;
            }
            result.ApiName = api.Name;
            if (await _roleMenuRepository.AsQueryable()
                .InnerJoin<MenuApi>((rm, ma) => rm.MenuId == ma.MenuId)
                .Where((rm, ma) => rm.RoleId == user.RoleId && ma.ApiId == api.Id)
                .AnyAsync())
            {
                result.IsSuccess = true;
                return result;
            }

            if (await _roleMenuPermissionRepository.AsQueryable()
                .InnerJoin<MenuPermissionApi>((rmp, mpa) => rmp.PermissionCode == mpa.PermissionCode)
                .Where((rmp, mpa) => rmp.RoleId == user.RoleId && mpa.ApiId == api.Id)
                .AnyAsync())
            {
                result.IsSuccess = true;
                return result;
            }
            //if (api.IsCommon && roleMenu != null)
            //{
            //    result.IsSuccess = true;
            //    //result.IsSuccess = await _appDbContext.Set<User>().Where(e => e.Id == _authContext.UserId).SelectMany(e => e.Role.RoleMenus).Select(e => e.Menu).SelectMany(e => e.MenuApis).AnyAsync(e => e.ApiId == api.Id);
            //    return result;
            //}
            //Expression<Func<RoleMenu, bool>> exp;
            //if (api.PermissionType == ApiPermissionType.READ && roleMenu.CanRead)
            //{
            //    result.IsSuccess = true;
            //    //exp = e => e.CanRead;
            //}
            //else if (api.PermissionType == ApiPermissionType.WRITE && roleMenu.CanWrite)
            //{
            //    result.IsSuccess = true;
            //    //exp = e => e.CanWrite;
            //}
            //else if (api.PermissionType == ApiPermissionType.REVIEW && roleMenu.CanReview)
            //{
            //    result.IsSuccess = true;
            //    //exp = e => e.CanReview;
            //}
            //result.IsSuccess = await _appDbContext.Set<Role>().Where(e => e.Id == user.RoleId).SelectMany(e => e.RoleMenus).Where(exp).Select(e => e.Menu).SelectMany(e => e.MenuApis).AnyAsync(e => e.ApiId == api.Id);
            return result;
        }

        public async Task ChangePasswordAsync(ChangePasswordDto dto)
        {
            if (dto.OldPassword == dto.NewPassword)
            {
                throw new ApiFailException(ApiFailCode.OPERATION_FAIL, "新密码与原密码一样，无法修改");
            }
            var user = await _userRepository.AsQueryable().Where(e => e.Id == _authContext.UserId).FirstAsync();
            if (user == null)
            {
                throw new ApiFailException(ApiFailCode.OPERATION_FAIL, "用户不存在");
            }
            if (user.LoginPassword != dto.OldPassword)
            {
                throw new ApiFailException(ApiFailCode.OPERATION_FAIL, "用户原密码不正确");
            }
            await _userRepository.AsUpdateable().SetColumns(i => i.LoginPassword == dto.NewPassword).Where(i => i.Id == user.Id).ExecuteCommandAsync();
            await _tokenService.DisableTokenByUserIdAsync(_authContext.UserId);
        }
    }
}
