using AutoMapper;
using ItSys.Dto;
using ItSys.Entity;
using CaiAdmin.Database;
using CaiAdmin.Service;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ItSys.Service
{
    public class ItAssetTypeService : BaseService<ItAssetType>
    {
        public ItAssetTypeService(Repository<ItAssetType>  repository, IMapper mapper, AuthContext authContext)
            : base(repository)
        {
            //orderDescDefaultValue = false;
            //orderProp = prop => e => e.order;
            //onAfterCreate = (entity, dto) =>
            //{
            //    dbContext.Database.ExecuteSqlCommand($"call proc_sync_tree_parent_ids({"it_asset_type"},{entity.Id})");
            //};
            //onAfterUpdate = (entity, dto) =>
            //{
            //    dbContext.Database.ExecuteSqlCommand($"call proc_sync_tree_parent_ids({"it_asset_type"},{entity.Id})");
            //};
        }
    }
}
