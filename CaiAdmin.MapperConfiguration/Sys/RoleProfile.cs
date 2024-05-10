using AutoMapper;
using System.Linq;
using CaiAdmin.Dto.Sys;
using CaiAdmin.Entity.Sys;

namespace CaiAdmin.MapperConfiguration.Sys
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<RoleSaveDto, Role>().AfterMap((src, dest) =>
            {
                dest.RoleMenus.ForEach(r =>
                {
                    r.RoleId = dest.Id;
                });
            });
        }
    }
}
