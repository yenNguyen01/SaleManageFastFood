using QLBH.Fastfood.Service;
using System.Web.Mvc;
using Unity;
using Unity.Mvc5;

namespace QLBH.Fastfood
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterType<ISanPhamService, SanPhamService>();
            container.RegisterType<ILoaiSanPhamService, LoaiSanPhamService>();
            container.RegisterType<ITaiKhoanService, TaiKhoanService>();
            container.RegisterType<IPhanQuyenService, PhanQuyenService>();
            container.RegisterType<IDonHangService, DonHangService>();
            container.RegisterType<IChiTietDonHangService, ChiTietDonHangService>();
            container.RegisterType<IGioHangService, GioHangService>();
            container.RegisterType<IDanhGiaService, DanhGiaService>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}