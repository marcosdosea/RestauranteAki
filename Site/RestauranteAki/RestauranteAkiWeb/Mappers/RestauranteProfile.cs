using AutoMapper;
using Core;
using RestauranteAkiWeb.Models;

namespace RestauranteAkiWeb.Mappers
{
    public class RestauranteProfile : Profile
    {
        public RestauranteProfile()
        {
            CreateMap<RestauranteViewModel, Restaurante>().ReverseMap();
        }
    }
}
