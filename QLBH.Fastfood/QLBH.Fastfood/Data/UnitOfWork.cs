using QLBH.Fastfood.Data.Repository;
using QLBH.Fastfood.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLBH.Fastfood.Data
{
    public class UnitOfWork : IDisposable
    {
        private QLBHFastfoodDbcontext DbContext = new QLBHFastfoodDbcontext();
        private GenericRepository<SanPham> sanPhamRepository;
        private GenericRepository<LoaiSanPham> loaiSanPhamRepository;
        private GenericRepository<TaiKhoan> taiKhoanRepository;
        private GenericRepository<PhanQuyen> phanQuyenRepository;
        private GenericRepository<DonHang> donHangRepository;
        private GenericRepository<ChiTietDonHang> chiTietDonHangRepository;
        private GenericRepository<GioHang> gioHangRepository;
        private GenericRepository<DanhGia> danhGiaRepository;

        public GenericRepository<SanPham> SanPhamRepository
        {
            get
            {
                if (this.sanPhamRepository == null)
                {
                    this.sanPhamRepository = new GenericRepository<SanPham>(DbContext);
                }
                return sanPhamRepository;
            }

        }
        public GenericRepository<LoaiSanPham> LoaiSanPhamRepository
        {
            get
            {
                if (this.loaiSanPhamRepository == null)
                {
                    this.loaiSanPhamRepository = new GenericRepository<LoaiSanPham>(DbContext);
                }
                return loaiSanPhamRepository;
            }

        }
        public GenericRepository<TaiKhoan> TaiKhoanRepository
        {
            get
            {
                if (this.taiKhoanRepository == null)
                {
                    this.taiKhoanRepository = new GenericRepository<TaiKhoan>(DbContext);
                }
                return taiKhoanRepository;
            }

        }
        public GenericRepository<PhanQuyen> PhanQuyenRepository
        {
            get
            {
                if (this.phanQuyenRepository == null)
                {
                    this.phanQuyenRepository = new GenericRepository<PhanQuyen>(DbContext);
                }
                return phanQuyenRepository;
            }

        }
        public GenericRepository<DonHang> DonHangRepository
        {
            get
            {
                if (this.donHangRepository == null)
                {
                    this.donHangRepository = new GenericRepository<DonHang>(DbContext);
                }
                return donHangRepository;
            }

        }
        public GenericRepository<ChiTietDonHang> ChiTietDonHangRepository
        {
            get
            {
                if (this.chiTietDonHangRepository == null)
                {
                    this.chiTietDonHangRepository = new GenericRepository<ChiTietDonHang>(DbContext);
                }
                return chiTietDonHangRepository;
            }

        }
        public GenericRepository<GioHang> GioHangRepository
        {
            get
            {
                if (this.gioHangRepository == null)
                {
                    this.gioHangRepository = new GenericRepository<GioHang>(DbContext);
                }
                return gioHangRepository;
            }

        }
        public GenericRepository<DanhGia> DanhGiaRepository
        {
            get
            {
                if (this.danhGiaRepository == null)
                {
                    this.danhGiaRepository = new GenericRepository<DanhGia>(DbContext);
                }
                return danhGiaRepository;
            }

        }
        public void Save()
        {
            DbContext.SaveChanges();
        }
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    DbContext.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}