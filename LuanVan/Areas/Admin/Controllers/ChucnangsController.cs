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
    public class ChucnangsController : Controller
    {
        private readonly NienluancosoContext _context;

        public ChucnangsController(NienluancosoContext context)
        {
            _context = context;
        }

        // GET: Admin/Chucnangs
        public async Task<IActionResult> Index()
        {
              return View(await _context.Chucnangs.ToListAsync());
        }

        // GET: Admin/Chucnangs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Chucnangs == null)
            {
                return NotFound();
            }

            var chucnang = await _context.Chucnangs
                .FirstOrDefaultAsync(m => m.MaCn == id);
            if (chucnang == null)
            {
                return NotFound();
            }

            return View(chucnang);
        }

        // GET: Admin/Chucnangs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Chucnangs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaCn,TenCn")] Chucnang chucnang)
        {
            if (ModelState.IsValid)
            {
                _context.Add(chucnang);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(chucnang);
        }

        // GET: Admin/Chucnangs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Chucnangs == null)
            {
                return NotFound();
            }

            var chucnang = await _context.Chucnangs.FindAsync(id);
            if (chucnang == null)
            {
                return NotFound();
            }
            return View(chucnang);
        }

        // POST: Admin/Chucnangs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaCn,TenCn")] Chucnang chucnang)
        {
            if (id != chucnang.MaCn)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chucnang);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChucnangExists(chucnang.MaCn))
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
            return View(chucnang);
        }

        // GET: Admin/Chucnangs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Chucnangs == null)
            {
                return NotFound();
            }

            var chucnang = await _context.Chucnangs
                .FirstOrDefaultAsync(m => m.MaCn == id);
            if (chucnang == null)
            {
                return NotFound();
            }

            return View(chucnang);
        }

        // POST: Admin/Chucnangs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Chucnangs == null)
            {
                return Problem("Entity set 'NienluancosoContext.Chucnangs'  is null.");
            }
            var chucnang = await _context.Chucnangs.FindAsync(id);
            if (chucnang != null)
            {
                _context.Chucnangs.Remove(chucnang);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChucnangExists(int id)
        {
          return _context.Chucnangs.Any(e => e.MaCn == id);
        }
    }
}
