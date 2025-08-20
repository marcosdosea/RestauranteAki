using AutoMapper;
using Core;
using Core.Service;
using Microsoft.AspNetCore.Mvc;
using RestauranteAkiWeb.Models;


namespace RestauranteWeb.Controllers
{
    public class PedidoitemcardapioController : Controller
    {
        private readonly IPedidoItemcardapioService pedidoitemcardapio;
        private readonly IItemcardapioService itemCardapioService;
        private readonly IPedidoService pedidoService;
        private readonly IMapper mapper;

        public PedidoitemcardapioController(
            IPedidoItemcardapioService pedidoitemcardapio,
            IItemcardapioService itemCardapioService,
            IPedidoService pedidoService,
            IMapper mapper)
        {
            this.pedidoitemcardapio = pedidoitemcardapio;
            this.itemCardapioService = itemCardapioService;
            this.pedidoService = pedidoService; 
            this.mapper = mapper;
        }

        // GET: Pedidoitemcardapio
        public ActionResult Index()
        {
            var listaPedidoitemcardapio = pedidoitemcardapio.GetAll();
            var PedidoitemcardapioViewModel = mapper.Map<List<PedidoItemcardapioViewModel>>(listaPedidoitemcardapio);
            return View(PedidoitemcardapioViewModel);
        }

        // GET: Pedidoitemcardapio/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Pedidoitemcardapio/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PedidoItemcardapioViewModel pedidoitemcardapioViewModel)
        {
            if (ModelState.IsValid)
            {
                var Pedidoitemcardapio = mapper.Map<PedidoItemcardapio>(pedidoitemcardapioViewModel);
                pedidoitemcardapio.Create(Pedidoitemcardapio);
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Pedidoitemcardapio/Edit/5
        public ActionResult Edit(int IdPedido)
        {
            if (ModelState.IsValid)
            {
                var pedidoitemcardapio1 = mapper.Map<PedidoItemcardapio>(IdPedido);
                pedidoitemcardapio.Edit(pedidoitemcardapio1);
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: Pedidoitemcardapio/Edit/5/10
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PedidoItemcardapioViewModel pedidoitemcardapioViewModel)
        {
            if (ModelState.IsValid)
            {
                var pedidoitemcardapio1 = mapper.Map<PedidoItemcardapio>(pedidoitemcardapioViewModel);
                pedidoitemcardapio.Edit(pedidoitemcardapio1);
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Pedidoitemcardapio/Details/5/10
        public ActionResult Details(int IdPedido)
        {
            var pedidoItemCardapio = pedidoitemcardapio.Get(IdPedido);
            var pedidoItemCardapioViewModel = mapper.Map<List<PedidoItemcardapioViewModel>>(pedidoItemCardapio);
            return View(pedidoItemCardapioViewModel);
        }

        // GET: Pedidoitemcardapio/Delete/5/10
        public ActionResult Delete(int IdPedido)
        {
            var Pedidoitemcardapio = pedidoitemcardapio.Get(IdPedido);
            var pedidoitemcardapioViewModel = mapper.Map<PedidoItemcardapioViewModel>(Pedidoitemcardapio);
            return View(pedidoitemcardapioViewModel);
        }

        // POST: Pedidoitemcardapio/Delete/5/10
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(PedidoItemcardapioViewModel pedidoitemcardapio2)
        {
            pedidoitemcardapio.Delete(pedidoitemcardapio2.IdPedido);
            return RedirectToAction(nameof(Index));
        }
    }
}