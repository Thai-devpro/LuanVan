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

        // GET: TtTraotangs/Create
        public IActionResult Create()
        {
            ViewData["MaCd"] = new SelectList(_context.Chiendiches, "MaCd", "MaCd");
            ViewData["MaHv"] = new SelectList(_context.HienVats, "MaHv", "MaHv");
            ViewData["MaTv"] = new SelectList(_context.Thanhviens, "MaTv", "MaTv");
            ViewData["Manoi"] = new SelectList(_context.Noihotros, "Manoi", "Manoi");
            return View();
        }

        // POST: TtTraotangs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaTt,Manoi,MaHv,MaCd,SoluongTt,Ngaytang,AnhTt,MaTv")] TtTraotang ttTraotang)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ttTraotang);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaCd"] = new SelectList(_context.Chiendiches, "MaCd", "MaCd", ttTraotang.MaCd);
            ViewData["MaHv"] = new SelectList(_context.HienVats, "MaHv", "MaHv", ttTraotang.MaHv);
            ViewData["MaTv"] = new SelectList(_context.Thanhviens, "MaTv", "MaTv", ttTraotang.MaTv);
            ViewData["Manoi"] = new SelectList(_context.Noihotros, "Manoi", "Manoi", ttTraotang.Manoi);
            return View(ttTraotang);
        }

        // GET: TtTraotangs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TtTraotangs == null)
            {
                return NotFound();
            }

            var ttTraotang = await _context.TtTraotangs.FindAsync(id);
            if (ttTraotang == null)
            {
                return NotFound();
            }
            ViewData["MaCd"] = new SelectList(_context.Chiendiches, "MaCd", "MaCd", ttTraotang.MaCd);
            ViewData["MaHv"] = new SelectList(_context.HienVats, "MaHv", "MaHv", ttTraotang.MaHv);
            ViewData["MaTv"] = new SelectList(_context.Thanhviens, "MaTv", "MaTv", ttTraotang.MaTv);
            ViewData["Manoi"] = new SelectList(_context.Noihotros, "Manoi", "Manoi", ttTraotang.Manoi);
            return View(ttTraotang);
        }

        // POST: TtTraotangs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaTt,Manoi,MaHv,MaCd,SoluongTt,Ngaytang,AnhTt,MaTv")] TtTraotang ttTraotang)
        {
            if (id != ttTraotang.MaTt)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ttTraotang);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TtTraotangExists(ttTraotang.MaTt))
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
            ViewData["MaCd"] = new SelectList(_context.Chiendiches, "MaCd", "MaCd", ttTraotang.MaCd);
            ViewData["MaHv"] = new SelectList(_context.HienVats, "MaHv", "MaHv", ttTraotang.MaHv);
            ViewData["MaTv"] = new SelectList(_context.Thanhviens, "MaTv", "MaTv", ttTraotang.MaTv);
            ViewData["Manoi"] = new SelectList(_context.Noihotros, "Manoi", "Manoi", ttTraotang.Manoi);
            return View(ttTraotang);
        }

        // GET: TtTraotangs/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: TtTraotangs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TtTraotangs == null)
            {
                return Problem("Entity set 'NienluancosoContext.TtTraotangs'  is null.");
            }
            var ttTraotang = await _context.TtTraotangs.FindAsync(id);
            if (ttTraotang != null)
            {
                _context.TtTraotangs.Remove(ttTraotang);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TtTraotangExists(int id)
        {
          return (_context.TtTraotangs?.Any(e => e.MaTt == id)).GetValueOrDefault();
        }
    }
}
