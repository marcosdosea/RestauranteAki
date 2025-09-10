using Core;
using Core.Service;
using Google.Protobuf.WellKnownTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace Service
{
    public class ItemcardapioService : IItemcardapioService
    {
        private readonly RestauranteAkiContext context;
        private readonly ICardapioService cardapioService;

        public ItemcardapioService(RestauranteAkiContext context, ICardapioService cardapioService)
        {
            this.context = context;
            this.cardapioService = cardapioService;
        }

        public int Create(Itemcardapio itemcardapio, string[] diasSemana)
        {
            // Define os dias da semana selecionados
            itemcardapio.DiaSemana = string.Join(",", diasSemana ?? []);

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

        public IEnumerable<string> GetAllIngredientes()
        {
            var todosIngredientes = context.Itemcardapios.AsNoTracking()// puxa tudo do campo descrição.
                .Where(p => !string.IsNullOrEmpty(p.Descricao))
                .Select(p => p.Descricao)
                .ToList();

            if(todosIngredientes.Count == 0)
            {
                return [];
            }

            var ingredientesUnicos = todosIngredientes
              .SelectMany(d => d.Split(',', StringSplitOptions.RemoveEmptyEntries))// divide em vírgulas, retira espaços em brancos e duplicatas.
              .Select(i => i.Trim())
              .Distinct(StringComparer.CurrentCultureIgnoreCase)
              .OrderBy(i => i);// lista em ordem alfabética.

            return ingredientesUnicos;
        }
    }
}
