using Core;
using Core.Service;
using Microsoft.EntityFrameworkCore;

namespace Service
{
    public class MesaService : IMesaService
    {
        private readonly RestauranteAkiContext context;

        public MesaService(RestauranteAkiContext context)
        {
            this.context = context;
        }
        public int Create(Mesa mesa)
        {
            context.Add(mesa);
            context.SaveChanges();
            return mesa.Id;
        }

        public void Delete(int id)
        {
            var mesa = context.Mesas.Find(id);
           if (mesa == null)
            {
                throw new ArgumentException("Mesa não encontrada");
            }
            context.Mesas.Remove(mesa);
            context.SaveChanges();
        }

        public void Edit(Mesa mesa)
        {
            context.Update(mesa);
            context.SaveChanges();
        }

        public Mesa? Get(int id)
        {
            return context.Mesas.Find(id);
        }

        public IEnumerable<Mesa> GetAll()
        {
            return context.Mesas.AsNoTracking();
        }
    }
}
