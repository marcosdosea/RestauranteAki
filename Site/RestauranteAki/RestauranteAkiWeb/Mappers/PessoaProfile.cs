using AutoMapper;
using Core;
using RestauranteAkiWeb.Models;

namespace RestauranteAkiWeb.Mappers
{
    public class PessoaProfile : Profile
    {
        public PessoaProfile()
        {
            CreateMap<Pessoa, PessoaViewModel>()
                .ForMember(
                    dest => dest.TipoPessoa,
                    opt => opt.MapFrom(src =>
                        !string.IsNullOrEmpty(src.TipoPessoa) ? (TipoPessoa)src.TipoPessoa[0] : default
                    )
                )
                .ForMember(
                    dest => dest.Foto,
                    opt => opt.Ignore()
                );

            CreateMap<PessoaViewModel, Pessoa>()
                .ForMember(
                    dest => dest.TipoPessoa,
                    opt => opt.MapFrom(src =>
                        ((char)src.TipoPessoa).ToString()
                    )
                )
                .ForMember(
                    dest => dest.Foto,
                    opt => opt.Ignore()
                );
        }
    }
}
