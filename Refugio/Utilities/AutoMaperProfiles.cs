using AutoMapper;
using Refugio.DTOs;
using Refugio.Entities;

namespace Refugio.Utilities
{
    public class AutoMaperProfiles: Profile
    {
        public AutoMaperProfiles() 
        {
            CreateMap<MascotaDTO, Mascota>();
            CreateMap<Mascota, MascotaDTO>()
                .ForMember(dest => dest.Tamaño, opt => opt.MapFrom(src => src.Tamaño))
                .ForMember(dest => dest.Genero, opt => opt.MapFrom(src => src.Genero))
                .ForMember(dest => dest.Especie, opt => opt.MapFrom(src => src.Especie));

            CreateMap<TamañoDTO, Tamaño>();
            CreateMap<Tamaño, TamañoDTO>();

            CreateMap<GeneroDTO, Genero>();
            CreateMap<Genero, GeneroDTO>();

            CreateMap<EspecieDTO, Especie>();
            CreateMap<Especie, EspecieDTO>();
        }
    }
}
