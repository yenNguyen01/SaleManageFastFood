using PagedList;
using QLBH.Fastfood.Models;
using QLBH.Fastfood.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace QLBH.Fastfood.Controllers
{
    //[Authorize(Roles = "AdminHome")]
    public class AdminController : Controller
    {
        private ITaiKhoanService _taiKhoanService;
        private IPhanQuyenService _phanQuyenService;
        private ISanPhamService _sanPhamService;
        private IDonHangService _donHangService;
        private ILoaiSanPhamService _loaiSanPhamService;
        public AdminController(ITaiKhoanService taiKhoanService, IPhanQuyenService phanQuyenService, ISanPhamService sanPhamService, IDonHangService donHangService, ILoaiSanPhamService loaiSanPhamService)
        {
            _taiKhoanService = taiKhoanService;
            _phanQuyenService = phanQuyenService;
            _sanPhamService = sanPhamService;
            _donHangService = donHangService;
            _loaiSanPhamService = loaiSanPhamService;
        }
        [HttpGet]
        public ActionResult Index()
        {
                //ViewBag.SumAccessTimes = HttpContext.Application["SumAccessTimes"].ToString();
                //ViewBag.RealAccessTimes = HttpContext.Application["RealAccessTimes"].ToString();
                TaiKhoan user = Session["User"] as TaiKhoan;
                ViewBag.TotalProductPurchased = _sanPhamService.GetTotalProductPurchased();
                decimal TotalRevenue = _donHangService.GetTotalRevenue();
                if (TotalRevenue < 1000000)
                {
                    TotalRevenue = TotalRevenue / 1000;
                    ViewBag.TotalRevenue = TotalRevenue.ToString("0.##") + "K";
                }
                else
                {
                    TotalRevenue = TotalRevenue / 1000000;
                    ViewBag.TotalRevenue = TotalRevenue.ToString("0.##") + "M";
                }
                ViewBag.TotalOrder = _donHangService.GetTotalOrder();

                return View();
        
        }

        // GET: Admin
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        //[HttpPost]
        //public ActionResult Login(TaiKhoan user)
        //{
        //    //Check login
        //    TaiKhoan userCheck = _taiKhoanService.CheckLogin(user.Email, user.MatKhau);
        //    if (userCheck != null)
        //    {

        //        IEnumerable<PhanQuyen> decentralizations = _decentralizationService.GetDecentralizationByUserTypeID(userCheck.UserTypeID);
        //        string role = "";
        //        foreach (var item in decentralizations)
        //        {
        //            role += item.Role.Name + ",";
        //        }

        //        role = role.Substring(0, role.Length - 1);
        //        Decentralization(userCheck.ID, role);

        //        Session["User"] = userCheck;
        //        return RedirectToAction("Index");
        //    }
        //    else
        //    {
        //        ViewBag.Message = "Success";
        //    }
        //    return View();
        //}
        private void Decentralization(int ID, string Role)
        {
            FormsAuthentication.Initialize();
            var ticket = new FormsAuthenticationTicket(1,
                ID.ToString(),
                DateTime.Now,
                DateTime.Now.AddHours(3),
                false,
                Role,
                FormsAuthentication.FormsCookiePath);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));
            if (ticket.IsPersistent)
            {
                cookie.Expires = ticket.Expiration;
            }
            Response.Cookies.Add(cookie);
        }
        [HttpGet]
        public ActionResult Logout()
        {
            Session["Emloyee"] = null;
            return RedirectToAction("Login");
        }
        public ActionResult Incompetent()
        {
            return View();
        }
        [HttpGet]
        public ActionResult InfoUser()
        {
            TaiKhoan user = Session["User"] as TaiKhoan;
            return View(user);
        }
        [HttpGet]
        public ActionResult Edit()
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("SignUp", "Home");
            }
            TaiKhoan user = Session["User"] as TaiKhoan;
            //Check null
            if (user != null)
            {
                //Return view
                return Json(new
                {
                    ID = user.IDUser,
                    FullName = user.HoTen,
                    Address = user.DiaChi,
                    Email = user.Email,
                    PhoneNumber = user.SDT,
                    status = true
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //Return 404
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }
        [HttpPost]
        public ActionResult Edit(TaiKhoan user, HttpPostedFileBase ImageUpload)
        {
            if (Session["Emloyee"] == null)
            {
                return RedirectToAction("Login");
            }
            //Get data for DropdownList
            ViewBag.EmloyeeTypeIDEdit = new SelectList(_phanQuyenService.GetListUserType().OrderBy(x => x.TenQuyen), "ID", "Name");

            if (ImageUpload != null)
            {
                int errorCount = 0;
                //Check content image
                if (ImageUpload != null && ImageUpload.ContentLength > 0)
                {
                    //Check format iamge
                    if (ImageUpload.ContentType != "image/jpeg" && ImageUpload.ContentType != "image/png" && ImageUpload.ContentType != "image/jpg")
                    {
                        //Set viewbag
                        ViewBag.upload += "Hình ảnh không hợp lệ<br/>";
                        //increase by 1 unit errorCount
                        errorCount++;
                    }
                    else
                    {
                        //Get file name
                        var fileName = Path.GetFileName(ImageUpload.FileName);
                        //Get path
                        var path = Path.Combine(Server.MapPath("~/Content/home/img/gallery"), fileName);
                        //Check exitst
                        if (!System.IO.File.Exists(path))
                        {
                            //Add image into folder
                            ImageUpload.SaveAs(path);
                        }
                    }
                }
            }
            //Set TempData for checking in view to show swal
            TempData["edit"] = "Success";
            //Update emloyeetype
            TaiKhoan u = _taiKhoanService.GetByID(user.IDUser);
            u.HoTen = user.HoTen;
            u.DiaChi = user.DiaChi;
            u.Email = user.Email;
            _taiKhoanService.Update(u);
            Session["User"] = u;
            string Url = Request.Url.ToString();
            return RedirectToAction("InfoUser");
        }
        [HttpGet]
        public ActionResult ResetPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ResetPassword(string CurrentPassword, string NewPassword)
        {
            TaiKhoan user = Session["User"] as TaiKhoan;
            TaiKhoan userCheck = _taiKhoanService.CheckLogin(user.Email, CurrentPassword);
            if (userCheck != null)
            {
                _taiKhoanService.ResetPassword(user.IDUser, NewPassword);
                TempData["ResetPassword"] = "Success";
                return RedirectToAction("InfoUser");
            }
            else
            {
                ViewBag.Message = "Mật khẩu hiện tại không đúng!";
            }
            return View();
        }

    }
}