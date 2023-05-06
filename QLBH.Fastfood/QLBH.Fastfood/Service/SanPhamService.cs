using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using QLBH.Fastfood.Data;
using QLBH.Fastfood.Models;
namespace QLBH.Fastfood.Service
{
        public interface ISanPhamService
        {
            SanPham AddProduct(SanPham product);
            //void AddViewCount(int ID);
            int GetTotalProduct();
            int GetTotalProductPurchased();
            IEnumerable<SanPham> GetProductList();
            IEnumerable<SanPham> GetProductListByCategory(int MaLoaiSP);
            IEnumerable<SanPham> GetProductListLast();
            //IEnumerable<SanPham> GetProductListViewedByUserID(int UserID);
            IEnumerable<SanPham> GetProductList(string keyWord);
            IEnumerable<SanPham> GetProductListForManage();
            IEnumerable<SanPham> GetProductListForManage(string keyWord);
            IEnumerable<SanPham> GetProductListForHomePage(int productCategoryID);
            IEnumerable<SanPham> GetProductListIndex();
            IEnumerable<SanPham> GetProductListDiscount();
            IEnumerable<SanPham> GetProductListRandom();
            IEnumerable<SanPham> GetProductListAlmostOver();
            IEnumerable<SanPham> GetProductListStocking();
            IEnumerable<SanPham> GetProductListSold(DateTime from, DateTime to);
            SanPham GetByID(int ID);
            void UpdateQuantity(int ID, int Quantity);
            void UpdatePurchasedCount(int ID, int PurchasedCount);
            List<string> GetProductListName(string keyword);
            void UpdateProduct(SanPham product);
            void DeleteProduct(SanPham product);
            void ActiveProduct(SanPham product);
            void MultiDeleteProduct(string[] IDs);
            void Save();
            SanPham GetByName(string Name);
            bool CheckName(string Name);
        }
        public class SanPhamService : ISanPhamService
    {
            private readonly UnitOfWork context;
            public SanPhamService(UnitOfWork repositoryContext)
            {
                this.context = repositoryContext;
            }
        public SanPham AddProduct(SanPham product)
        {
            product.NgayTao = DateTime.Now;
            product.SoLanMua = 0;
            product.GiaKhuyenMai = product.GiaSP - (product.GiaSP / 100 * product.GiamGia);
            this.context.SanPhamRepository.Insert(product);
            return product;
        }

        public void DeleteProduct(SanPham product)
        {
                product.HoatDong = false;
                this.context.SanPhamRepository.Delete(product);
         }

        public SanPham GetByID(int ID)
        {
                return this.context.SanPhamRepository.GetDataByID(ID);
        }

            public List<string> GetProductListName(string keyword)
            {
                IEnumerable<SanPham> listProductName = this.context.SanPhamRepository.GetAllData(x => x.TenSP.Contains(keyword) && x.HoatDong == true);
                List<string> names = new List<string>();
                foreach (var item in listProductName)
                {
                    names.Add(item.TenSP);
                }
                return names;
            }

            public IEnumerable<SanPham> GetProductList()
            {
                return this.context.SanPhamRepository.GetAllData(x => x.HoatDong == true);
            }

            public void Save()
            {
                throw new NotImplementedException();
            }

        public void UpdateProduct(SanPham product)
        {
            product.NgayTao = DateTime.Now;
            product.GiaKhuyenMai = product.GiaSP - (product.GiaSP / 100 * product.GiamGia);
            this.context.SanPhamRepository.Update(product);
            //this.context.Save();
        }

        public IEnumerable<SanPham> GetProductList(string keyWord)
            {
                IEnumerable<SanPham> listProduct = this.context.SanPhamRepository.GetAllData(x => x.TenSP.Contains(keyWord) && x.HoatDong == true);
                return listProduct;
            }

        public void MultiDeleteProduct(string[] IDs)
        {
            foreach (var id in IDs)
            {
                SanPham product = GetByID(int.Parse(id));
                product.HoatDong = false;
                UpdateProduct(product);
            }
        }

        public IEnumerable<SanPham> GetProductListByCategory(int ProductCategoryID)
            {
                IEnumerable<SanPham> listProduct = this.context.SanPhamRepository.GetAllData(x => x.MaLoaiSP == ProductCategoryID && x.HoatDong == true);
                return listProduct;
            }
            public IEnumerable<SanPham> GetProductListForHomePage(int productCategoryID)
            {
                IEnumerable<SanPham> listProduct = this.context.SanPhamRepository.GetAllData()
                    .Where(x => x.MaLoaiSP == productCategoryID && x.HoatDong == true).Take(3);
                return listProduct;
            }

            public IEnumerable<SanPham> GetProductListIndex()
            {
                IEnumerable<SanPham> listProduct = this.context.SanPhamRepository.GetAllData()
                    .Where(x => x.HoatDong == true && x.SoLuong > 0).Take(4);

                return listProduct;
            }

            public IEnumerable<SanPham> GetProductListRandom()
            {
                IEnumerable<SanPham> listProduct = this.context.SanPhamRepository.GetAllData().OrderBy(x => Guid.NewGuid()).Take(4);
                return listProduct;
            }

            public IEnumerable<SanPham> GetProductListLast()
            {
                IEnumerable<SanPham> listProduct = this.context.SanPhamRepository.GetAllData(x => x.HoatDong == true);
                return listProduct;
            }

            public IEnumerable<SanPham> GetProductListForManage()
            {
                IEnumerable<SanPham> listProduct = this.context.SanPhamRepository.GetAllData();
                return listProduct;
            }

            public IEnumerable<SanPham> GetProductListForManage(string keyWord)
            {
                IEnumerable<SanPham> listProduct = this.context.SanPhamRepository.GetAllData(x => x.TenSP.Contains(keyWord));
                return listProduct;
            }

            public void ActiveProduct(SanPham product)
            {
                product.HoatDong = true;
                this.context.SanPhamRepository.Update(product);
            }

            public void UpdateQuantity(int ID, int Quantity)
            {
                SanPham product = context.SanPhamRepository.GetDataByID(ID);
                product.SoLuong -= Quantity;
                context.SanPhamRepository.Update(product);
            }

            public void UpdatePurchasedCount(int ID, int PurchasedCount)
            {
                SanPham product = context.SanPhamRepository.GetDataByID(ID);
                product.SoLanMua += PurchasedCount;
                context.SanPhamRepository.Update(product);
            }

            public IEnumerable<SanPham> GetProductListAlmostOver()
            {
                return context.SanPhamRepository.GetAllData().OrderBy(x => x.SoLuong);
            }


            public int GetTotalProduct()
            {
                return context.SanPhamRepository.GetAllData().Count();
            }

            public int GetTotalProductPurchased()
            {
                return (int)context.SanPhamRepository.GetAllData().Sum(x => x.SoLanMua);
            }

            public IEnumerable<SanPham> GetProductListStocking()
            {
                return context.SanPhamRepository.GetAllData(x => x.SoLuong > 0 && x.HoatDong == true);
            }

            public IEnumerable<SanPham> GetProductListSold(DateTime from, DateTime to)
            {
                IEnumerable<ChiTietDonHang> orderDetails = context.ChiTietDonHangRepository.GetAllData(x => DbFunctions.TruncateTime(x.DonHang.NgayDat) >= from.Date && DbFunctions.TruncateTime(x.DonHang.NgayDat) <= to.Date);

                List<int> ProductIDs = new List<int>();
                foreach (var item in orderDetails)
                {
                    ProductIDs.Add(item.MaSP);
                }
                if (ProductIDs.Count() > 0)
                {
                    return context.SanPhamRepository.GetAllData(x => x.SoLanMua > 0 && ProductIDs.Contains(x.MaSP)).OrderByDescending(x => x.SoLanMua);
                }
                return null;
            }
            public SanPham GetByName(string Name)
            {
                SanPham product = context.SanPhamRepository.GetAllData().FirstOrDefault(x => x.TenSP == Name);
                return product;
            }
            public bool CheckName(string Name)
            {
                var check = context.SanPhamRepository.GetAllData(x => x.TenSP == Name && x.HoatDong == true);
                if (check.Count() > 0)
                {
                    return false;
                }
                return true;
            }

            public IEnumerable<SanPham> GetProductListDiscount()
            {
                return context.SanPhamRepository.GetAllData(x => x.GiamGia > 0 && x.HoatDong == true).Take(3);
            }
    }
}