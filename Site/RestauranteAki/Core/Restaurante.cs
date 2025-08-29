namespace Core;

public partial class Restaurante
{
    public int Id { get; set; }

    public string Nome { get; set; } = null!;

    public string NomeFantasia { get; set; } = null!;

    public string Cnpj { get; set; } = null!;

    public string? Cep { get; set; }

    public string Logradouro { get; set; } = null!;

    public int Numero { get; set; }

    public string Bairro { get; set; } = null!;

    public string Estado { get; set; } = null!;

    public string Cidade { get; set; } = null!;

    public string? Complemento { get; set; }

    public virtual ICollection<Cardapio> Cardapios { get; set; } = new List<Cardapio>();

    public virtual ICollection<Pessoa> Pessoas { get; set; } = new List<Pessoa>();


}
