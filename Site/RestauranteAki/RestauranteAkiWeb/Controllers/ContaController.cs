using AutoMapper;
using Core;
using Core.Service;
using Microsoft.AspNetCore.Mvc;
using RestauranteAkiWeb.Models;

namespace RestauranteAkiWeb.Controllers
{
    public class ContaController : Controller
    {
        private readonly IContumService contaService;
        private readonly IMapper mapper;

        public ContaController(IContumService contaService, IMapper mapper)
        {
            this.contaService = contaService;
            this.mapper = mapper;
        }

        // GET: ContaController
        public ActionResult Index()
        {
            var listaContas = contaService.GetAll();
            var listaContasViewModel = mapper.Map<List<ContumViewModel>>(listaContas);
            return View(listaContasViewModel);
        }

        // GET: ContaController/Details/1
        public ActionResult Details(int id)
        {
            var conta = contaService.Get(id);
            var contaViewModel = mapper.Map<ContumViewModel>(conta);
            return View(contaViewModel);
        }

        // GET: ContaController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ContaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ContumViewModel contaViewModel)
        {
            if (ModelState.IsValid)
            {
                var conta = mapper.Map<Contum>(contaViewModel);
                contaService.Create(conta);
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: ContaController/Edit/1
        public ActionResult Edit(int id)
        {
            var conta = contaService.Get(id);
            var contaViewModel = mapper.Map<ContumViewModel>(conta);
            return View(contaViewModel);
        }

        // POST: ContaController/Edit/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, ContumViewModel contaViewModel)
        {
            if (ModelState.IsValid)
            {
                var conta = mapper.Map<Contum>(contaViewModel);
                contaService.Edit(conta);
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: ContaController/Delete/1
        public ActionResult Delete(int id)
        {
            var conta = contaService.Get(id);
            var contaViewModel = mapper.Map<ContumViewModel>(conta);
            return View(contaViewModel);
        }

        // POST: ContaController/Delete/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(ContumViewModel contaViewModel)
        {
            contaService.Delete(contaViewModel.Id);
            return RedirectToAction(nameof(Index));
        }
    }
}