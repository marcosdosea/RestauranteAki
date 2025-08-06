using System;
using System.Collections.Generic;

namespace Core;

public partial class Pessoa
{
    public int Id { get; set; }

    public string NomeCompleto { get; set; } = null!;

    public string Cpf { get; set; } = null!;

    public string? Email { get; set; }

    public string Telefone { get; set; } = null!;

    public DateTime DataNascimento { get; set; }

    public string Cep { get; set; } = null!;

    public string Logradouro { get; set; } = null!;

    public string Bairro { get; set; } = null!;

    public string Cidade { get; set; } = null!;

    public string Estado { get; set; } = null!;

    public string? Complemento { get; set; }

    public byte[]? Foto { get; set; }

    public int IdRestaurante { get; set; }

    /// <summary>
    /// F - FUNCIONARIO
    /// G - GESTOR
    /// </summary>
    public string TipoPessoa { get; set; } = null!;

    public virtual Restaurante IdRestauranteNavigation { get; set; } = null!;

    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
}
