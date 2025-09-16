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

        public async Task<int> IniciarPedido(NovoPedidoDto novoPedido)
        {
            var mesa = await context.Mesas.AnyAsync(m => m.Id == novoPedido.IdMesa);
            if (!mesa)
            {
                throw new ArgumentException("Mesa não encontrada");
            }
            var pessoa = await context.Pessoas.FirstOrDefaultAsync(p => p.Email == novoPedido.EmailPessoa)
                ?? throw new ArgumentException("Pessoa não encontrada");


            var pedidosItens = novoPedido.ItensCardapios.Where(x => x.Quantidade > 0).Select(x => new PedidoItemcardapio
            {
                IdItemCardapio = x.ItemCardapioId,
                Quantidade = x.Quantidade,
            }).ToList();

            var valorTotal = (await context.Itemcardapios
                .Where(ic => novoPedido.ItensCardapios.Select(i => i.ItemCardapioId).Contains(ic.Id))
                .ToListAsync())
                .Sum(ic => ic.PrecoUnitario * novoPedido.ItensCardapios.First(i => i.ItemCardapioId == ic.Id).Quantidade);

            var pedidoExistente = context.Pedidos
                .Include(x => x.IdContaNavigation).Where(x => x.IdMesa == novoPedido.IdMesa && x.Status != "E"
                && x.IdPersonagem == novoPedido.IdPersonagem
            ).FirstOrDefault();

            if (pedidoExistente != null)
            {
                var pedidosExistentes =
                    context.PedidoItemcardapios.Where(x => x.IdPedido == pedidoExistente.Id && pedidosItens.Select(x => x.IdItemCardapio).Contains(x.IdItemCardapio)).ToList();

                foreach (var item in pedidosItens)
                {
                    item.Quantidade += pedidosExistentes.FirstOrDefault(x => x.IdItemCardapio == item.IdItemCardapio)?.Quantidade ?? 0;
                }

                var novosPedidosItens = pedidosItens.Where(x => !pedidosExistentes.Any(y => y.IdItemCardapio == x.IdItemCardapio)).ToList();
                foreach (var item in novosPedidosItens)
                {
                    item.IdPedido = pedidoExistente.Id;
                }

                await context.PedidoItemcardapios.AddRangeAsync(novosPedidosItens);
                await context.PedidoItemcardapios.AddRangeAsync(pedidosItens);

                pedidoExistente.IdContaNavigation!.Valor += valorTotal;
                await context.SaveChangesAsync();
                return pedidoExistente.Id;
            }

            var conta = new Contum
            {
                IdMesa = novoPedido.IdMesa,
                Status = "A", // A - Aberta
                Valor = valorTotal,
                FormaPagamento = ""
            };

            var pedido = new Pedido
            {
                IdMesa = novoPedido.IdMesa,
                IdPessoa = pessoa.Id,
                Status = "S", // S - Solicitado
                IdContaNavigation = conta,
                PedidoItemcardapios = pedidosItens,
                IdPersonagem = novoPedido.IdPersonagem,
            };
            await context.Pedidos.AddAsync(pedido);
            await context.SaveChangesAsync();
            return pedido.Id;
        }
    }
}
