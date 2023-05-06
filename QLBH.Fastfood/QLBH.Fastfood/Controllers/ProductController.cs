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
    public class ProductController : Controller
    {
        // GET: Product
        #region Initialize
        private ISanPhamService _sanPhamService;
        private ILoaiSanPhamService _loaiSanPhamService;
        private ITaiKhoanService _taiKhoanService;
        private IDanhGiaService _danhGiaService;
        private IChiTietDonHangService _chiTietDonHangService;

        QLBHFastfoodDbcontext dbcontext = new QLBHFastfoodDbcontext();

        public ProductController(ISanPhamService sanPhamService, ILoaiSanPhamService loaiSanPhamService, ITaiKhoanService taiKhoanService, IDanhGiaService danhGiaService, IChiTietDonHangService chiTietDonHangService)
        {
            _sanPhamService = sanPhamService;
            _loaiSanPhamService = loaiSanPhamService;
            _taiKhoanService = taiKhoanService;
            _danhGiaService = danhGiaService;
            _chiTietDonHangService = chiTietDonHangService;
        }
        #endregion
        public ActionResult Search(string keyword, int page = 1)
        {
            var listProduct = _sanPhamService.GetProductList(keyword);
            ViewBag.ListProduct = listProduct;
            if (keyword == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.Keyword = keyword;
            var products = _sanPhamService.GetProductList(keyword);
            PagedList<SanPham> listProductSearch = new PagedList<SanPham>(products, page, 8);
            return View(listProductSearch);
        }
        public ActionResult Details(int ID)
        {
            var product = _sanPhamService.GetByID(ID);
            var listProduct = _sanPhamService.GetProductListByCategory(product.MaLoaiSP).Where(x => x.MaSP != ID);
            ViewBag.ListProduct = listProduct;
            IEnumerable<TaiKhoan> listUser = _taiKhoanService.GetList();
            ViewBag.UserList = listUser;
                
            //Get rating
            ViewBag.Rating = _danhGiaService.GetRating(ID);
            //Get list rating
            ViewBag.ListRating = _danhGiaService.GetListRating(ID);
            return View(product);
        }
        public ActionResult ProductCategory(int ID, int page = 1)
        {
            var listProduct = _sanPhamService.GetProductList().Take(5);
            ViewBag.ListProduct = listProduct;

            ViewBag.productCategoryID = ID;
            LoaiSanPham productCategory = _loaiSanPhamService.GetByID(ID);
            ViewBag.Name = "Danh mục " + productCategory.TenLoaiSP;

            PagedList<SanPham> listProductPaging;
            IEnumerable<SanPham> products = _sanPhamService.GetProductListByCategory(ID);
            listProductPaging = new PagedList<SanPham>(products, page, 9);
            return View(listProductPaging);
        }

        public ActionResult NewProduct(int page = 1)
        {
            var listProduct = _sanPhamService.GetProductList().Take(5);
            ViewBag.ListProduct = listProduct;

            PagedList<SanPham> listProductPaging;
            IEnumerable<SanPham> products = _sanPhamService.GetProductListLast();
            listProductPaging = new PagedList<SanPham>(products, page, 3);
            return View(listProductPaging);
        }
   
        public ActionResult ProductPartial(SanPham product)
        {
            //Get rating
            ViewBag.Rating = _danhGiaService.GetRating(product.MaSP);
            return PartialView(product);
        }
        public ActionResult CommentPartial(int ID)
        {
            IEnumerable<TaiKhoan> listUser = _taiKhoanService.GetList();
            ViewBag.UserList = listUser;
            return PartialView("_CommentPartial");
        }
        [HttpPost]
        public ActionResult Rating(DanhGia rating, int OrderDetailID)
        {
            TaiKhoan user = Session["User"] as TaiKhoan;
            rating.IDUser = user.IDUser;
            _danhGiaService.AddRating(rating);
            _chiTietDonHangService.SetIsRating(OrderDetailID);
            return RedirectToAction("Details", "Product", new { ID = rating.MaSP });
        }
    }
}