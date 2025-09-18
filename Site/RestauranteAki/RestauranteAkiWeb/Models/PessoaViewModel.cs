using System.ComponentModel.DataAnnotations;

namespace RestauranteAkiWeb.Models
{
    public enum TipoPessoa
    {
        Funcionario = 'F',
        Gestor = 'G'
    }

    public class PessoaViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Nome Completo")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(50)]
        public string NomeCompleto { get; set; } = string.Empty;

        [Display(Name = "CPF")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(14)]
        public string Cpf { get; set; } = string.Empty;

        [Display(Name = "Data de Nascimento")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [DataType(DataType.Date)]
        public DateTime DataNascimento { get; set; }

        [Display(Name = "E-mail")]
        [EmailAddress(ErrorMessage = "O e-mail informado não é válido.")]
        [StringLength(50)]
        public string? Email { get; set; }

        [Display(Name = "Telefone")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(50)]
        public string Telefone { get; set; } = string.Empty;

        [Display(Name = "CEP")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(20)]
        public string Cep { get; set; } = string.Empty;

        [Display(Name = "Logradouro")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(50)]
        public string Logradouro { get; set; } = string.Empty;

        [Display(Name = "Bairro")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(50)]
        public string Bairro { get; set; } = string.Empty;

        [Display(Name = "Cidade")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(50)]
        public string Cidade { get; set; } = string.Empty;

        [Display(Name = "Estado")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(50)]
        public string Estado { get; set; } = string.Empty;

        [Display(Name = "Complemento")]
        [StringLength(50)]
        public string? Complemento { get; set; }

        [Display(Name = "Foto")]
        public IFormFile? Foto { get; set; }

        [Display(Name = "Restaurante associado")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public int IdRestaurante { get; set; }

        [Display(Name = "Tipo de Pessoa")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public TipoPessoa TipoPessoa { get; set; }
    }
}