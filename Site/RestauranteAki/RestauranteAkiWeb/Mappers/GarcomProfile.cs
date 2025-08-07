using AutoMapper;
using RestauranteAkiWeb.Models;
using Core;
namespace RestauranteAkiWeb.Mappers
{
    public class GarcomProfile : Profile
    {
        public GarcomProfile()
        {
            CreateMap<GarcomViewModel, Garcom>().ReverseMap();

        }
    }

}