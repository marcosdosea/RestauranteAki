using AutoMapper;
using Core;
using Core.Service;
using Microsoft.AspNetCore.Mvc;
using RestauranteAkiWeb.Models;
using System.Collections.Generic;

namespace RestauranteAkiWeb.Controllers
{
    public class RestauranteController : Controller
    {
        private readonly IRestauranteService restauranteService;
        private readonly IMapper mapper;

        public RestauranteController(IMapper mapper, IRestauranteService restauranteService)
        {
            this.mapper = mapper;
            this.restauranteService = restauranteService;
        }

        // GET: RestauranteController
        public ActionResult Index()
        {
            var listaRestaurantes = restauranteService.GetAll();
            var listaRestaurantesViewModel = mapper.Map<List<RestauranteViewModel>>(listaRestaurantes);
            return View(listaRestaurantesViewModel);
        }

        // GET: RestauranteController/Details/5
        public ActionResult Details(int id)
        {
            var restaurante = restauranteService.Get(id);
            var restauranteViewModel = mapper.Map<RestauranteViewModel>(restaurante);
            return View(restauranteViewModel);
        }

        // GET: RestauranteController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RestauranteController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RestauranteViewModel restauranteViewModel)
        {
            if (!ModelState.IsValid)
            {
                var restaurante = mapper.Map<Restaurante>(restauranteViewModel);
                restauranteService.Create(restaurante);
            }
            return RedirectToAction(nameof(Index));
          
        }

        // GET: RestauranteController/Edit/5
        public ActionResult Edit(int id)
        {
            var restaurante = restauranteService.Get(id);
            RestauranteViewModel restauranteViewModel = mapper.Map<RestauranteViewModel>(restaurante);
            return View(restauranteViewModel);
        }

        // POST: RestauranteController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, RestauranteViewModel restauranteViewModel)
        {
            if (ModelState.IsValid)
            {
                var restaurante = mapper.Map<Restaurante>(restauranteViewModel);
                restauranteService.Edit(restaurante);
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: RestauranteController/Delete/5
        public ActionResult Delete(int id)
        {
            var restaurante = restauranteService.Get(id);
            RestauranteViewModel restauranteViewModel = mapper.Map<RestauranteViewModel>(restaurante);
            return View(restauranteViewModel);
        }

        // POST: RestauranteController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, RestauranteViewModel restauranteViewModel)
        {
            restauranteService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}