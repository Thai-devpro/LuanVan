using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LuanVan.Data;
using System.Text.RegularExpressions;

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
               var ad = _context.Thanhviens.AsNoTracking().SingleOrDefault(n => n.SdtTv.Equals(tv.SdtTv) && n.MatkhauTv.Trim().Equals(tv.MatkhauTv.Trim()));
            if (ad != null)
            {

                HttpContext.Session.SetString("tktv", ad.TenTv);
                HttpContext.Session.SetInt32("idtv", ad.MaTv);
                HttpContext.Session.SetInt32("cvtv", ad.MaCv);
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
            if (HttpContext.Session.GetInt32("idtv") == null)
            {
                return RedirectToAction("Login", "ThanhVien");
            }
            var count = _context.Quyens.Where(c => c.MaCn == 6 && c.MaCv == HttpContext.Session.GetInt32("cvtv")).Count();
            if (count == 0)
            {
                return RedirectToAction("norole", "Home");
            }
            var nienluancosoContext = _context.Thanhviens.Include(t => t.MaCvNavigation).Include(t => t.TtTraotangs).Include(t => t.TtQuyengopHienvats).Include(t => t.Chiendiches);
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
            var count = _context.Quyens.Where(c => c.MaCn == 6 && c.MaCv == HttpContext.Session.GetInt32("cvtv")).Count();
            if (count == 0)
            {
                return RedirectToAction("norole", "Home");
            }
            ViewData["MaCv"] = new SelectList(_context.Chucvus, "MaCv", "TenCv");
            return View();
        }

        // POST: Admin/Thanhvien/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaTv,TenTv,GioitinhTv,DiachiTv,SdtTv,EmailTv,MatkhauTv,MaCv")] Thanhvien thanhvien)
        {
            
                if (string.IsNullOrEmpty(thanhvien.TenTv) == true)
                {
                    ModelState.AddModelError("TenTv", "Vui lòng nhập họ tên!");
                    ViewData["MaCv"] = new SelectList(_context.Chucvus, "MaCv", "TenCv", thanhvien.MaCv);
                    return View(thanhvien);
                }
                if (string.IsNullOrEmpty(thanhvien.DiachiTv) == true)
                {
                    ModelState.AddModelError("DiachiTv", "Vui lòng nhập địa chỉ!");
                    ViewData["MaCv"] = new SelectList(_context.Chucvus, "MaCv", "TenCv", thanhvien.MaCv);
                    return View(thanhvien);
                }
                string regex = @"^([\+]?33[-]?|[0])?[1-9][0-9]{8}$";
                if (thanhvien.SdtTv == null || !Regex.IsMatch(thanhvien.SdtTv.ToString(), regex))
                {
                    ModelState.AddModelError("SdtTv", "Vui lòng nhập số điện thoại chính xác!");
                    ViewData["MaCv"] = new SelectList(_context.Chucvus, "MaCv", "TenCv", thanhvien.MaCv);
                    return View(thanhvien);
                }
                Thanhvien ad = _context.Thanhviens.AsNoTracking().FirstOrDefault(n => n.SdtTv.Equals(thanhvien.SdtTv));
                if (ad != null)
                {
                    ModelState.AddModelError("SdtTv", "Số điện thoại đã được sử dụng hãy chọn số khác!");
                    ViewData["MaCv"] = new SelectList(_context.Chucvus, "MaCv", "TenCv", thanhvien.MaCv);
                    return View(thanhvien);
                }
                string regex2 = @"^[a-zA-Z0-9.!#$%&'*+\/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$";
                if (thanhvien.EmailTv == null || !Regex.IsMatch(thanhvien.EmailTv.ToString().Trim(), regex2))
                {
                    ModelState.AddModelError("EmailTv", "Vui lòng nhập email chính xác!");
                    ViewData["MaCv"] = new SelectList(_context.Chucvus, "MaCv", "TenCv", thanhvien.MaCv);
                    return View(thanhvien);
                }

                if (string.IsNullOrEmpty(thanhvien.MatkhauTv.Trim()) == true ||thanhvien.MatkhauTv.Trim().Length < 4 || thanhvien.MatkhauTv.Trim().Length > 9)
                {
                    ModelState.AddModelError("MatkhauTv", "Vui lòng nhập mật khẩu từ 5 đến 10 ký tự!");
                    ViewData["MaCv"] = new SelectList(_context.Chucvus, "MaCv", "TenCv", thanhvien.MaCv);
                    return View(thanhvien);
                }
                _context.Add(thanhvien);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
           
        }

        // GET: Admin/Thanhvien/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (HttpContext.Session.GetInt32("idtv") == null)
            {
                return RedirectToAction("Login", "ThanhVien");
            }
            var count = _context.Quyens.Where(c => c.MaCn == 6 && c.MaCv == HttpContext.Session.GetInt32("cvtv")).Count();
            if (count == 0)
            {
                return RedirectToAction("norole", "Home");
            }
            if (id == null || _context.Thanhviens == null)
            {
                return NotFound();
            }

            var thanhvien = await _context.Thanhviens.FindAsync(id);
            if (thanhvien == null)
            {
                return NotFound();
            }
            ViewBag.GioitinhTv = thanhvien.GioitinhTv;
            ViewData["MaCv"] = new SelectList(_context.Chucvus, "MaCv", "TenCv", thanhvien.MaCv);
            return View(thanhvien);
        }

        // POST: Admin/Thanhvien/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaTv,TenTv,GioitinhTv,DiachiTv,SdtTv,EmailTv,MatkhauTv,MaCv")] Thanhvien thanhvien)
        {
            
            if (id != thanhvien.MaTv)
            {
                return NotFound();
            }
            var tv = _context.Thanhviens.AsNoTracking().FirstOrDefault(t => t.MaTv == thanhvien.MaTv);
            thanhvien.MatkhauTv = tv.MatkhauTv;
            
                if (string.IsNullOrEmpty(thanhvien.TenTv) == true)
                {
                    ModelState.AddModelError("TenTv", "Vui lòng nhập họ tên!");
                ViewBag.GioitinhTv = thanhvien.GioitinhTv;
                ViewData["MaCv"] = new SelectList(_context.Chucvus, "MaCv", "TenCv", thanhvien.MaCv);
                    return View(thanhvien);
                }
                if (string.IsNullOrEmpty(thanhvien.DiachiTv) == true)
                {
                    ModelState.AddModelError("DiachiTv", "Vui lòng nhập địa chỉ!");
                ViewBag.GioitinhTv = thanhvien.GioitinhTv;
                ViewData["MaCv"] = new SelectList(_context.Chucvus, "MaCv", "TenCv", thanhvien.MaCv);
                    return View(thanhvien);
                }
                string regex = @"^([\+]?33[-]?|[0])?[1-9][0-9]{8}$";
                if (thanhvien.SdtTv == null || !Regex.IsMatch(thanhvien.SdtTv.ToString(), regex))
                {
                ViewBag.GioitinhTv = thanhvien.GioitinhTv;
                ModelState.AddModelError("SdtTv", "Vui lòng nhập số điện thoại chính xác!");
                    ViewData["MaCv"] = new SelectList(_context.Chucvus, "MaCv", "TenCv", thanhvien.MaCv);
                    return View(thanhvien);
                }
                Thanhvien ad = _context.Thanhviens.AsNoTracking().FirstOrDefault(n => n.SdtTv.Equals(thanhvien.SdtTv));
                if (ad != null && ad.MaTv != thanhvien.MaTv)
                {
                ViewBag.GioitinhTv = thanhvien.GioitinhTv;
                ModelState.AddModelError("SdtTv", "Số điện thoại đã được sử dụng hãy chọn số khác!");
                    ViewData["MaCv"] = new SelectList(_context.Chucvus, "MaCv", "TenCv", thanhvien.MaCv);
                    return View(thanhvien);
                }
                string regex2 = @"^[a-zA-Z0-9.!#$%&'*+\/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$";
                if (thanhvien.EmailTv == null || !Regex.IsMatch(thanhvien.EmailTv.ToString().Trim(), regex2))
                {
                ViewBag.GioitinhTv = thanhvien.GioitinhTv;
                ModelState.AddModelError("EmailTv", "Vui lòng nhập email chính xác!");
                    ViewData["MaCv"] = new SelectList(_context.Chucvus, "MaCv", "TenCv", thanhvien.MaCv);
                    return View(thanhvien);
                }




                
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
        public async Task<IActionResult> Edit2(int? id)
        {
            if (HttpContext.Session.GetInt32("idtv") == null)
            {
                return RedirectToAction("Login", "ThanhVien");
            }
            if (id == null || _context.Thanhviens == null)
            {
                return NotFound();
            }

            var thanhvien = await _context.Thanhviens.FindAsync(id);
            if (thanhvien == null)
            {
                return NotFound();
            }
            ViewBag.GioitinhTv = thanhvien.GioitinhTv;
            ViewData["MaCv"] = new SelectList(_context.Chucvus, "MaCv", "TenCv", thanhvien.MaCv);
            return View(thanhvien);
        }

        // POST: Admin/Thanhvien/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit2(int id, [Bind("MaTv,TenTv,GioitinhTv,DiachiTv,SdtTv,EmailTv,MatkhauTv,MaCv")] Thanhvien thanhvien)
        {

            if (id != thanhvien.MaTv)
            {
                return NotFound();
            }
            var tv = _context.Thanhviens.AsNoTracking().FirstOrDefault(t => t.MaTv == thanhvien.MaTv);
     

            if (string.IsNullOrEmpty(thanhvien.TenTv) == true)
            {
                ViewBag.GioitinhTv = thanhvien.GioitinhTv;
                ModelState.AddModelError("TenTv", "Vui lòng nhập họ tên!");
                ViewData["MaCv"] = new SelectList(_context.Chucvus, "MaCv", "TenCv", thanhvien.MaCv);
                return View(thanhvien);
            }
            if (string.IsNullOrEmpty(thanhvien.DiachiTv) == true)
            {
                ViewBag.GioitinhTv = thanhvien.GioitinhTv;
                ModelState.AddModelError("DiachiTv", "Vui lòng nhập địa chỉ!");
                ViewData["MaCv"] = new SelectList(_context.Chucvus, "MaCv", "TenCv", thanhvien.MaCv);
                return View(thanhvien);
            }
            string regex = @"^([\+]?33[-]?|[0])?[1-9][0-9]{8}$";
            if (thanhvien.SdtTv == null || !Regex.IsMatch(thanhvien.SdtTv.ToString(), regex))
            {
                ViewBag.GioitinhTv = thanhvien.GioitinhTv;
                ModelState.AddModelError("SdtTv", "Vui lòng nhập số điện thoại chính xác!");
                ViewData["MaCv"] = new SelectList(_context.Chucvus, "MaCv", "TenCv", thanhvien.MaCv);
                return View(thanhvien);
            }
            Thanhvien ad = _context.Thanhviens.AsNoTracking().FirstOrDefault(n => n.SdtTv.Equals(thanhvien.SdtTv));
            if (ad != null && ad.MaTv != thanhvien.MaTv)
            {
                ViewBag.GioitinhTv = thanhvien.GioitinhTv;
                ModelState.AddModelError("SdtTv", "Số điện thoại đã được sử dụng hãy chọn số khác!");
                ViewData["MaCv"] = new SelectList(_context.Chucvus, "MaCv", "TenCv", thanhvien.MaCv);
                return View(thanhvien);
            }
            string regex2 = @"^[a-zA-Z0-9.!#$%&'*+\/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$";
            if (thanhvien.EmailTv == null || !Regex.IsMatch(thanhvien.EmailTv.ToString().Trim(), regex2))
            {
                ViewBag.GioitinhTv = thanhvien.GioitinhTv;
                ModelState.AddModelError("EmailTv", "Vui lòng nhập email chính xác!");
                ViewData["MaCv"] = new SelectList(_context.Chucvus, "MaCv", "TenCv", thanhvien.MaCv);
                return View(thanhvien);
            }

            if (string.IsNullOrEmpty(thanhvien.MatkhauTv.Trim()) == true || thanhvien.MatkhauTv.Trim().Length < 4 || thanhvien.MatkhauTv.Trim().Length > 9)
            {
                ModelState.AddModelError("MatkhauTv", "Vui lòng nhập mật khẩu từ 5 đến 10 ký tự!");
                ViewData["MaCv"] = new SelectList(_context.Chucvus, "MaCv", "TenCv", thanhvien.MaCv);
                return View(thanhvien);
            }



            try
            {
                thanhvien.MaCv = tv.MaCv;
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
            ViewBag.thongbao = "Đã cập nhật thông tin";
            return RedirectToAction("Details", new { id = thanhvien.MaTv });

        }

        // GET: Admin/Thanhvien/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (HttpContext.Session.GetInt32("idtv") == null)
            {
                return RedirectToAction("Login", "ThanhVien");
            }
            var count = _context.Quyens.Where(c => c.MaCn == 6 && c.MaCv == HttpContext.Session.GetInt32("cvtv")).Count();
            if (count == 0)
            {
                return RedirectToAction("norole", "Home");
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
