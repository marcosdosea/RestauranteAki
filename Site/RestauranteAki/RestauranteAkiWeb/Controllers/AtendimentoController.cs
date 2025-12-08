using AutoMapper;
using Core.Service;
using Microsoft.AspNetCore.Mvc;
using RestauranteAkiWeb.Models;

namespace RestauranteAkiWeb.Controllers
{
    public class AtendimentoController : Controller
    {
        private readonly IMesaService mesaService;
        private readonly IContumService contaService;
        private readonly IMapper mapper;

        public AtendimentoController(IMesaService mesaService, IContumService contaService, IMapper mapper)
        {
            this.mesaService = mesaService;
            this.contaService = contaService;
            this.mapper = mapper;
        }

        public ActionResult Index()
        {
            var mesas = mesaService.GetAll();
            var mesasViewModel = mapper.Map<List<MesaViewModel>>(mesas);

            return View(mesasViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Mesa(int id) // ID da Mesa
        {
            var conta = await contaService.GetContaCompletaPorMesaAsync(id);

            // MOCK PARA EXEMPLO (Substitua pelo retorno do seu service acima)
            //var conta = new { Id = 100, IdMesa = id, Valor = 214.50m, Pedidos = new List<dynamic>(), Personagems = new List<dynamic>() };

            if (conta == null) return RedirectToAction("Index");

            // 2. Mapeamento para o ViewModel
            // Regra de negócio simples para dividir o valor (ajuste conforme sua regra real)
            decimal total = (decimal)conta.Valor;
            decimal subtotal = total / 1.1m;
            decimal servico = total - subtotal;

            var viewModel = new MesaHubViewModel
            {
                IdConta = conta.Id,
                IdMesa = id,
                TotalAtual = total,
                Subtotal = subtotal,
                Servico = servico
            };

            // 3. Função auxiliar local para mapear status do banco para visual
            (string texto, string cor) ObterStatusVisual(string statusDb)
            {
                return statusDb switch
                {
                    "E" => ("Entregue", "color-green"),     // Exemplo: E = Entregue
                    "S" => ("Em preparo", "color-orange"),  // Exemplo: P = Preparando
                    "P" => ("Pronto", "color-purple"),      // Exemplo: F = Finalizado/Pronto
                    _ => ("Pendente", "text-muted")
                };
            }

            // 4. Agrupamento: Itens da Mesa (IdPersonagem NULL)
            // Nota: Adapte "conta.Pedidos" para a estrutura real que seu service retorna
            // Busca todos os pedidos da mesa (IdPersonagem = -1)
            var pedidosMesa = conta.Pedidos
                .Where(pedido => pedido.IdPersonagem == -1)
                .ToList();

            // Projeta os itens de cada pedido da mesa
            var itensMesa = pedidosMesa
                .SelectMany(pedido =>
                {
                    var statusInfo = ObterStatusVisual(pedido.Status);

                    return pedido.PedidoItemcardapios.Select(item => new HubItemExtratoViewModel
                    {
                        IdItemPedido = item.IdPedido,
                        NomeItem = item.IdItemCardapioNavigation.Nome,
                        Quantidade = item.Quantidade ?? 0,
                        StatusTexto = statusInfo.texto,
                        StatusCorCss = statusInfo.cor
                    });
                })
                .ToList();

            // Adiciona ao grupo, se houver itens
            if (itensMesa.Any())
            {
                viewModel.GruposPedidos.Add(new HubGrupoPedidoViewModel
                {
                    Titulo = "Itens da mesa",
                    IdPersonagem = null,
                    Itens = itensMesa
                });
            }


            // 5. Agrupamento: Itens por Cliente
            var pedidosComItens = conta.Pedidos
                .Where(p => conta.Personagems.Select(x => x.Id).Contains(p.IdPersonagem))
                .Select(p => new
                {
                    Pedido = p,
                    Itens = p.PedidoItemcardapios.Select(i => new
                    {
                        i.IdPedido,
                        NomeItem = i.IdItemCardapioNavigation.Nome,
                        Quantidade = i.Quantidade,
                    }).ToList()
                })
                .ToList();

            var pedidosPorPersonagem = pedidosComItens
                .GroupBy(x => x.Pedido.IdPersonagem)
                .ToDictionary(g => g.Key, g => g.ToList());

            foreach (var personagem in conta.Personagems)
            {
                var itensCliente = new List<HubItemExtratoViewModel>();

                if (pedidosPorPersonagem.TryGetValue(personagem.Id, out var pedidosDoCliente))
                {
                    foreach (var ped in pedidosDoCliente)
                    {
                        var statusInfo = ObterStatusVisual(ped.Pedido.Status);

                        foreach (var item in ped.Itens)
                        {
                            itensCliente.Add(new HubItemExtratoViewModel
                            {
                                IdItemPedido = item.IdPedido,
                                NomeItem = item.NomeItem,
                                Quantidade = item.Quantidade ?? 0,
                                StatusTexto = statusInfo.texto,
                                StatusCorCss = statusInfo.cor
                            });
                        }
                    }
                }

                viewModel.GruposPedidos.Add(new HubGrupoPedidoViewModel
                {
                    Titulo = $"Cliente {personagem.Id}",
                    IdPersonagem = personagem.Id,
                    Itens = itensCliente
                });
            }

            // --- DADOS FAKE APENAS PARA VOCÊ VER A TELA FUNCIONANDO (Remova ao integrar o Service) ---
            //viewModel.GruposPedidos.Add(new HubGrupoPedidoViewModel
            //{
            //    Titulo = "Itens da mesa",
            //    IdPersonagem = null,
            //    Itens = new List<HubItemExtratoViewModel> {
            //    new HubItemExtratoViewModel { Quantidade = 1, NomeItem = "Chope Brahma", StatusTexto = "Entregue", StatusCorCss = "color-green" },
            //    new HubItemExtratoViewModel { Quantidade = 1, NomeItem = "Picanha Fatiada", StatusTexto = "Em preparo", StatusCorCss = "color-orange" },
            //    new HubItemExtratoViewModel { Quantidade = 1, NomeItem = "Caipirinha de morango com dose extra", StatusTexto = "Pronto", StatusCorCss = "color-purple" }
            //}
            //});
            //viewModel.GruposPedidos.Add(new HubGrupoPedidoViewModel { Titulo = "Cliente 2", IdPersonagem = 2 });
            //viewModel.GruposPedidos.Add(new HubGrupoPedidoViewModel { Titulo = "Cliente 3", IdPersonagem = 3 });
            // -----------------------------------------------------------------------------------------

            return View(viewModel);
        }
    }
}
