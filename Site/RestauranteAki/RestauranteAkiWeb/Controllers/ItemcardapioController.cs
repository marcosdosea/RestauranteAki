using AutoMapper;
using Core;
using Core.Service;
using Microsoft.AspNetCore.Mvc;
using RestauranteAkiWeb.Models;

namespace RestauranteAkiWeb.Controllers
{
    public class ItemcardapioController : Controller
    {
        private readonly IMapper mapper;
        private readonly IItemcardapioService itemcardapioService;

        public ItemcardapioController(IMapper mapper, IItemcardapioService itemcardapioService)
        {
            this.mapper = mapper;
            this.itemcardapioService = itemcardapioService;
        }

        // GET: ItemCardapioController
        public ActionResult Index()
        {
            var listaItemcardapio = itemcardapioService.GetAll();
            var itemCardapioViewModel = mapper.Map<List<ItemcardapioViewModel>>(listaItemcardapio);
            return View(itemCardapioViewModel);
        }

        // GET: ItemCardapioController/Details/5
        public ActionResult Details(int id)
        {
            var itemcardapio = itemcardapioService.Get(id);
            var itemcardapioViewModel = mapper.Map<ItemcardapioViewModel>(itemcardapio);
            return View(itemcardapioViewModel);
        }

        // GET: ItemCardapioController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ItemCardapioController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ItemcardapioViewModel itemcardapioViewModel, string[] DiasSemana)
        {
            if (itemcardapioViewModel != null)
            {
                itemcardapioViewModel.DiaSemana = string.Join(",", DiasSemana ?? Array.Empty<string>());
                
                var itemcardapio = mapper.Map<Itemcardapio>(itemcardapioViewModel);
                itemcardapioService.Create(itemcardapio, DiasSemana);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return BadRequest();
            }
        }

        // GET: ItemCardapioController/Edit/5
        public ActionResult Edit(int id)
        {
            var itemcardapio = itemcardapioService.Get(id);
            var itemcardapioViewModel = mapper.Map<ItemcardapioViewModel>(itemcardapio);

            return View(itemcardapioViewModel);
        }

        // POST: ItemCardapioController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, ItemcardapioViewModel itemcardapioViewModel)
        {
            if (id != itemcardapioViewModel.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(itemcardapioViewModel);
            }

            try
            {
                var itemcardapio = mapper.Map<Itemcardapio>(itemcardapioViewModel);
                itemcardapioService.Edit(itemcardapio); // <- ESSENCIAL
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Ocorreu um erro inesperado. Tente novamente.");
                return View(itemcardapioViewModel);
            }
        }

        // GET: ItemCardapioController/Delete/5
        public ActionResult Delete(int id)
        {
            var itemcardapio = itemcardapioService.Get(id);
            var itemcardapioViewModel = mapper.Map<ItemcardapioViewModel>(itemcardapio);
            return View(itemcardapioViewModel);
        }

        // POST: ItemCardapioController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, ItemcardapioViewModel itemcardapioViewModel)
        {
            if (id != itemcardapioViewModel.Id)
            {
                return BadRequest();
            }

            try
            {
                itemcardapioService.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Ocorreu um erro inesperado. Tente novamente.");
                return View();
            }
        }
    }
}
