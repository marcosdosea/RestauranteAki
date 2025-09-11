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

        // Pseudocódigo detalhado:
        // 1. No método Create, ao verificar cardapiosExistentes, preciso tratar o campo DiaSemana do itemcardapio.
        // 2. Se DiaSemana contém vírgula, significa que há mais de um dia.
        // 3. Dividir DiaSemana em uma lista de dias.
        // 4. Para cada cardápio ativo, verificar se o Nome do cardápio está presente na lista de dias.
        // 5. Se sim, adicionar o itemcardapio ao cardápio se ainda não estiver presente.

        public int Create(Itemcardapio itemcardapio)
        {
            context.Add(itemcardapio);
            context.SaveChanges();

            var cardapiosExistentes = context.Cardapios
                .Where(cardapios => cardapios.Ativo == 1)
                .ToList();

            // Tratar DiaSemana para múltiplos dias separados por vírgula
            var diasSemana = itemcardapio.DiaSemana.Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(d => d.Trim());

            foreach (var cardapio in cardapiosExistentes)
            {
                if (diasSemana.Contains(cardapio.Nome, StringComparer.OrdinalIgnoreCase))
                {
                    if (!cardapio.IdItemCardapios.Any(itemCardapio => itemCardapio.Id == itemcardapio.Id))
                    {
                        cardapio.IdItemCardapios.Add(itemcardapio);
                        context.Cardapios.Update(cardapio);
                    }
                }
            }
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
