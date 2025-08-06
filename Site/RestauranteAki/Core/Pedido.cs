using System;
using System.Collections.Generic;

namespace Core;

public partial class Pedido
{
    public int Id { get; set; }

    /// <summary>
    /// status
    /// P - Pronto
    /// S - Solicitado
    /// E - Entregue
    /// </summary>
    public string? Status { get; set; }

    public int IdConta { get; set; }

    public int IdMesa { get; set; }

    public int IdPersonagem { get; set; }

    public int IdPessoa { get; set; }

    public virtual Contum IdContaNavigation { get; set; } = null!;

    public virtual Mesa IdMesaNavigation { get; set; } = null!;

    public virtual Personagem IdPersonagemNavigation { get; set; } = null!;

    public virtual Pessoa IdPessoaNavigation { get; set; } = null!;

    public virtual ICollection<PedidoItemcardapio> PedidoItemcardapios { get; set; } = new List<PedidoItemcardapio>();
}
