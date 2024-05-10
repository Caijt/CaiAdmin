using AutoMapper;
using CaiAdmin.Common;
using CaiAdmin.Database;
using CaiAdmin.Service;
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
    public class ConfigService : BaseService<Config>
    {
        public ConfigService(Repository<Config> repository) : base(repository)
        {

        }

        /// <summary>
        /// 构建查询条件
        /// </summary>
        /// <param name="query"></param>
        /// <param name="queryDto"></param>
        private void BuildWhere(ISugarQueryable<Config> query, ConfigQueryDto queryDto)
        {
            query.WhereIF(queryDto.Keys != null, e => queryDto.Keys.Contains(e.Key.ToString()));
        }

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        public async Task<List<ConfigDto>> GetListAsync(ConfigQueryDto queryDto)
        {
            var query = this.Repository.AsQueryable();
            this.BuildWhere(query, queryDto);
            //query.BuildByQuery(queryDto);
            var configs = await query.ToListAsync();
            return configs.Select(r => new ConfigDto
            {
                Key = Enum.Parse<ConfigKey>(r.Key),
                Type = Enum.Parse<ConfigType>(r.Type),
                Value = r.Value
            }).ToList();
        }

        [CacheDeleteInterceptor("Config", "ConfigList")]
        public async Task<int> SaveAsync(List<ConfigDto> dtos)
        {
            var configs = dtos.Select(i => new Config
            {
                Key = i.Key.ToString(),
                Value = i.Value
            }).ToList();
            return await this.Repository.AsUpdateable(configs).UpdateColumns(i => i.Value).ExecuteCommandAsync();
        }

        /// <summary>
        /// 根据key值查询配置参数值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [CacheInterceptor("Config")]
        public virtual async Task<string> GetValueByKey(string key)
        {
            var key2 = Enum.Parse<ConfigKey>(key);
            return await this.Repository.AsQueryable().Where(e => e.Key.Equals(key)).Select(e => e.Value).FirstAsync();
        }
    }
}
