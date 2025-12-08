using AutoMapper;
using Core;
using Core.Dto;
using Core.Service;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RestauranteAkiWeb.Models;
using Service;
using System.Threading.Tasks;

namespace RestauranteAkiWeb.Controllers
{
    public class PedidoController : Controller
    {
        private readonly IPedidoService pedidoService;
        private readonly IMesaService mesaService;
        private readonly IItemcardapioService itemcardapioService;
        private readonly IPersonagemService personagemService;
        private readonly IContumService contaService;
        private readonly IMapper mapper;

        public PedidoController(IPedidoService pedidoService, IMesaService mesaService, IItemcardapioService itemcardapioService, IPersonagemService personagemService, IContumService contaService, IMapper mapper)
        {
            this.pedidoService = pedidoService;
            this.mesaService = mesaService;
            this.itemcardapioService = itemcardapioService;
            this.personagemService = personagemService;
            this.contaService = contaService;
            this.mapper = mapper;
        }


        // GET: PedidoController
        public ActionResult Index()
        {
            var listaPedidos = pedidoService.GetAll();
            var listaPedidosViewModel = mapper.Map<List<PedidoViewModel>>(listaPedidos);
            return View(listaPedidosViewModel);
        }

        // GET: PedidoController/Details/1
        public ActionResult Details(int id)
        {
            var pedido = pedidoService.Get(id);
            var pedidoViewModel = mapper.Map<PedidoViewModel>(pedido);
            return View(pedidoViewModel);
        }

        // GET: PedidoController/Create
        //[HttpGet]
        //public ActionResult Create([FromQuery] int personagemId)
        //{
        //    var mesas = mesaService.GetAll();
        //    var itensCardapio = itemcardapioService.GetAll().ToList();

        //    ViewBag.Mesas = mesas.Select(x => new SelectListItem
        //    {
        //        Text = "MESA " + x.Id.ToString(),
        //        Value = x.Id.ToString()
        //    }).ToList();
        //    ViewBag.ItensCardapio = mapper.Map<List<ItemcardapioViewModel>>(itensCardapio);

        //    return View(new NovoPedidoViewModel
        //    {
        //        IdPersonagem = personagemId,
        //    });
        //}

        // GET: PedidoController/Create
        [HttpGet]
        public async Task<IActionResult> Create(int idMesa)
        {
            var conta = await contaService.GetOrCreateContaAtiva(idMesa);
            var personagens = await personagemService.GetByContaAsync(conta.Id);
            var itens = await itemcardapioService.GetAllAsync();

            var viewModel = new PedidoCreateViewModel
            {
                IdConta = conta.Id,
                IdMesa = conta.IdMesa,
                Personagens = personagens
                    .Select(p => new PersonagemSelectionViewModel
                    {
                        Id = p.Id,
                        Nome = $"Cliente {p.Id}",
                    })
                    .ToList(),
                Categorias = new List<CategoriaCardapioViewModel>
                {
                    new CategoriaCardapioViewModel
                    {
                        NomeCategoria = "Hambúrguer",
                        Itens = itens
                            .Where(i => i.Categoria == 1)
                            .Select(i => new ItemCardapioSelectionViewModel
                            {
                                Id = i.Id,
                                Nome = i.Nome,
                                Preco = (decimal)i.PrecoUnitario,
                                Descricao = i.Descricao
                            })
                            .ToList()
                    },

                    new CategoriaCardapioViewModel
                    {
                        NomeCategoria = "Pizza",
                        Itens = itens
                            .Where(i => i.Categoria == 3)
                            .Select(i => new ItemCardapioSelectionViewModel
                            {
                                Id = i.Id,
                                Nome = i.Nome,
                                Preco = (decimal)i.PrecoUnitario,
                                Descricao = i.Descricao
                            })
                            .ToList()
                    },
                }
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SalvarPedido([FromBody] PedidoSubmissionDto dto)
        {
            var sucesso = await pedidoService.CriarPedidoAsync(dto);
            if (sucesso)
                return Ok(new { success = true, message = "Pedido enviado!" });

            return BadRequest(new { success = false, message = "Erro ao processar pedido." });
        }

        // GET: PedidoController/Edit/1
        public ActionResult Edit(int id)
        {
            var pedido = pedidoService.Get(id);
            var pedidoViewModel = mapper.Map<PedidoViewModel>(pedido);
            return View(pedidoViewModel);
        }

        // POST: PedidoController/Edit/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, PedidoViewModel pedidoViewModel)
        {
            if (ModelState.IsValid)
            {
                var pedido = mapper.Map<Pedido>(pedidoViewModel);
                pedidoService.Edit(pedido);
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: PedidoController/Delete/1
        public ActionResult Delete(int id)
        {
            var pedido = pedidoService.Get(id);
            var pedidoViewModel = mapper.Map<PedidoViewModel>(pedido);
            return View(pedidoViewModel);
        }

        // POST: PedidoController/Delete/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(PedidoViewModel pedidoViewModel)
        {
            pedidoService.Delete(pedidoViewModel.Id);
            return RedirectToAction(nameof(Index));
        }
    }
}