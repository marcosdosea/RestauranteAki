using AutoMapper;
using Core;
using Core.Service;
using Microsoft.AspNetCore.Mvc;
using RestauranteAkiWeb.Models;


namespace RestauranteAkiWeb.Controllers
{
    public class PessoaController : Controller
    {
        private IPessoaService pessoaService;
        private IMapper mapper;

        public PessoaController(IMapper mapper, IPessoaService pessoaService)
        {
            this.mapper = mapper;
            this.pessoaService = pessoaService;

        }

        // GET: PessoaController
        public ActionResult Index()
        {
            var listaPessoas = pessoaService.GetAll();
            var listaPessoasViewModel = mapper.Map<List<PessoaViewModel>>(listaPessoas);
            return View(listaPessoasViewModel);
        }

        // GET: PessoaController/Details/5
        public ActionResult Details(int id)
        {
            var pessoa = pessoaService.Get(id);
            var pessoaViewModel = mapper.Map<List<PessoaViewModel>>(pessoa);
            return View(pessoaViewModel);
        }

        // GET: PessoaController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PessoaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PessoaViewModel pessoaViewModel)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var pessoa = mapper.Map<Pessoa>(pessoaViewModel);
                pessoaService.Create(pessoa);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError(String.Empty, "Ocorreu um erro inesperádo. Tente novamente.");
                return View(pessoaViewModel);
            }
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
                return BadRequest();
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
                return BadRequest();
            }
            try
            {
                pessoaService.Delete(id);
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
