using Core;
using Core.Service;

namespace Service
{
    public class RestauranteService : IRestaurante
    {
        public int Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Nome { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string NomeFantasia { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Cnpj { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Endereco { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Bairro { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Estado { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Cidade { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string? Complemento { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ICollection<Cardapio> Cardapios { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ICollection<Pessoa> Pessoas { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
