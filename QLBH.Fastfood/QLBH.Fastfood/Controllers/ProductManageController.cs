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

namespace QLBH.Fastfood.Controllers
{
    public class ProductManageController : Controller
    {
        // GET: ProductManage
        #region Initialize
        private ISanPhamService _sanPhamService;
        private ILoaiSanPhamService _loaiSanPhamService;
 
        public ProductManageController(ISanPhamService sanPhamService,ILoaiSanPhamService loaiSanPhamService)
        {
            _sanPhamService = sanPhamService;
            _loaiSanPhamService = loaiSanPhamService;
        }
        #endregion
        [HttpGet]
        public ActionResult Index(int page = 1, string keyword = "")
        {
            ViewBag.MaLoaiSP = new SelectList(_loaiSanPhamService.GetProductCategoryList().OrderBy(x => x.TenLoaiSP), "MaLoaiSP", "TenLoaiSP");
            ViewBag.CategoryIDEdit = ViewBag.MaLoaiSP;
            ViewBag.CategoryIDDetail = ViewBag.MaLoaiSP;

            int pageSize = 5;
            if (keyword != "")
            {
                var products = _sanPhamService.GetProductListForManage().Where(x => x.TenSP.Contains(keyword)).OrderByDescending(x => x.NgayTao.Date);
                ViewBag.Products = products;
                PagedList<SanPham> listProduct = new PagedList<SanPham>(products, page, pageSize);
                ViewBag.KeyWord = keyword;
                if (listProduct != null)
                {
                    ViewBag.Page = page;
                    return View(listProduct);
                }
                else
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
            }
            else
            {
                var products = _sanPhamService.GetProductListForManage().OrderByDescending(x => x.NgayTao.Date);
                ViewBag.Products = products;
                PagedList<SanPham> listProduct = new PagedList<SanPham>(products, page, pageSize);
                if (listProduct != null)
                {
                    ViewBag.Page = page;
                    return View(listProduct);
                }
                else
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
            }
        }
        [HttpPost]
        public JsonResult ListName(string Prefix)
        {
            List<string> names = _sanPhamService.GetProductListName(Prefix).ToList();
            return Json(names, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult List(string keyword)
        {
            int pageSize = 5;
            if (keyword == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            var products = _sanPhamService.GetProductListForManage(keyword);
            PagedList<SanPham> listProduct = new PagedList<SanPham>(products, 1, pageSize);
            if (listProduct != null)
            {
                ViewBag.message = "Hiển thị kết quả tìm kiếm với '" + keyword + "";
                return View(listProduct);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.MaLoaiSP = new SelectList(_loaiSanPhamService.GetProductCategoryList().OrderBy(x => x.TenLoaiSP), "MaLoaiSP", "TenLoaiSP");
            return View();
        }
        [HttpPost]
        public ActionResult Create(SanPham product, HttpPostedFileBase ImageUpload, int page)
        {
            if (ImageUpload != null)
            {
                int errorCount = 0;
                if (ImageUpload != null && ImageUpload.ContentLength > 0)
                {
                    if (ImageUpload.ContentType != ".jpeg" && ImageUpload.ContentType != ".png" && ImageUpload.ContentType != ".jpg")
                    {
                        ViewBag.upload += "Hình ảnh không hợp lệ<br/>";
                        errorCount++;
                    }
                    else
                    {
                        var fileName = Path.GetFileName(ImageUpload.FileName);
                        var path = Path.Combine(Server.MapPath("~/Content/home/img/gallery"), fileName);
                        if (!System.IO.File.Exists(path))
                        {
                            ImageUpload.SaveAs(path);
                        }
                    }
                }
            }
            TempData["create"] = "Success";
            _sanPhamService.AddProduct(product);
            return RedirectToAction("Index", new { page = 1 });
        }    
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var product = _sanPhamService.GetByID(id);
            ViewBag.MaLoaiSPEdit = new SelectList(_loaiSanPhamService.GetProductCategoryList().OrderBy(x => x.TenLoaiSP), "MaLoaiSP", "TenLoaiSP", product.MaLoaiSP);
            if (product != null)
            {
                return Json(new
                {
                    MaSP = product.MaSP,
                    TenSP = product.TenSP,
                    MaLoaiSP = product.MaLoaiSP,
                    AnhSP = product.AnhSP,
                    GiaSP = product.GiaSP,
                    GiamGia = product.GiamGia,
                    MoTa = product.MoTa,
                    HoatDong = product.HoatDong,
                    SoLanMua = product.SoLanMua,
                    SoLuong = product.SoLuong,
                    status = true
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }
        [HttpPost]
        public ActionResult Edit(int id, HttpPostedFileBase ImageUpload, int page, int CategoryIDEdit)
        {
        

            var product = _sanPhamService.GetByID(id);
            ViewBag.MaLoaiSP = new SelectList(_loaiSanPhamService.GetProductCategoryList().OrderBy(x => x.TenLoaiSP), "MaLoaiSP", "TenLoaiSP", product.LoaiSanPham);

            if (ImageUpload != null)
            {
                int errorCount = 0;
                if (ImageUpload != null && ImageUpload.ContentLength > 0)
                {
                    if (ImageUpload.ContentType != "image/jpeg" && ImageUpload.ContentType != "image/png" && ImageUpload.ContentType != "image/jpg")
                    {
                        ViewBag.upload += "Hình ảnh không hợp lệ<br/>";
                        errorCount++;
                    }
                    else
                    {
                        var fileName = Path.GetFileName(ImageUpload.FileName);
                        var path = Path.Combine(Server.MapPath("~/Content/home/img/gallery"), fileName);
                        if (!System.IO.File.Exists(path))
                        {
                            ImageUpload.SaveAs(path);
                        }
                    }
                }
            }

            TempData["edit"] = "Success";
            product.MaLoaiSP = CategoryIDEdit;
            _sanPhamService.UpdateProduct(product);
            string Url = Request.Url.ToString();
            return RedirectToAction("Index", new { page = page });
        }
        public void Block(int id)
        {
            var product = _sanPhamService.GetByID(id);
            _sanPhamService.DeleteProduct(product);
        }
        public void Active(int id)
        {
            var product = _sanPhamService.GetByID(id);
            _sanPhamService.ActiveProduct(product);
        }
        public ActionResult ProductActivePartial(int ID)
        {
            return PartialView("ProductActivePartial", _sanPhamService.GetByID(ID));
        }
        [HttpPost]
        public JsonResult CheckName(string name, int id = 0)
        {
            SanPham product = _sanPhamService.GetByName(name);
            if (product != null)
            {
                if (product.MaSP == id)
                {
                    return Json(new
                    {
                        status = true
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if (_sanPhamService.CheckName(name))
                    {
                        return Json(new
                        {
                            status = true
                        }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            if (_sanPhamService.CheckName(name))
            {
                return Json(new
                {
                    status = true
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                status = false
            }, JsonRequestBehavior.AllowGet);
        }
    }
}