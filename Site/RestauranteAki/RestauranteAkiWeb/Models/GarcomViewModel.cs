using System.ComponentModel.DataAnnotations;

namespace RestauranteAkiWeb.Models
{
    public class GarcomViewModel
    {
        [Display(Name = "Código")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public int Id { get; set; }
    }
}
