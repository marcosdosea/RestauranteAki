using AutoMapper;
using Core.Service;
using Microsoft.AspNetCore.Mvc;
using RestauranteAkiWeb.Models;
using Service;

namespace RestauranteAkiWeb.Controllers
{
    [Route("[controller]")]
    public class CategoriaController : Controller
    {
        private readonly IItemcardapioService itemcardapioService;
        private readonly IMapper mapper;

        public CategoriaController(IItemcardapioService itemcardapioService, IMapper mapper)
        {
            this.itemcardapioService = itemcardapioService;
            this.mapper = mapper;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("{categoria}")]
        public IActionResult Itens(int categoria)
        {
            var listaPedidoitemcardapio = itemcardapioService.GetByCategoria(categoria);
            var PedidoitemcardapioViewModel = mapper.Map<List<ItemcardapioViewModel>>(listaPedidoitemcardapio);
            return View(PedidoitemcardapioViewModel);
        }
    }
}
