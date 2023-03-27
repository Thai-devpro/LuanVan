using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LuanVan.Data;

namespace LuanVan.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class NoihotroesController : Controller
    {
        private readonly NienluancosoContext _context;

        public NoihotroesController(NienluancosoContext context)
        {
            _context = context;
        }

        // GET: Admin/Noihotroes
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetInt32("idtv") == null)
            {
                return RedirectToAction("Login", "ThanhVien");
            }
            var nienluancosoContext = _context.Noihotros.Include(n => n.MaMtqNavigation).Include(n => n.MaTvNavigation);
            return View(await nienluancosoContext.ToListAsync());
        }

        // GET: Admin/Noihotroes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Noihotros == null)
            {
                return NotFound();
            }

            var noihotro = await _context.Noihotros
                .Include(n => n.MaMtqNavigation)
                .Include(n => n.MaTvNavigation)
                .FirstOrDefaultAsync(m => m.Manoi == id);
            if (noihotro == null)
            {
                return NotFound();
            }

            return View(noihotro);
        }

        // GET: Admin/Noihotroes/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetInt32("idtv") == null)
            {
                return RedirectToAction("Login", "ThanhVien");
            }
            ViewData["MaMtq"] = new SelectList(_context.Manhthuongquans, "MaMtq", "MaMtq");
            ViewData["MaTv"] = new SelectList(_context.Thanhviens, "MaTv", "MaTv");
            return View();
        }

        // POST: Admin/Noihotroes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Manoi,MaMtq,Diachi,Tinhtrang,Canhotro,TrangthaiNht,MaTv,AnhNth")] Noihotro noihotro)
        {
            if (ModelState.IsValid)
            {
                _context.Add(noihotro);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaMtq"] = new SelectList(_context.Manhthuongquans, "MaMtq", "MaMtq", noihotro.MaMtq);
            ViewData["MaTv"] = new SelectList(_context.Thanhviens, "MaTv", "MaTv", noihotro.MaTv);
            return View(noihotro);
        }

        // GET: Admin/Noihotroes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (HttpContext.Session.GetInt32("idtv") == null)
            {
                return RedirectToAction("Login", "ThanhVien");
            }
            if (id == null || _context.Noihotros == null)
            {
                return NotFound();
            }

            var noihotro = await _context.Noihotros.FindAsync(id);
            if (noihotro == null)
            {
                return NotFound();
            }
            ViewData["MaMtq"] = new SelectList(_context.Manhthuongquans, "MaMtq", "MaMtq", noihotro.MaMtq);
            ViewData["MaTv"] = new SelectList(_context.Thanhviens, "MaTv", "MaTv", noihotro.MaTv);
            return View(noihotro);
        }

        // POST: Admin/Noihotroes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Manoi,MaMtq,Diachi,Tinhtrang,Canhotro,TrangthaiNht,MaTv,AnhNth")] Noihotro noihotro)
        {
            if (id != noihotro.Manoi)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(noihotro);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NoihotroExists(noihotro.Manoi))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaMtq"] = new SelectList(_context.Manhthuongquans, "MaMtq", "MaMtq", noihotro.MaMtq);
            ViewData["MaTv"] = new SelectList(_context.Thanhviens, "MaTv", "MaTv", noihotro.MaTv);
            return View(noihotro);
        }

        // GET: Admin/Noihotroes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (HttpContext.Session.GetInt32("idtv") == null)
            {
                return RedirectToAction("Login", "ThanhVien");
            }
            if (id == null || _context.Noihotros == null)
            {
                return NotFound();
            }

            var noihotro = await _context.Noihotros
                .Include(n => n.MaMtqNavigation)
                .Include(n => n.MaTvNavigation)
                .FirstOrDefaultAsync(m => m.Manoi == id);
            if (noihotro == null)
            {
                return NotFound();
            }

            return View(noihotro);
        }

        // POST: Admin/Noihotroes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Noihotros == null)
            {
                return Problem("Entity set 'NienluancosoContext.Noihotros'  is null.");
            }
            var noihotro = await _context.Noihotros.FindAsync(id);
            if (noihotro != null)
            {
                _context.Noihotros.Remove(noihotro);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NoihotroExists(int id)
        {
          return (_context.Noihotros?.Any(e => e.Manoi == id)).GetValueOrDefault();
        }
    }
}
