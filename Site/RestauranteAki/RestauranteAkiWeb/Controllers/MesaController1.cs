using AutoMapper;
using Core;
using Core.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestauranteAkiWeb.Models;

namespace RestauranteAkiWeb.Controllers
{
    public class MesaController1 : Controller
    {
        private readonly IMesaService mesaService;
        private readonly IMapper mapper;

        public MesaController1(IMesaService mesaService,IMapper mapper)
        {
            this.mesaService = mesaService;
            this.mapper = mapper;
        }
        // GET: MesaController1
        public ActionResult Index()
        {
            var listaMesa = mesaService.GetAll();   
            var MesaViewModel = mapper.Map<List<MesaViewModel>>(listaMesa);
            return View(MesaViewModel);
        }

        // GET: MesaController1/Details/5
        public ActionResult Details(int id)
        {
            var mesa = mesaService.GetById(id);
            var MesaViewModel = mapper.Map<MesaViewModel>(mesa);
            return View(MesaViewModel);
        }

        // GET: MesaController1/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MesaController1/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MesaViewModel mesaViewModel)
        {
            try
            {
                var mesa = mapper.Map<Mesa>(mesaViewModel);
                mesaService.Create(mesa);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: MesaController1/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: MesaController1/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, MesaViewModel mesaViewModel)
        {
            try
            {
                var mesa = mapper.Map<Mesa>(mesaViewModel);
                mesaService.Edit(mesa);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: MesaController1/Delete/5
        public ActionResult Delete(int id)
        {
            var mesa = mesaService.Get(id);
            var mesaViewModel = mapper.Map<MesaViewModel>(mesa);
            return View(mesaViewModel);
        }

        // POST: MesaController1/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, MesaViewModel mesaViewModel)
        {
            try
            {
                if(id != mesaViewModel.Id)
                {
                    return NotFound();
                }
                mesaService.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
