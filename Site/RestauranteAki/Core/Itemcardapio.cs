namespace Core;

public partial class Itemcardapio
{
    public int Id { get; set; }

    public string Nome { get; set; } = null!;

    public float PrecoUnitario { get; set; }

    public int Porcao { get; set; }

    public string DiaSemana { get; set; } = null!;

    public bool Status { get; set; }

    public string? Descricao { get; set; }

    public int Categoria { get; set; }

    public byte[] Imagem { get; set; } = null!;

    public virtual ICollection<PedidoItemcardapio> PedidoItemcardapios { get; set; } = new List<PedidoItemcardapio>();

    public virtual ICollection<Cardapio> IdCardapios { get; set; } = new List<Cardapio>();


}
