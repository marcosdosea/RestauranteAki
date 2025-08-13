using Core;
using Core.Service;

namespace Service
{
    public class PessoaService : IPessoa
    {
        public int Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string NomeCompleto { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Cpf { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string? Email { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Telefone { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateTime DataNascimento { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Cep { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Logradouro { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Bairro { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Cidade { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Estado { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string? Complemento { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public byte[]? Foto { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int IdRestaurante { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string TipoPessoa { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Restaurante IdRestauranteNavigation { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ICollection<Pedido> Pedidos { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
