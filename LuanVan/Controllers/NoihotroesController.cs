using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LuanVan.Data;
using System.Data;
using X.PagedList;

namespace LuanVan.Controllers
{
    public class NoihotroesController : Controller
    {
        private readonly NienluancosoContext _context;

        public NoihotroesController(NienluancosoContext context)
        {
            _context = context;
        }

        // GET: Noihotroes
        public async Task<IActionResult> Index(string? SearchString, int? tt , int page = 1)
        {
            page = page < 1 ? 1 : page;
            int pagesize = 3;
            if (HttpContext.Session.GetInt32("idmtq") != null)
            {
                List<SelectListItem> items = new List<SelectListItem>
{
    new SelectListItem { Value = "1", Text = "Danh sách tất cả đề xuất" },
    new SelectListItem { Value = "2", Text = "Danh sách đề xuất của bản thân" },
   
};

                ViewBag.tt = new SelectList(items, "Value", "Text", tt);
            }
           
            if (SearchString != null)
            {
                var nienluancosoContext2 = _context.Noihotros.Include(n => n.MaMtqNavigation).Include(n => n.TtTraotangs).Include(n => n.Chiendiches).Where(c => c.Diachi.Contains(SearchString)).ToPagedList();
                if (nienluancosoContext2.Count() == 0)
                {
                    ViewBag.tb = "Không tìm thấy đề xuất hỗ trợ có địa chỉ : " + SearchString.ToString();
                }
                else
                {
                    ViewBag.tb = "Đã tìm thấy đề xuất hỗ trợ có địa chỉ : " + SearchString.ToString();
                }
                return View( nienluancosoContext2);
            }
            if (tt == 1)
            {

                var nienluancosoContext2 = _context.Noihotros.Include(n => n.MaMtqNavigation).Include(n => n.TtTraotangs).Include(n => n.Chiendiches).ToPagedList(page, pagesize);

                return View(nienluancosoContext2);
            }
            if (tt == 2)
            {

                var nienluancosoContext2 = _context.Noihotros.Include(n => n.MaMtqNavigation).Include(n => n.TtTraotangs).Include(n => n.Chiendiches).Where(n => n.MaMtq == HttpContext.Session.GetInt32("idmtq")).ToPagedList();



                return View( nienluancosoContext2);
            }
           
            var nienluancosoContext = _context.Noihotros.Include(n => n.MaMtqNavigation).Include(n => n.TtTraotangs).Include(n => n.Chiendiches).ToPagedList(page,pagesize);
            return View( nienluancosoContext);
        }

        // GET: Noihotroes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Noihotros == null)
            {
                return NotFound();
            }

            var noihotro = await _context.Noihotros
                 .Include(n => n.MaMtqNavigation)
                 .Include(n => n.TtTraotangs)
                 .Include(n => n.Chiendiches)
                 .FirstOrDefaultAsync(n => n.Manoi == id);
            if (noihotro == null)
            {
                return NotFound();
            }

            return View(noihotro);
        }

        // GET: Noihotroes/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetInt32("idmtq") == null)
                return RedirectToAction("Register", "User");
            ViewData["MaMtq"] = new SelectList(_context.Manhthuongquans.Where(s => s.MaMtq == HttpContext.Session.GetInt32("idmtq")), "MaMtq", "HotenMtq");
            
            return View();
        }

        // POST: Noihotroes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Manoi,MaMtq,Diachi,Tinhtrang,Canhotro,TrangthaiNht,MaTv,AnhNth")] Noihotro noihotro, IFormFile file)
        {
            if (noihotro.Diachi == null)
            {
                ViewData["MaMtq"] = new SelectList(_context.Manhthuongquans.Where(s => s.MaMtq == HttpContext.Session.GetInt32("idmtq")), "MaMtq", "HotenMtq");
                ModelState.AddModelError("Diachi", "Vui lòng nhập địa chỉ");
                return View(noihotro);
            }
            if (noihotro.Tinhtrang == null)
            {
                ViewData["MaMtq"] = new SelectList(_context.Manhthuongquans.Where(s => s.MaMtq == HttpContext.Session.GetInt32("idmtq")), "MaMtq", "HotenMtq");
                ModelState.AddModelError("Tinhtrang", "Vui lòng nhập tình trạng");
                return View(noihotro);
            }
            if (noihotro.Canhotro == null)
            {
                ViewData["MaMtq"] = new SelectList(_context.Manhthuongquans.Where(s => s.MaMtq == HttpContext.Session.GetInt32("idmtq")), "MaMtq", "HotenMtq");
                ModelState.AddModelError("Canhotro", "Vui lòng nhập những thứ cần hỗ trợ");
                return View(noihotro);
            }
            if (noihotro.AnhNth == null && file == null)
            {
                ViewData["MaMtq"] = new SelectList(_context.Manhthuongquans.Where(s => s.MaMtq == HttpContext.Session.GetInt32("idmtq")), "MaMtq", "HotenMtq");
                ModelState.AddModelError("AnhNth", "Vui lòng chọn ảnh làm minh chứng");
                return View(noihotro);
            }
            var fileName = Path.GetFileName(file.FileName);

            //Luu duong dan file anh
            var path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "Content/images/noihotro/"));

            //Kiem tra file da ton tai
            if (System.IO.File.Exists(path))
            {
                ViewBag.ThongBao = "Hình ảnh đã tồn tại";
            }
            else
            {
                await UploadFile(file);
            }
            
            noihotro.AnhNth = file.FileName;
            if (noihotro.AnhNth == null)
            {
                ViewData["MaMtq"] = new SelectList(_context.Manhthuongquans.Where(s => s.MaMtq == HttpContext.Session.GetInt32("idmtq")), "MaMtq", "HotenMtq");
                ModelState.AddModelError("AnhNth", "Vui lòng chọn ảnh làm minh chứng");
                return View(noihotro);
            }
           
                noihotro.TrangthaiNht = "Chưa duyệt";
                _context.Add(noihotro);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
           
        }

        // GET: Noihotroes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Noihotros == null)
            {
                return NotFound();
            }

            var noihotro = await _context.Noihotros.FindAsync(id);
            if (noihotro == null)
            {
                return NotFound();
            }
            ViewData["MaMtq"] = new SelectList(_context.Manhthuongquans.Where(s => s.MaMtq == HttpContext.Session.GetInt32("idmtq")), "MaMtq", "HotenMtq");

            return View(noihotro);
        }

        // POST: Noihotroes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Manoi,MaMtq,Diachi,Tinhtrang,Canhotro,TrangthaiNht,MaTv,AnhNth")] Noihotro noihotro, IFormFile file)
        {
            if (id != noihotro.Manoi)
            {
                return NotFound();
            }
            var nht = _context.Noihotros.AsNoTracking().SingleOrDefault(s => s.Manoi == noihotro.Manoi);
            
            if (file != null)
            {
                //Upload file
                var fileName = Path.GetFileName(file.FileName);

                //Luu duong dan file anh
                var path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "Content/images/noihotro/"));

                //Kiem tra file da ton tai
                if (System.IO.File.Exists(path))
                {
                    ViewBag.ThongBao = "Hình ảnh đã tồn tại";
                }
                else
                {
                    await UploadFile(file);
                }
                noihotro.AnhNth = file.FileName;
            }
            else
            {
                noihotro.AnhNth = nht.AnhNth;
            }
            if (noihotro.Diachi == null)
            {
                ViewData["MaMtq"] = new SelectList(_context.Manhthuongquans.Where(s => s.MaMtq == HttpContext.Session.GetInt32("idmtq")), "MaMtq", "HotenMtq");
                ModelState.AddModelError("Diachi", "Vui lòng nhập địa chỉ");
                return View(noihotro);
            }
            if (noihotro.Tinhtrang == null)
            {
                ViewData["MaMtq"] = new SelectList(_context.Manhthuongquans.Where(s => s.MaMtq == HttpContext.Session.GetInt32("idmtq")), "MaMtq", "HotenMtq");
                ModelState.AddModelError("Tinhtrang", "Vui lòng nhập tình trạng");
                return View(noihotro);
            }
            if (noihotro.Canhotro == null)
            {
                ViewData["MaMtq"] = new SelectList(_context.Manhthuongquans.Where(s => s.MaMtq == HttpContext.Session.GetInt32("idmtq")), "MaMtq", "HotenMtq");
                ModelState.AddModelError("Canhotro", "Vui lòng nhập những thứ cần hỗ trợ");
                return View(noihotro);
            }
            if (noihotro.AnhNth == null && file == null)
            {
                ViewData["MaMtq"] = new SelectList(_context.Manhthuongquans.Where(s => s.MaMtq == HttpContext.Session.GetInt32("idmtq")), "MaMtq", "HotenMtq");
                ModelState.AddModelError("AnhNth", "Vui lòng chọn ảnh làm minh chứng");
                return View(noihotro);
            }
            try
            {
              
                noihotro.TrangthaiNht = "Chưa duyệt";
                _context.Update(noihotro);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NoihotroExists(noihotro.Manoi))
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
        

        // GET: Noihotroes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Noihotros == null)
            {
                return NotFound();
            }

            var noihotro = await _context.Noihotros.Include(n => n.MaMtqNavigation)
                .FirstOrDefaultAsync(m => m.Manoi == id);
            if (noihotro == null)
            {
                return NotFound();
            }

            return View(noihotro);
        }

        // POST: Noihotroes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Noihotros == null)
            {
                return Problem("Entity set 'NienluancosoContext.Noihotros'  is null.");
            }
            var noihotro = await _context.Noihotros.FindAsync(id);
            if (noihotro != null)
            {
                _context.Noihotros.Remove(noihotro);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NoihotroExists(int id)
        {
          return (_context.Noihotros?.Any(e => e.Manoi == id)).GetValueOrDefault();
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
                    path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/images/noihotro/"));
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
