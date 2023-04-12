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
    public class TtTraotangsController : Controller
    {
        private readonly NienluancosoContext _context;

        public TtTraotangsController(NienluancosoContext context)
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

            return File(bytes, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "BaoCaoTraoTang.docx");
        }
        // GET: Admin/TtTraotangs
        public async Task<IActionResult> Index(string? SearchString, DateTime? tu, DateTime? den)
        {
            if (HttpContext.Session.GetInt32("idtv") == null)
            {
                return RedirectToAction("Login", "ThanhVien");
            }
            var count = _context.Quyens.Where(c => c.MaCn == 8 && c.MaCv == HttpContext.Session.GetInt32("cvtv")).Count();
            if (count == 0)
            {
                return RedirectToAction("norole", "Home");
            }
            if (SearchString != null)
            {
                var nienluancosoContext2 = _context.TtTraotangs.Include(t => t.MaCdNavigation).Include(t => t.MaHvNavigation).Include(t => t.MaTvNavigation).Include(t => t.ManoiNavigation).Where(q => q.ManoiNavigation.Diachi.Contains(SearchString));
                if (nienluancosoContext2.Count() == 0)
                {
                    ViewBag.tb = "Không tìm thấy trao tặng có địa chỉ : " + SearchString.ToString();
                }
                else
                {
                    ViewBag.tb = "Đã tìm thấy trao tặng có địa chỉ : " + SearchString.ToString();
                }
                return View(await nienluancosoContext2.ToListAsync());
            }
            if (tu != null && den != null)
            {
                var nienluancosoContext2 = _context.TtTraotangs.Include(t => t.MaCdNavigation).Include(t => t.MaHvNavigation).Include(t => t.MaTvNavigation).Include(t => t.ManoiNavigation).Where(q => q.Ngaytang > tu && q.Ngaytang < den);

                ViewBag.TuNgay = tu.Value.ToString("dd-MM-yyyy");
                ViewBag.DenNgay = den.Value.ToString("dd-MM-yyyy");
                return View(await nienluancosoContext2.ToListAsync());
            }
            var nienluancosoContext = _context.TtTraotangs.Include(t => t.MaCdNavigation).Include(t => t.MaHvNavigation).Include(t => t.MaTvNavigation).Include(t => t.ManoiNavigation);
            return View(await nienluancosoContext.ToListAsync());
        }

        // GET: Admin/TtTraotangs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (HttpContext.Session.GetInt32("idtv") == null)
            {
                return RedirectToAction("Login", "ThanhVien");
            }
            var count = _context.Quyens.Where(c => c.MaCn == 8 && c.MaCv == HttpContext.Session.GetInt32("cvtv")).Count();
            if (count == 0)
            {
                return RedirectToAction("norole", "Home");
            }
            if (id == null || _context.TtTraotangs == null)
            {
                return NotFound();
            }

            var ttTraotang = await _context.TtTraotangs
                .Include(t => t.MaCdNavigation)
                .Include(t => t.MaHvNavigation)
                .Include(t => t.MaTvNavigation)
                .Include(t => t.ManoiNavigation)
                .FirstOrDefaultAsync(m => m.MaTt == id);
            if (ttTraotang == null)
            {
                return NotFound();
            }

            return View(ttTraotang);
        }

        // GET: Admin/TtTraotangs/Create
        public IActionResult Create(int? manoi, int? macd)
        {
            if (HttpContext.Session.GetInt32("idtv") == null)
            {
                return RedirectToAction("Login", "ThanhVien");
            }
            var count = _context.Quyens.Where(c => c.MaCn == 8 && c.MaCv == HttpContext.Session.GetInt32("cvtv")).Count();
            if (count == 0)
            {
                return RedirectToAction("norole", "Home");
            }
            if (manoi != null)
            {
                
                var cd3 = _context.Chiendiches.Include(c => c.TtTraotangs).Where(c => c.MaNoi == manoi && c.Ngayketthuc < DateTime.Now && c.TtTraotangs.Count == 0).ToList();
                cd3.Insert(0, new Chiendich { MaCd = 0, TenCd = "---KHÔNG---" });
                ViewData["MaCd"] = new SelectList(cd3, "MaCd", "TenCd");
                ViewData["MaHv"] = new SelectList(_context.HienVats, "MaHv", "TenHv");
                ViewData["MaTv"] = new SelectList(_context.Thanhviens, "MaTv", "TenTv");
                ViewData["Manoi"] = new SelectList(_context.Noihotros.Where(n => n.Manoi == manoi), "Manoi", "Diachi");
                return View();
            }
            if (macd != null)
            {

                var cd2 = _context.Chiendiches.Where(c => c.MaCd == macd).ToList();
                if (cd2 != null)
                {
                    var hv = _context.HienVats.Where(c => c.MaHv == null).ToList();
                    hv.Insert(0, new HienVat { MaHv = 0, TenHv = "---Tất cả hiện vật đã nhận của chiến dịch---" });
                    Chiendich cd1 = _context.Chiendiches.FirstOrDefault(cd => cd.MaCd == macd);
                    ViewData["MaCd"] = new SelectList(cd2, "MaCd", "TenCd");
                    ViewData["MaHv"] = new SelectList(hv, "MaHv", "TenHv");
                    ViewData["MaTv"] = new SelectList(_context.Thanhviens, "MaTv", "TenTv");
                    ViewData["Manoi"] = new SelectList(_context.Noihotros.Where(n => n.Manoi == cd1.MaNoi), "Manoi", "Diachi");
                }
                return View();
            }
            var cd = _context.Chiendiches.Where(c => c.MaCd == null).ToList();
            cd.Insert(0, new Chiendich { MaCd = 0, TenCd = "---KHÔNG---" });
            ViewData["MaCd"] = new SelectList(cd, "MaCd", "TenCd");
            ViewData["MaHv"] = new SelectList(_context.HienVats, "MaHv", "TenHv");
            ViewData["MaTv"] = new SelectList(_context.Thanhviens, "MaTv", "TenTv");
            ViewData["Manoi"] = new SelectList(_context.Noihotros.Where(n => n.TrangthaiNht.Trim() == "Đã duyệt"), "Manoi", "Diachi");
            return View();
        }

        // POST: Admin/TtTraotangs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaTt,Manoi,MaHv,MaCd,SoluongTt,Ngaytang,AnhTt,MaTv")] TtTraotang ttTraotang, IFormFile file)
        {
            if (ttTraotang.MaCd != 0)
            {
                ttTraotang.SoluongTt = 0;
                
                if (ttTraotang.AnhTt == null && file == null && ttTraotang.MaCd != null)
                {
                    
                    ViewData["MaCd"] = new SelectList(_context.Chiendiches.Where(c => c.MaCd == ttTraotang.MaCd), "MaCd", "TenCd");
                    var hv = _context.HienVats.Where(c => c.MaHv == null).ToList();
                    hv.Insert(0, new HienVat { MaHv = 0, TenHv = "---Tất cả hiện vật đã nhận của chiến dịch---" });
                    ViewData["MaHv"] = new SelectList(hv, "MaHv", "TenHv", ttTraotang.MaHv);
                    ViewData["MaTv"] = new SelectList(_context.Thanhviens, "MaTv", "TenTv", ttTraotang.MaTv);
                    ViewData["Manoi"] = new SelectList(_context.Noihotros.Where(n => n.Manoi == ttTraotang.Manoi), "Manoi", "Diachi", ttTraotang.Manoi);
                    ModelState.AddModelError("AnhTt", "Vui lòng thêm ảnh trao tặng");
                    return View(ttTraotang);
                }
                var fileName = Path.GetFileName(file.FileName);

                //Luu duong dan file anh
                var path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "Content/images/traotang/"));

                //Kiem tra file da ton tai
                if (System.IO.File.Exists(path))
                {
                    ViewBag.ThongBao = "Hình ảnh đã tồn tại";
                }
                else
                {
                    await UploadFile(file);
                }
                ttTraotang.AnhTt = file.FileName;

                var listTtQghv =await _context.TtQuyengopHienvats.AsNoTracking().Where(t => t.MaCd == ttTraotang.MaCd && t.TrangthaiHv.Trim() == "Đã nhận").ToListAsync();

                foreach (var ttQghv in listTtQghv)
                {
                    ttQghv.TrangthaiHv = "Đã tặng";
                 _context.Update(ttQghv);
                   
                }
            
                ttTraotang.MaHv = null;
                _context.Add(ttTraotang);
                await _context.SaveChangesAsync();
            }
            else
            {
                ttTraotang.MaCd = null;

                HienVat hv = _context.HienVats.FirstOrDefault(h => h.MaHv == ttTraotang.MaHv);

                if (ttTraotang.SoluongTt > hv.Soluongcon && ttTraotang.MaCd == null)
                {
                    var cd = _context.Chiendiches.Where(c => c.MaCd == null).ToList();
                    cd.Insert(0, new Chiendich { MaCd = 0, TenCd = "---KHÔNG---" });
                    ViewData["MaCd"] = new SelectList(cd, "MaCd", "TenCd");
                    ViewData["MaHv"] = new SelectList(_context.HienVats, "MaHv", "TenHv", ttTraotang.MaHv);
                    ViewData["MaTv"] = new SelectList(_context.Thanhviens, "MaTv", "TenTv", ttTraotang.MaTv);
                    ViewData["Manoi"] = new SelectList(_context.Noihotros.Where(n => n.Manoi == ttTraotang.Manoi), "Manoi", "Diachi", ttTraotang.Manoi);
                    ModelState.AddModelError("SoluongTt", "Số lượng trao tặng vượt quá số lượng trong kho!");
                    return View(ttTraotang);
                }
                if (ttTraotang.MaCd == null && ttTraotang.SoluongTt <= 0)
                {
                    var cd = _context.Chiendiches.Where(c => c.MaCd == null).ToList();
                    cd.Insert(0, new Chiendich { MaCd = 0, TenCd = "---KHÔNG---" });
                    ViewData["MaCd"] = new SelectList(cd, "MaCd", "TenCd");
                    ViewData["MaHv"] = new SelectList(_context.HienVats, "MaHv", "TenHv", ttTraotang.MaHv);
                    ViewData["MaTv"] = new SelectList(_context.Thanhviens, "MaTv", "TenTv", ttTraotang.MaTv);
                    ViewData["Manoi"] = new SelectList(_context.Noihotros.Where(n => n.Manoi == ttTraotang.Manoi), "Manoi", "Diachi", ttTraotang.Manoi);
                    ModelState.AddModelError("SoluongTt", "Số lượng trao tặng ít nhất là 1!");
                    return View(ttTraotang);
                }
                if (ttTraotang.AnhTt == null && file == null && ttTraotang.MaCd == null)
                {
                    var cd = _context.Chiendiches.Where(c => c.MaCd == null).ToList();
                    cd.Insert(0, new Chiendich { MaCd = 0, TenCd = "---KHÔNG---" });
                    ViewData["MaCd"] = new SelectList(cd, "MaCd", "TenCd");
                    ViewData["MaHv"] = new SelectList(_context.HienVats, "MaHv", "TenHv", ttTraotang.MaHv);
                    ViewData["MaTv"] = new SelectList(_context.Thanhviens, "MaTv", "TenTv", ttTraotang.MaTv);
                    ViewData["Manoi"] = new SelectList(_context.Noihotros.Where(n => n.Manoi == ttTraotang.Manoi), "Manoi", "Diachi", ttTraotang.Manoi);
                    ModelState.AddModelError("AnhTt", "Vui lòng thêm ảnh trao tặng");
                    return View(ttTraotang);
                }
                var fileName = Path.GetFileName(file.FileName);

                //Luu duong dan file anh
                var path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "Content/images/traotang/"));

                //Kiem tra file da ton tai
                if (System.IO.File.Exists(path))
                {
                    ViewBag.ThongBao = "Hình ảnh đã tồn tại";
                }
                else
                {
                    await UploadFile(file);
                }
                ttTraotang.AnhTt = file.FileName;

                hv.Soluongcon -= ttTraotang.SoluongTt;
                await _context.SaveChangesAsync();
                _context.Add(ttTraotang);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));

        }

        // GET: Admin/TtTraotangs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (HttpContext.Session.GetInt32("idtv") == null)
            {
                return RedirectToAction("Login", "ThanhVien");
            }
            var count = _context.Quyens.Where(c => c.MaCn == 8 && c.MaCv == HttpContext.Session.GetInt32("cvtv")).Count();
            if (count == 0)
            {
                return RedirectToAction("norole", "Home");
            }
            if (id == null || _context.TtTraotangs == null)
            {
                return NotFound();
            }

            var ttTraotang = await _context.TtTraotangs.FindAsync(id);
            
            if (ttTraotang == null)
            {
                return NotFound();
            }
            if(ttTraotang.MaCd != null)
            {
                var cd2 = _context.Chiendiches.Where(c => c.MaCd == ttTraotang.MaCd).ToList();
                if (cd2 != null)
                {
                    var hv = _context.HienVats.Where(c => c.MaHv == null).ToList();
                    hv.Insert(0, new HienVat { MaHv = 0, TenHv = "---Tất cả hiện vật đã nhận của chiến dịch---" });
                    Chiendich cd1 = _context.Chiendiches.FirstOrDefault(cd => cd.MaCd == ttTraotang.MaCd);
                    ViewData["MaCd"] = new SelectList(cd2, "MaCd", "TenCd");
                    ViewData["MaHv"] = new SelectList(hv, "MaHv", "TenHv");
                    ViewData["MaTv"] = new SelectList(_context.Thanhviens, "MaTv", "TenTv");
                    ViewData["Manoi"] = new SelectList(_context.Noihotros.Where(n => n.Manoi == cd1.MaNoi), "Manoi", "Diachi");
                }
                return View(ttTraotang);
            }
            var cd = _context.Chiendiches.Where(c => c.MaCd == null).ToList();
            cd.Insert(0, new Chiendich { MaCd = 0, TenCd = "---KHÔNG---" }) ;
            ViewData["MaCd"] = new SelectList(cd, "MaCd", "TenCd");
            ViewData["MaHv"] = new SelectList(_context.HienVats, "MaHv", "TenHv");
            ViewData["MaTv"] = new SelectList(_context.Thanhviens, "MaTv", "TenTv");
            ViewData["Manoi"] = new SelectList(_context.Noihotros.Where(n => n.TrangthaiNht.Trim() == "Đã duyệt"), "Manoi", "Diachi");
            return View(ttTraotang);
        }

        // POST: Admin/TtTraotangs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaTt,Manoi,MaHv,MaCd,SoluongTt,Ngaytang,AnhTt,MaTv")] TtTraotang ttTraotang, IFormFile file)
        {
            if(ttTraotang.MaCd == 0) { ttTraotang.MaCd = null; }
            if (ttTraotang.MaCd != null )
            {
                var tt = _context.TtTraotangs.AsNoTracking().SingleOrDefault(s => s.MaCd == ttTraotang.MaCd);
                if (file != null)
                {
                    //Upload file
                    var fileName = Path.GetFileName(file.FileName);

                    //Luu duong dan file anh
                    var path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "Content/images/traotang/"));

                    //Kiem tra file da ton tai
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.ThongBao = "Hình ảnh đã tồn tại";
                    }
                    else
                    {
                        await UploadFile(file);
                    }
                    ttTraotang.AnhTt = file.FileName;
                }
                else
                {
                    ttTraotang.AnhTt = tt.AnhTt;
                }
                

                var listTtQghv = await _context.TtQuyengopHienvats.AsNoTracking().Where(t => t.MaCd == ttTraotang.MaCd && t.TrangthaiHv.Trim() == "Đã nhận").ToListAsync();

                foreach (var ttQghv in listTtQghv)
                {
                    ttQghv.TrangthaiHv = "Đã tặng";
                    _context.Update(ttQghv);

                }

                ttTraotang.MaHv = null;
                ttTraotang.SoluongTt = 0;
                _context.Update(ttTraotang);
                await _context.SaveChangesAsync();
            }
            else
            {
                
                var tt = _context.TtTraotangs.AsNoTracking().SingleOrDefault(s => s.MaTt == ttTraotang.MaTt);
                HienVat hvc = _context.HienVats.FirstOrDefault(h => h.MaHv == tt.MaHv);
                HienVat hvm = _context.HienVats.FirstOrDefault(h => h.MaHv == ttTraotang.MaHv);
                if (hvc.TenHv == hvm.TenHv && tt.SoluongTt != ttTraotang.SoluongTt)
                {
                
                    hvm.Soluongcon = hvc.Soluongcon += tt.SoluongTt;
                    
                }
                else if(hvc.TenHv != hvm.TenHv)
                {
                    hvc.Soluongcon += tt.SoluongTt;
                    hvm.Soluongcon -=ttTraotang.SoluongTt;
                }
                if (ttTraotang.SoluongTt > hvm.Soluongcon && ttTraotang.MaCd == null)
                {
                    var cd = _context.Chiendiches.Where(c => c.MaCd == null).ToList();
                    cd.Insert(0, new Chiendich { MaCd = 0, TenCd = "---KHÔNG---" });
                    ViewData["MaCd"] = new SelectList(cd, "MaCd", "TenCd");
                    ViewData["MaHv"] = new SelectList(_context.HienVats, "MaHv", "TenHv", ttTraotang.MaHv);
                    ViewData["MaTv"] = new SelectList(_context.Thanhviens, "MaTv", "TenTv", ttTraotang.MaTv);
                    ViewData["Manoi"] = new SelectList(_context.Noihotros.Where(n => n.Manoi == ttTraotang.Manoi), "Manoi", "Diachi", ttTraotang.Manoi);
                    ModelState.AddModelError("SoluongTt", "Số lượng trao tặng vượt quá số lượng trong kho!");
                    return View(ttTraotang);
                }
                if ( ttTraotang.MaCd == null && ttTraotang.SoluongTt <= 0)
                {
                    var cd = _context.Chiendiches.Where(c => c.MaCd == null).ToList();
                    cd.Insert(0, new Chiendich { MaCd = 0, TenCd = "---KHÔNG---" });
                    ViewData["MaCd"] = new SelectList(cd, "MaCd", "TenCd");
                    ViewData["MaHv"] = new SelectList(_context.HienVats, "MaHv", "TenHv", ttTraotang.MaHv);
                    ViewData["MaTv"] = new SelectList(_context.Thanhviens, "MaTv", "TenTv", ttTraotang.MaTv);
                    ViewData["Manoi"] = new SelectList(_context.Noihotros.Where(n => n.Manoi == ttTraotang.Manoi), "Manoi", "Diachi", ttTraotang.Manoi);
                    ModelState.AddModelError("SoluongTt", "Số lượng trao tặng ít nhất là 1!");
                    return View(ttTraotang);
                }
                if (ttTraotang.AnhTt == null && file == null && tt.AnhTt == null && ttTraotang.MaCd == null)
                {
                    var cd = _context.Chiendiches.Where(c => c.MaCd == null).ToList();
                    cd.Insert(0, new Chiendich { MaCd = 0, TenCd = "---KHÔNG---" });
                    ViewData["MaCd"] = new SelectList(cd, "MaCd", "TenCd");
                    ViewData["MaHv"] = new SelectList(_context.HienVats, "MaHv", "TenHv", ttTraotang.MaHv);
                    ViewData["MaTv"] = new SelectList(_context.Thanhviens, "MaTv", "TenTv", ttTraotang.MaTv);
                    ViewData["Manoi"] = new SelectList(_context.Noihotros.Where(n => n.Manoi == ttTraotang.Manoi), "Manoi", "Diachi", ttTraotang.Manoi);
                    ModelState.AddModelError("AnhTt", "Vui lòng thêm ảnh trao tặng");
                    return View(ttTraotang);
                }
               
                if (file != null)
                {
                    //Upload file
                    var fileName = Path.GetFileName(file.FileName);

                    //Luu duong dan file anh
                    var path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "Content/images/traotang/"));

                    //Kiem tra file da ton tai
                    if (System.IO.File.Exists(path))
                    {
                        ModelState.AddModelError("AnhTt", "Ảnh đã tồn tại! Vui lòng chọn ảnh khác");
                    }
                    else
                    {
                        await UploadFile(file);
                    }
                    ttTraotang.AnhTt = file.FileName;
                }
                else
                {
                    ttTraotang.AnhTt = tt.AnhTt;
                }
                if (hvc.TenHv == hvm.TenHv && tt.SoluongTt != ttTraotang.SoluongTt)
                {

                    hvm.Soluongcon -=ttTraotang.SoluongTt;

                }

                _context.Update(ttTraotang);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/TtTraotangs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (HttpContext.Session.GetInt32("idtv") == null)
            {
                return RedirectToAction("Login", "ThanhVien");
            }
            var count = _context.Quyens.Where(c => c.MaCn == 8 && c.MaCv == HttpContext.Session.GetInt32("cvtv")).Count();
            if (count == 0)
            {
                return RedirectToAction("norole", "Home");
            }
            if (id == null || _context.TtTraotangs == null)
            {
                return NotFound();
            }

            var ttTraotang = await _context.TtTraotangs
                .Include(t => t.MaCdNavigation)
                .Include(t => t.MaHvNavigation)
                .Include(t => t.MaTvNavigation)
                .Include(t => t.ManoiNavigation)
                .FirstOrDefaultAsync(m => m.MaTt == id);
            if (ttTraotang == null)
            {
                return NotFound();
            }

            return View(ttTraotang);
        }

        // POST: Admin/TtTraotangs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TtTraotangs == null)
            {
                return Problem("Entity set 'NienluancosoContext.TtTraotangs'  is null.");
            }
            var ttTraotang = await _context.TtTraotangs.FindAsync(id);
            if (ttTraotang != null  && ttTraotang.MaCd == null && ttTraotang.MaHv != null)
            {
                HienVat hvc = _context.HienVats.FirstOrDefault(h => h.MaHv == ttTraotang.MaHv);
                hvc.Soluongcon += ttTraotang.SoluongTt;
                _context.TtTraotangs.Remove(ttTraotang);
            }
            if(ttTraotang != null && ttTraotang.MaCd != null && ttTraotang.MaHv == null)
            {
                var listTtQghv = await _context.TtQuyengopHienvats.AsNoTracking().Where(t => t.MaCd == ttTraotang.MaCd && t.TrangthaiHv.Trim() == "Đã tặng").ToListAsync();

                foreach (var ttQghv in listTtQghv)
                {
                    ttQghv.TrangthaiHv = "Đã nhận";
                    _context.Update(ttQghv);

                }
                _context.TtTraotangs.Remove(ttTraotang);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TtTraotangExists(int id)
        {
            return (_context.TtTraotangs?.Any(e => e.MaTt == id)).GetValueOrDefault();
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
                    path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/images/traotang/"));
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
