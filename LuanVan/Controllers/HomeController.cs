using LuanVan.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace LuanVan.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
          //ViewBag.tkmtq = HttpContext.Session.GetString("tkmtq");
          //ViewBag.idmtq = HttpContext.Session.GetString("idmtq");
            return View();
        }
        public IActionResult About()
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