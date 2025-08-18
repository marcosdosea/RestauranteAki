using System.ComponentModel.DataAnnotations;

namespace RestauranteAkiWeb.Models
{
    public class PedidoItemcardapioViewModel
    {
        [Display(Name = "Pedido")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public int IdPedido { get; set; }

        [Display(Name = "Item do Cardápio")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public int IdItemCardapio { get; set; }

        [Display(Name = "Quantidade")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public int Quantidade { get; set; }

        [Display(Name = "Nome do Item")]
        public string? NomeItemCardapio { get; set; }

        [Display(Name = "Número do Pedido")]
        public string? NumeroPedido { get; set; }
    }
}
