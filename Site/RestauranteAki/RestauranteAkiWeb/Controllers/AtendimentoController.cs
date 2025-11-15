using AutoMapper;
using Core.Service;
using Microsoft.AspNetCore.Mvc;
using RestauranteAkiWeb.Models;

namespace RestauranteAkiWeb.Controllers
{
    public class AtendimentoController : Controller
    {
        private readonly IMesaService mesaService;
        private readonly IContumService contaService;
        private readonly IMapper mapper;

        public AtendimentoController(IMesaService mesaService, IContumService contaService,IMapper mapper)
        {
            this.mesaService = mesaService;
            this.contaService = contaService;
            this.mapper = mapper;
        }

        public ActionResult Index()
        {
            var mesas = mesaService.GetAll();
            var mesasViewModel = mapper.Map<List<MesaViewModel>>(mesas);

            return View(mesasViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Mesa(int id)
        {
            try
            {
                var conta = await contaService.GetOrCreateContaAtiva(id);
                var viewModel = mapper.Map<ContumViewModel>(conta);

                return View(viewModel);
            }
            catch (Exception ex)
            {
                TempData["Erro"] = $"Erro ao carregar mesa: {ex.Message}";
                return RedirectToAction("Index");
            }
        }
    }
}
