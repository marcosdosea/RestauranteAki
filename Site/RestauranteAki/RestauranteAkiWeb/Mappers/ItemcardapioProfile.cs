using AutoMapper;
using RestauranteAkiWeb.Models;
using Core;

namespace RestauranteAkiWeb.Mappers
{
    public class ItemcardapioProfile : Profile
    {
        public ItemcardapioProfile()
        {
            // Mapeamento do ViewModel para o Model (usado no Create/Edit POST)
            CreateMap<ItemcardapioViewModel, Itemcardapio>()
                // Converte a lista de strings 'DiasSemana' em uma única string separada por vírgula
                .ForMember(dest => dest.DiaSemana,
                           opt => opt.MapFrom(src => string.Join(",", src.DiasSemana)))
                // Ignora o campo de Imagem, pois será tratado manualmente no Controller
                .ForMember(dest => dest.Imagem, opt => opt.Ignore());

            // Mapeamento do Model para o ViewModel (usado no Edit GET)
            CreateMap<Itemcardapio, ItemcardapioViewModel>()
                // Converte a string 'DiaSemana' em uma lista de strings
                .ForMember(dest => dest.DiasSemana,
                           opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.DiaSemana)
                                                ? src.DiaSemana.Split(',', System.StringSplitOptions.RemoveEmptyEntries).ToList()
                                                : new List<string>()))
                // Ignora o campo de upload e o campo de URL, pois serão tratados no Controller
                .ForMember(dest => dest.ImagemUpload, opt => opt.Ignore())
                .ForMember(dest => dest.ImagemAtual, opt => opt.Ignore());
        }
    }
}