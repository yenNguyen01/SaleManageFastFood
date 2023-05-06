using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace QLBH.Fastfood.Models
{
    public class QLBHFastfoodDbcontext : DbContext
    {

        public QLBHFastfoodDbcontext() : base("QLBHffconnectionString")
        {
        }

        public DbSet<LoaiSanPham> LoaiSanPhams { get; set; }
        public DbSet<SanPham> SanPhams { get; set; }
        public DbSet<TaiKhoan> TaiKhoans { get; set; }
        public DbSet<DonHang> DonHangs { get; set; }
        public DbSet<PhanQuyen> PhanQuyens { get; set; }
        public DbSet<ChiTietDonHang> ChiTietDonHangs { get; set; }
        public DbSet<GioHang> GioHangs { get; set; }
        public DbSet<DanhGia> DanhGias { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaiKhoan>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<TaiKhoan>()
                .Property(e => e.MatKhau)
                .IsUnicode(false);

            modelBuilder.Entity<TaiKhoan>()
                .Property(e => e.SDT)
                .IsUnicode(false);

            modelBuilder.Entity<SanPham>()
                .Property(e => e.AnhSP)
                .IsUnicode(false);

            modelBuilder.Entity<GioHang>()
                .Property(e => e.Anh)
                .IsUnicode(false);

        }
    }

}