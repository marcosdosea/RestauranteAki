using System;
using System.Collections.Generic;

namespace Core;

public partial class Pagamento
{
    public int Id { get; set; }

    /// <summary>
    /// C - Cartao
    /// D - Dinheiro
    /// P - Pix
    /// </summary>
    public string TipoPagamento { get; set; } = null!;

    public DateTime DataHora { get; set; }

    public float ValorPagamento { get; set; }

    public int IdConta { get; set; }

    public int IdPersonagem { get; set; }

    public virtual Contum IdContaNavigation { get; set; } = null!;

    public virtual Personagem IdPersonagemNavigation { get; set; } = null!;
}
