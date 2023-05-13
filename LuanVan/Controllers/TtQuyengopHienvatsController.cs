using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LuanVan.Data;
using Microsoft.AspNetCore.Authorization;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;

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
        public async Task<IActionResult> Index(string? SearchString, DateTime? tu, DateTime? den, int? tt)
        {
            if (HttpContext.Session.GetInt32("idmtq") != null)
            {
                List<SelectListItem> items = new List<SelectListItem>
{
    new SelectListItem { Value = "1", Text = "Danh sách tất cả quyên góp" },
    new SelectListItem { Value = "2", Text = "Danh sách quyên góp của bản thân" },

};

                ViewBag.tt = new SelectList(items, "Value", "Text", tt);
            }
            if (tu != null && den != null && SearchString != null)
            {
                var nienluancosoContext2 = _context.TtQuyengopHienvats.Include(t => t.MaCdNavigation).Include(t => t.MaHvNavigation).Include(t => t.MaMtqNavigation).Include(t => t.MaTvNavigation).Where(q => q.NgayQg >= tu && q.NgayQg <= den && q.MaMtqNavigation.HotenMtq.Contains(SearchString));

                if (nienluancosoContext2.Count() == 0)
                {
                    ViewBag.tb = "Không tìm thấy quyên góp của: " + SearchString.ToString() + "Từ ngày: " + tu.Value.ToString("dd-MM-yyyy") + "Đến: " + den.Value.ToString("dd-MM-yyyy");
                }
                else
                {
                    ViewBag.tb = "Đã tìm thấy quyên góp của: " + SearchString.ToString() + " Từ ngày:" + tu.Value.ToString("dd-MM-yyyy") + " đến:" + den.Value.ToString("dd-MM-yyyy");
                }
                return View(await nienluancosoContext2.ToListAsync());
            }
            if (SearchString != null)
            {
                var nienluancosoContext2 = _context.TtQuyengopHienvats.Include(t => t.MaCdNavigation).Include(t => t.MaHvNavigation).Include(t => t.MaMtqNavigation).Include(t => t.MaTvNavigation).Where(q => q.MaMtqNavigation.HotenMtq.Contains(SearchString));
                if (nienluancosoContext2.Count() == 0)
                {
                    ViewBag.tb = "Không tìm thấy quyên góp của: " + SearchString.ToString();
                }
                else
                {
                    ViewBag.tb = "Đã tìm thấy quyên góp của: " + SearchString.ToString();
                }
                return View(await nienluancosoContext2.ToListAsync());
            }
            if (tu != null && den != null)
            {
                var nienluancosoContext2 = _context.TtQuyengopHienvats.Include(t => t.MaCdNavigation).Include(t => t.MaHvNavigation).Include(t => t.MaMtqNavigation).Include(t => t.MaTvNavigation).Where(q => q.NgayQg >= tu && q.NgayQg <= den);

                ViewBag.TuNgay = tu.Value.ToString("dd-MM-yyyy");
                ViewBag.DenNgay = den.Value.ToString("dd-MM-yyyy");
                return View(await nienluancosoContext2.ToListAsync());
            }
            if (tt == 1)
            {

                var nienluancosoContext2 = _context.TtQuyengopHienvats.Include(t => t.MaCdNavigation).Include(t => t.MaHvNavigation).Include(t => t.MaMtqNavigation).Include(t => t.MaTvNavigation);

                return View(await nienluancosoContext2.ToListAsync());
            }
            if (tt == 2)
            {

                var nienluancosoContext2 = _context.TtQuyengopHienvats.Include(t => t.MaCdNavigation).Include(t => t.MaHvNavigation).Include(t => t.MaMtqNavigation).Include(t => t.MaTvNavigation).Where(t => t.MaMtq == HttpContext.Session.GetInt32("idmtq"));



                return View(await nienluancosoContext2.ToListAsync());
            }
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
                .Include(t => t.MaTvNavigation)
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
            if (HttpContext.Session.GetInt32("idmtq") == null)
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

            if (ttQuyengopHienvat.SoluongQg <= 0)
            {
                ModelState.AddModelError("SoluongQg", "Vui lòng điền số lượng ?");
                ViewData["MaHv"] = new SelectList(_context.HienVats, "MaHv", "TenHv");


                ViewData["MaCd"] = new SelectList(_context.Chiendiches.Where(s => s.MaCd == ttQuyengopHienvat.MaCd), "MaCd", "TenCd");
                ViewData["MaMtq"] = new SelectList(_context.Manhthuongquans.Where(s => s.MaMtq == HttpContext.Session.GetInt32("idmtq")), "MaMtq", "HotenMtq");
                return View(ttQuyengopHienvat);


            }
            if (ttQuyengopHienvat.Ghichu == null)
            {
                ModelState.AddModelError("Ghichu", "Hãy viết gì đó cho chúng tôi");
                ViewData["MaHv"] = new SelectList(_context.HienVats, "MaHv", "TenHv");


                ViewData["MaCd"] = new SelectList(_context.Chiendiches.Where(s => s.MaCd == ttQuyengopHienvat.MaCd), "MaCd", "TenCd");
                ViewData["MaMtq"] = new SelectList(_context.Manhthuongquans.Where(s => s.MaMtq == HttpContext.Session.GetInt32("idmtq")), "MaMtq", "HotenMtq");
                return View(ttQuyengopHienvat);


            }
            ttQuyengopHienvat.NgayQg = DateTime.Now;
            ttQuyengopHienvat.TrangthaiHv = "Chưa nhận";
            Manhthuongquan mtq = _context.Manhthuongquans.AsNoTracking().FirstOrDefault(n => n.MaMtq.Equals(HttpContext.Session.GetInt32("idmtq")));
            //send mail



            _context.Add(ttQuyengopHienvat);
            await _context.SaveChangesAsync();
            return RedirectToAction("CamOn", new
            {
                id = ttQuyengopHienvat.MaQghv
            });
        }

        public async Task<IActionResult> CamOn(int id)
        {
            var ttqg = _context.TtQuyengopHienvats.Include(r => r.MaCdNavigation).Include(r => r.MaMtqNavigation).Include(r => r.MaHvNavigation).FirstOrDefault(s => s.MaQghv == id);
            if (ttqg.MaMtqNavigation.EmailMtq != null)
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Thiện Nguyện Online", "ThienNguyenOnline@gmail.com"));
                message.To.Add(new MailboxAddress("Mạnh thường quân", ttqg.MaMtqNavigation.EmailMtq));
                message.Subject = "Thư cảm ơn đăng ký quyên góp";
                message.Body = new TextPart("plain")
                {
                    Text = $"Cảm ơn bạn đã quyên góp\n" +
      
       $"Xin hãy gửi kèm MÃ QUYÊN GÓP HIỆN VẬT: \"{ttqg.MaQghv}\" này cho chúng tôi\n" +
      
       $"Mã quyên góp hiện vật: " +
       $"{ttqg.MaQghv}\n" +
       
       $"Mạnh thường quân: " +
      
       $"{ttqg.MaMtqNavigation.HotenMtq}\n" +
       $"Chiến dịch: " +
       $"{ttqg.MaCdNavigation.TenCd} \n" +
       
       $"Hiện vật: " +
      
       $"{ttqg.MaHvNavigation.TenHv} \n" +
      
       $"Số lượng: " +
      
       $"{ttqg.SoluongQg} " +
       $"{ttqg.MaHvNavigation.Donvitinh}  \n" +
       
       $"Ghi chú: " +
       
       $"{ttqg.Ghichu}  \n" +
       
       $"Trạng thái: " +
       
       $"{ttqg.TrangthaiHv} \n" +
      
       $"Ngày quyên góp: " +
      
       $"{ttqg.NgayQgFormatted}\n"
      
                };

                using var client = new SmtpClient();
                client.Connect("smtp.gmail.com");
                client.Authenticate("devthai3401@gmail.com", "mfpcaknsbfmwrvzd");
                client.Send(message);
                client.Disconnect(true);
            }
            return View(ttqg);
        }

        // GET: TtQuyengopHienvats/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TtQuyengopHienvats == null)
            {
                return NotFound();
            }
            if (HttpContext.Session.GetInt32("idmtq") == null)
                return RedirectToAction("Register", "User");
            var ttQuyengopHienvat = await _context.TtQuyengopHienvats.FindAsync(id);
            if (ttQuyengopHienvat == null)
            {
                return NotFound();
            }
            if (ttQuyengopHienvat.MaMtq != HttpContext.Session.GetInt32("idmtq") || ttQuyengopHienvat.TrangthaiHv.Trim() == "Đã nhận" || ttQuyengopHienvat.TrangthaiHv.Trim() == "Đã tặng")
            {

                return RedirectToAction("index", "TtQuyengopHienvats");
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
            if (HttpContext.Session.GetInt32("idmtq") == null)
                return RedirectToAction("Register", "User");
            var ttQuyengopHienvat = await _context.TtQuyengopHienvats
                .Include(t => t.MaCdNavigation)
                .Include(t => t.MaHvNavigation)
                .Include(t => t.MaMtqNavigation)
                .FirstOrDefaultAsync(m => m.MaQghv == id);
            if (ttQuyengopHienvat == null)
            {
                return NotFound();
            }
            if (ttQuyengopHienvat.MaMtq != HttpContext.Session.GetInt32("idmtq") || ttQuyengopHienvat.TrangthaiHv.Trim() == "Đã nhận" || ttQuyengopHienvat.TrangthaiHv.Trim() == "Đã tặng")
            {

                return RedirectToAction("index", "TtQuyengopHienvats");
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
