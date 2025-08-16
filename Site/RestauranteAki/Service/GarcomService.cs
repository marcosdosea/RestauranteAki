using Core;
using Core.Service;

namespace Service
{
    /// <summary>
    /// Manter dados do garçom no banco de dados.
    /// </summary>
    public class GarcomService : IGarcomService
    {
        private readonly RestauranteAkiContext context;

        public GarcomService(RestauranteAkiContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Insere um garçom no banco de dados.
        /// </summary>
        /// <param name="garcom">Garçom a ser inserido.</param>
        /// <returns>Id do garçom inserido.</returns>
        public int Create(Garcom garcom)
        {
            context.Garcons.Add(garcom);
            context.SaveChanges();
            return garcom.Id;
        }

        /// <summary>
        /// Remove um garçom do banco de dados.
        /// </summary>
        /// <param name="id">Id do garçom a ser removido.</param>
        public void Delete(int id)
        {
            var garcom = context.Garcons.Find(id);
            context.Garcons.Remove(garcom);
            context.SaveChanges();
        }
        /// <summary>
        /// edita um garçom no banco de dados.
        /// </summary>
        /// <param name="garcom">Garçom a ser editado.</param>
        public void Edit(Garcom garcom)
        {
            context.Garcons.Update(garcom);
            context.SaveChanges();
        }
        /// <summary>
        /// busca um garçom no banco de dados.
        /// </summary>
        /// <param name="id">Id do garçom a ser buscado.</param>
        /// <returns>Garçom encontrado ou null.</returns>
        public Garcom? Get(int id)
        {
            return context.Garcons.Find(id);
        }
        /// <summary>
        /// Obtém todos os garçons do banco de dados.
        /// </summary>
        /// <returns>Lista de garçons.</returns>
        public IEnumerable<Garcom> GetAll()
        {
            return context.Garcons.ToList();
        }
    }
}
