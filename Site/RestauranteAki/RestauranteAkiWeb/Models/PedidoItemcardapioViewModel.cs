using System.ComponentModel.DataAnnotations;
using Core;

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
        public int Quantidade { get; set; } 

        public Itemcardapio? IdItemCardapioNavigation { get; set; }

        public Pedido? IdPedidoNavigation { get; set; }
    }
}
 