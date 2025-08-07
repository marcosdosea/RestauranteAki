using System.ComponentModel.DataAnnotations;
using Core;

namespace RestauranteAkiWeb.Models
{
    public class RestauranteViewModel
    {
        [Display(Name = "Código")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public int Id { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string? Nome { get; set; } 

        [Display(Name = "Nome Fantasia")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string? NomeFantasia { get; set; }

        [Display(Name = "CNPJ")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string? Cnpj { get; set; } 

        [Display(Name = "Endereço")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string? Endereco { get; set; } 

        [Display(Name = "Bairro")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string? Bairro { get; set; } 

        [Display(Name = "Estado")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string? Estado { get; set; } 

        [Display(Name = "Cidade")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string? Cidade { get; set; } 

        [Display(Name = "Complemento")]
        public string? Complemento { get; set; }

        public ICollection<Cardapio>? Cardapios { get; set; } 

        public ICollection<Pessoa>? Pessoas { get; set; } 
    }
}
