using System.ComponentModel.DataAnnotations;

namespace RestauranteAkiWeb.Models
{
    public class ItemcardapioViewModel
    {
        [Display(Name = "Código")]
        public int Id { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string Nome { get; set; }

        [Display(Name = "Descrição")]
        public string? Descricao { get; set; }

        [Display(Name = "Preço Unitário")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [DataType(DataType.Currency)]
        public decimal PrecoUnitario { get; set; }

        [Display(Name = "Porção")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public int Porcao { get; set; }

        [Display(Name = "Dia da Semana")]
        public string? DiaSemana { get; set; }

        [Display(Name = "Status")]
        public bool Status { get; set; }

        [Display(Name = "Imagem")]
        public string? ImagemUrl { get; set; }

        [Display(Name = "Categoria")]
        public string CategoriaNome { get; set; }
    }
}
