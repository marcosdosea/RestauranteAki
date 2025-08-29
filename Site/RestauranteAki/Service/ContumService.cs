using Core;
using Core.Service;
using Microsoft.EntityFrameworkCore;

namespace Service
{
    /// <summary>
    /// Implementa serviços para manter dados da conta
    /// </summary>
    public class ContumService : IContumService
    {

        private readonly RestauranteAkiContext context;

        public ContumService(RestauranteAkiContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Criar uma nova conta na base de dados
        /// </summary>
        /// <param name="conta">dados da conta</param>
        /// <returns>id da conta</returns>
        public int Create(Contum conta)
        {
            context.Add(conta);
            context.SaveChanges();
            return conta.Id;
        }

        /// <summary>
        /// Remover a conta da base de dados
        /// </summary>
        /// <param name="id">id da conta</param>
        public void Delete(int id)
        {
            var conta = context.Conta.Find(id);
            if (conta != null)
            {
                context.Remove(conta);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Editar dados da conta na base de dados
        /// </summary>
        /// <param name="conta"></param>
        public void Edit(Contum conta)
        {
            context.Update(conta);
            context.SaveChanges();
        }

        /// <summary>
        /// Buscar uma conta na base de dados
        /// </summary>
        /// <param name="id">id da conta</param>
        /// <returns>dados da conta</returns>
        public Contum? Get(int id)
        {
            return context.Conta.Find(id);
        }

        /// <summary>
        /// Buscar todas as contas presentes na base de dados
        /// </summary>
        /// <returns>lista de contas</returns>
        public IEnumerable<Contum> GetAll()
        {
            return context.Conta.AsNoTracking();
        }
    }
}
