using AutoMapper;
using RestauranteAkiWeb.Models;
using Core;
namespace RestauranteAkiWeb.Mappers
{
    public class PagamentoProfile : Profile
    {
        public PagamentoProfile()
        {
            CreateMap<PagamentoViewModel, Pagamento>().ReverseMap();

        }
    }

}