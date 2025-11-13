using System.ComponentModel.DataAnnotations;

namespace RestauranteAkiWeb.Models
{
    public class ItemcardapioViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(50)]
        public string Nome { get; set; } = string.Empty;

        [Display(Name = "Ingredientes (Descrição)")]
        public string? Descricao { get; set; }

        [Display(Name = "Preço")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [DataType(DataType.Currency)]
        public float PrecoUnitario { get; set; }

        [Display(Name = "Serve (nº de pessoas)")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [Range(1, int.MaxValue, ErrorMessage = "O campo {0} deve ser no mínimo 1.")]
        public int Porcao { get; set; }

        [Display(Name = "Disponível")]
        public bool Status { get; set; }

        [Display(Name = "Categoria")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string Categoria { get; set; } = string.Empty;

        [Display(Name = "Dias da Semana Disponíveis")]
        public List<string> DiasSemana { get; set; } = new List<string>();

        [Display(Name = "Alterar Foto")]
        public IFormFile? ImagemUpload { get; set; }

        [Display(Name = "Foto Atual")]
        public string? ImagemAtual { get; set; }
    }
}