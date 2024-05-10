using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using CaiAdmin.Database;
using CaiAdmin.Service;
//using CaiAdmin.Dto.Sys;
using CaiAdmin.Entity.Sys;
using CaiAdmin.Service.Interceptors;
using CaiAdmin.Dto.Sys;

namespace CaiAdmin.Service.Sys
{
    public class ConfigServiceBak : EntityService<ConfigDto, ConfigDto, ConfigDto, ConfigQueryDto, Config, ConfigKey>
    {
        public ConfigServiceBak(AppDbContext appDbContext, IMapper mapper, IEnumerable<IEntityHandler<Config>> entityHandlers, IEnumerable<IQueryFilter<Config>> queryFilters) : base(appDbContext, mapper, entityHandlers, queryFilters)
        {
            this.IdFieldExpression = e => Enum.Parse<ConfigKey>(e.Key);
        }
        [CacheInterceptor("ConfigList")]
        public override Task<List<ConfigDto>> GetListAsync(ConfigQueryDto queryDto)
        {
            return base.GetListAsync(queryDto);
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        /// <param name="dtos"></param>
        /// <returns></returns>
        [CacheDeleteInterceptor("Config", "ConfigList")]
        public virtual async Task SaveAsync(List<ConfigDto> dtos)
        {
            this.AutoSaveChanges = false;
            foreach (var d in dtos)
            {
                await this.UpdateAsync(d);
            }
            await this.DbContext.SaveChangesAsync();
        }

        protected override void OnUpdating(Config newEntity, Config oldEntity, ConfigDto updateDto, List<Expression<Func<Config, object>>> excludeOrIncludeFields, bool isExcludeField)
        {
            if (isExcludeField)
            {
                excludeOrIncludeFields.Add(e => e.Type);
            }
        }

        protected override IQueryable<Config> OnFilter(IQueryable<Config> query, ConfigQueryDto queryDto)
        {
            if (queryDto.Keys != null)
            {
                query = query.Where(e => queryDto.Keys.Contains(e.Key.ToString()));
            }
            return query;
        }

        /// <summary>
        /// 根据key值查询配置参数值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [CacheInterceptor("Config")]
        public virtual async Task<string> GetValueByKey(string key)
        {
            return await this.DbQuery.Where(e => e.Key == key).Select(e => e.Value).FirstOrDefaultAsync();
        }
    }
}
