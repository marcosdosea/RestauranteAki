using System.ComponentModel.DataAnnotations;

namespace RestauranteAkiWeb.Models
{
    public class PagamentoViewModel
    {
        [Display(Name = "Código")]
        public int Id { get; set; }

        /// <summary>
        /// C - Cartão
        /// D - Dinheiro
        /// P - Pix
        /// </summary>
        [Display(Name = "Tipo de Pagamento")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string TipoPagamento { get; set; }

        [Display(Name = "Data e Hora")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public DateTime DataHora { get; set; }

        [Display(Name = "Valor do Pagamento")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [DataType(DataType.Currency)]
        public decimal ValorPagamento { get; set; }

        [Display(Name = "Conta")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public int IdConta { get; set; }

        [Display(Name = "Personagem")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public int IdPersonagem { get; set; }

        [Display(Name = "Nome do Personagem")]
        public string? NomePersonagem { get; set; }

        [Display(Name = "Número da Conta")]
        public string? NumeroConta { get; set; }
    }
}
