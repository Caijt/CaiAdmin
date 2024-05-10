using AutoMapper;
using CaiAdmin.Common;
using CaiAdmin.Common.CacheHelper;
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
using CaiAdmin.Dto;
using CaiAdmin.Dto.Sys;

namespace CaiAdmin.Service.Sys
{
    public class TokenService : BaseService<Token>
    {
        private readonly AuthContext _authContext;
        private readonly ICacheHelper _cacheHelper;
        public static string DisabledTokenCacheKey = "DisabledToken";

        public TokenService(Repository<Token> repository, ICacheHelper cacheHelper, IMapper mapper, AuthContext authContext) : base(repository)
        {
            _authContext = authContext;
            _cacheHelper = cacheHelper;
        }

        /// <summary>
        /// 构建查询条件
        /// </summary>
        /// <param name="query"></param>
        /// <param name="queryDto"></param>
        private void BuildWhere(ISugarQueryable<Token> query, TokenQueryDto queryDto)
        {
            if (!string.IsNullOrEmpty(queryDto.UserLoginName))
            {
                query.Where(e => e.User.LoginName.Contains(queryDto.UserLoginName));
            }
            if (!string.IsNullOrEmpty(queryDto.AccessToken))
            {
                query.Where(e => e.AccessToken.Contains(queryDto.AccessToken));
            }
            if (!string.IsNullOrEmpty(queryDto.RefreshToken))
            {
                query.Where(e => e.RefreshToken.Contains(queryDto.RefreshToken));
            }
            if (queryDto.InCacheDisabled.HasValue)
            {
                var disabledTokens = _cacheHelper.SetGet(DisabledTokenCacheKey);
                if (disabledTokens == null)
                {
                    disabledTokens = new HashSet<string>();
                }
                if (queryDto.InCacheDisabled.Value)
                {
                    query.Where(e => disabledTokens.Contains(e.AccessToken));
                }
                else
                {
                    query.Where(e => !disabledTokens.Contains(e.AccessToken));
                }
            }
        }

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        public async Task<PageListDto<TokenDto>> GetPageListAsync(TokenQueryDto queryDto)
        {
            var result = new PageListDto<TokenDto>();
            var query = this.Repository.AsQueryable();
            this.BuildWhere(query, queryDto);
            result.Total = await query.Clone().CountAsync();
            result.List = await query.Select(t => new TokenDto
            {
                Id = t.Id,
                AccessExpire = t.AccessExpire,
                UserId = t.UserId,
                AccessToken = t.AccessToken,
                CreateTime = t.CreateTime,
                Ip = t.Ip,
                IsDisabled = t.IsDisabled,
                RefreshExpire = t.RefreshExpire,
                RefreshToken = t.RefreshToken,
                UserLoginName = t.User.LoginName
            }).BuildByQuery(queryDto, i => i.CreateTime, OrderByType.Desc).ToListAsync();
            return result;
        }

        public async Task<long> CreateAsync(TokenDto dto)
        {
            ////创建
            //if (dto.Id == default)
            //{
            var token = new Token
            {
                Ip = _authContext.UserIP.ToString(),
                AccessExpire = dto.AccessExpire,
                AccessToken = dto.AccessToken,
                CreateTime = DateTime.Now,
                RefreshExpire = dto.RefreshExpire,
                RefreshToken = dto.RefreshToken,
                UserId = dto.UserId
            };
            dto.Id = await this.Repository.InsertReturnIdentityAsync(token);
            //}
            ////更新
            //else
            //{
            //    var role = await this.Repository.GetByIdAsync(dto.Id);
            //    if (role == null)
            //    {
            //        throw new ApiFailException(ApiFailCode.OPERATION_FAIL, "角色不存在");
            //    }
            //    //role.Name = dto.Name;
            //    //role.Remarks = dto.Remarks;
            //    //role.UpdateTime = DateTime.Now;
            //    var updateable = this.Repository.AsUpdateable(role);
            //    await updateable.ExecuteCommandAsync();
            //}
            return dto.Id;
        }



        /// <summary>
        /// 禁用Token
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public async Task DisableTokenAsync(string accessToken)
        {
            await this.Repository.AsUpdateable().SetColumns(i => i.IsDisabled == true).Where(i => i.AccessToken == accessToken).ExecuteCommandAsync();

            _cacheHelper.SetAdd(DisabledTokenCacheKey, accessToken);
        }


        /// <summary>
        /// 根据用户Id禁用Token
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public async Task DisableTokenByUserIdAsync(int userId)
        {
            var tokens = await this.Repository.AsQueryable().Where(e => e.UserId == userId && e.RefreshExpire >= DateTime.Now).Select(e => e.AccessToken).ToListAsync();
            foreach (var token in tokens)
            {
                await this.DisableTokenAsync(token);
            }
        }


        /// <summary>
        /// 重载已禁用的AccessToken到缓存中
        /// </summary>
        /// <returns></returns>
        public async Task ReloadDisabledTokenAsync()
        {
            var tokens = await this.Repository.AsQueryable().Where(e => e.IsDisabled == true && e.AccessExpire > DateTime.Now).Select(e => e.AccessToken).ToListAsync();
            _cacheHelper.Delete(DisabledTokenCacheKey);
            if (tokens.Count > 0)
            {
                _cacheHelper.SetAdd(DisabledTokenCacheKey, tokens.ToArray());
            }

        }

        /// <summary>
        /// 根据AccessToken获取Token记录
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public async Task<TokenDto> GetByAccessTokenAsync(string accessToken)
        {
            return await this.Repository.AsQueryable().Where(i => i.AccessToken == accessToken)
                .LeftJoin<User>((t, u) => t.UserId == u.Id).Select((t, u) => new TokenDto
                {
                    AccessExpire = t.AccessExpire,
                    AccessToken = t.AccessToken,
                    CreateTime = t.CreateTime,
                    Id = t.Id,
                    Ip = t.Ip,
                    IsDisabled = t.IsDisabled,
                    RefreshExpire = t.RefreshExpire,
                    RefreshToken = t.RefreshToken,
                    UserId = t.UserId,
                    UserLoginName = u.LoginName
                }).FirstAsync();
        }
    }
}
