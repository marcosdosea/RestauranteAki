using AutoMapper;
using RestauranteAkiWeb.Models;
using Core;
namespace RestauranteAkiWeb.Mappers
{
    public class MesaProfile : Profile
    {
        public MesaProfile()
        {
            CreateMap<MesaViewModel, Mesa>().ReverseMap();

        }
    }

}