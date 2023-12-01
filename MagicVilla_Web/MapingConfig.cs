using AutoMapper;
using MagicVilla_Web.Models.Dto;
using MagicVillaAPI.Models;


namespace MagicVilla_Web
{
    public class MapingConfig : Profile
    {
        public MapingConfig()
        {
            CreateMap<Villa, VillaDTO>().ReverseMap();
            CreateMap<VillaNumber, VillaNumberDTO>().ReverseMap();
        }
    }
}
