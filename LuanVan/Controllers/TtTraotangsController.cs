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
    public class TtTraotangsController : Controller
    {
        private readonly NienluancosoContext _context;

        public TtTraotangsController(NienluancosoContext context)
        {
            _context = context;
        }

        // GET: TtTraotangs
        public async Task<IActionResult> Index(string? SearchString, DateTime? tu, DateTime? den)
        {
            if (tu != null && den != null && SearchString != null)
            {
                var nienluancosoContext2 = _context.TtTraotangs.Include(t => t.MaCdNavigation).Include(t => t.MaHvNavigation).Include(t => t.MaTvNavigation).Include(t => t.ManoiNavigation).Where(q => q.Ngaytang >= tu && q.Ngaytang <= den && q.ManoiNavigation.Diachi.Contains(SearchString));

                if (nienluancosoContext2.Count() == 0)
                {
                    ViewBag.tb = "Không tìm thấy trao tặng có địa chỉ: " + SearchString.ToString() + "Từ ngày: " + tu.Value.ToString("dd-MM-yyyy") + "Đến: " + den.Value.ToString("dd-MM-yyyy");
                }
                else
                {
                    ViewBag.tb = "Đã tìm thấy  trao tặng có địa chỉ: " + SearchString.ToString() + " Từ ngày:" + tu.Value.ToString("dd-MM-yyyy") + " đến:" + den.Value.ToString("dd-MM-yyyy");
                }
                return View(await nienluancosoContext2.ToListAsync());
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
                var nienluancosoContext2 = _context.TtTraotangs.Include(t => t.MaCdNavigation).Include(t => t.MaHvNavigation).Include(t => t.MaTvNavigation).Include(t => t.ManoiNavigation).Where(q => q.Ngaytang >= tu && q.Ngaytang <= den);

                ViewBag.TuNgay = tu.Value.ToString("dd-MM-yyyy");
                ViewBag.DenNgay = den.Value.ToString("dd-MM-yyyy");
                return View(await nienluancosoContext2.ToListAsync());
            }
            var nienluancosoContext = _context.TtTraotangs.Include(t => t.MaCdNavigation).Include(t => t.MaHvNavigation).Include(t => t.MaTvNavigation).Include(t => t.ManoiNavigation);
            return View(await nienluancosoContext.ToListAsync());
        }

        // GET: TtTraotangs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
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

        
        
    }
}
