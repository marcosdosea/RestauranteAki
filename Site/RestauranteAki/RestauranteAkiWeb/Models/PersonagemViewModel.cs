using System.ComponentModel.DataAnnotations;
using Core;

namespace RestauranteAkiWeb.Models
{
    public class PersonagemViewModel
    {
        [Display(Name = "Código")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public int Id { get; set; }

        [Display(Name = "Cor do Identificador")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string? IdentificadorCor { get; set; } 

        [Display(Name = "Data de Criação")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public DateTime DataCriacao { get; set; }

        [Display(Name = "Data de Atualização")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public DateTime DataAtualizacao { get; set; }

        public ICollection<Pagamento>? Pagamentos { get; set; } 

        public ICollection<Pedido>? Pedidos { get; set; } 
    }
}
