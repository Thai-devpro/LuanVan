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
    public class HienVatController : Controller
    {
        private readonly NienluancosoContext _context;

        public HienVatController(NienluancosoContext context)
        {
            _context = context;
        }

        // GET: Admin/HienVat
        public async Task<IActionResult> Index()
        {
            var nienluancosoContext = _context.HienVats.Include(h => h.MaLoaiNavigation);
            return View(await nienluancosoContext.ToListAsync());
        }

        // GET: Admin/HienVat/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.HienVats == null)
            {
                return NotFound();
            }

            var hienVat = await _context.HienVats
                .Include(h => h.MaLoaiNavigation)
                .FirstOrDefaultAsync(m => m.MaHv == id);
            if (hienVat == null)
            {
                return NotFound();
            }

            return View(hienVat);
        }

        // GET: Admin/HienVat/Create
        public IActionResult Create()
        {
            ViewData["MaLoai"] = new SelectList(_context.LoaiHvs, "MaLoai", "MaLoai");
            return View();
        }

        // POST: Admin/HienVat/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaLoai,MaHv,TenHv,Donvitinh,Soluongcon,Gia")] HienVat hienVat)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hienVat);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaLoai"] = new SelectList(_context.LoaiHvs, "MaLoai", "MaLoai", hienVat.MaLoai);
            return View(hienVat);
        }

        // GET: Admin/HienVat/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.HienVats == null)
            {
                return NotFound();
            }

            var hienVat = await _context.HienVats.FindAsync(id);
            if (hienVat == null)
            {
                return NotFound();
            }
            ViewData["MaLoai"] = new SelectList(_context.LoaiHvs, "MaLoai", "MaLoai", hienVat.MaLoai);
            return View(hienVat);
        }

        // POST: Admin/HienVat/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaLoai,MaHv,TenHv,Donvitinh,Soluongcon,Gia")] HienVat hienVat)
        {
            if (id != hienVat.MaHv)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hienVat);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HienVatExists(hienVat.MaHv))
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
            ViewData["MaLoai"] = new SelectList(_context.LoaiHvs, "MaLoai", "MaLoai", hienVat.MaLoai);
            return View(hienVat);
        }

        // GET: Admin/HienVat/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.HienVats == null)
            {
                return NotFound();
            }

            var hienVat = await _context.HienVats
                .Include(h => h.MaLoaiNavigation)
                .FirstOrDefaultAsync(m => m.MaHv == id);
            if (hienVat == null)
            {
                return NotFound();
            }

            return View(hienVat);
        }

        // POST: Admin/HienVat/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.HienVats == null)
            {
                return Problem("Entity set 'NienluancosoContext.HienVats'  is null.");
            }
            var hienVat = await _context.HienVats.FindAsync(id);
            if (hienVat != null)
            {
                _context.HienVats.Remove(hienVat);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HienVatExists(int id)
        {
          return (_context.HienVats?.Any(e => e.MaHv == id)).GetValueOrDefault();
        }
    }
}
