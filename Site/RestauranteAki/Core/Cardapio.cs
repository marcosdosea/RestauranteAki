namespace Core;

public partial class Cardapio
{
    public int Id { get; set; }

    public string Nome { get; set; } = null!;

    public sbyte Ativo { get; set; }

    public DateTime DataInicio { get; set; }

    public DateTime DataFim { get; set; }

    public int IdRestaurante { get; set; }

    public virtual Restaurante IdRestauranteNavigation { get; set; } = null!;

    public virtual ICollection<Itemcardapio> IdItemCardapios { get; set; } = new List<Itemcardapio>();


}
