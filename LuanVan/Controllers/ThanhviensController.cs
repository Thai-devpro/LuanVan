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
    public class ThanhviensController : Controller
    {
        private readonly NienluancosoContext _context;

        public ThanhviensController(NienluancosoContext context)
        {
            _context = context;
        }

        // GET: Thanhviens
        public async Task<IActionResult> Index()
        {
            var nienluancosoContext = _context.Thanhviens.Include(t => t.MaCvNavigation);
            return View(await nienluancosoContext.ToListAsync());
        }

      
    }
}
