using System.ComponentModel.DataAnnotations;
using Core;

namespace RestauranteAkiWeb.Models
{
    public class MesaViewModel
    {
        [Display(Name = "Código")]
        public int Id { get; set; }

        [Display(Name = "Imagem")]
        public byte[]? Imagem { get; set; }

        [Display(Name = "Contas")]
        public ICollection<Contum> Conta { get; set; } = new List<Contum>();

        [Display(Name = "Pedidos")]
        public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
    }
}
