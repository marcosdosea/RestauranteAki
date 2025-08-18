using System.ComponentModel.DataAnnotations;

namespace RestauranteAkiWeb.Models
{
    public class RestauranteViewModel
    {
        [Display(Name = "Código")]
        public int Id { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string Nome { get; set; } = string.Empty;

        [Display(Name = "Nome Fantasia")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string NomeFantasia { get; set; } = string.Empty;

        [Display(Name = "CNPJ")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string Cnpj { get; set; } = string.Empty;

        [Display(Name = "Endereço")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string Endereco { get; set; } = string.Empty;

        [Display(Name = "Bairro")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string Bairro { get; set; } = string.Empty;

        [Display(Name = "Estado")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string Estado { get; set; } = string.Empty;

        [Display(Name = "Cidade")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string Cidade { get; set; } = string.Empty;

        [Display(Name = "Complemento")]
        public string? Complemento { get; set; }

        [Display(Name = "Quantidade de Cardápios")]
        public int QuantidadeCardapios { get; set; }

        [Display(Name = "Quantidade de Pessoas")]
        public int QuantidadePessoas { get; set; }
    }
}
