using Core;
using Core.Service;

namespace Service
{
    public class PedidoService : IPedido
    {
        public int Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string? Status { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int IdConta { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int IdMesa { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int IdPersonagem { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int IdPessoa { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Contum IdContaNavigation { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Mesa IdMesaNavigation { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Personagem IdPersonagemNavigation { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Pessoa IdPessoaNavigation { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ICollection<PedidoItemcardapio> PedidoItemcardapios { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
