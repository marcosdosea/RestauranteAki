using Core;
using Core.Service;
using Microsoft.EntityFrameworkCore;

namespace Service
{
    /// <summary>
    /// Implementa serviços para manter dados da mesa
    /// </summary>
    public class MesaService : IMesaService
    {
        private readonly RestauranteAkiContext context;

        public MesaService(RestauranteAkiContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Criar uma nova mesa na base de dados
        /// </summary>
        /// <param name="mesa">dados da mesa</param>
        /// <returns>id da mesa</returns>
        public int Create(Mesa mesa)
        {
            context.Add(mesa);
            context.SaveChanges();
            return mesa.Id;
        }

        /// <summary>
        /// Remover a mesa da base de dados
        /// </summary>
        /// <param name="id">id da mesa</param>
        public void Delete(int id)
        {
            var mesa = context.Mesas.Find(id);
            if (mesa != null)
            {
                context.Remove(mesa);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Editar dados da mesa na base de dados
        /// </summary>
        /// <param name="mesa">dados da mesa</param>
        public void Edit(Mesa mesa)
        {
            context.Update(mesa);
            context.SaveChanges();
        }

        /// <summary>
        /// Buscar uma mesa na base de dados
        /// </summary>
        /// <param name="id">id da mesa</param>
        /// <returns>dados da mesa</returns>
        public Mesa? Get(int id)
        {
            return context.Mesas
                .Include(m => m.Pedidos)
                .FirstOrDefault(m => m.Id == id);
        }

        /// <summary>
        /// Buscar todas as mesas cadastradas
        /// </summary>
        /// <returns>lista de mesas</returns>
        public IEnumerable<Mesa> GetAll()
        {
            return context.Mesas.AsNoTracking();
        }
    }
}
