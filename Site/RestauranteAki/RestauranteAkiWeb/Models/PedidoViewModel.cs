using System.ComponentModel.DataAnnotations;

namespace RestauranteAkiWeb.Models
{
    public class PedidoViewModel
    {
        [Display(Name = "Código")]
        public int Id { get; set; }

        /// <summary>
        /// Status:
        /// P - Pronto
        /// S - Solicitado
        /// E - Entregue
        /// </summary>
        [Display(Name = "Status")]
        public string Status { get; set; }

        [Display(Name = "Conta")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public int IdConta { get; set; }

        [Display(Name = "Mesa")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public int IdMesa { get; set; }

        [Display(Name = "Personagem")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public int IdPersonagem { get; set; }

        [Display(Name = "Pessoa")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public int IdPessoa { get; set; }

        [Display(Name = "Número da Mesa")]
        public string? NumeroMesa { get; set; }

        [Display(Name = "Nome da Pessoa")]
        public string? NomePessoa { get; set; }

        [Display(Name = "Nome do Personagem")]
        public string? NomePersonagem { get; set; }

        [Display(Name = "Itens do Pedido")]
        public ICollection<PedidoItemcardapioViewModel>? ItensPedido { get; set; }
    }
}
