using Core;
using Core.Service;

namespace Service
{
    public class ItemcardapioService : IItemcardapio
    {
        public int Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Nome { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string? Descricao { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public float PrecoUnitario { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int Porcao { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string DiaSemana { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool Status { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public byte[] Imagem { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int Categoria { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ICollection<PedidoItemcardapio> PedidoItemcardapios { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ICollection<Cardapio> IdCardapios { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
