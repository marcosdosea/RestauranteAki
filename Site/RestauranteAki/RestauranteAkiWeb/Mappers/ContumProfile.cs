using AutoMapper;
using RestauranteAkiWeb.Models;
using Core;
namespace RestauranteAkiWeb.Mappers
{
    public class ContumProfile : Profile
    {
        public ContumProfile()
        {
            CreateMap<ContumViewModel, Contum>().ReverseMap();

        }
    }

}