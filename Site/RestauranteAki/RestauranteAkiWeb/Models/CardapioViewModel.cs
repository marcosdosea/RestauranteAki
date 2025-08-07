using Core;
using System.ComponentModel.DataAnnotations;

namespace RestauranteAkiWeb.Models
{
    public class CardapioViewModel
    {
        [Display(Name = "Código")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public int Id { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string? Nome { get; set; }

        [Display(Name = "Data de Início")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public DateTime DataInicio { get; set; }

        [Display(Name = "Data de Fim")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public DateTime DataFim { get; set; }

        [Display(Name = "Ativo")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public sbyte Ativo { get; set; }

        [Display(Name = "Restaurante")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public int IdRestaurante { get; set; }

        public Restaurante? IdRestauranteNavigation { get; set; }

        public ICollection<Itemcardapio>? IdItemCardapios { get; set; }
    }
}
