using CaiAdmin.ApiService.CityOcean;
using CaiAdmin.Common;
using CaiAdmin.Dto;
using CaiAdmin.Dto.Fam;
using CaiAdmin.Database;
using CaiAdmin.Service;
using CaiAdmin.Service.Fam;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CaiAdmin.Dto.Platform;

namespace CaiAdmin.Service.Platform
{
    public class OrganizationUnitService
    {
        private readonly CityOceanRepository _cityOceanRepository;
        private readonly CityOceanApiService _cityOceanApiService;
        public OrganizationUnitService(CityOceanRepository cityOceanRepository, CityOceanApiService cityOceanApiService)
        {
            _cityOceanRepository = cityOceanRepository;
            _cityOceanApiService = cityOceanApiService;
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>        
        public async Task<List<OrganizationUnitDto>> GetListAsync(OrganizationUnitQueryDto queryDto)
        {
            if (!string.IsNullOrWhiteSpace(queryDto.IdString))
            {
                var ids = queryDto.IdString.Trim().Split(',', ' ');
                var guidIds = new List<Guid>();
                foreach (var item in ids)
                {
                    if (Guid.TryParse(item, out Guid id))
                    {
                        guidIds.Add(id);
                    }
                }
                if (queryDto.Ids != null && queryDto.Ids.Any())
                {
                    guidIds.AddRange(queryDto.Ids);
                }
                queryDto.Ids = guidIds.ToArray();
            }

            var query = _cityOceanRepository.Context.SqlQueryable<OrganizationUnitDto>(@"
                Select 
                    Id,
                    JSON_VALUE(LocalizationText,'$.DisplayName.zh') Name,
                    ParentId,
                    LevelCode,
                    Type,
                    IsDeleted
                From CO_Platform.dbo.OrganizationUnits")
               .WhereIF(!string.IsNullOrWhiteSpace(queryDto.Name), ou => ou.Name.Contains(queryDto.Name))
               .WhereIF(queryDto.Types != null && queryDto.Types.Any(), ou => queryDto.Types.Contains(ou.Type))
               .WhereIF(queryDto.Ids != null && queryDto.Ids.Any(), ou => queryDto.Ids.Contains(ou.Id))
               .WhereIF(queryDto.IsDeleted.HasValue, ou => ou.IsDeleted == queryDto.IsDeleted);

            if (!queryDto.IncludeParent && !queryDto.IncludeChildren)
            {
                return await query.ToListAsync();
            }

            var subSql = query
               .Select(ou => ou.LevelCode)
               .ToSql();
            var parentChildrenSubSql = new List<string>();
            if (queryDto.IncludeParent)
            {
                parentChildrenSubSql.Add($@"Select ou.Id From CO_Platform.dbo.OrganizationUnits ou INNER JOIN
                        ({ subSql.Key}) sub ON sub.LevelCode Like  ou.LevelCode + '%'");
            }
            if (queryDto.IncludeChildren)
            {
                parentChildrenSubSql.Add($@"Select ou.Id From CO_Platform.dbo.OrganizationUnits ou INNER JOIN
                        ({subSql.Key}) sub ON ou.LevelCode Like  sub.LevelCode +'%'");
            }

            return await _cityOceanRepository.Context.Ado.SqlQueryAsync<OrganizationUnitDto>($@"
                Select 
                    Id,
                    JSON_VALUE(LocalizationText,'$.DisplayName.zh') Name,
                    ParentId,
                    Type,
                    LevelCode,
                    IsDeleted
                From CO_Platform.dbo.OrganizationUnits ou 
                Where
                EXISTS (
                    Select TOP 1 1 From (
                        {string.Join(" UNION ALL ", parentChildrenSubSql)}
                    )
                    sub2 Where ou.Id=sub2.Id)
                ", subSql.Value);


        }


        /// <summary>
        /// 获取账单详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<BillDetailDto> GetDetailAsync(Guid Id)
        {
            var bill = await this._cityOceanRepository.Context.SqlQueryable<BillDetailDto>(@"Select 
                b.Id,
                b.No,
                b.RefId,
                b.AccountDate,
                b.DueDate,
                b.CreationTime,                    
                (Select top 1 CName From CO_SSO.dbo.Users u where u.Id = b.CreatorUserId) as CreatorUserName,
                b.LastModificationTime
                From CO_FAM.dbo.Bills b 
            ").Where(i => i.Id == Id).FirstAsync();
            if (bill == null)
            {
                throw new ApiFailException(ApiFailCode.OPERATION_FAIL, "账单不存在");
            }
            return bill;
        }
    }
}
