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
        public async Task<IActionResult> Mesa(int id)
        {
            var conta = await contaService.GetContaCompletaPorMesaAsync(id);
            if (conta == null) return RedirectToAction("Index");

            decimal subtotal = (decimal)conta.Valor;
            decimal servico = subtotal * 0.1m;
            decimal total = subtotal + servico;

            var viewModel = new MesaHubViewModel
            {
                IdConta = conta.Id,
                IdMesa = id,
                TotalAtual = total,
                Subtotal = subtotal,
                Servico = servico
            };

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


            // Busca pedidos associados a personagens
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

            return View(viewModel);
        }
    }
}
