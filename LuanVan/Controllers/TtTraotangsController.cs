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
    public class TtTraotangsController : Controller
    {
        private readonly NienluancosoContext _context;

        public TtTraotangsController(NienluancosoContext context)
        {
            _context = context;
        }

        // GET: TtTraotangs
        public async Task<IActionResult> Index()
        {
            var nienluancosoContext = _context.TtTraotangs.Include(t => t.MaCdNavigation).Include(t => t.MaHvNavigation).Include(t => t.MaTvNavigation).Include(t => t.ManoiNavigation);
            return View(await nienluancosoContext.ToListAsync());
        }

        // GET: TtTraotangs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TtTraotangs == null)
            {
                return NotFound();
            }

            var ttTraotang = await _context.TtTraotangs
                .Include(t => t.MaCdNavigation)
                .Include(t => t.MaHvNavigation)
                .Include(t => t.MaTvNavigation)
                .Include(t => t.ManoiNavigation)
                .FirstOrDefaultAsync(m => m.MaTt == id);
            if (ttTraotang == null)
            {
                return NotFound();
            }

            return View(ttTraotang);
        }

        
        
    }
}
