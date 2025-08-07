using AutoMapper;
using RestauranteAkiWeb.Models;
using Core;
namespace RestauranteAkiWeb.Mappers
{
    public class CardapioProfile : Profile
    {
        public CardapioProfile()
        {
            CreateMap<CardapioViewModel, Cardapio>().ReverseMap();

        }
    }

}

