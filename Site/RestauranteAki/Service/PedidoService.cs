using Core;
using Core.Dto;
using Core.Service;
using Microsoft.EntityFrameworkCore;

namespace Service
{
    /// <summary>
    /// Implementa serviços para manter dados do pedido
    /// </summary>
    public class PedidoService : IPedidoService
    {

        private readonly RestauranteAkiContext context;

        public PedidoService(RestauranteAkiContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Criar um novo pedido na base de dados
        /// </summary>
        /// <param name="pedido">dados do pedido</param>
        /// <returns>id do pedido</returns>
        public int Create(Pedido pedido)
        {
            context.Add(pedido);
            context.SaveChanges();
            return pedido.Id;
        }

        /// <summary>
        /// Remover o pedido da base de dados
        /// </summary>
        /// <param name="id">id do pedido</param>
        public void Delete(int id)
        {
            var pedido = context.Pedidos.Find(id);
            if (pedido != null)
            {
                context.Remove(pedido);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Editar dados do pedido na base de dados
        /// </summary>
        /// <param name="pedido"></param>
        public void Edit(Pedido pedido)
        {
            context.Update(pedido);
            context.SaveChanges();
        }

        /// <summary>
        /// Buscar um pedido na base de dados
        /// </summary>
        /// <param name="id">id do pedido</param>
        /// <returns>dados do pedido</returns>
        public Pedido? Get(int id)
        {
            return context.Pedidos.Find(id);
        }

        /// <summary>
        /// Buscar todos os pedidos cadastrados
        /// </summary>
        /// <returns>lista de pedidos</returns>
        public IEnumerable<Pedido> GetAll()
        {
            return context.Pedidos.AsNoTracking();
        }

        public async Task<bool> CriarPedidoAsync(PedidoSubmissionDto dto)
        {
            if (dto == null || dto.Itens == null || !dto.Itens.Any())
                return false;

            using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                var conta = await context.Conta
                    .FirstOrDefaultAsync(c => c.Id == dto.IdConta);

                if (conta == null) return false;

                // Criar o Cabeçalho do Pedido
                var novoPedido = new Pedido
                {
                    IdConta = dto.IdConta,
                    IdMesa = conta.IdMesa,
                    IdPersonagem = dto.IdPersonagem,
                    Status = "S",
                    IdPessoa = 1
                };

                context.Pedidos.Add(novoPedido);
                await context.SaveChangesAsync();

                // Processar e Salvar os Itens
                foreach (var itemInput in dto.Itens)
                {
                    var itemCardapio = await context.Itemcardapios
                        .FirstOrDefaultAsync(i => i.Id == itemInput.IdItem);

                    if (itemCardapio != null)
                    {
                        var novoItem = new PedidoItemcardapio
                        {
                            IdPedido = novoPedido.Id,
                            IdItemCardapio = itemCardapio.Id,
                            Quantidade = itemInput.Quantidade,
                            // PrecoUnitario = itemCardapio.PrecoUnitario
                        };

                        context.PedidoItemcardapios.Add(novoItem);
                    }
                }

                await context.SaveChangesAsync();
                await RecalcularValorTotalConta(dto.IdConta);

                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao criar pedido: {ex.Message}");
                await transaction.RollbackAsync();
                return false;
            }
        }

        private async Task RecalcularValorTotalConta(int idConta)
        {
            // 1. Busca todos os itens de todos os pedidos desta conta
            // Nota: Filtramos para não somar pedidos cancelados
            var total = await context.PedidoItemcardapios
                .Include(pi => pi.IdPedidoNavigation)
                .Include(pi => pi.IdItemCardapioNavigation)
                .Where(pi => pi.IdPedidoNavigation.IdConta == idConta && pi.IdPedidoNavigation.Status != "C") // "C" = Cancelado (implementar ainda)
                .SumAsync(pi => pi.Quantidade * pi.IdItemCardapioNavigation.PrecoUnitario);

            // 2. Busca a conta para atualizar
            var conta = await context.Conta.FindAsync(idConta);
            if (conta != null)
            {
                conta.Valor = (float)total; // Atualiza o campo Valor
                context.Conta.Update(conta);
                await context.SaveChangesAsync();
            }
        }
    }
}
