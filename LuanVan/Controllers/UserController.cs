using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LuanVan.Data;
using LuanVan.Models;
using System.Diagnostics.Eventing.Reader;
using Microsoft.AspNetCore.Session;
using Microsoft.AspNetCore.Http;
using static System.Collections.Specialized.BitVector32;
using System.Text.RegularExpressions;

namespace LuanVan.Controllers
{
    public class UserController : Controller
    {
        private readonly NienluancosoContext _context;
        //private readonly IHttpContextAccessor

        public UserController(NienluancosoContext context)
        {
            _context = context;
        }

        // GET: User
        

        public async Task<ActionResult> Login()
        {


            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> Login(Manhthuongquan mtq)
        {

            if (mtq.SdtMtq == null)
            {
                ModelState.AddModelError("SdtMtq", "Số điện thoại không được trống!");
               
                return View(mtq);
            }
            if (mtq.MatkhauMtq == null)
            {
                ModelState.AddModelError("MatkhauMtq", "Vui lòng nhập mật khẩu!");

                return View(mtq);
            }
            Manhthuongquan ad = _context.Manhthuongquans.AsNoTracking().SingleOrDefault(n => n.SdtMtq.Equals(mtq.SdtMtq) && n.MatkhauMtq.Trim().Equals(mtq.MatkhauMtq.Trim()));
                if (ad != null)
                {
                    
                    HttpContext.Session.SetString("tkmtq", ad.HotenMtq);
                    HttpContext.Session.SetInt32("idmtq", ad.MaMtq);
                    return RedirectToAction("index", "Home");
                }
                else
                    ViewBag.Thongbao = "Số điện thoại hoặc mật khẩu không đúng";
                return View(mtq);
           
           
        }
        
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            
            return RedirectToAction("Login", "User");
        }

        public async Task<IActionResult> Details()
        {
           
            if (HttpContext.Session.GetInt32("idmtq") == null || _context.Manhthuongquans == null)
            {
                return NotFound();
            }

            var manhthuongquan = await _context.Manhthuongquans
                .FirstOrDefaultAsync(m => m.MaMtq == HttpContext.Session.GetInt32("idmtq"));
            if (manhthuongquan == null)
            {
                return NotFound();
            }

            return View(manhthuongquan);
        }

        // GET: User/Create
        public IActionResult Register()
        {
            return View();
        }

        // POST: User/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("MaMtq,HotenMtq,GioitinhMtq,DonviTochucMtq,SdtMtq,DiachiMtq,MatkhauMtq,EmailMtq")] Manhthuongquan mtq)
        {
            if (string.IsNullOrEmpty(mtq.HotenMtq) == true)
            {
                ModelState.AddModelError("HotenMtq", "Vui lòng nhập họ tên!");
                return View(mtq);
            }
            
            if (string.IsNullOrEmpty(mtq.DonviTochucMtq) == true)
            {
                ModelState.AddModelError("DonviTochucMtq", "Bạn có đại diện cho tổ chức nào không!");
                return View(mtq);
            }
            string regex = @"^([\+]?33[-]?|[0])?[1-9][0-9]{8}$";
            if (mtq.SdtMtq == null || !Regex.IsMatch(mtq.SdtMtq.ToString(), regex))
            {
                ModelState.AddModelError("SdtMtq", "Vui lòng nhập số điện thoại chính xác!");
                return View(mtq);
            }
            Manhthuongquan ad = _context.Manhthuongquans.AsNoTracking().FirstOrDefault(n => n.SdtMtq.Equals(mtq.SdtMtq));
            if (ad != null)
            {
                ModelState.AddModelError("SdtMtq", "Số điện thoại đã được sử dụng hãy chọn số khác!");
                return View(mtq);
            }

            if (string.IsNullOrEmpty(mtq.DiachiMtq) == true)
            {
                ModelState.AddModelError("DiachiMtq", "Vui lòng nhập địa chỉ!");
                return View(mtq);
            }
            string regex2 = @"^[a-zA-Z0-9.!#$%&'*+\/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$";
            if (mtq.EmailMtq != null && !Regex.IsMatch(mtq.EmailMtq.ToString().Trim(), regex2))
            {
                ModelState.AddModelError("EmailMtq", "Vui lòng nhập email chính xác!");
                ViewData["MaCv"] = new SelectList(_context.Chucvus, "MaCv", "TenCv", mtq.EmailMtq);
                return View(mtq);
            }
            if (string.IsNullOrEmpty(mtq.MatkhauMtq) == true || mtq.MatkhauMtq.Length <5 || mtq.MatkhauMtq.Length >9)
            {
                ModelState.AddModelError("MatkhauMtq", "Vui lòng nhập mật khẩu từ 5 đến 10 ký tự!");
                return View(mtq);
            }
            _context.Add(mtq);
                await _context.SaveChangesAsync();
            HttpContext.Session.SetString("tkmtq", mtq.HotenMtq);
            HttpContext.Session.SetInt32("idmtq", mtq.MaMtq);
            return RedirectToAction(nameof(Details));
            
        }
        public async Task<IActionResult> forgot()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> forgot(Manhthuongquan mtq)
        {
            string regex = @"^([\+]?33[-]?|[0])?[1-9][0-9]{8}$";
            if (mtq.SdtMtq == null || !Regex.IsMatch(mtq.SdtMtq.ToString(), regex))
            {
                ModelState.AddModelError("SdtMtq", "Vui lòng nhập số điện thoại chính xác!");
                return View(mtq);
            }
            Manhthuongquan ad = _context.Manhthuongquans.AsNoTracking().FirstOrDefault(n => n.SdtMtq.Equals(mtq.SdtMtq));
            if (ad == null)
            {
                ModelState.AddModelError("SdtMtq", "Số điện thoại chưa được đăng ký!");
                return View(mtq);
            } else
                ViewBag.Thongbao = "Mật khẩu của bạn là: " + ad.MatkhauMtq;
            return View (mtq);

        }

        // GET: User/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
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

        // POST: User/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaMtq,HotenMtq,GioitinhMtq,DonviTochucMtq,SdtMtq,DiachiMtq,MatkhauMtq,EmailMtq")] Manhthuongquan mtq)
        {
            if (id != mtq.MaMtq)
            {
                return NotFound();
            }

            if (string.IsNullOrEmpty(mtq.HotenMtq) == true)
            {
                ModelState.AddModelError("HotenMtq", "Vui lòng nhập họ tên!");
                return View(mtq);
            }

            if (string.IsNullOrEmpty(mtq.DonviTochucMtq) == true)
            {
                ModelState.AddModelError("DonviTochucMtq", "Bạn có đại diện cho tổ chức nào không!");
                return View(mtq);
            }
            string regex = @"^([\+]?33[-]?|[0])?[1-9][0-9]{8}$";
            if (mtq.SdtMtq == null || !Regex.IsMatch(mtq.SdtMtq.ToString(), regex))
            {
                ModelState.AddModelError("SdtMtq", "Vui lòng nhập số điện thoại chính xác!");
                return View(mtq);
            }
            Manhthuongquan ad = _context.Manhthuongquans.AsNoTracking().FirstOrDefault(n => n.SdtMtq.Equals(mtq.SdtMtq));
            Manhthuongquan ad2 = _context.Manhthuongquans.AsNoTracking().FirstOrDefault(n => n.MaMtq.Equals(HttpContext.Session.GetInt32("idmtq")));

            if (ad != null && ad.SdtMtq.Equals(ad2.SdtMtq) != true)
            {
                ModelState.AddModelError("SdtMtq", "Số điện thoại đã được sử dụng hãy chọn số khác!");
                return View(mtq);
            }

            if (string.IsNullOrEmpty(mtq.DiachiMtq) == true)
            {
                ModelState.AddModelError("DiachiMtq", "Vui lòng nhập địa chỉ!");
                return View(mtq);
            }
            string regex2 = @"^[a-zA-Z0-9.!#$%&'*+\/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$";
            if (mtq.EmailMtq != null && !Regex.IsMatch(mtq.EmailMtq.ToString().Trim(), regex2))
            {
                ModelState.AddModelError("EmailMtq", "Vui lòng nhập email chính xác!");
                ViewData["MaCv"] = new SelectList(_context.Chucvus, "MaCv", "TenCv", mtq.EmailMtq);
                return View(mtq);
            }
            if (string.IsNullOrEmpty(mtq.MatkhauMtq) == true || mtq.MatkhauMtq.Trim().Length < 5 || mtq.MatkhauMtq.Trim().Length > 9)
            {
                ModelState.AddModelError("MatkhauMtq", "Vui lòng nhập mật khẩu từ 5 đến 10 ký tự!");
                return View(mtq);
            }
            try
                {
                    _context.Update(mtq);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ManhthuongquanExists(mtq.MaMtq))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Details));
           
        }

        // GET: User/Delete/5
       

        private bool ManhthuongquanExists(int id)
        {
            return (_context.Manhthuongquans?.Any(e => e.MaMtq == id)).GetValueOrDefault();
        }
    }
}
