using AutoMapper;
using Core;
using Core.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RestauranteAkiWeb.Models;
using Service;

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
                .Where(p => p.TipoPessoa == ((char)TipoPessoa.Gestor).ToString());

            var listaGestoresViewModel = mapper.Map<List<PessoaViewModel>>(listaGestores);
            return View(listaGestoresViewModel);
        }

        // GET: PessoaController/IndexGarcom
        public ActionResult IndexGarcom()
        {
            var listaGarcons = pessoaService.GetAll()
                .Where(p => p.TipoPessoa == ((char)TipoPessoa.Funcionario).ToString());

            var listaGarconsViewModel = mapper.Map<List<PessoaViewModel>>(listaGarcons);
            return View(listaGarconsViewModel);
        }

        // GET: PessoaController/Details/5
        public ActionResult Details(int id)
        {
            var pessoa = pessoaService.Get(id);
            if (pessoa == null)
            {
                return NotFound();
            }
            var pessoaViewModel = mapper.Map<PessoaViewModel>(pessoa);
            ConfigurarViewBag(pessoaViewModel, "Details");

            var restaurante = restauranteService.Get(pessoa.IdRestaurante);
            ViewBag.NomeRestaurante = restaurante?.NomeFantasia ?? "Não encontrado";

            return View(pessoaViewModel);
        }

        // GET: PessoaController/CreateGestor
        public ActionResult CreateGestor()
        {
            var viewModel = new PessoaViewModel
            {
                TipoPessoa = TipoPessoa.Gestor
            };

            ConfigurarViewBag(viewModel, "Create");

            SelectListRestaurante();
            return View("Create", viewModel);
        }

        // GET: PessoaController/CreateGarcom
        public ActionResult CreateGarcom()
        {
            var viewModel = new PessoaViewModel
            {
                TipoPessoa = TipoPessoa.Funcionario
            };

            ConfigurarViewBag(viewModel, "Create");

            SelectListRestaurante();
            return View("Create", viewModel);
        }

        // POST: PessoaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PessoaViewModel pessoaViewModel)
        {
            if (ModelState.IsValid)
            {
                var pessoa = mapper.Map<Pessoa>(pessoaViewModel);

                pessoaService.Create(pessoa);

                if (pessoaViewModel.TipoPessoa == TipoPessoa.Gestor)
                    return RedirectToAction(nameof(IndexGestor));
                else
                    return RedirectToAction(nameof(IndexGarcom));
            }

            ConfigurarViewBag(pessoaViewModel, "Create");
            SelectListRestaurante();
            return View(pessoaViewModel);
        }

        // GET: PessoaController/Edit/5
        public ActionResult Edit(int id)
        {
            var pessoa = pessoaService.Get(id);
            if (pessoa == null)
            {
                return NotFound();
            }

            var pessoaViewModel = mapper.Map<PessoaViewModel>(pessoa);
            ConfigurarViewBag(pessoaViewModel, "Edit");

            SelectListRestaurante(pessoaViewModel.IdRestaurante);
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

            if (ModelState.IsValid)
            {
                var pessoa = mapper.Map<Pessoa>(pessoaViewModel);

                pessoaService.Edit(pessoa);

                if (pessoaViewModel.TipoPessoa == TipoPessoa.Gestor)
                    return RedirectToAction(nameof(IndexGestor));
                else
                    return RedirectToAction(nameof(IndexGarcom));
            }

            ConfigurarViewBag(pessoaViewModel, "Edit");
            SelectListRestaurante(pessoaViewModel.IdRestaurante);
            return View(pessoaViewModel);
        }

        // GET: PessoaController/Delete/5
        public ActionResult Delete(int id)
        {
            var pessoa = pessoaService.Get(id);
            if (pessoa == null)
            {
                return NotFound();
            }

            var pessoaViewModel = mapper.Map<PessoaViewModel>(pessoa);
            ConfigurarViewBag(pessoaViewModel, "Delete");

            var restaurante = restauranteService.Get(pessoa.IdRestaurante);
            ViewBag.NomeRestaurante = restaurante?.NomeFantasia ?? "Não encontrado";

            return View(pessoaViewModel);
        }

        // POST: PessoaController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var pessoa = pessoaService.Get(id);
            if (pessoa == null)
            {
                return NotFound();
            }

            string tipoPessoa = pessoa.TipoPessoa;
            pessoaService.Delete(id);

            if (tipoPessoa == ((char)TipoPessoa.Gestor).ToString())
                return RedirectToAction(nameof(IndexGestor));
            else
                return RedirectToAction(nameof(IndexGarcom));
        }

        private void SelectListRestaurante(object? selectedValue = null)
        {
            var restaurantes = restauranteService.GetAll();
            ViewBag.Restaurantes = new SelectList(restaurantes, "Id", "NomeFantasia", selectedValue);
        }

        private void ConfigurarViewBag(PessoaViewModel pessoaViewModel, string operacao)
        {
            string tituloOperacao = operacao switch
            {
                "Create" => "Adicionar",
                "Edit" => "Editar",
                "Delete" => "Excluir",
                "Details" => "Detalhes do",
                _ => ""
            };

            if (pessoaViewModel.TipoPessoa == TipoPessoa.Gestor)
            {
                ViewBag.Title = $"{tituloOperacao} Gestor";
                ViewBag.HeaderText = "Gestor";
                ViewBag.CancelAction = nameof(IndexGestor);
            }
            else
            {
                ViewBag.Title = $"{tituloOperacao} Garçom";
                ViewBag.HeaderText = "Garçom";
                ViewBag.CancelAction = nameof(IndexGarcom);
            }
        }
    }
}
