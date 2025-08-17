namespace Core.Service
{
    public interface IPedidoItemcardapioService
    {
        int Create(PedidoItemcardapio pedidoItemcardapio);
        void Edit(PedidoItemcardapio pedidoItemcardapio);
        void Delete(int id);
        PedidoItemcardapio? Get(int id);
        IEnumerable<PedidoItemcardapio> GetAll();
    }
}