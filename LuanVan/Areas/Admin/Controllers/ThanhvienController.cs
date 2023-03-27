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
    public class ThanhvienController : Controller
    {
        private readonly NienluancosoContext _context;

        public ThanhvienController(NienluancosoContext context)
        {
            _context = context;
        }
        public async Task<ActionResult> Login()
        {


            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> Login(Thanhvien tv)
        {

            if (tv.SdtTv == null)
            {
                ModelState.AddModelError("SdtTv", "Số điện thoại không được trống!");

                return View(tv);
            }
            if (tv.MatkhauTv == null)
            {
                ModelState.AddModelError("MatkhauTv", "Vui lòng nhập mật khẩu!");

                return View(tv);
            }
                Thanhvien ad = _context.Thanhviens.AsNoTracking().SingleOrDefault(n => n.SdtTv.Equals(tv.SdtTv) && n.MatkhauTv.Trim().Equals(tv.MatkhauTv.Trim()));
            if (ad != null)
            {

                HttpContext.Session.SetString("tktv", ad.TenTv);
                HttpContext.Session.SetInt32("idtv", ad.MaTv);
                return RedirectToAction("index", "Home");
            }
            else
                ViewBag.Thongbao = "Tài khoản hoặc mật khẩu không đúng";
            return View(tv);


        }
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Login", "ThanhVien");
        }
        // GET: Admin/Thanhvien
        public async Task<IActionResult> Index()
        {
            var nienluancosoContext = _context.Thanhviens.Include(t => t.MaCvNavigation);
            return View(await nienluancosoContext.ToListAsync());
        }

        // GET: Admin/Thanhvien/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (HttpContext.Session.GetInt32("idtv") == null)
            {
                return RedirectToAction("Login", "ThanhVien");
            }
            if (id == null || _context.Thanhviens == null)
            {
                return NotFound();
            }

            var thanhvien = await _context.Thanhviens
                .Include(t => t.MaCvNavigation)
                .FirstOrDefaultAsync(m => m.MaTv == id);
            if (thanhvien == null)
            {
                return NotFound();
            }

            return View(thanhvien);
        }

        // GET: Admin/Thanhvien/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetInt32("idtv") == null)
            {
                return RedirectToAction("Login", "ThanhVien");
            }
            ViewData["MaCv"] = new SelectList(_context.Chucvus, "MaCv", "MaCv");
            return View();
        }

        // POST: Admin/Thanhvien/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaTv,TenTv,GioitinhTv,DiachiTv,SdtTv,EmailTv,MatkhauTv,MaCv")] Thanhvien thanhvien)
        {
            if (ModelState.IsValid)
            {
                _context.Add(thanhvien);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaCv"] = new SelectList(_context.Chucvus, "MaCv", "MaCv", thanhvien.MaCv);
            return View(thanhvien);
        }

        // GET: Admin/Thanhvien/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Thanhviens == null)
            {
                return NotFound();
            }

            var thanhvien = await _context.Thanhviens.FindAsync(id);
            if (thanhvien == null)
            {
                return NotFound();
            }
            ViewData["MaCv"] = new SelectList(_context.Chucvus, "MaCv", "MaCv", thanhvien.MaCv);
            return View(thanhvien);
        }

        // POST: Admin/Thanhvien/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaTv,TenTv,GioitinhTv,DiachiTv,SdtTv,EmailTv,MatkhauTv,MaCv")] Thanhvien thanhvien)
        {
            if (HttpContext.Session.GetInt32("idtv") == null)
            {
                return RedirectToAction("Login", "ThanhVien");
            }
            if (id != thanhvien.MaTv)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(thanhvien);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ThanhvienExists(thanhvien.MaTv))
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
            ViewData["MaCv"] = new SelectList(_context.Chucvus, "MaCv", "MaCv", thanhvien.MaCv);
            return View(thanhvien);
        }

        // GET: Admin/Thanhvien/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (HttpContext.Session.GetInt32("idtv") == null)
            {
                return RedirectToAction("Login", "ThanhVien");
            }
            if (id == null || _context.Thanhviens == null)
            {
                return NotFound();
            }

            var thanhvien = await _context.Thanhviens
                .Include(t => t.MaCvNavigation)
                .FirstOrDefaultAsync(m => m.MaTv == id);
            if (thanhvien == null)
            {
                return NotFound();
            }

            return View(thanhvien);
        }

        // POST: Admin/Thanhvien/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Thanhviens == null)
            {
                return Problem("Entity set 'NienluancosoContext.Thanhviens'  is null.");
            }
            var thanhvien = await _context.Thanhviens.FindAsync(id);
            if (thanhvien != null)
            {
                _context.Thanhviens.Remove(thanhvien);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ThanhvienExists(int id)
        {
          return (_context.Thanhviens?.Any(e => e.MaTv == id)).GetValueOrDefault();
        }
    }
}
