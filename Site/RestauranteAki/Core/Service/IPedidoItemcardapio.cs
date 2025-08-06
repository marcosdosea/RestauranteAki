using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service
{
    public interface IPedidoItemcardapio
    {
        public int IdPedido { get; set; }

        public int IdItemCardapio { get; set; }

        public int? Quantidade { get; set; }

        public Itemcardapio IdItemCardapioNavigation { get; set; }

        public Pedido IdPedidoNavigation { get; set; } 
    }
}
