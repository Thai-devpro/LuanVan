using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LuanVan.Data;
using System.Data;
using System.IO;
using SautinSoft.Document;
using System.Text;
using Microsoft.Extensions.Options;
using LuanVan.Areas.Admin.Models;
using System.Drawing;

namespace LuanVan.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ChiendichesController : Controller
    {
        private readonly NienluancosoContext _context;
       

        public ChiendichesController(NienluancosoContext context )
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

            return File(bytes, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "BaoCaoChienDich.docx");
        }
       
        public async Task<IActionResult> ThongKeCD(int? id)
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
            var cd = _context.Chiendiches.AsNoTracking().FirstOrDefault(c => c.MaCd == id);
            ViewBag.tencd = cd.TenCd;

            // Thống kê số lượt quyên góp theo năm và tháng
            var campaignStats = _context.TtQuyengopHienvats
                .Where(q => q.MaCd == id)
                .GroupBy(q => new { q.NgayQg.Year, q.NgayQg.Month })
                .Select(g => new { Year = g.Key.Year, Month = g.Key.Month, Total = g.Count() })
                .ToList();

            // Tạo bảng 2 chiều (12 tháng x các năm)
            var table = new int[12, DateTime.Today.Year - campaignStats.Min(stats => stats.Year) + 1];

            // Điền dữ liệu vào bảng
            foreach (var stats in campaignStats)
            {
                int row = stats.Month - 1; // Chuyển đổi tháng thành chỉ số hàng (0-11)
                int col = stats.Year - campaignStats.Min(s => s.Year); // Chuyển đổi năm thành chỉ số cột (0 trở đi)
                table[row, col] = stats.Total;
            }

            // Sử dụng bảng để tạo dữ liệu cho view
            var model = new List<YearlyCampaignStats>();
            int startYear = campaignStats.Min(stats => stats.Year);
            int endYear = DateTime.Today.Year;
            for (int year = startYear; year <= endYear; year++)
            {
                var yearlyStats = new YearlyCampaignStats { Year = year, MonthlyStats = new int[12] };
                for (int month = 1; month <= 12; month++)
                {
                    int row = month - 1;
                    int col = year - startYear;
                    int total = table[row, col];
                    yearlyStats.MonthlyStats[row] = total;
                }
                model.Add(yearlyStats);
            }

            return View(model);
        }
        public async Task<IActionResult> Index(string? SearchString, DateTime? tu, DateTime? den, string? noiht, int? tt)
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
            List<SelectListItem> items = new List<SelectListItem>
{
    new SelectListItem { Value = "1", Text = "Danh sách tất cả chiến dịch" },
    new SelectListItem { Value = "2", Text = "Danh sách chiến dịch đang diễn ra" },
    new SelectListItem { Value = "3", Text = "Danh sách chiến dịch đã kết thúc" },
     new SelectListItem { Value = "4", Text = "Danh sách chiến dịch chưa trao tặng" }
};
            ViewBag.tt = new SelectList(items, "Value", "Text", tt);
            if (tu != null && den != null && SearchString != null && noiht != null)
            {
                var nienluancosoContext2 = _context.Chiendiches.Include(c => c.MaNoiNavigation).Include(c => c.MaTvNavigation).Include(c => c.TtTraotangs).Include(c => c.TtQuyengopHienvats).Where(q => q.Ngaybatdau >= tu && q.Ngayketthuc <= den && q.TenCd.Contains(SearchString) && q.MaNoiNavigation.Diachi.Contains(noiht));

                if (nienluancosoContext2.Count() == 0)
                {
                    ViewBag.tb = "Không tìm thấy chiến dịch có tên: " + SearchString.ToString() + "Từ ngày: " + tu.Value.ToString("dd-MM-yyyy") + " đến: " + den.Value.ToString("dd-MM-yyyy") + " hỗ trợ cho địa chỉ: " + noiht.ToString();
                }
                else
                {
                    ViewBag.tb = "Đã tìm thấy chiến dịch có tên: " + SearchString.ToString() + " Từ ngày:" + tu.Value.ToString("dd-MM-yyyy") + " đến:" + den.Value.ToString("dd-MM-yyyy") + " hỗ trợ cho địa chỉ: " + noiht.ToString();
                }
                return View(await nienluancosoContext2.ToListAsync());
            }
            if (SearchString != null)
            {
                var nienluancosoContext2 = _context.Chiendiches.Include(c => c.MaNoiNavigation).Include(c => c.MaTvNavigation).Include(c => c.TtTraotangs).Include(c => c.TtQuyengopHienvats).Where(c => c.TenCd.Contains(SearchString));
                if (nienluancosoContext2.Count() == 0)
                {
                    ViewBag.tb = "Không tìm thấy chiến dịch có tên : " + SearchString.ToString();
                }
                else {
                    ViewBag.tb = "Đã tìm thấy chiến dịch có tên : " + SearchString.ToString();
                }
                return View(await nienluancosoContext2.ToListAsync());
            }
            if (noiht != null)
            {
                var nienluancosoContext2 = _context.Chiendiches.Include(c => c.MaNoiNavigation).Include(c => c.MaTvNavigation).Include(c => c.TtTraotangs).Include(c => c.TtQuyengopHienvats).Where(c => c.MaNoiNavigation.Diachi.Contains(noiht));
                if (nienluancosoContext2.Count() == 0)
                {
                    ViewBag.tb = "Không tìm thấy chiến dịch có địa chỉ nơi hỗ trợ: " + noiht.ToString();
                }
                else
                {
                    ViewBag.tb = "Đã tìm thấy chiến dịch có địa chỉ nơi hỗ trợ: " + noiht.ToString();
                }
                return View(await nienluancosoContext2.ToListAsync());
            }
            if (tu != null && den != null)
            {
                var nienluancosoContext2 = _context.Chiendiches.Include(c => c.MaNoiNavigation).Include(c => c.MaTvNavigation).Include(c => c.TtTraotangs).Include(c => c.TtQuyengopHienvats).Where(q => q.Ngaybatdau >= tu && q.Ngayketthuc <= den);

                if (nienluancosoContext2.Count() == 0)
                {
                    ViewBag.tb = "Không tìm thấy chiến dịch Từ ngày: " + tu.Value.ToString("dd - MM - yyyy") + " đến: " + den.Value.ToString("dd - MM - yyyy");
                }
                else
                {
                    ViewBag.tb = "Đã tìm thấy chiến dịch Từ ngày: " + tu.Value.ToString("dd - MM - yyyy") + " đến: " + den.Value.ToString("dd - MM - yyyy");
                }
                return View(await nienluancosoContext2.ToListAsync());
            }
            if (tt == 1)
            {

                var nienluancosoContext2 = _context.Chiendiches.Include(c => c.MaNoiNavigation).Include(c => c.MaTvNavigation).Include(c => c.TtTraotangs).Include(c => c.TtQuyengopHienvats);

                return View(await nienluancosoContext2.ToListAsync());
            }
            if (tt == 2)
            {

                var nienluancosoContext2 = _context.Chiendiches.Include(c => c.MaNoiNavigation).Include(c => c.MaTvNavigation).Include(c => c.TtTraotangs).Include(c => c.TtQuyengopHienvats).Where(q => q.Ngayketthuc > DateTime.Now);



                return View(await nienluancosoContext2.ToListAsync());
            }
            if (tt == 3)
            {

                var nienluancosoContext2 = _context.Chiendiches.Include(c => c.MaNoiNavigation).Include(c => c.MaTvNavigation).Include(c => c.TtTraotangs).Include(c => c.TtQuyengopHienvats).Where(q => q.Ngayketthuc < DateTime.Now);



                return View(await nienluancosoContext2.ToListAsync());
            }
            if (tt == 4)
            {

                var nienluancosoContext2 = _context.Chiendiches.Include(c => c.MaNoiNavigation).Include(c => c.MaTvNavigation).Include(c => c.TtTraotangs).Include(c => c.TtQuyengopHienvats).Where(q => q.Ngayketthuc < DateTime.Now && q.TtTraotangs.Count ==0 && q.MaNoi != null);



                return View(await nienluancosoContext2.ToListAsync());
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
            var count = _context.Quyens.Where(c => c.MaCn == 1 && c.MaCv == HttpContext.Session.GetInt32("cvtv")).Count();
            if (count == 0)
            {
                return RedirectToAction("norole", "Home");
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
        public IActionResult Create(int? id)
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
            if (id != null)
            {
                

                ViewData["MaNoi"] = new SelectList(_context.Noihotros.Where(n => n.Manoi == id), "Manoi", "Diachi");
            }
            else
            {
                var Noihotros = _context.Noihotros.Where(n => n.TrangthaiNht.Trim() == "Đã duyệt").ToList();
                Noihotros.Insert(0, new Noihotro { Manoi = 0, Diachi = "---KHÔNG---" });
                ViewData["MaNoi"] = new SelectList(Noihotros, "Manoi", "Diachi");
            }
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
            var count = _context.Quyens.Where(c => c.MaCn == 1 && c.MaCv == HttpContext.Session.GetInt32("cvtv")).Count();
            if (count == 0)
            {
                return RedirectToAction("norole", "Home");
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
            var count = _context.Quyens.Where(c => c.MaCn == 1 && c.MaCv == HttpContext.Session.GetInt32("cvtv")).Count();
            if (count == 0)
            {
                return RedirectToAction("norole", "Home");
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
                    path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/images/chiendich/"));
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
