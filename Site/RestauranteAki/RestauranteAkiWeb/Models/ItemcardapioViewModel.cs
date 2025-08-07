using System.ComponentModel.DataAnnotations;
using Core;

namespace RestauranteAkiWeb.Models
{
    public class ItemcardapioViewModel
    {
        [Display(Name = "Código")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public int Id { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string? Nome { get; set; } 

        [Display(Name = "Descrição")]
        public string? Descricao { get; set; }

        [Display(Name = "Preço Unitário")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public float PrecoUnitario { get; set; }

        [Display(Name = "Porção")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public int Porcao { get; set; }

        [Display(Name = "Dia da Semana")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string? DiaSemana { get; set; } 

        [Display(Name = "Status")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public bool Status { get; set; }

        [Display(Name = "Imagem")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public byte[]? Imagem { get; set; } 

        [Display(Name = "Categoria")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public int Categoria { get; set; }

        public ICollection<PedidoItemcardapio> PedidoItemcardapios { get; set; } = new List<PedidoItemcardapio>();

        public ICollection<Cardapio> IdCardapios { get; set; } = new List<Cardapio>();
    }
}
