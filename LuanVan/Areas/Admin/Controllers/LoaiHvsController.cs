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
    public class LoaiHvsController : Controller
    {
        private readonly NienluancosoContext _context;

        public LoaiHvsController(NienluancosoContext context)
        {
            _context = context;
        }

        // GET: Admin/LoaiHvs
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetInt32("idtv") == null)
            {
                return RedirectToAction("Login", "ThanhVien");
            }
            var count = _context.Quyens.Where(c => c.MaCn == 2 && c.MaCv == HttpContext.Session.GetInt32("cvtv")).Count();
            if (count == 0)
            {
                return RedirectToAction("norole", "Home");
            }
            return View(await _context.LoaiHvs.Include(m => m.HienVats).ToListAsync());
        }

        // GET: Admin/LoaiHvs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (HttpContext.Session.GetInt32("idtv") == null)
            {
                return RedirectToAction("Login", "ThanhVien");
            }
            var count = _context.Quyens.Where(c => c.MaCn == 2 && c.MaCv == HttpContext.Session.GetInt32("cvtv")).Count();
            if (count == 0)
            {
                return RedirectToAction("norole", "Home");
            }
            if (id == null || _context.LoaiHvs == null)
            {
                return NotFound();
            }

            var loaiHv = await _context.LoaiHvs
                .FirstOrDefaultAsync(m => m.MaLoai == id);
            if (loaiHv == null)
            {
                return NotFound();
            }

            return View(loaiHv);
        }

        // GET: Admin/LoaiHvs/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetInt32("idtv") == null)
            {
                return RedirectToAction("Login", "ThanhVien");
            }
            var count = _context.Quyens.Where(c => c.MaCn == 2 && c.MaCv == HttpContext.Session.GetInt32("cvtv")).Count();
            if (count == 0)
            {
                return RedirectToAction("norole", "Home");
            }
            return View();
        }

        // POST: Admin/LoaiHvs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaLoai,DienGiai")] LoaiHv loaiHv)
        {
            if (ModelState.IsValid)
            {
                var hv = _context.LoaiHvs.FirstOrDefault(t => t.DienGiai == loaiHv.DienGiai);
                if (hv != null)
                {
                    ModelState.AddModelError("DienGiai", "Loại hiện vật đã tồn tại!");
                    return View(loaiHv);
                }
                _context.Add(loaiHv);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(loaiHv);
        }

        // GET: Admin/LoaiHvs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (HttpContext.Session.GetInt32("idtv") == null)
            {
                return RedirectToAction("Login", "ThanhVien");
            }
            var count = _context.Quyens.Where(c => c.MaCn == 2 && c.MaCv == HttpContext.Session.GetInt32("cvtv")).Count();
            if (count == 0)
            {
                return RedirectToAction("norole", "Home");
            }
            if (id == null || _context.LoaiHvs == null)
            {
                return NotFound();
            }

            var loaiHv = await _context.LoaiHvs.FindAsync(id);
            if (loaiHv == null)
            {
                return NotFound();
            }
            return View(loaiHv);
        }

        // POST: Admin/LoaiHvs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaLoai,DienGiai")] LoaiHv loaiHv)
        {
            if (id != loaiHv.MaLoai)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var hv = _context.LoaiHvs.FirstOrDefault(t => t.DienGiai == loaiHv.DienGiai);
                if (hv != null )
                {
                    ModelState.AddModelError("DienGiai", "Loại hiện vật đã tồn tại!");
                    return View(loaiHv);
                }
                try
                {
                    _context.Update(loaiHv);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LoaiHvExists(loaiHv.MaLoai))
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
            return View(loaiHv);
        }

        // GET: Admin/LoaiHvs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (HttpContext.Session.GetInt32("idtv") == null)
            {
                return RedirectToAction("Login", "ThanhVien");
            }
            var count = _context.Quyens.Where(c => c.MaCn == 2 && c.MaCv == HttpContext.Session.GetInt32("cvtv")).Count();
            if (count == 0)
            {
                return RedirectToAction("norole", "Home");
            }
            if (id == null || _context.LoaiHvs == null)
            {
                return NotFound();
            }

            var loaiHv = await _context.LoaiHvs
                .FirstOrDefaultAsync(m => m.MaLoai == id);
            if (loaiHv == null)
            {
                return NotFound();
            }

            return View(loaiHv);
        }

        // POST: Admin/LoaiHvs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.LoaiHvs == null)
            {
                return Problem("Entity set 'NienluancosoContext.LoaiHvs'  is null.");
            }
            var loaiHv = await _context.LoaiHvs.FindAsync(id);
            if (loaiHv != null)
            {
                _context.LoaiHvs.Remove(loaiHv);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LoaiHvExists(int id)
        {
          return _context.LoaiHvs.Any(e => e.MaLoai == id);
        }
    }
}
