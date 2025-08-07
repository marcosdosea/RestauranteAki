using AutoMapper;
using Core;
using RestauranteAkiWeb.Models;

namespace RestauranteAkiWeb.Mappers
{
    public class PedidoProfile : Profile
    {
        public PedidoProfile()
        {
            CreateMap<PedidoViewModel, Pedido>().ReverseMap();
        }
    }
}
