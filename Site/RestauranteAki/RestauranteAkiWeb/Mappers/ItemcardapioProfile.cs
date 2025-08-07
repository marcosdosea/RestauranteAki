using AutoMapper;
using RestauranteAkiWeb.Models;
using Core;
namespace RestauranteAkiWeb.Mappers
{
    public class ItemcardapioProfile : Profile
    {
        public ItemcardapioProfile()
        {
            CreateMap<ItemcardapioViewModel, Itemcardapio>().ReverseMap();

        }
    }

}