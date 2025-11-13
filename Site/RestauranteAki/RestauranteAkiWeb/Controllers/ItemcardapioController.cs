using AutoMapper;
using Core;
using Core.Service;
using Microsoft.AspNetCore.Mvc;
using RestauranteAkiWeb.Models;
using System.Diagnostics;

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
            return View(new ItemcardapioViewModel());
        }

        // POST: ItemCardapioController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ItemcardapioViewModel viewModel)
        {
            if (string.IsNullOrWhiteSpace(viewModel.Descricao))
            {
                ModelState.AddModelError(nameof(viewModel.Descricao), "É necessário informar ao menos 1 ingrediente.");
            }

            if (ModelState.IsValid)
            {
                Debug.WriteLine(viewModel.Categoria);
                var itemcardapio = mapper.Map<Itemcardapio>(viewModel);

                if (viewModel.ImagemUpload != null && viewModel.ImagemUpload.Length > 0)
                {
                    using var memoryStream = new MemoryStream();
                    viewModel.ImagemUpload.CopyTo(memoryStream);
                    itemcardapio.Imagem = memoryStream.ToArray();
                }
                else
                {
                    itemcardapio.Imagem = System.Text.Encoding.UTF8.GetBytes("placeholder");
                }

                itemcardapioService.Create(itemcardapio);
                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }

        // GET: ItemCardapioController/Edit/5
        public ActionResult Edit(int id)
        {
            var itemcardapio = itemcardapioService.Get(id);
            if (itemcardapio == null)
            {
                return NotFound();
            }

            var itemcardapioViewModel = mapper.Map<ItemcardapioViewModel>(itemcardapio);

            if (itemcardapio.Imagem != null && itemcardapio.Imagem.Length > 0)
            {
                itemcardapioViewModel.ImagemAtual = Convert.ToBase64String(itemcardapio.Imagem);
            }

            return View(itemcardapioViewModel);
        }

        // POST: ItemCardapioController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, ItemcardapioViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return BadRequest();
            }

            if (string.IsNullOrWhiteSpace(viewModel.Descricao))
            {
                ModelState.AddModelError(nameof(viewModel.Descricao), "É necessário informar ao menos 1 ingrediente.");
            }

            if (ModelState.IsValid)
            {
                var itemcardapio = itemcardapioService.Get(id);
                if (itemcardapio == null)
                {
                    return NotFound();
                }

                mapper.Map(viewModel, itemcardapio);

                if (viewModel.ImagemUpload != null && viewModel.ImagemUpload.Length > 0)
                {
                    using var memoryStream = new MemoryStream();
                    viewModel.ImagemUpload.CopyTo(memoryStream);
                    itemcardapio.Imagem = memoryStream.ToArray();
                }
                else
                {
                    itemcardapio.Imagem = System.Text.Encoding.UTF8.GetBytes("placeholder");
                }

                itemcardapioService.Edit(itemcardapio);
                return RedirectToAction(nameof(Index));
            }
            return View(viewModel);
        }

        // GET: ItemCardapioController/Delete/5
        public ActionResult Delete(int id)
        {
            var itemcardapio = itemcardapioService.Get(id);
            if (itemcardapio == null)
            {
                return NotFound();
            }

            var itemcardapioViewModel = mapper.Map<ItemcardapioViewModel>(itemcardapio);
            if (itemcardapio.Imagem != null && itemcardapio.Imagem.Length > 0)
            {
                itemcardapioViewModel.ImagemAtual = Convert.ToBase64String(itemcardapio.Imagem);
            }

            return View(itemcardapioViewModel);
        }

        // POST: ItemCardapioController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var itemcardapio = itemcardapioService.Get(id);
            if (itemcardapio == null)
            {
                return NotFound();
            }
            itemcardapioService.Delete(id);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult GetIngredientesUnicos()
        {
            try
            {
                var ingredientes = itemcardapioService.GetAllIngredientes();
                return Ok(ingredientes);
            }
            catch (Exception)
            {
                return StatusCode(500, "Ocorreu um erro interno ao buscar os ingredientes.");
            }
        } 
    }
}
