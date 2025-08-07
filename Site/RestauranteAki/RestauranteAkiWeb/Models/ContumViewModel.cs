using Core;
using System.ComponentModel.DataAnnotations;

namespace RestauranteAkiWeb.Models
{
    public class ContumViewModel
    {
        [Display(Name = "Código")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public int Id { get; set; }

        [Display(Name = "Data e Hora do Encerramento")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public DateTime DataHoraEncerramento { get; set; }

        [Display(Name = "Valor Total")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public float Valor { get; set; }

        [Display(Name = "Forma de Pagamento")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string? FormaPagamento { get; set; }

        [Display(Name = "Status")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string Status { get; set; } = string.Empty;

        [Display(Name = "Mesa")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public int IdMesa { get; set; }

        public Mesa? IdMesaNavigation { get; set; }

        public ICollection<Pagamento> Pagamentos { get; set; } = new List<Pagamento>();

        public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
    }
}

