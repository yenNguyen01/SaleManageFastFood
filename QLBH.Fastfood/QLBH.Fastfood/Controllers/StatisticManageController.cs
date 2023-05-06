using OfficeOpenXml;
using QLBH.Fastfood.Models;
using QLBH.Fastfood.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLBH.Fastfood.Controllers
{
    public class StatisticManageController : Controller
    {
        private IDonHangService _donHangService;
        private ISanPhamService _sanPhamService;
        private ITaiKhoanService _taiKhoanService;
        public StatisticManageController(IDonHangService donHangService, ISanPhamService sanPhamService, ITaiKhoanService taiKhoanService)
        {
            _donHangService = donHangService;
            _sanPhamService = sanPhamService;
            _taiKhoanService = taiKhoanService;
        }
        // GET: Statistic
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult StatisticStocking()
        {
            IEnumerable<SanPham> products = _sanPhamService.GetProductListStocking();
            return View(products);
        }
        [HttpGet]
        public void DownloadExcelStatisticStocking()
        {
            TaiKhoan user = Session["User"] as TaiKhoan;

            IEnumerable<SanPham> products = _sanPhamService.GetProductListStocking();
            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");

            ws.Cells["A2"].Value = "Người lập";
            ws.Cells["B2"].Value = user.HoTen;

            ws.Cells["A3"].Value = "Ngày lập";
            ws.Cells["B3"].Value = DateTime.Now.ToString("dd/MM/yyyy");

            ws.Cells["A6"].Value = "Mã SP";
            ws.Cells["B6"].Value = "Tên SP";
            ws.Cells["C6"].Value = "Số lượng tồn";

            int rowStart = 7;
            foreach (var item in products)
            {
                ws.Cells[string.Format("A{0}", rowStart)].Value = item.MaSP;
                ws.Cells[string.Format("B{0}", rowStart)].Value = item.TenSP;
                ws.Cells[string.Format("C{0}", rowStart)].Value = item.SoLuong;
                rowStart++;
            }

            ws.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment: filename=" + "Danh sách tồn kho.xlsx");
            Response.BinaryWrite(pck.GetAsByteArray());
            Response.End();
        }
        [HttpGet]
        public ActionResult StatisticUser()
        {
            IEnumerable<TaiKhoan> users = _taiKhoanService.GetUserListForStatistic();
            return View(users);
        }
        [HttpGet]
        public ActionResult StatisticProductSold(DateTime from, DateTime to)
        {
            IEnumerable<SanPham> products = _sanPhamService.GetProductListSold(from, to);
            ViewBag.from = from;
            ViewBag.to = to;
            return View(products);
        }
        [HttpGet]
        public void DownloadExcelStatisticProductSold(DateTime from, DateTime to)
        {
            TaiKhoan user = Session["User"] as TaiKhoan;

            IEnumerable<SanPham> products = _sanPhamService.GetProductListSold(from, to);
            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");

            ws.Cells["A2"].Value = "Người lập";
            ws.Cells["B2"].Value = user.HoTen;

            ws.Cells["A3"].Value = "Ngày lập";
            ws.Cells["B3"].Value = DateTime.Now.ToString("dd/MM/yyyy");

            ws.Cells["A6"].Value = "Mã SP";
            ws.Cells["B6"].Value = "Tên SP";
            ws.Cells["C6"].Value = "Só Lượng Đã Bán";

            int rowStart = 7;
            foreach (var item in products)
            {
                ws.Cells[string.Format("A{0}", rowStart)].Value = item.MaSP;
                ws.Cells[string.Format("B{0}", rowStart)].Value = item.TenSP;
                ws.Cells[string.Format("C{0}", rowStart)].Value = item.SoLanMua;
                rowStart++;
            }

            ws.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment: filename=" + "Sản phẩm đã bán.xlsx");
            Response.BinaryWrite(pck.GetAsByteArray());
            Response.End();
        }
        [HttpGet]
        public ActionResult StatisticOrder(DateTime from, DateTime to)
        {
            IEnumerable<DonHang> orders = _donHangService.GetListOrderStatistic(from, to);
            ViewBag.from = from;
            ViewBag.to = to;
            return View(orders);
        }
        [HttpGet]
        public void DownloadExcelStatisticOrder(DateTime from, DateTime to)
        {
            TaiKhoan user = Session["User"] as TaiKhoan;

            IEnumerable<DonHang> orders = _donHangService.GetListOrderStatistic(from, to);
            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");

            ws.Cells["A2"].Value = "Người lập";
            ws.Cells["B2"].Value = user.HoTen;

            ws.Cells["A3"].Value = "Ngày lập";
            ws.Cells["B3"].Value = DateTime.Now.ToString("dd/MM/yyyy");

            ws.Cells["A6"].Value = "Mã Hóa Đơn";
            ws.Cells["B6"].Value = "Tên KH";
            ws.Cells["C6"].Value = "Ngày Đặt";
            ws.Cells["D6"].Value = "Ngày Giao";
            ws.Cells["E6"].Value = "Ưu Đãi";
            ws.Cells["F6"].Value = "Tình Trạng";
            ws.Cells["G6"].Value = "Thành Tiền";

            int rowStart = 6;
            foreach (var item in orders)
            {
                ws.Cells[string.Format("A{0}", rowStart)].Value = item.MaDH;
                ws.Cells[string.Format("B{0}", rowStart)].Value = item.TaiKhoan.HoTen;
                ws.Cells[string.Format("C{0}", rowStart)].Value = item.NgayDat.ToString("dd/MM/yyyy");
                ws.Cells[string.Format("D{0}", rowStart)].Value = item.NgayGiao.ToString("dd/MM/yyyy");
                if (item.DaNhan)
                    ws.Cells[string.Format("E{0}", rowStart)].Value = "Hoàn thành";
                else
                {
                    ws.Cells[string.Format("E{0}", rowStart)].Value = "Chưa hoàn thành";
                }
                ws.Cells[string.Format("F{0}", rowStart)].Value = item.TongCong;
                rowStart++;
            }

            ws.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment: filename=" + "Đơn đặt hàng.xlsx");
            Response.BinaryWrite(pck.GetAsByteArray());
            Response.End();
        }
  
    }
}