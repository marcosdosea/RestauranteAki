using AutoMapper;
using Core;
using RestauranteAkiWeb.Models;

namespace RestauranteAkiWeb.Mappers
{
    public class PessoaProfile : Profile
    {
        public PessoaProfile() 
        {
            CreateMap<PessoaViewModel, Pessoa>().ReverseMap();
        }
    }
}
