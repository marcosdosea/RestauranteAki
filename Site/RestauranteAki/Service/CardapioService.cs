using Core;
using Core.Service;

namespace Service
{
    public class CardapioService : ICardapio
    {
        public int Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Nome { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateTime DataInicio { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateTime DataFim { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public sbyte Ativo { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int IdRestaurante { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Restaurante IdRestauranteNavigation { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ICollection<Itemcardapio> IdItemCardapios { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
