namespace QLBH.Fastfood.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitTables1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SanPham", "MaLoaiSP", "dbo.LoaiSanPham");
            DropIndex("dbo.SanPham", new[] { "MaLoaiSP" });
            AddColumn("dbo.TaiKhoan", "SoLanDaMua", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.SanPham", "GiaKhuyenMai", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.SanPham", "SoLanMua", c => c.Int(nullable: false));
            AddColumn("dbo.SanPham", "NgayTao", c => c.DateTime(nullable: false));
            AddColumn("dbo.SanPham", "GiamGia", c => c.Int(nullable: false));
            AddColumn("dbo.LoaiSanPham", "HoatDong", c => c.Boolean(nullable: false));
            AddColumn("dbo.LoaiSanPham", "NgayTaoLSP", c => c.DateTime(nullable: false));
            AlterColumn("dbo.ChiTietDonHang", "GiaTien", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.SanPham", "GiaSP", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.SanPham", "HoatDong", c => c.Boolean(nullable: false));
            AlterColumn("dbo.SanPham", "MaLoaiSP", c => c.Int(nullable: false));
            CreateIndex("dbo.SanPham", "MaLoaiSP");
            AddForeignKey("dbo.SanPham", "MaLoaiSP", "dbo.LoaiSanPham", "MaLoaiSP", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SanPham", "MaLoaiSP", "dbo.LoaiSanPham");
            DropIndex("dbo.SanPham", new[] { "MaLoaiSP" });
            AlterColumn("dbo.SanPham", "MaLoaiSP", c => c.Int());
            AlterColumn("dbo.SanPham", "HoatDong", c => c.Boolean());
            AlterColumn("dbo.SanPham", "GiaSP", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.ChiTietDonHang", "GiaTien", c => c.Decimal(precision: 18, scale: 2));
            DropColumn("dbo.LoaiSanPham", "NgayTaoLSP");
            DropColumn("dbo.LoaiSanPham", "HoatDong");
            DropColumn("dbo.SanPham", "GiamGia");
            DropColumn("dbo.SanPham", "NgayTao");
            DropColumn("dbo.SanPham", "SoLanMua");
            DropColumn("dbo.SanPham", "GiaKhuyenMai");
            DropColumn("dbo.TaiKhoan", "SoLanDaMua");
            CreateIndex("dbo.SanPham", "MaLoaiSP");
            AddForeignKey("dbo.SanPham", "MaLoaiSP", "dbo.LoaiSanPham", "MaLoaiSP");
        }
    }
}
