using AutoMapper;
using Core;
using Core.Dto;
using RestauranteAkiWeb.Models;

namespace RestauranteAkiWeb.Mappers
{
    public class PedidoProfile : Profile
    {
        public PedidoProfile()
        {
            CreateMap<PedidoViewModel, Pedido>().ReverseMap();
            CreateMap<NovoPedidoViewModel, NovoPedidoDto>().ReverseMap();
            CreateMap<ItemCardapioQuantidadeDto, ItemCardapioQuantidadeViewModel>().ReverseMap();
        }
    }
}
