using AutoMapper;
using Core;
using RestauranteAkiWeb.Models;

namespace RestauranteAkiWeb.Mappers
{
    public class PersonagemProfile : Profile
    {
        public PersonagemProfile()
        {
            CreateMap<PersonagemViewModel, Personagem>().ReverseMap();
        }
    }
}
