
namespace QLBH.Fastfood.Models
{
        using System;
        using System.Collections.Generic;
        using System.ComponentModel.DataAnnotations;
        using System.Linq;
        using System.Web;

        public partial class GioHang
        {
            public GioHang(int id)
            {
                QLBHFastfoodDbcontext db = new QLBHFastfoodDbcontext();
                this.MaSP = id;
                SanPham product = db.SanPhams.Single(n => n.MaSP == id);
                this.Ten = product.TenSP;
                this.Anh = product.AnhSP;
                this.GiaTien = (decimal)product.GiaSP;
                this.SoLuong = 1;
                this.TongCong = GiaTien * SoLuong;
            }
            public GioHang()
            {

            }
            public int ID { get; set; }
            public int IDUser { get; set; }
            public int MaSP { get; set; }
            public int SoLuong { get; set; }
            public decimal TongCong { get; set; }
            public string Ten { get; set; }
            public string Anh { get; set; }
            public decimal GiaTien { get; set; }



            public virtual TaiKhoan TaiKhoans { get; set; }

            public virtual SanPham SanPhams { get; set; }

        }
}