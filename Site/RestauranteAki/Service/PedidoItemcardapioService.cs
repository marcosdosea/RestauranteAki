using Core;
using Core.Service;
using Microsoft.EntityFrameworkCore;

namespace Service
{
    public class PedidoItemcardapioService : IPedidoItemcardapioService
    {
        private readonly RestauranteAkiContext context;

        public PedidoItemcardapioService(RestauranteAkiContext context)
        {
            this.context = context;
        }
        public int Create(PedidoItemcardapio pedidoItemcardapio)
        {
            context.Add(pedidoItemcardapio);
            context.SaveChanges();
            return pedidoItemcardapio.IdPedido;
        }

        public void Delete(int id)
        {
            var pedidoitemcardapio = context.PedidoItemcardapios.FirstOrDefault(p => p.IdItemCardapio == id);

            if (pedidoitemcardapio != null) 
            {
                context.Remove(pedidoitemcardapio);
                context.SaveChanges();
            }
        }

        public void Edit(PedidoItemcardapio pedidoItemcardapio)
        {
            context.Update(pedidoItemcardapio);
            context.SaveChanges();
        }

        public PedidoItemcardapio? Get(int id)
        {
            return context.PedidoItemcardapios.FirstOrDefault(x => x.IdItemCardapio == id);
        }

        public IEnumerable<PedidoItemcardapio> GetAll()
        {
            return context.PedidoItemcardapios.AsNoTracking().ToList();
        }
    }
}
