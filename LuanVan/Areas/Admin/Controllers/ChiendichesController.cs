using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LuanVan.Data;
using System.Data;

namespace LuanVan.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ChiendichesController : Controller
    {
        private readonly NienluancosoContext _context;

        public ChiendichesController(NienluancosoContext context)
        {
            _context = context;
        }

        // GET: Admin/Chiendiches
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetInt32("idtv") == null)
            {
                return RedirectToAction("Login", "ThanhVien");
            }
            var nienluancosoContext = _context.Chiendiches.Include(c => c.MaNoiNavigation).Include(c => c.MaTvNavigation).Include(c => c.TtTraotangs).Include(c => c.TtQuyengopHienvats);
            return View(await nienluancosoContext.ToListAsync());
        }

        // GET: Admin/Chiendiches/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (HttpContext.Session.GetInt32("idtv") == null)
            {
                return RedirectToAction("Login", "ThanhVien");
            }
            if (id == null || _context.Chiendiches == null)
            {
                return NotFound();
            }

            var chiendich = await _context.Chiendiches
                .Include(c => c.MaNoiNavigation)
                .Include(c => c.MaTvNavigation)
                .Include(c => c.TtQuyengopHienvats)
                .Include(c => c.TtTraotangs)
                .FirstOrDefaultAsync(m => m.MaCd == id);
            if (chiendich == null)
            {
                return NotFound();
            }

            return View(chiendich);
        }

        // GET: Admin/Chiendiches/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetInt32("idtv") == null)
            {
                return RedirectToAction("Login", "ThanhVien");
            }
            var Noihotros = _context.Noihotros.ToList();
            Noihotros.Insert(0, new Noihotro { Manoi = 0, Diachi = "---KHÔNG---" });
            ViewData["MaNoi"] = new SelectList(Noihotros, "Manoi", "Diachi");

            ViewData["MaTv"] = new SelectList(_context.Thanhviens, "MaTv", "TenTv");
            return View();
        }

        // POST: Admin/Chiendiches/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaCd,TenCd,Ngaybatdau,Ngayketthuc,NoidungCd,AnhCd,MaTv,MaNoi")] Chiendich chiendich, IFormFile file)
        {
            var Noihotros = _context.Noihotros.ToList();
            Noihotros.Insert(0, new Noihotro { Manoi = 0, Diachi = "---KHÔNG---" });
            if (ModelState.IsValid)
            {
                if (chiendich.MaNoi == 0)
                    chiendich.MaNoi = null;

                
                //Upload file
                var fileName = Path.GetFileName(file.FileName);

                //Luu duong dan file anh
                var path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "Content/images/chiendich/"));

                //Kiem tra file da ton tai
                if (System.IO.File.Exists(path))
                {
                    ViewBag.ThongBao = "Hình ảnh đã tồn tại";
                }
                else
                {
                    await UploadFile(file);
                }
                chiendich.AnhCd = file.FileName;
                if (chiendich.Ngayketthuc <= chiendich.Ngaybatdau)
                {
                    ViewData["MaNoi"] = new SelectList(Noihotros, "Manoi", "Diachi", chiendich.MaNoi);

                    ViewData["MaTv"] = new SelectList(_context.Thanhviens, "MaTv", "TenTv", chiendich.MaTv);
                    ModelState.AddModelError("Ngayketthuc", "Ngày kết thúc không được nhỏ hơn ngày bắt đầu!");
                    return View(chiendich);
                }
                if (chiendich.AnhCd == null)
                {
                   
                   
                    ViewData["MaNoi"] = new SelectList(Noihotros, "Manoi", "Diachi", chiendich.MaNoi);

                    ViewData["MaTv"] = new SelectList(_context.Thanhviens, "MaTv", "TenTv", chiendich.MaTv);
                    ModelState.AddModelError("AnhCd", "Ảnh chiến dịch không được trống!");
                    return View(chiendich);
                }

                _context.Add(chiendich);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
           
            ViewData["MaNoi"] = new SelectList(Noihotros, "Manoi", "Diachi", chiendich.MaNoi);

            ViewData["MaTv"] = new SelectList(_context.Thanhviens, "MaTv", "TenTv", chiendich.MaTv);
            return View(chiendich);
        }

        // GET: Admin/Chiendiches/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (HttpContext.Session.GetInt32("idtv") == null)
            {
                return RedirectToAction("Login", "ThanhVien");
            }
            if (id == null || _context.Chiendiches == null)
            {
                return NotFound();
            }

            var chiendich = await _context.Chiendiches.FindAsync(id);
            if (chiendich == null)
            {
                return NotFound();
            }
            var Noihotros = _context.Noihotros.ToList();
            Noihotros.Insert(0, new Noihotro { Manoi = 0, Diachi = "---KHÔNG---" });
            ViewData["MaNoi"] = new SelectList(Noihotros, "Manoi", "Diachi", chiendich.MaNoi);
            ViewData["MaTv"] = new SelectList(_context.Thanhviens, "MaTv", "TenTv", chiendich.MaTv);
            return View(chiendich);
        }

        // POST: Admin/Chiendiches/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaCd,TenCd,Ngaybatdau,Ngayketthuc,NoidungCd,AnhCd,MaTv,MaNoi")] Chiendich chiendich, IFormFile file)
        {
            if (id != chiendich.MaCd)
            {
                return NotFound();
            }
            if (chiendich.MaNoi == 0)
                chiendich.MaNoi = null;
            var cd = _context.Chiendiches.AsNoTracking().SingleOrDefault(s => s.MaCd == chiendich.MaCd);
            var Noihotros = _context.Noihotros.ToList();
            Noihotros.Insert(0, new Noihotro { Manoi = 0, Diachi = "---KHÔNG---" });
            if (file != null)
            {
                //Upload file
                var fileName = Path.GetFileName(file.FileName);

                //Luu duong dan file anh
                var path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "Content/images/chiendich/"));

                //Kiem tra file da ton tai
                if (System.IO.File.Exists(path))
                {
                    ViewBag.ThongBao = "Hình ảnh đã tồn tại";
                }
                else
                {
                    await UploadFile(file);
                }
                chiendich.AnhCd = file.FileName;
            }
            else
            {
                chiendich.AnhCd = cd.AnhCd;
            }
            if (string.IsNullOrEmpty(chiendich.TenCd) == true)
            {
                ModelState.AddModelError("TenCd", "Tên chiến dịch không được trống!");

                ViewData["MaNoi"] = new SelectList(Noihotros, "Manoi", "Diachi", chiendich.MaNoi);
                ViewData["MaTv"] = new SelectList(_context.Thanhviens, "MaTv", "TenTv", chiendich.MaTv);
                return View(chiendich);
            }
            if (string.IsNullOrEmpty(chiendich.NoidungCd) == true)
            {
                ModelState.AddModelError("NoidungCd", "Nội dung chiến dịch không được trống!");

                ViewData["MaNoi"] = new SelectList(Noihotros, "Manoi", "Diachi", chiendich.MaNoi);
                ViewData["MaTv"] = new SelectList(_context.Thanhviens, "MaTv", "TenTv", chiendich.MaTv);
                return View(chiendich);
            }
            if (chiendich.AnhCd == null)
            {
                ModelState.AddModelError("AnhCd", "Ảnh chiến dịch không được trống!");

                ViewData["MaNoi"] = new SelectList(Noihotros, "Manoi", "Diachi", chiendich.MaNoi);
                ViewData["MaTv"] = new SelectList(_context.Thanhviens, "MaTv", "TenTv", chiendich.MaTv);
                return View(chiendich);
            }
            if (chiendich.Ngaybatdau == null)
            {
                ModelState.AddModelError("Ngaybatdau", "Ngày bắt đầu chiến dịch không được trống!");

                ViewData["MaNoi"] = new SelectList(Noihotros, "Manoi", "Diachi", chiendich.MaNoi);
                ViewData["MaTv"] = new SelectList(_context.Thanhviens, "MaTv", "TenTv", chiendich.MaTv);
                return View(chiendich);
            }
            if (chiendich.Ngayketthuc == null)
            {
                ModelState.AddModelError("Ngayketthuc", "Ngày kết thúc chiến dịch không được trống!");

                ViewData["MaNoi"] = new SelectList(Noihotros, "Manoi", "Diachi", chiendich.MaNoi);
                ViewData["MaTv"] = new SelectList(_context.Thanhviens, "MaTv", "TenTv", chiendich.MaTv);
                return View(chiendich);
            }
            if (chiendich.Ngayketthuc <= chiendich.Ngaybatdau)
            {
                ModelState.AddModelError("Ngayketthuc", "Ngày kết thúc không được nhỏ hơn ngày bắt đầu!");


                ViewData["MaNoi"] = new SelectList(Noihotros, "Manoi", "Diachi", chiendich.MaNoi);
                ViewData["MaTv"] = new SelectList(_context.Thanhviens, "MaTv", "TenTv", chiendich.MaTv);
                return View(chiendich);
            }
            try
            {


                _context.Update(chiendich);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));


            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChiendichExists(chiendich.MaCd))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
           
        }

        // GET: Admin/Chiendiches/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (HttpContext.Session.GetInt32("idtv") == null)
            {
                return RedirectToAction("Login", "ThanhVien");
            }
            if (id == null || _context.Chiendiches == null)
            {
                return NotFound();
            }

            var chiendich = await _context.Chiendiches
                .Include(c => c.MaNoiNavigation)
                .Include(c => c.MaTvNavigation)
                .FirstOrDefaultAsync(m => m.MaCd == id);
            if (chiendich == null)
            {
                return NotFound();
            }

            return View(chiendich);
        }

        // POST: Admin/Chiendiches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Chiendiches == null)
            {
                return Problem("Entity set 'NienluancosoContext.Chiendiches'  is null.");
            }

           
            
            var chiendich = await _context.Chiendiches.FindAsync(id);
            if (chiendich != null)
            {
                _context.Chiendiches.Remove(chiendich);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChiendichExists(int id)
        {
            return (_context.Chiendiches?.Any(e => e.MaCd == id)).GetValueOrDefault();
        }

        public async Task<bool> UploadFile(IFormFile file)
        {
            string path = "";
            bool iscopied = false;
            try
            {
                if (file.Length > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/images/chiendich"));
                    using (var filestream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                    {
                        await file.CopyToAsync(filestream);
                    }
                    iscopied = true;
                }
                else iscopied = false;
            }
            catch (Exception) { throw; }
            return iscopied;
        }
    }
}
