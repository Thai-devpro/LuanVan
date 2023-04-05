using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LuanVan.Data;
using SautinSoft.Document;

namespace LuanVan.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class NoihotroesController : Controller
    {
        private readonly NienluancosoContext _context;

        public NoihotroesController(NienluancosoContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> Export(int? id)
        {
            if (HttpContext.Session.GetInt32("idtv") == null)
            {
                return RedirectToAction("Login", "ThanhVien");
            }
            var count = _context.Quyens.Where(c => c.MaCn == 1 && c.MaCv == HttpContext.Session.GetInt32("cvtv")).Count();
            if (count == 0)
            {
                return RedirectToAction("norole", "Home");
            }
            if (id == null || _context.Chiendiches == null)
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
        [HttpPost]
        public IActionResult Export(string GridHtml)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "HTML");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string input = Path.Combine(path, "html1.html");
            string output = Path.Combine(path, "BaoCaoChienDich.docx");
            System.IO.File.WriteAllText(input, GridHtml);
            DocumentCore documentCore = DocumentCore.Load(input);
            documentCore.Save(output);
            byte[] bytes = System.IO.File.ReadAllBytes(output);

            Directory.Delete(path, true);

            return File(bytes, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "BaoCaoNoiHoTro.docx");
        }
        // GET: Admin/Noihotroes
        public async Task<IActionResult> Index(string? SearchString)
        {
            if (HttpContext.Session.GetInt32("idtv") == null)
            {
                return RedirectToAction("Login", "ThanhVien");
            }
            var count = _context.Quyens.Where(c => c.MaCn == 5 && c.MaCv == HttpContext.Session.GetInt32("cvtv")).Count();
            if (count == 0)
            {
                return RedirectToAction("norole", "Home");
            }
            if (SearchString != null)
            {
                var nienluancosoContext2 = _context.Noihotros.Include(n => n.MaMtqNavigation).Include(n => n.TtTraotangs).Include(n => n.Chiendiches).Where(c => c.Diachi.Contains(SearchString));
                if (nienluancosoContext2.Count() == 0)
                {
                    ViewBag.tb = "Không tìm thấy đề xuất hỗ trợ có địa chỉ : " + SearchString.ToString();
                }
                else
                {
                    ViewBag.tb = "Đã tìm thấy đề xuất hỗ trợ có địa chỉ : " + SearchString.ToString();
                }
                return View(await nienluancosoContext2.ToListAsync());
            }
            var nienluancosoContext = _context.Noihotros.Include(n => n.MaMtqNavigation).Include(n => n.TtTraotangs).Include(n => n.Chiendiches);
            return View(await nienluancosoContext.ToListAsync());
        }

        // GET: Admin/Noihotroes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (HttpContext.Session.GetInt32("idtv") == null)
            {
                return RedirectToAction("Login", "ThanhVien");
            }
            var count = _context.Quyens.Where(c => c.MaCn == 5 && c.MaCv == HttpContext.Session.GetInt32("cvtv")).Count();
            if (count == 0)
            {
                return RedirectToAction("norole", "Home");
            }
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

        // GET: Admin/Noihotroes/Create
        //public IActionResult Create()
        //{
        //    if (HttpContext.Session.GetInt32("idtv") == null)
        //    {
        //        return RedirectToAction("Login", "ThanhVien");
        //    }
        //    ViewData["MaMtq"] = new SelectList(_context.Manhthuongquans, "MaMtq", "MaMtq");
            
        //    return View();
        //}

        //// POST: Admin/Noihotroes/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Manoi,MaMtq,Diachi,Tinhtrang,Canhotro,TrangthaiNht,MaTv,AnhNth")] Noihotro noihotro)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(noihotro);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["MaMtq"] = new SelectList(_context.Manhthuongquans, "MaMtq", "MaMtq", noihotro.MaMtq);
           
        //    return View(noihotro);
        //}

        // GET: Admin/Noihotroes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (HttpContext.Session.GetInt32("idtv") == null)
            {
                return RedirectToAction("Login", "ThanhVien");
            }
            var count = _context.Quyens.Where(c => c.MaCn == 5 && c.MaCv == HttpContext.Session.GetInt32("cvtv")).Count();
            if (count == 0)
            {
                return RedirectToAction("norole", "Home");
            }
            if (id == null || _context.Noihotros == null)
            {
                return NotFound();
            }

            var noihotro = await _context.Noihotros.Include(n => n.MaMtqNavigation).FirstOrDefaultAsync(n => n.Manoi == id);
            if (noihotro == null)
            {
                return NotFound();
            }
            ViewData["MaMtq"] = new SelectList(_context.Manhthuongquans.Where(s => s.MaMtq == noihotro.MaMtq), "MaMtq", "HotenMtq");

            return View(noihotro);
        }

        // POST: Admin/Noihotroes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Manoi,MaMtq,Diachi,Tinhtrang,Canhotro,TrangthaiNht,MaTv,AnhNth")] Noihotro noihotro)
        {
            if (id != noihotro.Manoi)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var nht = _context.Noihotros.AsNoTracking().FirstOrDefault(n => n.Manoi == id);
                    noihotro.AnhNth = nht.AnhNth;
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
            ViewData["MaMtq"] = new SelectList(_context.Manhthuongquans, "MaMtq", "MaMtq", noihotro.MaMtq);
      
            return View(noihotro);
        }

        // GET: Admin/Noihotroes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (HttpContext.Session.GetInt32("idtv") == null)
            {
                return RedirectToAction("Login", "ThanhVien");
            }
            var count = _context.Quyens.Where(c => c.MaCn == 5 && c.MaCv == HttpContext.Session.GetInt32("cvtv")).Count();
            if (count == 0)
            {
                return RedirectToAction("norole", "Home");
            }
            if (id == null || _context.Noihotros == null)
            {
                return NotFound();
            }

            var noihotro = await _context.Noihotros
                .Include(n => n.MaMtqNavigation)
                
                .FirstOrDefaultAsync(m => m.Manoi == id);
            if (noihotro == null)
            {
                return NotFound();
            }

            return View(noihotro);
        }

        // POST: Admin/Noihotroes/Delete/5
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
    }
}
