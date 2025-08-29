namespace Core;

public partial class Contum
{
    public int Id { get; set; }

    public float Valor { get; set; }

    public string FormaPagamento { get; set; } = null!;

    public string Status { get; set; } = null!;

    public DateTime DataHoraEncerramento { get; set; }

    public int IdMesa { get; set; }

    public virtual Mesa IdMesaNavigation { get; set; } = null!;

    public virtual ICollection<Pagamento> Pagamentos { get; set; } = new List<Pagamento>();

    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();


}
