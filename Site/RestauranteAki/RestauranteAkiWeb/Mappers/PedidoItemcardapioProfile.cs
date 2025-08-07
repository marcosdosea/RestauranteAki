using AutoMapper;
using Core;
using RestauranteAkiWeb.Models;

namespace RestauranteAkiWeb.Mappers
{
    public class PedidoItemcardapioProfile: Profile
    {
        public PedidoItemcardapioProfile()
        {
            CreateMap<PedidoItemcardapioViewModel, PedidoItemcardapio>().ReverseMap();
        }
    }
  }

