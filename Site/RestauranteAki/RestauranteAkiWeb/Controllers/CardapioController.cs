using AutoMapper;
using Core;
using Core.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestauranteAkiWeb.Mappers;
using RestauranteAkiWeb.Models;

namespace RestauranteAkiWeb.Controllers
{
    public class CardapioController : Controller
    {
        private ICardapioService cardapioService;
        private IMapper mapper;

        public CardapioController(IMapper mapper, ICardapioService cardapioService)
        {
            this.mapper = mapper;
            this.cardapioService = cardapioService;
        }

        // GET: CardapioController
        public ActionResult Index()
        {
            var listaCardapios = cardapioService.GetAll();
            var listasCardapiosViewModel = mapper.Map<List<CardapioViewModel>>(listaCardapios);
            return View(listasCardapiosViewModel);
        }

        // Substitua o método Details pelo abaixo para corrigir o erro de mapeamento

        public ActionResult Details(int id)
        {
            var cardapio = cardapioService.Get(id);
            var cardapioViewModel = mapper.Map<CardapioViewModel>(cardapio);
            return View(cardapioViewModel);
        }

        // GET: CardapioController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CardapioController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CardapioViewModel cardapioViewModel)
        {
            try
            {
                var cardapio = mapper.Map<Cardapio>(cardapioViewModel);
                cardapioService.Create(cardapio);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError(String.Empty, "Ocorreu um erro inesperádo. Tente novamente.");
                return View(cardapioViewModel);
            }
        }

        // GET: CardapioController/Edit/5
        public ActionResult Edit(int id)
        {
            var cardapio = cardapioService.Get(id);
            CardapioViewModel cardapioViewModel = mapper.Map<CardapioViewModel>(cardapio);
            return View(cardapioViewModel);
        }

        // POST: CardapioController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, CardapioViewModel cardapioViewModel)
        {
            if (id != cardapioViewModel.Id)
            {
                return BadRequest();
            }
            try
            {
                var cardapio = mapper.Map<Cardapio>(cardapioViewModel);
                cardapioService.Edit(cardapio);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError(String.Empty, "Ocorreu um erro inesperádo. Tente novamente.");
                return View(cardapioViewModel);
            }
        }

        // GET: CardapioController/Delete/5
        public ActionResult Delete(int id)
        {
            var cardapio = cardapioService.Get(id);
            CardapioViewModel cardapioViewModel = mapper.Map<CardapioViewModel>(cardapio);
            return View(cardapioViewModel);
        }

        // POST: CardapioController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, CardapioViewModel cardapioViewModel)
        {
            if (id != cardapioViewModel.Id)
            {
                return BadRequest();
            }
            try
            {
                cardapioService.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError(String.Empty, "Ocorreu um erro inesperádo. Tente Novamente.");
                return View();
            }
        }
    }
}
