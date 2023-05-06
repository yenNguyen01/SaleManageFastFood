
namespace QLBH.Fastfood.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Web;

    [Table("ChiTietDonHang")]
    public partial class ChiTietDonHang
    {
        [Key]
        public int ID { get; set; }

        public int MaDH { get; set; }

        public int MaSP { get; set; }

        public decimal GiaTien { get; set; }

        public int SoLuong { get; set; }

        public bool DanhGia { get; set; }

        public virtual DonHang DonHang { get; set; }

        public virtual SanPham SanPham { get; set; }
    }
}