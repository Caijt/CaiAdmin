using CaiAdmin.Dto;
using CaiAdmin.Service;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace CaiAdmin.Service
{
    public static class SugarQueryableExtensions
    {
        /// <summary>
        /// 根据查询参数对象进行构建
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="queryDto"></param>
        /// <param name="isPage">是否分页</param>
        public static ISugarQueryable<T> BuildByQuery<T>(this ISugarQueryable<T> query, QueryDto queryDto, Expression<Func<T, object>> defaultOrderExp = null, OrderByType type = OrderByType.Asc, bool? isPage = null)
        {
            return query.BuildPageByQuery(queryDto, isPage).BuildOrderByQuery(queryDto, defaultOrderExp, type);
        }

        public static ISugarQueryable<T> BuildPageByQuery<T>(this ISugarQueryable<T> query, QueryDto queryDto, bool? isPage = null)
        {
            if (isPage == null)
            {
                isPage = queryDto.PageIndex >= 0 && queryDto.PageSize > 0;
            }

            if (isPage.HasValue && isPage.Value)
            {
                if (queryDto.PageContinuity)
                {
                    query.Skip(queryDto.PageIndex);
                }
                else
                {
                    query.Skip((queryDto.PageIndex - 1) * queryDto.PageSize);
                }
                query.Take(queryDto.PageSize);
            }
            return query;
        }

        public static ISugarQueryable<T> BuildOrderByQuery<T>(this ISugarQueryable<T> query, QueryDto queryDto, Expression<Func<T, object>> defaultOrderExp = null, OrderByType type = OrderByType.Asc)
        {
            //排序
            if (!string.IsNullOrWhiteSpace(queryDto.Order))
            {
                query.OrderBy(queryDto.Order);
            }
            else if (defaultOrderExp != null)
            {
                query.OrderBy(defaultOrderExp, type);
            }
            return query;
        }
    }
}
