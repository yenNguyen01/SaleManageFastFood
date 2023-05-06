
namespace QLBH.Fastfood.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Web;

    [Table("DonHang")]
    public partial class DonHang
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DonHang()
        {
            this.ChiTietDonHangs = new HashSet<ChiTietDonHang>();
        }

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int MaDH { get; set; }

        public int IDUser { get; set; }
        public System.DateTime NgayDat { get; set; }
        public System.DateTime NgayGiao { get; set; }
        public bool TraTien { get; set; }
        public bool HuyDon { get; set; }
        public bool XoaDon { get; set; }
        public bool GiaoHang { get; set; }
        public bool ChapNhan { get; set; }
        public bool DaNhan { get; set; }
        public Nullable<decimal> TongCong { get; set; }

        public virtual TaiKhoan TaiKhoan { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; }
    }
}