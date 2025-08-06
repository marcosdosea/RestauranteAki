using Core;
using Core.Service;

namespace Service
{
    public class MesaService : IMesa
    {
        public int Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public byte[]? Imagem { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ICollection<Contum> Conta { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ICollection<Pedido> Pedidos { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
