using AutoMapper;
using Core;
using Core.Service;
using Microsoft.AspNetCore.Mvc;
using RestauranteAkiWeb.Models;

namespace RestauranteAkiWeb.Controllers
{
    public class PedidoController : Controller
    {
        private readonly IPedidoService pedidoService;
        private readonly IMapper mapper;

        public PedidoController(IPedidoService pedidoService, IMapper mapper)
        {
            this.pedidoService = pedidoService;
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
        public ActionResult Create()
        {
            return View();
        }

        // POST: PedidoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PedidoViewModel pedidoViewModel)
        {
            if (ModelState.IsValid)
            {
                var pedido = mapper.Map<Pedido>(pedidoViewModel);
                pedidoService.Create(pedido);
            }
            return RedirectToAction(nameof(Index));
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