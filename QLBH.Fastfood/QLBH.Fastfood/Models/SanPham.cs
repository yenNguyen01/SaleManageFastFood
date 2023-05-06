
namespace QLBH.Fastfood.Models
{
        using System;
        using System.Collections.Generic;
        using System.ComponentModel.DataAnnotations;
        using System.ComponentModel.DataAnnotations.Schema;
        using System.Linq;
        using System.Web;

        [Table("SanPham")]
        public partial class SanPham
        {
            [Key]
            [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
            public int MaSP { get; set; }

            public string TenSP { get; set; }
            public string MoTa { get; set; }
            public decimal GiaSP { get; set; }
            public decimal GiaKhuyenMai { get; set; }
            public bool HoatDong { get; set; }
            public string AnhSP { get; set; }
            public int MaLoaiSP { get; set; }
            public int SoLuong { get; set; }
            public int SoLanMua { get; set; }
            public System.DateTime NgayTao { get; set; }
            public int GiamGia { get; set; }
        //public string Keyword { get; set; }

        public virtual LoaiSanPham LoaiSanPham { get; set; }
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
            public ICollection<ChiTietDonHang> ChiTietDonHangs { set; get; }
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
            public virtual ICollection<DanhGia> DanhGias { get; set; }
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
            public virtual ICollection<GioHang> GioHangs { get; set; }

            public SanPham()
            {
                ChiTietDonHangs = new HashSet<ChiTietDonHang>();
                this.DanhGias = new HashSet<DanhGia>();
                this.GioHangs = new HashSet<GioHang>();
            }

        }
}