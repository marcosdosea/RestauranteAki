using Core;
using Core.Service;
using Microsoft.EntityFrameworkCore;

namespace Service
{
    /// <summary>
    /// Implementa serviços para manter dados do pedido
    /// </summary>
    public class PedidoService : IPedidoService
    {
        
        private readonly RestauranteAkiContext context;

        public PedidoService(RestauranteAkiContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Criar um novo pedido na base de dados
        /// </summary>
        /// <param name="pedido">dados do pedido</param>
        /// <returns>id do pedido</returns>
        public int Create(Pedido pedido)
        {
            context.Add(pedido);
            context.SaveChanges();
            return pedido.Id;
        }

        /// <summary>
        /// Remover o pedido da base de dados
        /// </summary>
        /// <param name="id">id do pedido</param>
        public void Delete(int id)
        {
            var pedido = context.Pedidos.Find(id);
            if (pedido != null)
            {
                context.Remove(pedido);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Editar dados do pedido na base de dados
        /// </summary>
        /// <param name="pedido"></param>
        public void Edit(Pedido pedido)
        {
            context.Update(pedido);
            context.SaveChanges();
        }

        /// <summary>
        /// Buscar um pedido na base de dados
        /// </summary>
        /// <param name="id">id do pedido</param>
        /// <returns>dados do pedido</returns>
        public Pedido? Get(int id)
        {
            return context.Pedidos.Find(id);
        }

        /// <summary>
        /// Buscar todos os pedidos cadastrados
        /// </summary>
        /// <returns>lista de pedidos</returns>
        public IEnumerable<Pedido> GetAll()
        {
            return context.Pedidos.AsNoTracking();
        }
    }
}
