using Core;
using Core.Service;

namespace Service
{
    public class ContumService : IContum
    {
        public int Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateTime DataHoraEncerramento { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public float Valor { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string FormaPagamento { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Status { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int IdMesa { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Mesa IdMesaNavigation { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ICollection<Pagamento> Pagamentos { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ICollection<Pedido> Pedidos { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
