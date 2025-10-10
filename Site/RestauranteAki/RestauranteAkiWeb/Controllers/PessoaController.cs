using AutoMapper;
using Core;
using Core.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RestauranteAkiWeb.Models;

namespace RestauranteAkiWeb.Controllers
{
    public class PessoaController : Controller
    {
        private IPessoaService pessoaService;
        private IRestauranteService restauranteService;
        private IMapper mapper;

        public PessoaController(IPessoaService pessoaService, IRestauranteService restauranteService, IMapper mapper)
        {
            this.pessoaService = pessoaService;
            this.restauranteService = restauranteService;
            this.mapper = mapper;
        }

        // GET: PessoaController/IndexGestor
        public ActionResult IndexGestor()
        {
            var listaGestores = pessoaService.GetAll()
                .Where(p => p.TipoPessoa == "G");
            var listaGestoresViewModel = mapper.Map<List<PessoaViewModel>>(listaGestores);
            return View("IndexGestor", listaGestoresViewModel);
        }

        // GET: PessoaController/IndexGarcom
        public ActionResult IndexGarcom()
        {
            var listaGarcons = pessoaService.GetAll()
                .Where(p => p.TipoPessoa == "F");
            var listaGarconsViewModel = mapper.Map<List<PessoaViewModel>>(listaGarcons);
            return View("IndexGarcom", listaGarconsViewModel);
        }

        // GET: PessoaController/Details/5
        public ActionResult Details(int id)
        {
            var pessoa = pessoaService.Get(id);
            var pessoaViewModel = mapper.Map<PessoaViewModel>(pessoa);
            return View(pessoaViewModel);
        }

        // GET: PessoaController/Create
        public ActionResult Create(string tipo)
        {
            SelectListRestaurante();
            ViewBag.TipoPessoa = tipo; // "G" ou "F"
            return View();
        }

        // POST: PessoaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PessoaViewModel pessoaViewModel)
        {

            if (User.IsInRole("Gestor"))
            {
                pessoaViewModel.TipoPessoa = "G";
            }
            else
            {
                pessoaViewModel.TipoPessoa = "F";
            }
            var pessoa = mapper.Map<Pessoa>(pessoaViewModel);
            pessoaService.Create(pessoa);

            // Redireciona para a index correta
            if (pessoaViewModel.TipoPessoa == "G")
                return RedirectToAction(nameof(IndexGestor));
            else
                return RedirectToAction(nameof(IndexGarcom));
        }

        // GET: PessoaController/Edit/5
        public ActionResult Edit(int id)
        {
            var pessoa = pessoaService.Get(id);
            PessoaViewModel pessoaViewModel = mapper.Map<PessoaViewModel>(pessoa);

            return View(pessoaViewModel);
        }

        // POST: PessoaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, PessoaViewModel pessoaViewModel)
        {
            if (id != pessoaViewModel.Id)
            {
                return NotFound();
            }
            try
            {
                var pessoa = mapper.Map<Pessoa>(pessoaViewModel);
                pessoaService.Edit(pessoa);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError(String.Empty, "Ocorreu um erro inesperádo. Tente novamente.");
                return View(pessoaViewModel);
            }
        }

        // GET: PessoaController/Delete/5
        public ActionResult Delete(int id)
        {
            var pessoa = pessoaService.Get(id);
            PessoaViewModel pessoaViewModel = mapper.Map<PessoaViewModel>(pessoa);
            return View(pessoaViewModel);
        }

        // POST: PessoaController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, PessoaViewModel pessoaViewModel)
        {
            if (id != pessoaViewModel.Id)
            {
                return NotFound();
            }
            try
            {
                pessoaService.Delete(id);
                if (pessoaViewModel.TipoPessoa == "G")
                    return RedirectToAction(nameof(IndexGestor));
                else
                    return RedirectToAction(nameof(IndexGarcom));
            }
            catch
            {
                ModelState.AddModelError(String.Empty, "Ocorreu um erro inesperado. Tente Novamente.");
                return View(pessoaViewModel);
            }
        }
        private void SelectListRestaurante()
        {
            var restaurantes = restauranteService.GetAll();
            ViewBag.Restaurantes = restaurantes.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Nome
            }).ToList();
        }
    }
}
