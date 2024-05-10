using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using CaiAdmin.Dto;
using CaiAdmin.Entity;

namespace CaiAdmin.MapperConfiguration.Hr
{
    public class AttachProfile : Profile
    {
        public AttachProfile()
        {
            CreateMap<Attach, AttachDto>()
                .ForMember(dest => dest.PublicPath, opt => opt.MapFrom(src => src.IsPublic ? src.Path : ""));
        }
    }
}
