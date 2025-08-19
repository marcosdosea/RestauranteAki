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

        [Display(Name = "Número da Mesa")]
        public string? NumeroMesa { get; set; }

        public ICollection<PagamentoViewModel>? Pagamentos { get; set; }
        public ICollection<PedidoViewModel>? Pedidos { get; set; }
    }
}

namespace RestauranteAkiWeb.Models
{
    public class PagamentoViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Forma de Pagamento")]
        public string? FormaPagamento { get; set; }

        [Display(Name = "Valor")]
        public float Valor { get; set; }

        [Display(Name = "Data")]
        public DateTime DataPagamento { get; set; }
    }
}

namespace RestauranteAkiWeb.Models
{
    public class PedidoViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Descrição do Pedido")]
        public string? Descricao { get; set; }

        [Display(Name = "Valor Total")]
        public float ValorTotal { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; } = string.Empty;
    }
}
