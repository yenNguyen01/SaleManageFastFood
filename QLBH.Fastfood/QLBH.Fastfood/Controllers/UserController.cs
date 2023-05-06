using QLBH.Fastfood.Models;
using QLBH.Fastfood.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace QLBH.Fastfood.Controllers
{
    public class UserController : Controller
    {
        ITaiKhoanService _taiKhoanService;
        IDonHangService _donHangService;
        IChiTietDonHangService _chiTietDonHangService;
        ISanPhamService _sanPhamService;
        IDanhGiaService _danhGiaService;
        IGioHangService _gioHangService;

        public UserController(ITaiKhoanService taiKhoanService, IDonHangService donHangService, IChiTietDonHangService chiTietDonHangService, ISanPhamService sanPhamService, IDanhGiaService ratingService, IGioHangService gioHangService, IDanhGiaService danhGiaService)
        {
            _taiKhoanService = taiKhoanService;
            _donHangService = donHangService;
            _chiTietDonHangService = chiTietDonHangService;
            _sanPhamService = sanPhamService;
            _danhGiaService = danhGiaService;
            _gioHangService = gioHangService;
        }
        [HttpGet]
        public ActionResult Index()
        {
            TaiKhoan user = Session["User"] as TaiKhoan;
            return View(user);
        }
        [HttpGet]
        public ActionResult EditName(int id)
        {
            var member = _taiKhoanService.GetByID(id);
            //Check null
            if (member != null)
            {
                //Return view
                return Json(new
                {
                    ID = member.IDUser,
                    FullName = member.HoTen,
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
        public ActionResult EditName(int ID, string FullName)
        {
            TaiKhoan user = _taiKhoanService.GetByID(ID);
            user.HoTen = FullName;
            _taiKhoanService.Update(user);
            IEnumerable<TaiKhoan> users = _taiKhoanService.GetList();
            foreach (TaiKhoan item in users)
            {
                if (item.SDT == user.SDT)
                {
                    item.HoTen = user.HoTen;
                    _taiKhoanService.Update(item);
                }
            }
            Session["User"] = user;
            TempData["EditName"] = "Sucess";
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult EditAddress(int id)
        {
            var member = _taiKhoanService.GetByID(id);
            //Check null
            if (member != null)
            {
                //Return view
                return Json(new
                {
                    ID = member.IDUser,
                    Address = member.DiaChi,
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
        public ActionResult EditAddress(int ID, string Address)
        {
            TaiKhoan user = _taiKhoanService.GetByID(ID);
            user.DiaChi = Address;
            _taiKhoanService.Update(user);
            Session["Member"] = user;
            TempData["EditAddress"] = "Sucess";
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult CheckoutOrder()
        {
            TaiKhoan userSession = Session["User"] as TaiKhoan;
            if (userSession != null)
            {
                string Phone = _taiKhoanService.GetByID(userSession.IDUser).SDT;
                TaiKhoan user = _taiKhoanService.GetList().FirstOrDefault(x => x.SDT == Phone);
                if (user != null)
                {
                    var orders = _donHangService.GetAll().Where(x => x.TaiKhoan.SDT == user.SDT);
                    ViewBag.ProductRating = _danhGiaService.GetListAllRating().Where(x => x.IDUser == user.IDUser);
                    return View(orders);
                }
                return View();
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public ActionResult OrderDetail(int ID)
        {
            if (Session["User"] == null)
            {
                return View();
            }
            if (ID == null)
            {
                return null;
            }
            DonHang order = _donHangService.GetByID(ID);
            IEnumerable<ChiTietDonHang> orderDetails = _chiTietDonHangService.GetByOrderID(ID);
            if (orderDetails == null)
            {
                return null;
            }
            ViewBag.OrderID = ID;
            if (order.ChapNhan)
            {
                ViewBag.Approved = "Approved";
            }
            if (order.GiaoHang)
            {
                ViewBag.Delivere = "Delivere";
            }
            if (order.DaNhan)
            {
                ViewBag.Received = "Received";
            }
            ViewBag.Total = order.TongCong;
            return View(orderDetails);
        }
        public ActionResult GetDataProduct(int ID)
        {
            SanPham product = _sanPhamService.GetByID(ID);
            return Json(new
            {
                ID = product.MaSP,
                Name = product.TenSP,
                Image = product.AnhSP,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Received(int OrderID)
        {
            TaiKhoan user = Session["User"] as TaiKhoan;
            //Update AmountPurchased for member
            if (user != null)
            {
                _donHangService.Received(OrderID);
                _taiKhoanService.UpdateAmountPurchased(user.IDUser, _donHangService.GetByID(OrderID).TongCong.Value);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("OrderDetail", new { ID = OrderID });
        }
        public JsonResult Cancel(int ID)
        {
            DonHang order = _donHangService.GetByID(ID);
            order.HuyDon = true;
            _donHangService.Update(order);
            return Json(new
            {
                status = true
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult DeleteAccount()
        {
            TaiKhoan user = Session["User"] as TaiKhoan;
            return View(user);
        }
        [HttpPost]
        public JsonResult DeleteAccount(string Password)
        {
            TaiKhoan user = Session["User"] as TaiKhoan;
            TaiKhoan userCheck = _taiKhoanService.CheckLogin(user.Email, Password);
            if (userCheck != null)
            {
                _taiKhoanService.Block(userCheck);
                _gioHangService.RemoveCartDeleteAccount(user.IDUser);
                TempData["DeleteAccount"] = "Success";
                Session["User"] = null;
                Session["Cart"] = null;
                return Json(new
                {
                    status = true
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                TempData["IncorretPassword"] = "true";
            }
            return Json(new
            {
                status = false
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult ResetPassword()
        {
            return View();
        }
        //[HttpPost]
        //public ActionResult ResetPassword(string CurrentPassword, string NewPassword)
        //{
        //    TaiKhoan user = Session["User"] as TaiKhoan;
        //    TaiKhoan userCheck = _taiKhoanService.CheckLogin(user.Email, CurrentPassword);
        //    if (userCheck != null)
        //    {
        //        _taiKhoanService.ResetPassword(userCheck.IDUser, NewPassword);
        //        TempData["ResetPassword"] = "Success";
        //        return RedirectToAction("Index");
        //    }
        //    else
        //    {
        //        ViewBag.Message = "Mật khẩu hiện tại không đúng!";
        //    }
        //    return RedirectToAction("Index");
        //}

        public JsonResult GetOrderJson()
        {
            var list = _donHangService.GetAll().Where(x => x.ChapNhan == false).OrderByDescending(x => x.NgayDat).Take(5).Select(x => new { ID = x.MaDH, CustomerName = x.TaiKhoan.HoTen, DateOrder = (DateTime.Now - x.NgayDat).Minutes });
            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }
}