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

        /// <summary>
        /// Buscar ou criar uma conta ativa para a mesa informada
        /// </summary>
        /// <param name="idMesa"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Contum> GetOrCreateContaAtiva(int idMesa)
        {
            var contaAtiva = context.Conta
                .Include(c => c.Personagems)
                .FirstOrDefault(c => c.IdMesa == idMesa && c.Status == "A");
            if (contaAtiva != null)
            {
                return contaAtiva;
            }

            await using var transaction = await context.Database.BeginTransactionAsync();

            try
            {
                var novaConta = new Contum
                {
                    IdMesa = idMesa,
                    Status = "A",
                    Valor = 0,
                    FormaPagamento = ""
                };
                context.Conta.Add(novaConta);

                var horaAtual = DateTime.Now;

                var personagem1 = new Personagem
                {
                    IdentificadorCor = $"#{new Random().Next(0x1000000):X6}",
                    DataCriacao = horaAtual,
                    DataAtualizacao = horaAtual,
                    IdContaNavigation = novaConta
                };

                var personagem2 = new Personagem
                {
                    IdentificadorCor = $"#{new Random().Next(0x1000000):X6}",
                    DataCriacao = horaAtual,
                    DataAtualizacao = horaAtual,
                    IdContaNavigation = novaConta
                };

                context.Personagems.AddRange(personagem1, personagem2);

                await context.SaveChangesAsync();
                await transaction.CommitAsync();

                return novaConta;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("Não foi possível criar ou obter nova conta.", ex);
            }
        }
    }
}
