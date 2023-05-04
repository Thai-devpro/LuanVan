using LuanVan.Data;
using LuanVan.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace LuanVan.Controllers
{
    public class HomeController : Controller
    {
        private readonly NienluancosoContext _context;

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, NienluancosoContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
          
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
        public async Task<IActionResult> tintucAsync(int? maLoai)
        {
            ViewData["chiendich"] = _context.Chiendiches.Count();
            ViewData["dexuat"] = _context.Noihotros.Count();
            ViewData["quyengop"] = _context.TtQuyengopHienvats.Count();
            ViewData["traotang"] = _context.TtTraotangs.Count();
            var lhv = _context.LoaiHvs.ToList();
            lhv.Insert(0, new LoaiHv { MaLoai = 0, DienGiai = "------------Tất Cả------------" });
            ViewBag.MaLoai = new SelectList(lhv, "MaLoai", "DienGiai", maLoai);
            if (maLoai != 0 && maLoai != null)
            {
                ViewBag.MaLoai = new SelectList(lhv, "MaLoai", "DienGiai", maLoai);
                var nienluancosoContext2 = _context.HienVats.Include(h => h.MaLoaiNavigation).Include(h => h.TtQuyengopHienvats).Include(h => h.TtTraotangs).Where(h => h.MaLoai == maLoai);
                return View(await nienluancosoContext2.ToListAsync());
            }
            var nienluancosoContext = _context.HienVats.Include(h => h.MaLoaiNavigation).Include(h => h.TtQuyengopHienvats).Include(h => h.TtTraotangs);
            return View(await nienluancosoContext.ToListAsync());
         
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}