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

        public int Create(Itemcardapio itemcardapio, string[] diasSemana, ICardapioService cardapioService)
        {
            // Define os dias da semana selecionados
            itemcardapio.DiaSemana = string.Join(",", diasSemana ?? Array.Empty<string>());

            // Busca e associa os cardápios existentes conforme os dias selecionados
            var cardapiosAssociados = diasSemana?
                .SelectMany(dia => cardapioService.GetByNome(dia))
                .Distinct()
                .ToList() ?? new List<Cardapio>();

            itemcardapio.IdCardapios = cardapiosAssociados;

            // Anexa os cardápios ao contexto para garantir o rastreamento correto
            foreach (var cardapio in itemcardapio.IdCardapios)
            {
                context.Attach(cardapio);
            }

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
