using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LuanVan.Data;

namespace LuanVan.Controllers
{
    public class ChiendichesController : Controller
    {
        private readonly NienluancosoContext _context;

        public ChiendichesController(NienluancosoContext context)
        {
            _context = context;
        }

        // GET: Chiendiches
        public async Task<IActionResult> Index(string? SearchString, DateTime? tu, DateTime? den, string? noiht, int tt =2)
        {
            List<SelectListItem> items = new List<SelectListItem>
{
    new SelectListItem { Value = "1", Text = "Danh sách tất cả chiến dịch" },
    new SelectListItem { Value = "2", Text = "Danh sách chiến dịch đang diễn ra" },
    new SelectListItem { Value = "3", Text = "Danh sách chiến dịch đã kết thúc" }
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
                else
                {
                    ViewBag.tb = "Đã tìm thấy chiến dịch có tên : " + SearchString.ToString();
                }
                return View(await nienluancosoContext2.ToListAsync());
            }
            if (noiht != null)
            {
                var nienluancosoContext2 = _context.Chiendiches.Include(c => c.MaNoiNavigation).Include(c => c.MaTvNavigation).Include(c => c.TtTraotangs).Include(c => c.TtQuyengopHienvats).Where(c => c.MaNoiNavigation.Diachi.Contains(noiht));
                if (nienluancosoContext2.Count() == 0)
                {
                    ViewBag.tb = "Không tìm thấy chiến dịch có địa chỉ nơi hỗ trợ ở: " + noiht.ToString();
                }
                else
                {
                    ViewBag.tb = "Đã tìm thấy chiến dịch có địa chỉ nơi hỗ trợ ở: " + noiht.ToString();
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
            var nienluancosoContext = _context.Chiendiches.Include(c => c.MaNoiNavigation).Include(c => c.MaTvNavigation);
            return View(await nienluancosoContext.ToListAsync());
        }

        // GET: Chiendiches/Details/5
        public async Task<IActionResult> Details(int? id)
        {
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

        // GET: Chiendiches/Create
        private bool ChiendichExists(int id)
        {
          return (_context.Chiendiches?.Any(e => e.MaCd == id)).GetValueOrDefault();
        }
    }
}
