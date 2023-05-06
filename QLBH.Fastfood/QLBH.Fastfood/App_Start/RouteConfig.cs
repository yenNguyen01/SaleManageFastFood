using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace QLBH.Fastfood
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
             name: "ProductManage",
             url: "{controller}/{action}/{id}",
             defaults: new { controller = "ProductManage", action = "Edit", id = UrlParameter.Optional }
            );
            routes.MapRoute(
             name: "Home",
             url: "trang-chu",
             defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
            name: "ProductCategory",
            url: "danh-muc/{id}",
            defaults: new { controller = "Product", action = "ProductCategory", id = UrlParameter.Optional }
            );
            routes.MapRoute(
             name: "SignUp",
             url: "dang-ky",
             defaults: new { controller = "Home", action = "SignUp" }
         );
            routes.MapRoute(
               name: "SignIn",
               url: "dang-nhap",
               defaults: new { controller = "Home", action = "SignIn" }
           );
            routes.MapRoute(
               name: "Cart",
               url: "gio-hang",
               defaults: new { controller = "Cart", action = "Checkout" }
           );
            routes.MapRoute(
             name: "ProductDetail",
             url: "san-pham/{id}",
             defaults: new { controller = "Product", action = "Details", id = UrlParameter.Optional }
         );
            routes.MapRoute(
               name: "Product",
               url: "san-pham-moi",
               defaults: new { controller = "Product", action = "NewProduct" }
           );
        }
    }
}
