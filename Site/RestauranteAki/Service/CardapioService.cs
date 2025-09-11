using Core;
using Core.Service;
using Microsoft.EntityFrameworkCore;

namespace Service
{
    public class CardapioService : ICardapioService
    {
        private readonly RestauranteAkiContext context;

        public CardapioService(RestauranteAkiContext context)
        {
            this.context = context;
        }
        /// <summary>
        /// Insere um cardapio no banco de dados.
        /// </summary>
        /// <param name="cardapio">Cardapio a ser inserido.</param>
        /// <returns>Id do Cardapio inserido.</returns>
        public int Create(Cardapio cardapio)
        {
            context.Cardapios.Add(cardapio);
            context.SaveChanges();
            return cardapio.Id;
        }

        /// <summary>
        /// Remove um cardapio do banco de dados.
        /// </summary>
        /// <param name="id">Id do cardapio a ser removido.</param>

        public void Delete(int id)
        {
            var cardapio = context.Cardapios.Find(id);
            context.Cardapios.Remove(cardapio);
            context.SaveChanges();
        }

        /// <summary>
        /// edita um Cardapio no banco de dados.
        /// </summary>
        /// <param name="cardapio">Cardapio a ser editado.</param>
        public void Edit(Cardapio cardapio)
        {
            context.Cardapios.Update(cardapio);
            context.SaveChanges();
        }

        /// <summary>
        /// busca um cardapio no banco de dados.
        /// </summary>
        /// <param name="id">Id do cardapio a ser buscado.</param>
        /// <returns>Cardapio encontrado ou null.</returns>
        public Cardapio? Get(int id)
        {

            return context.Cardapios
                .Include(c => c.IdItemCardapios)
                .FirstOrDefault(c => c.Id == id);
        }

        /// <summary>
        /// Obtém todos os Cardapio do banco de dados.
        /// </summary>
        /// <returns>Lista de Cardapios.</returns>
        public IEnumerable<Cardapio> GetAll()
        {
            return context.Cardapios.AsNoTracking();
        }

        public IEnumerable<Cardapio> GetByNome(string nome)
        {
            return context.Cardapios.AsNoTracking().Where(c => c.Nome == nome);
        }
    }
}
