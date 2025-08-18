using System.ComponentModel.DataAnnotations;

namespace RestauranteAkiWeb.Models
{
    public class PersonagemViewModel
    {
        [Display(Name = "Código")]
        public int Id { get; set; }

        [Display(Name = "Cor do Identificador")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string IdentificadorCor { get; set; }

        [Display(Name = "Data de Criação")]
        public DateTime DataCriacao { get; set; }

        [Display(Name = "Data de Atualização")]
        public DateTime DataAtualizacao { get; set; }

        [Display(Name = "Quantidade de Pedidos")]
        public int QuantidadePedidos { get; set; }

        [Display(Name = "Total de Pagamentos")]
        public decimal TotalPagamentos { get; set; }
    }
}
