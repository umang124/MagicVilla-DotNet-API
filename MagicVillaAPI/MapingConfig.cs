using AutoMapper;
using MagicVillaAPI.Models;
using MagicVillaAPI.Models.Dto;

namespace MagicVillaAPI
{
    public class MapingConfig : Profile
    {
        public MapingConfig()
        {
            CreateMap<Villa, VillaDTO>().ReverseMap();
        }
    }
}
