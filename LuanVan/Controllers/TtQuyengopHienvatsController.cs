using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LuanVan.Data;
using Microsoft.AspNetCore.Authorization;

namespace LuanVan.Controllers
{
    public class TtQuyengopHienvatsController : Controller
    {
        private readonly NienluancosoContext _context;

        public TtQuyengopHienvatsController(NienluancosoContext context)
        {
            _context = context;
        }

        // GET: TtQuyengopHienvats
        public async Task<IActionResult> Index()
        {
            var nienluancosoContext = _context.TtQuyengopHienvats.Include(t => t.MaCdNavigation).Include(t => t.MaHvNavigation).Include(t => t.MaMtqNavigation).Include(t => t.MaTvNavigation);
            return View(await nienluancosoContext.ToListAsync());
        }

        // GET: TtQuyengopHienvats/Details/5
        public async Task<IActionResult> Details(int? id)
        {
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

        // GET: TtQuyengopHienvats/Create
        
        public IActionResult Create(int id)
        {
            if(HttpContext.Session.GetInt32("idmtq")== null)
                return RedirectToAction("Register", "User");
            else if (id != null)
                {
                    ViewData["MaHv"] = new SelectList(_context.HienVats, "MaHv", "TenHv");
                
      
                    ViewData["MaCd"] = new SelectList(_context.Chiendiches.Where(s => s.MaCd == id), "MaCd", "TenCd");
                ViewData["MaMtq"] = new SelectList(_context.Manhthuongquans.Where(s => s.MaMtq == HttpContext.Session.GetInt32("idmtq")), "MaMtq", "HotenMtq");
              
                return View();
                }
           
           
            return NotFound();
        }

        // POST: TtQuyengopHienvats/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaQghv,MaHv,MaMtq,MaCd,Ghichu,SoluongQg,TrangthaiHv,NgayQg")] TtQuyengopHienvat ttQuyengopHienvat)
        {
            if (ttQuyengopHienvat.SoluongQg == null || ttQuyengopHienvat.SoluongQg <= 0)
            {
                ModelState.AddModelError("SoLuongQg", "Vui lòng điền số lượng ?");
                ViewData["MaHv"] = new SelectList(_context.HienVats, "MaHv", "TenHv");


                ViewData["MaCd"] = new SelectList(_context.Chiendiches.Where(s => s.MaCd == ttQuyengopHienvat.MaCd), "MaCd", "TenCd");
                ViewData["MaMtq"] = new SelectList(_context.Manhthuongquans.Where(s => s.MaMtq == HttpContext.Session.GetInt32("idmtq")), "MaMtq", "HotenMtq");
                return View(ttQuyengopHienvat);

                
            }
            if (ttQuyengopHienvat.Ghichu == null )
            {
                ModelState.AddModelError("Ghichu", "Hãy viết gì đó cho chúng tôi");
                ViewData["MaHv"] = new SelectList(_context.HienVats, "MaHv", "TenHv");


                ViewData["MaCd"] = new SelectList(_context.Chiendiches.Where(s => s.MaCd == ttQuyengopHienvat.MaCd), "MaCd", "TenCd");
                ViewData["MaMtq"] = new SelectList(_context.Manhthuongquans.Where(s => s.MaMtq == HttpContext.Session.GetInt32("idmtq")), "MaMtq", "HotenMtq");
                return View(ttQuyengopHienvat);


            }
            ttQuyengopHienvat.NgayQg = DateTime.Now;
            ttQuyengopHienvat.TrangthaiHv = "Chưa nhận";
            _context.Add(ttQuyengopHienvat);
            await _context.SaveChangesAsync();
            return RedirectToAction("CamOn", new { id = ttQuyengopHienvat.MaQghv });
        }

        public async Task<IActionResult> CamOn(int id)
        {
            var ttqg =  _context.TtQuyengopHienvats.Include(r => r.MaCdNavigation).Include(r => r.MaMtqNavigation).Include(r => r.MaHvNavigation).FirstOrDefault(s => s.MaQghv == id);
            return View(ttqg);
        }

        // GET: TtQuyengopHienvats/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TtQuyengopHienvats == null)
            {
                return NotFound();
            }

            var ttQuyengopHienvat = await _context.TtQuyengopHienvats.FindAsync(id);
            if (ttQuyengopHienvat == null)
            {
                return NotFound();
            }
            ViewData["MaCd"] = new SelectList(_context.Chiendiches, "MaCd", "TenCd", ttQuyengopHienvat.MaCd);
            ViewData["MaHv"] = new SelectList(_context.HienVats, "MaHv", "TenHv", ttQuyengopHienvat.MaHv);
            ViewData["MaMtq"] = new SelectList(_context.Manhthuongquans.Where(s => s.MaMtq == HttpContext.Session.GetInt32("idmtq")), "MaMtq", "HotenMtq", ttQuyengopHienvat.MaMtq);
            return View(ttQuyengopHienvat);
        }

        // POST: TtQuyengopHienvats/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaQghv,MaHv,MaMtq,MaCd,Ghichu,SoluongQg,TrangthaiHv,NgayQg")] TtQuyengopHienvat ttQuyengopHienvat)
        {
            if (id != ttQuyengopHienvat.MaQghv)
            {
                return NotFound();
            }
            var ttqg = _context.TtQuyengopHienvats.AsNoTracking().SingleOrDefault(s => s.MaQghv == ttQuyengopHienvat.MaQghv);
            if (ttQuyengopHienvat.SoluongQg == null || ttQuyengopHienvat.SoluongQg <= 0)
            {
                ModelState.AddModelError("SoLuongQg", "Vui lòng điền số lượng ?");
                ViewData["MaCd"] = new SelectList(_context.Chiendiches, "MaCd", "TenCd", ttQuyengopHienvat.MaCd);
                ViewData["MaHv"] = new SelectList(_context.HienVats, "MaHv", "TenHv", ttQuyengopHienvat.MaHv);
                ViewData["MaMtq"] = new SelectList(_context.Manhthuongquans, "MaMtq", "HotenMtq", ttQuyengopHienvat.MaMtq);
                return View(ttQuyengopHienvat);


            }
            if (ttQuyengopHienvat.Ghichu == null)
            {
                ModelState.AddModelError("Ghichu", "Hãy viết gì đó cho chúng tôi");
                ViewData["MaCd"] = new SelectList(_context.Chiendiches, "MaCd", "TenCd", ttQuyengopHienvat.MaCd);
                ViewData["MaHv"] = new SelectList(_context.HienVats, "MaHv", "TenHv", ttQuyengopHienvat.MaHv);
                ViewData["MaMtq"] = new SelectList(_context.Manhthuongquans, "MaMtq", "HotenMtq", ttQuyengopHienvat.MaMtq);
                return View(ttQuyengopHienvat);


            }


            try
                {
                ttQuyengopHienvat.NgayQg = ttqg.NgayQg;
                ttQuyengopHienvat.TrangthaiHv = "Chưa nhận";
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

        // GET: TtQuyengopHienvats/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
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

        // POST: TtQuyengopHienvats/Delete/5
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
          return _context.TtQuyengopHienvats.Any(e => e.MaQghv == id);
        }
    }
}
