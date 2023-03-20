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
    public class ChucvusController : Controller
    {
        private readonly NienluancosoContext _context;

        public ChucvusController(NienluancosoContext context)
        {
            _context = context;
        }

        // GET: Admin/Chucvus
        public async Task<IActionResult> Index()
        {
              return View(await _context.Chucvus.ToListAsync());
        }

        // GET: Admin/Chucvus/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Chucvus == null)
            {
                return NotFound();
            }

            var chucvu = await _context.Chucvus
                .FirstOrDefaultAsync(m => m.MaCv == id);
            if (chucvu == null)
            {
                return NotFound();
            }

            return View(chucvu);
        }

        // GET: Admin/Chucvus/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Chucvus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaCv,TenCv")] Chucvu chucvu)
        {
            if (ModelState.IsValid)
            {
                _context.Add(chucvu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(chucvu);
        }

        // GET: Admin/Chucvus/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Chucvus == null)
            {
                return NotFound();
            }

            var chucvu = await _context.Chucvus.FindAsync(id);
            if (chucvu == null)
            {
                return NotFound();
            }
            return View(chucvu);
        }

        // POST: Admin/Chucvus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaCv,TenCv")] Chucvu chucvu)
        {
            if (id != chucvu.MaCv)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chucvu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChucvuExists(chucvu.MaCv))
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
            return View(chucvu);
        }

        // GET: Admin/Chucvus/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Chucvus == null)
            {
                return NotFound();
            }

            var chucvu = await _context.Chucvus
                .FirstOrDefaultAsync(m => m.MaCv == id);
            if (chucvu == null)
            {
                return NotFound();
            }

            return View(chucvu);
        }

        // POST: Admin/Chucvus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Chucvus == null)
            {
                return Problem("Entity set 'NienluancosoContext.Chucvus'  is null.");
            }
            var chucvu = await _context.Chucvus.FindAsync(id);
            if (chucvu != null)
            {
                _context.Chucvus.Remove(chucvu);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChucvuExists(int id)
        {
          return _context.Chucvus.Any(e => e.MaCv == id);
        }
    }
}
