
namespace QLBH.Fastfood.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public partial class DanhGia
    {
        public int ID { get; set; }
        public int MaSP { get; set; }
        public int IDUser { get; set; }
        public int Sao { get; set; }
        public string NoiDung { get; set; }

        public virtual SanPham SanPhams { get; set; }
        public virtual TaiKhoan TaiKhoans { get; set; }
    }
}