using System;
using System.Collections.Generic;

namespace Core;

public partial class PedidoItemcardapio
{
    public int IdPedido { get; set; }

    public int IdItemCardapio { get; set; }

    public int? Quantidade { get; set; }

    public virtual Itemcardapio IdItemCardapioNavigation { get; set; } = null!;

    public virtual Pedido IdPedidoNavigation { get; set; } = null!;
}
