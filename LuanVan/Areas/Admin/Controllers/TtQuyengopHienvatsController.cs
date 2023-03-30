using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LuanVan.Data;
using Microsoft.CodeAnalysis;

namespace LuanVan.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TtQuyengopHienvatsController : Controller
    {
        private readonly NienluancosoContext _context;

        public TtQuyengopHienvatsController(NienluancosoContext context)
        {
            _context = context;
        }
        
        // GET: Admin/TtQuyengopHienvats
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetInt32("idtv") == null)
            {
                return RedirectToAction("Login", "ThanhVien");
            }
            var nienluancosoContext = _context.TtQuyengopHienvats.Include(t => t.MaCdNavigation).Include(t => t.MaHvNavigation).Include(t => t.MaMtqNavigation).Include(t => t.MaTvNavigation);
            return View(await nienluancosoContext.ToListAsync());
        }

        // GET: Admin/TtQuyengopHienvats/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (HttpContext.Session.GetInt32("idtv") == null)
            {
                return RedirectToAction("Login", "ThanhVien");
            }
            if (id == null || _context.TtQuyengopHienvats == null)
            {
                return NotFound();
            }

            var ttQuyengopHienvat = await _context.TtQuyengopHienvats
                .Include(t => t.MaCdNavigation)
                .Include(t => t.MaHvNavigation)
                .Include(t => t.MaMtqNavigation)
                .FirstOrDefaultAsync(m => m.MaQghv == id);
            if (ttQuyengopHienvat == null)
            {
                return NotFound();
            }

            return View(ttQuyengopHienvat);
        }

        // GET: Admin/TtQuyengopHienvats/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetInt32("idtv") == null)
            {
                return RedirectToAction("Login", "ThanhVien");
            }
            ViewData["MaCd"] = new SelectList(_context.Chiendiches, "MaCd", "MaCd");
            ViewData["MaHv"] = new SelectList(_context.HienVats, "MaHv", "MaHv");
            ViewData["MaMtq"] = new SelectList(_context.Manhthuongquans, "MaMtq", "MaMtq");
            return View();
        }

        // POST: Admin/TtQuyengopHienvats/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaQghv,MaHv,MaMtq,MaCd,Ghichu,SoluongQg,TrangthaiHv,NgayQg")] TtQuyengopHienvat ttQuyengopHienvat)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ttQuyengopHienvat);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaCd"] = new SelectList(_context.Chiendiches, "MaCd", "MaCd", ttQuyengopHienvat.MaCd);
            ViewData["MaHv"] = new SelectList(_context.HienVats, "MaHv", "MaHv", ttQuyengopHienvat.MaHv);
            ViewData["MaMtq"] = new SelectList(_context.Manhthuongquans, "MaMtq", "MaMtq", ttQuyengopHienvat.MaMtq);
            return View(ttQuyengopHienvat);
        }

        // GET: Admin/TtQuyengopHienvats/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (HttpContext.Session.GetInt32("idtv") == null)
            {
                return RedirectToAction("Login", "ThanhVien");
            }
            if (id == null || _context.TtQuyengopHienvats == null)
            {
                return NotFound();
            }

            var ttQuyengopHienvat = await _context.TtQuyengopHienvats.Include(n => n.MaMtqNavigation).Include(n => n.MaHvNavigation).FirstOrDefaultAsync(n => n.MaQghv == id);
            if (ttQuyengopHienvat == null)
            {
                return NotFound();
            }
            ViewData["MaCd"] = new SelectList(_context.Chiendiches.Where(s => s.MaCd == ttQuyengopHienvat.MaCd), "MaCd", "TenCd");
            ViewData["MaHv"] = new SelectList(_context.HienVats.Where(s => s.MaHv == ttQuyengopHienvat.MaHv), "MaHv", "TenHv");
            ViewData["MaMtq"] = new SelectList(_context.Manhthuongquans.Where(s => s.MaMtq == ttQuyengopHienvat.MaMtq), "MaMtq", "HotenMtq");
            ViewData["MaTv"] = new SelectList(_context.Thanhviens.ToList(), "MaTv", "TenTv");
            return View(ttQuyengopHienvat);
        }

        // POST: Admin/TtQuyengopHienvats/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaQghv,MaHv,MaTv,MaMtq,MaCd,Ghichu,SoluongQg,TrangthaiHv,NgayQg")] TtQuyengopHienvat ttQuyengopHienvat)
        {
            if (id != ttQuyengopHienvat.MaQghv)
            {
                return NotFound();
            }
            if (ttQuyengopHienvat.SoluongQg == null || ttQuyengopHienvat.SoluongQg <= 0)
            {
                ModelState.AddModelError("SoLuongQg", "Vui lòng điền số lượng ?");
                ViewData["MaCd"] = new SelectList(_context.Chiendiches.Where(s => s.MaCd == ttQuyengopHienvat.MaCd), "MaCd", "TenCd");
                ViewData["MaHv"] = new SelectList(_context.HienVats.Where(s => s.MaHv == ttQuyengopHienvat.MaHv), "MaHv", "TenHv");
                ViewData["MaMtq"] = new SelectList(_context.Manhthuongquans.Where(s => s.MaMtq == ttQuyengopHienvat.MaMtq), "MaMtq", "HotenMtq");
                ViewData["MaTv"] = new SelectList(_context.Thanhviens.ToList(), "MaTv", "TenTv");
                return View(ttQuyengopHienvat);


            }
            if (ttQuyengopHienvat.Ghichu == null)
            {
                ModelState.AddModelError("Ghichu", "Vui lòng thêm ghi chú ?");
                ViewData["MaCd"] = new SelectList(_context.Chiendiches.Where(s => s.MaCd == ttQuyengopHienvat.MaCd), "MaCd", "TenCd");
                ViewData["MaHv"] = new SelectList(_context.HienVats.Where(s => s.MaHv == ttQuyengopHienvat.MaHv), "MaHv", "TenHv");
                ViewData["MaMtq"] = new SelectList(_context.Manhthuongquans.Where(s => s.MaMtq == ttQuyengopHienvat.MaMtq), "MaMtq", "HotenMtq");
                ViewData["MaTv"] = new SelectList(_context.Thanhviens.ToList(), "MaTv", "TenTv");
                return View(ttQuyengopHienvat);


            }

            try
                {
                
               var ttqg = _context.TtQuyengopHienvats.AsNoTracking().FirstOrDefault(od => od.MaQghv == ttQuyengopHienvat.MaQghv);
                var cd = _context.Chiendiches.AsNoTracking().FirstOrDefault(c => c.MaCd == ttQuyengopHienvat.MaCd);
                if (ttQuyengopHienvat.TrangthaiHv == "Đã nhận" && ttqg.TrangthaiHv.Trim() == "Chưa nhận" && cd.MaNoi == null)
                {

                    var hv = _context.HienVats.FirstOrDefault(od => od.MaHv == ttQuyengopHienvat.MaHv);
                    hv.Soluongcon += ttQuyengopHienvat.SoluongQg;
                    await _context.SaveChangesAsync();
                }
                
                if (ttqg.TrangthaiHv.Trim() == "Đã nhận" && ttQuyengopHienvat.TrangthaiHv == "Chưa nhận" && cd.MaNoi == null)
                {
                    var hv = _context.HienVats.FirstOrDefault(od => od.MaHv == ttQuyengopHienvat.MaHv);
                    hv.Soluongcon -= ttQuyengopHienvat.SoluongQg;
                    await _context.SaveChangesAsync();
                    ttQuyengopHienvat.MaTv = null;
                }
                      
                    
                    _context.Update(ttQuyengopHienvat);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TtQuyengopHienvatExists(ttQuyengopHienvat.MaQghv))
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

        // GET: Admin/TtQuyengopHienvats/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (HttpContext.Session.GetInt32("idtv") == null)
            {
                return RedirectToAction("Login", "ThanhVien");
            }
            if (id == null || _context.TtQuyengopHienvats == null)
            {
                return NotFound();
            }

            var ttQuyengopHienvat = await _context.TtQuyengopHienvats
                .Include(t => t.MaCdNavigation)
                .Include(t => t.MaHvNavigation)
                .Include(t => t.MaMtqNavigation)
                .FirstOrDefaultAsync(m => m.MaQghv == id);
            if (ttQuyengopHienvat == null)
            {
                return NotFound();
            }

            return View(ttQuyengopHienvat);
        }

        // POST: Admin/TtQuyengopHienvats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TtQuyengopHienvats == null)
            {
                return Problem("Entity set 'NienluancosoContext.TtQuyengopHienvats'  is null.");
            }
            var ttQuyengopHienvat = await _context.TtQuyengopHienvats.FindAsync(id);
            if (ttQuyengopHienvat != null)
            {
                _context.TtQuyengopHienvats.Remove(ttQuyengopHienvat);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TtQuyengopHienvatExists(int id)
        {
          return (_context.TtQuyengopHienvats?.Any(e => e.MaQghv == id)).GetValueOrDefault();
        }
    }
}
