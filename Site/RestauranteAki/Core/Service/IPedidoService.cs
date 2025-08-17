namespace Core.Service
{
    public interface IPedidoService
    {
        int Create(Pedido pedido);
        void Edit(Pedido pedido);
        void Delete(int id);
        Pedido? Get(int id);
        IEnumerable<Pedido> GetAll();
    }
}