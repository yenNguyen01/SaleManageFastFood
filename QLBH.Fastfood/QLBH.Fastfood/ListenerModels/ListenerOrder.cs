using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLBH.Fastfood.ListenerModels
{
    public class ListenerOrder
    {
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
    }
}