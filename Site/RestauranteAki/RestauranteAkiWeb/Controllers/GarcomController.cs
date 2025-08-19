using AutoMapper;
using Core;
using Core.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestauranteAkiWeb.Models;
using Service;

namespace RestauranteAkiWeb.Controllers
{
    public class GarcomController : Controller
    {
        private IGarcomService garcomService;
        private IMapper mapper;

        public GarcomController(IMapper mapper, IGarcomService garcomService)
        {
            this.mapper = mapper;
            this.garcomService = garcomService;
        }
        // GET: GarcomController
        public ActionResult Index()
        {
            var listaGarcons = garcomService.GetAll();
            var listaGarconsViewModel = mapper.Map<List<GarcomViewModel>>(listaGarcons);
            return View(listaGarconsViewModel);
        }

        // GET: GarcomController/Details/5
        public ActionResult Details(int id)
        {
            var garcom = garcomService.Get(id);
            var garcomViewModel = mapper.Map<List<GarcomViewModel>>(garcom);
            return View(garcom);
        }

        // GET: GarcomController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GarcomController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GarcomViewModel  garcomViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var garcom = mapper.Map<Garcom>(garcomViewModel);
                garcomService.Create(garcom);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError(String.Empty, "Ocorreu um erro inesperádo. Tente novamente.");
                return View(garcomViewModel);
            }
        }

        // GET: GarcomController/Edit/5
        public ActionResult Edit(int id)
        {
            var garcom = garcomService.Get(id);
            GarcomViewModel garcomViewModel = mapper.Map<GarcomViewModel>(garcom);
            return View(garcomViewModel);
        }

        // POST: GarcomController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, GarcomViewModel garcomViewModel)
        {
            if (id != garcomViewModel.Id)
            {
                return BadRequest();
            }
            try
            {
                var garcom = mapper.Map<Garcom>(garcomViewModel);
                garcomService.Edit(garcom);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError(String.Empty, "Ocorreu um erro inesperádo. Tente novamente.");
                return View(garcomViewModel);
            }


        }

        // GET: GarcomController/Delete/5
        public ActionResult Delete(int id)
        {
            var garcom = garcomService.Get(id);
            GarcomViewModel garcomViewModel = mapper.Map<GarcomViewModel>(garcom);
            return View(garcomViewModel);
        }

        // POST: GarcomController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, GarcomViewModel garcomViewModel)
        {
            if(id != garcomViewModel.Id)
            {
                return BadRequest();
            }
            try
            {
                garcomService.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError(String.Empty, "Ocorreu um erro inesperádo. Tente novamente.");
                return View();
            }
        }
    }
}
