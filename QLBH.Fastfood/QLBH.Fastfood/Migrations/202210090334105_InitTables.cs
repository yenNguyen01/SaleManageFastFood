namespace QLBH.Fastfood.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ChiTietDonHang",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        MaDH = c.Int(nullable: false),
                        MaSP = c.Int(nullable: false),
                        GiaTien = c.Decimal(precision: 18, scale: 2),
                        SoLuong = c.Int(nullable: false),
                        DanhGia = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.DonHang", t => t.MaDH, cascadeDelete: true)
                .ForeignKey("dbo.SanPham", t => t.MaSP, cascadeDelete: true)
                .Index(t => t.MaDH)
                .Index(t => t.MaSP);
            
            CreateTable(
                "dbo.DonHang",
                c => new
                    {
                        MaDH = c.Int(nullable: false, identity: true),
                        IDUser = c.Int(nullable: false),
                        NgayDat = c.DateTime(nullable: false),
                        NgayGiao = c.DateTime(nullable: false),
                        Offer = c.Int(nullable: false),
                        TraTien = c.Boolean(nullable: false),
                        HuyDon = c.Boolean(nullable: false),
                        XoaDon = c.Boolean(nullable: false),
                        GiaoHang = c.Boolean(nullable: false),
                        ChapNhan = c.Boolean(nullable: false),
                        DaNhan = c.Boolean(nullable: false),
                        TongCong = c.Decimal(precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.MaDH)
                .ForeignKey("dbo.TaiKhoan", t => t.IDUser, cascadeDelete: true)
                .Index(t => t.IDUser);
            
            CreateTable(
                "dbo.TaiKhoan",
                c => new
                    {
                        IDUser = c.Int(nullable: false, identity: true),
                        Email = c.String(nullable: false, unicode: false),
                        MatKhau = c.String(nullable: false, maxLength: 20, unicode: false),
                        HoTen = c.String(),
                        DiaChi = c.String(),
                        SDT = c.String(unicode: false),
                        XoaTK = c.Boolean(),
                        IDRole = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IDUser)
                .ForeignKey("dbo.PhanQuyen", t => t.IDRole, cascadeDelete: true)
                .Index(t => t.IDRole);
            
            CreateTable(
                "dbo.DanhGias",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        MaSP = c.Int(nullable: false),
                        IDUser = c.Int(nullable: false),
                        Sao = c.Int(nullable: false),
                        NoiDung = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.SanPham", t => t.MaSP, cascadeDelete: true)
                .ForeignKey("dbo.TaiKhoan", t => t.IDUser, cascadeDelete: true)
                .Index(t => t.MaSP)
                .Index(t => t.IDUser);
            
            CreateTable(
                "dbo.SanPham",
                c => new
                    {
                        MaSP = c.Int(nullable: false, identity: true),
                        TenSP = c.String(nullable: false, maxLength: 50),
                        MoTa = c.String(),
                        GiaSP = c.Decimal(precision: 18, scale: 2),
                        HoatDong = c.Boolean(),
                        AnhSP = c.String(unicode: false),
                        MaLoaiSP = c.Int(),
                        SoLuong = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.MaSP)
                .ForeignKey("dbo.LoaiSanPham", t => t.MaLoaiSP)
                .Index(t => t.MaLoaiSP);
            
            CreateTable(
                "dbo.GioHangs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        IDUser = c.Int(nullable: false),
                        MaSP = c.Int(nullable: false),
                        SoLuong = c.Int(nullable: false),
                        TongCong = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Ten = c.String(),
                        Anh = c.String(unicode: false),
                        GiaTien = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.SanPham", t => t.MaSP, cascadeDelete: true)
                .ForeignKey("dbo.TaiKhoan", t => t.IDUser, cascadeDelete: true)
                .Index(t => t.IDUser)
                .Index(t => t.MaSP);
            
            CreateTable(
                "dbo.LoaiSanPham",
                c => new
                    {
                        MaLoaiSP = c.Int(nullable: false, identity: true),
                        TenLoaiSP = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.MaLoaiSP);
            
            CreateTable(
                "dbo.PhanQuyen",
                c => new
                    {
                        IDRole = c.Int(nullable: false, identity: true),
                        TenQuyen = c.String(maxLength: 20),
                    })
                .PrimaryKey(t => t.IDRole);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TaiKhoan", "IDRole", "dbo.PhanQuyen");
            DropForeignKey("dbo.DonHang", "IDUser", "dbo.TaiKhoan");
            DropForeignKey("dbo.DanhGias", "IDUser", "dbo.TaiKhoan");
            DropForeignKey("dbo.SanPham", "MaLoaiSP", "dbo.LoaiSanPham");
            DropForeignKey("dbo.GioHangs", "IDUser", "dbo.TaiKhoan");
            DropForeignKey("dbo.GioHangs", "MaSP", "dbo.SanPham");
            DropForeignKey("dbo.DanhGias", "MaSP", "dbo.SanPham");
            DropForeignKey("dbo.ChiTietDonHang", "MaSP", "dbo.SanPham");
            DropForeignKey("dbo.ChiTietDonHang", "MaDH", "dbo.DonHang");
            DropIndex("dbo.GioHangs", new[] { "MaSP" });
            DropIndex("dbo.GioHangs", new[] { "IDUser" });
            DropIndex("dbo.SanPham", new[] { "MaLoaiSP" });
            DropIndex("dbo.DanhGias", new[] { "IDUser" });
            DropIndex("dbo.DanhGias", new[] { "MaSP" });
            DropIndex("dbo.TaiKhoan", new[] { "IDRole" });
            DropIndex("dbo.DonHang", new[] { "IDUser" });
            DropIndex("dbo.ChiTietDonHang", new[] { "MaSP" });
            DropIndex("dbo.ChiTietDonHang", new[] { "MaDH" });
            DropTable("dbo.PhanQuyen");
            DropTable("dbo.LoaiSanPham");
            DropTable("dbo.GioHangs");
            DropTable("dbo.SanPham");
            DropTable("dbo.DanhGias");
            DropTable("dbo.TaiKhoan");
            DropTable("dbo.DonHang");
            DropTable("dbo.ChiTietDonHang");
        }
    }
}
