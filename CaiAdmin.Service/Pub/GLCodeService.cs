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
using CaiAdmin.Dto.Pub;

namespace CaiAdmin.Service.Pub
{
    public class GLCodeService
    {
        private readonly CityOceanRepository _cityOceanRepository;
        private readonly CityOceanApiService _cityOceanApiService;
        public GLCodeService(CityOceanRepository cityOceanRepository, CityOceanApiService cityOceanApiService)
        {
            _cityOceanRepository = cityOceanRepository;
            _cityOceanApiService = cityOceanApiService;
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>        
        public async Task<List<GLCodeDto>> GetListAsync(GLCodeQueryDto queryDto)
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
            var subSql = _cityOceanRepository.Context.SqlQueryable<GLCodeDto>(@"
                Select 
                    Id,
                    JSON_VALUE(LocalizationText,'$.Name.zh') Name,
                    ParentId,
                    Code,
                    LevelCode,
                    GLCodeType
                From CO_PUB.dbo.GLCodes")
               .WhereIF(!string.IsNullOrWhiteSpace(queryDto.Name), ou => ou.Name.Contains(queryDto.Name))
               .WhereIF(queryDto.Types != null && queryDto.Types.Any(), ou => queryDto.Types.Contains(ou.Type))
               .WhereIF(queryDto.Ids != null && queryDto.Ids.Any(), ou => queryDto.Ids.Contains(ou.Id))
               .Select(ou => ou.LevelCode)
               .ToSql();

            return await _cityOceanRepository.Context.Ado.SqlQueryAsync<GLCodeDto>($@"
                Select 
                    Id,
                    JSON_VALUE(LocalizationText,'$.Name.zh') Name,
                    ParentId,
                    Code,
                    GLCodeType,
                    LevelCode
                From CO_PUB.dbo.GLCodes ou 
                Where
                EXISTS (
                    Select TOP 1 1 From (
                        Select ou.Id From CO_PUB.dbo.GLCodes ou INNER JOIN
                        ({subSql.Key}) sub ON sub.LevelCode Like  ou.LevelCode +'%'
                        UNION ALL
                        Select ou.Id From CO_PUB.dbo.GLCodes ou INNER JOIN
                        ({subSql.Key}) sub ON ou.LevelCode Like  sub.LevelCode +'%'
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
