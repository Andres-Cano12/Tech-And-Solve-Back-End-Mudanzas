using AccessData.Entities;
using AutoMapper;
using Common.Classes.BussinesLogic;
using System.Collections.Generic;

namespace App.Config.Dependencies
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Move, MoveDTO>().ReverseMap().ForMember(d => d.MoveDetail, o => o.MapFrom(s => s.MoveDetailDTO));
            CreateMap<MoveDetail, MoveDetailDTO>().ReverseMap();
         }
    }
}