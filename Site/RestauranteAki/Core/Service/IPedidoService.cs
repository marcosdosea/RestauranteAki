using Core.Dto;

namespace Core.Service
{
    public interface IPedidoService
    {
        int Create(Pedido pedido);
        Task<bool> CriarPedidoAsync(PedidoSubmissionDto dto);
        void Edit(Pedido pedido);
        void Delete(int id);
        Pedido? Get(int id);
        IEnumerable<Pedido> GetAll();
    }
}