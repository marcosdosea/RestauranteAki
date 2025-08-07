using System.ComponentModel.DataAnnotations;
using Core;

namespace RestauranteAkiWeb.Models
{
    public class PessoaViewModel
    {
        [Display(Name = "Código")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public int Id { get; set; }

        [Display(Name = "Nome Completo")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string NomeCompleto { get; set; } = string.Empty;

        [Display(Name = "CPF")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string Cpf { get; set; } = string.Empty;

        [Display(Name = "E-mail")]
        public string? Email { get; set; }

        [Display(Name = "Telefone")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string Telefone { get; set; } = string.Empty;

        [Display(Name = "Data de Nascimento")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public DateTime DataNascimento { get; set; }

        [Display(Name = "CEP")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string Cep { get; set; } = string.Empty;

        [Display(Name = "Logradouro")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string Logradouro { get; set; } = string.Empty;

        [Display(Name = "Bairro")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string Bairro { get; set; } = string.Empty;

        [Display(Name = "Cidade")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string Cidade { get; set; } = string.Empty;

        [Display(Name = "Estado")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string Estado { get; set; } = string.Empty;

        [Display(Name = "Complemento")]
        public string? Complemento { get; set; }

        [Display(Name = "Foto")]
        public byte[]? Foto { get; set; }

        [Display(Name = "Restaurante")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public int IdRestaurante { get; set; }

        /// <summary>
        /// F - Funcionário
        /// G - Gestor
        /// </summary>
        [Display(Name = "Tipo de Pessoa")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string TipoPessoa { get; set; } = string.Empty;

        public Restaurante? IdRestauranteNavigation { get; set; }

        public ICollection<Pedido>? Pedidos { get; set; } 
    }
}
