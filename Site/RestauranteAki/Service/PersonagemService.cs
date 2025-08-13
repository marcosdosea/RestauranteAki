using Core;
using Core.Service;

namespace Service
{
    public class PersonagemService : IPersonagem
    {
        public int Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string IdentificadorCor { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateTime DataCriacao { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateTime DataAtualizacao { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ICollection<Pagamento> Pagamentos { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ICollection<Pedido> Pedidos { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
