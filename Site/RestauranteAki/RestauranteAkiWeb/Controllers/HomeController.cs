using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestauranteAkiWeb.Models;
using System.Diagnostics;

namespace RestauranteAkiWeb.Controllers
{
    [AllowAnonymous] //permite deixa a página home publica(não exige autenticação)
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
