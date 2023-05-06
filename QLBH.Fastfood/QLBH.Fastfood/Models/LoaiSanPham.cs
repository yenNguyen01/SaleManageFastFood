namespace QLBH.Fastfood.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Web;

    [Table("LoaiSanPham")]
    public partial class LoaiSanPham
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int MaLoaiSP { get; set; }

        [StringLength(50)]
        public string TenLoaiSP { get; set; }

        public bool HoatDong { get; set; }
        public System.DateTime NgayTaoLSP { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SanPham> Sanphams { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LoaiSanPham()
        {
            this.Sanphams = new HashSet<SanPham>();
        }

    }
}