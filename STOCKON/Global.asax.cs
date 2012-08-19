using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace STOCKON
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            
        }

    }

     

    public static class MyGlobalVariables
    {
        public static short mid { get; set; }
        public static short faid { get; set; }
        public static short fvid { get; set; }
        public static STOCKON.Models.DateModel datem { get; set; }

        public static string FormatCode(string Prefixe, int racine)
        {
            string v = "";
            if (racine.ToString().Length >= 5)
                v = Prefixe + racine.ToString();
            else
                v = Prefixe.PadRight(5 - racine.ToString().Length + Prefixe.Length, '0') + racine.ToString();

            return v;
        }
    }
}