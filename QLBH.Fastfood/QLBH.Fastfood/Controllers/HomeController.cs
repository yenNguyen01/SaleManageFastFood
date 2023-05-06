using QLBH.Fastfood.Models;
using QLBH.Fastfood.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace QLBH.Fastfood.Controllers
{
    public class HomeController : Controller
    {
        #region Initialize
        private ISanPhamService _sanPhamService;
        private ILoaiSanPhamService _loaiSanPhamService;
        private ITaiKhoanService _taiKhoanService;
        private IGioHangService _gioHangService;
        private IPhanQuyenService _phanQuyenService;

        public HomeController(ISanPhamService sanPhamService, ILoaiSanPhamService loaiSanPhamService, ITaiKhoanService taiKhoanService, IGioHangService gioHangService, IPhanQuyenService phanQuyenService)
        {
            _sanPhamService = sanPhamService;
            _loaiSanPhamService = loaiSanPhamService;
            _taiKhoanService = taiKhoanService;
            _gioHangService = gioHangService;
            _phanQuyenService = phanQuyenService;
        }
        #endregion
        public ActionResult Index()
        {
            var listProductView = _sanPhamService.GetProductListLast();
            ViewBag.ListProductNew = listProductView;
            var listProdudct = _sanPhamService.GetProductListIndex();
            ViewBag.listProduct = listProdudct;
            var listProdudctDisocunt = _sanPhamService.GetProductListDiscount();
            ViewBag.listProductDiscount = listProdudctDisocunt;

            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult HeaderTopPartial()
        {
            return PartialView();
        }
        public ActionResult MenuPartial()
        {
            ViewBag.ListProductCategory = _loaiSanPhamService.GetProductCategoryHome();
            return PartialView();
        }
        [HttpGet]
        public ActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SignUp(TaiKhoan user)
        {
            Models.TaiKhoan check = _taiKhoanService.GetByEmail(user.Email);
            if (check == null)
            {
                Models.TaiKhoan check2 = _taiKhoanService.GetByPhoneNumber(user.SDT);
                if (check2 != null)
                {
                    check2.HoTen = user.HoTen;
                    check2.DiaChi = user.DiaChi;
                    check2.MatKhau = user.MatKhau;
                    check2.SDT = user.SDT;
                    _taiKhoanService.Update(check2);
                    return RedirectToAction("SignIn");
                }
            }
            else if (check != null)
            {
                check.HoTen = user.HoTen;
                check.DiaChi = user.DiaChi;
                check.MatKhau = user.MatKhau;
                check.SDT = user.SDT;
                _taiKhoanService.Update(check);
                return RedirectToAction("SignIn");
            }

            bool fail = false;
            //Check email
            if (_taiKhoanService.CheckEmail(user.Email) == false)
            {
                ViewBag.MessageEmail = "Email đã tồn tại";
                fail = true;
            }
            //Check phonenumber
            if (_taiKhoanService.CheckPhoneNumber(user.SDT) == false)
            {
                ViewBag.MessagePhoneNumber = "Số điện thoại đã tồn tại";
                fail = true;
            }
            if (fail)
            {
                return View(user);
            }
            else 
            {
                TaiKhoan user1 = _taiKhoanService.Add(user);
                return RedirectToAction("SignIn");
            }
        }
        [HttpGet]
        public ActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SignIn(TaiKhoan user, FormCollection collection)
        {
            string email = collection["Email"].ToString();
            string pass = collection["Password"].ToString();
            var userCheck = _taiKhoanService.CheckLogin(email, pass);
            if (userCheck != null)
            {
                if (email == "yennguyen01012k1@gmail.com")
                {
                    Session["User"] = userCheck;
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    Session["User"] = userCheck;
                    if (_gioHangService.CheckCartUser(userCheck.IDUser))
                    {
                        List<GioHang> carts = _gioHangService.GetCart(userCheck.IDUser);
                        Session["Cart"] = carts;
                        return RedirectToAction("Index");
                    }
                    if (Session["Cart"] != null)
                    {
                        List<GioHang> listCart = Session["Cart"] as List<GioHang>;
                        foreach (var item in listCart)
                        {
                            item.ID = userCheck.IDUser;
                            _gioHangService.AddCartIntoUser(item);
                        }
                    }
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ViewBag.Message = "Email hoặc mật khẩu không đúng.";
                return View();
            }
        }

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

        public ActionResult SignOut()
        {
            Session.Remove("User");
            Session.Remove("Cart");
            string[] myCookies = Request.Cookies.AllKeys;
            foreach (string cookie in myCookies)
            {
                Response.Cookies[cookie].Expires = DateTime.Now.AddDays(-1);
            }
            return RedirectToAction("Index");
        }
    }
}