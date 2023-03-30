using AutoMapper;
using MagicTown_API.Modelos;
using MagicTown_API.Modelos.DTO;

namespace MagicTown_API
{
    public class MappingConfig: Profile
    {
        public MappingConfig()
        {
            CreateMap<Town, TownDTO>();
            CreateMap<TownDTO, Town>();

            CreateMap<Town, TownCreateDTO>().ReverseMap();
            CreateMap<Town, TownUpdateDTO>().ReverseMap();
        }
    }
}
