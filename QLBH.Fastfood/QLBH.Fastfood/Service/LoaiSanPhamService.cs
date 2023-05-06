using QLBH.Fastfood.Data;
using QLBH.Fastfood.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;



namespace QLBH.Fastfood.Service
{
    public interface ILoaiSanPhamService
    {
        LoaiSanPham AddProductCategory(LoaiSanPham productCategory);
        IEnumerable<LoaiSanPham> GetProductCategoryList();
        IEnumerable<LoaiSanPham> GetProductCategoryHome();
        LoaiSanPham GetProductCategoryByName(string Name);
        IEnumerable<LoaiSanPham> GetProductCategoryList(string keyWord);
        List<string> GetProductCategoryListName(string keyword);
        LoaiSanPham GetByID(int ID);
        void UpdateProductCategory(LoaiSanPham productCategory);
        void Block(LoaiSanPham productCategory);
        void Active(LoaiSanPham productCategory);
        void MultiDeleteProductCategory(string[] IDs);
        void Save();
        LoaiSanPham GetByName(string Name);
        bool CheckName(string Name);
    }
    public class LoaiSanPhamService : ILoaiSanPhamService
    {
        private readonly UnitOfWork context;
        public LoaiSanPhamService(UnitOfWork repositoryContext)
        {
            this.context = repositoryContext;
        }
        public LoaiSanPham AddProductCategory(LoaiSanPham productCategory)
        {
            productCategory.NgayTaoLSP = DateTime.Now;
            this.context.LoaiSanPhamRepository.Insert(productCategory);
            return productCategory;
        }

        public void Block(LoaiSanPham productCategory)
        {
            productCategory.HoatDong = false;
            this.context.LoaiSanPhamRepository.Update(productCategory);
        }
        public void Active(LoaiSanPham productCategory)
        {
            productCategory.HoatDong = true;
            this.context.LoaiSanPhamRepository.Update(productCategory);
        }
        public void MultiDeleteProductCategory(string[] IDs)
        {
            foreach (var id in IDs)
            {
                LoaiSanPham productCategory = GetByID(int.Parse(id));
                productCategory.HoatDong = false;
                UpdateProductCategory(productCategory);
            }
        }

        public LoaiSanPham GetByID(int ID)
        {
            return this.context.LoaiSanPhamRepository.GetDataByID(ID);
        }

        public IEnumerable<LoaiSanPham> GetProductCategoryList()
        {
            IEnumerable<LoaiSanPham> listProductCategory = this.context.LoaiSanPhamRepository.GetAllData();
            return listProductCategory;
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void UpdateProductCategory(LoaiSanPham productCategory)
        {
            productCategory.NgayTaoLSP = DateTime.Now;
            IEnumerable<SanPham> products = this.context.SanPhamRepository.GetAllData(x => x.MaLoaiSP == productCategory.MaLoaiSP);
            foreach (var item in products)
            {
                if (item.HoatDong != productCategory.HoatDong)
                {
                    item.HoatDong = productCategory.HoatDong;
                }
                this.context.SanPhamRepository.Update(item);
            }
            this.context.LoaiSanPhamRepository.Update(productCategory);
        }

        public IEnumerable<LoaiSanPham> GetProductCategoryList(string keyWord)
        {
            IEnumerable<LoaiSanPham> listProductCategory = this.context.LoaiSanPhamRepository.GetAllData(x => x.TenLoaiSP.Contains(keyWord));
            return listProductCategory;
        }

        public List<string> GetProductCategoryListName(string keyword)
        {
            IEnumerable<LoaiSanPham> listProductCategoryName = this.context.LoaiSanPhamRepository.GetAllData(x => x.TenLoaiSP.Contains(keyword) && x.HoatDong == true);
            List<string> names = new List<string>();
            foreach (var item in listProductCategoryName)
            {
                names.Add(item.TenLoaiSP);
            }
            return names;
        }

        public LoaiSanPham GetProductCategoryByName(string Name)
        {
            LoaiSanPham productCategory = this.context.LoaiSanPhamRepository.GetAllData().SingleOrDefault(x => x.TenLoaiSP == Name);
            return productCategory;
        }

        public IEnumerable<LoaiSanPham> GetProductCategoryHome()
        {
            IEnumerable<LoaiSanPham> listProductCategory = this.context.LoaiSanPhamRepository.GetAllData(x => x.HoatDong == true);
            return listProductCategory;
        }
        public LoaiSanPham GetByName(string Name)
        {
            LoaiSanPham productCategory = context.LoaiSanPhamRepository.GetAllData().FirstOrDefault(x => x.TenLoaiSP == Name);
            return productCategory;
        }
        public bool CheckName(string Name)
        {
            var check = context.LoaiSanPhamRepository.GetAllData(x => x.TenLoaiSP == Name && x.HoatDong == true);
            if (check.Count() > 0)
            {
                return false;
            }
            return true;
        }
    }
  
}