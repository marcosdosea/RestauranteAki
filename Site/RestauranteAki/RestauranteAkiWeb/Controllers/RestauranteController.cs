using AutoMapper;
using Core;
using Core.Service;
using Microsoft.AspNetCore.Mvc;
using RestauranteAkiWeb.Models;

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
            var restaurantes = restauranteService.GetAll();
            var listaRestaurantes = mapper.Map<List<RestauranteViewModel>>(restaurantes);
            return View(listaRestaurantes);
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

        // POST: RestaunteController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RestauranteViewModel restauranteViewModel)
        {

            if (ModelState.IsValid)
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
            var restauranteViewModel = mapper.Map<RestauranteViewModel>(restaurante);
            return View(restauranteViewModel);
        }

        // POST: RestauranteController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RestauranteViewModel restauranteViewModel)
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
            var restauranteViewModel = mapper.Map<RestauranteViewModel>(restaurante);
            return View(restauranteViewModel);
        }

        // POST: RestauranteController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(RestauranteViewModel restauranteViewModel)
        {
            restauranteService.Delete(restauranteViewModel.Id);
            return RedirectToAction(nameof(Index));
        }
    }
}