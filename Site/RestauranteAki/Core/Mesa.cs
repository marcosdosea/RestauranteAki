using System;
using System.Collections.Generic;

namespace Core;

public partial class Mesa
{
    public int Id { get; set; }

    public byte[]? Imagem { get; set; }

    public virtual ICollection<Contum> Conta { get; set; } = new List<Contum>();

    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
}
