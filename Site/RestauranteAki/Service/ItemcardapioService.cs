using Core;
using Core.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace Service
{
    public class ItemcardapioService : IItemcardapioService
    {
        private readonly RestauranteAkiContext context;

        public ItemcardapioService(RestauranteAkiContext context)
        {
            this.context = context;
        }

        public int Create(Itemcardapio itemcardapio)
        {
            context.Add(itemcardapio);
            context.SaveChanges();
            return itemcardapio.Id;
        }

        public void Delete(int id)
        {
            var itemCardapio = context.Itemcardapios.Find(id);
            if (itemCardapio == null)
            {
                throw new ArgumentException("Usuário não encontrado");
            }
            context.Remove(itemCardapio);
            context.SaveChanges();
        }

        public void Edit(Itemcardapio itemcardapio)
        {
            context.Update(itemcardapio);
            context.SaveChanges();
        }

        public Itemcardapio? Get(int id)
        {
            return context.Itemcardapios.Find(id);
        }

        public IEnumerable<Itemcardapio> GetAll()
        {
            return context.Itemcardapios.AsNoTracking();
        }
    }
}
