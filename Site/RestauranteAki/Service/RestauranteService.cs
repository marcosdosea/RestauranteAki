using Core;
using Core.Service;
using Microsoft.EntityFrameworkCore;
using System; 
using System.Collections.Generic; 
using System.Linq; 
namespace Service
{
    public class RestauranteService : IRestauranteService
    {
        private readonly RestauranteAkiContext context;

        /// <summary>
        /// Implementa os serviços para manter os dados de Restaurante
        /// </summary>
        /// <param name="context">Contexto do banco de dados para interagir com a entidade Restaurante</param>
        public RestauranteService(RestauranteAkiContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Cria um novo Restaurante no banco de dados.
        /// </summary>
        /// <param name="restaurante">Entidade Restaurante que será criada</param>
        /// <returns>Id do restaurante criado</returns>
        public int Create(Restaurante restaurante)
        {
            context.Add(restaurante);
            context.SaveChanges();
            return restaurante.Id;
        }

        /// <summary>
        /// Remove um Restaurante existente com base no seu Id.
        /// </summary>
        /// <param name="id">Id do Restaurante a ser removido</param>
        public void Delete(int id)
        {
            var restaurante = context.Restaurantes.Find(id);

            if (restaurante == null)
            {
                throw new ArgumentException("Restaurante não encontrado para o ID informado.");
            }
            context.Restaurantes.Remove(restaurante);
            context.SaveChanges();
        }

        /// <summary>
        /// Atualiza os dados de um Restaurante existente.
        /// </summary>
        /// <param name="restaurante">Entidade Restaurante com os dados atualizados</param>
        /// <exception cref="ArgumentException">Lançada quando o restaurante com o Id fornecido não é encontrado na base de dados.</exception>
        public void Edit(Restaurante restaurante)
        {
            var restauranteExistente = context.Restaurantes.AsNoTracking().FirstOrDefault(r => r.Id == restaurante.Id);
            if (restauranteExistente == null)
            {
                throw new ArgumentException("Não é possível editar, pois o restaurante não foi encontrado.");
            }
            context.Update(restaurante);
            context.SaveChanges();
        }

        /// <summary>
        /// Busca um Restaurante específico pelo seu Id.
        /// </summary>
        /// <param name="id">Id do restaurante a ser encontrado</param>
        /// <returns>A entidade Restaurante correspondente ao Id, ou null se não for encontrado.</returns>
        public Restaurante? Get(int id)
        {
            return context.Restaurantes.Find(id);
        }

        /// <summary>
        /// Retorna uma lista com todos os Restaurantes cadastrados.
        /// </summary>
        /// <returns>Uma coleção (IEnumerable) de todas as entidades Restaurante.</returns>
        public IEnumerable<Restaurante> GetAll()
        {
           
            return context.Restaurantes.AsNoTracking();
        }
    }
}