using PagedList;
using QLBH.Fastfood.Models;
using QLBH.Fastfood.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLBH.Fastfood.Controllers
{
    public class OrderManageController : Controller
    {
        // GET: OrderManage
        //[Authorize(Roles = "OrderManage")]
            private IDonHangService _donHangService;
            private IChiTietDonHangService _chiTietDonHangService;
            private ISanPhamService _sanPhamService;
            private ITaiKhoanService _taiKhoanService;
            public OrderManageController(IDonHangService donHangService, IChiTietDonHangService chiTietDonHangService, ISanPhamService sanPhamService, ITaiKhoanService taiKhoanService)
            {
                _donHangService = donHangService;
                _chiTietDonHangService = chiTietDonHangService;
                _sanPhamService = sanPhamService;
                _taiKhoanService = taiKhoanService;
            }
            // GET: OrderManage
            public ActionResult NotApproval(int page = 1)
            {
                IEnumerable<DonHang> orderList = _donHangService.GetOrderNotApproval();
                PagedList<DonHang> orderListPaging = new PagedList<DonHang>(orderList, page, 5);

                return View(orderListPaging);
            }
            public ActionResult NotDelivery(int page = 1)
            {
                IEnumerable<DonHang> orderList = _donHangService.GetOrderNotDelivery();
                PagedList<DonHang> orderListPaging = new PagedList<DonHang>(orderList, page, 5);

                return View(orderListPaging);
            }

            public ActionResult DeliveredAndPaid(int page = 1)
            {
                IEnumerable<DonHang> orderList = _donHangService.GetOrderDeliveredAndPaid();
                PagedList<DonHang> orderListPaging = new PagedList<DonHang>(orderList, page, 5);

                return View(orderListPaging);
            }
            public ActionResult ApprovedAndNotDelivery(int page = 1)
            {
                IEnumerable<DonHang> orderList = _donHangService.ApprovedAndNotDelivery();
                PagedList<DonHang> orderListPaging = new PagedList<DonHang>(orderList, page, 5);

                return View(orderListPaging);
            }
            public ActionResult DeliveredList(int page = 1)
            {
                IEnumerable<DonHang> orderList = _donHangService.GetDelivered();
                PagedList<DonHang> orderListPaging = new PagedList<DonHang>(orderList, page, 5);

                return View(orderListPaging);
            }
            [HttpGet]
            public ActionResult OrderApproval(int ID)
            {
                DonHang order = _donHangService.Approved(ID);
                //Get email customer
                string Email = _taiKhoanService.GetEmailByID(order.IDUser);
                //SentMail("Đơn hàng của bạn đã được duyệt", Email, "khuongip564gb@gmail.com", "google..khuongip564gb", "Vào đơn hàng của bạn để xem thông tin chi tiết");
                return RedirectToAction("ApprovedAndNotDelivery");
            }
            [HttpGet]
            public ActionResult Delivered(int ID)
            {
                DonHang order = _donHangService.Delivered(ID);
                //Get email customer
                string Email = _taiKhoanService.GetEmailByID(order.IDUser);
                string urlBase = Request.Url.GetLeftPart(UriPartial.Authority) + Url.Content("~");
                //SentMail("Đơn hàng của bạn đã được giao cho đối tác vận chuyển", Email, "khuongip564gb@gmail.com", "google..khuongip564gb", "Vào đơn hàng của bạn để xem thông tin chi tiết. Sau khi nhận được đơn hàng, bạn vui lòng click vào link sau để xác nhận đã nhận được đơn hàng từ đơn vị vận chuyển: " + urlBase + "/OrderManage/Received/" + ID + "");
                return RedirectToAction("DeliveredList");
            }
            [HttpGet]
            public ActionResult Detail(int ID)
            {
                if (ID == null)
                {
                    return null;
                }
                IEnumerable<ChiTietDonHang> orderDetails = _chiTietDonHangService.GetByOrderID(ID);
                if (orderDetails == null)
                {
                    return null;
                }
                ViewBag.MaDH = ID;
                DonHang order = _donHangService.GetByID(ID);
                ViewBag.IsApproved = order.ChapNhan;
                return View(orderDetails);
            }
            //public void SentMail(string Title, string ToEmail, string FromEmail, string Password, string Content)
            //{
            //    MailMessage mail = new MailMessage();
            //    mail.To.Add(ToEmail);
            //    mail.From = new MailAddress(ToEmail);
            //    mail.Subject = Title;
            //    mail.Body = Content;
            //    mail.IsBodyHtml = true;
            //    SmtpClient smtp = new SmtpClient();
            //    smtp.Host = "smtp.gmail.com";
            //    smtp.Port = 587;
            //    smtp.UseDefaultCredentials = false;
            //    smtp.Credentials = new NetworkCredential(FromEmail, Password);
            //    smtp.EnableSsl = true;
            //    smtp.Send(mail);
            //}
            [AllowAnonymous]
            [HttpGet]
            public ActionResult Received(int ID)
            {
                DonHang order = _donHangService.GetByID(ID);
                order.DaNhan = true;
                order.TraTien = true;
                order.NgayGiao = DateTime.Now;
                _donHangService.Update(order);
                IEnumerable<ChiTietDonHang> orderDetail = _chiTietDonHangService.GetByOrderID(order.MaDH);
                foreach (var item in orderDetail)
                {
                    _sanPhamService.UpdateQuantity(item.MaSP, item.SoLuong);
                    _sanPhamService.UpdatePurchasedCount(item.MaSP, item.SoLuong);
                }
                return RedirectToAction("Index", "Home");
            }
        
    }
}