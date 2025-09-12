using AutoMapper;
using Core.Service;
using Microsoft.AspNetCore.Mvc;
using RestauranteAkiWeb.Models;

namespace RestauranteAkiWeb.Controllers
{
    public class AtendimentoController : Controller
    {
        private readonly IMesaService mesaService;
        private readonly IMapper mapper;

        public AtendimentoController(IMesaService mesaService, IMapper mapper)
        {
            this.mesaService = mesaService;
            this.mapper = mapper;
        }

        public ActionResult Index()
        {
            var mesas = mesaService.GetAll();
            var mesasViewModel = mapper.Map<List<MesaViewModel>>(mesas);

            return View(mesasViewModel);
        }
    }
}
