using System.ComponentModel.DataAnnotations;
using Core;

namespace RestauranteAkiWeb.Models
{
    public class PedidoViewModel
    {
        [Display(Name = "Código")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public int Id { get; set; }

        /// <summary>
        /// Status:
        /// P - Pronto
        /// S - Solicitado
        /// E - Entregue
        /// </summary>
        [Display(Name = "Status")]
        public string? Status { get; set; } 

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

        public Contum? IdContaNavigation { get; set; }

        public Mesa? IdMesaNavigation { get; set; }

        public Personagem? IdPersonagemNavigation { get; set; }

        public Pessoa? IdPessoaNavigation { get; set; }

        public ICollection<PedidoItemcardapio>? PedidoItemcardapios { get; set; } 
    }
}
