using QLBH.Fastfood.Models;
using QLBH.Fastfood.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLBH.Fastfood.Controllers
{
    public class CartController : Controller
    {
        private ISanPhamService _sanPhamService;
        private ITaiKhoanService _taiKhoanService;
        private IDonHangService _donHangService;
        private IChiTietDonHangService _chiTietDonHangService;
        private IGioHangService _gioHangService;

        public CartController(ISanPhamService sanPhamService, ITaiKhoanService taiKhoanService, IDonHangService donHangService, IChiTietDonHangService chiTietDonHangService, IGioHangService gioHangService)
        {
            _sanPhamService = sanPhamService;
            _taiKhoanService = taiKhoanService;
            _donHangService = donHangService;
            _chiTietDonHangService = chiTietDonHangService;
            _gioHangService = gioHangService;
        }
        // GET: Cart
        public List<GioHang> GetCart()
        {
            TaiKhoan user = Session["User"] as TaiKhoan;
            if (user != null)
            {
                if (_gioHangService.CheckCartUser(user.IDUser))
                {
                    List<GioHang> listCart = _gioHangService.GetCart(user.IDUser);
                    foreach (GioHang item in listCart)
                    {
                        if (item.Anh == null || item.Anh == "")
                        {
                            item.Anh = _sanPhamService.GetByID(item.MaSP).AnhSP;
                        }
                        if (item.GiaTien == 0)
                        {
                            item.GiaTien = _sanPhamService.GetByID(item.MaSP).GiaKhuyenMai;
                        }
                    }
                    Session["Cart"] = listCart;
                    return listCart;
                }
            }
            else
            {
                List<GioHang> listCart = Session["Cart"] as List<GioHang>;
                //Check null session Cart
                if (listCart == null)
                {
                    //Initialization listCart
                    listCart = new List<GioHang>();
                    Session["Cart"] = listCart;
                    return listCart;
                }
                return listCart;
            }
            return null;
        }
        [HttpPost]
        public ActionResult AddItemCart(int ID)
        {
            //Check product already exists in DB
            SanPham product = _sanPhamService.GetByID(ID);
            if (product == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            List<GioHang> listCart = GetCart();
            TaiKhoan user = Session["User"] as TaiKhoan;
            if (user != null)
            {
                //Case 1: If product already exists in Member Cart
                if (_gioHangService.CheckProductInCart(ID, user.IDUser))
                {
                    _gioHangService.AddQuantityProductCartUser(ID, user.IDUser);
                }
                else
                {
                    //Case 2: If product does not exist in User Cart
                    //Get product
                    GioHang itemCart = new GioHang(ID);
                    itemCart.IDUser = user.IDUser;
                    _gioHangService.AddCartIntoUser(itemCart);
                }
                List<GioHang> carts = _gioHangService.GetCart(user.IDUser);
                foreach (GioHang item in carts)
                {
                    if (item.Anh == null || item.Anh == "")
                    {
                        item.Anh = _sanPhamService.GetByID(item.MaSP).AnhSP;
                    }
                }
                Session["Cart"] = carts;
                ViewBag.TotalQuanity = GetTotalQuanity();
                ViewBag.TotalPrice = GetTotalPrice().ToString("#,##");
                return PartialView("CartPartial");
            }
            else
            {
                if (listCart != null)
                {
                    //Case 1: If product already exists in session Cart
                    GioHang itemCart = listCart.SingleOrDefault(n => n.MaSP == ID);
                    if (itemCart != null)
                    {
                        //Check inventory before letting customers make a purchase
                        if (product.SoLuong < itemCart.SoLuong)
                        {
                            return View("ThongBao");
                        }
                        itemCart.SoLuong++;
                        itemCart.TongCong = itemCart.SoLuong * product.GiaSP;
                        ViewBag.TotalQuanity = GetTotalQuanity();
                        ViewBag.TotalPrice = GetTotalPrice().ToString("#,##");
                        return PartialView("CartPartial");
                    }
                    //Case 2: If product does not exist in the Session Cart
                    GioHang item = new GioHang(ID);
                    item.Anh = _sanPhamService.GetByID(item.MaSP).AnhSP;
                    listCart.Add(item);
                }
            }
            ViewBag.TotalQuanity = GetTotalQuanity();
            ViewBag.TotalPrice = GetTotalPrice().ToString("#,##");
            return PartialView("CartPartial");
        }
        [HttpPost]
        public ActionResult CheckQuantityAdd(int ID)
        {
            //Check product already exists in DB
            SanPham product = _sanPhamService.GetByID(ID);
            if (product == null)
            {
                //product does not exist
                Response.StatusCode = 404;
                return null;
            }
            //Get cart
            List<GioHang> listCart = GetCart();
            //Check quantity
            if (listCart != null)
            {
                int sum = 0;
                foreach (GioHang item in listCart.Where(x => x.MaSP == ID))
                {
                    sum += item.SoLuong;
                }
                if (product.SoLuong > sum)
                {
                    return Json(new
                    {
                        status = true
                    });
                }
                else
                {
                    return Json(new
                    {
                        status = false
                    });
                }
            }
            else
            {
                if (product.SoLuong > 0)
                {

                    return Json(new
                    {
                        status = true
                    });
                }
                else
                {
                    return Json(new
                    {
                        status = false
                    });
                }
            }
        }
        [HttpPost]
        public ActionResult CheckQuantityUpdate(int ID, int Quantity)
        {
            //Check product already exists in DB
            SanPham product = _sanPhamService.GetByID(ID);
            if (product.SoLuong >= Quantity)
            {
                return Json(new
                {
                    status = true
                });
            }
            else
            {
                return Json(new
                {
                    status = false
                });
            }
        }
        public ActionResult CartPartial()
        {
            if (GetTotalQuanity() == 0)
            {
                ViewBag.TotalQuanity = 0;
                ViewBag.TotalPrice = 0;
                return PartialView();
            }
            ViewBag.TotalQuanity = GetTotalQuanity();
            ViewBag.TotalPrice = GetTotalPrice().ToString("#,##");
            return PartialView();
        }
        public double GetTotalQuanity()
        {
            List<GioHang> listCart = Session["Cart"] as List<GioHang>;
            if (listCart == null)
            {
                return 0;
            }
            return listCart.Sum(n => n.SoLuong);
        }
        public decimal GetTotalPrice()
        {
            //Lấy giỏ hàng
            List<GioHang> listCart = Session["Cart"] as List<GioHang>;
            if (listCart == null)
            {
                return 0;
            }
            var f = listCart.Sum(n => n.TongCong);
            return f;
        }
    
        [HttpGet]
        public ActionResult EditCart(int ID)
        {
            //Check null session cart
            if (Session["Cart"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            //Check whether the product exists in the database or not?
            SanPham product = _sanPhamService.GetByID(ID);
            if (product == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //Get cart
            List<GioHang> listCart = GetCart();
            //Check if the product exists in the shopping cart
            GioHang item = listCart.SingleOrDefault(n => n.MaSP == ID);
            if (item == null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Cart = listCart;
            return Json(new
            {
                ID = item.ID,
                Price = item.GiaTien,
                ProductID = item.MaSP,
                Quantity = item.SoLuong,
                Image = item.Anh,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult EditCart(int ID, int Quantity)
        {
            SanPham product = _sanPhamService.GetByID(ID);
            //Updated quantity in cart Session
            List<GioHang> listCart = GetCart();
            //Get products from within listCart to update
            GioHang itemCartUpdate = listCart.Find(n => n.MaSP == ID);
            itemCartUpdate.SoLuong = Quantity;
            itemCartUpdate.TongCong = itemCartUpdate.SoLuong * itemCartUpdate.GiaTien;

            TaiKhoan user = Session["User"] as TaiKhoan;
            if (user != null)
            {
                //Update Cart Quantity Member
                _gioHangService.UpdateQuantityCartUser(Quantity, ID, user.IDUser);
                Session["Cart"] = listCart;
            }

            return RedirectToAction("Checkout");

        }
        [HttpGet]
        public ActionResult RemoveItemCart(int ID)
        {
            //Check null session Cart
            if (Session["Cart"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            SanPham product = _sanPhamService.GetByID(ID);
            if (product == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //Get cart
            List<GioHang> listCart = GetCart();
            //Check if the product exists in the shopping cart
            GioHang item = listCart.SingleOrDefault(n => n.MaSP == ID);
            if (item == null)
            {
                return RedirectToAction("Index", "Home");
            }
            //Remove item cart
            listCart.Remove(item);
            TaiKhoan user = Session["User"] as TaiKhoan;
            if (user != null)
            {
                _gioHangService.RemoveCart(ID, user.IDUser);
                List<GioHang> carts = _gioHangService.GetCart(user.IDUser);
                Session["Cart"] = carts;
            }
            ViewBag.TotalQuantity = GetTotalQuanity();
            return PartialView("CheckoutPartial");
        }
        [HttpPost]
        public ActionResult AddOrder(TaiKhoan user, int NumberDiscountPass = 0, string CodePass = "", string payment = "")
        {
            //Check null session cart
            if (Session["Cart"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            TaiKhoan usercheck = new TaiKhoan();
            bool status = true;
            //Is User
            TaiKhoan userOrder = new TaiKhoan();
            if (Session["User"] == null)
            {
                status = false;
                return RedirectToAction("SignIn", "Home");

            }
            //Add order
            Models.DonHang order = new Models.DonHang();
            if (status)
            {
                order.IDUser = (Session["User"] as TaiKhoan).IDUser;
            }
            else
            {
                order.IDUser = userOrder.IDUser;
            }
            order.NgayDat = DateTime.Now;
            order.NgayGiao = DateTime.Now;
            order.TraTien = false;
            order.XoaDon = false;
            order.GiaoHang = false;
            order.ChapNhan = false;
            order.DaNhan = false;
            order.HuyDon = false;
            Models.DonHang o = _donHangService.AddOrder(order);
            Session["OrderId"] = o.MaDH;
            //Add order detail
            List<GioHang> listCart = GetCart();
            decimal sumtotal = 0;
            foreach (GioHang item in listCart)
            {
                ChiTietDonHang orderDetail = new ChiTietDonHang();
                orderDetail.MaDH = order.MaDH;
                orderDetail.MaSP = item.MaSP;
                orderDetail.SoLuong = item.SoLuong;
                orderDetail.GiaTien = item.GiaTien;
                _chiTietDonHangService.AddOrderDetail(orderDetail);
                sumtotal += orderDetail.SoLuong * orderDetail.GiaTien;

                if (Session["User"] != null)
                {
                    //Remove Cart
                    _gioHangService.RemoveCart(item.MaSP, item.IDUser);
                }
            }
            _donHangService.UpdateTotal(order.MaDH, sumtotal);

            // Payment
            if (payment == "paypal")
            {
                return RedirectToAction("PaymentWithPaypal", "Payment");
            }
  
            Session.Remove("Code");
            Session.Remove("num");
            Session.Remove("Cart");
            Session.Remove("OrderId");

            return RedirectToAction("Message", new { mess = "Đặt hàng thành công" });
        }
        public ActionResult Checkout()
        {
            ViewBag.TotalQuantity = GetTotalQuanity();
            TaiKhoan user = Session["User"] as TaiKhoan;
            try
            {
                Session["Cart"] = GetCart();
                return View();
            }
            catch (Exception)
            {
                return View();
            }
        }
        [HttpGet]
        public ActionResult Message(string mess)
        {
            ViewBag.Message = mess;
            return View();
        }
    }
}