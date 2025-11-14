using System;
using System.Collections.Generic;

namespace Core;

public partial class Personagem
{
    public int Id { get; set; }

    public string IdentificadorCor { get; set; } = null!;

    public DateTime DataCriacao { get; set; }

    public DateTime DataAtualizacao { get; set; }

    public int IdConta { get; set; }

    public virtual Contum IdContaNavigation { get; set; } = null!;

    public virtual ICollection<Pagamento> Pagamentos { get; set; } = new List<Pagamento>();

    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
}
