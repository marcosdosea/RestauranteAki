using AutoMapper;
using Core;
using Core.Service;
using Microsoft.AspNetCore.Mvc;
using RestauranteAkiWeb.Models;


namespace RestauranteWeb.Controllers
{
    public class PedidoitemcardapioController : Controller
    {
        private readonly IPedidoItemcardapioService pedidoitemcardapioService;
        private readonly IMapper mapper;

        public PedidoitemcardapioController(
            IPedidoItemcardapioService pedidoitemcardapioService,
            IMapper mapper)
        {
            this.pedidoitemcardapioService = pedidoitemcardapioService;
            this.mapper = mapper;
        }

        // GET: Pedidoitemcardapio
        public ActionResult Index()
        {
            var listaPedidoitemcardapio = pedidoitemcardapioService.GetAll();
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
                pedidoitemcardapioService.Create(Pedidoitemcardapio);
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Pedidoitemcardapio/Edit/5
        public ActionResult Edit(int IdPedido)
        {
            if (ModelState.IsValid)
            {
                var pedidoitemCardapio = mapper.Map<PedidoItemcardapio>(IdPedido);
                pedidoitemcardapioService.Edit(pedidoitemCardapio);
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: Pedidoitemcardapio/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PedidoItemcardapioViewModel pedidoitemcardapioViewModel)
        {
            if (ModelState.IsValid)
            {
                var pedidoitemCardapio = mapper.Map<PedidoItemcardapio>(pedidoitemcardapioViewModel);
                pedidoitemcardapioService.Edit(pedidoitemCardapio);
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Pedidoitemcardapio/Details/5
        public ActionResult Details(int IdPedido)
        {
            var pedidoItemCardapio = pedidoitemcardapioService.Get(IdPedido);
            var pedidoItemCardapioViewModel = mapper.Map<PedidoItemcardapioViewModel>(pedidoItemCardapio);
            return View(pedidoItemCardapioViewModel);
        }

        // GET: Pedidoitemcardapio/Delete/5
        public ActionResult Delete(int IdPedido)
        {
            var Pedidoitemcardapio = pedidoitemcardapioService.Get(IdPedido);
            var pedidoitemcardapioViewModel = mapper.Map<PedidoItemcardapioViewModel>(Pedidoitemcardapio);
            return View(pedidoitemcardapioViewModel);
        }

        // POST: Pedidoitemcardapio/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int IdPedido, PedidoItemcardapioViewModel pedidoitemCardapio)
        {
            pedidoitemcardapioService.Delete(IdPedido);
            return RedirectToAction(nameof(Index));
        }
    }
}