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
    public class ManhthuongquansController : Controller
    {
        private readonly NienluancosoContext _context;

        public ManhthuongquansController(NienluancosoContext context)
        {
            _context = context;
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

            return File(bytes, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "DanhSachNguoiDung.docx");
        }
        // GET: Admin/Manhthuongquans
        public async Task<IActionResult> Index(DateTime? tu, DateTime? den)
        {
            if (HttpContext.Session.GetInt32("idtv") == null)
            {
                return RedirectToAction("Login", "ThanhVien");
            }
            var count = _context.Quyens.Where(c => c.MaCn == 4 && c.MaCv == HttpContext.Session.GetInt32("cvtv")).Count();
            if (count == 0)
            {
                return RedirectToAction("norole", "Home");
            }
            if (tu != null && den != null)
            {
                var nienluancosoContext2 = _context.Manhthuongquans
    .Select(m => new
    {
        Manhthuongquan = m,
        TotalQuyengopHienvat = m.TtQuyengopHienvats.Count(q => q.NgayQg >= tu && q.NgayQg <= den)
    })
    .OrderByDescending(m => m.TotalQuyengopHienvat)
    .Select(m => m.Manhthuongquan)
    .ToList();

                ViewBag.TuNgay = tu.Value.ToString("dd-MM-yyyy");
                ViewBag.DenNgay = den.Value.ToString("dd-MM-yyyy");
                return View(nienluancosoContext2);
            }

            return _context.Manhthuongquans != null ?
                          View(await _context.Manhthuongquans.ToListAsync()) :
                          Problem("Entity set 'NienluancosoContext.Manhthuongquans'  is null.");
        }

        // GET: Admin/Manhthuongquans/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (HttpContext.Session.GetInt32("idtv") == null)
            {
                return RedirectToAction("Login", "ThanhVien");
            }
            var count = _context.Quyens.Where(c => c.MaCn == 4 && c.MaCv == HttpContext.Session.GetInt32("cvtv")).Count();
            if (count == 0)
            {
                return RedirectToAction("norole", "Home");
            }
            if (id == null || _context.Manhthuongquans == null)
            {
                return NotFound();
            }

            var manhthuongquan = await _context.Manhthuongquans
                .FirstOrDefaultAsync(m => m.MaMtq == id);
            if (manhthuongquan == null)
            {
                return NotFound();
            }

            return View(manhthuongquan);
        }

        // GET: Admin/Manhthuongquans/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetInt32("idtv") == null)
            {
                return RedirectToAction("Login", "ThanhVien");
            }
            var count = _context.Quyens.Where(c => c.MaCn == 4 && c.MaCv == HttpContext.Session.GetInt32("cvtv")).Count();
            if (count == 0)
            {
                return RedirectToAction("norole", "Home");
            }
            return View();
        }

        // POST: Admin/Manhthuongquans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaMtq,HotenMtq,GioitinhMtq,DonviTochucMtq,SdtMtq,DiachiMtq")] Manhthuongquan manhthuongquan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(manhthuongquan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(manhthuongquan);
        }

        // GET: Admin/Manhthuongquans/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (HttpContext.Session.GetInt32("idtv") == null)
            {
                return RedirectToAction("Login", "ThanhVien");
            }
            var count = _context.Quyens.Where(c => c.MaCn == 4 && c.MaCv == HttpContext.Session.GetInt32("cvtv")).Count();
            if (count == 0)
            {
                return RedirectToAction("norole", "Home");
            }
            if (id == null || _context.Manhthuongquans == null)
            {
                return NotFound();
            }

            var manhthuongquan = await _context.Manhthuongquans.FindAsync(id);
            if (manhthuongquan == null)
            {
                return NotFound();
            }
            return View(manhthuongquan);
        }

        // POST: Admin/Manhthuongquans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaMtq,HotenMtq,GioitinhMtq,DonviTochucMtq,SdtMtq,DiachiMtq")] Manhthuongquan manhthuongquan)
        {
            if (id != manhthuongquan.MaMtq)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(manhthuongquan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ManhthuongquanExists(manhthuongquan.MaMtq))
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
            return View(manhthuongquan);
        }

        // GET: Admin/Manhthuongquans/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (HttpContext.Session.GetInt32("idtv") == null)
            {
                return RedirectToAction("Login", "ThanhVien");
            }
            var count = _context.Quyens.Where(c => c.MaCn == 4 && c.MaCv == HttpContext.Session.GetInt32("cvtv")).Count();
            if (count == 0)
            {
                return RedirectToAction("norole", "Home");
            }
            if (id == null || _context.Manhthuongquans == null)
            {
                return NotFound();
            }

            var manhthuongquan = await _context.Manhthuongquans
                .FirstOrDefaultAsync(m => m.MaMtq == id);
            if (manhthuongquan == null)
            {
                return NotFound();
            }

            return View(manhthuongquan);
        }

        // POST: Admin/Manhthuongquans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (HttpContext.Session.GetInt32("idtv") == null)
            {
                return RedirectToAction("Login", "ThanhVien");
            }
            if (_context.Manhthuongquans == null)
            {
                return Problem("Entity set 'NienluancosoContext.Manhthuongquans'  is null.");
            }
            var manhthuongquan = await _context.Manhthuongquans.FindAsync(id);
            if (manhthuongquan != null)
            {
                _context.Manhthuongquans.Remove(manhthuongquan);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ManhthuongquanExists(int id)
        {
            return (_context.Manhthuongquans?.Any(e => e.MaMtq == id)).GetValueOrDefault();
        }
    }
}
