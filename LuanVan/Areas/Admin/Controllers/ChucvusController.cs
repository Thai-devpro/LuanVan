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
            if (HttpContext.Session.GetInt32("idtv") == null)
            {
                return RedirectToAction("Login", "ThanhVien");
            }
            var count = _context.Quyens.Where(c => c.MaCn == 9 && c.MaCv == HttpContext.Session.GetInt32("cvtv")).Count();
            if (count == 0)
            {
                return RedirectToAction("norole", "Home");
            }
            var chucvu = _context.Chucvus.Include(c => c.Quyens).Include(c => c.Thanhviens);

           

            return View(await chucvu.ToListAsync());

          
        }
        public async Task<IActionResult> ThemQuyen(int id)
        {
            if (HttpContext.Session.GetInt32("idtv") == null)
            {
                return RedirectToAction("Login", "ThanhVien");
            }
            var count = _context.Quyens.Where(c => c.MaCn == 9 && c.MaCv == HttpContext.Session.GetInt32("cvtv")).Count();
            if (count == 0)
            {
                return RedirectToAction("norole", "Home");
            }
            var chucvu = _context.Chucvus.Find(id);
            if (chucvu == null)
            {
                return NotFound();
            }

            var chucnangs = _context.Chucnangs.Include(c => c.Quyens);

            HttpContext.Session.SetInt32("macv", chucvu.MaCv);

            return View(await chucnangs.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> ThemQuyen(int id, int[] quyen)
        {
            var chucvu = _context.Chucvus.Find(id);
            if (chucvu == null)
            {
                return NotFound();
            }

            // Xóa các quyền cũ
            var quyens = _context.Quyens.Where(q => q.MaCv == id).ToList();
            foreach (var q in quyens)
            {
                _context.Quyens.Remove(q);
            }

            // Thêm quyền mới
            if (quyen != null)
            {
                foreach (var maCn in quyen)
                {
                    var chucnang = _context.Chucnangs.Find(maCn);
                    if (chucnang != null)
                    {
                        var q = new Quyen { MaCv = id, MaCn = maCn };
                        _context.Quyens.Add(q);
                    }
                }
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Chucvus/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (HttpContext.Session.GetInt32("idtv") == null)
            {
                return RedirectToAction("Login", "ThanhVien");
            }
            var count = _context.Quyens.Where(c => c.MaCn == 9 && c.MaCv == HttpContext.Session.GetInt32("cvtv")).Count();
            if (count == 0)
            {
                return RedirectToAction("norole", "Home");
            }
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
            if (HttpContext.Session.GetInt32("idtv") == null)
            {
                return RedirectToAction("Login", "ThanhVien");
            }
            var count = _context.Quyens.Where(c => c.MaCn == 9 && c.MaCv == HttpContext.Session.GetInt32("cvtv")).Count();
            if (count == 0)
            {
                return RedirectToAction("norole", "Home");
            }
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
                var cv = _context.Chucvus.FirstOrDefault(t => t.TenCv == chucvu.TenCv);
                if (cv != null)
                {
                    ModelState.AddModelError("DienGiai", "chức vụ đã tồn tại!");
                    return View(chucvu);
                }
                _context.Add(chucvu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(chucvu);
        }

        // GET: Admin/Chucvus/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (HttpContext.Session.GetInt32("idtv") == null)
            {
                return RedirectToAction("Login", "ThanhVien");
            }
            var count = _context.Quyens.Where(c => c.MaCn == 9 && c.MaCv == HttpContext.Session.GetInt32("cvtv")).Count();
            if (count == 0)
            {
                return RedirectToAction("norole", "Home");
            }
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
                var cv = _context.Chucvus.FirstOrDefault(t => t.TenCv == chucvu.TenCv);
                if (cv != null && cv.MaCv != chucvu.MaCv)
                {
                    ModelState.AddModelError("DienGiai", "chức vụ đã tồn tại!");
                    return View(chucvu);
                }
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
            if (HttpContext.Session.GetInt32("idtv") == null)
            {
                return RedirectToAction("Login", "ThanhVien");
            }
            var count = _context.Quyens.Where(c => c.MaCn == 9 && c.MaCv == HttpContext.Session.GetInt32("cvtv")).Count();
            if (count == 0)
            {
                return RedirectToAction("norole", "Home");
            }
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
