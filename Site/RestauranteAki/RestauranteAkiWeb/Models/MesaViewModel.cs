using System.ComponentModel.DataAnnotations;

namespace RestauranteAkiWeb.Models
{
    public class MesaViewModel
    {
        [Display(Name = "Código")]
        public int Id { get; set; }

        [Display(Name = "Imagem")]
        public string? ImagemUrl { get; set; }

        [Display(Name = "Quantidade de Contas")]
        public int QuantidadeContas { get; set; }

        [Display(Name = "Quantidade de Pedidos")]
        public int QuantidadePedidos { get; set; }
    }
}
