using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LuanVan.Data;

namespace LuanVan.Controllers
{
    public class ChiendichesController : Controller
    {
        private readonly NienluancosoContext _context;

        public ChiendichesController(NienluancosoContext context)
        {
            _context = context;
        }

        // GET: Chiendiches
        public async Task<IActionResult> Index()
        {
            var nienluancosoContext = _context.Chiendiches.Include(c => c.MaNoiNavigation).Include(c => c.MaTvNavigation);
            return View(await nienluancosoContext.ToListAsync());
        }

        // GET: Chiendiches/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Chiendiches == null)
            {
                return NotFound();
            }

            var chiendich = await _context.Chiendiches
                .Include(c => c.MaNoiNavigation)
                .Include(c => c.MaTvNavigation)
                .FirstOrDefaultAsync(m => m.MaCd == id);
            if (chiendich == null)
            {
                return NotFound();
            }

            return View(chiendich);
        }

        // GET: Chiendiches/Create
        private bool ChiendichExists(int id)
        {
          return (_context.Chiendiches?.Any(e => e.MaCd == id)).GetValueOrDefault();
        }
    }
}
